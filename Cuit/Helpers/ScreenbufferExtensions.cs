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
        Dotted
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
            foreach(var chr in str)
            {
                buffer.SetChar(left + i++, top, chr, foreground, background);
            }
        }

        private static readonly Dictionary<RectangleDrawStyle, char[]> _rectangleCharacters = new Dictionary<RectangleDrawStyle, char[]>
        {
            {RectangleDrawStyle.Clear, new char[] {'\0', '\0', '\0', '\0', '\0', '\0'} },
            //{RectangleDrawStyle.Single, new char[] {(char)196, (char)179, (char)218, (char)191, (char)217, (char)192} },
            {RectangleDrawStyle.Single, new char[] { '-', '|', '#', '#', '#', '#' } },
            {RectangleDrawStyle.SlantedCornersOnly, new char[] { '\0', '\0', '/', '\\', '/', '\\' } },
            {RectangleDrawStyle.Double, new char[] {'\0', '\0', '\0', '\0', '\0', '\0'} },
            {RectangleDrawStyle.Dotted, new char[] {'.', '.', '.', '.', '.', '.'} },
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
            for(int i = 0; i < width; i++)
            {
                buffer.SetChar(left + i, top, _rectangleCharacters[drawStyle][0], foreground, background);
                buffer.SetChar(left + i, top + height - 1, _rectangleCharacters[drawStyle][0], foreground, background);
        }

            for (int i = 0; i < height; i++)
            {
                buffer.SetChar(left, top + i, _rectangleCharacters[drawStyle][1], foreground, background);
                buffer.SetChar(left + width - 1, top + i, _rectangleCharacters[drawStyle][1], foreground, background);
            }

            buffer.SetChar(left, top, _rectangleCharacters[drawStyle][2], foreground, background);
            buffer.SetChar(left + width - 1, top, _rectangleCharacters[drawStyle][3], foreground, background);
            buffer.SetChar(left + width - 1, top + height - 1, _rectangleCharacters[drawStyle][4], foreground, background);
            buffer.SetChar(left, top + height - 1, _rectangleCharacters[drawStyle][5], foreground, background);
        }
    }
}
