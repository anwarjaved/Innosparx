using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    using System.Web.UI;

    public static class WebExtensions
    {
        public static T FindControlByID<T>(this Control container, string id) where T : Control
        {
            return container.FindChildren<T>(c => c.ID.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static ICollection<T> FindChildren<T>(this Control element) where T : Control
        {
            return FindChildren<T>(element, null);
        }

        public static bool HasChildren<T>(this Control element, Func<T, bool> condition)
            where T : Control
        {
            return FindChildren(element, condition).Count != 0;
        }

        public static ICollection<T> FindChildren<T>(this Control element, Func<T, bool> condition)
            where T : Control
        {
            List<T> results = new List<T>();
            FindChildren(element, condition, results);
            return results;
        }


        private static void FindChildren<T>(Control element, Func<T, bool> condition, ICollection<T> results) where T : Control
        {
            if (element != null)
            {

                int childrenCount = element.Controls.Count;

                for (int i = 0; i < childrenCount; i++)
                {
                    Control child = element.Controls[i];

                    T t = child as T;
                    if (t != null)
                    {
                        if (condition == null) results.Add(t);
                        else if (condition(t)) results.Add(t);
                    }

                    FindChildren(child, condition, results);
                }
            }
        }
    }
}