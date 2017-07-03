using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control.Behaviors
{
    interface ISelectable<T>
    {
        event EventHandler<T> SelectionChanged;
        IEnumerable<T> Selected { get; }
    }
}
