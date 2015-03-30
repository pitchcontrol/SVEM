using System;
using System.IO;
using System.ServiceModel;

namespace SMEVService
{
    /// <summary>
    /// Блок вложений
    /// </summary>
    [MessageContract]
    public class AppDocument
    {
    }

    /// <summary>
    /// Блок вложений. Используется либо BinaryData либо Reference+DigestValue
    /// </summary>
    [MessageContract]
    public class AppDocumentType
    {
        public AppDocumentType()
        {
            //Формирование обращения на сервис ИС ОВ в формате XML c именем req_GUID.xml со ссылками на файлы-вложения.
            RequestCode = string.Format("req_{0:D}", Guid.NewGuid());
        }

        /// <summary>
        /// Код заявления
        /// </summary>
        public string RequestCode { get; set; }
        /// <summary>
        /// Контент вложения
        /// </summary>
        public byte[] BinaryData { get; set; }
        /// <summary>
        /// Ссылка на вложение
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Хеш-код вложения
        /// </summary>
        public Stream DigestValue { get; set; }
    }

}