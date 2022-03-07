using EQGameParserv2.Models;
using EQGameParserv2.Sorters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EQGameParserv2
{
    public partial class Main : Form
    {
        StreamReader reader;
        System.IO.FileStream stream;
        Dictionary<string, Character> _data = new Dictionary<string, Character>();
        volatile Boolean _stopTask = false;
        Task _ProcessingTask=null;
        private ListViewColumnSorter _lvwColumnSorter= new ListViewColumnSorter();
        System.Diagnostics.Stopwatch _stopWatch = new System.Diagnostics.Stopwatch();
        public Main()
        {
            _stopWatch.Start();
            InitializeComponent();
            this.listView_Overview.ListViewItemSorter = _lvwColumnSorter;
        }
        
        private void loadLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"G:\EQ\Project Lazarus\Logs";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                string filePath;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    if (_ProcessingTask != null)
                    {
                        _stopTask = true;
                        _ProcessingTask.Wait();
                        _stopTask = false;
                        reader.Dispose();
                        stream.Dispose();
                    }
                    stream= new System.IO.FileStream(openFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    reader = new StreamReader(stream);
                    listViewFights.Items.Clear();
                    listView_Overview.Items.Clear();
                    _data.Clear();
                    
                    _ProcessingTask = new Task(() => ProcessingLoop());
                    _ProcessingTask.Start();

                }
            }
        }

        private void listViewFights_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            string timeBatchIDAsString = e.Item.Text;
            Int64 timeBatchID = System.Int64.Parse(timeBatchIDAsString);
            listView_Overview_Populate(timeBatchID, null);
            //now sort by damage done
            // Determine if clicked column is already the column that is being sorted.
            listView_Overview_DefaultSort();


        }
        private void listView_Overview_DefaultSort()
        {
            // Set the column number that is to be sorted; default to ascending.
            _lvwColumnSorter.SortColumn = 2;
            _lvwColumnSorter.Order = SortOrder.Descending;
            //_lvwColumnSorter.Order = SortOrder.Ascending;

            // Perform the sort with these new sort options.
            listView_Overview.Sort();
            listView_Overview.BeginUpdate();
            Int32 counter = 1;
            foreach (ListViewItem item in listView_Overview.Items)
            {
                item.SubItems[8].Text = counter.ToString();
                counter++;
            }
            listView_Overview.EndUpdate();
        }
        private void comboBoxTargets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listViewFights.SelectedItems.Count>0 && comboBoxTargets.SelectedItem!=null)
            {
                string selectedItem = comboBoxTargets.SelectedItem.ToString();
                Int64 timeBatchID = System.Int64.Parse(listViewFights.SelectedItems[0].Text);
                listView_Overview_Populate(timeBatchID, comboBoxTargets.SelectedItem.ToString());
                listView_Overview.BeginUpdate();
                Int32 counter = 1;
                foreach (ListViewItem item in listView_Overview.Items)
                {
                    item.SubItems[8].Text = counter.ToString();
                    counter++;
                }
                listView_Overview.EndUpdate();

            }


        }
        private void listView_Overview_Populate(Int64 timeBatchID, string TargetFilter)
        {
            Int64 startTimeStopWatch = _stopWatch.ElapsedMilliseconds;
            Int64 endTimeStopWatch;
            if (TargetFilter == null)
            {
                comboBoxTargets.Items.Clear();

            }

            //get the people during this

            List<Character> peopleInThisBatch = new List<Character>();

            foreach (var person in _data.Values)
            {
                if (person.DamageEntriesByTimeID.ContainsKey(timeBatchID))
                {
                    peopleInThisBatch.Add(person);
                }
            }

            endTimeStopWatch = _stopWatch.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("TotalTime-GettingPeople: " + (endTimeStopWatch - startTimeStopWatch) + " milliseconds");
            this.listView_Overview.ListViewItemSorter = null;
            listView_Overview.BeginUpdate();
            listView_Overview.Items.Clear();
            Int32 counter = 0;
            HashSet<String> uniqueTargets = new HashSet<string>();
            foreach (var person in peopleInThisBatch)
            {

                List<DamageEntry> damages = person.DamageEntriesByTimeID[timeBatchID];
                Dictionary<String, TargetDPS> targetDamage = new Dictionary<string, TargetDPS>();


                DateTime startTime = damages[0].TimeStamp;
                DateTime endTime = damages.Last().TimeStamp;
                Int64 totalTimeForDps = (Int64)endTime.Subtract(startTime).TotalSeconds;

                if (totalTimeForDps < 1)
                {
                    totalTimeForDps = 1;
                }

                Int64 totalDamage = 0;
                foreach (var entry in damages)
                {
                    if (!targetDamage.ContainsKey(entry.Target))
                    {
                        if (!uniqueTargets.Contains(entry.Target))
                        {
                            uniqueTargets.Add(entry.Target);
                        }
                        targetDamage.Add(entry.Target, new TargetDPS());
                        targetDamage[entry.Target].StartTime = entry.TimeStamp;
                    }
                    TargetDPS dpsEntry = targetDamage[entry.Target];
                    dpsEntry.Damage += entry.Damage;
                    dpsEntry.DamageCount += 1;
                    if (entry.DamageMod == DamageModifier.Crit && entry.DamageType == "directdmg")
                    {
                        dpsEntry.SpellCritDamage += entry.Damage;
                        dpsEntry.SpellCritDamageCount += 1;
                    }
                    else if (entry.DamageMod == DamageModifier.Crit || entry.DamageMod == DamageModifier.Crippling)
                    {
                        //melee crits and crippling
                        dpsEntry.CritDamage += entry.Damage;
                        dpsEntry.CritDamageCount += 1;
                    }

                    dpsEntry.EndTime = entry.TimeStamp;
                    totalDamage += entry.Damage;
                }


                foreach (var pair in targetDamage)
                {
                    //if target filter is active, only accept that target
                    if (TargetFilter != null && TargetFilter != pair.Key)
                    {
                        continue;
                    }
                    counter++;
                    var newItem = new ListViewItem();
                    newItem.Name = "lvi_overview_" + person.Name + "_" + (counter);
                    newItem.Text = person.Name;

                    newItem.SubItems.Add(pair.Key);
                    newItem.SubItems.Add(pair.Value.Damage.ToString());
                    newItem.SubItems.Add(totalDamage.ToString());
                    Int64 targetTimeForDps = (Int64)pair.Value.EndTime.Subtract(pair.Value.StartTime).TotalSeconds;
                    if (targetTimeForDps < 1)
                    {
                        targetTimeForDps = 1;
                    }
                    newItem.SubItems.Add((pair.Value.Damage / targetTimeForDps).ToString());
                    newItem.SubItems.Add((totalDamage / totalTimeForDps).ToString());
                    newItem.SubItems.Add((pair.Value.CritDamage).ToString());
                    newItem.SubItems.Add((pair.Value.SpellCritDamage).ToString());
                    newItem.SubItems.Add(counter.ToString());

                    Decimal percentCrit = 0m;
                    Decimal percentSpellCrit = 0m;

                    if (pair.Value.SpellCritDamageCount > 0)
                    {
                        percentSpellCrit = ((Decimal)pair.Value.SpellCritDamageCount / (Decimal)pair.Value.DamageCount) * 100;
                        percentSpellCrit = Math.Round(percentSpellCrit, 2);
                    }
                    if (pair.Value.CritDamageCount > 0)
                    {
                        percentCrit = ((Decimal)pair.Value.CritDamageCount / (Decimal)pair.Value.DamageCount) * 100;
                        percentCrit = Math.Round(percentCrit, 2);
                    }
                    newItem.SubItems.Add((percentCrit).ToString());
                    newItem.SubItems.Add((percentSpellCrit).ToString());

                    listView_Overview.Items.Add(newItem);

                }

            }

            listView_Overview.EndUpdate();
            this.listView_Overview.ListViewItemSorter = _lvwColumnSorter;
            endTimeStopWatch = _stopWatch.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("TotalTime-EndingListViewUpdate: " + (endTimeStopWatch - startTimeStopWatch) + " milliseconds");


            if (TargetFilter == null)
            {
                comboBoxTargets.BeginUpdate();
                foreach (var value in uniqueTargets)
                {
                    comboBoxTargets.Items.Add(value);
                }
                comboBoxTargets.EndUpdate();
            }

            endTimeStopWatch = _stopWatch.ElapsedMilliseconds;
            Int64 totalMillisecondsStopWatch = endTimeStopWatch - startTimeStopWatch;
            System.Diagnostics.Debug.WriteLine("TotalTime: " + totalMillisecondsStopWatch + " milliseconds");




        }
        private void listView_Overview_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    _lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _lvwColumnSorter.SortColumn = e.Column;
                //_lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listView_Overview.Sort();
            listView_Overview.BeginUpdate();
            Int32 counter = 1;
            foreach (ListViewItem item in listView_Overview.Items)
            {
               item.SubItems[8].Text = counter.ToString();
               counter++;    
            }
            listView_Overview.EndUpdate();
        }

        private void ProcessingLoop()
        {
            Int32 listViewCount = 0;
        
            CultureInfo provider = CultureInfo.InvariantCulture;
            Int64 timeBatchID = 0;
            DateTime lastTimeForDamage = System.DateTime.MinValue;
            Boolean hadDamageRecently = false;
            Int64 linecounter = 0;
            string previousLine = String.Empty;
            Int32 countSinceLastReadLine = 0;
            while (!_stopTask)
            {

                string currentLine = reader.ReadLine();
                if (!String.IsNullOrEmpty(currentLine))
                {
                    if (!currentLine.StartsWith("["))
                    {
                        continue;
                    }
                    linecounter++;
                    
                    if (Parsing.LinerParser.ParseDamage(currentLine, previousLine, _data, ref timeBatchID, ref lastTimeForDamage))
                    {
                        countSinceLastReadLine = 0;
                        hadDamageRecently = true;

                    }

                    previousLine = currentLine;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    countSinceLastReadLine++;
                    if (countSinceLastReadLine > 100 && hadDamageRecently && lastTimeForDamage != DateTime.MinValue && System.DateTime.Now.Subtract(lastTimeForDamage).TotalSeconds > 10)
                    {
                        //new timestamp
                        timeBatchID++;
                        hadDamageRecently = false;
                        countSinceLastReadLine = 1;


                    }
                    List<ListViewItem> itemsToAdd = null;
                    if (listViewCount < timeBatchID)
                    {
                        itemsToAdd = new List<ListViewItem>();
                        while (listViewCount < timeBatchID)
                        {
                            //add the missing fights

                            List<Character> charsInFight = new List<Character>();

                            foreach (var person in _data.Values)
                            {
                                if (person.DamageEntriesByTimeID.ContainsKey(listViewCount))
                                {
                                    charsInFight.Add(person);
                                }
                            }

                            if (charsInFight.Count > 0)
                            {
                                HashSet<string> targets = new HashSet<string>();
                                //find the target with the most tamage 

                                List<DamageEntry> entries = charsInFight[0].DamageEntriesByTimeID[listViewCount];
                                Dictionary<string, Int64> targetsAndDamageTaken = new Dictionary<string, long>();

                                foreach (var character in charsInFight)
                                {
                                    foreach (var dmgEntry in character.DamageEntriesByTimeID[listViewCount])
                                    {
                                        if (!targetsAndDamageTaken.ContainsKey(dmgEntry.Target))
                                        {
                                            targetsAndDamageTaken.Add(dmgEntry.Target, 0);
                                        }

                                        targetsAndDamageTaken[dmgEntry.Target] += dmgEntry.Damage;
                                    }
                                }




                                var newItem = new ListViewItem();
                                newItem.Name = "lvi_" + (listViewCount);
                                newItem.Text = listViewCount.ToString();
                                //add the highest damaged target as the text

                                string currentTopDmgTargetString = String.Empty;
                                Int64 currentTopDmgTargetValue = 0;

                                foreach (var pair in targetsAndDamageTaken)
                                {
                                    if (currentTopDmgTargetValue < pair.Value)
                                    {
                                        currentTopDmgTargetValue = pair.Value;
                                        currentTopDmgTargetString = pair.Key;
                                    }
                                }


                                newItem.SubItems.Add(currentTopDmgTargetString);

                                itemsToAdd.Add(newItem);
                                listViewCount++;


                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("ERROR");
                            }

                        }

                        if (itemsToAdd != null)
                        {
                            listViewFights.Invoke(new MethodInvoker(delegate
                            {
                                listViewFights.BeginUpdate();
                                foreach (var item in itemsToAdd)
                                {
                                    listViewFights.Items.Add(item);
                                }
                                listViewFights.EndUpdate();

                            }));
                        }


                    }



                }
            }


        }

    }
}
