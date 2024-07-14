// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Utility.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 转时间戳
        /// </summary>
        /// <param name="date">DateTime 对象</param>
        /// <param name="is10bitSec">
        /// 默认true
        /// true 秒级
        /// false 毫秒级 </param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime date, bool is10bitSec = true)
        {
            TimeSpan timeSpan = date.ToUniversalTime() - new DateTime(1970, 1, 1);
            return is10bitSec ? (long)timeSpan.TotalSeconds : (long)timeSpan.TotalMilliseconds;
        }
    }
}
