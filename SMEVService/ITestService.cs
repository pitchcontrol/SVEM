using System;
using System.ServiceModel;
using System.Xml.Serialization;
using SMEVService.Helpers;
using SMEVService.smev;

namespace SMEVService
{
    /// <summary>
    /// Тестовый сервис
    /// </summary>
    [ServiceContract(Namespace = "http://smev.gosuslugi.ru/rev150301")]
    [XmlSerializerFormat()]
    public interface ITestService
    {
        /// <summary>
        /// Получить текущею дату
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        Smev GetCurrentDate();
        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        Smev GetFile();

        /// <summary>
        /// Всегда ошибка
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        Smev GetError();
    }


    /// <summary>
    /// Инкапсулирует пакет для СМЭВ
    /// </summary>
    [MessageContract(IsWrapped = true)]
    public class Smev
    {

        /// <summary>
        /// Заголовок конверта
        /// </summary>
        [MessageHeader(Namespace = Namespaces.SVEM)]
        public Header Header { get; set; }

        // Body
        [MessageBodyMember(Namespace = Namespaces.SVEM)]
        public Message Message { get; set; }

        [MessageBodyMember(Namespace = Namespaces.SVEM)]
        public MessageData MessageData { get; set; }

        public Smev()
        {

        }
        [XmlNamespaceDeclarations()]
        public XmlSerializerNamespaces xmlsn
        {
            get
            {
                XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                xsn.Add("smev", "http://smev.gosuslugi.ru/rev120315");
                return xsn;
            }
            set
            {
                //Just provide an empty setter. 
            }
        }
    }

    [MessageContract()]
    public class Header
    {
        /// <summary>
        /// Уникальный идентификатор узла СМЭВ, состоящий из двух символов
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// Представляет собой уникальный идентификатор электронного сообщения (запроса или ответа) в рамках узла СМЭВ.
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// Дата и время создания сообщения в формате UTC 'yyyy-MM-dd'T'HH:mm:ss.SSSZ’
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Идентификатор, указывающий является ли электронное сообщение запросом от потребителя к поставщику или ответом от поставщика потребителю.
        /// </summary>
        public MessageClassType MessageClass { get; set; }

        [XmlNamespaceDeclarations()]
        public XmlSerializerNamespaces xmlsn
        {
            get
            {
                XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                xsn.Add("smev", "http://smev.gosuslugi.ru/rev120315");
                return xsn;
            }
            set
            {
                //Just provide an empty setter. 
            }
        }
    }
}
