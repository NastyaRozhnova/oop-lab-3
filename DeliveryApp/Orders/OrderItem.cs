using System;

namespace DeliveryApp
{
    public class OrderItem
    {
        public MenuItem MenuItem { get; }
        public int Quantity { get; }
        public string? Comment { get; }
        public decimal GetTotalPrice() => MenuItem.Price * Quantity;

        public OrderItem(MenuItem menuItem, int quantity, string? comment = null)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));
            MenuItem = menuItem;

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));
            Quantity = quantity;

            Comment = comment;
        }
        public override string ToString()
        {
            if (Comment == null)
            {
                return $"{MenuItem.Name} x{Quantity} (итого: {GetTotalPrice()} руб)";
            }

            return $"{MenuItem.Name} x{Quantity} (итого: {GetTotalPrice()} руб), комментарий: {Comment}";
        }

    }
}