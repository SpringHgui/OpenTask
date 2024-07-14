// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using TimeCrontab;

namespace OpenTask.Utility.Helpers
{
    public class CrontabUtility
    {
        public static Crontab Parse(string TimeExpression)
        {
            int len = TimeExpression.Trim().Split(" ").Length;
            CronStringFormat cronStringFormat = len == 6
                ? CronStringFormat.WithSeconds
                : len == 7 ? CronStringFormat.WithSecondsAndYears : throw new ArgumentException($"表达式错误: {TimeExpression}");
            Crontab crontab = Crontab.Parse(TimeExpression, cronStringFormat);
            return crontab;
        }
    }
}
