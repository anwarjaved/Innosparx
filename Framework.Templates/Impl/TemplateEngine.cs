namespace Framework.Templates.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Framework.Ioc;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mustache Template engine.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [InjectBind(typeof(ITemplateEngine), LifetimeType.Singleton)]
    public class TemplateEngine : ITemplateEngine
    {
        private static readonly Regex ExpressionParamRegex1 = new Regex(
            @"([\w]+)\s*=\'\s*([^']*)\'\s*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ExpressionParamRegex2 = new Regex(
            @"([\w]+)\s*=""\s*([^""]*)""\s*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ExpressionParamRegex3 = new Regex(
            @"([\w]+)\s*=\s*(\[.*\])\s*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ExpressionParamRegex4 = new Regex(
            @"([\w]+)\s*=\s*(\{.*\})\s*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex ExpressionRegex = new Regex(
            @"^([\w]+)\s*\:\s*([\w]+)\s*(.*)\s*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex MarkerRegex = new Regex(
            @"(\s)*\{\{([{]?.+?\}?)\}\}",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex StripRegex = new Regex(
            @"\G[\r\t\v ]+\n",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Compiles the template and return as <see cref="ICompiledTemplate" />.
        /// </summary>
        /// <param name="template">
        ///     The template.
        /// </param>
        /// <returns>
        ///     An Instance of <see cref="ICompiledTemplate" />.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public ICompiledTemplate Compile(string template)
        {
            IEnumerable<ITemplatePart> parts = Scan(template);
            return new CompiledTemplate(parts);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Compiles the template and return as <see cref="ICompiledTemplate" />.
        /// </summary>
        /// <remarks>
        ///     Anwar Javed, 09/17/2013 12:51 PM.
        /// </remarks>
        /// <param name="templatePath">
        ///     Full pathname of the template file.
        /// </param>
        /// <returns>
        ///     An Instance of <see cref="ICompiledTemplate" />.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public ICompiledTemplate CompileFile(string templatePath)
        {
            return this.Compile(File.ReadAllText(templatePath));
        }

        [DebuggerStepThrough]
        internal static IEnumerable<ITemplatePart> Scan(string template)
        {
            int index = 0;
            Match match;

            var partStack = new Stack<SectionInfo>();

            while ((match = MarkerRegex.Match(template, index)).Success)
            {
                string literal = template.Substring(index, match.Index - index);

                if (!string.IsNullOrWhiteSpace(literal))
                {
                    if (partStack.Count == 0)
                    {
                        yield return new LiteralPart(literal);
                    }
                    else
                    {
                        partStack.Peek().Parts.Add(new LiteralPart(literal));
                    }
                }

                string whiteSpace = match.Groups[1].Value;

                if (whiteSpace.Length > 0)
                {
                    if (partStack.Count == 0)
                    {
                        index += whiteSpace.Length;
                        yield return new LiteralPart(whiteSpace);
                    }
                    else
                    {
                        partStack.Peek().Parts.Add(new LiteralPart(whiteSpace));
                    }
                }

                string marker = match.Groups[2].Value.Trim();

                bool stripOutNewLine = false;

                if (marker[0] == '#')
                {
                    string sectionName = marker.Substring(1).Trim();
                    partStack.Push(new SectionInfo(sectionName, false, false));

                    stripOutNewLine = true;
                }
                else if (marker[0] == '^')
                {
                    string sectionName = marker.Substring(1).Trim();
                    if (ExpressionRegex.IsMatch(sectionName))
                    {
                        if (sectionName.EndsWith("/"))
                        {
                            var section = new SectionInfo(sectionName.Substring(0, sectionName.Length - 1), false, true);
                            if (partStack.Count == 0)
                            {
                                yield return
                                    new ExpressionPart(
                                        section.Name,
                                        true,
                                        section.Properties,
                                        section.JsonKeys,
                                        section.Parts);
                            }
                            else
                            {
                                partStack.Peek()
                                    .Parts.Add(
                                        new ExpressionPart(
                                            section.Name,
                                            true,
                                            section.Properties,
                                            section.JsonKeys,
                                            section.Parts));
                            }
                        }
                        else
                        {
                            partStack.Push(new SectionInfo(sectionName, true, false));
                            stripOutNewLine = true;
                        }
                    }
                    else
                    {
                        partStack.Push(new SectionInfo(sectionName, true, false));
                        stripOutNewLine = true;
                    }
                }
                else if (marker[0] == '/')
                {
                    string sectionName = marker.Substring(1).Trim();
                    SectionInfo sectionInfo = partStack.Pop();

                    if (sectionInfo.Name != sectionName)
                    {
                        throw new ArgumentOutOfRangeException(
                            "Expecting section:" + sectionInfo.Name + " at position " + index);
                    }

                    if (ExpressionRegex.IsMatch(sectionName))
                    {
                        if (partStack.Count == 0)
                        {
                            yield return
                                new ExpressionPart(
                                    sectionName,
                                    sectionInfo.Inverted,
                                    sectionInfo.Properties,
                                    sectionInfo.JsonKeys,
                                    sectionInfo.Parts);
                        }
                        else
                        {
                            partStack.Peek()
                                .Parts.Add(
                                    new ExpressionPart(
                                        sectionName,
                                        sectionInfo.Inverted,
                                        sectionInfo.Properties,
                                        sectionInfo.JsonKeys,
                                        sectionInfo.Parts));
                        }
                    }
                    else
                    {
                        if (partStack.Count == 0)
                        {
                            yield return new SectionPart(sectionName, sectionInfo.Inverted, sectionInfo.Parts);
                        }
                        else
                        {
                            partStack.Peek()
                                .Parts.Add(new SectionPart(sectionName, sectionInfo.Inverted, sectionInfo.Parts));
                        }
                    }

                    stripOutNewLine = true;
                }
                else if (marker[0] == '>')
                {
                    yield return new TemplateInclude(marker.Substring(1).Trim());
                    stripOutNewLine = true;
                }
                else if (marker[0] != '!')
                {
                    if (ExpressionRegex.IsMatch(marker))
                    {
                        marker = marker.Trim();
                        if (marker.EndsWith("/"))
                        {
                            var section = new SectionInfo(marker.Substring(0, marker.Length - 1), false, true);
                            if (partStack.Count == 0)
                            {
                                yield return
                                    new ExpressionPart(
                                        section.Name,
                                        false,
                                        section.Properties,
                                        section.JsonKeys,
                                        section.Parts);
                            }
                            else
                            {
                                partStack.Peek()
                                    .Parts.Add(
                                        new ExpressionPart(
                                            section.Name,
                                            false,
                                            section.Properties,
                                            section.JsonKeys,
                                            section.Parts));
                            }
                        }
                        else
                        {
                            var section = new SectionInfo(marker, false, true);
                            partStack.Push(section);
                            stripOutNewLine = true;
                        }
                    }
                    else
                    {
                        if (partStack.Count == 0)
                        {
                            yield return new VariablePart(marker.Trim());
                        }
                        else
                        {
                            partStack.Peek().Parts.Add(new VariablePart(marker.Trim()));
                        }
                    }
                }

                index = match.Index + match.Length;

                Match s;
                if (stripOutNewLine && (s = StripRegex.Match(template, index)).Success)
                {
                    index += s.Length;
                }
            }

            if (index < template.Length)
            {
                yield return new LiteralPart(template.Substring(index));
            }
        }

        private class SectionInfo
        {
            private readonly bool inverted;

            private readonly Collection<string> jsonKeys;

            private readonly string name;

            private readonly Collection<ITemplatePart> parts;

            private readonly IDictionary<string, object> properties;

            public SectionInfo(string name, bool inverted, bool expression)
            {
                this.name = name;
                this.inverted = inverted;
                this.parts = new Collection<ITemplatePart>();
                this.properties = new Dictionary<string, object>();
                this.jsonKeys = new Collection<string>();
                if (expression)
                {
                    Match match = ExpressionRegex.Match(name);

                    if (match.Success)
                    {
                        this.name = match.Groups[1].Value.Trim() + ":" + match.Groups[2].Value.Trim();
                        string values = match.Groups[3].Value.Trim();

                        IEnumerable<KeyValuePair<string, string>> attributesMatches =
                            ExpressionParamRegex1.Matches(values)
                                .Cast<Match>()
                                .Concat(ExpressionParamRegex2.Matches(values).Cast<Match>())
                                .Where(
                                    x =>
                                    x.Success && !string.IsNullOrWhiteSpace(x.Groups[1].Value)
                                    && !string.IsNullOrWhiteSpace(x.Groups[2].Value))
                                .Select(x => new KeyValuePair<string, string>(x.Groups[1].Value, x.Groups[2].Value));
                        IEnumerable<KeyValuePair<string, string>> jsonMatches =
                            ExpressionParamRegex3.Matches(values)
                                .Cast<Match>()
                                .Concat(ExpressionParamRegex4.Matches(values).Cast<Match>())
                                .Where(
                                    x =>
                                    x.Success && !string.IsNullOrWhiteSpace(x.Groups[1].Value)
                                    && !string.IsNullOrWhiteSpace(x.Groups[2].Value))
                                .Select(x => new KeyValuePair<string, string>(x.Groups[1].Value, x.Groups[2].Value));

                        foreach (var attributesMatch in attributesMatches)
                        {
                            if (!this.properties.ContainsKey(attributesMatch.Key))
                            {
                                this.properties.Add(attributesMatch.Key, attributesMatch.Value);
                            }
                        }

                        foreach (var jsonMatch in jsonMatches)
                        {
                            if (!this.jsonKeys.Contains(jsonMatch.Key))
                            {
                                this.jsonKeys.Add(jsonMatch.Key);
                                this.properties.Add(jsonMatch.Key, jsonMatch.Value);
                            }
                        }
                    }
                }
            }

            public bool Inverted
            {
                get
                {
                    return this.inverted;
                }
            }

            public ICollection<string> JsonKeys
            {
                get
                {
                    return this.jsonKeys;
                }
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
            }

            public Collection<ITemplatePart> Parts
            {
                get
                {
                    return this.parts;
                }
            }

            public IDictionary<string, object> Properties
            {
                get
                {
                    return this.properties;
                }
            }
        }
    }
}