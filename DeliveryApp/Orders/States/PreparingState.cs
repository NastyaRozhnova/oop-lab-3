namespace DeliveryApp
{
    public class PreparingState : OrderState
    {
        public PreparingState(Order order) : base(order) { }

        public override string Name => "Собирается";

        public override void StartDelivery()
        {
            SetState(new DeliveringState(Order));
        }

        public override void Cancel()
        {
            SetState(new CancelledState(Order));
        }
    }
}