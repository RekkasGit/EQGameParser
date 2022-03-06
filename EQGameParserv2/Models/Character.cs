using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQGameParserv2.Models
{
    public class Character
    {
        public String Name { get; set; }
        public List<Character> Pets { get; set; }

        public Dictionary<Int64, List<DamageEntry>> DamageEntriesByTimeID = new Dictionary<long, List<DamageEntry>>();
       
    }
}
