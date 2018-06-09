using System;
using System.Collections.Generic;

namespace XmlToCode
{
    internal static class EnumerableExtension
    {
        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            if (list == null) throw new ArgumentNullException("null");
            if (action == null) throw new ArgumentNullException("action");

            for (int i = 0; i < list.Count; i++)
            {
                action(list[i]);
            }
        }
    }
}
