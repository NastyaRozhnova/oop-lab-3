namespace DeliveryApp
{
    public class SpecialOrderFactory : OrderFactory
    {
        protected override Order CreateOrder(
            int id,
            Customer customer,
            string deliveryAddress,
            bool isExpress,
            string? specialInstructions,
            bool noContactDelivery)
        {

            return new SpecialOrder(id, customer, deliveryAddress)
            {
                IsExpress = isExpress,
                SpecialInstructions = specialInstructions,
                NoContactDelivery = noContactDelivery
            };
        }
    }
}