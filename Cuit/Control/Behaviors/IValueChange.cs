using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control.Behaviors
{
    public interface IValueChange<T>
    {
        event EventHandler<T> ValueChanged;
    }
}
