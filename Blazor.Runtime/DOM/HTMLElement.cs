
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Blazor.Util;

namespace CSharpWeb
{
    public class HTMLElement
    {
        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        private static extern string CreateElement(string tagName);

        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        private static extern string GetInnerText(string id);

        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        private static extern void SetInnerText(string descriptor);

        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        private static extern void AppendChild(string descriptor);

        private string _id;

        public string InnerText
        {
            get { return GetInnerText(_id); }
            set { SetInnerText(JsonUtil.Serialize(new Dictionary<string, string> { { "id", _id }, { "innerText", value } })); }
        }

        public HTMLElement(string tagName)
        {
            _id = CreateElement(tagName);
        }

        public void AppendChild(HTMLElement child)
        {
            AppendChild(JsonUtil.Serialize(new Dictionary<string, string> { { "id", child._id }, { "parentId", _id } }));
        }

        public static void AppendChildToRoot(HTMLElement child)
        {
            AppendChild(JsonUtil.Serialize(new Dictionary<string, string> { { "id", child._id } }));
        }

        // TODO: on finalize, remove the id from elementsById table
    }

    public class HTMLDivElement : HTMLElement
    {
        public HTMLDivElement() : base("div")
        {

        }
    }
}
