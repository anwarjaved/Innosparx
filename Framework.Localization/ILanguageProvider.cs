using System.Collections.Generic;

namespace Framework.Localization
{
    using System.Security;

    using Framework.Paging;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for language provider.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/19/2014 11:26 AM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public interface ILanguageProvider
    {
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
        IPagedList<ILanguageResource> Get(int page = 0, int size = 10);

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
        IPagedList<ILanguageResource> Get(string sortBy, string sortDirection, int page = 0, int size = 10);

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
        IPagedList<ILanguageResource> GetByCategory(string category, int page = 0, int size = 10);

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
        IPagedList<ILanguageResource> GetByCategory(string category, string sortBy, string sortDirection, int page = 0, int size = 10);


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
        IPagedList<ILanguageResource> GetByCategory(string category, string text, int page = 0, int size = 10);

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
        IPagedList<ILanguageResource> GetByCategory(string category, string text, string sortBy, string sortDirection, int page = 0, int size = 10);


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
        IReadOnlyList<ILanguageResource> Get(string key);

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
        ILanguageResource Get(string key, LanguageCode code);

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
        IPagedList<ILanguageResource> Get(LanguageCode code, int page = 0, int size = 10);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves a resource.
        /// </summary>
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
        void SaveResource(
            string key,
            string text,
            LanguageCode code = LanguageCode.English,
            string tooltipText = null,
            bool? canShowTooltip = null, string category = null);

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
        void RemoveResource(string key, LanguageCode code = LanguageCode.English);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Searches for the first match.
        /// </summary>
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
        IPagedList<ILanguageResource> Find(string text, int page = 0, int size = 10);

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
        IPagedList<ILanguageResource> Find(string text, string sortBy, string sortDirection, int page = 0, int size = 10);

    }
}
