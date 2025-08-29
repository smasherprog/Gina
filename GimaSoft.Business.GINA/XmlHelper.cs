using System.Xml;

namespace GimaSoft.Business.GINA
{
    public static class XmlHelper
    {
        public static string GetElementValue(this XmlElement element, string name, string defaultValue = null)
        {
            if (element == null)
            {
                return defaultValue ?? string.Empty;
            }

            var node = element[name];
            return node != null ? node.InnerText : (defaultValue ?? string.Empty);
        }


        public static string GetAttributeValue(XmlElement element, string attributeName)
        {
            if (element == null)
            {
                return null;
            }

            return element.HasAttribute(attributeName) ? element.GetAttribute(attributeName) : null;
        }
    }
}
