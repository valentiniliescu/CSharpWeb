using System.Collections.Generic;
using System.Runtime.InteropServices;
using MiniJSON;

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

        [DllImport(@"browser.js", CharSet = CharSet.Ansi)]
        private static extern void AddEventListener(string descriptor);

        private static int nextEventHandlerId = 0;
        private static readonly IDictionary<int, DomEventHandler> eventHandlersMap = new Dictionary<int, DomEventHandler>();

        public static void ExecuteEventHandler(string eventHandlerId)
        {
            eventHandlersMap[int.Parse(eventHandlerId)]();
        }

        private string _id;

        public string InnerText
        {
            get { return GetInnerText(_id); }
            set { SetInnerText(Json.Serialize(new Dictionary<string, string> { { "id", _id }, { "innerText", value } })); }
        }

        public HTMLElement(string tagName)
        {
            _id = CreateElement(tagName);
        }

        public void AppendChild(HTMLElement child)
        {
            AppendChild(Json.Serialize(new Dictionary<string, string> { { "id", child._id }, { "parentId", _id } }));
        }

        public static void AppendChildToRoot(HTMLElement child)
        {
            AppendChild(Json.Serialize(new Dictionary<string, string> { { "id", child._id } }));
        }

        protected void AddEventHandler(string eventName, DomEventHandler handler)
        {
            var eventHandlerId = ++nextEventHandlerId;
            eventHandlersMap.Add(eventHandlerId, handler);
            AddEventListener(Json.Serialize(new Dictionary<string, string> { { "id", _id }, { "eventHandlerId", eventHandlerId.ToString() }, { "eventName", eventName } }));
        }
    }
}
