using CSharpWeb;

namespace CSharpWebApplication1
{
    public class Program
    {
        static void Main()
        {
            var div = new HTMLDivElement();
            HTMLElement.AppendChildToRoot(div);
            div.InnerText = "Hello world!";

            var button = new HTMLButtonElement();
            HTMLElement.AppendChildToRoot(button);
            button.InnerText = "Click me!";

            button.Click += () => { div.InnerText = "Hello world again!"; };
        }
    }
}
