using System;
using System.Collections.Generic;
using System.Text;

namespace Cuit.Screen
{
    public interface IScreen
    {
        CuitApplication Application { get; set; }
        void HandleKeypress(ConsoleKeyInfo key);
        void Update(Screenbuffer buffer, bool force);
    }
}
