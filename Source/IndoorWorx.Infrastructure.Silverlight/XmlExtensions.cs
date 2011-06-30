using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Collections.Generic;

namespace IndoorWorx.Infrastructure
{
    /// <summary>
    /// Defines a set of extensions method for the <seealso cref="XElement"/> and <seealso cref="XAttribute"/> classes.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Gets the value of the XElement.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <returns>The XElement value or null.</returns>
        public static string GetValue(this XElement element)
        {
            if (element != null)
            {
                return element.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the XAttribute.
        /// </summary>
        /// <param name="attribute">The XAttribute.</param>
        /// <returns>The XAttribute value or null.</returns>
        public static string GetValue(this XAttribute attribute)
        {
            if (attribute != null)
            {
                return attribute.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the XAttribute as long.
        /// </summary>
        /// <param name="attribute">The XAttribute.</param>
        /// <returns>The XAttribute value as long or null.</returns>
        public static long? GetValueAsLong(this XAttribute attribute)
        {
            long result;

            if (attribute != null && long.TryParse(attribute.GetValue(), out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the XAttribute as double.
        /// </summary>
        /// <param name="attribute">The XAttribute.</param>
        /// <returns>The XAttribute value as double or null.</returns>
        public static double? GetValueAsDouble(this XAttribute attribute)
        {
            double result;

            if (attribute != null && double.TryParse(attribute.GetValue(), out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the XAttribute as Guid.
        /// </summary>
        /// <param name="attribute">The XAttribute.</param>
        /// <returns>The XAttribute value as Guid or a emtpy Guid.</returns>
        public static Guid GetValueAsGuid(this XAttribute attribute)
        {
            try
            {
                return new Guid(attribute.GetValue());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the value of the XElement as Guid.
        /// </summary>
        /// <param name="element">The XElemenet.</param>
        /// <returns>The XElement value as Guid or a emtpy Guid.</returns>
        public static Guid GetValueAsGuid(this XElement element)
        {
            try
            {
                return new Guid(element.GetValue());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the value of the XElement as double.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <returns>The XElement value as double or null.</returns>
        public static double? GetValueAsDouble(this XElement element)
        {
            double result;

            if (element != null && double.TryParse(element.GetValue(), out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the XElement as int.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <returns>The XElement value as int or null.</returns>
        public static int? GetValueAsInt(this XElement element)
        {
            int result;

            if (element != null && int.TryParse(element.GetValue(), out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the XElement as Uri.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <returns>The XElement value as Uri or null.</returns>
        public static Uri GetValueAsUri(this XElement element)
        {
            return element.GetValueAsUri(UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Gets the value of the XElement as Uri.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <param name="uriKind">The UriKind used to try to create the Uri.</param>
        /// <returns>The XElement value as Uri or null.</returns>
        public static Uri GetValueAsUri(this XElement element, UriKind uriKind)
        {
            Uri uri;

            if (element != null && Uri.TryCreate(element.GetValue(), uriKind, out uri))
            {
                return uri;
            }

            return null;
        }

        /// <summary>
        /// Gets the value as a string list of the XElement.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <param name="separator">The separator list.</param>
        /// <returns>The list of values.</returns>
        public static IList<string> GetValueAsStringList(this XElement element, string separator)
        {
            List<string> list = new List<string>();

            if (element != null && !string.IsNullOrEmpty(element.Value))
            {
                string[] array = element.Value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

                list.AddRange(array);
            }

            return list;
        }

        /// <summary>
        /// Gets the value as a Uro list of the XElement.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <param name="separator">The separator list.</param>
        /// <returns>The list of values.</returns>
        public static IList<Uri> GetValueAsUriList(this XElement element, string separator)
        {
            List<Uri> list = new List<Uri>();

            if (element != null && !string.IsNullOrEmpty(element.Value))
            {
                string[] array = element.Value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string uriString in array)
                {
                    Uri uri;

                    if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                    {
                        list.Add(uri);
                    }
                }
            }

            return list;
        }
    }
}
