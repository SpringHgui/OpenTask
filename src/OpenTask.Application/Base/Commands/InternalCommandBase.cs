// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

namespace OpenTask.Application.Base.Commands
{
    public abstract class InternalCommandBase : ICommand
    {
        public Guid Id { get; }

        protected InternalCommandBase(Guid id)
        {
            Id = id;
        }
    }

    public abstract class InternalCommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; }

        protected InternalCommandBase()
        {
            Id = Guid.NewGuid();
        }

        protected InternalCommandBase(Guid id)
        {
            Id = id;
        }
    }
}