using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control
{
    public class ControlBase : IControl
    {
        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    IsDirty = true;
                }
            }
        }

        public int Top { get; protected set; }

        public int Left { get; protected set; }

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public event EventHandler<bool> IsDirtyChanged = delegate { };
        private bool isDirty = true;
        public bool IsDirty
        {
            get => isDirty;
            set
            {
                if(value != isDirty)
                {
                    isDirty = value;
                    IsDirtyChanged(this, value);
                }
            }
        }

        public ControlBase(int left, int top)
        {
            Left = left;
            Top = top;
        }

        public virtual void Draw(Screenbuffer buffer)
        {

        }

        public virtual void HandleKeypress(ConsoleKeyInfo key)
        {

        }
    }
}
