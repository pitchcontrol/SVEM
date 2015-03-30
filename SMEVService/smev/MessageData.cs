using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// ”нифицированный служебный блок-обертка данных (smev:MessageData) сообщени€ в —ћЁ¬ €вл€етс€ группирующим элементом, 
    /// содержащим внутри себ€ унифицированные служебные блоки: блок структурированных сведений (в соответствии с требовани€ми поставщика) и блок вложений
    /// </summary>
    [MessageContract]
    public class MessageData : BaseElement
    {
        /// <summary>
        /// блок структурированных сведений в соответствии с требовани€ми поставщика
        /// </summary>
        public AppData AppData { get; set; }

        /// <summary>
        /// блок вложений 
        /// </summary>
        public AppDocumentType AppDocument { get; set; }

    }
}