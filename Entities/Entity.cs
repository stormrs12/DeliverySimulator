
using SFML.Graphics;
using SFML.System;
using System;

namespace DeliverySimulator.Entities
{
    public class Entity
    {
        public Vector2f Position;
        public Vector2f Velocity = new Vector2f();
        public Vector2f Size { get; protected set; } = new Vector2f(1, 1);

        public float Speed { get; set; } = 0;
        public float Weight { get; set; } = 1;
        public bool Visible { get; set; } = true;

        protected Sprite sprite;
        protected SpriteAnimator spriteAnimator;

        private Vertex[] debugBox;

        public Entity(Vector2f position) 
        {
            Position = position;
            debugBox = new Vertex[4] {
                new Vertex(),
                new Vertex(),
                new Vertex(),
                new Vertex()
            };

            debugBox[0].Color = Color.Red;
            debugBox[1].Color = Color.Red;
            debugBox[2].Color = Color.Red;
            debugBox[3].Color = Color.Red;
        }

        public virtual void Draw(RenderWindow rw)
        {
            if (Game.Debug)
            {
                debugBox[0].Position = Position;
                debugBox[1].Position = Position + new Vector2f(Size.X, 0);
                debugBox[2].Position = Position + new Vector2f(Size.X, Size.Y);
                debugBox[3].Position = Position + new Vector2f(0, Size.Y);

                rw.Draw(debugBox, PrimitiveType.Quads);
            }

            if (sprite != null && Visible) 
            {
                rw.Draw(sprite);
            }   
        }



        public virtual void Update(float delta) 
        {
            float velocityLength = (float)Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y);

            if (velocityLength != 0)
            {
                var normalized = Velocity / velocityLength;
                var finalVelocity = normalized * Speed;

                Position += finalVelocity * delta;

                spriteAnimator.Update(delta);
            }

            if (sprite != null)
            {
                sprite.Position = Position;

                if (spriteAnimator != null)
                {
                    sprite.TextureRect = spriteAnimator.TextureRect;
                }
            }
        }
    }
}
