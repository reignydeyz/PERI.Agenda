using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PERI.Agenda.Core
{
    public static class ListExtension
    {
        /// <summary>
        /// Checker for property type
        /// Will only allow these datatypes
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static bool IsAllowedPropertyInfo(PropertyInfo prop)
        {
            return prop.PropertyType == typeof(String)
                    || prop.PropertyType == typeof(int)
                    || prop.PropertyType == typeof(int?)
                    || prop.PropertyType == typeof(long)
                    || prop.PropertyType == typeof(long?)
                    || prop.PropertyType == typeof(decimal)
                    || prop.PropertyType == typeof(decimal?)
                    || prop.PropertyType == typeof(double)
                    || prop.PropertyType == typeof(double?)
                    || prop.PropertyType == (typeof(bool))
                    || prop.PropertyType == (typeof(bool?))
                    || prop.PropertyType == (typeof(DateTime))
                    || prop.PropertyType == (typeof(DateTime?))
                    || prop.PropertyType == (typeof(DateTimeOffset))
                    || prop.PropertyType == (typeof(DateTimeOffset?));
        }

        /// <summary>
        /// Gets the CSV as StringBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder ExportToCsv<T>(this List<T> list)
        {
            StringBuilder fileContent = new StringBuilder();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!IsAllowedPropertyInfo(prop))
                    continue;

                fileContent.Append(prop.Name + ",");
            }

            fileContent.Append("\r\n");

            foreach (var rec in list)
            {
                foreach (var field in rec.GetType().GetProperties())
                {
                    if (!IsAllowedPropertyInfo(field))
                        continue;

                    fileContent.Append((field.GetValue(rec, null) ?? "").ToString()
                        .Replace(',', ' ')
                        .Replace(Environment.NewLine, " ")
                        + ",");
                }

                fileContent.Append("\r\n");
            }

            return fileContent;
        }
    }
}
