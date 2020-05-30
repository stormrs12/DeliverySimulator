using SFML.Graphics;
using SFML.System;
using System;

namespace DeliverySimulator.Tiles
{
    public class DeliverySpaceTile : Tile
    {
        private RectangleShape orangeRect;

        private byte Opacity 
        { 
            get 
            { 
                return orangeRect.FillColor.A; 
            } 
            set
            {
                var color = orangeRect.FillColor;
                orangeRect.FillColor = new 
                    Color(color.R, color.G, color.B, value);
            }

        }

        public DeliverySpaceTile(float x, float y) : base(x, y, "")
        {
            orangeRect = new RectangleShape(new Vector2f(16, 16));
            orangeRect.FillColor = new Color(255, 127, 0);

            Opacity = 0;
        }

        private float cosColor = 0.0f;
        public override void Draw(RenderWindow rw)
        {
            rw.Draw(orangeRect);
            base.Draw(rw);
        }

        public override void Update(float delta)
        {
            orangeRect.Position = new Vector2f(X, Y);
            cosColor += delta * 5;

            float cosValue = ((float)Math.Cos(cosColor) + 1) / 2.0f;
            Opacity = (byte)(255.0f * cosValue);

            base.Update(delta);
        }
    }
}
