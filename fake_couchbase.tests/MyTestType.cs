using System;
using Newtonsoft.Json;

namespace fake_couchbase.tests
{
    public class MyTestType
    {
        [JsonProperty("type")]
        public string Type
        {
            get { return "what I am"; }
        }

        [JsonProperty("id_1")]
        public Guid? Id1 { get; set; }

        [JsonProperty("id_2")]
        public Guid? Id2 { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is MyTestType))
                return false;

            var other = (MyTestType) obj;

            return this.Id1 == other.Id1 && this.Id2 == other.Id2;
        }
    }
}