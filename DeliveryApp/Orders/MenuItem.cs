using System;

namespace DeliveryApp
{
    public class MenuItem
    {
        public int Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public MenuItem(int id, string name, decimal price)
        {
            Id = id;

            if (name == null)
                throw new ArgumentNullException(nameof(name));
            Name = name;

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));
            Price = price;
        }

        public override string ToString() => $"{Name} ({Price} рублей)";
    }
}