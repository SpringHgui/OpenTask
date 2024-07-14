// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Domain.Lockers;

namespace OpenTask.Application.Lockers
{
    public class LockerService
    {
        private readonly ILockerRepository lockerRepository;

        public LockerService(ILockerRepository lockerRepository)
        {
            this.lockerRepository = lockerRepository;
        }

        public bool TryLock(string key, string lockerby, int expirdAfterSec, out Locker? locker)
        {
            locker = lockerRepository.FindByKey(key);
            if (locker == null)
            {
                // 新增
                return lockerRepository.NewLocker(key, lockerby, out locker);
            }
            else
            {
                if (locker.LockedBy != lockerby)
                {
                    if (!locker.IsExpired(expirdAfterSec))
                    {
                        return false;
                    }

                    // 更新
                    if (lockerRepository.UpdateLocker(key, lockerby, locker.Version))
                    {
                        return true;
                    }
                }
                else
                {
                    // 重入
                    if (lockerRepository.ReEnterLocker(key, lockerby, locker.Version))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Release(string key, string lockerby)
        {
            _ = lockerRepository.ReleaseLocker(key, lockerby);
        }
    }
}
