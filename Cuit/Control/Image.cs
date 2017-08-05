using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class Image : ControlBase
    {
        public override int Height { get => 2 + (_imageCharacters.Any() ? _imageCharacters.Max(c => c.Top) + 1 : 1); }
        public override int Width { get => 2 + (_imageCharacters.Any() ? _imageCharacters.Max(c => c.Left) + 1 : 1); }

        private RectangleDrawStyle _border = RectangleDrawStyle.Clear;
        public RectangleDrawStyle Border
        {
            get => _border;
            set
            {
                _border = value;
                IsDirty = true;
            }
        }

        public ConsoleColor BorderColor { get; set; } = ConsoleColor.White;
        public string Path { set { SetImageFromFile(value); } }

        private List<BufferCharacter> _imageCharacters = new List<BufferCharacter>();

        public Image(int left, int top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void SetImageFromFile(string path)
        {
            //Read from file
            
        }

        public void SetImageFromContent(string content, ConsoleColor foreground)
        {
            _imageCharacters.Clear();

            var lines = content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int y = 0;y < lines.Length; y++){
                for (int x = 0; x < lines[y].Length; x++)
                {
                    _imageCharacters.Add(new BufferCharacter
                    {
                        Left = x,
                        Top = y,
                        Character = lines[y][x],
                        Foreground = foreground,
                        Background = Screenbuffer.DEFAULT_BACKGROUND
                    });
                }
            }

            IsDirty = true;
        }

        public override void Draw(Screenbuffer buffer)
        {
            buffer.DrawFill('\0', Left, Top, Width, Height);
            buffer.DrawRectangle(_border, Left, Top, Width, Height, BorderColor);

            foreach(var character in _imageCharacters)
            {
                buffer.SetChar(character.Left + Left + 1,
                               character.Top + Top + 1,
                               character.Character,
                               character.Foreground,
                               character.Background);
            }
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            //
        }
    }
}
