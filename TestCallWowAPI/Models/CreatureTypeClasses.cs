using System;
using System.Collections.Generic;
using System.Text;

namespace TestCallWowAPI.Models
{
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
