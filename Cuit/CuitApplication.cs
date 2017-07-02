using Cuit.Screen;
using System;

namespace Cuit
{
    public class CuitApplication
    {
        private int _width;
        private int _height;
        private bool _quit;

        private Screenbuffer _buffer;
        public Screenbuffer Screenbuffer
        {
            get
            {
                if (_buffer == null)
                    _buffer = new Screenbuffer();

                return _buffer;
            }
        }

        public IScreen ActiveScreen{ get; private set; }

        public CuitApplication()
        {

        }

        public CuitApplication(int width, int height)
        {

        }

        public T InstantiateScreen<T>() where T : IScreen
        {
            var instance = Activator.CreateInstance<T>();
            instance.Application = this;

            return instance;
        }

        public void Run<T>() where T : IScreen
        {
            ActiveScreen = InstantiateScreen<T>();

            SetupConsole();
            Loop();
        }

        public void Run<T>(T screen) where T: IScreen
        {
            ActiveScreen = screen;

            SetupConsole();
            Loop();
        }

        private void SetupConsole()
        {
            Console.CursorVisible = false;
        }

        private void Loop()
        {
            while (!_quit)
            {
                ActiveScreen.Update(Screenbuffer);

                Render(Screenbuffer);

                var key = Console.ReadKey(true);
                ActiveScreen.HandleKeypress(key);
            }
        }

        private void Render(Screenbuffer screenbuffer)
        {
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;

            foreach(var changedCharacter in screenbuffer.GetChangedCharacters(true))
            {
                Console.SetCursorPosition(changedCharacter.Left, changedCharacter.Top);
                Console.Write(changedCharacter.Character);
            }

            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
    }
}
