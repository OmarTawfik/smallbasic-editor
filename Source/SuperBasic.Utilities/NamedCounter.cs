﻿// <copyright file="NamedCounter.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    using System.Collections.Generic;

    public sealed class NamedCounter
    {
        private readonly Dictionary<string, int> counters = new Dictionary<string, int>();

        public string GetNext(string name)
        {
            int value;
            if (!this.counters.TryGetValue(name, out value))
            {
                value = 0;
            }

            this.counters[name] = ++value;
            return name + value;
        }
    }
}
