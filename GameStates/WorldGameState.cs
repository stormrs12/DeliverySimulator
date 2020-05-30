using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

using DeliverySimulator.Tiles;
using SFML.Window;
using DeliverySimulator.Items;

using DeliverySimulator.Entities;

namespace DeliverySimulator.GameStates
{
    public class WorldGameState : GameState
    {
        protected Sprite worldMap;
        protected List<Tile> tiles;
        protected List<Entity> entities;

        RectangleShape tmp= new RectangleShape(new Vector2f(16, 16));
        protected View camera;
        protected View guiView;

        protected float cameraBoundX = float.MinValue;
        protected float cameraBoundY = float.MinValue;
        protected float cameraBoundW = float.MaxValue;
        protected float cameraBoundH = float.MaxValue;

        protected string addressOnHand = "";

        protected Dictionary<string, Vector2f> addresses;
        protected DeliverySpaceTile deliverySpace = new DeliverySpaceTile(-10000, -10000);

        protected enum GameData {
            Day,
            TimeOfDay,
            DeliveryAddress
        }

        protected Dictionary<GameData, object> blob = new Dictionary<GameData, object>();

        protected int timeOfDay = 0;
        protected int timeResidentialSleep = 1;

        protected float deliverySuccessPopupY = 0f;
        protected Vector2f deliverySuccessTextPos = new Vector2f(-10000,-10000);

        protected int menuCursor = 0;
        protected bool inventoryUp = false;
        protected List<Item> inventoryItems = new List<Item>();

        Stack<int> lastMenuPositions = new Stack<int>();

        enum TruckMenu 
        {
            Drive,
            AccessInventory,
            Exit
        }
        string[] truckMenuOptions = new string[] { "Drive", "Inventory", "Exit" };

        enum TruckInventoryMenu 
        {
            
        }

        enum PlayerControlTarget
        {
            PlayerCharacter = 0,
            PlayerTruckMenu = 1,
            PlayerTruck = 2,
            Inventory = 3
        }

        Character player = new Character(new Vector2f(130, 30), "Assets/Sprites/sprite_main.png");
        Vehicle playerTruck;
        PlayerControlTarget inputTarget = PlayerControlTarget.PlayerCharacter;
        float[] playerInput = new float[] { 0f, 0f, 0f, 0f };
        bool onCar = false;
        private RectangleShape playerCursor;

        
        public WorldGameState(GameStateConstants thisState, string worldSpritePath) 
            : base(thisState)
        {
            worldMap = new Sprite(new Texture(worldSpritePath));
            tiles = new List<Tile>();
            entities = new List<Entity>();

            camera = new View(player.Position + player.Size / 2, new Vector2f(426, 240));
            guiView = new View(camera.Size / 2, camera.Size);

            tiles.Add(new Tile(0, 0));

            inventoryItems.Add(new BoxItem("Adam Smith", "1-1-1"));
            inventoryItems.Add(new BoxItem("Gary Baker", "1-1-2"));
            inventoryItems.Add(new BoxItem("Joshua Bright", "1-1-3"));

            playerCursor = new RectangleShape(new Vector2f(10, 10));
            playerTruck = new Vehicle(new Vector2f(130, 100), "Assets/Sprites/blue_vehicles_waifu2x.png");
            
            entities.Add(player);
            entities.Add(playerTruck);
        }

        public override void Draw(RenderWindow rw)
        {
            rw.SetView(camera);

            rw.Draw(worldMap);

            deliverySpace.Draw(rw);

            var dstPos = deliverySuccessTextPos;

            for (int i = 0; i < entities.Count; i++) 
            {
                entities[i].Draw(rw);
            }

            Gui.FloatUpText(rw, "+1", dstPos.X, dstPos.Y, 10, deliverySuccessPopupY);

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(rw);
            }

            if (Game.Debug)
                DrawDebug(rw);

            DrawGUI(rw);
        }

        RectangleShape[] debugRectCache;
        protected void DrawDebug(RenderWindow rw)
        {
            DrawPlayerCursor(rw);
        }

