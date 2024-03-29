﻿using Airport.Infrastructure.Messaging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SecurityService
{
    class SecuritylogManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        private string _logPath;

        public SecuritylogManager(IMessageHandler messageHandler, SecuritylogManagerConfig config)
        {
            _messageHandler = messageHandler;
            _logPath = config.LogPath;

            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            string logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff")} - {message}{Environment.NewLine}";
            string logFile = Path.Combine(_logPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}-auditlog.txt");
            await File.AppendAllTextAsync(logFile, logMessage);
            Log.Information("{MessageType} - {Body}", messageType, message);
            return true;
        }
    }
}
