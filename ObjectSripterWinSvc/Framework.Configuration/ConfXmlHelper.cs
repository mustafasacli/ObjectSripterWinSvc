using System.Xml;

namespace Framework.Configuration
{
    internal static class ConfXmlHelper
    {
        /// <summary>
        /// Tries to Get Attribute Value
        /// </summary>
        /// <param name="attributeCollection"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string TryGetValue(XmlAttributeCollection attributeCollection, string attributeName)
        {
            string res = string.Empty;

            if (attributeCollection == null)
                return res;

            if (string.IsNullOrWhiteSpace(attributeName))
                return res;

            try
            {
                res = attributeCollection[attributeName].Value ?? string.Empty;
            }
            finally
            {
            }

            return res;
        }


        public static string TryGetSubNodeValue(XmlNode node, string subNodeName)
        {
            string res = string.Empty;

            if (node == null)
                return res;

            if (string.IsNullOrWhiteSpace(subNodeName))
                return res;

            try
            {
                XmlNode sbnode = node.SelectSingleNode(subNodeName);
                if (sbnode != null)
                    res = sbnode.InnerText ?? string.Empty;
            }
            finally
            {
            }

            return res;
        }

        public static bool IsValid(XmlNodeList nodeList)
        {
            bool isvld = false;

            if (nodeList != null) { isvld = nodeList.Count > 0; }

            return isvld;
        }
    }
}
