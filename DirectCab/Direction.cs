using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace GeoDirections
{
    public static class Direction
    {
        public static string GetNodeValue(XmlNode nd, string tagName)
        {
            string value = "";
            if (nd == null)
                return "";
            else if (nd.SelectSingleNode(tagName) == null)
                return "";
            else
            {
                value = nd.SelectSingleNode(tagName).InnerXml;
                if (value.IndexOf("<![CDATA[") > -1)
                {
                    value = GetCData(value);
                }
                else
                {
                    value = Decrypt(nd.SelectSingleNode(tagName).InnerXml);
                }
                return value;
            }
        }

        public static string GetCData(string value)
        {
            if (value.IndexOf("<![CDATA[") > -1)
            {
                value = value.Substring(9);
                value = value.Substring(0, value.Length - 3);
                return value;
            }
            return "";
        }

        public static string Decrypt(string text)
        {
            return text.Replace("&amp;", "&").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&apos;", "'").Replace("&quot;", "\"");
        }

        public static string GetWebResponse(string url)
        {
            string text = "";
            Uri myUri = new Uri(url);
            StreamReader sr = null;

            try
            {
                WebRequest myWebRequest = WebRequest.Create(myUri);
                WebResponse myWebResponse = myWebRequest.GetResponse();
                sr = new StreamReader(myWebResponse.GetResponseStream(), Encoding.UTF8);
                text = sr.ReadToEnd();
                sr.Close();
                myWebResponse.Close();
                myWebRequest = null;
                myWebResponse = null;
                myUri = null;
            }
            catch (System.Exception e)
            {
                text = "<Error>" + e.Message + "</Error>";
            }
            return text;
        }

        public static int GetNumericalValue(XmlNode nd, string tagName, int defaultValue)
        {
            string value = GetNodeValue(nd, tagName);
            int result = defaultValue;
            if (Int32.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public struct DirectionEntry
        {
            public double start_lat;
            public double start_lng;
            public double end_lat;
            public double end_lng;
            public int distance;
            public int timeSpan;
            public int currency;
        }

    }
}
