namespace DeliveryApp
{
    public interface IPriceRule
    {
        IPriceRule SetNext(IPriceRule next);
        
        decimal Apply(Order order, decimal currentPrice);
    }
}