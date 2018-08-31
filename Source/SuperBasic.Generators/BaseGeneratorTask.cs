﻿// <copyright file="BaseGeneratorTask.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Generators
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Microsoft.Build.Utilities;
    using SuperBasic.Utilities;

    public abstract class BaseGeneratorTask<TModel> : Task
    {
        private bool braceLastAdded = false;
        private int indentationLevel = 0;
        private StringBuilder builder = new StringBuilder();

        public string Input { get; set; }

        public string Output { get; set; }

        public sealed override bool Execute()
        {
            var settings = new XmlReaderSettings();
            settings.ValidationEventHandler += (sender, args) =>
            {
                switch (args.Severity)
                {
                    case XmlSeverityType.Error:
                        this.Log.LogError(args.Message);
                        break;
                    case XmlSeverityType.Warning:
                        this.Log.LogWarning(args.Message);
                        break;
                    default:
                        throw ExceptionUtilities.UnexpectedValue(args.Severity);
                }
            };

            using (var stream = new MemoryStream(File.ReadAllBytes(this.Input)))
            {
                using (var xmlReader = XmlReader.Create(stream, settings))
                {
                    var serializer = new XmlSerializer(typeof(TModel));
                    var model = (TModel)serializer.Deserialize(xmlReader);

                    this.Line($@"// <copyright file=""{Path.GetFileName(this.Output)}"" company=""2018 Omar Tawfik"">");
                    this.Line(@"// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.");
                    this.Line(@"// </copyright>");
                    this.Blank();
                    this.Line("/// <summary>");
                    this.Line("/// This file is auto-generated by a build task. It shouldn't be edited by hand.");
                    this.Line("/// </summary>");
                    this.Generate(model);

                    string result = this.builder.ToString();
                    if (!File.Exists(this.Output) || result != File.ReadAllText(this.Output))
                    {
                        File.WriteAllText(this.Output, result);
                    }
                }
            }

            return !this.Log.HasLoggedErrors;
        }

        protected abstract void Generate(TModel model);

        protected void Indent() => this.indentationLevel++;

        protected void Unindent() => this.indentationLevel--;

        protected void Brace()
        {
            this.Line("{");
            this.Indent();
        }

        protected void Unbrace()
        {
            this.braceLastAdded = false;
            this.Unindent();
            this.Line("}");
            this.braceLastAdded = true;
        }

        protected void Blank()
        {
            this.braceLastAdded = false;
            this.builder.AppendLine();
        }

        protected void Line(string line)
        {
            if (this.braceLastAdded)
            {
                this.builder.AppendLine();
                this.braceLastAdded = false;
            }

            this.builder.Append(' ', this.indentationLevel * 4);
            this.builder.AppendLine(line);
        }
    }
}
