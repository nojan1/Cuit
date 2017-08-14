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
            RegisterControl(label);

            var textbox = new Textbox(5, 5);
            textbox.Width = 40;
            RegisterControl(textbox);

            var textbox2 = new Textbox(5, 10);
            textbox2.Width = 40;
            RegisterControl(textbox2);

            var label2 = new Label(50, 2);
            label2.Text = "And a listbox below me";
            label2.Foreground = ConsoleColor.Green;
            label2.Background = ConsoleColor.DarkMagenta;
            RegisterControl(label2);

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

            RegisterControl(listbox);

            var button = new Button(5, 15);
            button.Text = "Click me";
            button.Click += (s, e) =>
            {
                Application.SwitchTo<ExampleForm2>();
            };
            RegisterControl(button);

            var starButton = new Button(18, 15);
            starButton.Text = "See stars";
            starButton.Click += (s, e) =>
            {
                Application.SwitchTo<Starfield>();
            };
            RegisterControl(starButton);

            var xmlFormButton = new Button(32, 15);
            xmlFormButton.Text = "Load from xml";
            xmlFormButton.Click += (s, e) =>
            {
                Application.SwitchTo<ExampleXmlForm>();
            };
            RegisterControl(xmlFormButton);
        }
    }

    public class ExampleForm2 : FormScreen
    {
        public ExampleForm2()
        {
            var label = new Label(5, 2);
            label.Text = "Form 2";
            RegisterControl(label);

            var label2 = new Label(30, 2);
            RegisterControl(label2);

            var progressBar = new Progressbar(20, 9);
            progressBar.Maximum = 1;
            progressBar.Width = 40;
            RegisterControl(progressBar);

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
            RegisterControl(numericUpDown);

            var button = new Button(5, 5);
            button.Text = "Go back";
            button.Click += (s, e) =>
            {
                Application.GoBack();
            };
            RegisterControl(button);

            var button2 = new Button(5, 9);
            button2.Text = "Increment";
            button2.Click += (s, e) =>
            {
                progressBar.Value++;
            };
            RegisterControl(button2);

            var checkbox = new Checkbox(5, 13);
            checkbox.Text = "Check me to be awesome";
            RegisterControl(checkbox);

            var image = new Image(5, 16);
            image.Border = Helpers.RectangleDrawStyle.Single;
            image.BorderColor = ConsoleColor.Yellow;
            image.SetImageFromContent(
@"${c1}                    ..
                  .oK0l
                 :0KKKKd.
               .xKO0KKKKd
              ,Od' .d0000l
             .c;.   .'''...           ..'.
.,:cloddxxxkkkkOOOOkkkkkkkkxxxxxxxxxkkkx:
;kOOOOOOOkxOkc'...',;;;;,,,'',;;:cllc:,.
 .okkkkd,.lko  .......',;:cllc:;,,'''''.
   .cdo. :xd' cd:.  ..';'',,,'',,;;;,'.
      . .ddl.;doooc'..;oc;'..';::;,'.
        coo;.oooolllllllcccc:'.  .
       .ool''lllllccccccc:::::;.
       ;lll. .':cccc:::::::;;;;'
       :lcc:'',..';::::;;;;;;;,,.
       :cccc::::;...';;;;;,,,,,,.
       ,::::::;;;,'.  ..',,,,'''.
        ........          ......");
            RegisterControl(image);
        }
    }

}
