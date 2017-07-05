using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control
{
    public interface IControl
    {
        bool IsVisible { get; set; }
        int Top { get; }
        int Left { get; }
        int Width { get; }
        int Height { get; }
        bool IsDirty { get; set; }
        void Draw(Screenbuffer buffer);
        void HandleKeypress(ConsoleKeyInfo key);
    }
}
