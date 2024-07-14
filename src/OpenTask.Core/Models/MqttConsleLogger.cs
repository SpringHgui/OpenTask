// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MQTTnet.Diagnostics;
using System;

namespace OpenTask.Core.Models
{
    public class MqttConsleLogger : IMqttNetLogger
    {
        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
        {
            switch (logLevel)
            {
                case MqttNetLogLevel.Verbose:
                    break;
                case MqttNetLogLevel.Info:
                    Console.WriteLine(message);
                    break;
                case MqttNetLogLevel.Warning:
                    Console.WriteLine(message);
                    break;
                case MqttNetLogLevel.Error:
                    Console.WriteLine(message);
                    break;
                default:
                    break;
            }
        }

    }
}
