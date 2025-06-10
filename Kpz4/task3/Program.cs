using System;
using System.Collections.Generic;

namespace TextEditorWithMemento
{
    // Memento — зберігає стан документа
    class TextDocumentMemento
    {
        public string State { get; }

        public TextDocumentMemento(string state)
        {
            State = state;
        }
    }

    // TextDocument — документ
    class TextDocument
    {
        private string _content = "";

        public void Write(string text)
        {
            _content += text;
        }

        public void Erase()
        {
            _content = "";
        }

        public string GetContent()
        {
            return _content;
        }

        public TextDocumentMemento Save()
        {
            return new TextDocumentMemento(_content);
        }

        public void Restore(TextDocumentMemento memento)
        {
            _content = memento.State;
        }
    }

    // History — менеджер знімків
    class History
    {
        private Stack<TextDocumentMemento> _history = new Stack<TextDocumentMemento>();

        public void Save(TextDocumentMemento memento)
        {
            _history.Push(memento);
        }

        public TextDocumentMemento? Undo()
        {
            if (_history.Count > 0)
            {
                return _history.Pop();
            }
            return null;
        }
    }

    // TextEditor — редактор
    class TextEditor
    {
        private TextDocument _document = new TextDocument();
        private History _history = new History();

        public void Write(string text)
        {
            // Перед зміною зберігаємо стан
            _history.Save(_document.Save());
            _document.Write(text);
        }

        public void Erase()
        {
            _history.Save(_document.Save());
            _document.Erase();
        }

        public void Undo()
        {
            var memento = _history.Undo();
            if (memento != null)
            {
                _document.Restore(memento);
            }
            else
            {
                Console.WriteLine("Nothing to undo.");
            }
        }

        public void Print()
        {
            Console.WriteLine($"Current document content: \"{_document.GetContent()}\"");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TextEditor editor = new TextEditor();

            editor.Write("Hello ");
            editor.Print();

            editor.Write("World!");
            editor.Print();

            Console.WriteLine("Undo 1:");
            editor.Undo();
            editor.Print();

            Console.WriteLine("Undo 2:");
            editor.Undo();
            editor.Print();

            Console.WriteLine("Undo 3:");
            editor.Undo(); // буде "Nothing to undo"
            editor.Print();
        }
    }
}
