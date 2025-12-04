namespace DeliveryApp
{
    public interface IOrderState
    {
        string Name { get; }

        void StartPreparation();
        void StartDelivery();
        void Complete();
        void Cancel();
    }
}