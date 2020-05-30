using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace DeliverySimulator.Entities
{
    public class Character : Entity
    {
        public Character(Vector2f position, string spritePath) : base(position)
        {
            sprite = new Sprite(new Texture(spritePath));
            spriteAnimator = new SpriteAnimator(sprite.Texture, 4, 4, 15);
            Size = new Vector2f(16, 16);
            Speed = 100;
        }

        public int SpriteRow {  get => spriteAnimator.CurrentRow; }
        
        public override void Update(float delta)
        {
            if (Velocity.Y > 0)
            {
                spriteAnimator.CurrentRow = 0;
            }

            if (Velocity.Y < 0)
            {
                spriteAnimator.CurrentRow = 2;
            }

            if (Velocity.X < 0) 
            {
                spriteAnimator.CurrentRow = 1;
            }

            if (Velocity.X > 0)
            {
                spriteAnimator.CurrentRow = 3;
            }

            base.Update(delta);
        }
    }
}
