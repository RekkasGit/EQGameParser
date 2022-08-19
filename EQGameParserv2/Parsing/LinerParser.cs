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
            //perf opt, no need to do checks if we know this is a valid line.
            
            //note, this only works with 'EndsWith'
            if (line.EndsWith(" damage.") || line.EndsWith(" damage. (Rampage)")) return true;
            if (line.EndsWith(">")) return true;
            if (line.EndsWith(" is interrupted!")) return true;
            if (line.EndsWith(" spell fizzles!")) return true;
            //[Fri Feb 25 21:24:57 2022] Jruhid begins to cast a spell. <Tempest Wind>
            //[Fri Feb 25 21:57:14 2022] Speedhax is enveloped in the fierce eye aura.
            //[Fri Feb 25 21:31:05 2022] Speedhax is consumed by the rhythm <Rhythm of the Night>
            //[Tue Feb 22 10:46:37 2022] Heeler's casting is interrupted!
            //[Fri Feb 25 21:23:41 2022] Ewiclip's body is consumed in rage.
            //[Wed Mar 02 22:24:55 2022] Wenlowang's spell fizzles!
            //end perf opt

   
            if (line.EndsWith(" cannot invite yourself.")) return false; //invite failed
            if (line.EndsWith(" your group.")) return false; //invite failed
            if (line.EndsWith(" the group.")) return false; //group join [Fri Mar 04 20:28:58 2022] You have joined the group.
            if (line.Contains("] You invite ")) return false; //inviting
            if (line.EndsWith(" song ends.")) return false; //bard spam
            if (line.EndsWith(" song to a close!")) return false; //bard spam
            if (line.EndsWith("] You are encumbered!")) return false; //status spam
            if (line.Contains("] --")) return false; //loot message
            if (line.Contains("] You have gained ")) return false; //xp message
            if (line.Contains("] There is ")) return false; //] There is 1 player in EverQuest.
            if (line.Contains(" says '")) return false; //says
            if (line.Contains("] You feel different.")) return false; //random message
            if (line.Contains("] You are ")) return false; //] You are idle, Auto-AFK
            if (line.Contains(" says out of character, '")) return false; //ooc
            if (line.Contains(" tells you, '")) return false;//tell
            if (line.Contains(" guild, '")) return false;//guild
            if (line.Contains(" party, '")) return false;//party
            if (line.Contains(" raid, '")) return false;//raid
            if (line.Contains(" group, '")) return false;//group
            if (line.Contains(" auctions, '")) return false;//auction
            if (line.Contains(" You feel replenished.")) return false; //bard spam
            if (line.Contains("] A missed note brings ")) return false; //bard spam
            if (line.Contains("] You told ")) return false; //tell
            if (line.Contains("] MESSAGE OF THE DAY:")) return false;//motd
            if (line.Contains("] GUILD MOTD:")) return false;//guildmotd
            if (line.Contains("] Channels:")) return false;//channels
            if (line.Contains("] The Universal Chat service")) return false; //chat service.
            if (line.Contains("] You are not currently assigned to an adventure.")) return false; //adventure
            if (line.Contains("] Announcing ")) return false; //anounce
            if (line.Contains("] Welcome to EverQuest!")) return false;//welcome
            if (line.Contains("] You have entered")) return false;//zone
            if (line.Contains(" regards you ")) return false;//coning
            if (line.Contains("] It will take ")) return false; //camping , ] It will take you about 30 seconds to prepare your camp.
            if (line.Contains("] The Universal Chat service")) return false; //chat service.
            if (line.Contains("] A problem occurred")) return false; //macroquest stuff
            if (line.Contains("] Targeted (Player):")) return false; //kiss stuff?
            if (line.Contains("] Right click on yourself to")) return false; //] Right click on yourself to bring up your own inspect window. Use the /toggleinspect
          
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
                    //TODO:change to the name configured
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
            //[Wed Mar 02 22:24:55 2022] Wenlowang's spell fizzles!
            //[Wed Mar 02 22:24:55 2022] A lightning warrior ohm knight's enchantments fades.
            if (line.Contains("begins to cast a spell. <"))
            {
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfNameEnd = line.IndexOf(" begins to cast a spell.");

                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);

                Int32 indexOfEventNameStart = line.IndexOf("<");
                indexOfNameStart += 2;
                Int32 indexOfEventNameEnd = line.IndexOf(">");
                indexOfEventNameEnd -= 1;

                string eventName = line.Substring(indexOfEventNameStart+1, indexOfEventNameEnd - indexOfEventNameStart);

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);

                return true;
            }
            else if(line.EndsWith("'s enchantments fades."))
            {
                //[Wed Mar 02 22:24:55 2022] A lightning warrior ohm knight's enchantments fades.
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfNameEnd = line.IndexOf("'s enchantments fades.");

                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);

                string eventName = "enchantments fades";

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);

                return true;
            }
            else if(line.EndsWith(" was burned."))
            {
                //[Fri Feb 25 21:23:43 2022] Ewiclip was burned.
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfNameEnd = line.IndexOf(" was burned.");

                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);

                string eventName = "was burned";

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);

                return true;
            }
            else if (line.EndsWith("Your spell is interrupted."))
            {
                //[Fri Mar 04 21:35:16 2022] Your spell is interrupted.
                
                string name = "YOU";//TODO: Come back here to add current configured name

                string eventName = "interrupted";

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);
            }
            else if (line.Contains("'s casting is interrupted!"))
            {
                ////[Tue Feb 22 10:46:37 2022] Heeler's casting is interrupted!
            
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfNameEnd = line.IndexOf("'s casting is interrupted!");

                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);

                string eventName = "interrupted";

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);
                return true;
            }
            else if (line.Contains("'s spell fizzles!"))
            {
                //[Wed Mar 02 22:24:55 2022] Wenlowang's spell fizzles!
            
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfNameEnd = line.IndexOf("'s spell fizzles!");

                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);

                string eventName = "spell fizzle";

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);
                return true;
            }
            else if(line.Contains("'s body is "))
            {
                ////[Fri Feb 25 21:23:41 2022] Ewiclip's body is consumed in rage.

                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfIS = line.IndexOf(" ", indexOfNameStart);
                indexOfIS += 1;
                Int32 indexOfISEnd = line.IndexOf(" ", indexOfIS);

                string isValue = line.Substring(indexOfIS, indexOfISEnd - indexOfIS);

                if (isValue != "body")
                {
                    //this is not a valid body is
                    return false;
                }

                Int32 indexOfNameEnd = line.IndexOf("'s body is ");
                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);
                Int32 indexOfEventNameStart = indexOfNameEnd + 11;
                Int32 indexOfEventNameEnd = line.Length - 1;
                string eventName = line.Substring(indexOfEventNameStart, indexOfEventNameEnd - indexOfEventNameStart);

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);

                return true;
               
            }
            else if(line.EndsWith(">") && line.IndexOf("]  <")<1)
            {
                //Bards being special like always
                List<string> validBardActions = new List<string>() {"is", "lets", "calls" };
                //[Fri Feb 25 21:20:32 2022] Mintsong is consumed by the rhythm <Rhythm of the Night>
                //[Fri Feb 25 21:07:08 2022] RabidBard lets loose a piercing blast. <Harmony of Sound>
                Int32 indexOfNameStart = line.IndexOf("]");
                indexOfNameStart += 2;
                Int32 indexOfIS = line.IndexOf(" ", indexOfNameStart);
                indexOfIS += 1;
                Int32 indexOfISEnd = line.IndexOf(" ", indexOfIS);

                string isValue = line.Substring(indexOfIS, indexOfISEnd - indexOfIS);

                if (!validBardActions.Contains(isValue))
                {
                    //this is not a valid is
                    return false;
                }
                Int32 indexOfNameEnd = line.IndexOf(" "+isValue+" ");
                string name = line.Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart);
             
                Int32 indexOfEventNameStart = line.IndexOf("<");
                indexOfNameStart += 2;
                Int32 indexOfEventNameEnd = line.IndexOf(">");
                indexOfEventNameEnd -= 1;

                string eventName = line.Substring(indexOfEventNameStart + 1, indexOfEventNameEnd - indexOfEventNameStart);

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);
            }
            else if (line.Contains(" is "))
            {
                //this is the final check, as " is " is far too common and want the before checks to act as filters
                //[Fri Feb 25 21:57:14 2022] Speedhax is enveloped in the fierce eye aura.
                //[Fri Feb 25 21:57:07 2022] Ture is bitten by an asp!

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
                Int32 indexOfEventNameEnd = line.Length-1;
                string eventName = line.Substring(indexOfEventNameStart, indexOfEventNameEnd - indexOfEventNameStart);

                EventEntry tmpEventEntry = new EventEntry();
                tmpEventEntry.EventName = eventName;
                AddEventEntry(data, name, timeBatchID, tmpEventEntry, line);

                return true;
            }



            return returnValue;
        }
        private static void AddEventEntry(Dictionary<string, Character> data, string name, Int64 timeBatchID, EventEntry tmpEventEntry, string line)
        {
            string timeStamp = line.Substring(1, line.IndexOf("]") - 1);
            DateTime linetimeStamp = DateTime.ParseExact(timeStamp, "ddd MMM dd HH:mm:ss yyyy", provider);
            tmpEventEntry.RawData = line;
            tmpEventEntry.TimeStamp = linetimeStamp;
            Character tmpCharacter;
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
            if (!tmpCharacter.EventEntriesByTimeID.ContainsKey(timeBatchID))
            {
                tmpCharacter.EventEntriesByTimeID.Add(timeBatchID, new List<EventEntry>());
            }
            tmpCharacter.EventEntriesByTimeID[timeBatchID].Add(tmpEventEntry);
        }


    }

}

