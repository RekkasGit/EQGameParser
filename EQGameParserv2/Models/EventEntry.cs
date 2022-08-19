using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQGameParserv2.Models
{
    public class EventEntry
    {
        public String EventName { get; set; }
        public string RawData { get; set; }
        public DateTime TimeStamp { get; set; }
        public String Target { get; set; }

    }
}
