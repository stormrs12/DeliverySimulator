using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace DeliverySimulator.Tiles
{
    public class DirectionTile : Tile
    {
        public enum Direction 
        {
            Left,
            Right,
            Up, 
            Down
        }

        private Direction direction;

        public DirectionTile(float x, float y, Direction direction) : base(x, y, "")
        {
            H = 16;
            W = 16;

            this.direction = direction;
            Solid = false;
        }

        public override void Draw(RenderWindow rw)
        {
            string directionString = "";
            switch (direction) 
            {
                case Direction.Left: directionString = "Left"; break;
                case Direction.Right: directionString = "Right"; break;
                case Direction.Up: directionString = "Up"; break;
                case Direction.Down: directionString = "Down"; break;
            }

            if (Game.Debug) { 
                Gui.Text(rw, directionString, X, Y);
            }

            base.Draw(rw);
        }

        public override void OnCollide(object b)
        {
            var entity = (Entities.Entity)b;

            switch (direction)
            {
                case Direction.Left:
                    entity.Velocity.X = -1;
                    entity.Velocity.Y =  0;
                    break;
                case Direction.Right:
                    entity.Velocity.X = 1;
                    entity.Velocity.Y = 0;
                    break;
                case Direction.Up:
                    entity.Velocity.X =  0;
                    entity.Velocity.Y = -1;
                    break;
                case Direction.Down:
                    entity.Velocity.X =  0;
                    entity.Velocity.Y =  1;
                    break;
            }
        }
    }
}
