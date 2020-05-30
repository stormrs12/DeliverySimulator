using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace DeliverySimulator.Entities
{
    public class Vehicle : Entity
    {
        public Vehicle(Vector2f position, string spritePath) : base(position)
        {
            sprite = new Sprite(new Texture(spritePath));
            spriteAnimator = new SpriteAnimator(sprite.Texture, 8, 8, 15);
            Size = new Vector2f(30, 30);
            Speed = 100;

            spriteAnimator.CurrentRow = 3; 
            spriteAnimator.CurrentFrame = 7;
            Size = new Vector2f(26, 37); spriteAnimator.Playing = false;

            Weight = 10;
        }

        public Vector2f SpritePosition
            => sprite.Position;

        public Vector2f SpriteFrameSize
            => (Vector2f)spriteAnimator.FrameSize;

        public int SpriteFrame
            => spriteAnimator.CurrentFrame;

        public override void Update(float delta)
        {
            float vehicleAngle = (float)Math.Atan2(Velocity.Y, Velocity.X) * 180 / (float)Math.PI;
            Console.WriteLine(vehicleAngle);

            float velocityLength = (float)Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y);

            if (velocityLength > 0)
            {
                switch (vehicleAngle)
                {
                    case 0f: // VX > 0 
                        spriteAnimator.CurrentFrame = 5;
                        Size = new Vector2f(50, 26);
                        break;
                    case 90f: // VY > 0
                        spriteAnimator.CurrentFrame = 7;
                        Size = new Vector2f(26, 37);
                        break;
                    case 180f: // VX < 0
                        spriteAnimator.CurrentFrame = 1;
                        Size = new Vector2f(50, 26);
                        break;
                    case -90f: // VY < 0
                        spriteAnimator.CurrentFrame = 3;
                        Size = new Vector2f(26, 38);
                        break;
                }
            }

            base.Update(delta);

            sprite.Position = Position + 
                Size / 2 - 
                new Vector2f(
                sprite.TextureRect.Width,
                sprite.TextureRect.Height
                ) / 2;
        }
    }
}
