using DeliverySimulator.GameStates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliverySimulator.Items
{
    public enum ItemUsage 
    {
        None, // Story Item
        TransferToHand
    }

    public abstract class Item
    {
        public float Weight { get; private set; }
        public int Quantity { get; set; }

        public readonly string ItemID;

        public string Description { get; private set; }

        public abstract ItemUsage Usage { get; }

        private string name = "ITEM_NAME";
        public string Name { get {
                if (Quantity > 1)
                    return name + " (x" + Quantity.ToString() + ")";

                return name;
            } 
        }

        public Item(string itemId, string itemName, string description, float weight = 0.0f) 
        {
            name = itemName;
            Description = description;
            Weight = weight;
            ItemID = itemId;
        }

        public static Item SpawnItem(string id, string[] args)
        {
            switch (id)
            {
                case "ITEM_BOX":
                    return new BoxItem("Package", args[0]);
            }
            return null;
        }
    }
}
