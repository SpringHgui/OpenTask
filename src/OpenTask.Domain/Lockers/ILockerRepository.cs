// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Domain.Base.Repositorys;

namespace OpenTask.Domain.Lockers
{
    public interface ILockerRepository : IRepository<Locker>
    {
        Locker FindByKey(string resource);

        bool NewLocker(string key, string lockerby, out Locker? locker);

        bool UpdateLocker(string key, string lockerby, int currentVersion);

        bool ReEnterLocker(string key, string lockerby, int currentVersion);

        bool ReleaseLocker(string key, string lockerby);

    }
}
