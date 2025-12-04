namespace DeliveryApp
{
    public class DeliveryFeeRule : PriceRuleBase
    {
        private readonly decimal _standardFee;
        private readonly decimal _expressFee;

        public DeliveryFeeRule(decimal standardFee, decimal expressFee)
        {
            _standardFee = standardFee;
            _expressFee = expressFee;
        }

        protected override decimal ApplyCore(Order order, decimal currentPrice)
        {
            decimal fee;

            if (order is SpecialOrder special && special.IsExpress)
                fee = _expressFee;
            else
                fee = _standardFee;

            return currentPrice + fee;
        }
    }
}