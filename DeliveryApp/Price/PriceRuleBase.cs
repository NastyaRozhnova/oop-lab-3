namespace DeliveryApp
{
    public abstract class PriceRuleBase : IPriceRule
    {
        private IPriceRule? _next;

        public IPriceRule SetNext(IPriceRule next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            _next = next;
            
            return next;
        }

        public decimal Apply(Order order, decimal currentPrice)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var newPrice = ApplyCore(order, currentPrice);

            if (_next == null)
                return newPrice;

            return _next.Apply(order, newPrice);
        }
        
        protected abstract decimal ApplyCore(Order order, decimal currentPrice);
    }
}