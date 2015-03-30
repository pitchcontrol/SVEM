using System.Runtime.Serialization;

namespace SMEVService
{
    [DataContract]
    public class Fault
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        
    }
}