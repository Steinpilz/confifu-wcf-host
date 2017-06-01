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
    public class WcfHostRunner 
    {
        readonly WcfHostParams @params;
        readonly ILogger logger;
        readonly List<ServiceHost> hosts;

        public WcfHostRunner(WcfHostParams @params, ILogger logger)
        {
            this.logger = logger;
            this.@params = @params;
            hosts = new List<ServiceHost>();

            foreach (var service in @params.Services)
            {
                var host = new ServiceHost(service.Implementation, new Uri(@params.BaseAddress + service.Name));

                logger.LogDebug($"Starting ServiceHost at {@params.BaseAddress + service.Name}");

                host.Description.Behaviors.Add(service.Behavior ?? DefaultSettings.Behavior());

                var debug = host.Description.Behaviors.OfType<ServiceDebugBehavior>().First();
                debug.IncludeExceptionDetailInFaults = true;

                host.AddServiceEndpoint(service.Interface, service.Binding ?? DefaultSettings.Binding(), "");
                hosts.Add(host);

            }

        }

        public void Open()
        {
            foreach (var host in hosts)
                host.Open();
        }
    }
}
