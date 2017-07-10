using Cuit.Control;
using Cuit.Models;
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

            var label2 = new Label(50, 2);
            label2.Text = "And a listbox below me";
            label2.Foreground = ConsoleColor.Green;
            label2.Background = ConsoleColor.DarkMagenta;
            Controls.Add(label2);

            var listbox = new Listbox<DateTimeOffset?>(50, 4);
            listbox.Multiselect = false;
            listbox.Autoselect = true;
            listbox.Width = 50;
            listbox.Height = 15;

            Enumerable.Range(0, 15).ToList().ForEach(i =>
            {
                listbox.Items.Add(DateTimeOffset.Now.AddDays(i));
            });

            listbox.SelectionChanged += (s, v) =>
            {
                if (v == null)
                {
                    label2.Text = "And a listbox below me";
                }
                else
                {
                    label2.Text = v.ToString();
                }
            };

            Controls.Add(listbox);

            var button = new Button(5, 15);
            button.Text = "Click me";
            button.Click += (s, e) =>
            {
                Application.SwitchTo<ExampleForm2>();
            };
            Controls.Add(button);

            var starButton = new Button(18, 15);
            starButton.Text = "See stars";
            starButton.Click += (s, e) =>
            {
                Application.SwitchTo<Starfield>();
            };
            Controls.Add(starButton);

            var xmlFormButton = new Button(32, 15);
            xmlFormButton.Text = "Load from xml";
            xmlFormButton.Click += (s, e) =>
            {
                Application.SwitchTo<ExampleXmlForm>();
            };
            Controls.Add(xmlFormButton);
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

            var progressBar = new Progressbar(20, 9);
            progressBar.Maximum = 1;
            progressBar.Width = 40;
            Controls.Add(progressBar);

            var numericUpDown = new NumericUpDown(30, 5);
            numericUpDown.ValueChanged += (s, v) => label2.Text = v.ToString();
            numericUpDown.ValueChanged += (s, v) =>
            {
                if(progressBar.Maximum < v)
                {
                    progressBar.Maximum =(int)v;
                }

                progressBar.Value = (int)v;
            };
            Controls.Add(numericUpDown);

            var button = new Button(5, 5);
            button.Text = "Go back";
            button.Click += (s, e) =>
            {
                Application.GoBack();
            };
            Controls.Add(button);

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

            var image = new Image(5, 16);
            image.Border = Helpers.RectangleDrawStyle.Single;
            image.BorderColor = ConsoleColor.Yellow;
            image.SetImageFromContent(
@" /--------\ 
 |        | 
 |        | 
 \--------/ ",
                ConsoleColor.Blue);
            Controls.Add(image);
        }
    }

}
