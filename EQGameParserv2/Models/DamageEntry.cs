using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQGameParserv2.Models
{
    public class DamageEntry
    {
        public Int64 Damage { get; set; }
        public String DamageType { get; set; }
        public DamageModifier DamageMod { get; set; }
        public String Target { get; set; }
        public DateTime TimeStamp { get; set; }

        public string RawData { get; set;}


    }
}
