namespace DeliveryApp
{
    public class OrderObserver : IOrderObserver
    {
        public void OnOrderStateChanged(Order order, IOrderState oldState, IOrderState newState)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (oldState == null)
                throw new ArgumentNullException(nameof(oldState));
                
            if (newState == null)
                throw new ArgumentNullException(nameof(newState));

            Console.WriteLine($"Статус заказа #{order.Id} изменён: {oldState.Name} -> {newState.Name}");
        }
    }
}