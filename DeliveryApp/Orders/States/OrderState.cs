using System;

namespace DeliveryApp
{
    public abstract class OrderState : IOrderState
    {
        protected OrderState(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            Order = order;
        }

        protected Order Order { get; }

        public abstract string Name { get; }

        public virtual void StartPreparation() => ThrowInvalid(nameof(StartPreparation));
        public virtual void StartDelivery() => ThrowInvalid(nameof(StartDelivery));
        public virtual void Complete() => ThrowInvalid(nameof(Complete));
        public virtual void Cancel() => ThrowInvalid(nameof(Cancel));

        protected void ThrowInvalid(string action)
        {
            throw new InvalidOperationException($"Нельзя {action} когда заказ находится в состоянии '{Name}'");
        }

        protected void SetState(IOrderState newState)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState));

            Order.SetState(newState);
        }
    }
}