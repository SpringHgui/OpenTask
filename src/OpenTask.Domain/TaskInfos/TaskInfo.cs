// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Utility.Helpers;

namespace OpenTask.Domain.TaskInfos
{
    public class TaskInfo
    {
        public long Id { get; set; }

        public string Appid { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Handler { get; set; } = null!;

        public string? Description { get; set; }

        public string TimeType { get; set; } = null!;

        public string TimeConf { get; set; } = null!;

        public int AttemptInterval { get; set; }

        public int AttemptMax { get; set; }

        public string? HandleParams { get; set; } = "";

        public string ScheduleMode { get; set; } = null!;

        public string AlarmType { get; set; } = null!;

        public string? AlarmConf { get; set; } = string.Empty;

        public bool Enabled { get; set; }

        public long TriggerNextTime { get; set; }

        public int Slot { get; set; }

        public IEnumerable<DateTime> GetNextOccurrence(int times)
        {
            if (TriggerNextTime <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            TimeCrontab.Crontab crontab = CrontabUtility.Parse(TimeConf);

            DateTime current = DateTime.Now;
            for (int i = 0; i < times; i++)
            {
                yield return crontab.GetNextOccurrence(current);
            }
        }
    }
}
