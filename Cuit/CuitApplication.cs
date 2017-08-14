using System.Reflection;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using Cuit.Control.Behaviors;
using System.Threading.Tasks;
using System.Linq;

namespace Cuit
{
    public class CuitApplication
    {
        private bool _fullRedrawPending = true;
        private bool _gotFocusEventRaised = false;

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
            if (_screens.Count > 0)
            {

                RaiseFocusEventIfApplicable(ActiveScreen, false);
                ActiveScreen.UpdateRenderRequested -= OnUpdateRenderRequested;
            }

            _screens.Push(screen);
            _fullRedrawPending = true;
            _gotFocusEventRaised = false;

            RaiseLoadedEventIfApplicable(screen);

            ActiveScreen.UpdateRenderRequested += OnUpdateRenderRequested;
            UpdateAndRender(true);

            return screen;
        }

        public IScreen GoBack()
        {
            if (_screens.Count > 1)
            {
                RaiseFocusEventIfApplicable(ActiveScreen, false);
                ActiveScreen.UpdateRenderRequested -= OnUpdateRenderRequested;
                _screens.Pop();
            }
;
            _fullRedrawPending = true;
            _gotFocusEventRaised = false;

            ActiveScreen.UpdateRenderRequested += OnUpdateRenderRequested;
            UpdateAndRender(true);

            return ActiveScreen;
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
            var screen = InstantiateScreen<T>();
            Run(screen);
        }

        public void Run<T>(T screen) where T : IScreen
        {
            screen.Application = this;
            _screens.Push(screen);

            SetupConsole();
            RaiseLoadedEventIfApplicable(screen);
            _gotFocusEventRaised = false;

            ActiveScreen.UpdateRenderRequested += OnUpdateRenderRequested;

            Loop();
        }

        private void OnUpdateRenderRequested(object sender, EventArgs e)
        {
            UpdateAndRender(false);
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
            while (!Quit)
            {
                if (_fullRedrawPending)
                {
                    _fullRedrawPending = false;
                    Console.Clear();

                    UpdateAndRender(true);
                }

                var key = Console.ReadKey(true);
                ActiveScreen.HandleKeypress(key);
            }
        }

        private void UpdateAndRender(bool force)
        {
            if (force)
                Screenbuffer.Invalidate();

            RaiseFocusEventIfApplicable(ActiveScreen, true);
            ActiveScreen.Update(Screenbuffer, force);
            Render(Screenbuffer);
        }

        private void Render(Screenbuffer screenbuffer)
        {
            var writeGroupingBuffer = new StringBuilder();
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;
            var backgroundColor = Console.BackgroundColor;
            var foregroundColor = Console.ForegroundColor;
            var cursorVisible = Console.CursorVisible;

            Console.CursorVisible = false;

            var changedCharacters = screenbuffer.GetChangedCharacters(true)
                                                .OrderBy(c => c.Top)
                                                .ThenBy(c => c.Left)
                                                .ToList();

            for (int i = 0; i < changedCharacters.Count; i++)
            {
                if (changedCharacters[i].Left < 0 || changedCharacters[i].Left >= Console.BufferWidth ||
                    changedCharacters[i].Top < 0 || changedCharacters[i].Top >= Console.BufferHeight)
                    continue;

                writeGroupingBuffer.Clear();

                int x = 0;
                do
                {
                    writeGroupingBuffer.Append(changedCharacters[i + x++].Character);
                }
                while (i + x < changedCharacters.Count &&
                       changedCharacters[i].Top == changedCharacters[i + x].Top &&
                       changedCharacters[i + x].Left == changedCharacters[i + x - 1].Left + 1 &&
                       changedCharacters[i].Background == changedCharacters[i + x].Background &&
                       changedCharacters[i].Foreground == changedCharacters[i + x].Foreground);

                Console.BackgroundColor = changedCharacters[i].Background;
                Console.ForegroundColor = changedCharacters[i].Foreground;
                Console.SetCursorPosition(changedCharacters[i].Left, changedCharacters[i].Top);

                Console.Write(writeGroupingBuffer);

                if (writeGroupingBuffer.Length > 1)
                {
                    i += writeGroupingBuffer.Length - 1;
                }
            }

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.CursorVisible = cursorVisible;
        }

        private void RaiseLoadedEventIfApplicable(IScreen screen)
        {
            var loaded = screen as ILoaded;
            if (loaded != null)
            {
                loaded.OnLoaded();
            }
        }

        private void RaiseFocusEventIfApplicable<T>(T screen, bool gotFocus)
            where T : IScreen
        {
            var focusable = screen as IFocusable;
            if (focusable != null)
            {
                if (gotFocus)
                {
                    if (!_gotFocusEventRaised)
                    {
                        focusable.OnGotFocus();
                        _gotFocusEventRaised = true;
                    }
                }
                else
                {
                    focusable.OnLostFocus();
                }
            }
        }

    }
}
