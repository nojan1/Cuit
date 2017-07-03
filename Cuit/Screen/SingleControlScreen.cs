using Cuit.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Screen
{
    public class SingleControlScreen<T> : IScreen
        where T : IControl
    {
        public CuitApplication Application { get; set; }
        public T Control { get; private set; }

        public SingleControlScreen(T control)
        {
            Control = control;
        }

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            Control.HandleKeypress(key);
        }

        public void Update(Screenbuffer buffer, bool force)
        {
            Control.Draw(buffer);
        }
    }
}
