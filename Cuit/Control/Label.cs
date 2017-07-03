using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control
{
    public class Label : IControl
    {
        public bool IsDirty { get; set; }
        public string Text { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }

        public int Height => 1;
        public int Width => Text.Length;

        public ConsoleColor Foreground { get; set; } = Screenbuffer.DEFAULT_FOREGROUND;
        public ConsoleColor Background { get; set; } = Screenbuffer.DEFAULT_BACKGROUND;

        public Label(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            buffer.DrawString(Left, Top, Text, Foreground, Background);
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            //Nope
        }
    }
}
