using CSharpWeb;

namespace CSharpWebApplication1
{
    public class Program
    {
        static void Main()
        {
            var rootDiv = new HTMLDivElement();
            HTMLElement.AppendChildToRoot(rootDiv);

            var div = new HTMLDivElement();
            rootDiv.AppendChild(div);
            div.InnerText = "Hello world!";

            var button = new HTMLButtonElement();
            rootDiv.AppendChild(button);
            button.InnerText = "Click me!";

            button.Click += () => { div.InnerText = "Hello world again!"; };
        }
    }
}
