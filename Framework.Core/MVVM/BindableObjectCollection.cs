using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MVVM
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public sealed class BindableObjectCollection<T>: ObservableCollection<T> where T : BindableObject
    {
     public BindableObjectCollection()
        {
            this.CollectionChanged += this.OnCollectionChanged;

        }

     public BindableObjectCollection(IEnumerable<T> list)
            : this()
        {
            foreach (T item in list)
            {
                this.Add(item);
                item.PropertyChanged += this.OnPropertyChanged;
            }
        }


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e != null)
            {
                if (e.OldItems != null)
                    foreach (INotifyPropertyChanged item in e.OldItems)
                    {
                        item.PropertyChanged -= this.OnPropertyChanged;
                    }

                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged item in e.NewItems)
                    {
                        item.PropertyChanged += this.OnPropertyChanged;
                    }
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            this.OnCollectionChanged(reset);
        }
    }
}
