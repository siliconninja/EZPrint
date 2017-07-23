using System.Runtime.Serialization;

namespace GoogleCloudPrintServices.DTO
{
    [DataContract]
    public class CloudPrintJob
    {
        [DataMember(Order = 0)]
        public bool success { get; set; }

        [DataMember(Order = 1)]
        public string message { get; set; }
    }
}