﻿using Cuit.Control.Behaviors;
using Cuit.Helpers;
using Cuit.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cuit.Control
{
    public class Listbox<T> : ControlBase, IFocusable, ISelectable<T>, IPreviewable<T>
    {
        public bool IsEnabled { get; set; } = true;

        private int _width = -1;
        public override int Width
        {
            get
            {
                return _width == -1 ? 4 + Items.Select(x => x.ToString().Length).OrderByDescending(x => x).FirstOrDefault()
                                    : _width;
            }
            set
            {
                _width = value;
                IsDirty = true;
            }
        }

        private int _height = -1;
        public override int Height
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

        private readonly List<T> _selected = new List<T>();
        public IEnumerable<T> Selected => _selected;

        public bool Multiselect { get; set; }
        public bool Autoselect { get; set; } = false;

        public ListItemCollection<T> Items { get; private set; } = new ListItemCollection<T>();

        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };
        public event EventHandler<T> SelectionChanged = delegate { };
        public event EventHandler<T> PreviewChanged = delegate { };

        private bool _displayMarker = false;
        private int _markerPosition = 0;
        private int _rowOffset = 0;

        public Listbox(int left, int top)
            : base(left, top)
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, EventArgs e)
        {
            if (_markerPosition > Items.Count - 1)
            {
                _markerPosition = Math.Max(0, Items.Count - 1);
                _rowOffset = 0;
            }

            _selected.RemoveAll(s => !Items.Contains(s));

            IsDirty = true;
        }

        public override void Draw(Screenbuffer buffer)
        {
            buffer.StartTrackingForObject(this);

            int numToDisplay = _height == -1 ? Items.Count
                                             : _height - 2;

            for(int i = 0; i < numToDisplay; i++)
            {
                if (i + _rowOffset <= Items.Count - 1)
                {
                    buffer.DrawString(Left + 1,
                                  Top + i + 1,
                                  FixItemStringLength("  " + Items[i + _rowOffset].ToString()),
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
                else
                {
                    break;
                }
            }

            buffer.DrawRectangle(RectangleDrawStyle.ShadedSingle, Left, Top, Width, Height);

            if (_rowOffset > 0)
            {
                buffer.SetChar(Left + Width - 1, Top + 1, '^', ConsoleColor.DarkGray, Screenbuffer.DEFAULT_BACKGROUND);
            }

            if (_height != -1 && Items.Count > _height - 2)
            {
                buffer.SetChar(Left + Width - 1, Top + Height - 2, 'v', ConsoleColor.DarkGray, Screenbuffer.DEFAULT_BACKGROUND);
            }

            buffer.CommitTrackingData();
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            if ((key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow) && Items.Any())
            {
                _markerPosition += key.Key == ConsoleKey.UpArrow ? -1 : 1;

                if (_markerPosition < 0)
                {
                    _markerPosition = Items.Count - 1;
                }
                else if (_markerPosition > Items.Count - 1)
                {
                    _markerPosition = 0;
                }

                if (Autoselect && !Multiselect)
                {
                    _selected.Clear();
                    _selected.Add(Items[_markerPosition]);

                    SelectionChanged(this, Items[_markerPosition]);
                }

                SyncRowOffset();
                PreviewChanged(this, Items[_markerPosition]);
            }
            else if (key.Key == ConsoleKey.Spacebar && !Autoselect)
            {
                var item = Items[_markerPosition];
                if (_selected.Contains(item))
                {
                    _selected.Remove(item);
                    SelectionChanged(this, default(T));
                }
                else
                {
                    if (Multiselect || !_selected.Any())
                    {
                        _selected.Add(item);
                        SelectionChanged(this, item);
                    }
                }
            }

            IsDirty = true;
        }

        public void OnGotFocus()
        {
            if (Autoselect && !Multiselect && Items.Any() && !_selected.Any())
            {
                _selected.Add(Items[0]);
                SelectionChanged(this, Items[0]);
            }

            if (Items.Any() && _markerPosition < Items.Count)
            {
                PreviewChanged(this, Items[_markerPosition]);
            }

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

        public void SetSelection(T item, bool isSelected)
        {
            if (_selected.Contains(item) && !isSelected)
            {
                _selected.Remove(item);
            }
            else if (!_selected.Contains(item) && isSelected)
            {
                _selected.Add(item);
            }

            IsDirty = true;
        }

        public void ClearSelection()
        {
            _selected.Clear();
            IsDirty = true;
        }

        private string FixItemStringLength(string str)
        {
            if (_width != -1 && str.Length > _width - 3)
            {
                return str.Substring(0, _width - 3);
            }
            else
            {
                var padding = string.Concat(Enumerable.Repeat(' ', _width - 3 - str.Length));
                return str + padding;
            }
        }

        private void SyncRowOffset()
        {
            if (_height == -1)
            {
                _rowOffset = 0;
            }
            else
            {
                int maxRows = _height - 2;
                if (Items.Count > maxRows)
                {
                    if (_markerPosition >= maxRows)
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
