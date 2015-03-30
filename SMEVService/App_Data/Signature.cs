using System.ServiceModel;

namespace SMEVService.App_Data
{
    /// <summary>
    /// Для сообщений, не содержащих вложения, для удостоверения блока структурированных данных, используется электронная подпись, сформированная в соответствии с форматом XMLDSig (XMLDSIG-CORE «XML-Signature Syntax and Processing». 
    ///Опубликован в Интернете по адресу: http://www.w3.org/TR/2002/REC-xmldsig-core-20020212).
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