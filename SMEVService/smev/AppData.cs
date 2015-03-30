using System;
using System.ServiceModel;
using System.Xml.Serialization;
using SMEVService.App_Data;

namespace SMEVService.smev
{
    /// <summary>
    /// ���� ����������������� �������� � ������������ � ������������ ���������� 
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
        /// ���� �� ��� ����� ���������������� �������
        /// </summary>
        public Signature Signature { get; set; }
    }
}