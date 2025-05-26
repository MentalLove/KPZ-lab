using System;
using System.Collections.Generic;

abstract class Subscription
{
    public decimal MonthlyFee { get; protected set; }
    public int MinPeriodMonths { get; protected set; }
    public List<string> Channels { get; protected set; }
    public List<string> Features { get; protected set; }

    public virtual void ShowDetails()
    {
        Console.WriteLine($"{this.GetType().Name}");
        Console.WriteLine($"Monthly Fee: {MonthlyFee}");
        Console.WriteLine($"Minimum Period: {MinPeriodMonths} months");
        Console.WriteLine("Channels: " + string.Join(", ", Channels));
        Console.WriteLine("Features: " + string.Join(", ", Features));
        Console.WriteLine();
    }
}

class DomesticSubscription : Subscription
{
    public DomesticSubscription()
    {
        MonthlyFee = 10.99m;
        MinPeriodMonths = 3;
        Channels = new List<string> { "News", "Local Sports", "Family" };
        Features = new List<string> { "HD Streaming" };
    }
}

class EducationalSubscription : Subscription
{
    public EducationalSubscription()
    {
        MonthlyFee = 5.99m;
        MinPeriodMonths = 6;
        Channels = new List<string> { "Discovery", "History", "Science" };
        Features = new List<string> { "Offline Access", "Quiz Mode" };
    }
}

class PremiumSubscription : Subscription
{
    public PremiumSubscription()
    {
        MonthlyFee = 19.99m;
        MinPeriodMonths = 1;
        Channels = new List<string> { "All Access", "Cinema HD", "Live Sports" };
        Features = new List<string> { "4K Streaming", "Family Sharing", "Priority Support" };
    }
}

interface ISubscriptionCreator
{
    Subscription CreateSubscription(string type);
}

class WebSite : ISubscriptionCreator
{
    public Subscription CreateSubscription(string type)
    {
        return type switch
        {
            "Domestic" => new DomesticSubscription(),
            "Educational" => new EducationalSubscription(),
            "Premium" => new PremiumSubscription(),
            _ => throw new ArgumentException("Invalid type")
        };
    }
}

class MobileApp : ISubscriptionCreator
{
    public Subscription CreateSubscription(string type)
    {
        if (type == "Premium")
        {
            var premium = new PremiumSubscription();
            premium.Features.Add("Mobile Bonus Feature");
            return premium;
        }
        return new WebSite().CreateSubscription(type);
    }
}

class ManagerCall : ISubscriptionCreator
{
    public Subscription CreateSubscription(string type)
    {
        var sub = new WebSite().CreateSubscription(type);
        sub.MinPeriodMonths += 1;
        return sub;
    }
}

class Program
{
    static void Main()
    {
        ISubscriptionCreator website = new WebSite();
        ISubscriptionCreator app = new MobileApp();
        ISubscriptionCreator call = new ManagerCall();

        var sub1 = website.CreateSubscription("Domestic");
        var sub2 = app.CreateSubscription("Premium");
        var sub3 = call.CreateSubscription("Educational");

        sub1.ShowDetails();
        sub2.ShowDetails();
        sub3.ShowDetails();
    }
}
