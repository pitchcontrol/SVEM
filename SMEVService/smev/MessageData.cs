using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// ��������������� ��������� ����-������� ������ (smev:MessageData) ��������� � ���� �������� ������������ ���������, 
    /// ���������� ������ ���� ��������������� ��������� �����: ���� ����������������� �������� (� ������������ � ������������ ����������) � ���� ��������
    /// </summary>
    [MessageContract]
    public class MessageData : BaseElement
    {
        /// <summary>
        /// ���� ����������������� �������� � ������������ � ������������ ����������
        /// </summary>
        public AppData AppData { get; set; }

        /// <summary>
        /// ���� �������� 
        /// </summary>
        public AppDocumentType AppDocument { get; set; }

    }
}