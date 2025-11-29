using System;
using System.Collections.Generic;

abstract class User
{
    protected string id;
    protected string name;
    protected string email;
    protected string address;
    protected string phone;
    protected string role;

    public User(string id, string name, string email, string address, string phone, string role)
    {
        this.id = id;
        this.name = name;
        this.email = email;
        this.address = address;
        this.phone = phone;
        this.role = role;
    }

    public abstract void Register();
    public abstract bool Login(string email, string name);
    public abstract void UpdateData(string newName, string newAddress, string newPhone);

    public string GetName() => name;
    public string GetRole() => role;
}

class Client : User
{
    private int loyaltyPoints = 0;
    private List<Order> myOrders = new List<Order>();

    public Client(string id, string name, string email, string address, string phone)
        : base(id, name, email, address, phone, "client") { }

    public void AddPoints(double amount)
    {
        loyaltyPoints += (int)(amount / 10);
    }

    public int GetPoints() => loyaltyPoints;

    public override void Register() { }

    public override bool Login(string email, string name)
    {
        return this.email == email && this.name == name;
    }

    public override void UpdateData(string newName, string newAddress, string newPhone)
    {
        name = newName;
        address = newAddress;
        phone = newPhone;
    }
}

class Admin : User
{
    private static List<string> logs = new List<string>();

    public Admin(string id, string name, string email, string address, string phone)
        : base(id, name, email, address, phone, "admin") { }

    public static void Log(string text)
    {
        logs.Add(text);
    }

    public static List<string> GetLogs() => logs;

    public override void Register() { }

    public override bool Login(string email, string name)
    {
        return this.email == email && this.name == name;
    }

    public override void UpdateData(string newName, string newAddress, string newPhone)
    {
        name = newName;
        address = newAddress;
        phone = newPhone;
    }
}

class Category
{
    public string name;
    public Category(string name) { this.name = name; }
}

class Product
{
    public string id;
    public string name;
    public string desc;
    public double price;
    public int stock;
    public Category category;

    public Product(string id, string name, string desc, double price, int stock, Category category)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.price = price;
        this.stock = stock;
        this.category = category;
    }
}

class Payment
{
    public string id;
    public string type;
    public double amount;
    public string status;

    public Payment(string id, string type, double amount)
    {
        this.id = id;
        this.type = type;
        this.amount = amount;
        this.status = "pending";
    }

    public void Process() { status = "paid"; }
    public void Refund() { status = "refunded"; }
}

class Delivery
{
    public string id;
    public string address;
    public string status;
    public string courier;

    public Delivery(string id, string address, string courier)
    {
        this.id = id;
        this.address = address;
        this.courier = courier;
        this.status = "processing";
    }

    public void Send() { status = "shipped"; }
    public void Track() { }
    public void Finish() { status = "delivered"; }
}

class Order
{
    public string id;
    public Client client;
    public List<Product> items = new List<Product>();
    public double total = 0;
    public string status = "created";
    public Payment payment;
    public Delivery delivery;

    public Order(string id, Client client)
    {
        this.id = id;
        this.client = client;
    }

    public void AddProduct(Product p)
    {
        items.Add(p);
        total += p.price;
    }

    public void ApplyPromo(string code)
    {
        if (code == "SALE10") total *= 0.9;
    }

    public void Place() { status = "placed"; }

    public void Cancel() { status = "cancelled"; }

    public void Pay(string method)
    {
        payment = new Payment(Guid.NewGuid().ToString(), method, total);
        payment.Process();
        status = "paid";
        client.AddPoints(total);
    }

    public void Ship()
    {
        delivery = new Delivery(Guid.NewGuid().ToString(), client.address, "Courier A");
        delivery.Send();
        status = "shipped";
    }
}

class ECommerceApp
{
    static List<Client> clients = new List<Client>();
    static List<Admin> admins = new List<Admin>();
    static List<Product> products = new List<Product>();

