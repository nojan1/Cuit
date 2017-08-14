using Cuit.Control.Behaviors;
using Cuit.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cuit.Sample
{
    enum AnimationStage
    {
        Unlit = 1,
        FadingIn = 2,
        Lit = 3,
        FadingOut = 4
    }

    class Star
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public AnimationStage Stage { get; set; }
        public int Slot { get; set; }
    }

    class Starfield : IScreen, ILoaded, IFocusable
    {
        private const int MAX_TOP = 30;
        private const int MAX_LEFT = 120;
        private const int NUM_STARS = 200;
        private const int NUM_SLOTS = NUM_STARS / 10;

        private readonly Dictionary<AnimationStage, ConsoleColor> _colors = new Dictionary<AnimationStage, ConsoleColor>
        {
            { AnimationStage.Unlit, ConsoleColor.DarkGray },
            { AnimationStage.FadingIn, ConsoleColor.Gray },
            { AnimationStage.Lit, ConsoleColor.White  },
            { AnimationStage.FadingOut, ConsoleColor.Gray }
        };

        private Random rnd = new Random();
        private int _currentSlot = 0;
        private readonly List<Star> _stars = new List<Star>();

        public event EventHandler UpdateRenderRequested = delegate { };

        public CuitApplication Application { get; set; }
        public bool IsEnabled { get => false; set { return; } }

        public event EventHandler Loaded = delegate { };
        public event EventHandler GotFocus = delegate { };
        public event EventHandler LostFocus = delegate { };

        private CancellationTokenSource cancelTokenSource;

        public void HandleKeypress(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Escape)
            {
                Application.GoBack();
            }
        }

        public void OnLoaded()
        {
            Loaded(this, new EventArgs());

            for(int i = 0; i < NUM_STARS; i++)
            {
                _stars.Add(new Star
                {
                    Left = rnd.Next(0, MAX_LEFT),
                    Top = rnd.Next(0, MAX_TOP),
                    Slot = rnd.Next(0, NUM_SLOTS),
                    Stage = AnimationStage.Unlit
                });
            }
        }

        public void Update(Screenbuffer buffer, bool force)
        {
            foreach(var star in _stars.Where(s => s.Slot == _currentSlot))
            {
                buffer.SetChar(star.Left, star.Top, '*', _colors[star.Stage], ConsoleColor.Black);

                int newStage = (int)star.Stage + 1;
                if (newStage > 4)
                    newStage = 1;

                star.Stage = (AnimationStage)newStage;
            }

            _currentSlot = rnd.Next(0, NUM_SLOTS);
        }

        public void OnGotFocus()
        {
            cancelTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!cancelTokenSource.IsCancellationRequested)
                {
                    UpdateRenderRequested(this, new EventArgs());
                    await Task.Delay(100, cancelTokenSource.Token);
                }
            }, cancelTokenSource.Token);
        }

        public void OnLostFocus()
        {
            if(cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }
        }
    }
}
