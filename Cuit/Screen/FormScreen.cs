using Cuit.Control;
using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Screen
{
    public class FormScreen : IScreen, ILoaded
    {
        private int _tabIndex = 0;

        public event EventHandler Loaded = delegate { };

        public CuitApplication Application { get; set; }
        public List<IControl> Controls { get; private set; } = new List<IControl>();

        public IControl ActiveControl
        {
            get
            {
                return _tabIndex < Controls.Count ? Controls[_tabIndex] : null;
            }
        }

        public virtual void InstantiateComponents() { }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Tab)
            {
                HandleControlGotLostFocus(true);

                CycleControl(key.Modifiers.HasFlag(ConsoleModifiers.Shift) ? -1 : 1);

                HandleControlGotLostFocus(false);
            }

            ActiveControl?.HandleKeypress(key);
        }

        public void Update(Screenbuffer buffer, bool force)
        {
            foreach (var control in Controls.Where(c => force || c.IsDirty))
            {
                if (control.IsVisible)
                {
                    control.Draw(buffer);
                }
                else
                {
                    buffer.DrawFill(' ', control.Left, control.Top, control.Width, control.Height);
                }

                control.IsDirty = false;
            }
        }

        public void OnLoaded()
        {
            Loaded(this, new EventArgs());

            foreach (var loadableControl in Controls.OfType<ILoaded>())
            {
                loadableControl.OnLoaded();
            }
        }

        private void DrawFocusMarker(bool clear)
        {
            int top = ActiveControl.Top - 1;
            int left = ActiveControl.Left - 1;
            int width = ActiveControl.Width + 2;
            int height = ActiveControl.Height + 2;

            Application.Screenbuffer.DrawRectangle(clear ? RectangleDrawStyle.Clear : RectangleDrawStyle.Dotted,
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

            } while (KeepCycling(previousTabIndex));
        }

        private bool KeepCycling(int previousTabIndex)
        {
            if(_tabIndex == previousTabIndex)
            {
                return false;
            }

            if (!Controls[_tabIndex].IsVisible)
            {
                return true;
            }

            var focusable = Controls[_tabIndex] as IFocusable;
            if (focusable == null)
            {
                return true;
            }

            return !focusable.IsEnabled;
        }

        private void HandleControlGotLostFocus(bool lostFocus)
        {
            var focusableControl = ActiveControl as IFocusable;
            if (focusableControl != null)
            {
                DrawFocusMarker(lostFocus);

                if (lostFocus)
                {
                    focusableControl.OnLostFocus();
                }
                else
                {
                    focusableControl.OnGotFocus();
                }
            }
        }
    }
}
