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

        public Button(int top, int left)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            buffer.DrawString(Top, Left, Text);
            buffer.DrawString(Top + 1, Left, String.Concat(Enumerable.Repeat('-', Text.Length)));

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
