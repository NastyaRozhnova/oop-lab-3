using System;
using DeliveryApp;
using Xunit;


namespace DeliveryApp.Tests
{
    internal static class Creator
    {
        public static Customer CreateCustomer() => new Customer(1, "Иван", "+70000000000", "Адрес");

        public static MenuItem CreateMenuItem(int id = 1, string name = "Пицца", decimal price = 500m) => new MenuItem(id, name, price);

        public static OrderBuilder CreateOrderBuilder()
        {
            var standardFactory = new StandardOrderFactory();
            var specialFactory = new SpecialOrderFactory();

            return new OrderBuilder(standardFactory, specialFactory);
        }

        public static Order CreateStandardOrder()
        {
            var customer = CreateCustomer();
            var builder = CreateOrderBuilder();

            builder.SetCustomer(customer);
            builder.SetDeliveryAddress("Адрес доставки");
            builder.AddItem(CreateMenuItem(), 1);

            return builder.GetResult(); 
        }

        public static SpecialOrder CreateSpecialOrder(
            bool isExpress = true,
            string? specialInstructions = "Аккуратно",
            bool noContactDelivery = true)
        {
            var customer = CreateCustomer();
            var builder = CreateOrderBuilder();

            builder.SetCustomer(customer);
            builder.SetDeliveryAddress("Адрес доставки");
            builder.AddItem(CreateMenuItem(), 1);
            builder.SetExpressDelivery(isExpress);
            builder.SetSpecialInstructions(specialInstructions);
            builder.SetNoContactDelivery(noContactDelivery);

            var order = builder.GetResult();
            return (SpecialOrder)order; 
        }
    }
    public class UnitTest1
    {
        private static Order CreateOrder() => Creator.CreateStandardOrder();

        [Fact]
        public void GetResult_WithoutCustomer_Throws()
        {
            var builder = Creator.CreateOrderBuilder();
            builder.SetDeliveryAddress("Адрес");
            builder.AddItem(Creator.CreateMenuItem(), 1);

            Assert.Throws<InvalidOperationException>(() => builder.GetResult());
        }

        [Fact]
        public void GetResult_WithoutItems_Throws()
        {
            var builder = Creator.CreateOrderBuilder();
            builder.SetCustomer(Creator.CreateCustomer());
            builder.SetDeliveryAddress("Адрес");

            Assert.Throws<InvalidOperationException>(() => builder.GetResult());
        }

        [Fact]
        public void GetResult_StandardOrder_WhenNoSpecialOptions()
        {
            var builder = Creator.CreateOrderBuilder();
            builder.SetCustomer(Creator.CreateCustomer());
            builder.SetDeliveryAddress("Адрес");
            builder.AddItem(Creator.CreateMenuItem(), 1);

            var order = builder.GetResult();

            Assert.IsType<StandardOrder>(order);
        }

        [Fact]
        public void GetResult_SpecialOrder_WhenExpress()
        {
            var builder = Creator.CreateOrderBuilder();
            builder.SetCustomer(Creator.CreateCustomer());
            builder.SetDeliveryAddress("Адрес");
            builder.AddItem(Creator.CreateMenuItem(), 1);
            builder.SetExpressDelivery(true);

            var order = builder.GetResult();

            var special = Assert.IsType<SpecialOrder>(order);
            Assert.True(special.IsExpress);
        }

        [Fact]
        public void GetResult_SpecialOrder_WhenHasSpecialInstructions()
        {
            var builder = Creator.CreateOrderBuilder();
            builder.SetCustomer(Creator.CreateCustomer());
            builder.SetDeliveryAddress("Адрес");
            builder.AddItem(Creator.CreateMenuItem(), 1);
            builder.SetSpecialInstructions("Аккуратно");

            var order = builder.GetResult();

            var special = Assert.IsType<SpecialOrder>(order);
            Assert.Equal("Аккуратно", special.SpecialInstructions);
        }

        [Fact]
        public void GetResult_SpecialOrder_WhenNoContactDelivery()
        {
            var builder = Creator.CreateOrderBuilder();
            builder.SetCustomer(Creator.CreateCustomer());
            builder.SetDeliveryAddress("Адрес");
            builder.AddItem(Creator.CreateMenuItem(), 1);
            builder.SetNoContactDelivery(true);

            var order = builder.GetResult();

            var special = Assert.IsType<SpecialOrder>(order);
            Assert.True(special.NoContactDelivery);
        }

