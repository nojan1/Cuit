using Cuit.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Screen
{
    public class SingleFocusControlFormScreen : FormScreen
    {
        protected override int _tabIndex { get => 0; }

        protected override void DrawFocusMarker(bool clear)
        {
            //Do not draw focus markers
        }
    }
}
