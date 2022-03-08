using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQGameParserv2.Models
{
    public class TargetDPS
    {
        public Int64 TotalDamage { get { return Damage + SpellDamage; } }
        public Int64 Damage { get; set; }
        public Int64 DamageCount {get;set;}
        public Int64 CritDamage { get; set; }
        public Int64 SpellDamage { get; set; }
        public Int64 SpellDamageCount { get; set; }
        public Int64 CritDamageCount { get; set; }
        public Int64 SpellCritDamage { get; set; }
        public Int64 SpellCritDamageCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
