using System;
using System.ServiceModel;
using System.Xml.Serialization;
using SMEVService.App_Data;

namespace SMEVService.smev
{
    /// <summary>
    /// блок структурированных сведений в соответствии с требованиями поставщика 
    /// </summary>
    [MessageContract]
    public class AppData
    {
        public AppData()
        {
            Id = "AppData";
        }

        [XmlAttribute("Id")]
        public string Id { get; set; }

        public DateTime MyTime { get; set; }

        /// <summary>
        /// Блок ЭП для блока структумрованных сведенй
        /// </summary>
        public Signature Signature { get; set; }
    }
}