using SFML.Graphics;
using SFML.Window;
using System;

namespace DeliverySimulator.GameStates
{
    public enum GameStateConstants : byte 
    {
        Null,
        MainMenu,
        City,
        PauseScreen
    }

    public abstract class GameState
    {
        public bool Finished { get; private set; }
        public GameStateConstants NextState { get; private set; }
        public GameStateConstants ThisState { get; private set; }

        public GameState(GameStateConstants thisState) 
        {
            Finished = false;
            NextState = GameStateConstants.Null;
            ThisState = thisState;
        }

        public virtual void StateDone() 
        {
            Finished = true;
        }

        public abstract void Draw(RenderWindow rw);
        public abstract void Update(float delta);

        public virtual void KeyDown(Keyboard.Key key) { }
        public virtual void KeyUp(Keyboard.Key key) { }
    }
}
