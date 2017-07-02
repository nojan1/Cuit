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
        public bool IsDirty { get; set; }
    }

    public class Screenbuffer
    {
        private List<BufferCharacter> _buffer = new List<BufferCharacter>();

        public char this[int top, int left]
        {
            get
            {
                return Get(top, left).Character;
            }
            set
            {
                var character = Get(top, left);
                character.Character = value;
                character.IsDirty = true;
            }
        }

        public ICollection<BufferCharacter> GetChangedCharacters(bool clear)
        {
            var dirty = _buffer.Where(b => b.IsDirty).ToList();

            if (clear)
            {
                dirty.ForEach(b => b.IsDirty = false);
            }

            return dirty;
        }

        private BufferCharacter Get(int top, int left)
        {
            var character = _buffer.FirstOrDefault(c => c.Top == top && c.Left == left);
            if(character == null)
            {
                character = new BufferCharacter
                {
                    Top = top,
                    Left = left,
                    IsDirty = false
                };
                _buffer.Add(character);
            }

            return character;
        }
    }
}
