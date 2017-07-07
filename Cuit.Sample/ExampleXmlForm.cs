using Cuit.Control;
using Cuit.Control.Behaviors;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Sample
{
    class ExampleXmlForm : FormScreenXml
    {
        private Button btn => this["TestButton"];

        public ExampleXmlForm() 
            : base("ExampleXMLForm.xml")
        {
           
        }

        public override void OnLoaded()
        {
            base.OnLoaded();

            btn.Text = "Helly";
            btn.IsDirty = true;
        }
    }
}
