using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control.Behaviors
{
    public interface IFocusable
    {
        event EventHandler GotFocus;
        event EventHandler LostFocus;
        void OnGotFocus();
        void OnLostFocus();
    }
}
