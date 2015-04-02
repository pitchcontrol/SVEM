using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using SMEVService.Helpers;

namespace SMEVService.Interceptors
{
    /// <summary>
    /// Класс для перехвата сообщений на выходе
    /// </summary>
    public class MessageInspector : IDispatchMessageInspector
    {


        private XmlElement CreateSign(string uri, XmlDocument document)
        {
            SmevSignedXml signedXml = new SmevSignedXml(document);

            //Получаем сертификат
            X509Certificate2 certificate = KeyService.Certificate();

            signedXml.SigningKey = certificate.PrivateKey;

            Reference reference = new Reference
            {
                Uri = uri,
                DigestMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3411UrlObsolete
            };

            //Задаём алгоритм хэширования подписываемого узла - ГОСТ Р 34.11-94. Необходимо использовать устаревший идентификатор данного алгоритма, 
            //т.к. именно такой идентификатор используется в СМЭВ:
            //Преобразование (transform) для создания приложенной подписи в данном примере не нужно. 
            //Для СМЭВ необходимо добавить преобразование, приводящее подписываемый узел к каноническому виду по алгоритму http://www.w3.org/2001/10/xml-exc-c14n#:
            XmlDsigExcC14NTransform c14 = new XmlDsigExcC14NTransform();
            reference.AddTransform(c14);

            signedXml.AddReference(reference);

            //Задаём преобразование для приведения узла ds:SignedInfo к каноническому виду по алгоритму http://www.w3.org/2001/10/xml-exc-c14n# в соответствии с методическими рекомендациями СМЭВ:
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            //Задаём алгоритм подписи - ГОСТ Р 34.10-2001. Необходимо использовать устаревший идентификатор данного алгоритма, т.к. именно такой идентификатор используется в СМЭВ:
            signedXml.SignedInfo.SignatureMethod = CryptoPro.Sharpei.Xml.CPSignedXml.XmlDsigGost3410UrlObsolete;

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.KeyInfo = keyInfo;

            //Вычисляем
            signedXml.ComputeSignature();

            return signedXml.GetXml();
        }

        private void SignAppData(XmlDocument document)
        {
            X509Certificate2 certificate = KeyService.Certificate();
            string sign = "<ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig\">" +
                "<ds:SignedInfo>" +
                    "<ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>" +
                    "<ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#gostr34102001-gostr3411\"/>" +
                    "<ds:Reference URI=\"#AppData\">" +
                    "<ds:Transforms>" +
                        "<ds:Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\"/>" +
                        "<ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>" +
                    "</ds:Transforms>" +
                    "<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#gostr3411\"/>" +
                    "<ds:DigestValue>{0}</ds:DigestValue>" +
                    "</ds:Reference>" +
                    "</ds:SignedInfo>" +
                    "<ds:SignatureValue>{1}</ds:SignatureValue>" +
                    "<ds:KeyInfo>" +
                    "<ds:X509Data>" +
                        "<ds:X509Certificate>{2}</ds:X509Certificate>" +
                    "</ds:X509Data>" +
                    "</ds:KeyInfo>" +
                "</ds:Signature>";
            //Строим зашифрованнй блок для AppData
            XmlElement xmlDigitalSignature = CreateSign("#AppData", document);
            sign = string.Format(sign,
                xmlDigitalSignature.GetElementsByTagName("SignatureValue")[0].InnerText,
                xmlDigitalSignature.GetElementsByTagName("SignedInfo")[0].InnerText,
                Convert.ToBase64String(certificate.RawData)
                );

            XmlDocumentFragment xfrag = document.CreateDocumentFragment();
            xfrag.InnerXml = sign;
            var messageData = document.GetElementsByTagName("smev:AppData")[0];
            messageData.AppendChild(xfrag);
        }


        private XmlDocument Prepare(string xml)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Resource1.template);

            var tmplateBody = xmlDoc.GetElementsByTagName("s:Body");
            var tmplateHead = xmlDoc.GetElementsByTagName("s:Header");

            var xmlDoc2 = new XmlDocument();
            xmlDoc2.LoadXml(xml.Trim(new[] { '\uFEFF', '\u200B' }));
            //Основное тело запроса
            var subElem = xmlDoc2.GetElementsByTagName("Smev")[0];
            //Заголовок
            var subHead = xmlDoc2.GetElementsByTagName("h:Header")[0];
            
            var node = subElem.OuterXml;

            // стандартный десериализатор добавляет какую-то левую фигню
            node = node.Replace(@" xmlns=" + "\"" + "http://smev.gosuslugi.ru/rev120315" + "\"", "");

            node = node.Replace("xmlns:xsi=" + "\"" + "http://www.w3.org/2001/XMLSchema-instance" + "\" " + "xmlns:xsd=" + "\"" + "http://www.w3.org/2001/XMLSchema" + "\"",
                @" xmlns=" + "\"" + "http://smev.gosuslugi.ru/rev120315" + "\"");

            tmplateBody[0].InnerXml = node;

            //tmplateHead[0].InnerText = subHead.OuterXml;

            XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
            xfrag.InnerXml = subHead.OuterXml;

            //Добавляем заглушку с подписью в заголовок 
            tmplateHead[0].AppendChild(xfrag);

            xmlDoc2.PreserveWhitespace = false;

            return xmlDoc;
        }
        /// <summary>
        /// Добавляет в заголовок блок с подписью
        /// </summary>
        private System.ServiceModel.Channels.Message ChangeString2(System.ServiceModel.Channels.Message oldMessage)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms);
            oldMessage.WriteMessage(xw);
            xw.Flush();
            string body = Encoding.UTF8.GetString(ms.ToArray());
            xw.Close();

            XmlDocument doc = Prepare(body);
            //Получаем сертификат
            X509Certificate2 certificate = KeyService.Certificate();

            var appData = doc.SelectSingleNode("//*[@Id='AppData']");
            if (appData != null)
                SignAppData(doc);

            XmlElement xmlDigitalSignature = CreateSign("#body", doc);
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignatureValue")[0], true));
            doc.GetElementsByTagName("ds:Signature")[0].PrependChild(doc.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignedInfo")[0], true));
            doc.GetElementsByTagName("wsse:BinarySecurityToken")[0].InnerText = xmlDigitalSignature.GetElementsByTagName("X509Certificate")[0].InnerText;

            //Отладка
            File.WriteAllText(@"D:\Temp\sign.txt", doc.InnerXml);
            ms = new MemoryStream(Encoding.UTF8.GetBytes(doc.InnerXml));
            XmlDictionaryReader xdr = XmlDictionaryReader.CreateTextReader(ms, new XmlDictionaryReaderQuotas());
            System.ServiceModel.Channels.Message newMessage = System.ServiceModel.Channels.Message.CreateMessage(xdr, int.MaxValue, oldMessage.Version);
            newMessage.Properties.CopyProperties(oldMessage.Properties);
            return newMessage;
        }

        //BeforeSendReply is called after the response has been constructed by the service operation
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Debug.WriteLine("BeforeSendReply");
            reply = ChangeString2(reply);
        }

        //The AfterReceiveRequest method is fired after the message has been received but prior to invoking the service operation
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Debug.WriteLine("AfterReceiveRequest");
            //request = TraceMessage(request.CreateBufferedCopy(int.MaxValue));
            return null;
        }

    }
}