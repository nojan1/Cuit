using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class Label : IControl
    {
        public bool IsDirty { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }

        public int Height => 1;
        public int Width => Text.Length;

        public ConsoleColor Foreground { get; set; } = Screenbuffer.DEFAULT_FOREGROUND;
        public ConsoleColor Background { get; set; } = Screenbuffer.DEFAULT_BACKGROUND;

        private string _text = "";
        public string Text { get { return _text; } set { _text = value; IsDirty = true; } }

        private int _lastRenderLength = 0;

        public Label(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            var stringToDraw = Text;
            if(Text.Length < _lastRenderLength)
            {
                stringToDraw = stringToDraw + string.Concat(Enumerable.Repeat(' ', _lastRenderLength - Text.Length));
            }

            buffer.DrawString(Left, Top, stringToDraw, Foreground, Background);

            _lastRenderLength = Text.Length;
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            //Nope
        }
    }
}
