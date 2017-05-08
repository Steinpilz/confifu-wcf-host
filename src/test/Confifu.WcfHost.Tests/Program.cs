using System;
using Confifu.WcfHost;
using Confifu.Abstractions;
using System.Collections.Generic;
using Confifu.Logging.Microsoft;
using Microsoft.Extensions.Logging;

namespace Confifu.WcfHost.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = new ConfigVariablesBuilder();
            var app = new App(b.Build());
            app.Setup();
            app.Run();

            Console.ReadKey();
        }
    }

    public class App : Confifu.AppSetup
    {
        public App(IConfigVariables env) : base(env)
        {
            Configure(Prepare);
        }
       
        public void Prepare()
        {
            var @params = new WcfHostParams() { BaseAddress= "http://localhost:8093/" };
            @params.Services = new List<ServiceDescription>() { new ServiceDescription() {Implementation  = typeof(Test),Interface = typeof(ITest), Name = "Test.svc" } };

            AppConfig
                .UseMicrosoftLogger(logger => logger.AddConsole(LogLevel.Debug))
                .UseWcfHost(@params);
        }
    }

}
