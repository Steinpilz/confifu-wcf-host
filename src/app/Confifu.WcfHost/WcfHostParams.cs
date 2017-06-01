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

}
