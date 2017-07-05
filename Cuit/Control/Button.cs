using Cuit.Control.Behaviors;
using System;
using Cuit.Helpers;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cuit.Control
{
    public class Button : ControlBase, IClickable, IFocusable
    {
        public bool IsEnabled { get; set; } = true;
        public override int Width => Text.Length + 2;
        public override int Height => 3;

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

        public override void Draw(Screenbuffer buffer)
        {
            buffer.DrawRectangle(RectangleDrawStyle.Single, Left, Top, Width, Height);
            buffer.DrawString(Left + 1, Top + 1, Text);
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
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
