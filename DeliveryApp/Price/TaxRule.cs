namespace DeliveryApp
{
    public class TaxRule : PriceRuleBase
    {
        private readonly decimal _taxRate;

        public TaxRule(decimal taxRate)
        {
            _taxRate = taxRate;
        }

        protected override decimal ApplyCore(Order order, decimal currentPrice)
        {
            var tax = currentPrice * _taxRate;
            return currentPrice + tax;
        }
    }
}