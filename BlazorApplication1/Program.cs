using CSharpWeb;

namespace BlazorApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var div = new HTMLDivElement();
            HTMLElement.AppendChildToRoot(div);
            div.InnerText = "Hello world!";
        }
    }
}
