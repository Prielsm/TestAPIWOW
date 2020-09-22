using System;
using System.Collections.Generic;
using System.Text;

namespace TestCallWowAPI.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class CreatureDisplay
    {
        public int id { get; set; }
    }

    public class Name
    {
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string en_GB { get; set; }
        public string zh_TW { get; set; }
        public string ko_KR { get; set; }
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string es_ES { get; set; }
        public string zh_CN { get; set; }
        public string fr_FR { get; set; }
        public string de_DE { get; set; }
    }

    public class Type
    {
        public Name name { get; set; }
        public int id { get; set; }
    }

    public class Family
    {
        public Name name { get; set; }
        public int id { get; set; }
    }

    public class Data
    {
        public List<CreatureDisplay> creature_displays { get; set; }
        public bool is_tameable { get; set; }
        public Name name { get; set; }
        public int id { get; set; }
        public Type type { get; set; }
        public Family family { get; set; }
    }

    public class Result
    {
        public Key key { get; set; }
        public Data data { get; set; }
    }

    public class PaginatedResult
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int maxPageSize { get; set; }
        public int pageCount { get; set; }
        public List<Result> results { get; set; }
    }
}
