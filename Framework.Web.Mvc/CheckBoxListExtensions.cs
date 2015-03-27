namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Security;
    using System.Web;
    using System.Web.Mvc;

    using Framework.Web.Mvc.UI;

    /// <summary>
    /// CheckBox List Extensions.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>1/22/2011 3:44 PM</datetime>
    public static class CheckBoxListExtensions
    {
        /// <summary>
        /// Returns a checkbox list element using the specified HTML helper, the name of the form field, and the specified list items.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="name">The name of the form field to return.</param>
        /// <param name="checkBoxList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        [SecuritySafeCritical]
        public static IHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode)
        {
            return htmlHelper.CheckBoxListHelper(name, checkBoxList, renderMode, null);
        }

        /// <summary>
        /// Returns a checkbox list element using the specified HTML helper, the name of the form field, the specified list items, and the specified HMTL attributes.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="name">The name of the form field to return.</param>
        /// <param name="checkBoxList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list..
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        [SecuritySafeCritical]
        public static IHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.CheckBoxListHelper(name, checkBoxList, renderMode, htmlAttributes);
        }

        /// <summary>
        /// Returns a checkbox list element using the specified HTML helper, the name of the form field, and the specified list items.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="name">The name of the form field to return.</param>
        /// <param name="checkBoxList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An HTML select element with an option subelement for each item in the list..
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The <paramref name="name"/> parameter is null or empty.</exception>
        [SecuritySafeCritical]
        public static IHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode, object htmlAttributes)
        {
            return htmlHelper.CheckBoxListHelper(name, checkBoxList, renderMode, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Returns an HTML checkbox list for each property in the object that is represented by the specified expression and using the specified list items.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <param name="checkBoxList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        [SecuritySafeCritical]
        public static IHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode)
        {
            return htmlHelper.CheckBoxListHelper(ExpressionHelper.GetExpressionText(expression), checkBoxList, renderMode, null);
        }

        /// <summary>
        /// Returns an HTML checkbox list for each property in the object that is represented by the specified expression using the specified list items and HTML attributes.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <param name="checkBoxList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        [SecuritySafeCritical]
        public static IHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.CheckBoxListHelper(ExpressionHelper.GetExpressionText(expression), checkBoxList, renderMode, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML checkbox list element for each property in the object that is represented by the specified expression using the specified list items and HTML attributes.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <param name="checkBoxList">A collection of <see cref="T:System.Web.Mvc.SelectListItem"/> objects that are used to populate the drop-down list.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// An HTML select element for each property in the object that is represented by the expression.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="expression"/> parameter is null.</exception>
        [SecuritySafeCritical]
        public static IHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode, object htmlAttributes)
        {
            return htmlHelper.CheckBoxListHelper(ExpressionHelper.GetExpressionText(expression), checkBoxList, renderMode, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        [SecuritySafeCritical]
        private static IHtmlString CheckBoxListHelper(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListItem> checkBoxList, RenderMode renderMode, IDictionary<string, object> htmlAttributes)
        {
            ModelState modelState;
            string fieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            switch (renderMode)
            {
                case RenderMode.Flow:
                    var span = new TagBuilder("span");
                    span.GenerateId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(fieldName));
                    if (htmlAttributes != null)
                    {
                        span.MergeAttributes(htmlAttributes, true);
                    }

                    span.AddCssClass("checkbox-list");

                    foreach (var listItem in checkBoxList)
                    {
                        var childSpan = new TagBuilder("span");
                        childSpan.MergeAttribute("class", "item");
                        var label = new TagBuilder("label");
                        var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(fieldName + "_" + listItem.Text);

                        // Set the attributes
                        var checkbox = new TagBuilder("input");
                        checkbox.MergeAttribute("type", "checkbox");
                        checkbox.MergeAttribute("name", fieldName);
                        checkbox.MergeAttribute("value", listItem.Value);
                        checkbox.MergeAttribute("id", id);
                        if (listItem.Selected)
                        {
                            checkbox.MergeAttribute("checked", "checked");
                        }

                        // Render the tags
                        childSpan.InnerHtml = checkbox.ToString(TagRenderMode.SelfClosing);
                        label.SetInnerText(listItem.Text);
                        label.MergeAttribute("for", id);
                        childSpan.InnerHtml += label.ToString();

                        // Add to the span tag
                        span.InnerHtml += childSpan.ToString();
                    }

                    if (htmlHelper.ViewData.ModelState.TryGetValue(fieldName, out modelState) && modelState.Errors.Count > 0)
                    {
                        span.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    }

                    return span.ToHtmlString();

                default:
                    var ul = new TagBuilder("ul");
                    ul.GenerateId(fieldName);

                    if (htmlAttributes != null)
                    {
                        ul.MergeAttributes(htmlAttributes, true);
                    }

                    ul.AddCssClass("checkbox-list");

                    foreach (var listItem in checkBoxList)
                    {
                        var li = new TagBuilder("li");
                        li.MergeAttribute("class", "item");
                        var label = new TagBuilder("label");
                        var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(fieldName + "_" + listItem.Text);

                        // Set the attributes
                        var checkbox = new TagBuilder("input");
                        checkbox.MergeAttribute("type", "checkbox");
                        checkbox.MergeAttribute("name", fieldName);
                        checkbox.MergeAttribute("value", listItem.Value);
                        checkbox.MergeAttribute("id", id);
                        if (listItem.Selected)
                        {
                            checkbox.MergeAttribute("checked", "checked");
                        }

                        // Render the tags
                        li.InnerHtml = checkbox.ToString(TagRenderMode.SelfClosing);
                        label.SetInnerText(listItem.Text);
                        label.MergeAttribute("for", id);
                        li.InnerHtml += label.ToString();

                        // Add to the ul tag
                        ul.InnerHtml += li.ToString();
                    }

                    // Add validation higlighting if necessary
                    if (htmlHelper.ViewData.ModelState.TryGetValue(fieldName, out modelState) && modelState.Errors.Count > 0)
                    {
                        ul.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    }

                    return ul.ToHtmlString();
            }
        }
    }
}
