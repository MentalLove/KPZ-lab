using System;
using System.Collections.Generic;
using System.Text;

namespace LightHTML
{
    // 2. Базовий клас для елементів розмітки
    public abstract class LightNode
    {
        public LightElementNode? Parent { get; set; }  // опціонально для ієрархії
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

    // 4. Текстовий вузол — містить лише текст
    public class LightTextNode : LightNode
    {
        public string Text { get; set; }

        public LightTextNode(string text)
        {
            Text = text;
        }

        public override string OuterHTML => Text;
        public override string InnerHTML => Text;
    }

    // Тип відображення (блочний чи рядковий)
    public enum DisplayType
    {
        Block,
        Inline
    }

    // Тип закриття тегу
    public enum TagClosureType
    {
        SelfClosing,       // <img />
        Normal             // <div>...</div>
    }

    // 3,5. Елемент з тегом, атрибутами, дітьми, відображенням і т.п.
    public class LightElementNode : LightNode
    {
        public string TagName { get; set; }
        public DisplayType Display { get; set; }
        public TagClosureType ClosureType { get; set; }
        public List<string> CssClasses { get; } = new List<string>();
        public List<LightNode> Children { get; } = new List<LightNode>();

        public LightElementNode(string tagName, DisplayType display, TagClosureType closureType)
        {
            TagName = tagName;
            Display = display;
            ClosureType = closureType;
        }

        public int ChildCount => Children.Count;

        public void AddChild(LightNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public override string InnerHTML
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var child in Children)
                {
                    sb.Append(child.OuterHTML);
                }
                return sb.ToString();
            }
        }

        public override string OuterHTML
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"<{TagName}");

                if (CssClasses.Count > 0)
                {
                    sb.Append(" class=\"");
                    sb.Append(string.Join(" ", CssClasses));
                    sb.Append("\"");
                }

                if (ClosureType == TagClosureType.SelfClosing)
                {
                    sb.Append(" />");
                    return sb.ToString();
                }
                else
                {
                    sb.Append(">");
                    sb.Append(InnerHTML);
                    sb.Append($"</{TagName}>");
                    return sb.ToString();
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // 6. Створимо приклад: простий ul список з 2 елементами

            var ul = new LightElementNode("ul", DisplayType.Block, TagClosureType.Normal);
            ul.CssClasses.Add("my-list");

            var li1 = new LightElementNode("li", DisplayType.Block, TagClosureType.Normal);
            li1.AddChild(new LightTextNode("Перший елемент"));

            var li2 = new LightElementNode("li", DisplayType.Block, TagClosureType.Normal);
            li2.AddChild(new LightTextNode("Другий елемент"));

            ul.AddChild(li1);
            ul.AddChild(li2);

            // Вивід outerHTML
            Console.WriteLine("OuterHTML ul:");
            Console.WriteLine(ul.OuterHTML);

            // Вивід innerHTML ul (тільки вміст без тегів ul)
            Console.WriteLine("\nInnerHTML ul:");
            Console.WriteLine(ul.InnerHTML);

            // Показуємо кількість дочірніх елементів
            Console.WriteLine($"\nКількість дочірніх елементів ul: {ul.ChildCount}");
        }
    }
}