        [Fact]
        public void CreatedState_Name_IsCorrect()
        {
            var order = CreateOrder();

            Assert.IsType<CreatedState>(order.State);
            Assert.Equal("Создан", order.State.Name);
        }

        [Fact]
        public void CreatedState_StartPreparation_ChangesToPreparingState()
        {
            var order = CreateOrder();

            order.StartPreparation();

            Assert.IsType<PreparingState>(order.State);
            Assert.Equal("Собирается", order.State.Name);
        }

        [Fact]
        public void CreatedState_Cancel_ChangesToCancelledState()
        {
            var order = CreateOrder();

            order.Cancel();

            Assert.IsType<CancelledState>(order.State);
            Assert.Equal("Отменен", order.State.Name);
        }

        [Fact]
        public void CreatedState_StartDelivery_Throws()
        {
            var order = CreateOrder();

            Assert.Throws<InvalidOperationException>(() => order.StartDelivery());
        }

        [Fact]
        public void CreatedState_Complete_Throws()
        {
            var order = CreateOrder();

            Assert.Throws<InvalidOperationException>(() => order.Complete());
        }

        [Fact]
        public void PreparingState_StartDelivery_ChangesToDelivering()
        {
            var order = CreateOrder();
            order.StartPreparation();

            order.StartDelivery();

            Assert.IsType<DeliveringState>(order.State);
            Assert.Equal("В доставке", order.State.Name);
        }

        [Fact]
        public void PreparingState_Cancel_ChangesToCancelled()
        {
            var order = CreateOrder();
            order.StartPreparation();

            order.Cancel();

            Assert.IsType<CancelledState>(order.State);
            Assert.Equal("Отменен", order.State.Name);
        }

        [Fact]
        public void PreparingState_Complete_Throws()
        {
            var order = CreateOrder();
            order.StartPreparation();

            Assert.Throws<InvalidOperationException>(() => order.Complete());
        }

        [Fact]
        public void DeliveringState_Complete_ChangesToCompleted()
        {
            var order = CreateOrder();
            order.StartPreparation();
            order.StartDelivery();

            order.Complete();

            Assert.IsType<CompletedState>(order.State);
            Assert.Equal("Завершен", order.State.Name);
        }

        [Fact]
        public void DeliveringState_Cancel_ChangesToCancelled()
        {
            var order = CreateOrder();
            order.StartPreparation();
            order.StartDelivery();

            order.Cancel();

            Assert.IsType<CancelledState>(order.State);
            Assert.Equal("Отменен", order.State.Name);
        }

