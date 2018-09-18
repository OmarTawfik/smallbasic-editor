﻿// <copyright file="BooleanValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    public sealed class BooleanValue : BaseValue
    {
        public BooleanValue(bool value)
        {
            this.Value = value;
        }

        public bool Value { get; private set; }

        internal override bool ToBoolean() => this.Value;

        internal override decimal ToNumber() => 0;

        internal override string ToString() => this.Value ? "True" : "False";

        internal override ArrayValue ToArray() => new ArrayValue();
    }
}
