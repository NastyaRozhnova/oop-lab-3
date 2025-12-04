namespace DeliveryApp
{
    public class DiscountRule : PriceRuleBase
    {
        private readonly decimal _threshold;
        private readonly decimal _percentage;

        public DiscountRule(decimal threshold, decimal percentage)
        {
            _threshold = threshold;
            _percentage = percentage;
        }

        protected override decimal ApplyCore(Order order, decimal currentPrice)
        {
            if (currentPrice >= _threshold)
            {
                var discount = currentPrice * _percentage;
                return currentPrice - discount;
            }

            return currentPrice;
        }
    }
}