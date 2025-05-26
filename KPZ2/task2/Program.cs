using System;

interface ILaptop
{
    void ShowSpecs();
}

interface INetbook
{
    void ShowSpecs();
}

interface IEBook
{
    void ShowSpecs();
}

interface ISmartphone
{
    void ShowSpecs();
}

interface IDeviceFactory
{
    ILaptop CreateLaptop();
    INetbook CreateNetbook();
    IEBook CreateEBook();
    ISmartphone CreateSmartphone();
}

class IProneLaptop : ILaptop
{
    public void ShowSpecs() => Console.WriteLine("IProne Laptop: Retina Display, M2 chip");
}

class IProneNetbook : INetbook
{
    public void ShowSpecs() => Console.WriteLine("IProne Netbook: Lightweight, iOS");
}

class IProneEBook : IEBook
{
    public void ShowSpecs() => Console.WriteLine("IProne EBook: e-Ink, Apple Books");
}

class IProneSmartphone : ISmartphone
{
    public void ShowSpecs() => Console.WriteLine("IProne Smartphone: iOS, A15 chip");
}

class KiaomiLaptop : ILaptop
{
    public void ShowSpecs() => Console.WriteLine("Kiaomi Laptop: MIUI OS, Intel CPU");
}

class KiaomiNetbook : INetbook
{
    public void ShowSpecs() => Console.WriteLine("Kiaomi Netbook: Compact, MIUI NetOS");
}

class KiaomiEBook : IEBook
{
    public void ShowSpecs() => Console.WriteLine("Kiaomi EBook: Android, ePub reader");
}

class KiaomiSmartphone : ISmartphone
{
    public void ShowSpecs() => Console.WriteLine("Kiaomi Smartphone: Android, Snapdragon");
}

class BalaxyLaptop : ILaptop
{
    public void ShowSpecs() => Console.WriteLine("Balaxy Laptop: AMOLED screen, Exynos chip");
}

class BalaxyNetbook : INetbook
{
    public void ShowSpecs() => Console.WriteLine("Balaxy Netbook: Portable, OneUI");
}

class BalaxyEBook : IEBook
{
    public void ShowSpecs() => Console.WriteLine("Balaxy EBook: OLED display, Balaxy Reader");
}

class BalaxySmartphone : ISmartphone
{
    public void ShowSpecs() => Console.WriteLine("Balaxy Smartphone: OneUI, Snapdragon");
}

class IProneFactory : IDeviceFactory
{
    public ILaptop CreateLaptop() => new IProneLaptop();
    public INetbook CreateNetbook() => new IProneNetbook();
    public IEBook CreateEBook() => new IProneEBook();
    public ISmartphone CreateSmartphone() => new IProneSmartphone();
}

class KiaomiFactory : IDeviceFactory
{
    public ILaptop CreateLaptop() => new KiaomiLaptop();
    public INetbook CreateNetbook() => new KiaomiNetbook();
    public IEBook CreateEBook() => new KiaomiEBook();
    public ISmartphone CreateSmartphone() => new KiaomiSmartphone();
}

class BalaxyFactory : IDeviceFactory
{
    public ILaptop CreateLaptop() => new BalaxyLaptop();
    public INetbook CreateNetbook() => new BalaxyNetbook();
    public IEBook CreateEBook() => new BalaxyEBook();
    public ISmartphone CreateSmartphone() => new BalaxySmartphone();
}

class Program
{
    static void Main()
    {
        IDeviceFactory factory1 = new IProneFactory();
        IDeviceFactory factory2 = new KiaomiFactory();
        IDeviceFactory factory3 = new BalaxyFactory();

        var ipronePhone = factory1.CreateSmartphone();
        var kiaomiLaptop = factory2.CreateLaptop();
        var balaxyEBook = factory3.CreateEBook();

        ipronePhone.ShowSpecs();
        kiaomiLaptop.ShowSpecs();
        balaxyEBook.ShowSpecs();
    }
}
