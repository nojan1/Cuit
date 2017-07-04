
using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class Checkbox : IControl, IFocusable, IValueChange<bool>
    {
        public bool IsDirty { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }

        public int Height => 1;
        public int Width => 4 + Text.Length;

        private string _text = "";
        public string Text { get { return _text; } set { _text = value; IsDirty = true; } }

        private bool _checked;
        public bool Checked { get { return _checked; } set { _checked = value; IsDirty = true; } }

        private int _lastRenderLength = 0;

        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };
        public event EventHandler<bool> ValueChanged = delegate { };

        public Checkbox(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            var stringToRender = $"{(Checked ? "[x]" : "[ ]")} {Text}";
            if(stringToRender.Length < _lastRenderLength)
            {
                stringToRender += string.Concat(Enumerable.Repeat(' ', _lastRenderLength - stringToRender.Length));
            }

            _lastRenderLength = Width;

            buffer.DrawString(Left, Top, stringToRender);
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Spacebar)
            {
                Checked = !Checked;
                ValueChanged(this, Checked);
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
