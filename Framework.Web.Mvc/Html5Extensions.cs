using System;
using System.Collections.Generic;

namespace Framework
{
    using System.Linq.Expressions;
    using System.Security;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     HTML 5 input extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public static class Html5Extensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A HtmlHelper extension method that HTML 5 text box.
        /// </summary>
        ///
        /// <param name="htmlHelper">
        ///     The htmlHelper to act on.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="inputType">
        ///     (optional) type of the input.
        /// </param>
        /// <param name="placeHolder">
        ///     (optional) the place holder.
        /// </param>
        /// <param name="required">
        ///     (optional) the required.
        /// </param>
        /// <param name="readonly">
        ///     (optional) the is read only.
        /// </param>
        /// <param name="disabled">
        ///     (optional) the disabled.
        /// </param>
        /// <param name="autoFocus">
        ///     (optional) the automatic focus.
        /// </param>
        /// <param name="autocomplete">
        ///     (optional) the autocomplete.
        /// </param>
        /// <param name="htmlAttributes">
        ///     (optional) the HTML attributes.
        /// </param>
        ///
        /// <returns>
        ///     An input element whose type attribute is set to <see cref="Html5InputType"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static IHtmlString Html5TextBox(this HtmlHelper htmlHelper, string name, object value = null, Html5InputType inputType = Html5InputType.Text, string placeHolder = null, bool required = false, bool @readonly = false, bool disabled = false, bool autoFocus = false,
            bool autocomplete = true,
            object htmlAttributes = null)
        {
            IDictionary<string, object> dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            BuildHtmlAttributes(dictionary, inputType, placeHolder, required, @readonly, disabled, autoFocus, autocomplete);
            return htmlHelper.TextBox(name, value, dictionary);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A HtmlHelper&lt;TModel&gt; extension method that HTML 5 text box for.
        /// </summary>
        ///
        /// <typeparam name="TModel">
        ///     Type of the model.
        /// </typeparam>
        /// <typeparam name="TProperty">
        ///     Type of the property.
        /// </typeparam>
        /// <param name="htmlHelper">
        ///     The htmlHelper to act on.
        /// </param>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="inputType">
        ///     (optional) type of the input.
        /// </param>
        /// <param name="placeHolder">
        ///     (optional) the place holder.
        /// </param>
        /// <param name="required">
        ///     (optional) the required.
        /// </param>
        /// <param name="readonly">
        ///     (optional) the is read only.
        /// </param>
        /// <param name="disabled">
        ///     (optional) the disabled.
        /// </param>
        /// <param name="autoFocus">
        ///     (optional) the automatic focus.
        /// </param>
        /// <param name="autocomplete">
        ///     (optional) the autocomplete.
        /// </param>
        /// <param name="htmlAttributes">
        ///     (optional) the HTML attributes.
        /// </param>
        ///
        /// <returns>
        ///     An input element whose type attribute is set to <see cref="Html5InputType"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static IHtmlString Html5TextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            Html5InputType inputType = Html5InputType.Text,
            string placeHolder = null,
            bool required = false,
            bool @readonly = false,
            bool disabled = false,
            bool autoFocus = false,
            bool autocomplete = true,
            object htmlAttributes = null)
        {
            IDictionary<string, object> dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            BuildHtmlAttributes(dictionary, inputType, placeHolder, required, @readonly, disabled, autoFocus, autocomplete);
            return htmlHelper.TextBoxFor(expression, dictionary);
        }

        private static void BuildHtmlAttributes(IDictionary<string, object> dictionary, Html5InputType inputType, string placeHolder,
            bool required,
            bool @readonly,
            bool disabled,
            bool autoFocus,
            bool autocomplete)
        {
            dictionary["type"] = inputType.ToString();

            if (!string.IsNullOrWhiteSpace(placeHolder))
            {
                dictionary["placeHolder"] = placeHolder;
            }

            if (required)
            {
                dictionary["required"] = "required";
                dictionary["aria-required"] = "true";
            }

            if (@readonly)
            {
                dictionary["readonly"] = "readonly";
            }

            if (disabled)
            {
                dictionary["disabled"] = "disabled";
            }

            if (autoFocus)
            {
                dictionary["autoFocus"] = "autoFocus";
            }

            if (!autocomplete)
            {
                dictionary["autoFocus"] = "off";
            }
        }
    }
}
