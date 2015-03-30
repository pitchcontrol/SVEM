using System;
using System.IO;
using System.ServiceModel;

namespace SMEVService
{
    /// <summary>
    /// ���� ��������
    /// </summary>
    [MessageContract]
    public class AppDocument
    {
    }

    /// <summary>
    /// ���� ��������. ������������ ���� BinaryData ���� Reference+DigestValue
    /// </summary>
    [MessageContract]
    public class AppDocumentType
    {
        public AppDocumentType()
        {
            //������������ ��������� �� ������ �� �� � ������� XML c ������ req_GUID.xml �� �������� �� �����-��������.
            RequestCode = string.Format("req_{0:D}", Guid.NewGuid());
        }

        /// <summary>
        /// ��� ���������
        /// </summary>
        public string RequestCode { get; set; }
        /// <summary>
        /// ������� ��������
        /// </summary>
        public byte[] BinaryData { get; set; }
        /// <summary>
        /// ������ �� ��������
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// ���-��� ��������
        /// </summary>
        public Stream DigestValue { get; set; }
    }

}