using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryApp
{
    public abstract class Order
    {
        private readonly List<OrderItem> _items = new();
        private readonly List<IOrderObserver> _observers = new();

        public int Id { get; }
        public Customer Customer { get; }
        public string DeliveryAddress { get; private set; }
        public DateTime CreatedAt { get; }

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public IOrderState State { get; private set; }
        public IReadOnlyCollection<IOrderObserver> Observers => _observers.AsReadOnly();

        private static readonly IPriceCalculator PriceCalculator = new PriceCalculator();

        public decimal TotalPrice => PriceCalculator.Calculate(this);

        protected Order(int id, Customer customer, string deliveryAddress)
        {
            Id = id;

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            Customer = customer; 

            if (deliveryAddress == null)
                throw new ArgumentNullException(nameof(deliveryAddress));
            DeliveryAddress = deliveryAddress; 

            CreatedAt = DateTime.UtcNow;

            State = new CreatedState(this);
        }

        public void ChangeDeliveryAddress(string newAddress)
        {
            if (string.IsNullOrWhiteSpace(newAddress))
                throw new ArgumentException("Введите адрес доставки", nameof(newAddress));

            DeliveryAddress = newAddress;

            Console.WriteLine($"Адрес доставки заказа #{Id} изменен на {newAddress}");
        }

        public void AddItem(MenuItem menuItem, int quantity, string? comment = null)
        {
            var item = new OrderItem(menuItem, quantity, comment);
            _items.Add(item);

            Console.WriteLine($"В заказ #{Id} добавлена позиция: {item}");
        }

        public void SetState(IOrderState newState)
        {
            var oldState = State;
            State = newState;

            if (oldState != null)
            {
                NotifyStateChanged(oldState, newState);
            }
        }

        public void AttachObserver(IOrderObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void DetachObserver(IOrderObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            _observers.Remove(observer);
        }

        private void NotifyStateChanged(IOrderState oldState, IOrderState newState)
        {
            foreach (var observer in _observers)
            {
                observer.OnOrderStateChanged(this, oldState, newState);
            }
        }

        public void StartPreparation() => State.StartPreparation();
        public void StartDelivery() => State.StartDelivery();
        public void Complete() => State.Complete();
        public void Cancel() => State.Cancel();

        public override string ToString()
        {
            return $"Заказ #{Id} (создан в {CreatedAt}) для {Customer.Name}, позиций: {Items.Count}, итого: {TotalPrice}, состояние: {State.Name}";
        }
    }
}