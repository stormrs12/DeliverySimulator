
using DeliverySimulator.GameStates;

namespace DeliverySimulator.Items
{
    public class BoxItem : Item
    {
        public BoxItem(string name, string address) 
         : base("ITEM_BOX", "Package", 
               "A package for " + name + " " + address)
        {
            Address = address;
        }

        public string Address { get; set; }

        public override ItemUsage Usage  { get { return ItemUsage.TransferToHand; } }
    }
}
