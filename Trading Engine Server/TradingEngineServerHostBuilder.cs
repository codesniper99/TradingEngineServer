using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Core;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggingConfiguration;

namespace TradingEngineServer.Core
{
    public sealed class TradingEngineServerHostBuilder
    {
        public static IHost BuildTradingEngineServer()
            => Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                // Start with Configuration
                services.AddOptions();
                services.Configure<TradingEngineServerConfiguration>(context.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));

                services.Configure<LoggerConfiguration>(context.Configuration.GetSection(nameof(LoggerConfiguration)));

                // ADD singleton objects
                services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
                services.AddSingleton<ITextLogger, TextLogger>();

                //Add Hosted service
                services.AddHostedService<TradingEngineServer>();

            }).Build();

    }
}
