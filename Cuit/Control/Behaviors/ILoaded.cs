using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control.Behaviors
{
    public interface ILoaded
    {
        event EventHandler Loaded;
        void OnLoaded();
    }
}
