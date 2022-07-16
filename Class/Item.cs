using System;
namespace Lister.Class
{
    public class Item
    {
        public Item()
        {
        }

        public Item Parent { get; set; }
        public string Name { get; set; }
        public bool Packed { get; set; }
        public int Amount { get; set; }
    }
}
