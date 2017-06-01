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
}
