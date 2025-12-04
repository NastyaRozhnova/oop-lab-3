namespace DeliveryApp
{
    public interface IPriceCalculator
    {
        decimal Calculate(Order order);
    }
}