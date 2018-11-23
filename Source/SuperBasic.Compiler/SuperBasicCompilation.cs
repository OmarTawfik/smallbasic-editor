// <copyright file="SuperBasicCompilation.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using SuperBasic.Compiler.Binding;
    using SuperBasic.Compiler.Diagnostics;
    using SuperBasic.Compiler.Parsing;
    using SuperBasic.Compiler.Scanning;
    using SuperBasic.Compiler.Services;
    using SuperBasic.Utilities;

    public sealed class SuperBasicCompilation
    {
        private readonly Scanner scanner;
        private readonly Parser parser;
        private readonly Binder binder;
        private readonly DiagnosticBag diagnostics;

        public SuperBasicCompilation(string text)
#if IsBuildingForDesktop
           : this(text, isRunningOnDesktop: true)
#else
           : this(text, isRunningOnDesktop: false)
#endif
        {
        }

        public SuperBasicCompilation(string text, bool isRunningOnDesktop)
        {
            this.Text = text;
            this.diagnostics = new DiagnosticBag();

            this.scanner = new Scanner(this.Text, this.diagnostics);
            this.parser = new Parser(this.scanner.Tokens, this.diagnostics);
            this.binder = new Binder(this.parser.SyntaxTree, this.diagnostics, isRunningOnDesktop);
        }

        public string Text { get; private set; }

        // TODO: this will eventually move to an engine analyzer, that computes that, along with things like, does it track mouse? should we terminate or does it listen to events? etc....
        public bool UsesGraphicsWindow => this.binder.UsesGraphicsWindow;

        public IReadOnlyList<Diagnostic> Diagnostics => this.diagnostics.Contents;

        internal BoundStatementBlock MainModule => this.binder.MainModule;

        internal IReadOnlyDictionary<string, BoundSubModule> SubModules => this.binder.SubModules;

        public MonacoCompletionItem[] ProvideCompletionItems(TextPosition position) => CompletionItemProvider.Provide(this.parser, position);

        public string[] ProvideHover(TextPosition position) => HoverProvider.Provide(this.diagnostics, this.parser, position);
    }
}
