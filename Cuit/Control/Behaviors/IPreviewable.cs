using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control.Behaviors
{
    public interface IPreviewable<T>
    {
        event EventHandler<T> PreviewChanged;
    }
}
