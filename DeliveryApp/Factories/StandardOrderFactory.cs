namespace DeliveryApp
{
    public class StandardOrderFactory : OrderFactory
    {
        protected override Order CreateOrder(
            int id,
            Customer customer,
            string deliveryAddress,
            bool isExpress,
            string? specialInstructions,
            bool noContactDelivery)
        {

            return new StandardOrder(id, customer, deliveryAddress); 
            
        }
    }
}