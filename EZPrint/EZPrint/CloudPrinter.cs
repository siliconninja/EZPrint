using System.Runtime.Serialization;

namespace GoogleCloudPrintServices.DTO
{
    [DataContract]
    public class CloudPrinter
    {
        [DataMember(Order = 0)]
        public string id { get; set; }

        [DataMember(Order = 1)]
        public string name { get; set; }

        [DataMember(Order = 2)]
        public string description { get; set; }

        [DataMember(Order = 3)]
        public string proxy { get; set; }

        [DataMember(Order = 4)]
        public string status { get; set; }

        [DataMember(Order = 5)]
        public string capsHash { get; set; }

        [DataMember(Order = 6)]
        public string createTime { get; set; }

        [DataMember(Order = 7)]
        public string updateTime { get; set; }

        [DataMember(Order = 8)]
        public string accessTime { get; set; }

        [DataMember(Order = 9)]
        public bool confirmed { get; set; }

        [DataMember(Order = 10)]
        public int numberOfDocuments { get; set; }

        [DataMember(Order = 11)]
        public int numberOfPages { get; set; }
    }
}

