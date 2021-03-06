﻿using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class Label : ControlBase
    {

        public override int Height => GetLines().Length;

        private int _width = -1;
        public override int Width
        {
            get
            {
                return _width == -1 ? GetLines().OrderByDescending(l => l.Length).FirstOrDefault()?.Length ?? 0
                                    : _width;
            }
            set
            {
                _width = value;
                IsDirty = true;
            }
        }

        public ConsoleColor Foreground { get; set; } = Screenbuffer.DEFAULT_FOREGROUND;
        public ConsoleColor Background { get; set; } = Screenbuffer.DEFAULT_BACKGROUND;
        public bool IsMultiline { get; set; }

        private string _text = "";
        public string Text
        {
            get { return _text; }
            set
            {
                if (IsMultiline)
                {
                    _text = value;
                }
                else
                {
                    _text = value.Replace(Environment.NewLine, "");
                }

                IsDirty = true;
            }
        }

        public Label(int left, int top)
            : base(left, top)
        { }

        public override void Draw(Screenbuffer buffer)
        {
            var lines = GetLines();

            buffer.StartTrackingForObject(this);

            for (int i = 0; i < lines.Length; i++)
            {
                buffer.DrawString(Left, Top + i, lines[i], Foreground, Background);
            }

            buffer.CommitTrackingData();
        }

        private string[] GetLines()
        {
            var lines = _text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            if (_width != -1)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    while (lines[i].Length > _width)
                    {
                        var fullLine = lines[i];

                        lines[i] = fullLine.Substring(0, _width);
                        lines.Insert(i+1, fullLine.Substring(_width));
                    }
                }
            }

            return lines.ToArray();
        }
    }
}
