using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Xml;
using SMEVService.Helpers;

namespace SMEVService
{
    class SmevSignedXml : SignedXml
    {
        public SmevSignedXml(XmlDocument document)
            : base(document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument document, string idValue)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("s", Namespaces.SOAP);
            return document.SelectSingleNode("//*[@Id='" + idValue + "']", nsmgr) as XmlElement;
            //return document.GetElementsByTagName("Body", Namespaces.SOAP)[0] as XmlElement;
        }
    }
}