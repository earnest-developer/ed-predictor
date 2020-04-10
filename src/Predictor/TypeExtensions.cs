using System;
using System.Collections.Generic;
using System.Linq;

namespace Predictor
{
    public static class TypeExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            string str = type.Name;

            if (type.IsGenericType)
            {
                int startIndex = str.IndexOf('`');

                if (startIndex > 0)
                    str = str.Remove(startIndex);

                IEnumerable<string> values = type.GetGenericArguments().Select(p => p.GetDisplayName());
                str = str + "<" + string.Join(", ", values) + ">";
            }

            return str;
        }
    }
}