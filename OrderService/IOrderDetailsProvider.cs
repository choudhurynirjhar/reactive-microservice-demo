namespace OrderService
{
    public interface IOrderDetailsProvider
    {
        OrderDetail[] Get();
    }
}