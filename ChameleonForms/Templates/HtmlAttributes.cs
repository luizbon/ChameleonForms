﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// Represents a set of HTML attributes.
    /// </summary>
    public class HtmlAttributes : IHtmlString
    {
        private readonly TagBuilder _tagBuilder = new TagBuilder("p");

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using lambda methods to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(style => "width: 100%;", cellpadding => 0, @class => "class1 class2", src => "http://url/", data_somedata => "\"rubbi&amp;h\"");
        /// </example>
        /// <param name="attributes">A list of lambas where the lambda variable name is the name of the attribute and the value is the value</param>
        public HtmlAttributes(params Func<object, object>[] attributes)
        {
            Attrs(attributes);
        }

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using a dictionary to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new Dictionary&lt;string, object&gt; {{"style", "width: 100%;"}, {"cellpadding", 0}, {"class", "class1 class2"}, {"src", "http://url/"}, {"data-somedata", "\"rubbi&amp;h\""}});
        /// </example>
        /// <param name="attributes">A dictionary of attributes</param>
        public HtmlAttributes(IDictionary<string, object> attributes)
        {
            Attrs(attributes);
        }

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using an anonymous object to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new { style = "width: 100%;", cellpadding = 0, @class = "class1 class2", src = "http://url/", data_somedata = "\"rubbi&amp;h\"" });
        /// </example>
        /// <param name="attributes">An anonymous object of attributes</param>
        public HtmlAttributes(object attributes)
        {
            Attrs(attributes);
        }

        /// <summary>
        /// Adds a CSS class (or a number of CSS classes) to the attributes.
        /// </summary>
        /// <param name="class">The CSS class(es) to add</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes AddClass(string @class)
        {
            _tagBuilder.AddCssClass(@class);

            return this;
        }

        /// <summary>
        /// Adds or updates a HTML attribute with a given value.
        /// </summary>
        /// <param name="key">The name of the HTML attribute to add/update</param>
        /// <param name="value">The value of the HTML attribute to add/update</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attr(string key, object value)
        {
            _tagBuilder.MergeAttribute(key, value.ToString(), true);

            return this;
        }

        /// <summary>
        /// Adds or updates a HTML attribute with using a lambda method to express the attribute.
        /// </summary>
        /// <example>
        /// h.Attr(style => "width: 100%;")
        /// </example>
        /// <param name="attribute">A lambda expression representing the attribute to set and its value</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attr(Func<object, object> attribute)
        {
            var item = attribute(null);
            _tagBuilder.MergeAttribute(attribute.Method.GetParameters()[0].Name.Replace("_", "-"), item.ToString(), true);

            return this;
        }

        /// <summary>
        /// Adds or updates a set of HTML attributes using lambda methods to express the attributes.
        /// </summary>
        /// <param name="attributes">A list of lambas where the lambda variable name is the name of the attribute and the value is the value</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(params Func<object, object>[] attributes)
        {
            foreach (var func in attributes)
                Attr(func);

            return this;
        }

        /// <summary>
        /// Adds or updates a set of HTML attributes using a dictionary to express the attributes.
        /// </summary>
        /// <param name="attributes">A dictionary of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(IDictionary<string, object> attributes)
        {
            _tagBuilder.MergeAttributes(attributes, true);

            return this;
        }

        /// <summary>
        /// Adds or updates a set of HTML attributes using anonymous objects to express the attributes.
        /// </summary>
        /// <param name="attributes">An anonymous object of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(object attributes)
        {
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            _tagBuilder.MergeAttributes(attrs, true);

            return this;
        }

        /// <summary>
        /// Implicitly convert from a dictionary to a new <see cref="HtmlAttributes"/> object.
        /// </summary>
        /// <param name="attributes">The dictionary of HTML attributes</param>
        /// <returns>The new <see cref="HtmlAttributes"/> object</returns>
        public static implicit operator HtmlAttributes(Dictionary<string, object> attributes)
        {
            return new HtmlAttributes(attributes);
        }

        public string ToHtmlString()
        {
            var sb = new StringBuilder();
            foreach (var attr in _tagBuilder.Attributes)
            {
                sb.Append(string.Format(" {0}=\"{1}\"",
                    HttpUtility.HtmlEncode(attr.Key),
                    HttpUtility.HtmlEncode(attr.Value))
                );
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="HtmlAttributes"/> class.
    /// </summary>
    public static class HtmlAttributesExtensions
    {
        /// <summary>
        /// Explicitly convert a dictionary to a <see cref="HtmlAttributes"/> class.
        /// </summary>
        /// <param name="htmlAttributes">A dictionary of HTML attributes</param>
        /// <returns>A new <see cref="HtmlAttributes"/> with the attributes</returns>
        public static HtmlAttributes ToHtmlAttributes(this IDictionary<string, object> htmlAttributes)
        {
            return new HtmlAttributes(htmlAttributes);
        }

        /// <summary>
        /// Convert from an anonymous object to a <see cref="HtmlAttributes"/> class.
        /// </summary>
        /// <param name="htmlAttributes">An anonymous object of HTML attributes</param>
        /// <returns>A new <see cref="HtmlAttributes"/> with the attributes</returns>
        public static HtmlAttributes ToHtmlAttributes(this object htmlAttributes)
        {
            return new HtmlAttributes(htmlAttributes);
        }
    }
}
