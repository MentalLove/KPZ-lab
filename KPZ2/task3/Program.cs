using System;
using System.Threading;

sealed class Authenticator
{
    private static readonly Lazy<Authenticator> instance = new Lazy<Authenticator>(() => new Authenticator(), true);

    public static Authenticator Instance => instance.Value;

    private Authenticator()
    {
        Console.WriteLine("Authenticator instance created.");
    }

    public void Authenticate(string username, string password)
    {
        Console.WriteLine($"Authenticating {username}...");
        Thread.Sleep(500);
        Console.WriteLine("Authentication complete.");
    }
}
