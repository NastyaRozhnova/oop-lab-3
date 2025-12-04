using System;
using System.Collections.Generic;

namespace DeliveryApp
{
    public class OrderBuilder : IOrderBuilder
    {
        private readonly OrderFactory _standardFactory;
        private readonly OrderFactory _specialFactory;

        private Customer? _customer;
        private string? _deliveryAddress;
        private bool _isExpress;
        private string? _specialInstructions;
        private bool _noContactDelivery;
        private readonly List<(MenuItem MenuItem, int Quantity, string? Comment)> _items = new();

        public OrderBuilder(OrderFactory standardFactory, OrderFactory specialFactory)
        {
            _standardFactory = standardFactory;
            _specialFactory = specialFactory;
            Reset();
        }

        public void Reset()
        {
            _customer = null;
            _deliveryAddress = null;
            _isExpress = false;
            _specialInstructions = null; 
            _noContactDelivery = false;
            _items.Clear();
        }

        public void SetCustomer(Customer customer) => _customer = customer;

        public void SetDeliveryAddress(string address) => _deliveryAddress = address;

        public void AddItem(MenuItem menuItem, int quantity, string? comment = null) => _items.Add((menuItem, quantity, comment));

        public void SetExpressDelivery(bool isExpress) => _isExpress = isExpress;

        public void SetSpecialInstructions(string? specialInstructions) => _specialInstructions = specialInstructions;
        public void SetNoContactDelivery(bool noContactDelivery) => _noContactDelivery = noContactDelivery;
        public Order GetResult()
        {

            if (_customer == null)
                throw new InvalidOperationException("Клиент должен быть указан до создания заказа");

            if (_deliveryAddress == null)
                throw new InvalidOperationException("Адрес доставки должен быть указан до создания заказа");

            if (_items.Count == 0)
                throw new InvalidOperationException("Заказ должен состоять хотя бы из одной позиции");

            bool hasSpecialInstructions = _specialInstructions != null;

            var factory = _standardFactory;

            if (_isExpress || hasSpecialInstructions || _noContactDelivery) 
            {
                factory = _specialFactory;
            }

            var order = factory.Create(
                _customer,
                _deliveryAddress!,
                _isExpress,
                _specialInstructions,
                _noContactDelivery);

            foreach (var (menuItem, quantity, itemComment) in _items)
            {
                order.AddItem(menuItem, quantity, itemComment);
            }

            Reset();

            return order;
        }
    }
}