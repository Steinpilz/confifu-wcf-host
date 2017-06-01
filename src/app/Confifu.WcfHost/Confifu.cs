using Confifu.Abstractions;
using Confifu.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;

namespace Confifu.WcfHost
{
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
                       new WcfHostRunner(@params, logger).Open();
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
