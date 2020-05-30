using SFML.Graphics;
using System;
using SFML.System;
using System.Collections.Generic;

namespace DeliverySimulator
{
    public static class Gui
    {

        public static Font CurrentFont = null;
        public static int Focus = -1;
        public static Color PrimaryColor = Color.White;
        public static Color OutlineColor = Color.White;
        public static int OutlineThickness = 1;

        private static RectangleShape RectShape;
        private static Text DisplayText;

        private static RenderTexture Panel;
        private static Sprite PanelSprite;

        public static void Init()
        {
            CurrentFont = new Font("Assets/PressStart2P-Regular.ttf"); 
            DisplayText = new Text("", CurrentFont, 64);
            DisplayText.Scale = new Vector2f(.125f, .125f); // Text looks better when scaled down.
            RectShape = new RectangleShape(new Vector2f());
            Panel = new RenderTexture(800, 800);
            PanelSprite = new Sprite();
        }

        public static void FloatUpText(RenderWindow rw, string text, float x, float y, float max, float localY)
        {
            if (localY > max) return;

            Text(rw, text, x, y - localY);
        }

        /*
        public static void Inventory(RenderWindow rw, int cursor, string[] dat) 
        {
            var cam = rw.GetView();
            var inventoryPos = new Vector2f(cam.Size.X * .1f, cam.Size.Y * .1f);
            var inventorySize = new Vector2f(cam.Size.X * (1.0f - 0.2f), cam.Size.Y * (1.0f - 0.2f));
            var scrollbarPos = new Vector2f(inventoryPos.X + 4, inventoryPos.Y + 4);
            var scrollbarSize = new Vector2f(4, 100);
            var itemsPanelPos = new Vector2f(scrollbarPos.X + 8, scrollbarPos.Y + 2);

            var itemDescPos = new Vector2f(itemsPanelPos.X + inventorySize.X / 2, itemsPanelPos.Y);
            
            OutlineColor = Color.White;
            PrimaryColor = Color.Black;
            Rect(rw, inventoryPos.X, inventoryPos.Y, inventorySize.X, inventorySize.Y);

            PrimaryColor = Color.White;
            OutlineColor = Color.Transparent;
            Rect(rw, scrollbarPos.X, scrollbarPos.Y, scrollbarSize.X, scrollbarSize.Y);

            for (int i = 0; i < dat.Length; i += 2) 
            {
                string itemName = dat[i];

                string cursorString = i / 2 == cursor ? "->" : "  ";
                Text(rw, cursorString + itemName, itemsPanelPos.X, itemsPanelPos.Y + 4 * i);
            }


            if (dat.Length != 0)
            {
                string itemDesc = dat[cursor * 2 + 1];
                if (itemDesc.Length > 18)
                {
                    itemDesc = itemDesc.Insert(18, "\n");
                }
                Text(rw, itemDesc, itemDescPos.X, itemDescPos.Y);
            }
        }*/

        public static void Inventory(RenderWindow rw, int cursor, 
            string[] itemNames, string[] itemDescs)
        {
            var cam = rw.GetView();

            string currentDesc = itemDescs[cursor];

            Menu(rw, 10, 10, cursor, itemNames);
            Menu(rw, 150, 10, -1, new string[] { currentDesc });
        }

        public static void Menu(RenderWindow rw, float x, float y, int cursor, string[] choices, int wrapW = -1) 
        {
            int longestLength = 0;

            for (int i = 0; i < choices.Length; i++)
            {
                if ((choices[i]).Length > longestLength)
                    longestLength = choices[i].Length + 2;
            }

            OutlineColor = Color.White;
            PrimaryColor = Color.Black;

            Rect(rw, x, y, 8 + longestLength * 8 + 8, 8 + choices.Length*8 + 8);

            cursor %= choices.Length;

            for (int i = 0; i < choices.Length; i++) 
            {
                string selector = 
                    (i == cursor) && (cursor > -1)? 
                    "->" : "  ";


                Text(rw, selector + choices[i], 8 + x, 8 + (y + 8 * i));
            }

        }

        public static void Textbox(RenderWindow rw, float x, float y, string text) 
        {
            
        }

        public static void Rect(RenderWindow rw, float x, float y, float w, float h)
        {
            RectShape.FillColor = PrimaryColor;
            RectShape.Position = new Vector2f(x, y);
            RectShape.Size = new Vector2f(w, h);
            RectShape.OutlineColor = OutlineColor;
            RectShape.OutlineThickness = OutlineThickness;

            rw.Draw(RectShape);
        }

        public static void Text(RenderWindow rw, string text, float x, float y)
        {
            DisplayText.Position = new Vector2f(x, y);
            DisplayText.DisplayedString = text;

            rw.Draw(DisplayText);
        }

        public static void Draw(RenderWindow rw) 
        {   
         
        }
    }
}
