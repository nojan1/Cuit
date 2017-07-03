using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cuit.Control
{
    public class Textbox : IControl, IFocusable
    {
        private const int TEXT_MIN_WIDTH = 30;

        public bool IsDirty { get; set; }
        public int Top { get; private set; }
        public int Left { get; private set; }
        public int Width => 2 + (_stringBuilder.Length > TEXT_MIN_WIDTH ? _stringBuilder.Length : TEXT_MIN_WIDTH);
        public int Height => 3;

        public string Text
        {
            get
            {
                return _stringBuilder.ToString();
            }
            set
            {
                _stringBuilder = new StringBuilder(value);
                _cursorPosition = _stringBuilder.Length - 1;

                IsDirty = true;
            }
        }

        private StringBuilder _stringBuilder = new StringBuilder();
        private int _cursorPosition = 0;
        private int _lastRenderTextLength = 0;

        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };

        public Textbox(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void Draw(Screenbuffer buffer)
        {
            buffer.DrawRectangle(RectangleDrawStyle.Single, Left, Top, Width, Height);

            //TODO: Send chars directly from string build to avoid memory allocation
            if (_lastRenderTextLength > _stringBuilder.Length)
            {
                buffer.DrawString(Left + 1, 
                                  Top + 1, 
                                  Text + string.Concat(Enumerable.Repeat(' ', _lastRenderTextLength - _stringBuilder.Length)), 
                                  ConsoleColor.White, 
                                  ConsoleColor.Black);
            }
            else
            {
                buffer.DrawString(Left + 1, Top + 1, Text, ConsoleColor.White, ConsoleColor.Black);
            }

            _lastRenderTextLength = _stringBuilder.Length;
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.LeftArrow)
            {
                if (--_cursorPosition < 0)
                    _cursorPosition = 0;
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (++_cursorPosition > _stringBuilder.Length)
                    _cursorPosition = _stringBuilder.Length;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (_stringBuilder.Length > 0)
                {
                    _stringBuilder.Remove(_cursorPosition - 1, 1);

                    if (--_cursorPosition < 0)
                        _cursorPosition = 0;

                    IsDirty = true;
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                _stringBuilder.Insert(_cursorPosition, key.KeyChar);
                _cursorPosition++;

                IsDirty = true;
            }

            Console.SetCursorPosition(Left + 1 + _cursorPosition, Top + 1);
        }

        public void OnGotFocus()
        {
            Console.SetCursorPosition(Left + 1, Top + 1);
            Console.CursorVisible = true;

            GotFocus(this, new EventArgs());
        }

        public void OnLostFocus()
        {
            Console.CursorVisible = false;

            LostFocus(this, new EventArgs());
        }
    }
}