    static void Main()
    {
        InitData();
        MainMenu();
    }

    static void InitData()
    {
        admins.Add(new Admin("1", "admin", "admin@mail.com", "HQ", "000"));
        var c1 = new Category("Электроника");
        products.Add(new Product("p1", "Телефон", "Смартфон", 150000, 10, c1));
        products.Add(new Product("p2", "Ноутбук", "Игровой", 450000, 5, c1));
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("1 Регистрация клиента");
            Console.WriteLine("2 Вход клиента");
            Console.WriteLine("3 Вход администратора");
            Console.WriteLine("0 Выход");

            string choice = Console.ReadLine();

            if (choice == "1") RegisterClient();
            else if (choice == "2") ClientLogin();
            else if (choice == "3") AdminLogin();
            else if (choice == "0") return;
        }
    }

    static void RegisterClient()
    {
        Console.Write("Имя: "); string name = Console.ReadLine();
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("Адрес: "); string addr = Console.ReadLine();
        Console.Write("Телефон: "); string tel = Console.ReadLine();

        var c = new Client(Guid.NewGuid().ToString(), name, email, addr, tel);
        clients.Add(c);
        Console.WriteLine("Готово");
    }

    static void ClientLogin()
    {
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("Имя: "); string name = Console.ReadLine();

        foreach (var c in clients)
        {
            if (c.Login(email, name))
            {
                ClientMenu(c);
                return;
            }
        }
        Console.WriteLine("Неверные данные");
    }

    static void AdminLogin()
    {
        Console.Write("Email: "); string email = Console.ReadLine();
        Console.Write("Имя: "); string name = Console.ReadLine();

        foreach (var a in admins)
        {
            if (a.Login(email, name)) AdminMenu(a);
        }
    }

    static void ClientMenu(Client c)
    {
        while (true)
        {
            Console.WriteLine("1 Просмотр товаров");
            Console.WriteLine("2 Создать заказ");
            Console.WriteLine("3 Баллы: " + c.GetPoints());
            Console.WriteLine("0 Назад");

            string ch = Console.ReadLine();

            if (ch == "1") ShowProducts();
            else if (ch == "2") CreateOrder(c);
            else if (ch == "0") return;
        }
    }

    static void ShowProducts()
    {
        foreach (var p in products)
        {
            Console.WriteLine($"{p.id} | {p.name} | {p.price}");
        }
    }

    static void CreateOrder(Client c)
    {
        var o = new Order(Guid.NewGuid().ToString(), c);
        while (true)
        {
            ShowProducts();
            Console.Write("Введите ID товара или 0: ");
            string id = Console.ReadLine();
            if (id == "0") break;

            foreach (var p in products)
                if (p.id == id) o.AddProduct(p);
        }

        Console.Write("Промокод: ");
        o.ApplyPromo(Console.ReadLine());
        o.Place();

        Console.Write("Метод оплаты (card/wallet): ");
        o.Pay(Console.ReadLine());
        o.Ship();

        Console.WriteLine("Заказ оформлен. Итог: " + o.total);
    }

    static void AdminMenu(Admin a)
    {
        while (true)
        {
            Console.WriteLine("1 Логи");
            Console.WriteLine("2 Добавить товар");
            Console.WriteLine("0 Назад");

            string ch = Console.ReadLine();

            if (ch == "1")
                foreach (var log in Admin.GetLogs()) Console.WriteLine(log);
            else if (ch == "2") AddProduct(a);
            else if (ch == "0") return;
        }
    }

    static void AddProduct(Admin a)
    {
        Console.Write("Название: "); string n = Console.ReadLine();
        Console.Write("Описание: "); string d = Console.ReadLine();
        Console.Write("Цена: "); double p = double.Parse(Console.ReadLine());

        products.Add(new Product(Guid.NewGuid().ToString(), n, d, p, 10, new Category("Новое")));
        Admin.Log("Админ " + a.GetName() + " добавил товар " + n);
    }
}
