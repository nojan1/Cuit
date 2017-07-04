﻿using Cuit.Control;
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
                Application.SwitchTo<ExampleForm2>();
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

    public class ExampleForm2 : FormScreen
    {
        public ExampleForm2()
        {
            var label = new Label(5, 2);
            label.Text = "Form 2";
            Controls.Add(label);

            var label2 = new Label(30, 2);
            Controls.Add(label2);

            var numericUpDown = new NumericUpDown(30, 5);
            numericUpDown.ValueChanged += (s, v) => label2.Text = v.ToString();
            Controls.Add(numericUpDown);

            var button = new Button(5, 5);
            button.Text = "Go back";
            button.Click += (s, e) =>
            {
                Application.GoBack();
            };
            Controls.Add(button);

            var progressBar = new Progressbar(20, 9);
            progressBar.Width = 40;
            Controls.Add(progressBar);

            var button2 = new Button(5, 9);
            button2.Text = "Increment";
            button2.Click += (s, e) =>
            {
                progressBar.Value++;
            };
            Controls.Add(button2);

            var checkbox = new Checkbox(5, 13);
            checkbox.Text = "Check me to be awesome";
            Controls.Add(checkbox);
        }
    }
}
