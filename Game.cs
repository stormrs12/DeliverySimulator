using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

using DeliverySimulator.GameStates;
using SFML.System;

namespace DeliverySimulator
{
    public class Game
    {
        const int width = 1600;
        const int height = 900;
        const string title = "Orange Express";
        int fps = 0;


        const int guiViewWidth = 426;
        const int guiViewHeight = 240;
        
        private Stack<GameState> states = new Stack<GameState>();
        private System.Timers.Timer timer;
        private Clock clock;

        public static bool Debug = false;

        public void Run() {

            clock = new Clock();

            using (RenderWindow rw = new RenderWindow(new VideoMode(width, height), "DeliverySimulator")) 
            {
                rw.Closed += (s, e) => { 
                    rw.Close(); 
                };
                rw.SetFramerateLimit(120);

                timer = new System.Timers.Timer(1000);
                timer.AutoReset = true;
                timer.Elapsed += (s, e) => {
                    string currentTitle = title + " | FPS: " + fps.ToString() + (Debug ? " (Debug)" : "");
                    rw.SetTitle(currentTitle);
                };
                timer.Enabled = true;

                states.Push(new CityGameState());

                rw.KeyPressed += Rw_KeyPressed;
                rw.KeyReleased += Rw_KeyReleased;

                Gui.Init();

                while (rw.IsOpen) {
                    rw.DispatchEvents();

                    var time = clock.Restart();
                    float delta = time.AsSeconds();
                    fps = (int)(1f / delta);

                    if (states.Count > 0) 
                    {
                        if (rw.HasFocus()) {
                            rw.Clear();
                            states.Peek().Update(delta);

                            states.Peek().Draw(rw);

                            rw.Display();
                        }
                    } else { 
                        rw.Close(); 
                    }
                }
            }
        }

        private void Rw_KeyReleased(object sender, KeyEventArgs e)
        {
            if (states.Count > 0)
            {
                states.Peek().KeyUp(e.Code);
            }
        }

        private void Rw_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.F3)
            {
                Debug = !Debug;
            }

            if (states.Count > 0)
            {
                states.Peek().KeyDown(e.Code);
            }
        }
    }
}
