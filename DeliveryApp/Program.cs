using System;
using System.Text;

namespace DeliveryApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var service = OrderService.Instance;

            Console.WriteLine("=== Меню ===");
            foreach (var item in service.Menu)
            {
                Console.WriteLine($"{item.Id}. {item.Name} — {item.Price} рублей");
            }
            Console.WriteLine(new string('=', 80));

            var standardOrder = service.CreateSampleStandardOrder();
            var specialOrder  = service.CreateSampleSpecialOrder();

            Console.WriteLine("=== Созданные заказы ===");
            service.PrintOrderInfo(standardOrder);
            service.PrintOrderInfo(specialOrder);
            Console.WriteLine(new string('=', 80));

            Console.WriteLine("=== Жизненный цикл стандартного заказа ===");
            service.RunOrderLifecycle(standardOrder);

            Console.WriteLine("=== Жизненный цикл специального заказа ===");
            service.RunOrderLifecycle(specialOrder);

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}