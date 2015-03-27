using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Windows;
    using System.Windows.Media;

    public static class DependencyObjectExtension
    {
        public static IReadOnlyList<TChild> FindChildren<TChild>(this DependencyObject d)
               where TChild : DependencyObject
        {
            List<TChild> children = new List<TChild>();

            int childCount = VisualTreeHelper.GetChildrenCount(d);

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject o = VisualTreeHelper.GetChild(d, i);

                if (o is TChild)
                    children.Add(o as TChild);

                children.AddRange(o.FindChildren<TChild>());
            }

            return children;
        }
    }
}
