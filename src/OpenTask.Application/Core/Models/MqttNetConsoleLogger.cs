// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using MQTTnet.Diagnostics;
using System.Text;

namespace OpenTask.Application.Core.Models
{
    public static class MqttNetConsoleLogger
    {
        private static readonly object _lock = new();

        public static void ForwardToConsole(MqttNetEventLogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogMessagePublished -= PrintToConsole;
            logger.LogMessagePublished += PrintToConsole;
        }

        public static void PrintToConsole(string message, ConsoleColor color)
        {
            lock (_lock)
            {
                ConsoleColor backupColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = backupColor;
            }
        }

        private static void PrintToConsole(object sender, MqttNetLogMessagePublishedEventArgs e)
        {
            StringBuilder output = new();
            _ = output.AppendLine($">> [{e.LogMessage.Timestamp:O}] [{e.LogMessage.ThreadId}] [{e.LogMessage.Source}] [{e.LogMessage.Level}]: {e.LogMessage.Message}");
            if (e.LogMessage.Exception != null)
            {
                _ = output.AppendLine(e.LogMessage.Exception.ToString());
            }

            ConsoleColor color = ConsoleColor.Red;
            switch (e.LogMessage.Level)
            {
                case MqttNetLogLevel.Error:
                    color = ConsoleColor.Red;
                    break;
                case MqttNetLogLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case MqttNetLogLevel.Info:
                    color = ConsoleColor.Green;
                    break;
                case MqttNetLogLevel.Verbose:
                    color = ConsoleColor.Gray;
                    break;
            }

            PrintToConsole(output.ToString(), color);
        }
    }
}