using System;
using System.Collections.Generic;


public class Money
{
    private int wholePart;      
    private int fractionalPart; 

    public Money(int whole, int fractional)
    {
        if (fractional < 0 || fractional > 99)
            throw new ArgumentException("Дробова частина має бути від 0 до 99");
        wholePart = whole;
        fractionalPart = fractional;
    }

    public int WholePart
    {
        get => wholePart;
        set => wholePart = value;
    }

    public int FractionalPart
    {
        get => fractionalPart;
        set
        {
            if (value < 0 || value > 99)
                throw new ArgumentException("Дробова частина має бути від 0 до 99");
            fractionalPart = value;
        }
    }

    
    public override string ToString()
    {
        return $"{wholePart},{fractionalPart:D2}";
    }

    
    public void Decrease(Money amount)
    {
        int totalThis = wholePart * 100 + fractionalPart;
        int totalAmount = amount.wholePart * 100 + amount.fractionalPart;

        int result = totalThis - totalAmount;
        if (result < 0)
            throw new InvalidOperationException("Сума не може бути від'ємною");

        wholePart = result / 100;
        fractionalPart = result % 100;
    }
}


public class Product
{
    public string Name { get; set; }
    public string Category { get; set; } 
    public Money Price { get; private set; }

    public Product(string name, Money price, string category = "General")
    {
        Name = name;
        Price = price;
        Category = category;
    }

   
    public void DecreasePrice(Money discount)
    {
        Price.Decrease(discount);
    }

    public override string ToString()
    {
        return $"{Name} ({Category}), Price: {Price}";
    }
}


public class Warehouse
{
    public string ProductName { get; private set; }
    public string Unit { get; private set; }          
    public Money UnitPrice { get; private set; }       
    public int Quantity { get; private set; }         
    public DateTime LastDeliveryDate { get; private set; } 

    public Warehouse(string productName, string unit, Money unitPrice, int quantity, DateTime lastDelivery)
    {
        ProductName = productName;
        Unit = unit;
        UnitPrice = unitPrice;
        Quantity = quantity;
        LastDeliveryDate = lastDelivery;
    }

    public void AddStock(int quantity, DateTime deliveryDate)
    {
        Quantity += quantity;
        if (deliveryDate > LastDeliveryDate)
            LastDeliveryDate = deliveryDate;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity > Quantity)
            throw new InvalidOperationException("Недостатньо товару на складі");
        Quantity -= quantity;
    }

    public override string ToString()
    {
        return $"{ProductName} — {Quantity} {Unit}, Price per unit: {UnitPrice}, Last delivery: {LastDeliveryDate:d}";
    }
}

public class Reporting
{
    private List<Warehouse> warehouseItems;

    public Reporting()
    {
        warehouseItems = new List<Warehouse>();
    }

    public void RegisterArrival(Warehouse item, int quantity, DateTime deliveryDate)
    {
        var existing = warehouseItems.Find(w => w.ProductName == item.ProductName);
        if (existing == null)
        {
            warehouseItems.Add(new Warehouse(item.ProductName, item.Unit, item.UnitPrice, quantity, deliveryDate));
        }
        else
        {
            existing.AddStock(quantity, deliveryDate);
        }
    }

    public void RegisterShipment(string productName, int quantity)
    {
        var existing = warehouseItems.Find(w => w.ProductName == productName);
        if (existing == null)
            throw new InvalidOperationException("Товар не знайдено на складі");
        existing.RemoveStock(quantity);
    }

    public void PrintInventory()
    {
        Console.WriteLine("Звіт по інвентаризації:");
        foreach (var item in warehouseItems)
        {
            Console.WriteLine(item);
        }
    }
}

class Program
{
    static void Main()
    {
        Money price = new Money(100, 50); // 100,50
        Product apple = new Product("Apple", price, "Fruit");

        Console.WriteLine(apple);

        apple.DecreasePrice(new Money(10, 25)); // знижка 10,25
        Console.WriteLine("After discount: " + apple);

        Warehouse warehouse = new Warehouse(apple.Name, "kg", apple.Price, 200, DateTime.Now);
        Reporting report = new Reporting();

        report.RegisterArrival(warehouse, 200, DateTime.Now);
        report.PrintInventory();

        report.RegisterShipment("Apple", 50);
        report.PrintInventory();
    }
}
