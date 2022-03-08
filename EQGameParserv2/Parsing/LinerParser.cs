using EQGameParserv2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQGameParserv2.Parsing
{
    public class LinerParser
    {
        static CultureInfo provider = CultureInfo.InvariantCulture;
        private static readonly Dictionary<string, string> HitTypeMap = new Dictionary<string, string>
        {
        { "bash", "bashes" }, { "backstab", "backstabs" }, { "bite", "bites" }, { "claw", "claws" }, { "crush", "crushes" },
        { "frenzy", "frenzies" }, { "gore", "gores" }, { "hit", "hits" }, { "kick", "kicks" }, { "learn", "learns" },
        { "maul", "mauls" }, { "punch", "punches" }, { "pierce", "pierces" }, { "rend", "rends" }, { "shoot", "shoots" },
        { "slash", "slashes" }, { "slam", "slams" }, { "slice", "slices" }, { "smash", "smashes" }, { "sting", "stings" },
        { "strike", "strikes" }, { "sweep", "sweeps" }
        };
        private static readonly Dictionary<string, string> HitTypeMapReverse = new Dictionary<string, string>
        {
        { "bashes", "bash" }, { "backstabs", "backstab" }, { "bites", "bite" }, { "claws", "claw" }, { "crushes", "crush" },
        { "frenzies", "frenzy" }, { "gores", "gore" }, { "hits", "hit" }, { "kicks", "kick" }, { "learns", "learn" },
        { "mauls", "maul" }, { "punches", "punch" }, { "pierces", "pierce" }, { "rends", "rend" }, { "shoots", "shoot" },
        { "slashes", "slash" }, { "slams", "slam" }, { "slices", "slice" }, { "smashes", "smash" }, { "stings", "sting" },
        { "strikes", "strike" }, { "sweeps", "sweep" }
        };


        private static List<String> validHitTypes = new List<string> { " hit ", " hits ", " was hit "," bashes ", " bash "," strikes ", " strike ", " punches ", " punch ", " pierces ", " pierce "," kicks ", " kick "," claws ", " claw "," bites ", " bite "," crushes ", " crush "," slashes "," slash "," backstabs "," backstab "
            ," frenzies ", " frenzy"," gores ", " gore ", " mauls ", " maul ", " rends " , " shoots ", " shoot ", " slams ", " slam ", " slices ", " smashes ", " smash ", " stings "," sweeps ", " sweep ", " static fist hits "};


        public static Boolean IsValidLine(string line)
        {
            /*
            [Tue Feb 22 10:44:31 2022] You are not currently assigned to an adventure.
            [Tue Feb 22 10:44:33 2022] A problem occurred when automatically launching the welcome screen on your last login.Auto - launching of the welcome screen has been disabled.You may invoke the welcome screen manually from the EQ menu.
            [Tue Feb 22 10:44:33 2022] Announcing now off
            [Tue Feb 22 10:44:34 2022] Welcome to EverQuest!
            [Tue Feb 22 10:44:34 2022] You have entered The Plane of Knowledge.
            [Tue Feb 22 10:44:34 2022] MESSAGE OF THE DAY: Welcome to Project Lazarus!Currently set in the Omens of War expansion.Please make sure you join us on discord @ https://discord.gg/qmSGhJq as most communications happen there first.  You can also find more information at lazaruseq.com  Make sure you are using the ROF2 client (see getting started on discord/wiki), and grab the server patch files from our discord.  The Titanium client (p99 install) is not supported.  Have fun and enjoy!  -Dhaymion
            [Tue Feb 22 10:44:39 2022] GUILD MOTD: Devoure - Welcome to Tenacity!Please remember to sign up for raids as the sign - ups become available! Guid bank is working now!feel free to load it up!
            [Tue Feb 22 10:44:39 2022] Channels: 1 = General(483), 2 = Shadowknight(31), 3 = Planes(280)
           */
            if (line.Contains("] There is ")) return false;
            if (line.Contains(" says '")) return false;
            if (line.Contains("] You feel different.")) return false;
            if (line.Contains("] You are ")) return false;
            if (line.Contains(" says out of character, '")) return false;
            if (line.Contains(" tells you, '")) return false;
            if (line.Contains(" your guild, '")) return false;
            if (line.Contains(" your group, '")) return false;
            if (line.Contains(" your party, '")) return false;
            if (line.Contains(" your raid, '")) return false;
            if (line.Contains(" tells the guild, '")) return false;
            if (line.Contains(" tells the group, '")) return false;
            if (line.Contains(" You feel replenished.")) return false;
            if (line.Contains("]] You told ")) return false;
            if (line.Contains("] MESSAGE OF THE DAY:")) return false;
            if (line.Contains("] GUILD MOTD:")) return false;
            if (line.Contains("] Channels:")) return false;
            if (line.Contains("] You are not currently assigned to an adventure.")) return false;
            if (line.Contains("] Announcing ")) return false;
            if (line.Contains("] Welcome to EverQuest!")) return false;
            if (line.Contains("] You have entered")) return false;
            if (line.Contains(" regards you ")) return false;
            if (line.Contains("] It will take ")) return false;
            if (line.Contains("] A missed note brings " )) return false;
            if (line.Contains("] The Universal Chat service")) return false;
           
            return true;
        }
        public static Boolean ParseDamage(string line, string previousLine, Dictionary<string, Character> data, ref Int64 timeBatchID, ref DateTime lastTimeForDamage)
        {

            //no damage
            if (line.EndsWith("but do no damage.")) return false;

            //damage you have recieved
            if (line.Contains("You have taken") || line.Contains("You were")) return false;
            
            //damage shields
            if (line.Contains("was hit by non-melee ") || line.Contains("You hit ")) return false;
            
            //heals or mana stones
            if (line.Contains(" healed ") || line.Contains(" himself ")) return false;
            if (line.Contains(" herself ") || line.Contains(" itself ")) return false;
         
            //Below is a bunch of string manipulation BS :) 

            //if we don't match, assume this is not a damage record
            Boolean returnValue = false;
 
            //now lets process the line!
            if (line.EndsWith(" damage.") || line.EndsWith(" damage. (Rampage)")) //normal damage
            {  //jump to the 1st ] for the datetime skip
                Int32 index = line.IndexOf(']');
                //get to the start of the character name
                index += 2;

                //[Fri Mar 04 20:28:52 2022] Evinbard hit Temple Diabo Xi Va for 1120 points of non-melee damage.
                //[Fri Feb 25 21:57:07 2022] Ture hits Jaykits for 2877 points of damage. (Rampage)
                Int32 indexOfHitType = -1;
                foreach (var hittype in validHitTypes)
                {
                    indexOfHitType = line.IndexOf(hittype);
                    if (indexOfHitType > 0) break;
                }
                if (indexOfHitType < 1)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR");
                    return false;
                }
                Int32 indexNameEnd = indexOfHitType;

                string name = line.Substring(index, indexNameEnd - index);

                if(name=="You")
                {
                    //change to the name configured
                    name = "Rekken";
                }

                if(name.EndsWith("`s pet"))
                {
                    name = name.Replace("`s pet", "");
                }

                //[Fri Mar 04 20:54:54 2022] Yona hit a girplan geomancer for 10605 points of non - melee damage.
                //normal damage
                index = indexNameEnd + 1;
                string damageType;
                Int32 indexDamageTypeEnd = line.IndexOf(' ', index);
                damageType = line.Substring(index, indexDamageTypeEnd - index);

                //special case of the use of 'on a' insteead of just 'a'
                if (damageType == "frenzies")
                {
                    line = line.Replace("frenzies on", "frenzies");
                }

                Int32 indexPointsOfDamage = line.IndexOf(" for ");
                string target = line.Substring(indexDamageTypeEnd + 1, indexPointsOfDamage - indexDamageTypeEnd - 1);

                if (target == name)
                {
                    //we are hitting ourselves, this isn't damage
                    return false;
                }
                string timeStamp = line.Substring(1, line.IndexOf("]") - 1);
                DateTime linetimeStamp = DateTime.ParseExact(timeStamp, "ddd MMM dd HH:mm:ss yyyy", provider);

               if (lastTimeForDamage != DateTime.MinValue && linetimeStamp.Subtract(lastTimeForDamage).TotalSeconds > 10)
                {
                    //new timestamp
                    timeBatchID++;
                    //returning true from this, will set the proper variable of has done damage.
                }
                lastTimeForDamage = linetimeStamp;

                DamageEntry tmpDamageEntry = new DamageEntry();
                if(damageType == "hit" && line.EndsWith("non-melee damage."))
                {
                    tmpDamageEntry.DamageType = "directdmg";
                }
                else
                {
                    tmpDamageEntry.DamageType = damageType;
                }
                    
                tmpDamageEntry.RawData = line;

                tmpDamageEntry.DamageMod = DamageModifier.Normal;

                if(previousLine.Contains("a critical blast!"))
                {
                    tmpDamageEntry.DamageMod = DamageModifier.Crit;
                }
                else if (previousLine.Contains("a critical hit!"))
                {
                    tmpDamageEntry.DamageMod = DamageModifier.Crit;
                }
                else if(previousLine.Contains("Crippling Blow!"))
                {
                    tmpDamageEntry.DamageMod = DamageModifier.Crippling;
                }

                tmpDamageEntry.Target = target;
                tmpDamageEntry.TimeStamp = linetimeStamp;
                indexPointsOfDamage += 5;
                Int32 indexPointsOfDamageEnd = line.IndexOf(" ", indexPointsOfDamage);
                string pointsOfDamage = line.Substring(indexPointsOfDamage, indexPointsOfDamageEnd - indexPointsOfDamage);
                tmpDamageEntry.Damage = Int64.Parse(pointsOfDamage);

                Character tmpCharacter = null;
                if (!data.ContainsKey(name))
                {
                    tmpCharacter = new Character();
                    tmpCharacter.Name = name;
                    data.Add(name, tmpCharacter);
                }
                else
                {
                    tmpCharacter = data[name];
                }

                if (!tmpCharacter.DamageEntriesByTimeID.ContainsKey(timeBatchID))
                {
                    tmpCharacter.DamageEntriesByTimeID.Add(timeBatchID, new List<DamageEntry>());

                }

                tmpCharacter.DamageEntriesByTimeID[timeBatchID].Add(tmpDamageEntry);

                
                return true;
            }
            else if(line.Contains(" damage from "))//Dot damage!
            {   //[Fri Mar 04 21:13:27 2022] Keldovan the Harrier has taken 13501 damage from Seeqi by Dread Pyre.

                //normal damage
                Int32 indexOfHitType = line.IndexOf(" has taken ");
                if (indexOfHitType < 1)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR");
                    return false;
                }
                Int32 indexNameStart = line.IndexOf(" damage from ");
                indexNameStart += 13;
                Int32 indexNameEnd = line.IndexOf(" ", indexNameStart + 1);
                string name = line.Substring(indexNameStart, indexNameEnd - indexNameStart);

              
                Int32 startOfTarget = line.IndexOf("]") +2;


                string target = line.Substring(startOfTarget, indexOfHitType - startOfTarget);

                if (target == name)
                {
                    //we are hitting ourselves, this isn't damage
                    return false;
                }
                string timeStamp = line.Substring(1, line.IndexOf("]") - 1);
                DateTime linetimeStamp = DateTime.ParseExact(timeStamp, "ddd MMM dd HH:mm:ss yyyy", provider);

                if (lastTimeForDamage != DateTime.MinValue && linetimeStamp.Subtract(lastTimeForDamage).TotalSeconds > 10)
                {
                    //new timestamp
                    timeBatchID++;
                    //returning true from this, will set the proper variable of has done damage.
                }
                lastTimeForDamage = linetimeStamp;

                DamageEntry tmpDamageEntry = new DamageEntry();
                tmpDamageEntry.DamageType = "dot";
                tmpDamageEntry.RawData = line;
                tmpDamageEntry.DamageMod = DamageModifier.Normal;
                tmpDamageEntry.Target = target;
                tmpDamageEntry.TimeStamp = linetimeStamp;

                Int32 indexPointsOfDamage = indexOfHitType;
                indexPointsOfDamage += 11;
                Int32 indexPointsOfDamageTypeEnd = line.IndexOf(' ', indexPointsOfDamage);
                string pointsOfDamage = line.Substring(indexPointsOfDamage, indexPointsOfDamageTypeEnd - indexPointsOfDamage);
                tmpDamageEntry.Damage = Int64.Parse(pointsOfDamage);

                Character tmpCharacter = null;
                if (!data.ContainsKey(name))
                {
                    tmpCharacter = new Character();
                    tmpCharacter.Name = name;
                    data.Add(name, tmpCharacter);
                }
                else
                {
                    tmpCharacter = data[name];
                }

                if (!tmpCharacter.DamageEntriesByTimeID.ContainsKey(timeBatchID))
                {
                    tmpCharacter.DamageEntriesByTimeID.Add(timeBatchID, new List<DamageEntry>());

                }

                tmpCharacter.DamageEntriesByTimeID[timeBatchID].Add(tmpDamageEntry);

                return true;
            }
            return returnValue;
        }
        public static Boolean ParseEvent(string line, string previousLine, Dictionary<string, Character> data, ref Int64 timeBatchID, ref DateTime lastTimeForDamage)
        {
            //if we don't match, assume this is not a damage record
            Boolean returnValue = false;
            //[Fri Feb 25 21:24:57 2022] Jruhid begins to cast a spell. <Tempest Wind>
            //[Fri Feb 25 21:57:14 2022] Speedhax is enveloped in the fierce eye aura.
            //[Fri Feb 25 21:31:05 2022] Speedhax is consumed by the rhythm <Rhythm of the Night>
            //[Tue Feb 22 10:46:37 2022] Heeler's casting is interrupted!
            //[Fri Feb 25 21:23:41 2022] Ewiclip's body is consumed in rage.
            if (line.Contains("begins to cast a spell. <"))
            {
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfNameEnd = line.IndexOf(" begins to cast a spell.");

                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);

                Int32 indexOfEventNameStart = line.IndexOf("<");
                indexOfNameStart += 1;
                Int32 indexOfEventNameEnd = line.IndexOf(">");


                string eventName = line.Substring(indexOfEventNameStart, indexOfEventNameEnd - indexOfEventNameStart);

                return true;
            }
            else if(line.EndsWith(" was burned."))
            {
                //[Fri Feb 25 21:23:43 2022] Ewiclip was burned.

            }
            else if(line.Contains("casting is interrupted!"))
            {
                ////[Tue Feb 22 10:46:37 2022] Heeler's casting is interrupted!
                ///
                return true;
            }
            else if(line.Contains("  body is "))
            {
                ////[Fri Feb 25 21:23:41 2022] Ewiclip's body is consumed in rage.
                return true;
            }
            else if (line.Contains(" is "))
            {
                //this is the final check, as " is " is far too common and want the before checks to act as filters
                //[Fri Feb 25 21:57:14 2022] Speedhax is enveloped in the fierce eye aura.



                Int32 indexOfNameStart = line.IndexOf("]");
                

                indexOfNameStart += 2;

                Int32 indexOfIS = line.IndexOf(" ", indexOfNameStart);
                indexOfIS += 1;
                Int32 indexOfISEnd = line.IndexOf(" ", indexOfIS);

                string isValue = line.Substring(indexOfIS, indexOfISEnd - indexOfIS);

                if(isValue!="is")
                {
                    //this is not a valid is
                    return false;
                }

                
                Int32 indexOfNameEnd = line.IndexOf(" is ");
                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);
                Int32 indexOfEventNameStart = indexOfNameEnd + 4;
                Int32 indexOfEventNameEnd = line.IndexOf(".");


                string eventname = line.Substring(indexOfEventNameStart, indexOfEventNameEnd - indexOfEventNameStart);



                return true;
            }



            return returnValue;
        }


    }

}

