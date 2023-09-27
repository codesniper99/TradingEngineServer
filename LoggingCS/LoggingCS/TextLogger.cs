using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using LoggingCS;


using TradingEngineServer.Logging.LoggingConfiguration;
using System.Threading.Tasks.Dataflow;
using System.Data.SqlTypes;

namespace TradingEngineServer.Logging
{
    public class TextLogger : AbstractLogger, ITextLogger
    {
        private readonly LoggerConfiguration _loggingConfiguration;

        public TextLogger(IOptions<LoggerConfiguration> loggingConfiguration) : base()
        {
            _loggingConfiguration = loggingConfiguration.Value ?? throw new ArgumentNullException(nameof(loggingConfiguration));
            
            if(_loggingConfiguration.LoggerType != LoggerType.Text)
            {
                throw new InvalidOperationException($"{nameof(TextLogger)} doesn't match LoggerType of {_loggingConfiguration.LoggerType}");

            }
            var now = DateTime.Now;
            string logDirectory = Path.Combine(_loggingConfiguration.TextLoggerConfiguration.Directory, $"{now:yyyy-MM-dd}");
            string uniqueLogName = $"{_loggingConfiguration.TextLoggerConfiguration.Filename}-{now:HH_mm_ss}";
            string baseLogName = Path.ChangeExtension(uniqueLogName, _loggingConfiguration.TextLoggerConfiguration.FileExtension);
            string filepath = Path.Combine(logDirectory, baseLogName);
            Directory.CreateDirectory(logDirectory);

            _ = Task.Run(() => LogAsync(filepath, _logQueue, _tokenSource.Token));
        }

        private static async void LogAsync(string filepath, BufferBlock<LogInformation> logQueue, CancellationToken token)
        {
            using var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            using var sw = new StreamWriter(fs) { AutoFlush = true };
            try
            {
                while (true)
                {
                    var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    string formattedMessage = FormatLogItem(logItem);
                    await sw.WriteAsync(formattedMessage).ConfigureAwait(false);
                }

            }
            catch (OperationCanceledException)
            {

            }
        }

        private static string FormatLogItem(LogInformation logItem)
        {
            return $"[{logItem.Now: yyyy-MM-dd HH-mm-ss.fffffff}] [{logItem.ThreadName,-30}:{logItem.ThreadId:000}]" +
                $"[{logItem.LogLevel}] {logItem.Message}";
        }

        protected override void Log(LogLevel logLevel, string module, string message)
        {
            _logQueue.Post(new LogInformation(logLevel, module, message, DateTime.Now,
                Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextLogger() { 
            
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed)
                    return;
                _disposed = true;
            }
           
            if(disposing)
            {
                // Get rid of managed resources
                _tokenSource.Cancel();
                _tokenSource.Dispose();

            }
            // Get rid of unmanaged resources

        }
        private readonly BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly object _lock = new object();
        private bool _disposed = false;
       
    }
}
