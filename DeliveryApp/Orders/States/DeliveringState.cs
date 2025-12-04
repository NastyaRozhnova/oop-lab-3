namespace DeliveryApp
{
    public class DeliveringState : OrderState
    {
        public DeliveringState(Order order) : base(order) { }

        public override string Name => "В доставке";

        public override void Complete()
        {
            SetState(new CompletedState(Order));
        }

        public override void Cancel()
        {
            SetState(new CancelledState(Order));
        }
    }
}