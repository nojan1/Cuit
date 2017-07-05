using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Models
{
    public class ListItemCollection<T> : IList<T>
    {
        internal event EventHandler CollectionChanged = delegate { };

        private readonly List<ListItem<T>> _list = new List<ListItem<T>>();

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public T this[int index] {
            get => _list[index].Value;
            set => _list[index].Value = value;
        }

        public void Add(T item)
        {
            _list.Add(new ListItem<T>(item));
            CollectionChanged(this, new EventArgs());
        }

        public void AddRange(IEnumerable<T> items)
        {
            _list.AddRange(items.Select(i => new ListItem<T>(i)));
            CollectionChanged(this, new EventArgs());
        }

        public void Clear()
        {
            _list.Clear();
            CollectionChanged(this, new EventArgs());
        }

        public bool Contains(T item)
        {
            return _list.Any(x => x.Value.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array.Select(x => new ListItem<T>(x)).ToArray(), arrayIndex);
            CollectionChanged(this, new EventArgs());
        }

        public bool Remove(T item)
        {
            var listItem = _list.FirstOrDefault(x => x.Value.Equals(item));
            if (listItem == null)
                return false;

            var retVal = _list.Remove(listItem);
            if(retVal)
                CollectionChanged(this, new EventArgs());

            return retVal;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _list.Select(x => x.Value).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _list.Select(x => x.Value).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _list.FindIndex(x => x.Value.Equals(item));
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, new ListItem<T>(item));
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
    }
}
