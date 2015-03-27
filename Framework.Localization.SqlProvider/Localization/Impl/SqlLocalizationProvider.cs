namespace Framework.Localization.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Web.Configuration;

    using Framework.Domain;
    using Framework.Ioc;
    using Framework.Paging;

    [InjectBind(typeof(ILanguageProvider), "SqlLocalization", LifetimeType.Request)]
    public class SqlLocalizationProvider : ILanguageProvider
    {
        private readonly string nameOrConnectionString;

        public SqlLocalizationProvider()
        {
            var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.LocalizationContext"];
            if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
            {
                this.nameOrConnectionString = overriddenConnectionString;
            }

            if (string.IsNullOrWhiteSpace(this.nameOrConnectionString))
            {
                this.nameOrConnectionString = "AppContext";
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 3:25 PM.
        /// </remarks>
        /// <returns>
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> Get(int page = 0, int size = 10)
        {
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.OrderBy(x => x.Key).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="sortBy">
        ///     Describes who sort this ILanguageProvider.
        /// </param>
        /// <param name="sortDirection">
        ///     The sort direction.
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
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> Get(string sortBy, string sortDirection, int page = 0, int size = 10)
        {
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.Sort(sortBy, sortDirection).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
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
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> GetByCategory(string category, int page = 0, int size = 10)
        {
            return this.GetByCategory(category, null, page, size);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="category">
        ///     The category.
        /// </param>
        /// <param name="sortBy">
        ///     Describes who sort this ILanguageProvider.
        /// </param>
        /// <param name="sortDirection">
        ///     The sort direction.
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
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> GetByCategory(string category, string sortBy, string sortDirection, int page = 0, int size = 10)
        {
            return this.GetByCategory(category, null, sortBy, sortDirection, page, size);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
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
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> GetByCategory(string category, string text, int page = 0, int size = 10)
        {
           
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return configContext.Resources.Where(x => x.Category == category).OrderBy(x => x.Key).ToPagedList<ILanguageResource>(page, size);
                }

                return configContext.Resources.Where(x => x.Category == category && (x.Key.Contains(text) || x.Text.Contains(text))).OrderBy(x => x.Key).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="category">
        ///     The category.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="sortBy">
        ///     Describes who sort this ILanguageProvider.
        /// </param>
        /// <param name="sortDirection">
        ///     The sort direction.
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
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> GetByCategory(string category, string text, string sortBy, string sortDirection, int page = 0, int size = 10)
        {
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return configContext.Resources.Where(x => x.Category == category).Sort(sortBy, sortDirection).ToPagedList<ILanguageResource>(page, size);
                }

                return configContext.Resources.Where(x => x.Category == category && (x.Key.Contains(text) || x.Text.Contains(text))).Sort(sortBy, sortDirection).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        ///
        /// <returns>
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public IReadOnlyList<ILanguageResource> Get(string key)
        {
            key = key.ToLower();
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.Where(x => x.Key.StartsWith(key)).ToList();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first match.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/02/2014 1:16 PM.
        /// </remarks>
        ///
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
        ///     A list of.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> Find(string text, int page = 0, int size = 10)
        {
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.Where(x => x.Key.Contains(text) || x.Text.Contains(text)).OrderBy(x => x.Key).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first match.
        /// </summary>
        ///
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="sortBy">
        ///     Describes who sort this ILanguageProvider.
        /// </param>
        /// <param name="sortDirection">
        ///     The sort direction.
        /// </param>
        /// <param name="page">
        ///     (Optional) the page.
        /// </param>
        /// <param name="size">
        ///     (Optional) the size.
        /// </param>
        ///
        /// <returns>
        ///     A list of.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> Find(string text, string sortBy, string sortDirection, int page = 0, int size = 10)
        {
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.Where(x => x.Key.Contains(text) || x.Text.Contains(text)).Sort(sortBy, sortDirection).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="code">
        ///     the code.
        /// </param>
        ///
        /// <returns>
        ///     IReadOnlyList{ILanguageResource}.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public ILanguageResource Get(string key, LanguageCode code)
        {
            key = key.ToLower();
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.FirstOrDefault(x => x.Key == key && x.Code == code);
            }
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
        [SecuritySafeCritical]
        public IPagedList<ILanguageResource> Get(LanguageCode code, int page = 0, int size = 10)
        {
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                return configContext.Resources.Where(x => x.Code == code).OrderBy(x => x.Key).ToPagedList<ILanguageResource>(page, size);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves a resource.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/19/2014 12:06 PM.
        /// </remarks>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="text">
        ///     The text.
        /// </param>
        /// <param name="code">
        ///     (Optional) the code.
        /// </param>
        /// <param name="tooltipText">
        ///     (Optional) the tooltip text.
        /// </param>
        /// <param name="canShowTooltip">
        ///     (Optional) true if this ILanguageProvider can show tooltip.
        /// </param>
        /// <param name="category">
        ///     (Optional) The category.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public void SaveResource(
            string key,
            string text,
            LanguageCode code = LanguageCode.English,
            string tooltipText = null,
            bool? canShowTooltip = null, string category = null)
        {
            key = key.ToLower();
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                LanguageResource resource = configContext.Resources.FirstOrDefault(x => x.Key == key && x.Code == code);

                if (resource == null)
                {
                    resource = new LanguageResource();
                    resource.Code = code;
                    resource.Key = key.ToLower();
                    resource.Text = string.Empty;
                    resource.CanShowTooltip = true;
                    configContext.Resources.Add(resource);
                }

                if (text != null)
                {
                    resource.Text = text;
                }

                if (category != null)
                {
                    resource.Category = category;
                }

                if (tooltipText != null)
                {
                    resource.TooltipText = tooltipText;
                }

                if (canShowTooltip.HasValue)
                {
                    resource.CanShowTooltip = canShowTooltip.Value;
                }

                configContext.SaveChanges();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes the resource.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="code">
        ///     (Optional) the code.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public void RemoveResource(string key, LanguageCode code = LanguageCode.English)
        {
            key = key.ToLower();
            using (SqlLocalizationContext configContext = new SqlLocalizationContext(this.nameOrConnectionString))
            {
                LanguageResource resource = configContext.Resources.FirstOrDefault(x => x.Key == key && x.Code == code);

                if (resource != null)
                {
                    configContext.Resources.Remove(resource);
                }

                configContext.SaveChanges();
            }
        }
    }
}
