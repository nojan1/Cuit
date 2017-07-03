using Cuit.Control.Behaviors;
using System;
using Cuit.Helpers;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cuit.Control
{
    public class Button : IControl, IClickable, IFocusable
    {
        public int Top { get; private set; }
        public int Left { get; private set; }
        public int Width => Text.Length;
        public int Height => 2;

        public bool IsDirty { get;  set; }
        public string Text { get; set; }

        public event EventHandler Click = delegate { };
        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };

        public Button(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            buffer.DrawString(Left, Top, Text);
            buffer.DrawString(Left, Top + 1, String.Concat(Enumerable.Repeat('-', Text.Length)));

            IsDirty = false;
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                Click(this, new EventArgs());
            }
        }

        public void OnGotFocus()
        {
            GotFocus(this, new EventArgs());
        }

        public void OnLostFocus()
        {
            LostFocus(this, new EventArgs());
        }
    }
}
