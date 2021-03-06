﻿using Cuit.Control.Behaviors;
using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class NumericUpDown : ControlBase, IFocusable, IValueChange<decimal>
    {
        public bool IsEnabled { get; set; } = true;
        public string Text { get; set; }

        public override int Height => 3;

        private int _width = -1;
        public override int Width
        {
            get
            {
                return _width != -1 ? _width : 30;
            }
            set
            {
                _width = value;
            }
        }

        public decimal Minimum { get; set; } = 0M;
        public decimal Maximum { get; set; } = 100M;
        public decimal Value { get; set; } = 0M;
        public int DecimalPlaces { get; set; } = 0;
        public decimal Increment { get; set; } = 1.0M;

        public event EventHandler<decimal> ValueChanged = delegate { };
        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };

        public NumericUpDown(int left, int top)
            : base(left, top)
        { }

        public override void Draw(Screenbuffer buffer)
        {
            buffer.StartTrackingForObject(this);

            buffer.DrawRectangle(RectangleDrawStyle.Double, Left, Top, Width, Height);
            buffer.DrawString(Left + 1, Top + 1, "^v");

            var valueString = Value.ToString($"F{DecimalPlaces}");
            valueString += string.Concat(Enumerable.Repeat(' ', Width - valueString.Length - 5));

            buffer.DrawString(Left + 4, Top + 1, valueString, ConsoleColor.White);

            buffer.CommitTrackingData();
        }

        public override void HandleKeypress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    UpdateValue(Increment);
                    break;
                case ConsoleKey.DownArrow:
                    UpdateValue(-Increment);
                    break;
            }
        }

        public void OnGotFocus()
        {
            GotFocus(this, new EventArgs());
        }

        public void OnLostFocus()
        {
            LostFocus(this, new EventArgs());
        }

        private void UpdateValue(decimal increment)
        {
            var oldValue = Value;

            Value += increment;
            Value = Math.Max(Minimum, Value);
            Value = Math.Min(Maximum, Value);

            if (Value != oldValue)
            {
                IsDirty = true;
                ValueChanged(this, Value);
            }
        }
    }
}
