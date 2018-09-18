﻿// <copyright file="ArrayValue.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Utilities;

    public sealed class ArrayValue : BaseValue, IReadOnlyDictionary<string, BaseValue>
    {
        private readonly Dictionary<string, BaseValue> contents;

        public ArrayValue()
        {
            this.contents = new Dictionary<string, BaseValue>();
        }

        public ArrayValue(IReadOnlyDictionary<string, BaseValue> contents)
        {
            this.contents = contents.ToDictionary(p => p.Key, p => p.Value);
        }

        public IEnumerable<string> Keys => this.contents.Keys;

        public IEnumerable<BaseValue> Values => this.contents.Values;

        public int Count => this.contents.Count;

        public BaseValue this[string key] => this.contents[key];

        public Dictionary<string, BaseValue> ToDictionary() => new Dictionary<string, BaseValue>(this.contents);

        public bool ContainsKey(string key) => this.contents.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, BaseValue>> GetEnumerator() => this.contents.GetEnumerator();

        public bool TryGetValue(string key, out BaseValue value) => this.contents.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => this.contents.GetEnumerator();

        internal override bool ToBoolean() => false;

        internal override decimal ToNumber() => 0;

        internal override string ToString() => $"[{this.contents.Select(pair => $"{pair.Key}={pair.Value.ToString()}").Join(", ")}]";

        internal override ArrayValue ToArray() => this;
    }
}
