using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CM.Core
{
    public static class Extensions
    {
        #region LINQ extensions
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) 
        { 
            foreach (T item in enumeration) { action(item); }
        }

        public static IEnumerable<DataRow> AsEnumerableRows(this DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                yield return row;
            }
        }
        #endregion

        // Cast method for anonymous types - thanks to type inference when calling methods it 
        // is possible to cast object to type without knowing the type name
        public static T Cast<T>(this object obj, T type)
        {
            return (T)obj;
        }

        #region String extensions
        public static string FormatWith(this string format, params object[] args) 
        { 
            if (format == null)
                throw new ArgumentNullException("format"); 
            
            return string.Format(format, args); 
        } 
        
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args) 
        { 
            if (format == null)
                throw new ArgumentNullException("format"); 
            
            return string.Format(provider, format, args);
        }

        public static string SpaceOut(this string mixedOrCamelCase)
        {
            if (String.IsNullOrEmpty(mixedOrCamelCase)) return mixedOrCamelCase;
            string pattern = @"(?<![A-Z^])([A-Z])";
            return Regex.Replace(mixedOrCamelCase, pattern, " $1").Trim();
        }

        public static bool IsIn(this string searchingFor, params string[] listToSearchIn)
        {
            return listToSearchIn.Contains(searchingFor);
        }

        public static string ToTitleCase(this string source)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);
        }

        #endregion

        #region Value type extensions
        public static string GetOrdinal(this int i)
        {
            int remain = i % 10;
            string defaultSuffix = "th";
            string suffix = String.Empty;
            switch (remain)
            {
                case 1:
                    suffix = "st";
                    break;
                case 2:
                    suffix = "nd";
                    break;
                case 3:
                    suffix = "rd";
                    break;
                default:
                    suffix = defaultSuffix;
                    break;
            }

            /// Special cases
            switch (i % 100)
            {
                case 11:
                case 12:
                case 13:
                    suffix = defaultSuffix;
                    break;
            }

            return "{0}{1}".FormatWith(i, suffix);
        }

        public static int GetRandom(this int seed, int modValue)
        {
            return new Random(seed).Next() % modValue;
        }
        #endregion

        #region Enum extensions
        public static string GetEnumCode(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the enumcode attributes
            EnumCodeAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(EnumCodeAttribute), false) as EnumCodeAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Code : null;
        }
        
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
        #endregion
    }
}
