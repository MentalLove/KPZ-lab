using System;
using System.Collections.Generic;

public class Character
{
    public string Name { get; set; }
    public int Height { get; set; }
    public string BodyType { get; set; }
    public string HairColor { get; set; }
    public string EyeColor { get; set; }
    public string Clothes { get; set; }
    public List<string> Inventory { get; set; } = new List<string>();
    public List<string> GoodDeeds { get; set; } = new List<string>();
    public List<string> EvilDeeds { get; set; } = new List<string>();

    public override string ToString()
    {
        string info = $"Name: {Name}\nHeight: {Height} cm\nBody: {BodyType}\nHair: {HairColor}\nEyes: {EyeColor}\nClothes: {Clothes}";
        info += $"\nInventory: {string.Join(", ", Inventory)}";
        if (GoodDeeds.Count > 0)
            info += $"\nGood Deeds: {string.Join(", ", GoodDeeds)}";
        if (EvilDeeds.Count > 0)
            info += $"\nEvil Deeds: {string.Join(", ", EvilDeeds)}";
        return info;
    }
}

public interface ICharacterBuilder
{
    ICharacterBuilder SetName(string name);
    ICharacterBuilder SetHeight(int height);
    ICharacterBuilder SetBodyType(string bodyType);
    ICharacterBuilder SetHairColor(string hairColor);
    ICharacterBuilder SetEyeColor(string eyeColor);
    ICharacterBuilder SetClothes(string clothes);
    ICharacterBuilder AddInventory(string item);
    Character Build();
}

public class HeroBuilder : ICharacterBuilder
{
    private Character hero = new Character();

    public ICharacterBuilder SetName(string name) { hero.Name = name; return this; }
    public ICharacterBuilder SetHeight(int height) { hero.Height = height; return this; }
    public ICharacterBuilder SetBodyType(string bodyType) { hero.BodyType = bodyType; return this; }
    public ICharacterBuilder SetHairColor(string hairColor) { hero.HairColor = hairColor; return this; }
    public ICharacterBuilder SetEyeColor(string eyeColor) { hero.EyeColor = eyeColor; return this; }
    public ICharacterBuilder SetClothes(string clothes) { hero.Clothes = clothes; return this; }
    public ICharacterBuilder AddInventory(string item) { hero.Inventory.Add(item); return this; }
    public HeroBuilder AddGoodDeed(string deed) { hero.GoodDeeds.Add(deed); return this; }
    public Character Build() { return hero; }
}

public class EnemyBuilder : ICharacterBuilder
{
    private Character enemy = new Character();

    public ICharacterBuilder SetName(string name) { enemy.Name = name; return this; }
    public ICharacterBuilder SetHeight(int height) { enemy.Height = height; return this; }
    public ICharacterBuilder SetBodyType(string bodyType) { enemy.BodyType = bodyType; return this; }
    public ICharacterBuilder SetHairColor(string hairColor) { enemy.HairColor = hairColor; return this; }
    public ICharacterBuilder SetEyeColor(string eyeColor) { enemy.EyeColor = eyeColor; return this; }
    public ICharacterBuilder SetClothes(string clothes) { enemy.Clothes = clothes; return this; }
    public ICharacterBuilder AddInventory(string item) { enemy.Inventory.Add(item); return this; }
    public EnemyBuilder AddEvilDeed(string deed) { enemy.EvilDeeds.Add(deed); return this; }
    public Character Build() { return enemy; }
}

public class CharacterDirector
{
    public Character CreateHero(HeroBuilder builder)
    {
        return builder
            .SetName("Aria")
            .SetHeight(170)
            .SetBodyType("Athletic")
            .SetHairColor("Silver")
            .SetEyeColor("Blue")
            .SetClothes("Dragon Armor")
            .AddInventory("Sword of Light")
            .AddInventory("Healing Potion")
            .AddGoodDeed("Saved the village")
            .AddGoodDeed("Rescued the prince")
            .Build();
    }

    public Character CreateEnemy(EnemyBuilder builder)
    {
        return builder
            .SetName("Shadowfang")
            .SetHeight(190)
            .SetBodyType("Muscular")
            .SetHairColor("Black")
            .SetEyeColor("Red")
            .SetClothes("Dark Robe")
            .AddInventory("Poison Dagger")
            .AddInventory("Necromancer’s Book")
            .AddEvilDeed("Destroyed the kingdom")
            .AddEvilDeed("Cursed the forest")
            .Build();
    }
}

class Program
{
    static void Main()
    {
        var director = new CharacterDirector();

        var heroBuilder = new HeroBuilder();
        var enemyBuilder = new EnemyBuilder();

        Character hero = director.CreateHero(heroBuilder);
        Character enemy = director.CreateEnemy(enemyBuilder);

        Console.WriteLine("HERO:");
        Console.WriteLine(hero);
        Console.WriteLine("\nENEMY:");
        Console.WriteLine(enemy);
    }
}
