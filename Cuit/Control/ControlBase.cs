using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Control
{
    public abstract class ControlBase : IControl
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

        public bool IsDirty { get; set; }

        public abstract void Draw(Screenbuffer buffer);

        public abstract void HandleKeypress(ConsoleKeyInfo key);
    }
}
