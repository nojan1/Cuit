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

        private int _width = -1;
        public int Width
        {
            get
            {
                return _width == -1 ? 4 + Items.Select(x => x.Value.ToString().Length).OrderByDescending(x => x).FirstOrDefault() 
                                    : _width;
            }
            set
            {
                _width = value;
                IsDirty = true;
            }
        }

        private int _height = -1;
        public int Height
        {
            get
            {
                return _height == -1 ? 2 + Items.Count : _height;
            }
            set
            {
                _height = value;
                IsDirty = true;
            }
        }

        private readonly List<ListItem<T>> _selected = new List<ListItem<T>>();
        public IEnumerable<ListItem<T>> Selected => _selected;

        public List<ListItem<T>> Items { get; private set; } = new List<ListItem<T>>();

        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };
        public event EventHandler<ListItem<T>> SelectionChanged = delegate { };

        private bool _displayMarker = false;
        private int _markerPosition = 0;
        private int _rowOffset = 0;

        public Listbox(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            buffer.DrawRectangle(RectangleDrawStyle.Dotted, Left, Top, Width, Height);

            for (int i = 0; i < ((_height == -1) ? Items.Count : _height - 2); i++)
            {
                
                buffer.DrawString(Left + 1,
                                  Top + i + 1,
                                  FixItemStringLength("  " + Items[i + _rowOffset].Value.ToString()),
                                  ConsoleColor.White,
                                  _selected.Contains(Items[i + _rowOffset]) ? ConsoleColor.DarkGray : Screenbuffer.DEFAULT_BACKGROUND);

                if (_displayMarker && i + _rowOffset == _markerPosition)
                {
                    buffer.SetChar(Left + 1, 
                                   Top + 1 + _markerPosition - _rowOffset, 
                                   '>', 
                                   ConsoleColor.Magenta,
                                   _selected.Contains(Items[i + _rowOffset]) ? ConsoleColor.DarkGray : Screenbuffer.DEFAULT_BACKGROUND);
                }
            }

            if(_rowOffset > 0)
            {
                buffer.SetChar(Left + Width - 1, Top + 1, '^', ConsoleColor.DarkGray, Screenbuffer.DEFAULT_BACKGROUND);
            }

            if(_height != -1 && Items.Count > _height - 2)
            {
                buffer.SetChar(Left + Width - 1, Top + Height - 2, 'v', ConsoleColor.DarkGray, Screenbuffer.DEFAULT_BACKGROUND);
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

                SyncRowOffset();
            }
            else if(key.Key == ConsoleKey.DownArrow)
            {
                if(++_markerPosition > Items.Count - 1)
                {
                    _markerPosition = 0;
                }

                SyncRowOffset();
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

        private string FixItemStringLength(string str)
        {
            if(_width != -1 && str.Length > _width - 3)
            {
                return str.Substring(0, _width - 3);
            }else
            {
                return str;
            }
        }

        private void SyncRowOffset()
        {
            if(_height == -1)
            {
                _rowOffset = 0;
            }
            else
            {
                int maxRows = _height - 2;
                if (Items.Count > maxRows)
                {
                    if(_markerPosition >= maxRows)
                    {
                        _rowOffset = Math.Max(0, _markerPosition - maxRows + 1);
                    }
                    else
                    {
                        _rowOffset = 0;
                    }
                }
                else
                {
                    _rowOffset = 0;
                }
            }
        }
    }
}
