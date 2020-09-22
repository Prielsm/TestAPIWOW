using System;
using System.Collections.Generic;
using System.Text;

namespace TestCallWowAPI.Models
{
    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Key
    {
        public string href { get; set; }
    }

    public class CreatureType
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class RootCreatureType
    {
        public Links _links { get; set; }
        public List<CreatureType> creature_types { get; set; }
    }
}
