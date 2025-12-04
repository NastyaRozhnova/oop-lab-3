namespace DeliveryApp
{
    public class PriceCalculator : IPriceCalculator
    {
        private readonly IPriceRule _firstRule;

        public PriceCalculator()
        {
            _firstRule = BuildDefaultChain();
        }

        public decimal Calculate(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return _firstRule.Apply(order, 0m);
        }

        private static IPriceRule BuildDefaultChain()
        {
            var baseRule = new BasePriceRule();
            var discount = new DiscountRule(1000m, 0.1m);
            var tax = new TaxRule(0.2m);
            var delivery = new DeliveryFeeRule(100m, 200m);

            baseRule
                .SetNext(discount)
                .SetNext(tax)
                .SetNext(delivery);

            return baseRule;
        }
    }
}