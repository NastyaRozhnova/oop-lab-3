namespace DeliveryApp
{
    public class StandardOrder : Order
    {
        public StandardOrder(int id, Customer customer, string deliveryAddress)
            : base(id, customer, deliveryAddress)
        {
        }
    }
}