using DeliverySimulator.Tiles;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliverySimulator.GameStates
{
    public class CityGameState : WorldGameState
    {
        public CityGameState() : 
            base(GameStateConstants.City, "Assets/Levels/city.png")
        {
            cameraBoundX = 0;
            cameraBoundY = 0;
            cameraBoundW = 1440;
            cameraBoundH = 1008;

            InitInvisibleWalls();
            InitAddresses();
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        private void InitAddresses() 
        {
            addresses = new Dictionary<string, Vector2f>();
            addresses.Add("1-1-1", new Vector2f(272, 287));
            addresses.Add("1-1-2", new Vector2f(448, 287));
        }

        private void InitInvisibleWalls() 
        {
            // City Walls
            tiles.Add(new InvisibleWallTile(112, 0, 16, 1008));
            tiles.Add(new InvisibleWallTile(128, 992, 400, 16));
            tiles.Add(new InvisibleWallTile(608, 992, 816, 16));
            tiles.Add(new InvisibleWallTile(1424, 0, 16, 1008));
            tiles.Add(new InvisibleWallTile(208, 0, 1216, 16));
            tiles.Add(new InvisibleWallTile(208, 16, 16, 16));

            // Block 1-1
            tiles.Add(new InvisibleWallTile(224, 144, 112, 144));
            tiles.Add(new InvisibleWallTile(356, 257, 26, 30));
            tiles.Add(new InvisibleWallTile(356, 193, 26, 30));
            tiles.Add(new InvisibleWallTile(356, 129, 26, 30));
            tiles.Add(new InvisibleWallTile(400, 176, 112, 112));

            // Block 1-2
            tiles.Add(new InvisibleWallTile(624, 144, 112, 144));
            tiles.Add(new InvisibleWallTile(751, 128, 160, 160));


            // Block 1-3
            tiles.Add(new InvisibleWallTile(1040, 144, 111, 144));
            tiles.Add(new InvisibleWallTile(1184, 144, 111, 144));

            // Block 2-1
            tiles.Add(new InvisibleWallTile(240, 416, 112, 160));
            tiles.Add(new InvisibleWallTile(352, 416, 144, 144));

            // Block 2-2 
            tiles.Add(new InvisibleWallTile(640, 432, 112, 144));
            tiles.Add(new InvisibleWallTile(784, 432, 112, 144));

            // Block 2-3
            tiles.Add(new InvisibleWallTile(1040, 416, 256, 160));

            // Block 3-1
            tiles.Add(new InvisibleWallTile(240, 720, 112, 144));
            tiles.Add(new InvisibleWallTile(384, 720, 112, 144));

            // Block 3-2
            tiles.Add(new InvisibleWallTile(640, 704, 144, 160));
            tiles.Add(new InvisibleWallTile(784, 736, 112, 112));

            // Block 3-3
            tiles.Add(new InvisibleWallTile(1024, 704, 160, 160));
            tiles.Add(new InvisibleWallTile(1200, 720, 112, 144));
        }
    }
}
