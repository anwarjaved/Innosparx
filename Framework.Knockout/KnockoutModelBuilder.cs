namespace Framework.Knockout
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Text;

    using Framework.Activator;
    using Framework.Configuration;
    using Framework.DataAnnotations;
    using Framework.Serialization.Json;

    using Container = Framework.Ioc.Container;
    using DescriptionAttribute = Framework.DescriptionAttribute;
    using UrlAttribute = Framework.DataAnnotations.UrlAttribute;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Knockout model builder.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class KnockoutModelBuilder
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Scan assembles for <see cref="KnockoutAssemblyAttribute"/> and Build Model from types.
        /// </summary>
        ///
        /// <param name="folderPath">
        ///     The folder path to save.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public static void Save(string folderPath)
        {
            IEnumerable<Assembly> assemblies = from Assembly assembly in ActivationManager.Assemblies
                                               where
                                                 assembly.GetCustomAttributes(typeof(KnockoutAssemblyAttribute), false).Any()
                                               select assembly;

            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetExportedTypes().Where(type => (type.IsClass || type.IsEnum) && type.GetCustomAttributes(typeof(KnockoutModelAttribute), false).Any()));
            }

            Save(folderPath, types);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Build Model from types.
        /// </summary>
        ///
        /// <param name="folderPath">
        ///     The folder path to save.
        /// </param>
        /// <param name="list">
        ///     The list.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void Save(string folderPath, IReadOnlyList<Type> list)
        {
            bool localizationEnabled = false;

            var config = ConfigManager.GetConfig();

            if (config != null)
            {
                localizationEnabled = config.Application.LocalizationEnabled;
            }

            folderPath = Path.Combine(folderPath, "models");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }



            string fileName = Path.Combine(folderPath, "models.js");

            if (!File.Exists(fileName))
            {
                using (StreamWriter stream = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    stream.Write(Build(list));

                }

            }

        }


        private static string Build(IReadOnlyList<Type> list)
        {
            List<Type> extraTypes = new List<Type>();

            StringBuilder sb = new StringBuilder();

            using (StringWriter stream = new StringWriter(sb))
            {
                using (IndentedTextWriter writer = new IndentedTextWriter(stream))
                {
                    foreach (Type type in list)
                    {
                        writer.Indent++;
                        if (type.IsClass)
                        {
                            BuildModel(type, writer, extraTypes);
                        }
                        else if (type.IsEnum)
                        {
                            WriteEnumModel(type, writer);
                        }
                        writer.Indent--;
                    }

                    foreach (Type type in extraTypes)
                    {
                        if (!list.Contains(type))
                        {
                            writer.Indent++;
                            if (type.IsClass)
                            {
                                BuildModel(type, writer);
                            }
                            else if (type.IsEnum)
                            {
                                WriteEnumModel(type, writer);
                            }
                            writer.Indent--;
                        }
                    }

                    writer.WriteLine();
                }

            }

            return sb.ToString();
        }
        private static void BuildModel(Type type, IndentedTextWriter writer, List<Type> extraTypes = null)
        {
            var typeName = type.Name;

            if (type.IsGenericType)
            {
                typeName = typeName.Remove(typeName.IndexOf('`'));
            }

            writer.WriteLine("function " + typeName + "() {");
            writer.Indent++;

            writer.WriteLine("var self = this;");
            writer.WriteLine("self.IsValidatable = ko.observable(true);");
            List<string> ignoreList = new List<string>() { "\"observable\"", "\"validationProperties\"", "\"ignore\"", "\"include\"", "\"type\"", "\"mapping\"", "\"IsValidatable\"", "\"_create\"", "\"hasID\"" };

            List<string> includeList = new List<string>() { "\"_destroy\"" };
            List<string> createList = new List<string>() { };
            List<string> validationList = new List<string>();

            foreach (PropertyInfo p in type.GetProperties().Where(p => p.CanRead))
            {
                bool isIEnumerable = false;
                bool isDictionary = false;
                bool isClass = false;
                Type mappingType = p.PropertyType;

                if (p.GetCustomAttribute<IgnoreAttribute>() != null)
                {
                    ignoreList.Add("\"" + p.Name + "\"");
                    continue;
                }

                if (p.CanWrite)
                {
                    includeList.Add("\"" + p.Name + "\"");
                }
                else
                {
                    ignoreList.Add("\"" + p.Name + "\"");
                }


                if (extraTypes != null)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                    {
                        if (p.PropertyType.IsGenericType)
                        {
                            mappingType = p.PropertyType.GetGenericArguments()[0];

                            if (!mappingType.ContainsGenericParameters && mappingType.IsClass && mappingType != typeof(string) && mappingType != typeof(object))
                            {
                                if (!extraTypes.Contains(mappingType))
                                {
                                    extraTypes.Add(mappingType);
                                }

                                isIEnumerable = true;
                            }

                        }

                        if (typeof(IDictionary).IsAssignableFrom(p.PropertyType))
                        {
                            createList.Add("\"" + p.Name + "\"");
                            isDictionary = true;
                        }

                    }
                    else if (p.PropertyType.IsEnum || (p.PropertyType.IsClass && p.PropertyType != typeof(string) && p.PropertyType != typeof(object)))
                    {
                        if (!extraTypes.Contains(p.PropertyType))
                        {
                            extraTypes.Add(p.PropertyType);
                        }

                        if (p.PropertyType.IsClass)
                        {
                            isClass = true;
                        }
                    }
                }

                bool isValidatable;

                var extenders = WriteExtenders(p, out isValidatable);

                if (isValidatable)
                {
                    validationList.Add("\"" + p.Name + "\"");
                }


                if (p.PropertyType == typeof(string))
                {
                    writer.WriteLine("self." + p.Name + GetValuePropertyDefaultValue(p));
                    writer.WriteLine("self." + p.Name + extenders + ";");
                }
                else if (p.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("self." + p.Name + GetValuePropertyDefaultValue(p));
                    writer.WriteLine("self." + p.Name + extenders + ";");
                }
                else if (p.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("self." + p.Name + GetValuePropertyDefaultValue(p));
                    writer.WriteLine("self." + p.Name + extenders + ";");
                }
                else if (p.PropertyType.IsValueType)
                {
                    if (p.PropertyType == typeof(bool))
                    {
                        writer.WriteLine("self." + p.Name + GetValuePropertyDefaultValue(p));
                        writer.WriteLine("self." + p.Name + extenders + ";");
                    }
                    else if (p.PropertyType.IsEnum)
                    {
                        writer.WriteLine("self." + p.Name + GetValuePropertyDefaultValue(p));
                        writer.WriteLine("self." + p.Name + extenders + ";");
                    }
                    else
                    {
                        writer.WriteLine("self." + p.Name + GetValuePropertyDefaultValue(p));
                        writer.WriteLine("self." + p.Name + extenders + ";");
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                {
                    if (p.PropertyType == typeof(object))
                    {
                        writer.WriteLine("self." + p.Name + " = ko.observableArray();");
                    }
                    else
                    {
                        if (p.PropertyType.IsGenericType)
                        {
                            Type[] typeParameters = p.PropertyType.GetGenericArguments();

                            if (typeParameters.Length > 1)
                            {
                                isIEnumerable = false;
                            }
                        }
                        if (isDictionary)
                        {
                            writer.WriteLine("self." + p.Name + " = ko.observableDictionary();");
                            writer.WriteLine("self." + p.Name + extenders + ";");
                        }
                        else if (isIEnumerable)
                        {
                            var mappingName = mappingType.Name;
                            writer.WriteLine("self." + p.Name + " = ko.observableArray().mapping(function(){ return new " + mappingName + "(); });");
                            writer.WriteLine("self." + p.Name + extenders + ";");
                        }
                        else
                        {
                            writer.WriteLine("self." + p.Name + " = ko.observableArray();");
                            writer.WriteLine("self." + p.Name + extenders + ";");
                        }
                    }
                }
                else if (p.PropertyType.IsClass)
                {
                    WriteObservable(writer, p, isClass, mappingType);
                }
            }

            writer.WriteLine("self.include = [" + string.Join(", ", includeList) + "];");
            //writer.WriteLine("self.validationModel = ko.validatedObservable({" + string.Join(", ", validationList) + "});");
            writer.WriteLine("self.validationProperties = [" + string.Join(", ", validationList) + "];");

            writer.WriteLine("self._create = [" + string.Join(", ", createList) + "];");
            writer.WriteLine("self.ignore = [" + string.Join(", ", ignoreList) + "];");
            writer.WriteLine("self.type = \"" + type.FullName + "\";");
            writer.WriteLine("ko.utils.makeComputeds(self);");
            writer.WriteLine("ko.utils.makeAsynCommands(self);");

            writer.WriteLine("ko.initModel(self);");
            writer.WriteLine("if (self.init) self.init();");

            //writer.WriteLine("self._Observable = false;");


            writer.WriteLine("}");
            writer.WriteLine();

            writer.WriteLine(typeName + ".Mapping = function(){ return new " + typeName + "(); }");

            writer.WriteLine();
            writer.Indent--;
        }

        private static string GetValuePropertyDefaultValue(PropertyInfo property)
        {
            DefaultValueAttribute defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute != null)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Guid))
                {
                    return " = ko.observable(\"{0}\");".FormatString(defaultValueAttribute.Value);
                }

                if (property.PropertyType == typeof(bool))
                {
                    return " = ko.observable({0});".FormatString(bool.Parse(defaultValueAttribute.Value.ToString()) ? "true" : "false");
                }

                if (property.PropertyType.IsEnum)
                {
                    try
                    {
                        var enumValue = ((Enum)Enum.Parse(property.PropertyType, Convert.ToString(defaultValueAttribute.Value))).ToString("G");

                        return " = ko.observable(\"{0}\");".FormatString(enumValue);
                    }
                    catch (Exception)
                    {
                        return " = ko.observable();";
                    }
                }

                return " = ko.observable({0});".FormatString(defaultValueAttribute.Value);
            }
            return " = ko.observable();";
        }

        private static void WriteObservable(IndentedTextWriter writer, PropertyInfo p, bool init, Type mappingType)
        {
            if (p.PropertyType == typeof(object))
            {
                writer.WriteLine("self." + p.Name + " = ko.observable();");
            }
            else
            {
                if (init)
                {
                    writer.WriteLine("self." + p.Name + " = ko.observable(new " + p.PropertyType.Name + "());");

                }
                else
                {
                    writer.WriteLine("self." + p.Name + " = ko.observable().mapping(function(){ return new " + mappingType.Name + "(); })");
                }
            }
        }

        
        private static string WriteExtenders(PropertyInfo p, out bool isValidatable)
        {
            isValidatable = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(".extend({ editState : false, disableValidation : false, empty : true })");

            RequiredAttribute requiredAttribute = p.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null)
            {
                sb.Append(".extend({ required: { message: \"{0}\", onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{1}\"); } } })".FormatString(requiredAttribute.FormatErrorMessage(p.Name), p.Name));
                isValidatable = true;
            }

            MinLengthAttribute minLengthAttribute = p.GetCustomAttribute<MinLengthAttribute>();
            if (minLengthAttribute != null)
            {
                sb.Append(".extend({ minLength: { message: \"{0}\", params: {1}, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(minLengthAttribute.FormatErrorMessage(p.Name), minLengthAttribute.Length, p.Name));
                isValidatable = true;
            }

            MaxLengthAttribute maxLengthAttribute = p.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null)
            {
                sb.Append(".extend({ maxLength: { message: \"{0}\", params: {1}, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(maxLengthAttribute.FormatErrorMessage(p.Name), maxLengthAttribute.Length, p.Name));
                isValidatable = true;
            }

            RegularExpressionAttribute regularExpressionAttribute = p.GetCustomAttribute<RegularExpressionAttribute>();
            if (regularExpressionAttribute != null)
            {
                sb.Append(".extend({ pattern: { message: \"{0}\", params: /{1}/, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(regularExpressionAttribute.FormatErrorMessage(p.Name), regularExpressionAttribute.Pattern, p.Name));
                isValidatable = true;
            }

            UrlAttribute urlAttribute = p.GetCustomAttribute<UrlAttribute>();
            if (urlAttribute != null)
            {
                sb.Append(".extend({ pattern: { message: \"{0}\", params: /{1}/, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(urlAttribute.FormatErrorMessage(p.Name), urlAttribute.Regex, p.Name));
                isValidatable = true;
            }

            DateAttribute dateAttribute = p.GetCustomAttribute<DateAttribute>();
            if (dateAttribute != null)
            {
                sb.Append(".extend({ date: { message: \"{0}\", onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{1}\"); } } })".FormatString(dateAttribute.FormatErrorMessage(p.Name), p.Name));
                isValidatable = true;
            }

            NumericAttribute numericAttribute = p.GetCustomAttribute<NumericAttribute>();
            if (numericAttribute != null)
            {
                sb.Append(
                    ".extend({ number: { message: \"{0}\", onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{1}\"); } } }).extend({ numeric: {2} })".FormatString(
                        numericAttribute.FormatErrorMessage(p.Name), p.Name, numericAttribute.Precision));
                isValidatable = true;
            }

            EmailAttribute emailAttribute = p.GetCustomAttribute<EmailAttribute>();
            if (emailAttribute != null)
            {
                sb.Append(".extend({ email: { message: \"{0}\", onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{1}\"); } } })".FormatString(emailAttribute.FormatErrorMessage(p.Name), p.Name));
                isValidatable = true;
            }

            DigitsAttribute digitsAttribute = p.GetCustomAttribute<DigitsAttribute>();
            if (digitsAttribute != null)
            {
                sb.Append(".extend({ digit: { message: \"{0}\", onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{1}\"); } } }).extend({ numeric: 0 })".FormatString(digitsAttribute.FormatErrorMessage(p.Name), p.Name));
                isValidatable = true;
            }

            MinAttribute minAttribute = p.GetCustomAttribute<MinAttribute>();
            if (minAttribute != null)
            {
                sb.Append(".extend({ min: { message: \"{0}\", params: {1}, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(minAttribute.FormatErrorMessage(p.Name), minAttribute.Min, p.Name));
                isValidatable = true;
            }

            MaxAttribute maxAttribute = p.GetCustomAttribute<MaxAttribute>();
            if (maxAttribute != null)
            {
                sb.Append(".extend({ max: { message: \"{0}\", params: {1}, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(maxAttribute.FormatErrorMessage(p.Name), maxAttribute.Max, p.Name));
                isValidatable = true;
            }

            EqualToAttribute equalToAttribute = p.GetCustomAttribute<EqualToAttribute>();
            if (equalToAttribute != null)
            {
                sb.Append(".extend({ equal: { message: \"{0}\", params: {1}, onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(equalToAttribute.FormatErrorMessage(p.Name), equalToAttribute.OtherProperty, p.Name));
                isValidatable = true;
            }

            CompareAttribute compareAttribute = p.GetCustomAttribute<CompareAttribute>();
            if (compareAttribute != null)
            {
                sb.Append(".extend({ equal: { message: \"{0}\", params: \"{1}\", onlyIf: function() { return ko.utils.isPropertyValidatable(self, \"{2}\"); } } })".FormatString(compareAttribute.FormatErrorMessage(p.Name), compareAttribute.OtherProperty, p.Name));
                isValidatable = true;
            }

            FormatterAttribute formatterAttribute = p.GetCustomAttribute<FormatterAttribute>();

            if (formatterAttribute != null)
            {
                IJsonSerializer serializer = Container.Get<IJsonSerializer>();
                sb.Append(".formatted({0}, {1})".FormatString(formatterAttribute.Formatter, serializer.Serialize(formatterAttribute.Arguments)));
            }

            return sb.ToString();
        }

        private static void WriteEnumModel(Type type, IndentedTextWriter writer)
        {
            writer.WriteLine("var " + type.Name + " = [");
            int index = 1;

            writer.Indent++;
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo fieldInfo in fields)
            {
                DescriptionAttribute attribs =
                    (from attr in
                         fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                     select attr).Cast<DescriptionAttribute>().FirstOrDefault();

                string description = null;
                if (attribs != null)
                {
                    description = attribs.GetLocalizedDescription();
                }
                if (string.IsNullOrWhiteSpace(description))
                {
                    description = fieldInfo.Name;
                }

                writer.Write("{ Text: \"{0}\", Value: \"{1}\", NumericValue: {2} }".FormatString(description, fieldInfo.Name, fieldInfo.GetRawConstantValue()));
                writer.WriteLine(index < fields.Length ? "," : string.Empty);
                index++;
            }

            writer.Indent--;
            writer.WriteLine("]");
            writer.WriteLine();
            writer.WriteLine();

            foreach (FieldInfo fieldInfo in fields)
            {
                DescriptionAttribute attribs =
            (from attr in
                 fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
             select attr).Cast<DescriptionAttribute>().FirstOrDefault();

                string description = null;
                if (attribs != null)
                {
                    description = attribs.GetLocalizedDescription();
                }
                if (string.IsNullOrWhiteSpace(description))
                {
                    description = fieldInfo.Name;
                }

                writer.WriteLine("{0}.{1} = { Text: \"{2}\", Value: {3} };".FormatString(type.Name, fieldInfo.Name, description, fieldInfo.GetRawConstantValue()));
            }

            writer.WriteLine();
            writer.WriteLine();
        }
    }
}
