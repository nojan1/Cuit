using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cuit
{
    public class BufferCharacter
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public char Character { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; } 
        public bool IsDirty { get; set; }
    }

    public class Screenbuffer
    {
        public const ConsoleColor DEFAULT_FOREGROUND = ConsoleColor.Gray;
        public const ConsoleColor DEFAULT_BACKGROUND = ConsoleColor.Black;

        private readonly Dictionary<(int left, int top), BufferCharacter> _buffer = new Dictionary<(int left, int top), BufferCharacter>();

        public char this[int left, int top]
        {
            get
            {
                return Get(left, top).Character;
            }
            set
            {
                var character = Get(left, top);

                if (character.Character != value)
                {
                    character.Background = DEFAULT_BACKGROUND;
                    character.Foreground = DEFAULT_FOREGROUND;
                    character.Character = value;
                    character.IsDirty = true;
                }
            }
        }

        public void SetChar(int left, int top, char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            var charObject = Get(left, top);

            charObject.Character = character;
            charObject.Background = backgroundColor;
            charObject.Foreground = foregroundColor;
            charObject.IsDirty = true;
        }

        public ICollection<BufferCharacter> GetChangedCharacters(bool clear)
        {
            var dirty = _buffer.Values.Where(b => b.IsDirty).ToList();

            if (clear)
            {
                dirty.ForEach(b => b.IsDirty = false);
            }

            return dirty;
        }

        private BufferCharacter Get(int left, int top)
        {
            if (_buffer.ContainsKey((left, top)))
            {
                return _buffer[(left, top)];
            }
            else
            {
                var character = new BufferCharacter
                {
                    Top = top,
                    Left = left,
                    IsDirty = false
                };
                _buffer.Add((left, top), character);

                return character;
            }
        }

        internal void Invalidate()
        {
            _buffer.Clear();
        }
    }
}
