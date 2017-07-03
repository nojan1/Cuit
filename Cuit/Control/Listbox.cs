using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cuit.Control
{
    public class ListItem<T>
    {
        public ListItem(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }

    public class Listbox<T> : IControl, IFocusable, ISelectable<ListItem<T>>
    {
        public bool IsDirty { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }
        public int Width => 4 + Items.Select(x => x.Value.ToString().Length).OrderByDescending(x => x).FirstOrDefault();
        public int Height => 2 + Items.Count;

        private readonly List<ListItem<T>> _selected = new List<ListItem<T>>();
        public IEnumerable<ListItem<T>> Selected => _selected;

        public List<ListItem<T>> Items { get; private set; } = new List<ListItem<T>>();

        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };
        public event EventHandler<ListItem<T>> SelectionChanged = delegate { };

        private bool _displayMarker = false;
        private int _markerPosition = 0;

        public Listbox(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            buffer.DrawRectangle(RectangleDrawStyle.Dotted, Left, Top, Width, Height);

            for (int i = 0; i < Items.Count; i++)
            {
                buffer.DrawString(Left + 1,
                                  Top + i + 1,
                                  "  " + Items[i].Value.ToString(),
                                  ConsoleColor.White,
                                  _selected.Contains(Items[i]) ? ConsoleColor.DarkGray : Screenbuffer.DEFAULT_BACKGROUND);

                if (_displayMarker && i == _markerPosition)
                {
                    buffer.SetChar(Left + 1, 
                                   Top + _markerPosition + 1, 
                                   '>', 
                                   ConsoleColor.Blue,
                                   _selected.Contains(Items[i]) ? ConsoleColor.DarkGray : Screenbuffer.DEFAULT_BACKGROUND);
                }
            }
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.UpArrow)
            {
                if(--_markerPosition < 0)
                {
                    _markerPosition = Items.Count - 1;
                }
            }
            else if(key.Key == ConsoleKey.DownArrow)
            {
                if(++_markerPosition > Items.Count - 1)
                {
                    _markerPosition = 0;
                }
            }
            else if(key.Key == ConsoleKey.Spacebar)
            {
                var item = Items[_markerPosition];
                if (_selected.Contains(item))
                {
                    _selected.Remove(item);
                }
                else
                {
                    _selected.Add(item);
                }
            }

            IsDirty = true;
        }

        public void OnGotFocus()
        {
            _displayMarker = true;
            IsDirty = true;

            GotFocus(this, new EventArgs());
        }

        public void OnLostFocus()
        {
            _displayMarker = false;
            IsDirty = true;

            LostFocus(this, new EventArgs());
        }
    }
}
