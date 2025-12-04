namespace DeliveryApp
{
    public class BasePriceRule : PriceRuleBase
    {
        protected override decimal ApplyCore(Order order, decimal currentPrice)
        {
            decimal itemsTotal = 0;

            foreach (var item in order.Items)
            {
                itemsTotal += item.GetTotalPrice();
            }

            return currentPrice + itemsTotal;
        }
    }
}