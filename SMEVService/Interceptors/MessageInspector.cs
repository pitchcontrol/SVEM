﻿using System;
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

        /// <summary>
        /// Добавляет в заголовок блок с подписью
        /// </summary>
        private System.ServiceModel.Channels.Message ChangeString(System.ServiceModel.Channels.Message oldMessage)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(ms);
            oldMessage.WriteMessage(xw);
            xw.Flush();
            string body = Encoding.UTF8.GetString(ms.ToArray());
            xw.Close();

            string sign = "<wsse:Security s:actor=\"http://smev.gosuslugi.ru/actors/smev\" xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
             "<wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"{0}\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
             "</wsse:BinarySecurityToken>" +
                "<ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\">" +
                     "<ds:KeyInfo Id=\"KeyId-{0}\">" +
                         "<wsse:SecurityTokenReference wsu:Id=\"STRId-{0}\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" >" +
                            "<wsse:Reference URI=\"#CertId-{0}\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"/>" +
                         "</wsse:SecurityTokenReference>" +
                     "</ds:KeyInfo>" +
                 "</ds:Signature>" +
             "</wsse:Security>";

            //Получаем сертификат
            X509Certificate2 certificate = KeyService.Certificate();
            //Заполняем заглушку
            sign = string.Format(sign, certificate.SerialNumber.ToUpper());

            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.LoadXml(body.Remove(0, 1));
            //Находим заголовок
            var header = myXmlDocument.DocumentElement.ChildNodes[0];
            XmlDocumentFragment xfrag = myXmlDocument.CreateDocumentFragment();
            xfrag.InnerXml = sign;
            //Добавляем тэг ид что бы клас SmevSignedXml нашел его
            XmlNode bodyNode = myXmlDocument.GetElementsByTagName("Body", "http://schemas.xmlsoap.org/soap/envelope/")[0];
            ((XmlElement)bodyNode).SetAttribute("Id",/* "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd",*/ "body");


            //Проверим есть ли блок для плдписи структуированных данных
            //var appData = myXmlDocument.GetElementById("AppData");

            var appData = myXmlDocument.SelectSingleNode("//*[@Id='AppData']");
            if (appData != null)
                SignAppData(myXmlDocument);


            //Добавляем заглушку с подписью в заголовок 
            header.AppendChild(xfrag);

            //Строим зашифрованнй блок для body
            XmlElement xmlDigitalSignature = CreateSign("#body", myXmlDocument);

            //После вычисления подписи вместо добавления полученного узла ds:Signature в документ целиком необходимо взять лишь некоторые подузлы и вставить их в заготовленное место:
            myXmlDocument.GetElementsByTagName("ds:Signature")[0].PrependChild(
                    myXmlDocument.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignatureValue")[0], true));
            myXmlDocument.GetElementsByTagName("ds:Signature")[0].PrependChild(
                    myXmlDocument.ImportNode(xmlDigitalSignature.GetElementsByTagName("SignedInfo")[0], true));

            myXmlDocument.GetElementsByTagName("wsse:BinarySecurityToken")[0].InnerText =
            Convert.ToBase64String(certificate.RawData);


            //Пишем ответ
            ms = new MemoryStream(Encoding.UTF8.GetBytes(myXmlDocument.InnerXml));
            XmlDictionaryReader xdr = XmlDictionaryReader.CreateTextReader(ms, new XmlDictionaryReaderQuotas());
            System.ServiceModel.Channels.Message newMessage = System.ServiceModel.Channels.Message.CreateMessage(xdr, int.MaxValue, oldMessage.Version);
            newMessage.Properties.CopyProperties(oldMessage.Properties);
            return newMessage;
        }

        //BeforeSendReply is called after the response has been constructed by the service operation
        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Debug.WriteLine("BeforeSendReply");
            reply = ChangeString(reply);
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