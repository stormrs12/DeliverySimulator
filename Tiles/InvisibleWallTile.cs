using System;
using System.Collections.Generic;
using SFML.Window;
using System.Text;
using SFML.Graphics;

namespace DeliverySimulator.Tiles
{
    public class InvisibleWallTile : Tile
    {
        public InvisibleWallTile(float x, float y, float w, float h, string path = "") : base(x, y, path)
        {
            debugShape.OutlineColor = Color.Green;
            
            W = w;
            H = h;
        }
    }
}
