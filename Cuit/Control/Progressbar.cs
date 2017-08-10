using Cuit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Control
{
    public class Progressbar : ControlBase
    {
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

        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;

        private int _value = 0;
        public int Value
        {
            get { return _value; }
            set {
                if (value > Maximum)
                    return;

                _value = value;
                IsDirty = true;
            }
        }

        public Progressbar(int left, int top)
            : base(left, top)
        { }

        public override void Draw(Screenbuffer buffer)
        {
            buffer.StartTrackingForObject(this);

            buffer.DrawRectangle(RectangleDrawStyle.Single, Left, Top, Width, Height);

            string progressString = "";
            if (Value > Minimum)
            {
                var percentage = (int)Math.Round(((Width - 2.0) / (Maximum - Minimum)) * Value);
                progressString = string.Concat(Enumerable.Repeat((char)0x2588, percentage));
            }

            if (progressString.Length < Width - 2)
            {
                progressString += string.Concat(Enumerable.Repeat(' ', Width - 2 - progressString.Length));
            }

            buffer.DrawString(Left + 1, Top + 1, progressString, ConsoleColor.DarkBlue);

            var percentageString = $"{Value}%";
            var percentageStringLeft = Left + 1 + ((Width - 2) / 2) - (percentageString.Length / 2);
            buffer.DrawString(percentageStringLeft, Top + 1, percentageString, ConsoleColor.White, ConsoleColor.DarkBlue);

            buffer.CommitTrackingData();
        }
    }
}
