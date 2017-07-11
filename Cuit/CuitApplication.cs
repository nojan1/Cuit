using System.Reflection;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using Cuit.Control.Behaviors;
using System.Threading.Tasks;

namespace Cuit
{
    public class CuitApplication
    {
        private bool _fullRedrawPending = true;

        public int EventLoopIdleTime { get; set; } = 50;
        public bool Quit { get; set; }

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

        private readonly Stack<IScreen> _screens = new Stack<IScreen>();
        public IScreen ActiveScreen => _screens.Peek();

        public int Width => Console.BufferWidth;
        public int Height => Console.BufferHeight;

        public CuitApplication()
        {
            //Use default buffer size
        }

        public CuitApplication(int width, int height)
        {
            Console.BufferHeight = height;
            Console.BufferWidth = width;
        }

        public CuitApplication(int width)
        {
            Console.BufferWidth = width;
        }

        public T SwitchTo<T>() where T : IScreen
        {
            var screen = InstantiateScreen<T>();
            return SwitchTo(screen);
        }

        public T SwitchTo<T>(T screen) where T : IScreen
        {
            _screens.Push(screen);
            _fullRedrawPending = true;

            RaiseLoadedEventIfApplicable(screen);

            return screen;
        }

        public IScreen GoBack()
        {
            _screens.Pop();
            _fullRedrawPending = true;
            return _screens.Peek();
        }

        public T InstantiateScreen<T>() where T : IScreen
        {
            var instance = Activator.CreateInstance<T>();
            instance.Application = this;

            var formScreen = instance as FormScreen;
            if (formScreen != null)
            {
                formScreen.InstantiateComponents();
            }

            return instance;
        }

        public void Run<T>() where T : IScreen
        {
            _screens.Push(InstantiateScreen<T>());

            RaiseLoadedEventIfApplicable(ActiveScreen);
            SetupConsole();
            Loop();
        }

        public void Run<T>(T screen) where T : IScreen
        {
            screen.Application = this;
            _screens.Push(screen);

            RaiseLoadedEventIfApplicable(screen);
            SetupConsole();
            Loop();
        }

        private void SetupConsole()
        {
            Console.TreatControlCAsInput = true;
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            
            Console.WindowWidth = Console.BufferWidth;
        }

        private void Loop()
        {
            Task.Run(async () =>
            {
                while (!Quit)
                {
                    if (_fullRedrawPending)
                    {
                        _fullRedrawPending = false;
                        Console.Clear();
                        Screenbuffer.Invalidate();

                        ActiveScreen.Update(Screenbuffer, true);
                    }
                    else
                    {
                        ActiveScreen.Update(Screenbuffer, false);
                    }

                    Render(Screenbuffer);

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        ActiveScreen.HandleKeypress(key);
                    }
                    else
                    {
                        await Task.Delay(EventLoopIdleTime);
                    }

                }
            }).Wait();
        }

        private void Render(Screenbuffer screenbuffer)
        {
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;
            var backgroundColor = Console.BackgroundColor;
            var foregroundColor = Console.ForegroundColor;

            foreach (var changedCharacter in screenbuffer.GetChangedCharacters(true))
            {
                if (changedCharacter.Left < 0 || changedCharacter.Left >= Console.BufferWidth || 
                    changedCharacter.Top < 0 || changedCharacter.Top >= Console.BufferHeight)
                    continue;

                Console.BackgroundColor = changedCharacter.Background;
                Console.ForegroundColor = changedCharacter.Foreground;
                Console.SetCursorPosition(changedCharacter.Left, changedCharacter.Top);
                Console.Write(changedCharacter.Character);
            }

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        private void RaiseLoadedEventIfApplicable(IScreen screen)
        {
            var loaded = screen as ILoaded;
            if (loaded != null)
            {
                loaded.OnLoaded();
            }
        }
    }
}
