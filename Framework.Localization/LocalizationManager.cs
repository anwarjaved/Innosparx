namespace Framework.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;

    using Framework.Caching;
    using Framework.Collections;
    using Framework.Ioc;
    using Framework.Paging;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Manager for localizations.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class LocalizationManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The language lookup.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public static readonly IDictionary<int, LanguageCode> LanguageLookup = GetLangLookupDictionary();

        private static readonly IDictionary<LanguageCode, IList<int>> ReverseLanguageLookup = GetReverseLangLookupDictionary();


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets language lookup dictionary.
        /// </summary>
        ///
        /// <returns>
        ///     The language lookup dictionary.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        private static IDictionary<int, LanguageCode> GetLangLookupDictionary()
        {
            Array values = Enum.GetValues(typeof(LanguageCode));
            IDictionary<int, LanguageCode> dictionary = new Dictionary<int, LanguageCode>();

            foreach (Enum @enum in values)
            {
                var languages = @enum.ToDescription().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                languages.ForEach(
                    x =>
                        {
                            int code = Convert.ToInt32(x);

                            if (!dictionary.ContainsKey(code))
                            {
                                dictionary.Add(code, (LanguageCode)@enum);
                            }
                        });

            }
            return dictionary;
        }

        private static IDictionary<LanguageCode, IList<int>> GetReverseLangLookupDictionary()
        {
            Array values = Enum.GetValues(typeof(LanguageCode));
            IDictionary<LanguageCode, IList<int>> dictionary = new Dictionary<LanguageCode, IList<int>>();

            foreach (Enum @enum in values)
            {
                var languages = @enum.ToDescription().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                dictionary.Add((LanguageCode)@enum, languages.Select(x => Convert.ToInt32(x)).ToList());
            }
            return dictionary;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the language.
        /// </summary>
        ///
        /// <returns>
        ///     The language.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetLanguage()
        {
            return GetLanguage(CultureInfo.CurrentUICulture);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets language code.
        /// </summary>
        ///
        /// <returns>
        ///     The language code.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static LanguageCode GetLanguageCode()
        {
            return GetLanguageCode(CultureInfo.CurrentUICulture);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets language code.
        /// </summary>
        ///
        /// <param name="culture">
        ///     The culture.
        /// </param>
        ///
        /// <returns>
        ///     The language code.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        private static LanguageCode GetLanguageCode(CultureInfo culture)
        {
            var language = LanguageLookup.Where(x => x.Key == culture.LCID)
                              .Select(x => x.Value)
                              .FirstOrDefault()
                              .DefaultIfEmpty(LanguageCode.English);

            return language;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets language lcid.
        /// </summary>
        ///
        /// <returns>
        ///     The language lcid.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static int GetLanguageLCID()
        {
            return CultureInfo.CurrentUICulture.LCID;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets text information.
        /// </summary>
        ///
        /// <returns>
        ///     The text information.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TextInfo GetTextInfo()
        {
            return GetTextInfo(LanguageCode.English);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets text information.
        /// </summary>
        ///
        /// <param name="language">
        ///     The language.
        /// </param>
        ///
        /// <returns>
        ///     The text information.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TextInfo GetTextInfo(LanguageCode language)
        {
            return new CultureInfo(ReverseLanguageLookup[language].First()).TextInfo;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the language.
        /// </summary>
        ///
        /// <param name="culture">
        ///     The culture.
        /// </param>
        ///
        /// <returns>
        ///     The language.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetLanguage(CultureInfo culture)
        {
            return
                LanguageLookup.Where(x => x.Key == (culture.LCID))
                              .Select(x => x.Value.ToString("G").ToLowerInvariant())
                              .FirstOrDefault()
                              .DefaultIfEmpty("english") + "-lang";
        }

        /// <summary>
        /// Gets model file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The model file name.</returns>
        /// -------------------------------------------------------------------------------------------------
        /// -------------------------------------------------------------------------------------------------
        public static string GetLocalizedFileName(string fileName, CultureInfo culture)
        {
            var language =
                LanguageLookup.Where(x => x.Key == culture.LCID && x.Value != LanguageCode.English)
                              .Select(x => x.Value.ToString("G").ToLowerInvariant())
                              .FirstOrDefault();

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return fileNameWithoutExtension + (!string.IsNullOrWhiteSpace(language) ? "." + language : string.Empty) + extension;
        }

        /// <summary>
        /// Gets the name of the localized file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public static string GetLocalizedFileName(string fileName)
        {
            return GetLocalizedFileName(fileName, CultureInfo.CurrentCulture);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets language lcid.
        /// </summary>
        ///
        /// <param name="language">
        ///     The language.
        /// </param>
        ///
        /// <returns>
        ///     The language lcid.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static int GetLanguageLCID(LanguageCode language)
        {
            return
                ReverseLanguageLookup[language].First();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets a language.
        /// </summary>
        ///
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <param name="language">
        ///     The language.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SetLanguage(HttpContextBase context, LanguageCode language)
        {
            if (context != null)
            {
                int lcid = LocalizationManager.GetLanguageLCID(language);
                HttpCookie cookie = context.Request.Cookies[LocalizationConstants.CookieName]
                                    ?? new HttpCookie(
                                        LocalizationConstants.CookieName);
                cookie.Value = lcid.ToString(CultureInfo.InvariantCulture);
                cookie.Expires = DateTime.UtcNow.AddDays(7);

                context.Response.Cookies.Add(cookie);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lcid);
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a text.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="defaultText">
        ///     (optional) the default text.
        /// </param>
        ///
        /// <returns>
        ///     The text.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetText(string key, string defaultText = "")
        {
            return GetText(CultureInfo.CurrentUICulture, key, defaultText);
        }



        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets tooltip text.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 4:00 PM.
        /// </remarks>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="defaultText">
        ///     (optional) the default text.
        /// </param>
        ///
        /// <returns>
        ///     The tooltip text.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetTooltipText(string key, string defaultText = "")
        {
            return GetTooltipText(CultureInfo.CurrentUICulture, key, defaultText);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the resources.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 5:08 PM.
        /// </remarks>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        ///
        /// <returns>
        ///     The resources.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<ILanguageResource> GetResources(string key)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Get(key);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a resource.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 5:08 PM.
        /// </remarks>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="code">
        ///     (Optional) the code.
        /// </param>
        ///
        /// <returns>
        ///     The resource.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ILanguageResource GetResource(string key, LanguageCode code = LanguageCode.English)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Get(key, code);
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a resource.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 4:03 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="uiCulture">
        ///     The culture.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        ///
        /// <returns>
        ///     The resource.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ILanguageResource GetResource(CultureInfo uiCulture, string key)
        {
            try
            {
                if (uiCulture == null)
                {
                    throw new ArgumentNullException("uiCulture");
                }


                Dictionary<string, LanguageResource> dictionary = null;

                ICache cache = Container.TryGet<ICache>();

                if (cache != null && cache.Exists(LocalizationConstants.LocalizationKey))
                {
                    dictionary = cache.Get<Dictionary<string, LanguageResource>>(LocalizationConstants.LocalizationKey);
                }

                if (dictionary == null)
                {
                    dictionary = GetLanguageDictionary();

                    if (cache != null)
                    {
                        cache.Set(LocalizationConstants.LocalizationKey, dictionary, TimeSpan.FromMinutes(30));
                    }
                }

                string cultureKey = GetLanguageCode(uiCulture).ToString("G").ToLowerInvariant() + "[" + key + "]";
                string defaultcultureKey = LanguageCode.English.ToString("G").ToLowerInvariant() + "[" + key + "]";

                ILanguageResource value = null;

                if (dictionary.ContainsKey(cultureKey))
                {
                    value = dictionary[cultureKey];
                }

                if (value == null)
                {
                    if (dictionary.ContainsKey(defaultcultureKey))
                    {
                        value = dictionary[defaultcultureKey];
                    }
                }

                return value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determine if we can show tooltip.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 3:59 PM.
        /// </remarks>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        ///
        /// <returns>
        ///     true if we can show tooltip, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool CanShowTooltip(string key)
        {
            return CanShowTooltip(CultureInfo.CurrentUICulture, key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determine if we can show tooltip.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 3:11 PM.
        /// </remarks>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="uiCulture">
        ///     The culture.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        ///
        /// <returns>
        ///     true if we can show tooltip, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool CanShowTooltip(CultureInfo uiCulture, string key)
        {
            try
            {
                if (uiCulture == null)
                {
                    throw new ArgumentNullException("uiCulture");
                }


                Dictionary<string, LanguageResource> dictionary = null;

                ICache cache = Container.TryGet<ICache>();

                if (cache != null && cache.Exists(LocalizationConstants.LocalizationKey))
                {
                    dictionary = cache.Get<Dictionary<string, LanguageResource>>(LocalizationConstants.LocalizationKey);
                }

                if (dictionary == null)
                {
                    dictionary = GetLanguageDictionary();

                    if (cache != null)
                    {
                        cache.Set(LocalizationConstants.LocalizationKey, dictionary, TimeSpan.FromMinutes(30));
                    }
                }

                string cultureKey = GetLanguageCode(uiCulture).ToString("G").ToLowerInvariant() + "[" + key + "]";
                string defaultcultureKey = LanguageCode.English.ToString("G").ToLowerInvariant() + "[" + key + "]";
                LanguageResource resource = null;

                if (dictionary.ContainsKey(cultureKey))
                {
                    resource = dictionary[cultureKey];
                }
                else if (dictionary.ContainsKey(defaultcultureKey))
                {
                    resource = dictionary[defaultcultureKey];
                }

                if (resource != null && resource.CanShowTooltip)
                {
                    if (!string.IsNullOrWhiteSpace(resource.TooltipText))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets tooltip text.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 3:06 PM.
        /// </remarks>
        ///
        /// <param name="uiCulture">
        ///     The culture.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="defaultText">
        ///     (optional) the default text.
        /// </param>
        ///
        /// <returns>
        ///     The tooltip text.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetTooltipText(CultureInfo uiCulture, string key, string defaultText = "")
        {
            try
            {
                if (uiCulture == null)
                {
                    throw new ArgumentNullException("uiCulture");
                }


                Dictionary<string, LanguageResource> dictionary = null;

                ICache cache = Container.TryGet<ICache>();

                if (cache != null && cache.Exists(LocalizationConstants.LocalizationKey))
                {
                    dictionary = cache.Get<Dictionary<string, LanguageResource>>(LocalizationConstants.LocalizationKey);
                }

                if (dictionary == null)
                {
                    dictionary = GetLanguageDictionary();

                    if (cache != null)
                    {
                        cache.Set(LocalizationConstants.LocalizationKey, dictionary, TimeSpan.FromMinutes(30));
                    }
                }

                string cultureKey = GetLanguageCode(uiCulture).ToString("G").ToLowerInvariant() + "[" + key + "]";
                string defaultcultureKey = LanguageCode.English.ToString("G").ToLowerInvariant() + "[" + key + "]";

                string value = string.Empty;

                if (dictionary.ContainsKey(cultureKey))
                {
                    value = dictionary[cultureKey].TooltipText;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (dictionary.ContainsKey(defaultcultureKey))
                    {
                        value = dictionary[defaultcultureKey].TooltipText;
                    }
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    value = !string.IsNullOrWhiteSpace(defaultText) ? cultureKey : defaultText;
                }


                return value;
            }
            catch (Exception)
            {
                return defaultText;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a text.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="uiCulture">
        ///     The culture.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="defaultText">
        ///     (optional) the default text.
        /// </param>
        ///
        /// <returns>
        ///     The text.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetText(CultureInfo uiCulture, string key, string defaultText = "")
        {
            try
            {
                if (uiCulture == null)
                {
                    throw new ArgumentNullException("uiCulture");
                }


                Dictionary<string, LanguageResource> dictionary = null;

                ICache cache = Container.TryGet<ICache>();

                if (cache != null && cache.Exists(LocalizationConstants.LocalizationKey))
                {
                    dictionary = cache.Get<Dictionary<string, LanguageResource>>(LocalizationConstants.LocalizationKey);
                }

                if (dictionary == null)
                {
                    dictionary = GetLanguageDictionary();

                    if (cache != null)
                    {
                        cache.Set(LocalizationConstants.LocalizationKey, dictionary, TimeSpan.FromMinutes(30));
                    }
                }

                string cultureKey = GetLanguageCode(uiCulture).ToString("G").ToLowerInvariant() + "[" + key + "]";
                string defaultcultureKey = LanguageCode.English.ToString("G").ToLowerInvariant() + "[" + key + "]";

                string value = string.Empty;

                if (dictionary.ContainsKey(cultureKey))
                {
                    value = dictionary[cultureKey].Text;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (dictionary.ContainsKey(defaultcultureKey))
                    {
                        value = dictionary[defaultcultureKey].Text;
                    }
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    value = !string.IsNullOrWhiteSpace(defaultText) ? cultureKey : defaultText;
                }


                return value;
            }
            catch (Exception)
            {
                return defaultText;
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets language dictionary.
        /// </summary>
        ///
        /// <returns>
        ///     The language dictionary.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        private static Dictionary<string, LanguageResource> GetLanguageDictionary()
        {
            Dictionary<string, LanguageResource> dictionary = new Dictionary<string, LanguageResource>(StringComparer.OrdinalIgnoreCase);
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();
            IEnumerable<IGrouping<LanguageCode, ILanguageResource>> list = provider.Get(0, int.MaxValue).Distinct().GroupBy(x => x.Code);

            foreach (var langResources in list)
            {
                foreach (var languageResource in langResources)
                {
                    LanguageResource resource = new LanguageResource(languageResource);
                    string key = langResources.Key.ToString("G").ToLowerInvariant() + "[" + resource.Key + "]";
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, resource);
                    }
                }
            }

            return dictionary;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Refreshes.
        /// </summary>
        ///
        /// <param name="forceRefresh">
        ///     true to force refresh.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void Refresh(bool forceRefresh)
        {
            ICache cache = Container.Get<ICache>();

            cache.Remove(LocalizationConstants.LocalizationKey);

        }

        public static void RemoveResource(string key, LanguageCode code = LanguageCode.English)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                provider.RemoveResource(key, code);
                Refresh(true);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="page">
        ///     (Optional) the page.
        /// </param>
        /// <param name="size">
        ///     (Optional) the size.
        /// </param>
        ///
        /// <returns>
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IPagedList<ILanguageResource> Get(int page = 0, int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Get(page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        public static IPagedList<ILanguageResource> Find(string text, int page = 0, int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Find(text, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        public static IPagedList<ILanguageResource> Find(
            string text,
            string sortBy,
            string sortDirection,
            int page = 0,
            int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Find(text, sortBy, sortDirection, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="code">
        ///     the code.
        /// </param>
        /// <param name="page">
        ///     (Optional) the page.
        /// </param>
        /// <param name="size">
        ///     (Optional) the size.
        /// </param>
        ///
        /// <returns>
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IPagedList<ILanguageResource> Get(LanguageCode code, int page = 0, int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Get(code, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets by category.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/19/2014 11:59 AM.
        /// </remarks>
        ///
        /// <param name="category">
        ///     The category.
        /// </param>
        /// <param name="page">
        ///     (Optional) the page.
        /// </param>
        /// <param name="size">
        ///     (Optional) the size.
        /// </param>
        ///
        /// <returns>
        ///     The by category.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IPagedList<ILanguageResource> GetByCategory(string category, int page = 0, int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.GetByCategory(category, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        public static IPagedList<ILanguageResource> Get(
            string sortBy,
            string sortDirection,
            int page = 0,
            int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.Get(sortBy, sortDirection, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        public static IPagedList<ILanguageResource> GetByCategory(
            string category,
            string sortBy,
            string sortDirection,
            int page = 0,
            int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.GetByCategory(category, sortBy, sortDirection, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        public static IPagedList<ILanguageResource> GetByCategory(
            string category,
            string text,
            string sortBy,
            string sortDirection,
            int page = 0,
            int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.GetByCategory(category, text, sortBy, sortDirection, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }



        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets by category.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/24/2014 3:18 PM.
        /// </remarks>
        ///
        /// <param name="category">
        ///     The category.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="page">
        ///     (Optional) the page.
        /// </param>
        /// <param name="size">
        ///     (Optional) the size.
        /// </param>
        ///
        /// <returns>
        ///     The by category.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IPagedList<ILanguageResource> GetByCategory(string category, string text, int page = 0, int size = 10)
        {
            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                return provider.GetByCategory(category, text, page, size);
            }

            return PagedList<ILanguageResource>.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves a resource.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 4:16 PM.
        /// </remarks>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="tooltipText">
        ///     (Optional) the tooltip text.
        /// </param>
        /// <param name="canShowTooltip">
        ///     (Optional) the can show tooltip.
        /// </param>
        /// <param name="category">
        ///     (Optional) The category.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SaveResource(
            string key,
            string text,
            string tooltipText = null,
            bool? canShowTooltip = null, string category = null)
        {
            SaveResource(LanguageCode.English, key, text, tooltipText, canShowTooltip, category);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves a resource.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 4:16 PM.
        /// </remarks>
        ///
        /// <param name="code">
        ///     The culture.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="tooltipText">
        ///     (Optional) the tooltip text.
        /// </param>
        /// <param name="canShowTooltip">
        ///     (Optional) the can show tooltip.
        /// </param>
        /// <param name="category">
        ///     (Optional) The category.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SaveResource(LanguageCode code,
            string key,
            string text,
            string tooltipText = null,
            bool? canShowTooltip = null, string category = null)
        {

            ILanguageProvider provider = Container.TryGet<ILanguageProvider>();

            if (provider != null)
            {
                provider.SaveResource(key, text, code, tooltipText, canShowTooltip, category);
                Refresh(true);
            }
        }
    }
}
