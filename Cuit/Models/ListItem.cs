using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Models
{
    public class ListItem<T>
    {
        public ListItem(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
