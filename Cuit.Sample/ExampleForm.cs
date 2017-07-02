using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Sample
{
    public class ExampleForm : FormScreen
    {
        public ExampleForm()
        {
            var label = new Label(2, 10);
            label.Text = "Example string";
            Controls.Add(label);

            var textbox = new Textbox(5, 10);
            Controls.Add(textbox);

            var textbox2 = new Textbox(10, 10);
            Controls.Add(textbox2);
        }
    }
}
