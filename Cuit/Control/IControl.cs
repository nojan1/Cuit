using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control
{
    public interface IControl
    {
        int Top { get; }
        int Left { get; }
        int Width { get; }
        int Height { get; }
        bool IsDirty { get; }
        void Draw(Screenbuffer buffer);
        void HandleKeypress(ConsoleKeyInfo key);
    }
}
