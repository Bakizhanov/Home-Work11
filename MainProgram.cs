using System;
using System.Collections.Generic;

interface IOrderAPI
{
    void CreateOrder(string clientId, List<string> products);
    void CancelOrder(string orderId);
}

interface IDeliveryAPI
{
    void UpdateStatus(string orderId, string status);
    string Track(string orderId);
}

interface IWarehouseAPI
{
    bool ReserveProduct(string productId, int amount);
}

interface IRouteOptimizationAPI
{
    string GetOptimalRoute(string from, string to);
}

interface ICourierIntegrationAPI
{
    string GetCourierStatus(string trackingId);
}

interface INotificationAPI
{
    void NotifyClient(string clientId, string message);
}

interface IAnalyticsAPI
{
    void GenerateReport();
}

class OrderService : IOrderAPI
{
    Dictionary<string, string> orders = new Dictionary<string, string>();

    public void CreateOrder(string clientId, List<string> products)
    {
        orders.Add(Guid.NewGuid().ToString(), "created");
    }

    public void CancelOrder(string orderId)
    {
        orders[orderId] = "cancelled";
    }
}

class DeliveryTrackingService : IDeliveryAPI
{
    Dictionary<string, string> status = new Dictionary<string, string>();

    public void UpdateStatus(string orderId, string s)
    {
        status[orderId] = s;
    }

    public string Track(string orderId)
    {
        return status.ContainsKey(orderId) ? status[orderId] : "unknown";
    }
}

class WarehouseService : IWarehouseAPI
{
    Dictionary<string, int> stock = new Dictionary<string, int>();

    public bool ReserveProduct(string productId, int amount)
    {
        int qty = stock.ContainsKey(productId) ? stock[productId] : 0;
        if (qty >= amount)
        {
            stock[productId] = qty - amount;
            return true;
        }
        return false;
    }
}

class RouteOptimizationAdapter : IRouteOptimizationAPI
{
    public string GetOptimalRoute(string from, string to)
    {
        return "Route " + from + " -> " + to;
    }
}

class CourierIntegrationAdapter : ICourierIntegrationAPI
{
    public string GetCourierStatus(string trackingId)
    {
        return "In transit";
    }
}

class NotificationService : INotificationAPI
{
    public void NotifyClient(string clientId, string message)
    {
        Console.WriteLine("Notify " + clientId + ": " + message);
    }
}

class AnalyticsService : IAnalyticsAPI
{
    public void GenerateReport()
    {
        Console.WriteLine("Generating report");
    }
}

class MainProgram
{
    static void Main()
    {
        var orderService = new OrderService();
        var tracking = new DeliveryTrackingService();
        var warehouse = new WarehouseService();
        var routes = new RouteOptimizationAdapter();
        var courier = new CourierIntegrationAdapter();
        var notify = new NotificationService();
        var analytics = new AnalyticsService();

        while (true)
        {
            Console.WriteLine("1 Создать заказ");
            Console.WriteLine("2 Обновить доставку");
            Console.WriteLine("3 Отслеживать заказ");
            Console.WriteLine("4 Оптимальный маршрут");
            Console.WriteLine("5 Статус курьера");
            Console.WriteLine("6 Отчет");
            Console.WriteLine("0 Выход");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                orderService.CreateOrder("client1", new List<string>() { "p1", "p2" });
                notify.NotifyClient("client1", "Заказ создан");
            }
            else if (choice == "2")
            {
                Console.Write("ID заказа: ");
                string id = Console.ReadLine();
                Console.Write("Новый статус: ");
                tracking.UpdateStatus(id, Console.ReadLine());
            }
            else if (choice == "3")
            {
                Console.Write("ID заказа: ");
                string id = Console.ReadLine();
                Console.WriteLine(tracking.Track(id));
            }
            else if (choice == "4")
            {
                Console.WriteLine(routes.GetOptimalRoute("Warehouse A", "Client B"));
            }
            else if (choice == "5")
            {
                Console.WriteLine(courier.GetCourierStatus("track123"));
            }
            else if (choice == "6")
            {
                analytics.GenerateReport();
            }
            else if (choice == "0")
            {
                break;
            }
        }
    }
}
