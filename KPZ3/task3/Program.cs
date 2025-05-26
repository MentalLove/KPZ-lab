using System;

// Інтерфейс рендерера
public interface IRenderer
{
    void Render(string shapeName);
}

// Реалізація векторного рендерера
public class VectorRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as vectors");
    }
}

// Реалізація растрового рендерера
public class RasterRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as pixels");
    }
}

// Абстракція - Shape
public abstract class Shape
{
    protected IRenderer renderer;

    public Shape(IRenderer renderer)
    {
        this.renderer = renderer;
    }

    public abstract void Draw();
}

// Конкретні форми
public class Circle : Shape
{
    public Circle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Circle");
    }
}

public class Square : Shape
{
    public Square(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Square");
    }
}

public class Triangle : Shape
{
    public Triangle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Triangle");
    }
}

// Головна програма
class Program
{
    static void Main()
    {
        IRenderer vector = new VectorRenderer();
        IRenderer raster = new RasterRenderer();

        Shape shape1 = new Circle(vector);
        Shape shape2 = new Square(raster);
        Shape shape3 = new Triangle(vector);
        Shape shape4 = new Circle(raster);

        shape1.Draw(); // Circle as vectors
        shape2.Draw(); // Square as pixels
        shape3.Draw(); // Triangle as vectors
        shape4.Draw(); // Circle as pixels
    }
}
