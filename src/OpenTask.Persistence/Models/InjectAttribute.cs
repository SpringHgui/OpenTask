// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.Extensions.DependencyInjection;

namespace OpenTask.Persistence.Models
{
    public class InjectAttribute : Attribute
    {
        private readonly ServiceLifetime lifetime;

        public InjectAttribute(ServiceLifetime Lifetime = ServiceLifetime.Transient)
        {
            lifetime = Lifetime;
        }

        public ServiceLifetime GetServiceLifetime()
        {
            return lifetime;
        }
    }
}
