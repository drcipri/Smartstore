﻿using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Smartstore.Web.Rendering
{
    public static class TagBuilderExtensions
    {
        public static TagBuilder AppendCssClass(this TagBuilder builder, Func<string> cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass(), false);
            return builder;
        }

        public static TagBuilder PrependCssClass(this TagBuilder builder, Func<string> cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass(), true);
            return builder;
        }

        public static TagBuilder AppendCssClass(this TagBuilder builder, string cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass, false);
            return builder;
        }

        public static TagBuilder PrependCssClass(this TagBuilder builder, string cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass, true);
            return builder;
        }

        /// <summary>
        /// Creates a DOM-like CSS class list object. Call 'Dispose()' to flush
        /// the result back to <paramref name="builder"/>.
        /// </summary>
        public static CssClassList GetClassList(this TagBuilder builder)
        {
            return new CssClassList(builder.Attributes);
        }

        public static void AddCssStyle(this TagBuilder builder, string expression, object value)
        {
            Guard.NotEmpty(expression);
            Guard.NotNull(value);

            var style = expression + ": " + Convert.ToString(value, CultureInfo.InvariantCulture);

            if (builder.Attributes.TryGetValue("style", out var str))
            {
                builder.Attributes["style"] = style + "; " + str;
            }
            else
            {
                builder.Attributes["style"] = style;
            }
        }

        public static void AddCssStyles(this TagBuilder builder, string styles)
        {
            Guard.NotEmpty(styles);

            if (builder.Attributes.TryGetValue("style", out var str))
            {
                builder.Attributes["style"] = styles.EnsureEndsWith("; ") + str;
            }
            else
            {
                builder.Attributes["style"] = styles;
            }
        }

        public static bool MergeAttribute(this TagBuilder builder, string key, string value, bool replaceExisting, bool ignoreNull)
        {
            if (value == null && ignoreNull)
            {
                return false;
            }

            builder.MergeAttribute(key, value, replaceExisting);
            return true;
        }

        public static bool MergeAttribute(this TagBuilder builder, string key, Func<string> valueAccessor, bool replaceExisting, bool ignoreNull)
        {
            Guard.NotEmpty(key);
            Guard.NotNull(valueAccessor);

            if (replaceExisting || !builder.Attributes.ContainsKey(key))
            {
                var value = valueAccessor();
                if (value != null || !ignoreNull)
                {
                    builder.Attributes[key] = value;
                    return true;
                }
            }

            return false;
        }
    }
}