        private void DrawGUI(RenderWindow rw) 
        {
            rw.SetView(guiView);

            switch (inputTarget) 
            {
                case PlayerControlTarget.PlayerTruckMenu:
                    Gui.Menu(rw, 10, 10, menuCursor, truckMenuOptions);
                    break;
                case PlayerControlTarget.Inventory:

                    string[] inventoryNames = new string[inventoryItems.Count];
                    string[] inventoryDescs = new string[inventoryItems.Count];
                    for (int i = 0; i < inventoryItems.Count; i++)
                    {
                        inventoryNames[i] = inventoryItems[i].Name;
                        inventoryDescs[i] = inventoryItems[i].Description;
                    }

                    Gui.Inventory(rw, menuCursor, inventoryNames, inventoryDescs);
                    break;
            }
        }

        public override void Update(float delta)
        {
            UpdateCamera(delta);
            UpdateDeliverySpace(delta);
            UpdatePlayerVelocity(delta);
            UpdatePlayerCursor(delta);


            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].Visible)
                    continue;

                entities[i].Update(delta);

                var entityCollisionList = new List<Entity>(entities);
                entityCollisionList.RemoveAt(i);

                for (int j = 0; j < entityCollisionList.Count; j++)
                {
                    if (!entityCollisionList[j].Visible)
                        continue;

                    if (Collision(
                        entities[i].Position.X, entities[i].Position.Y, entities[i].Size.X, entities[i].Size.Y,
                        entityCollisionList[j].Position.X, entityCollisionList[j].Position.Y,
                        entityCollisionList[j].Size.X, entityCollisionList[j].Size.Y))
                    {
                        if (entities[i].Weight < entityCollisionList[j].Weight)
                        {
                            entities[i].Position = CollisionResponsePixelSnap(
                        entities[i].Position.X, entities[i].Position.Y, entities[i].Size.X, entities[i].Size.Y,
                        entityCollisionList[j].Position.X, entityCollisionList[j].Position.Y,
                        entityCollisionList[j].Size.X, entityCollisionList[j].Size.Y);
                        }
                    }
                }

                for (int k = 0; k < tiles.Count; k++)
                {
                    if (Collision(
                        entities[i].Position.X, entities[i].Position.Y, entities[i].Size.X, entities[i].Size.Y,
                        tiles[k].X, tiles[k].Y, tiles[k].W, tiles[k].H))
                    {
                        entities[i].Position = CollisionResponsePixelSnap(
                            entities[i].Position.X, entities[i].Position.Y, entities[i].Size.X, entities[i].Size.Y,
                        tiles[k].X, tiles[k].Y, tiles[k].W, tiles[k].H
                            );
                    }
                }
            }

            for (int k = 0; k < tiles.Count; k++)
            {
                tiles[k].Update(delta);
            }
        }

        public override void KeyDown(Keyboard.Key key)
        {
            switch (inputTarget) 
            {
                case PlayerControlTarget.PlayerTruck:
                    if (key <= Keyboard.Key.Down && key >= Keyboard.Key.Left)
                        playerInput[key - Keyboard.Key.Left] = 1f;

                    if (key == Keyboard.Key.X) 
                    {
                        float exitAngle = 0f;
                        switch (playerTruck.SpriteFrame) 
                        {
                            case 5:
                                exitAngle = 0f;
                                break;
                            case 7:
                                exitAngle = 90f;
                                break;
                            case 1:
                                exitAngle = 180;
                                break;
                            case 3:
                                exitAngle = -90f;
                                break;
                        }

                        exitAngle += 90f;
                        exitAngle = exitAngle * (float)Math.PI / 180f;

                        player.Position =
                            playerTruck.Position + 
                            playerTruck.Size / 2 +
                            -player.Size / 2 +
                            new Vector2f((float)Math.Cos(exitAngle), (float)Math.Sin(exitAngle)) * 10;

                        inputTarget = PlayerControlTarget.PlayerCharacter;

                        playerTruck.Velocity.X = 0;
                        playerTruck.Velocity.Y = 0;

                        player.Visible = true;
                    }

                    break;
                case PlayerControlTarget.PlayerCharacter:
                    if (key <= Keyboard.Key.Down && key >= Keyboard.Key.Left)
                        playerInput[key - Keyboard.Key.Left] = 1f;

                    if (key == Keyboard.Key.Z)
                    {
                        if (Collision(playerCursor.Position.X, playerCursor.Position.Y, playerCursor.Size.X, playerCursor.Size.Y,
                                playerTruck.Position.X, playerTruck.Position.Y, playerTruck.Size.X, playerTruck.Size.Y))
                        {
                            Array.Fill(playerInput, 0f);
                            inputTarget = PlayerControlTarget.PlayerTruckMenu;
                        }
                    }
                    break;
                case PlayerControlTarget.PlayerTruckMenu:
                    if (key == Keyboard.Key.Down)
                    {
                        menuCursor = (menuCursor + 1) % 3;
                    }
                    else if (key == Keyboard.Key.Up)
                    {
                        menuCursor = (menuCursor == 0 ? 2 : menuCursor - 1);
                    }

                    if (key == Keyboard.Key.Z) 
                    {
                        switch (menuCursor)
                        {
                            case (int)TruckMenu.Drive:
                                player.Visible = false;
                                inputTarget = PlayerControlTarget.PlayerTruck;
                                break;
                            case (int)TruckMenu.AccessInventory:
                                inputTarget = PlayerControlTarget.Inventory;
                                menuCursor = 0;
                                break;
                            case (int)TruckMenu.Exit:
                                menuCursor = 0;
                                inputTarget = PlayerControlTarget.PlayerCharacter;
                                break;
                        }
                    }
                    break;
                case PlayerControlTarget.Inventory:
                    if (key == Keyboard.Key.Down)
                    {
                        menuCursor = (menuCursor + 1) % 3;
                    }
                    else if (key == Keyboard.Key.Up)
                    {
                        menuCursor = (menuCursor == 0 ? 2 : menuCursor - 1);
                    }
                    break;
            }

            /*
            if (inventoryUp)
            {
                if (key == Keyboard.Key.Down) inventoryCursor = (inventoryCursor + 1) > inventoryItems.Count ? 0 : inventoryCursor + 1;
                if (key == Keyboard.Key.Up) inventoryCursor = inventoryCursor <= 0 ? inventoryItems.Count - 1 : inventoryCursor - 1;

                if (key == Keyboard.Key.Z) 
                {
                    if (inventoryItems[inventoryCursor].ItemID == "ITEM_BOX") 
                    {
                        addressOnHand = (inventoryItems[inventoryCursor] as BoxItem).Address;
                        inventoryItems.RemoveAt(inventoryCursor);
                    }
                }
            }*/
        }

        public override void KeyUp(Keyboard.Key key)
        {
            switch (inputTarget)
            {
                case PlayerControlTarget.PlayerCharacter:
                case PlayerControlTarget.PlayerTruck:
                    if (key <= Keyboard.Key.Down && key >= Keyboard.Key.Left)
                        playerInput[key - Keyboard.Key.Left] = 0f;
                    break;
            }            
        }

        private void DrawPlayerCursor(RenderWindow rw) 
        {
            rw.Draw(playerCursor);
        }

        private void UpdatePlayerCursor(float delta) 
        {
            float playerAngle = 0f;

            switch (player.SpriteRow) 
            {
                case 0:
                    playerAngle = 90;
                    break;
                case 1:
                    playerAngle = 180;
                    break;
                case 2:
                    playerAngle = 270;
                    break;
                case 3:
                    playerAngle = 0;
                    break;
            }
            playerAngle = playerAngle * (float)Math.PI / 180f;

            playerCursor.Position =
                player.Position + player.Size / 2 +  
                (new Vector2f((float)Math.Cos(playerAngle),(float)Math.Sin(playerAngle))* 10)
                - playerCursor.Size / 2;
        }

        private void UpdatePlayerVelocity(float delta)
        {
            if (inputTarget == PlayerControlTarget.PlayerTruck)
            {
                playerTruck.Velocity.X = playerInput[1] - playerInput[0];
                playerTruck.Velocity.Y = playerInput[3] - playerInput[2];
            }

            else if (inputTarget == PlayerControlTarget.PlayerCharacter)
            {
                player.Velocity.X = playerInput[1] - playerInput[0];
                player.Velocity.Y = playerInput[3] - playerInput[2];
            }

            else if (inputTarget == PlayerControlTarget.PlayerTruckMenu)
            {
                player.Velocity.X = 0;
                player.Velocity.Y = 0;
            }
        }

        private void UpdateDeliverySpace(float delta)
        {
            if (addresses != null)
            {
                if (addresses.ContainsKey(addressOnHand)) 
                {
                    var newPos = addresses[addressOnHand];
                    deliverySpace.Position = newPos;
                }
            }

            
            if (Collision(player.Position.X, player.Position.Y, 16, 16,
                deliverySpace.X, deliverySpace.Y, 16, 16)) 
            {
                addressOnHand = string.Empty;
                deliverySuccessTextPos = deliverySpace.Position;
                deliverySpace.Position = new Vector2f(-10000, -10000);
                deliverySuccessPopupY = 0;
            }

            deliverySuccessPopupY += 10 * delta;
            deliverySuccessPopupY = 
                deliverySuccessPopupY >= float.MaxValue ? float.MaxValue - 1 : deliverySuccessPopupY;

            deliverySpace.Update(delta);
        }

        private void UpdateCamera(float delta) 
        {
            if (inputTarget == PlayerControlTarget.PlayerTruck)
                camera.Center = playerTruck.Position + player.Size / 2;
            else if (inputTarget == PlayerControlTarget.PlayerCharacter)
                camera.Center = player.Position + player.Size / 2;

            float cameraLeft = camera.Center.X - camera.Size.X / 2;
            float cameraTop = camera.Center.Y - camera.Size.Y / 2;
            float cameraRight = cameraLeft + camera.Size.X;
            float cameraBottom = cameraTop + camera.Size.Y;

            if (cameraLeft < cameraBoundX)
                camera.Center = new Vector2f(cameraBoundX + camera.Size.X / 2, camera.Center.Y);

            if (cameraTop < cameraBoundY)
                camera.Center = new Vector2f(camera.Center.X, cameraBoundY + camera.Size.Y / 2);

            if (cameraRight >= cameraBoundW)
                camera.Center = new Vector2f(cameraBoundW - camera.Size.X / 2, camera.Center.Y);

            if (cameraBottom >= cameraBoundH)
                camera.Center = new Vector2f(camera.Center.X, cameraBoundH - camera.Size.Y / 2);
        }

        protected Vector2f CollisionResponsePixelSnap
            (float ax, float ay, float aw, float ah, 
            float bx, float by, float bw, float bh) 
        {
            float a_cenX = ax + aw / 2;
            float a_cenY = ay + ah / 2;

            float b_cenX = bx + bw / 2;
            float b_cenY = by + bh / 2;

            var resultVec = new Vector2f(ax, ay);
            
            var dist = new Vector2f(
                    a_cenX - b_cenX,
                    a_cenY - b_cenY
                );


            // Center of the entity is either at the top or the bottom 
            // of the collider.
            // Taking abs() because the value can be negative, which
            // doesn't make sense when comparing sizes.
            if (Math.Abs(dist.Y / bh) > Math.Abs(dist.X / bw))
            {
                // dist vector between the entity's center 
                // and the collider's center is pointing down.
                if (dist.Y > 0)
                {
                    resultVec = new Vector2f(resultVec.X, by + bh);
                }
                else //pointing up
                {
                    resultVec = new Vector2f(resultVec.X, by - ah);
                }
            }
            // X is longer. Entity is either on the left or right
            // of the collider.
            else
            {
                if (dist.X > 0)
                {
                    resultVec = new Vector2f(bx + bw, resultVec.Y);
                }
                else 
                {
                    resultVec = new Vector2f(bx - aw, resultVec.Y);
                }
            }

            return resultVec;
        }

        protected bool Collision
            (float ax, float ay, float aw, float ah,
             float bx, float by, float bw, float bh)
        {
            return 
                (ax + aw >= bx &&
                 ax < bx + bw &&
                 ay + ah >= by &&
                 ay < by + bh);
        }
    }
}