        [Fact]
        public void CompletedState_AllTransitions_Throw()
        {
            var order = CreateOrder();
            order.StartPreparation();
            order.StartDelivery();
            order.Complete();

            Assert.IsType<CompletedState>(order.State);

            Assert.Throws<InvalidOperationException>(() => order.StartPreparation());
            Assert.Throws<InvalidOperationException>(() => order.StartDelivery());
            Assert.Throws<InvalidOperationException>(() => order.Complete());
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void CancelledState_AllTransitions_Throw()
        {
            var order = CreateOrder();
            order.Cancel();

            Assert.IsType<CancelledState>(order.State);

            Assert.Throws<InvalidOperationException>(() => order.StartPreparation());
            Assert.Throws<InvalidOperationException>(() => order.StartDelivery());
            Assert.Throws<InvalidOperationException>(() => order.Complete());
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }
        private static Order CreateStandardOrder() => Creator.CreateStandardOrder();
        private static SpecialOrder CreateSpecialOrder() => Creator.CreateSpecialOrder();

        [Fact]
        public void ChangeDeliveryAddress_Valid_ChangesProperty()
        {
            var order = CreateStandardOrder();

            order.ChangeDeliveryAddress("Новый адрес");

            Assert.Equal("Новый адрес", order.DeliveryAddress);
        }

        [Fact]
        public void AddItem_AddsToItems()
        {
            var order = CreateStandardOrder();
            var menuItem = new MenuItem(99, "Суши", 300m);

            order.AddItem(menuItem, 2, "Соевый соус отдельно");

            var lastItem = order.Items.Last();

            Assert.Equal(menuItem, lastItem.MenuItem);
            Assert.Equal(2, lastItem.Quantity);
            Assert.Equal("Соевый соус отдельно", lastItem.Comment);
        }

        [Fact]
        public void TotalPrice_UsesDefaultPriceCalculatorChain()
        {
            var customer = Creator.CreateCustomer();
            var builder = Creator.CreateOrderBuilder();
            builder.SetCustomer(customer);
            builder.SetDeliveryAddress("Адрес доставки");

            var menuItem = new MenuItem(1, "Пицца", 500m);
            builder.AddItem(menuItem, 2);

            var order = builder.GetResult();

            Assert.Equal(1180m, order.TotalPrice);
        }

        [Fact]
        public void SpecialOrder_HasAdditionalProperties()
        {
            var order = CreateSpecialOrder();

            Assert.True(order.IsExpress);
            Assert.True(order.NoContactDelivery);
            Assert.Equal("Аккуратно", order.SpecialInstructions);
        }

        [Fact]
        public void BasePriceRule_SumsItemPrices()
        {
            var customer = Creator.CreateCustomer();
            var builder = Creator.CreateOrderBuilder();

            builder.SetCustomer(customer);
            builder.SetDeliveryAddress("Адрес");

            var pizza = new MenuItem(1, "Пицца", 300m);
            var cola = new MenuItem(2, "Кола", 100m);

            builder.AddItem(pizza, 2);
            builder.AddItem(cola, 3);  

            var order = builder.GetResult();

            var rule   = new BasePriceRule();
            var result = rule.Apply(order, 0m);

            Assert.Equal(900m, result);
        }

        [Fact]
        public void DeliveryFeeRule_StandardOrder_UsesStandardFee()
        {
            var order = Creator.CreateStandardOrder();
            var rule = new DeliveryFeeRule(100m, 200m);

            var result = rule.Apply(order, 500m);

            Assert.Equal(600m, result);
        }

        [Fact]
        public void DeliveryFeeRule_ExpressSpecialOrder_UsesExpressFee()
        {
            var order = Creator.CreateSpecialOrder(isExpress: true);
            var rule = new DeliveryFeeRule(100m, 200m);

            var result = rule.Apply(order, 500m);

            Assert.Equal(700m, result);
        }

        [Fact]
        public void DiscountRule_Applies_WhenThresholdReached()
        {
            var order = Creator.CreateStandardOrder();
            var rule = new DiscountRule(1000m, 0.1m); 

            var result = rule.Apply(order, 1000m);

            Assert.Equal(900m, result);
        }

        [Fact]
        public void DiscountRule_NotApplied_WhenBelowThreshold()
        {
            var order = Creator.CreateStandardOrder();
            var rule = new DiscountRule(1000m, 0.1m);

            var result = rule.Apply(order, 999m);

            Assert.Equal(999m, result);
        }

        [Fact]
        public void TaxRule_AddsTax()
        {
            var order = Creator.CreateStandardOrder();
            var rule = new TaxRule(0.2m); 

            var result = rule.Apply(order, 1000m);

            Assert.Equal(1200m, result);
        }

        [Fact]
        public void PriceCalculator_DefaultChain_StandardOrder()
        {
            var customer = Creator.CreateCustomer();
            var builder = Creator.CreateOrderBuilder();

            builder.SetCustomer(customer);
            builder.SetDeliveryAddress("Адрес");
            var item = new MenuItem(1, "Пицца", 500m);
            builder.AddItem(item, 2); 

            var order = builder.GetResult();
            var calculator = new PriceCalculator();

            var price = calculator.Calculate(order);

            Assert.Equal(1180m, price);
        }

        [Fact]
        public void PriceCalculator_DefaultChain_ExpressSpecialOrder()
        {
            var customer = Creator.CreateCustomer();
            var builder = Creator.CreateOrderBuilder();

            builder.SetCustomer(customer);
            builder.SetDeliveryAddress("Адрес");
            var item = new MenuItem(1, "Пицца", 500m);
            builder.AddItem(item, 3);
            builder.SetExpressDelivery(true);

            var order = builder.GetResult(); 
            var calculator = new PriceCalculator();

            var price = calculator.Calculate(order);

            Assert.Equal(1820m, price);
        }

    }

}
