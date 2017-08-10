
using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class Checkbox : ControlBase, IFocusable, IValueChange<bool>
    {
        public bool IsEnabled { get; set; } = true;
      
        public override int Height => 1;
        public override int Width => 4 + Text.Length;

        private string _text = "";
        public string Text { get { return _text; } set { _text = value; IsDirty = true; } }

        private bool _checked;
        public bool Checked { get { return _checked; } set { _checked = value; IsDirty = true; } }

        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };
        public event EventHandler<bool> ValueChanged = delegate { };

        public Checkbox(int left, int top)
            : base(left, top)
        { }

        public override void Draw(Screenbuffer buffer)
        {
            buffer.StartTrackingForObject(this);

            var stringToRender = $"{(Checked ? "[x]" : "[ ]")} {Text}";
            buffer.DrawString(Left, Top, stringToRender);

            buffer.CommitTrackingData();
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
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
