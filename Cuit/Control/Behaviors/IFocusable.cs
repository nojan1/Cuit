using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control.Behaviors
{
    public interface IFocusable
    {
        bool IsEnabled { get; set; }
        event EventHandler GotFocus;
        event EventHandler LostFocus;
        void OnGotFocus();
        void OnLostFocus();
    }
}
