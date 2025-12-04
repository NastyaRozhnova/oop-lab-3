namespace DeliveryApp
{
    public class CancelledState : OrderState
    {
        public CancelledState(Order order) : base(order) { }

        public override string Name => "Отменен";
    }
}