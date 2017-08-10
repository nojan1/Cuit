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

        private readonly Dictionary<object, List<(int left, int top)>> _previousChangesStore = new Dictionary<object, List<(int left, int top)>>();
        private object _currentTrackedObject;
        private readonly List<(int left, int top)> _changeTracking = new List<(int left, int top)>();

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

                    MarkChanged(left, top);
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

            MarkChanged(left, top);
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
        
        public void StartTrackingForObject(object obj)
        {
            _changeTracking.Clear();
            _currentTrackedObject = obj;
        }

        public void CommitTrackingData()
        {
            if (_previousChangesStore.ContainsKey(_currentTrackedObject))
            {
                //Blank out characters not touched by this render
                foreach(var ghostCharacter in _previousChangesStore[_currentTrackedObject].Where(x => !_changeTracking.Contains(x)))
                {
                    var charObject = Get(ghostCharacter.left, ghostCharacter.top);
                    charObject.Character = '\0';
                    charObject.IsDirty = true;
                }
            }

            _previousChangesStore[_currentTrackedObject] = new List<(int left, int top)>(_changeTracking);

            _currentTrackedObject = null;
            _changeTracking.Clear();
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

        private void MarkChanged(int left, int top)
        {
            if(_currentTrackedObject != null && !_changeTracking.Contains((left, top)))
            {
                _changeTracking.Add((left, top));
            }
        }

        internal void Invalidate()
        {
            _buffer.Clear();
        }
    }
}
