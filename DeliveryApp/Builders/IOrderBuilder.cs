namespace DeliveryApp
{
    public interface IOrderBuilder
    {
        void Reset();
        void SetCustomer(Customer customer);
        void SetDeliveryAddress(string address);
        void AddItem(MenuItem menuItem, int quantity, string? comment = null);
        void SetExpressDelivery(bool isExpress);
        void SetSpecialInstructions (string? specialInstructions);
        void SetNoContactDelivery (bool noContactDelivery);
        Order GetResult();
    }
}