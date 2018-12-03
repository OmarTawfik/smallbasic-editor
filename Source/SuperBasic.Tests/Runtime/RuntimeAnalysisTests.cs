// <copyright file="RuntimeAnalysisTests.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Tests.Runtime
{
    using FluentAssertions;
    using SuperBasic.Compiler;
    using SuperBasic.Compiler.Binding;
    using Xunit;

    public sealed class RuntimeAnalysisTests
    {
        [Fact]
        public void ItDoesNotUseGraphicsWindowWhenNotNeeded()
        {
            var compilation = new SuperBasicCompilation("TextWindow.WriteLine(5)");
            var analysis = new RuntimeAnalysis(compilation);
            analysis.UsesTextWindow.Should().Be(true);
            analysis.UsesGraphicsWindow.Should().Be(false);
        }

        [Fact]
        public void ItUsesGraphicsWindowWhenNeeded()
        {
            var compilation = new SuperBasicCompilation("GraphicsWindow.Clear()");
            var analysis = new RuntimeAnalysis(compilation);
            analysis.UsesTextWindow.Should().Be(false);
            analysis.UsesGraphicsWindow.Should().Be(true);
        }

        [Fact]
        public void ItUsesTextWindowWhenNothingIsNeeded()
        {
            var compilation = new SuperBasicCompilation("x = 1");
            var analysis = new RuntimeAnalysis(compilation);
            analysis.UsesTextWindow.Should().Be(true);
            analysis.UsesGraphicsWindow.Should().Be(false);
        }
    }
}
