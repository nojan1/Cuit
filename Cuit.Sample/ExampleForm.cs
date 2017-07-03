using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuit.Sample
{
    public class ExampleForm : FormScreen
    {
        public ExampleForm()
        {
            var label = new Label(5, 2);
            label.Text = "Some text boxes below";
            label.Foreground = ConsoleColor.Red;
            Controls.Add(label);

            var textbox = new Textbox(5, 5);
            textbox.Width = 40;
            Controls.Add(textbox);

            var textbox2 = new Textbox(5, 10);
            textbox2.Width = 40;
            Controls.Add(textbox2);

            var button = new Button(5, 15);
            button.Text = "Click me";
            button.Click += (s, e) =>
            {
                Console.Beep();
            };
            Controls.Add(button);

            var label2 = new Label(50, 2);
            label2.Text = "And a listbox below me";
            label2.Foreground = ConsoleColor.Green;
            label2.Background = ConsoleColor.DarkMagenta;
            Controls.Add(label2);

            var listbox = new Listbox<DateTimeOffset>(50, 4);
            listbox.Width = 50;
            listbox.Height = 15;

            Enumerable.Range(0, 15).ToList().ForEach(i =>
            {
                listbox.Items.Add(new ListItem<DateTimeOffset>(DateTimeOffset.Now.AddDays(i)));
            });

            Controls.Add(listbox);
        }
    }
}
