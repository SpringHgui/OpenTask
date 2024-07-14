// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.Logging;
using MQTTnet.Diagnostics;

namespace OpenTask.Application.Core
{
    public class MyLog : IMqttNetLogger
    {
        private readonly ILogger<MyLog> logger;

        public MyLog(ILogger<MyLog> logger)
        {
            this.logger = logger;
        }

        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
        {
            switch (logLevel)
            {
                case MqttNetLogLevel.Verbose:
                    logger.LogTrace(message);
                    break;
                case MqttNetLogLevel.Info:
                    logger.LogInformation(message);
                    break;
                case MqttNetLogLevel.Warning:
                    logger.LogWarning(message);
                    break;
                case MqttNetLogLevel.Error:
                    logger.LogError(exception, message);
                    break;
                default:
                    break;
            }
        }

    }
}
