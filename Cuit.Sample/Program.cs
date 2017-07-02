using Cuit.Control;
using Cuit.Screen;
using System;

namespace Cuit.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var application = new CuitApplication();
            application.Run<ExampleForm>();
        }
    }
}