namespace DeliveryApp
{
    public class CreatedState : OrderState
    {
        public CreatedState(Order order) : base(order) { }

        public override string Name => "Создан";

        public override void StartPreparation()
        {
            SetState(new PreparingState(Order));
        }

        public override void Cancel()
        {
            SetState(new CancelledState(Order));
        }
    }
}