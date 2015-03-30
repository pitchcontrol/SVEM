using System;
using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// Унифицированный служебный блок атрибутов сообщения СМЭВ предназначен для передачи атрибутивных сведений об 
    /// участниках и назначении сообщения в рамках информационного обмена через СМЭВ.
    /// </summary>
    [MessageContract(IsWrapped = true)]
    public class Message : BaseElement
    {
        

        public Message()
        {
            Date = DateTime.Now;
        }
        /// <summary>
        /// Данные о системе-инициаторе взаимодействия (Потребителе)
        /// </summary>
        public Sender Sender { get; set; }
        /// <summary>
        /// Данные о системе-получателе сообщения (Поставщике)
        /// </summary>
        public Recipient Recipient { get; set; }
        /// <summary>
        /// Данные о системе, инициировавшей цепочку из нескольких запросов-ответов, объединенных единым процессом в рамках взаимодействия
        /// </summary>
        public Originator Originator { get; set; }
        /// <summary>
        /// Данные о вызываемом сервисе 
        /// </summary>
        public Service Service { get; set; }
        /// <summary>
        /// Тип сообщения 
        /// </summary>
        public TypeCodeType TypeCode { get; set; }
        /// <summary>
        /// Статус сообщения
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// Дата создания сообщения
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Категория взаимодействия
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// Идентификатор сообщения-запроса, инициировавшего взаимодействие
        /// </summary>
        public string RequestIdRef { get; set; }

        /// <summary>
        /// Идентификатор сообщения-запроса, инициировавшего цепочку из нескольких запросов-ответов, объединенных единым процессом в рамках взаимодействия
        /// </summary>
        public string OriginRequestIdRef { get; set; }

        /// <summary>
        /// Код государственной услуги, в рамках оказания которой осуществляется информационный обмен
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// Номер дела в информационной системе-отправителе
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// Признак тестового взаимодействия
        /// </summary>
        public string TestMsg { get; set; }
        /// <summary>
        /// Код муниципального образования по ОКТМО
        /// </summary>
        public string OKTMO { get; set; }

    }
}