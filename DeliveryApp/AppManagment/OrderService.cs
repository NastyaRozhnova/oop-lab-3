using System;
using System.Collections.Generic;

namespace DeliveryApp
{
    public sealed class OrderService
    {

        private static readonly Lazy<OrderService> _instance =
            new Lazy<OrderService>(() => new OrderService());

        public static OrderService Instance => _instance.Value;

        private readonly IOrderBuilder _orderBuilder;
        private readonly List<Order> _orders = new();
        private readonly List<MenuItem> _menu = new();
        private readonly IOrderObserver _orderObserver = new OrderObserver();

        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
        public IReadOnlyList<MenuItem> Menu => _menu.AsReadOnly();

        private OrderService()
        {
            var standardFactory = new StandardOrderFactory();
            var specialFactory = new SpecialOrderFactory();

            _orderBuilder = new OrderBuilder(standardFactory, specialFactory);

            SeedMenu();
        }

        private void SeedMenu()
        {
            _menu.Add(new MenuItem(id: 1, name: "Пицца 4 сыра", price: 540m));
            _menu.Add(new MenuItem(id: 2, name: "Кока кола 0.5", price: 90m));
            _menu.Add(new MenuItem(id: 3, name: "Роллы Филадельфия", price: 970m));
            _menu.Add(new MenuItem(id: 4, name: "Медовик", price: 150m));
        }

        public Order CreateOrder(
            Customer customer,
            string deliveryAddress,
            bool isExpress,
            string? specialInstructions,
            bool noContactDelivery,
            IReadOnlyCollection<(MenuItem item, int quantity, string? itemComment)> items)
        {
            if (customer == null)
                throw new ArgumentException("Покупатель не указан", nameof(customer));
                
            if (deliveryAddress == null)
                throw new ArgumentException("Адрес доставки не может быть пустым", nameof(deliveryAddress));

            _orderBuilder.Reset();
            _orderBuilder.SetCustomer(customer);
            _orderBuilder.SetDeliveryAddress(deliveryAddress);
            _orderBuilder.SetExpressDelivery(isExpress);
            _orderBuilder.SetSpecialInstructions(specialInstructions);
            _orderBuilder.SetNoContactDelivery(noContactDelivery);

            foreach (var (item, quantity, itemComment) in items)
            {
                _orderBuilder.AddItem(item, quantity, itemComment);
            }

            var order = _orderBuilder.GetResult();
            order.AttachObserver(_orderObserver);
            _orders.Add(order);

            return order;
        }

        public Order CreateSampleStandardOrder()
        {
            var customer = new Customer(
                id: 1, 
                name: "Петр Васильев", 
                phone: "+7 952 812 52 52", 
                defaultAddress: "г. Санкт-Петербург, ул. Парашютная, кв. 80");

            var pizza = _menu[0]; 
            var cola = _menu[1];

            var items = new List<(MenuItem, int, string?)>
            {
                (pizza, 2, "побольше сыра"), (cola,  2, null)
            };

            return CreateOrder(
                customer,
                customer.DefaultAddress,
                isExpress: false,
                specialInstructions: null,
                noContactDelivery: false,
                items: items);
        }

        public Order CreateSampleSpecialOrder()
        {
            var customer = new Customer(
                id: 2,
                name: "Пётр Петров",
                phone: "+7 911 199 45 67",
                defaultAddress: "г. Санкт-Петербург, ул. Серпуховская, д. 10");

            var sushi = _menu[2];
            var medovik = _menu[3]; 

            var items = new List<(MenuItem, int, string?)>
            {
                (sushi, 1, "соевый соус отдельно"), (medovik, 3, null)
            };

            var order = CreateOrder(
                customer,
                "ИТМО, ул. Ломоносова, д. 9М",
                isExpress: true,
                specialInstructions: "Доставить ко входу, мы встретим",
                noContactDelivery: false,
                items: items);

            if (order is SpecialOrder specialOrder)
            {
                if (specialOrder.IsExpress)
                {
                    Console.WriteLine($"Заказ #{specialOrder.Id} оформлен как экспресс-доставка");
                }

                if (specialOrder.SpecialInstructions != null)
                {
                    Console.WriteLine($"Специальные инструкции к заказу #{specialOrder.Id}: {specialOrder.SpecialInstructions}");
                }

                if (specialOrder.NoContactDelivery)
                {
                    Console.WriteLine($"Оставить заказ #{specialOrder.Id} у двери");
                }
            }

            return order;
        }
        public void PrintOrderInfo(Order order)
        {
            Console.WriteLine(order);
        }

        public void RunOrderLifecycle(Order order)
        {
            Console.WriteLine("Начальное состояние:");
            Console.WriteLine(order);

            try
            {
                order.StartPreparation();
                Console.WriteLine("После StartPreparation:");
                Console.WriteLine(order);

                order.StartDelivery();
                Console.WriteLine("После StartDelivery:");
                Console.WriteLine(order);

                order.Complete();
                Console.WriteLine("После Complete:");
                Console.WriteLine(order);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Ошибка при смене состояния");
            }

            Console.WriteLine(new string('-', 80));
        }
    }
}