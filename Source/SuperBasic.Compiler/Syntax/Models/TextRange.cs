﻿// <copyright file="TextRange.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Syntax
{
    internal readonly struct TextRange
    {
        public TextPosition Start { get; }

        public TextPosition End { get; }
    }
}
