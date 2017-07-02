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
            var label = new Label(2, 5);
            label.Text = "Example string";
            Controls.Add(label);

            var textbox = new Textbox(5, 5);
            Controls.Add(textbox);

            var textbox2 = new Textbox(10, 5);
            Controls.Add(textbox2);

            var button = new Button(15, 5);
            button.Text = "Click me";
            button.Click += (s, e) =>
            {
                Console.Beep();
            };
            Controls.Add(button);
        }
    }
}
