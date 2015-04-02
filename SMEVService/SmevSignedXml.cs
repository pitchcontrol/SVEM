using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Xml;
using SMEVService.Helpers;

namespace SMEVService
{
    public class SmevSignedXml : SignedXml
    {
        public SmevSignedXml(XmlDocument document)
            : base(document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            //nsmgr.AddNamespace("s", Namespaces.SOAP);
            //nsmgr.AddNamespace("wsu", Namespaces.WSU);
            //return document.SelectSingleNode("//*[@id='" + idValue + "' or @Id='" + idValue + "']", nsmgr) as XmlElement;
            //return document.GetElementsByTagName("Body", Namespaces.SOAP)[0] as XmlElement;
            XmlNameTable myXmlNameTable = new NameTable();
            XmlNamespaceManager myNamespacemanager = new XmlNamespaceManager(myXmlNameTable);
            myNamespacemanager.AddNamespace("wsu", Namespaces.WSU);
            XmlNodeList lst = document.SelectNodes("//*[@wsu:Id='" + idValue + "' or @wsu:ID='" + idValue +
                    "' or @wsu:ID='" + idValue + "' or @Id='" + idValue +"']", myNamespacemanager);
            if (lst.Count != 1)
                return null;
            return (XmlElement)lst.Item(0);
        }
    }
}