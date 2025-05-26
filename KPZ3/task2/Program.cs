using System;

// Базовий інтерфейс героя
public interface IHero
{
    string GetDescription();
    int GetPower();
}

// Реалізації героїв
public class Warrior : IHero
{
    public string GetDescription() => "Warrior";
    public int GetPower() => 50;
}

public class Mage : IHero
{
    public string GetDescription() => "Mage";
    public int GetPower() => 40;
}

public class Paladin : IHero
{
    public string GetDescription() => "Paladin";
    public int GetPower() => 45;
}

// Базовий клас декоратора
public abstract class HeroDecorator : IHero
{
    protected IHero _hero;

    public HeroDecorator(IHero hero)
    {
        _hero = hero;
    }

    public virtual string GetDescription() => _hero.GetDescription();
    public virtual int GetPower() => _hero.GetPower();
}

// Декоратор: Зброя
public class Weapon : HeroDecorator
{
    public Weapon(IHero hero) : base(hero) { }

    public override string GetDescription() => _hero.GetDescription() + " with Weapon";
    public override int GetPower() => _hero.GetPower() + 20;
}

// Декоратор: Одяг
public class Armor : HeroDecorator
{
    public Armor(IHero hero) : base(hero) { }

    public override string GetDescription() => _hero.GetDescription() + " in Armor";
    public override int GetPower() => _hero.GetPower() + 10;
}

// Декоратор: Артефакт
public class Artifact : HeroDecorator
{
    public Artifact(IHero hero) : base(hero) { }

    public override string GetDescription() => _hero.GetDescription() + " with Artifact";
    public override int GetPower() => _hero.GetPower() + 30;
}

// Тест програма
public class Program
{
    public static void Main()
    {
        IHero hero1 = new Warrior();
        hero1 = new Weapon(hero1);
        hero1 = new Armor(hero1);
        hero1 = new Artifact(hero1);
        Console.WriteLine($"{hero1.GetDescription()} | Power: {hero1.GetPower()}");

        IHero hero2 = new Mage();
        hero2 = new Artifact(hero2);
        hero2 = new Weapon(hero2);
        Console.WriteLine($"{hero2.GetDescription()} | Power: {hero2.GetPower()}");

        IHero hero3 = new Paladin();
        hero3 = new Armor(hero3);
        hero3 = new Artifact(hero3);
        hero3 = new Artifact(hero3); // ще один артефакт
        Console.WriteLine($"{hero3.GetDescription()} | Power: {hero3.GetPower()}");
    }
}
