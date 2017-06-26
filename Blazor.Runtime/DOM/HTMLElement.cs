
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Blazor.Util;

namespace CSharpWeb
{
    public class HTMLElement
    {
        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        protected static extern string CreateElement(string tagName);

        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        protected static extern string GetInnerHTML(string id);

        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        protected static extern string SetInnerHTML(string descriptor);

        private string _id;

        public string InnerHTML
        {
            get { return GetInnerHTML(_id); }
            set { SetInnerHTML(JsonUtil.Serialize(new Dictionary<string, string> { { "id", _id }, { "innerHTML", value } })); }
        }

        public HTMLElement(string tagName)
        {
            _id = CreateElement(tagName);
        }
    }

    public class HTMLDivElement : HTMLElement
    {
        public HTMLDivElement() : base("div")
        {
            
        }
    }
}
