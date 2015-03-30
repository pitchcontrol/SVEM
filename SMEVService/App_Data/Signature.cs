using System.ServiceModel;

namespace SMEVService.App_Data
{
    /// <summary>
    /// ��� ���������, �� ���������� ��������, ��� ������������� ����� ����������������� ������, ������������ ����������� �������, �������������� � ������������ � �������� XMLDSig (XMLDSIG-CORE �XML-Signature Syntax and Processing�. 
    ///����������� � ��������� �� ������: http://www.w3.org/TR/2002/REC-xmldsig-core-20020212).
    /// </summary>
    [MessageContract]
    public class Signature
    {
        public SignedInfo SignedInfo { get; set; }
        
    }
    [MessageContract]
    public class SignedInfo
    {
        public string CanonicalizationMethod { get; set; }
        public string SignatureMethod { get; set; }
        public Reference Reference { get; set; }
    }
    [MessageContract]
    public class Reference
    {
        
    }
}