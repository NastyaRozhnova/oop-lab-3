namespace DeliveryApp
{
    public class CompletedState : OrderState
    {
        public CompletedState(Order order) : base(order) { }

        public override string Name => "Завершен";
    }
}