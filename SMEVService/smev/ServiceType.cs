using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// Информация о целевом сервисе
    /// </summary>
    [MessageContract]
    public class ServiceType
    {
        public string Mnemonic { get; set; }
        public string Version { get; set; }
    }
}