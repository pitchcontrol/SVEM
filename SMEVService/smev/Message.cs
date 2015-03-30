using System;
using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// ��������������� ��������� ���� ��������� ��������� ���� ������������ ��� �������� ������������ �������� �� 
    /// ���������� � ���������� ��������� � ������ ��������������� ������ ����� ����.
    /// </summary>
    [MessageContract(IsWrapped = true)]
    public class Message : BaseElement
    {
        

        public Message()
        {
            Date = DateTime.Now;
        }
        /// <summary>
        /// ������ � �������-���������� �������������� (�����������)
        /// </summary>
        public Sender Sender { get; set; }
        /// <summary>
        /// ������ � �������-���������� ��������� (����������)
        /// </summary>
        public Recipient Recipient { get; set; }
        /// <summary>
        /// ������ � �������, �������������� ������� �� ���������� ��������-�������, ������������ ������ ��������� � ������ ��������������
        /// </summary>
        public Originator Originator { get; set; }
        /// <summary>
        /// ������ � ���������� ������� 
        /// </summary>
        public Service Service { get; set; }
        /// <summary>
        /// ��� ��������� 
        /// </summary>
        public TypeCodeType TypeCode { get; set; }
        /// <summary>
        /// ������ ���������
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// ���� �������� ���������
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// ��������� ��������������
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        /// ������������� ���������-�������, ��������������� ��������������
        /// </summary>
        public string RequestIdRef { get; set; }

        /// <summary>
        /// ������������� ���������-�������, ��������������� ������� �� ���������� ��������-�������, ������������ ������ ��������� � ������ ��������������
        /// </summary>
        public string OriginRequestIdRef { get; set; }

        /// <summary>
        /// ��� ��������������� ������, � ������ �������� ������� �������������� �������������� �����
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// ����� ���� � �������������� �������-�����������
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// ������� ��������� ��������������
        /// </summary>
        public string TestMsg { get; set; }
        /// <summary>
        /// ��� �������������� ����������� �� �����
        /// </summary>
        public string OKTMO { get; set; }

    }
}