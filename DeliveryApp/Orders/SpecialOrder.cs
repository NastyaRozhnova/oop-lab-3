namespace DeliveryApp
{
    public class SpecialOrder : Order
    {
        public SpecialOrder(int id, Customer customer, string deliveryAddress)
            : base(id, customer, deliveryAddress)
        {
        }
        public string? SpecialInstructions { get; set; }
        public bool IsExpress { get; set; }
        public bool NoContactDelivery { get; set; }
    }
}