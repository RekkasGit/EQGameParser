
using EQGameParserv2.Sorters;

namespace EQGameParserv2
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewFights = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl_Overview = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBoxTargets = new System.Windows.Forms.ComboBox();
            this.listView_Overview = new System.Windows.Forms.ListView();
            this.columnHeaderOverview_DamageBy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_Target = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_TargetDamge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_Total = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_TargetDPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_TotalDPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_MeleeCritDmg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_SpellCritDmg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_MeleeCritDmgPct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOverview_SpellCritDmgPct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView_Events_Characters = new System.Windows.Forms.ListView();
            this.columnHeader_Event_Names_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.tabControl_Overview.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1510, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadLogToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadLogToolStripMenuItem
            // 
            this.loadLogToolStripMenuItem.Name = "loadLogToolStripMenuItem";
            this.loadLogToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.loadLogToolStripMenuItem.Text = "Load Log";
            this.loadLogToolStripMenuItem.Click += new System.EventHandler(this.loadLogToolStripMenuItem_Click);
            // 
            // listViewFights
            // 
            this.listViewFights.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewFights.FullRowSelect = true;
            this.listViewFights.GridLines = true;
            this.listViewFights.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewFights.HideSelection = false;
            this.listViewFights.Location = new System.Drawing.Point(12, 27);
            this.listViewFights.MultiSelect = false;
            this.listViewFights.Name = "listViewFights";
            this.listViewFights.Size = new System.Drawing.Size(227, 919);
            this.listViewFights.TabIndex = 1;
            this.listViewFights.UseCompatibleStateImageBehavior = false;
            this.listViewFights.View = System.Windows.Forms.View.Details;
            this.listViewFights.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewFights_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Fights";
            this.columnHeader1.Width = 46;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Oponents";
            this.columnHeader2.Width = 269;
            // 
            // tabControl_Overview
            // 
            this.tabControl_Overview.Controls.Add(this.tabPage1);
            this.tabControl_Overview.Controls.Add(this.tabPage2);
            this.tabControl_Overview.Controls.Add(this.tabPage3);
            this.tabControl_Overview.Location = new System.Drawing.Point(245, 27);
            this.tabControl_Overview.Name = "tabControl_Overview";
            this.tabControl_Overview.SelectedIndex = 0;
            this.tabControl_Overview.Size = new System.Drawing.Size(1255, 919);
            this.tabControl_Overview.TabIndex = 2;
            this.tabControl_Overview.SelectedIndexChanged += new System.EventHandler(this.tabControl_Overview_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.comboBoxTargets);
            this.tabPage1.Controls.Add(this.listView_Overview);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1247, 893);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Overview";
            // 
            // comboBoxTargets
            // 
            this.comboBoxTargets.AllowDrop = true;
            this.comboBoxTargets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargets.FormattingEnabled = true;
            this.comboBoxTargets.Location = new System.Drawing.Point(139, 0);
            this.comboBoxTargets.Name = "comboBoxTargets";
            this.comboBoxTargets.Size = new System.Drawing.Size(124, 21);
            this.comboBoxTargets.TabIndex = 1;
            this.comboBoxTargets.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargets_SelectedIndexChanged);
            // 
            // listView_Overview
            // 
            this.listView_Overview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderOverview_DamageBy,
            this.columnHeaderOverview_Target,
            this.columnHeaderOverview_TargetDamge,
            this.columnHeaderOverview_Total,
            this.columnHeaderOverview_TargetDPS,
            this.columnHeaderOverview_TotalDPS,
            this.columnHeaderOverview_MeleeCritDmg,
            this.columnHeaderOverview_SpellCritDmg,
            this.columnHeaderOverview_ID,
            this.columnHeaderOverview_MeleeCritDmgPct,
            this.columnHeaderOverview_SpellCritDmgPct});
            this.listView_Overview.FullRowSelect = true;
            this.listView_Overview.GridLines = true;
            this.listView_Overview.HideSelection = false;
            this.listView_Overview.Location = new System.Drawing.Point(0, 21);
            this.listView_Overview.Name = "listView_Overview";
            this.listView_Overview.Size = new System.Drawing.Size(1241, 872);
            this.listView_Overview.TabIndex = 0;
            this.listView_Overview.UseCompatibleStateImageBehavior = false;
            this.listView_Overview.View = System.Windows.Forms.View.Details;
            this.listView_Overview.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_Overview_ColumnClick);
            // 
            // columnHeaderOverview_DamageBy
            // 
            this.columnHeaderOverview_DamageBy.DisplayIndex = 1;
            this.columnHeaderOverview_DamageBy.Text = "Damage By";
            this.columnHeaderOverview_DamageBy.Width = 94;
            // 
            // columnHeaderOverview_Target
            // 
            this.columnHeaderOverview_Target.DisplayIndex = 2;
            this.columnHeaderOverview_Target.Text = "Target";
            this.columnHeaderOverview_Target.Width = 125;
            // 
            // columnHeaderOverview_TargetDamge
            // 
            this.columnHeaderOverview_TargetDamge.DisplayIndex = 3;
            this.columnHeaderOverview_TargetDamge.Text = "Target Damage";
            this.columnHeaderOverview_TargetDamge.Width = 100;
            // 
            // columnHeaderOverview_Total
            // 
            this.columnHeaderOverview_Total.DisplayIndex = 4;
            this.columnHeaderOverview_Total.Text = "Total";
            this.columnHeaderOverview_Total.Width = 104;
            // 
            // columnHeaderOverview_TargetDPS
            // 
            this.columnHeaderOverview_TargetDPS.DisplayIndex = 5;
            this.columnHeaderOverview_TargetDPS.Text = "Target DPS";
            this.columnHeaderOverview_TargetDPS.Width = 106;
            // 
            // columnHeaderOverview_TotalDPS
            // 
            this.columnHeaderOverview_TotalDPS.DisplayIndex = 6;
            this.columnHeaderOverview_TotalDPS.Text = "Total DPS";
            this.columnHeaderOverview_TotalDPS.Width = 88;
            // 
            // columnHeaderOverview_MeleeCritDmg
            // 
            this.columnHeaderOverview_MeleeCritDmg.DisplayIndex = 7;
            this.columnHeaderOverview_MeleeCritDmg.Text = "MeleeCrit Dmg";
            this.columnHeaderOverview_MeleeCritDmg.Width = 103;
            // 
            // columnHeaderOverview_SpellCritDmg
            // 
            this.columnHeaderOverview_SpellCritDmg.DisplayIndex = 8;
            this.columnHeaderOverview_SpellCritDmg.Text = "Spell Crit Dmg";
            this.columnHeaderOverview_SpellCritDmg.Width = 100;
            // 
            // columnHeaderOverview_ID
            // 
            this.columnHeaderOverview_ID.DisplayIndex = 0;
            this.columnHeaderOverview_ID.Text = "#";
            this.columnHeaderOverview_ID.Width = 43;
            // 
            // columnHeaderOverview_MeleeCritDmgPct
            // 
            this.columnHeaderOverview_MeleeCritDmgPct.Text = "MeeleeCrit Dmg%";
            this.columnHeaderOverview_MeleeCritDmgPct.Width = 108;
            // 
            // columnHeaderOverview_SpellCritDmgPct
            // 
            this.columnHeaderOverview_SpellCritDmgPct.Text = "Spell Crit Dmg %";
            this.columnHeaderOverview_SpellCritDmgPct.Width = 101;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1247, 670);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Player DPS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.listView1);
            this.tabPage3.Controls.Add(this.listView_Events_Characters);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1247, 670);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Events";
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(190, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1054, 661);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // listView_Events_Characters
            // 
            this.listView_Events_Characters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Event_Names_Name});
            this.listView_Events_Characters.FullRowSelect = true;
            this.listView_Events_Characters.GridLines = true;
            this.listView_Events_Characters.HideSelection = false;
            this.listView_Events_Characters.Location = new System.Drawing.Point(6, 6);
            this.listView_Events_Characters.Name = "listView_Events_Characters";
            this.listView_Events_Characters.Size = new System.Drawing.Size(178, 244);
            this.listView_Events_Characters.TabIndex = 0;
            this.listView_Events_Characters.UseCompatibleStateImageBehavior = false;
            this.listView_Events_Characters.View = System.Windows.Forms.View.Details;
            this.listView_Events_Characters.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_Events_Characters_ItemSelectionChanged);
            // 
            // columnHeader_Event_Names_Name
            // 
            this.columnHeader_Event_Names_Name.Text = "Name";
            this.columnHeader_Event_Names_Name.Width = 174;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1510, 958);
            this.Controls.Add(this.tabControl_Overview);
            this.Controls.Add(this.listViewFights);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl_Overview.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadLogToolStripMenuItem;
        private System.Windows.Forms.ListView listViewFights;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabControl tabControl_Overview;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listView_Overview;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_DamageBy;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_Total;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_Target;
        public System.Windows.Forms.ColumnHeader columnHeaderOverview_TargetDamge;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_TargetDPS;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_TotalDPS;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_MeleeCritDmg;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_SpellCritDmg;
        private System.Windows.Forms.ComboBox comboBoxTargets;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_ID;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_MeleeCritDmgPct;
        private System.Windows.Forms.ColumnHeader columnHeaderOverview_SpellCritDmgPct;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listView_Events_Characters;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader_Event_Names_Name;
    }
}

