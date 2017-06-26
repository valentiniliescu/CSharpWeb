using CSharpWeb;

namespace BlazorApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var div = new HTMLDivElement();
            div.InnerHTML = "Hello world!";
        }
    }
}
