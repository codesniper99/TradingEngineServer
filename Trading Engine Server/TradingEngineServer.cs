using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TradingEngineServer.Core
{
    sealed class TradingEngineServer : BackgroundService, ITradingEngineServer
    {
        private readonly ILogger<TradingEngineServer> _logger;
        private readonly TradingEngineServerConfiguration _tradingEngineServerConfig;
        public TradingEngineServer(ILogger<TradingEngineServer> logger,
            IOptions<TradingEngineServerConfiguration> config) 
        { 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
            _tradingEngineServerConfig = config.Value ?? throw new ArgumentNullException(nameof(config)); 

        }

        public Task Run(CancellationToken token) => ExecuteAsync(token);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Started {nameof(TradingEngineServer)}");
            while(!stoppingToken.IsCancellationRequested)
            {

            }
            _logger.LogInformation($"Stopped {nameof(TradingEngineServer)}");
            return Task.CompletedTask;
        }
    }
}
