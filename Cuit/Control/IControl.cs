using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control
{
    public interface IControl
    {
        bool IsDirty { get; set; }
        event EventHandler<bool> IsDirtyChanged;
        bool IsVisible { get; set; }
        int Top { get; }
        int Left { get; }
        int Width { get; }
        int Height { get; }
        void Draw(Screenbuffer buffer);
        void HandleKeypress(ConsoleKeyInfo key);
    }
}
