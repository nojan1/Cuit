using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Helpers
{
    public enum RectangleDrawStyle
    {
        Clear,
        Single,
        Double,
        SlantedCornersOnly,
        Dotted,
        ShadedSingle
    }

    public static class ScreenbufferExtensions
    {
        public static void DrawString(this Screenbuffer buffer,
                                      int left,
                                      int top,
                                      IEnumerable<char> str,
                                      ConsoleColor foreground = Screenbuffer.DEFAULT_FOREGROUND,
                                      ConsoleColor background = Screenbuffer.DEFAULT_BACKGROUND)
        {
            int i = 0;
            foreach (var chr in str)
            {
                buffer.SetChar(left + i++, top, chr, foreground, background);
            }
        }

        //HorizontalUpper, HorizontalLower, VerticalLeft, VerticalRight, TopLeft, TopRight, BottomRight, BottomLeft
        private static readonly Dictionary<RectangleDrawStyle, char[]> _rectangleCharacters = new Dictionary<RectangleDrawStyle, char[]>
        {
            {RectangleDrawStyle.Clear, new char[] {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'} },
            {RectangleDrawStyle.Single, new char[] {(char)0x2500, (char)0x2500, (char)0x2502, (char)0x2502, (char)0x250c, (char)0x2510, (char)0x2518, (char)0x2514} },
            {RectangleDrawStyle.SlantedCornersOnly, new char[] { '\0', '\0', '\0', '\0', (char)0x2571, (char)0x2572, (char)0x2571, (char)0x2572 } },
            {RectangleDrawStyle.Double, new char[] { (char)0x2550, (char)0x2550, (char)0x2551, (char)0x2551, (char)0x2554, (char)0x2557, (char)0x255d, (char)0x255a } },
            {RectangleDrawStyle.Dotted, new char[] { (char)0x2509, (char)0x2509, (char)0x250b, (char)0x250b, (char)0x256d, (char)0x256e, (char)0x256f, (char)0x2570 } },
            {RectangleDrawStyle.ShadedSingle, new char[] { (char)0x2500, (char)0x2550, (char)0x2502, (char)0x2551, (char)0x250c, (char)0x2556, (char)0x255d, (char)0x2558 } },
        };
        public static void DrawRectangle(this Screenbuffer buffer,
                                         RectangleDrawStyle drawStyle,
                                         int left,
                                         int top,
                                         int width,
                                         int height,
                                         ConsoleColor foreground = Screenbuffer.DEFAULT_FOREGROUND,
                                         ConsoleColor background = Screenbuffer.DEFAULT_BACKGROUND)
        {
            for (int i = 0; i < width; i++)
            {
                buffer.SetChar(left + i, top, _rectangleCharacters[drawStyle][0], foreground, background);
                buffer.SetChar(left + i, top + height - 1, _rectangleCharacters[drawStyle][1], foreground, background);
            }

            for (int i = 0; i < height; i++)
            {
                buffer.SetChar(left, top + i, _rectangleCharacters[drawStyle][2], foreground, background);
                buffer.SetChar(left + width - 1, top + i, _rectangleCharacters[drawStyle][3], foreground, background);
            }

            buffer.SetChar(left, top, _rectangleCharacters[drawStyle][4], foreground, background);
            buffer.SetChar(left + width - 1, top, _rectangleCharacters[drawStyle][5], foreground, background);
            buffer.SetChar(left + width - 1, top + height - 1, _rectangleCharacters[drawStyle][6], foreground, background);
            buffer.SetChar(left, top + height - 1, _rectangleCharacters[drawStyle][7], foreground, background);
        }
    }
}
