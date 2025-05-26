using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace LightHTMLFlyweightExample
{
    // Абстрактний клас, як раніше
    public abstract class LightNode
    {
        public LightElementNode? Parent { get; set; }
        public abstract string OuterHTML { get; }
        public abstract string InnerHTML { get; }
    }

    public class LightTextNode : LightNode
    {
        public string Text { get; set; }
        public LightTextNode(string text) => Text = text;
        public override string OuterHTML => Text;
        public override string InnerHTML => Text;
    }

    // enum як раніше
    public enum DisplayType { Block, Inline }
    public enum TagClosureType { SelfClosing, Normal }

    // Flyweight для тегів і css класів
    public class TagFlyweight
    {
        private static readonly Dictionary<string, TagFlyweight> _cache = new Dictionary<string, TagFlyweight>();

        public string TagName { get; }
        public DisplayType Display { get; }
        public TagClosureType ClosureType { get; }

        private TagFlyweight(string tagName, DisplayType display, TagClosureType closureType)
        {
            TagName = tagName;
            Display = display;
            ClosureType = closureType;
        }

        public static TagFlyweight Get(string tagName, DisplayType display, TagClosureType closureType)
        {
            string key = $"{tagName}_{display}_{closureType}";
            if (!_cache.TryGetValue(key, out var tag))
            {
                tag = new TagFlyweight(tagName, display, closureType);
                _cache[key] = tag;
            }
            return tag;
        }
    }

    public class LightElementNode : LightNode
    {
        // Замість окремих полів - flyweight тег
        public TagFlyweight TagInfo { get; }
        public List<string> CssClasses { get; } = new List<string>();
        public List<LightNode> Children { get; } = new List<LightNode>();

        public LightElementNode(TagFlyweight tagInfo)
        {
            TagInfo = tagInfo;
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
                    sb.Append(child.OuterHTML);
                return sb.ToString();
            }
        }

        public override string OuterHTML
        {
            get
            {
                var tagName = TagInfo.TagName;
                StringBuilder sb = new StringBuilder();
                sb.Append($"<{tagName}");
                if (CssClasses.Count > 0)
                {
                    sb.Append(" class=\"");
                    sb.Append(string.Join(" ", CssClasses));
                    sb.Append("\"");
                }

                if (TagInfo.ClosureType == TagClosureType.SelfClosing)
                {
                    sb.Append(" />");
                    return sb.ToString();
                }
                else
                {
                    sb.Append(">");
                    sb.Append(InnerHTML);
                    sb.Append($"</{tagName}>");
                    return sb.ToString();
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // Текст книги (приклад)
            string[] bookLines = new string[]
            {
                "Назва книги: Пригоди",
                "Автор: Іван Франко",
                "  Цей рядок починається з пробілу, тому blockquote.",
                "Це звичайний параграф тексту книги, який довший за 20 символів.",
                "Коротко.",
                "  Ще один blockquote для цитати.",
                "Заключний параграф тексту."
            };

            // Кореневий елемент сторінки
            var root = new LightElementNode(TagFlyweight.Get("div", DisplayType.Block, TagClosureType.Normal));
            root.CssClasses.Add("book");

            foreach (var line in bookLines)
            {
                LightElementNode node;

                if (root.ChildCount == 0)
                {
                    // Перший рядок -> h1
                    node = new LightElementNode(TagFlyweight.Get("h1", DisplayType.Block, TagClosureType.Normal));
                }
                else if (line.StartsWith(" "))
                {
                    // Починається з пробілу -> blockquote
                    node = new LightElementNode(TagFlyweight.Get("blockquote", DisplayType.Block, TagClosureType.Normal));
                }
                else if (line.Length < 20)
                {
                    // Менше 20 символів -> h2
                    node = new LightElementNode(TagFlyweight.Get("h2", DisplayType.Block, TagClosureType.Normal));
                }
                else
                {
                    // Інакше p
                    node = new LightElementNode(TagFlyweight.Get("p", DisplayType.Block, TagClosureType.Normal));
                }

                node.AddChild(new LightTextNode(line.Trim()));
                root.AddChild(node);
            }

            // Вивід у консоль
            Console.WriteLine("=== HTML верстка книги ===");
            Console.WriteLine(root.OuterHTML);

            // Крок 2: Виміряти пам’ять (орієнтовно) для дерева
            long approxSizeBytes = EstimateObjectSize(root);
            Console.WriteLine($"\nПриблизний розмір дерева в пам’яті: {approxSizeBytes} байт");

        }

        // Проста оцінка пам’яті об’єктів (базово, в реальному випадку краще профайлити)
        static long EstimateObjectSize(object obj)
        {
            // Тут робимо дуже просту оцінку: підрахунок рядків і символів
            if (obj is LightElementNode el)
            {
                long size = 100; // орієнтовний базовий розмір об'єкта
                foreach (var cls in el.CssClasses)
                    size += 2 * cls.Length;
                foreach (var child in el.Children)
                    size += EstimateObjectSize(child);
                return size;
            }
            else if (obj is LightTextNode textNode)
            {
                return 20 + 2 * textNode.Text.Length;
            }
            return 0;
        }
    }
}
