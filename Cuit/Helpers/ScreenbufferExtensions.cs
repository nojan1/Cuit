using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Helpers
{
    public enum RectangleDrawStyle
    {
        Clear,
        Single,
        Double
    }

    public static class ScreenbufferExtensions
    {
        public static void DrawString(this Screenbuffer buffer, int top, int left, string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                buffer[top, left + i] = str[i];
            }
        }

        private static readonly Dictionary<RectangleDrawStyle, char[]> _rectangleCharacters = new Dictionary<RectangleDrawStyle, char[]>
        {
            {RectangleDrawStyle.Clear, new char[] {'\0', '\0', '\0', '\0', '\0', '\0'} },
            {RectangleDrawStyle.Single, new char[] {(char)196, (char)179, (char)218, (char)191, (char)217, (char)192} },
            {RectangleDrawStyle.Double, new char[] {'\0', '\0', '\0', '\0', '\0', '\0'} },
        };
        public static void DrawRectangle(this Screenbuffer buffer, RectangleDrawStyle drawStyle, int top, int left, int width, int height)
        {
            for(int i = 0; i < width; i++)
            {
                buffer[top, left + i] = _rectangleCharacters[drawStyle][0];
                buffer[top + height - 1, left + i] = _rectangleCharacters[drawStyle][0];
            }

            for (int i = 0; i < height; i++)
            {
                buffer[top + 1, left] = _rectangleCharacters[drawStyle][1];
                buffer[top + 1, left + width - 1] = _rectangleCharacters[drawStyle][1];
            }

            buffer[top, left] = _rectangleCharacters[drawStyle][2];
            buffer[top, left + width - 1] = _rectangleCharacters[drawStyle][3];
            buffer[top + height - 1, left + width - 1] = _rectangleCharacters[drawStyle][4];
            buffer[top + height - 1, left] = _rectangleCharacters[drawStyle][5];
        }
    }
}
