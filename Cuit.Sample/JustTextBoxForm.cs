using Cuit.Control;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Sample
{
    class JustTextBoxForm : SingleFocusControlFormScreen
    {
        public override void InstantiateComponents()
        {
            var textbox = new Textbox(5, 2);
            textbox.Width = Application.Width - 10;
            RegisterControl(textbox);

            RegisterControl(new Textbox(5, 5));
        }
    }
}
