using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Solution.Task.PostBuild
{
    [Serializable, DataContract]
    public class ModManifest
    {
        [DataMember, JsonProperty]
        public string FriendlyName { get; set; }

        [DataMember, JsonProperty]
        public string Description { get; set; }

        [DataMember, JsonProperty]
        public string Author { get; set; }

        [DataMember, JsonProperty]
        public string Contact { get; set; }

        [DataMember, JsonProperty]
        public string Porter { get; set; }

        [DataMember, JsonProperty]
        public string PorterContact { get; set; }

        [DataMember, JsonProperty]
        public string ModuleFileName { get; set; }

        [DataMember, JsonProperty]
        public string[] Dependencies { get; set; }

        [DataMember, JsonProperty]
        public string[] RequiredGSLs { get; set; }

        [DataMember, JsonProperty]
        public bool SkipLoad { get; set; }

        [DataMember, JsonProperty]
        public int Priority { get; set; }
    }
}
