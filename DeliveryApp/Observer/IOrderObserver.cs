namespace DeliveryApp
{
    public interface IOrderObserver
    {
        void OnOrderStateChanged(Order order, IOrderState oldState, IOrderState newState);
    }
}