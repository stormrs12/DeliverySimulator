using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliverySimulator.Tiles
{
    public enum TileID
    {
        Test,
        InvisibleWall
    }

    public class Tile
    {
        private Sprite sprite;
        protected RectangleShape debugShape;
        public bool Empty { get; private set; }
        public bool Solid { get; set; } = true;
        public List<string> CollisionException = new List<string>();

        public Vector2f Position
        {
            get
            {
                return sprite.Position;
            }

            set 
            {
                sprite.Position = value;
            }
        }

        public float X { 
            get { 
                return sprite.Position.X;  
            }
            set {
                sprite.Position = new Vector2f(value, sprite.Position.Y);
                debugShape.Position = sprite.Position;
            } 
        }

        public float Y
        {
            get
            {
                return sprite.Position.Y;
            }
            set
            {
                sprite.Position = new Vector2f(sprite.Position.X, value);
                debugShape.Position = sprite.Position;
            }
        }

        private float w;
        public float W
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
                debugShape.Size = new Vector2f(value, debugShape.Size.Y);
            }
        }

        private float h;
        public float H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
                debugShape.Size = new Vector2f(debugShape.Size.X, value);
            }
        }

        public Tile(float x, float y, string path = "") 
        {
            sprite = String.IsNullOrEmpty(path) ? 
                new Sprite(new Texture(new Image(16, 16))) : 
                new Sprite(new Texture(path));

            debugShape = new RectangleShape((Vector2f)sprite.Texture.Size);
            debugShape.FillColor = Color.Transparent;
            debugShape.OutlineThickness = -1;
            debugShape.OutlineColor = Color.Blue;

            w = debugShape.Size.X;
            h = debugShape.Size.Y;

            Empty = String.IsNullOrEmpty(path);
            X = x;
            Y = y;
        }

        public virtual void Draw(RenderWindow rw) 
        {
            if (!Empty)
                rw.Draw(sprite);

            if (Game.Debug)
                rw.Draw(debugShape);
        }

        public virtual void Update(float delta) { }

        public virtual void OnCollide(object b) { }
    }
}
