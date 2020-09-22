using System;
using System.Collections.Generic;
using System.Text;

namespace TestCallWowAPI.Models
{
    public class Key
    {
        public string href { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }
}
