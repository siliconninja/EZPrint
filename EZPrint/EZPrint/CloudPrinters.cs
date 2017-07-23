using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleCloudPrintServices.DTO
{
    [DataContract]
    public class CloudPrinters
    {
        [DataMember(Order = 0)]
        public bool success { get; set; }

        [DataMember(Order = 1)]
        public List<CloudPrinter> printers { get; set; }
    }
}
