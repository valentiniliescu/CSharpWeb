namespace CSharpWeb
{
    public class HTMLButtonElement : HTMLElement
    {
        public event DomEventHandler Click
        {
            add { AddEventHandler("click", value); }
            remove { }
        }

        public HTMLButtonElement() : base("button")
        {

        }
    }
}
