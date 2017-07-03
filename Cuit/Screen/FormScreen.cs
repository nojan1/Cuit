using Cuit.Control;
using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Screen
{
    public class FormScreen : IScreen
    {
        private int _tabIndex = 0;

        public CuitApplication Application { get; set; }
        public List<IControl> Controls { get; private set; } = new List<IControl>();

        public IControl ActiveControl
        {
            get
            {
                return _tabIndex < Controls.Count ? Controls[_tabIndex] : null;
            }
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Tab)
            {
                if (ActiveControl is IFocusable)
                {
                    DrawFocusMarker(true);
                    ((IFocusable)ActiveControl).OnLostFocus();
                }

                CycleControl(key.Modifiers.HasFlag(ConsoleModifiers.Shift) ? -1 : 1);

                if (ActiveControl is IFocusable)
                {
                    DrawFocusMarker(false);
                    ((IFocusable)ActiveControl).OnGotFocus();
                }
            }

            ActiveControl?.HandleKeypress(key);
        }

        public void Update(Screenbuffer buffer, bool force)
        {
            foreach (var control in Controls.Where(c => force || c.IsDirty))
            {
                control.Draw(buffer);
                control.IsDirty = false;
            }
        }

        private void DrawFocusMarker(bool clear)
        {
            int top = ActiveControl.Top - 1;
            int left = ActiveControl.Left - 1;
            int width = ActiveControl.Width + 2;
            int height = ActiveControl.Height + 2;

            Application.Screenbuffer.DrawRectangle(clear ? RectangleDrawStyle.Clear : RectangleDrawStyle.Single, 
                                                   left, 
                                                   top, 
                                                   width, 
                                                   height,
                                                   ConsoleColor.Cyan);
        }

        private void CycleControl(int direction)
        {
            int previousTabIndex = _tabIndex;

            do
            {
                _tabIndex += direction;

                if (_tabIndex >= Controls.Count)
                    _tabIndex = 0;
                else if (_tabIndex < 0)
                    _tabIndex = Controls.Count - 1;

            } while (!(Controls[_tabIndex] is IFocusable) && _tabIndex != previousTabIndex);
        }
    }
}
