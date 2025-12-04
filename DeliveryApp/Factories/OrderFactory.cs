using System;

namespace DeliveryApp
{
    public abstract class OrderFactory
    {
        private static int _nextId = 1;

        protected abstract Order CreateOrder(
            int id,
            Customer customer,
            string deliveryAddress,
            bool isExpress,
            string? specialInstructions,
            bool noContactDelivery);

        public Order Create(
            Customer customer,
            string deliveryAddress,
            bool isExpress,
            string? specialInstructions,
            bool noContactDelivery)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (deliveryAddress == null)
                throw new ArgumentNullException(nameof(deliveryAddress));

            var id = _nextId++;

            var order = CreateOrder(id, customer, deliveryAddress, isExpress, specialInstructions, noContactDelivery);

            return order;
        }
    }
}