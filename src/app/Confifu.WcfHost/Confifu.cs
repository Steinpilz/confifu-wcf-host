using Confifu.Abstractions;
using Confifu.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace Confifu.WcfHost
{
    public class WcfHostParams
    {
        public string BaseAddress { get; set; }
        public List<ServiceDescription> Services { get; set; }
    }
    public class ServiceDescription
    {
        public Type Interface { get; set; }
        public Type Implementation { get; set; }
        public string Name { get; set; }
        public Binding Binding { get; set; }
        public ServiceMetadataBehavior Behavior { get; set; }
    }

    public static class DefaultSettings
    {
        public static ServiceMetadataBehavior Behavior() => new ServiceMetadataBehavior()
        {
            HttpGetEnabled = true,
            MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
        };


        public static Binding Binding() => new WSHttpBinding
        {
            Security = new WSHttpSecurity
            {
                Mode = SecurityMode.Message,
                Message = new NonDualMessageSecurityOverHttp
                {
                    ClientCredentialType = MessageCredentialType.Windows
                }
            },
            SendTimeout = TimeSpan.FromDays(1),
            ReceiveTimeout = TimeSpan.FromDays(1),
            MaxReceivedMessageSize = Int32.MaxValue,
            ReaderQuotas = new XmlDictionaryReaderQuotas()
            {
                MaxStringContentLength = Int32.MaxValue,
                MaxArrayLength = int.MaxValue,
                MaxBytesPerRead = int.MaxValue,
                MaxDepth = int.MaxValue,
                MaxNameTableCharCount = int.MaxValue

            }
        };
    }

    public static class AppConfigExtensions
    {
        public static IAppConfig UseWcfHost(
            this IAppConfig appConfig, WcfHostParams @params)
        {
            appConfig.AddAppRunnerAfter(() =>
               {
                   var logger = appConfig.GetLogger("");

                   try
                   {
                       foreach (var service in @params.Services)
                       {
                           var host = new ServiceHost(service.Implementation, new Uri(@params.BaseAddress + service.Name));

                           logger.LogDebug($"Starting ServiceHost at {@params.BaseAddress + service.Name}");

                           host.Description.Behaviors.Add(service.Behavior ?? DefaultSettings.Behavior());

                           var debug = host.Description.Behaviors.OfType<ServiceDebugBehavior>().First();
                           debug.IncludeExceptionDetailInFaults = true;

                           host.AddServiceEndpoint(service.Interface, service.Binding ?? DefaultSettings.Binding(), "");

                           host.Open();
                       }
                   }
                   catch (Exception ex)
                   {
                       logger.LogDebug(ex.ToString());
                   }
               });
            return appConfig;
        }
  }
}
