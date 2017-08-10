using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cuit.Control
{
    public class Image : ControlBase
    {
        private readonly Dictionary<int, ConsoleColor> ColorMap = new Dictionary<int, ConsoleColor>
        {
            {0, ConsoleColor.Black },
            {1, ConsoleColor.DarkRed },
            {2, ConsoleColor.DarkGreen },
            {3, ConsoleColor.DarkYellow },
            {4, ConsoleColor.DarkBlue },
            {5, ConsoleColor.DarkMagenta },
            {6, ConsoleColor.DarkCyan },
            {7, ConsoleColor.Gray },
            {8, ConsoleColor.DarkGray },
            {9, ConsoleColor.Red },
            {10, ConsoleColor.Green },
            {11, ConsoleColor.Yellow },
            {12, ConsoleColor.Blue },
            {13, ConsoleColor.Magenta },
            {14, ConsoleColor.Cyan },
            {15, ConsoleColor.White }
        };

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

        private string _path;
        public string Path { set { SetImageFromFile(value); } get => _path; }

        private readonly List<BufferCharacter> _imageCharacters = new List<BufferCharacter>();

        public Image(int left, int top) : base(left, top)
        {
            Top = top;
            Left = left;
            IsDirty = true;
        }

        public void SetImageFromFile(string path, ConsoleColor? color = null)
        {
            _path = path;
            var content = File.ReadAllText(path);
            LoadContent(content, color);
        }

        public void SetImageFromContent(string content, ConsoleColor? color = null)
        {
            _path = "[memory]";
            LoadContent(content, color);
        }
        
        public override void Draw(Screenbuffer buffer)
        {
            buffer.StartTrackingForObject(this);

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

            buffer.CommitTrackingData();
        }

        private void LoadContent(string content, ConsoleColor? color)
        {
            _imageCharacters.Clear();
            var currentColor = color ?? Screenbuffer.DEFAULT_FOREGROUND;

            var lines = content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var colorModifierMatch = Regex.Match(lines[y].Substring(x), @"^\$\{c(\d+)\}");
                    if (colorModifierMatch.Success)
                    {
                        var colorIndex = Convert.ToInt32(colorModifierMatch.Groups[1].Value);
                        if (ColorMap.ContainsKey(colorIndex))
                        {
                            var newColor = ColorMap[colorIndex];
                            if (!color.HasValue)
                            {
                                currentColor = newColor;
                            }
                        }

                        //Remove
                        lines[y] = lines[y].Substring(colorModifierMatch.Value.Length);
                    }

                    if (!lines[y].Any())
                        break;

                    _imageCharacters.Add(new BufferCharacter
                    {
                        Left = x,
                        Top = y,
                        Character = lines[y][x],
                        Foreground = currentColor,
                        Background = Screenbuffer.DEFAULT_BACKGROUND
                    });
                }
            }

            IsDirty = true;
        }
    }
}
