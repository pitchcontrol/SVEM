using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Test
{
    class Program
    {
        private static void T()
        {
            XDocument document = XDocument.Load("../../XML/1.xml");

            XPathDocument x = new XPathDocument("../../XML/1.xml");
            XPathNavigator foo = x.CreateNavigator();
            foo.MoveToFollowing(XPathNodeType.Element);
            IDictionary<string, string> whatever = foo.GetNamespacesInScope(XmlNamespaceScope.All);


            var secutity = document.Root.Elements().ElementAt(0).Elements().FirstOrDefault(i => i.Name.LocalName == "Security");
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            var newSecurity = new XElement(wsse + "Security", secutity.Elements());
            // secutity.Name.

            //XName name = XName.Get("Security", "http://smev.gosuslugi.ru/rev120315");
            ////var elm = document.Element(name);
            //var elm = document.XPathSelectElement("//Security");

            //XNamespace s = "http://schemas.xmlsoap.org/soap/envelope/";
            //var add = new XElement(wsse + "Security",
            //    new XAttribute(XNamespace.Xmlns + "wsse", wsse),
            //    new XAttribute(s + "actor", "http://smev.gosuslugi.ru/actors/smev")
            //    );
        }
        static void Main(string[] args)
        {

            string sign = "<wsse:Security s:actor=\"http://smev.gosuslugi.ru/actors/smev\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
              "<wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"CertId-1E42AC2E0B920AAF70131180067340425\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"></wsse:BinarySecurityToken>" +
              "<ds:Signature Id=\"Signature-10\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\">" +
                  "<ds:SignedInfo>" +
                      "<ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>" +
                      "<ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411\"/>" +
                      "<ds:Reference URI=\"#sampleRequest\">" +
                          "<ds:Transforms>" +
                              "<ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>" +
                          "</ds:Transforms>" +
                          "<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#gostr3411\"/>" +
                          "<ds:DigestValue></ds:DigestValue>" +
                      "</ds:Reference>" +
                  "</ds:SignedInfo>" +
                  "<ds:SignatureValue>" +
                  "</ds:SignatureValue>" +
                  "<ds:KeyInfo Id=\"KeyId-1E42AC2E0B920AAF70131180067340426\">" +
                      "<wsse:SecurityTokenReference wsu:Id=\"STRId-1E42AC2E0B920AAF70131180067340427\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
                          "<wsse:Reference URI=\"#CertId-1E42AC2E0B920AAF70131180067340425\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"/>" +
                      "</wsse:SecurityTokenReference>" +
              "</ds:KeyInfo>" +
              "</ds:Signature>" +
              "</wsse:Security>";
            string file = File.ReadAllText("../../XML/1.xml");
            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.LoadXml(file);
            //XDocument document = XDocument.Load(file);
            var header = myXmlDocument.DocumentElement.ChildNodes[0];
            XmlDocumentFragment xfrag =myXmlDocument.CreateDocumentFragment();
            xfrag.InnerXml = sign;
            header.AppendChild(xfrag);
            //document.Root.Elements().ElementAt(0).Add(XElement.Parse(sign));

            Console.ReadKey();
        }
    }
}
