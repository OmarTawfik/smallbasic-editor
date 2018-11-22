// <copyright file="CSInterop.Generated.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

/// <summary>
/// This file is auto-generated by a build task. It shouldn't be edited by hand.
/// </summary>
namespace SuperBasic.Editor.Interop
{
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using SuperBasic.Compiler.Services;

    internal interface IMonacoInterop
    {
        Task<MonacoCompletionItem[]> ProvideCompletionItems(string code, decimal line, decimal column);

        Task<string[]> ProvideHover(string code, decimal line, decimal column);
    }

    public static class CSInterop
    {
        private static readonly IMonacoInterop Monaco = new MonacoInterop();

        [JSInvokable("CSIntrop.Monaco.ProvideCompletionItems")]
        public static async Task<MonacoCompletionItem[]> Monaco_ProvideCompletionItems(string code, decimal line, decimal column)
        {
            return await Monaco.ProvideCompletionItems(code, line, column);
        }

        [JSInvokable("CSIntrop.Monaco.ProvideHover")]
        public static async Task<string[]> Monaco_ProvideHover(string code, decimal line, decimal column)
        {
            return await Monaco.ProvideHover(code, line, column);
        }
    }
}
