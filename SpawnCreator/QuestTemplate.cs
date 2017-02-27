﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Diagnostics;

namespace SpawnCreator
{
    public partial class QuestTemplate : Form
    {
        public QuestTemplate()
        {
            InitializeComponent();
        }

        private bool _mouseDown;
        private Point lastLocation;

        Form_MainMenu mainmenu = new Form_MainMenu();

        public static string stringSQLShare;
        public static string stringEntryShare;

        private void QuestTemplate_Load(object sender, EventArgs e)
        {
            timer1.Start(); //Stopwatch
            timer2.Start(); //Check if MySQL is still running

            comboBox1.SelectedIndex = 0; // Show default
            comboBox2.SelectedIndex = 0; /*All Races*/
            comboBox3.SelectedIndex = 0; // INSERT

            button3_Click(sender, e);

            textBox107.Text = textBox1.Text;
            textBox108.Text = textBox1.Text;
        }

        private void GenerateSqlCode(object sender, EventArgs e)
        {
            uint quest_flags_st = 0;

            string[] checkedIndicies1 = Properties.Settings.Default.QuestFlags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i1 = 0; i1 < checkedIndicies1.Length; i1++)
            {
                int idx;
                if ((int.TryParse(checkedIndicies1[i1], out idx)))
                {
                    switch (idx)
                    {
                        case 0: quest_flags_st += 0; break; // QUEST_FLAGS_NONE
                        case 1: quest_flags_st += 1; break; // QUEST_FLAGS_STAY_ALIVE
                        case 2: quest_flags_st += 2; break; // QUEST_FLAGS_PARTY_ACCEPT
                        case 3: quest_flags_st += 4; break; // QUEST_FLAGS_EXPLORATION
                        case 4: quest_flags_st += 8; break; // QUEST_FLAGS_SHARABLE
                        case 5: quest_flags_st += 16; break; // QUEST_FLAGS_NONE2
                        case 6: quest_flags_st += 32; break; // QUEST_FLAGS_EPIC
                        case 7: quest_flags_st += 64; break; // QUEST_FLAGS_RAID
                        case 8: quest_flags_st += 128; break; // QUEST_FLAGS_TBC
                        case 9: quest_flags_st += 256; break; // QUEST_FLAGS_DELIVER_MORE
                        case 10: quest_flags_st += 512; break; // QUEST_FLAGS_HIDDEN_REWARDS
                        case 11: quest_flags_st += 1024; break; // QUEST_FLAGS__AUTO_REWARDED
                        case 12: quest_flags_st += 2048; break; // QUEST_FLAGS_TBC_RACES
                        case 13: quest_flags_st += 4096; break; // QUEST_FLAGS_DAILY
                        case 14: quest_flags_st += 8192; break; // QUEST_FLAGS_REPEATABLE
                        case 15: quest_flags_st += 16384; break; // QUEST_FLAGS_UNAVAILABLE
                        case 16: quest_flags_st += 32768; break; // QUEST_FLAGS_WEEKLY
                        case 17: quest_flags_st += 65536; break; // QUEST_FLAGS_AUTOCOMPLETE
                        case 18: quest_flags_st += 131072; break; // QUEST_FLAGS_SPECIAL_ITEM
                        case 19: quest_flags_st += 262144; break; // QUEST_FLAGS_OBJ_TEXT
                        case 20: quest_flags_st += 524288; break; // QUEST_FLAGS_AUTO_ACCEPT
                        case 21: quest_flags_st += 1048576; break; // QUEST_FLAGS_AUTO_SUBMIT
                        case 22: quest_flags_st += 2097152; break; // QUEST_FLAGS_AUTO_TAKE
                    }
                }
            }
            
            // Prepare SQL
            // select insertion columns
            string BuildSQLFile;
            BuildSQLFile = textBox105.Text + " INTO " + mainmenu.textbox_mysql_worldDB.Text + ".quest_template ";
            BuildSQLFile += "(ID, QuestType, QuestLevel, MinLevel, QuestSortID, QuestInfoID, SuggestedGroupNum, RequiredFactionId1, RequiredFactionId2, RequiredFactionValue1, ";
            BuildSQLFile += "RequiredFactionValue2, RewardNextQuest, RewardXPDifficulty, RewardMoney, RewardBonusMoney, RewardDisplaySpell, RewardSpell, RewardHonor, ";
            BuildSQLFile += "RewardKillHonor, StartItem, Flags, RequiredPlayerKills, RewardItem1, RewardAmount1, RewardItem2, RewardAmount2, RewardItem3, RewardAmount3, RewardItem4, ";
            BuildSQLFile += "RewardAmount4, ItemDrop1, ItemDropQuantity1, ItemDrop2, ItemDropQuantity2, ItemDrop3, ItemDropQuantity3, ItemDrop4, ";
            BuildSQLFile += "ItemDropQuantity4, RewardChoiceItemID1, RewardChoiceItemQuantity1, RewardChoiceItemID2, RewardChoiceItemQuantity2, RewardChoiceItemID3, ";
            BuildSQLFile += "RewardChoiceItemQuantity3, RewardChoiceItemID4, RewardChoiceItemQuantity4, RewardChoiceItemID5, RewardChoiceItemQuantity5, RewardChoiceItemID6, RewardChoiceItemQuantity6, ";
            BuildSQLFile += "POIContinent, POIx, POIy, POIPriority, RewardTitle, RewardTalents, RewardArenaPoints, RewardFactionID1, RewardFactionValue1, RewardFactionOverride1, ";
            BuildSQLFile += "RewardFactionID2, RewardFactionValue2, RewardFactionOverride2, RewardFactionID3, RewardFactionValue3, RewardFactionOverride3, RewardFactionID4, ";
            BuildSQLFile += "RewardFactionValue4, RewardFactionOverride4, RewardFactionID5, RewardFactionValue5, RewardFactionOverride5, TimeAllowed, AllowableRaces, LogTitle, LogDescription, ";
            BuildSQLFile += "QuestDescription, AreaDescription, QuestCompletionLog, RequiredNpcOrGo1, RequiredNpcOrGo2, RequiredNpcOrGo3, RequiredNpcOrGo4, RequiredNpcOrGoCount1, ";
            BuildSQLFile += "RequiredNpcOrGoCount2, RequiredNpcOrGoCount3, RequiredNpcOrGoCount4, RequiredItemId1, RequiredItemId2, RequiredItemId3, RequiredItemId4, RequiredItemId5, RequiredItemId6, ";
            BuildSQLFile += "RequiredItemCount1, RequiredItemCount2, RequiredItemCount3, RequiredItemCount4, RequiredItemCount5, ";
            BuildSQLFile += "RequiredItemCount6, Unknown0, ObjectiveText1, ObjectiveText2, ObjectiveText3, ObjectiveText4, VerifiedBuild) ";

            // values now
            BuildSQLFile += "VALUES \n";
            BuildSQLFile += "(";
            BuildSQLFile += textBox1.Text + ", "; // ID
            BuildSQLFile += textBox2.Text + ", "; // QuestType
            BuildSQLFile += textBox3.Text + ", "; // QuestLevel
            BuildSQLFile += textBox4.Text + ", "; // MinLevel
            BuildSQLFile += textBox8.Text + ", "; // QuestSortID
            BuildSQLFile += textBox7.Text + ", "; // QuestInfoID
            BuildSQLFile += textBox6.Text + ", "; // SuggestedGroupNum
            BuildSQLFile += textBox5.Text + ", "; // RequiredFactionId1
            BuildSQLFile += textBox12.Text + ", "; // RequiredFactionId2
            BuildSQLFile += textBox11.Text + ", "; // RequiredFactionValue1
            BuildSQLFile += textBox10.Text + ", "; // RequiredFactionValue2
            BuildSQLFile += textBox9.Text + ", "; // RewardNextQuest
            BuildSQLFile += textBox20.Text + ", "; // RewardXPDifficulty
            BuildSQLFile += textBox19.Text + ", "; // RewardMoney
            BuildSQLFile += textBox18.Text + ", "; // RewardBonusMoney
            BuildSQLFile += textBox17.Text + ", "; // RewardDisplaySpell
            BuildSQLFile += textBox16.Text + ", "; // RewardSpell
            BuildSQLFile += textBox15.Text + ", "; // RewardHonor
            BuildSQLFile += textBox14.Text + ", "; // RewardKillHonor
            BuildSQLFile += textBox13.Text + ", "; // StartItem
            BuildSQLFile += quest_flags_st + ", "; // Flags
            BuildSQLFile += textBox40.Text + ", "; // RequiredPlayerKills
            BuildSQLFile += textBox39.Text + ", "; // RewardItem1
            BuildSQLFile += textBox38.Text + ", "; // RewardAmount1
            BuildSQLFile += textBox37.Text + ", "; // RewardItem2
            BuildSQLFile += textBox36.Text + ", "; // RewardAmount2
            BuildSQLFile += textBox35.Text + ", "; // RewardItem3
            BuildSQLFile += textBox34.Text + ", "; // RewardAmount3
            BuildSQLFile += textBox33.Text + ", "; // RewardItem4
            BuildSQLFile += textBox32.Text + ", "; // RewardAmount4
            BuildSQLFile += textBox31.Text + ", "; // ItemDrop1
            BuildSQLFile += textBox30.Text + ", "; // ItemDropQuantity1
            BuildSQLFile += textBox29.Text + ", "; // ItemDrop2
            BuildSQLFile += textBox28.Text + ", "; // ItemDropQuantity2
            BuildSQLFile += textBox27.Text + ", "; // ItemDrop3
            BuildSQLFile += textBox26.Text + ", "; // ItemDropQuantity3
            BuildSQLFile += textBox25.Text + ", "; // ItemDrop4
            BuildSQLFile += textBox24.Text + ", "; // ItemDropQuantity4
            BuildSQLFile += textBox23.Text + ", "; // RewardChoiceItemID1
            BuildSQLFile += textBox22.Text + ", "; // RewardChoiceItemQuantity1
            BuildSQLFile += textBox21.Text + ", "; // RewardChoiceItemID2
            BuildSQLFile += textBox60.Text + ", "; // RewardChoiceItemQuantity2
            BuildSQLFile += textBox59.Text + ", "; // RewardChoiceItemID3
            BuildSQLFile += textBox58.Text + ", "; // RewardChoiceItemQuantity3
            BuildSQLFile += textBox57.Text + ", "; // RewardChoiceItemID4
            BuildSQLFile += textBox56.Text + ", "; // RewardChoiceItemQuantity4
            BuildSQLFile += textBox55.Text + ", "; // RewardChoiceItemID5
            BuildSQLFile += textBox54.Text + ", "; // RewardChoiceItemQuantity5
            BuildSQLFile += textBox53.Text + ", "; // RewardChoiceItemID6
            BuildSQLFile += textBox52.Text + ", "; // RewardChoiceItemQuantity6
            BuildSQLFile += textBox51.Text + ", "; // POIContinent
            BuildSQLFile += textBox50.Text + ", "; // POIx
            BuildSQLFile += textBox49.Text + ", "; // POIy
            BuildSQLFile += textBox48.Text + ", "; // POIPriority
            BuildSQLFile += textBox47.Text + ", "; // RewardTitle
            BuildSQLFile += textBox46.Text + ", "; // RewardTalents
            BuildSQLFile += textBox45.Text + ", "; // RewardArenaPoints
            BuildSQLFile += textBox44.Text + ", "; // RewardFactionID1
            BuildSQLFile += textBox43.Text + ", "; // RewardFactionValue1
            BuildSQLFile += textBox42.Text + ", "; // RewardFactionOverride1
            BuildSQLFile += textBox41.Text + ", "; // RewardFactionID2
            BuildSQLFile += textBox80.Text + ", "; // RewardFactionValue2
            BuildSQLFile += textBox79.Text + ", "; // RewardFactionOverride2
            BuildSQLFile += textBox78.Text + ", "; // RewardFactionID3
            BuildSQLFile += textBox77.Text + ", "; // RewardFactionValue3
            BuildSQLFile += textBox76.Text + ", "; // RewardFactionOverride3
            BuildSQLFile += textBox75.Text + ", "; // RewardFactionID4
            BuildSQLFile += textBox74.Text + ", "; // RewardFactionValue4
            BuildSQLFile += textBox73.Text + ", "; // RewardFactionOverride4
            BuildSQLFile += textBox72.Text + ", "; // RewardFactionID5
            BuildSQLFile += textBox71.Text + ", "; // RewardFactionValue5
            BuildSQLFile += textBox70.Text + ", "; // RewardFactionOverride5
            BuildSQLFile += textBox69.Text + ", "; // TimeAllowed
            BuildSQLFile += textBox68.Text + ", "; // AllowableRaces 
            BuildSQLFile += "'" + textBox67.Text + "', "; // LogTitle
            BuildSQLFile += "'" + textBox66.Text + "', "; // LogDescription
            BuildSQLFile += "'" + textBox65.Text + "', "; // QuestDescription
            BuildSQLFile += "'" + textBox64.Text + "', "; // AreaDescription
            BuildSQLFile += "'" + textBox63.Text + "', "; // QuestCompletionLog
            BuildSQLFile += textBox62.Text + ", "; // RequiredNpcOrGo1
            BuildSQLFile += textBox61.Text + ", "; // RequiredNpcOrGo2
            BuildSQLFile += textBox83.Text + ", "; // RequiredNpcOrGo3
            BuildSQLFile += textBox81.Text + ", "; // RequiredNpcOrGo4
            BuildSQLFile += textBox86.Text + ", "; // RequiredNpcOrGoCount1
            BuildSQLFile += textBox85.Text + ", "; // RequiredNpcOrGoCount2
            BuildSQLFile += textBox84.Text + ", "; // RequiredNpcOrGoCount3
            BuildSQLFile += textBox83.Text + ", "; // RequiredNpcOrGoCount4
            BuildSQLFile += textBox90.Text + ", "; // RequiredItemId1
            BuildSQLFile += textBox87.Text + ", "; // RequiredItemId2
            BuildSQLFile += textBox99.Text + ", "; // RequiredItemId3
            BuildSQLFile += textBox96.Text + ", "; // RequiredItemId4
            BuildSQLFile += textBox93.Text + ", "; // RequiredItemId5
            BuildSQLFile += textBox102.Text + ", "; // RequiredItemId6
            BuildSQLFile += textBox89.Text + ", "; // RequiredItemCount1
            BuildSQLFile += textBox101.Text + ", "; // RequiredItemCount2
            BuildSQLFile += textBox98.Text + ", "; // RequiredItemCount3
            BuildSQLFile += textBox95.Text + ", "; // RequiredItemCount4
            BuildSQLFile += textBox92.Text + ", "; // RequiredItemCount5
            BuildSQLFile += textBox103.Text + ", "; // RequiredItemCount6
            BuildSQLFile += textBox88.Text + ", "; // Unknown0
            BuildSQLFile += "'" + textBox97.Text + "', "; // ObjectiveText1
            BuildSQLFile += "'" + textBox94.Text + "', "; // ObjectiveText2
            BuildSQLFile += "'" + textBox91.Text + "', "; // ObjectiveText3
            BuildSQLFile += "'" + textBox104.Text + "', "; // ObjectiveText4
            BuildSQLFile += textBox100.Text; // VerifiedBuild
            BuildSQLFile += "); \n";

            //Creature Quest Starter
            BuildSQLFile += textBox105.Text + " INTO " + mainmenu.textbox_mysql_worldDB.Text + ".creature_queststarter ";
            BuildSQLFile += "(id, quest) ";
            BuildSQLFile += "VALUES \n";
            BuildSQLFile += "(";
            BuildSQLFile += textBox106.Text + ", "; // Creature Entry
            BuildSQLFile += textBox107.Text; // Quest ID
            BuildSQLFile += "); \n";

            //Creature Quest Ender
            BuildSQLFile += textBox105.Text + " INTO " + mainmenu.textbox_mysql_worldDB.Text + ".creature_questender ";
            BuildSQLFile += "(id, quest) ";
            BuildSQLFile += "VALUES \n";
            BuildSQLFile += "(";
            BuildSQLFile += textBox109.Text + ", "; // Creature Entry
            BuildSQLFile += textBox108.Text; // Quest ID
            BuildSQLFile += ");";

            stringSQLShare = BuildSQLFile;
            stringEntryShare = textBox1.Text;
        }

        //private bool CheckTextBoxes( /*TextBox textbox, bool true*/)
        //{          
        //    return true;
        //}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label78_Click(object sender, EventArgs e)
        {
            Close();

            BackToMainMenu back = new BackToMainMenu();
            back.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.Firebrick;
            label2.ForeColor = Color.White;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(58, 89, 114);
            label2.ForeColor = Color.Black;
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.Firebrick;
            label1.ForeColor = Color.White;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(58, 89, 114);
            label1.ForeColor = Color.Black;
        }

        private void label78_MouseEnter(object sender, EventArgs e)
        {
            label78.BackColor = Color.Firebrick;
            label78.ForeColor = Color.White;
        }

        private void label78_MouseLeave(object sender, EventArgs e)
        {
            label78.BackColor = Color.FromArgb(58, 89, 114);
            label78.ForeColor = Color.Black;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://emucraft.com");
        }

        //===================================================================
        int idx = 1;
        DateTime dt = new DateTime();
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_stopwatch.Text = dt.AddSeconds(idx).ToString("HH:mm:ss");
            idx++;
        }
        //===================================================================

        private void label83_MouseEnter(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Firebrick;
        }

        private void label83_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.FromArgb(58, 89, 114);
        }
        //===================================================================
        private void label86_MouseEnter(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Firebrick;
        }

        private void label86_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void panel5_MouseEnter(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Firebrick;
        }

        private void panel5_MouseLeave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void panel7_MouseEnter(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Firebrick;
        }

        private void panel7_MouseLeave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.FromArgb(58, 89, 114);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(comboBox1.SelectedIndex);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                string myConnection = "datasource=" + mainmenu.textbox_mysql_hostname.Text + ";port=" + mainmenu.textbox_mysql_port.Text + ";username=" + mainmenu.textbox_mysql_username.Text + ";password=" + mainmenu.textbox_mysql_pass.Text;
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter();
                //myDataAdapter.SelectCommand = new MySqlCommand("select * from auth.account;");
                MySqlCommandBuilder cb = new MySqlCommandBuilder(myDataAdapter);
                myConn.Open();
                DataSet ds = new DataSet();

                label_mysql_status2.Text = "Connected!";
                label_mysql_status2.ForeColor = Color.LawnGreen;

                myConn.Close();
            }
            catch (Exception /*ex*/)
            {
                //MessageBox.Show(ex.Message);
                label_mysql_status2.Text = "Connection Lost - MySQL is not running";
                label_mysql_status2.ForeColor = Color.Red;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //--------------------------------\\
           //  Max + 1 from Database - Button  \\
          //------------------------------------\\

            MySqlConnection connection = new MySqlConnection("datasource=" + mainmenu.textbox_mysql_hostname.Text + ";port=" + mainmenu.textbox_mysql_port.Text + ";username=" + mainmenu.textbox_mysql_username.Text + ";password=" + mainmenu.textbox_mysql_pass.Text);
            string insertQuery = "SELECT max(ID)+1 FROM " + mainmenu.textbox_mysql_worldDB.Text + ".quest_template;";
            //string insertQuery = textBox_SelectMaxPlus1.Text;
            connection.Open();
            MySqlCommand command = new MySqlCommand(insertQuery, connection);

            try
            {
                textBox1.Text = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QuestFlags_Form questFlags = new QuestFlags_Form();
            questFlags.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://trinitycore.atlassian.net/wiki/display/tc/quest_template");
        }

        private void label83_Click(object sender, EventArgs e)
        {
            GenerateSqlCode(sender, e);

            if (textBox1.Text == "")
            {
                MessageBox.Show("ID should not be empty", "Error");
                return;
            }
            if (textBox1.Text == "0")
            {
                MessageBox.Show("ID should not be 0", "Error");
                return;
            }
            if (textBox67.Text == "")
            {
                MessageBox.Show("Quest Title should not be empty", "Error");
                return;
            }
            if (textBox106.Text == "")
            {
                MessageBox.Show("Creature Quest Starter Entry should not be empty", "Error");
                return;
            }
            if (textBox109.Text == "")
            {
                MessageBox.Show("Creature Quest Ender Entry should not be empty", "Error");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "sql files (*.sql)|*.sql";
                sfd.FilterIndex = 2;
                //                                                 LogTitle
                sfd.FileName = "Quest[" + stringEntryShare + "]" + textBox67.Text;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, stringSQLShare);
                    label_saved.Visible = true;
                }
            }
        }

        private void label86_Click(object sender, EventArgs e)
        {
            GenerateSqlCode(sender, e);

            if (textBox1.Text == "")
            {
                MessageBox.Show("ID should not be empty", "Error");
                return;
            }
            if (textBox1.Text == "0")
            {
                MessageBox.Show("ID should not be 0", "Error");
                return;
            }
            if (textBox67.Text == "")
            {
                MessageBox.Show("Quest Title should not be empty", "Error");
                return;
            }
            if (textBox106.Text == "")
            {
                MessageBox.Show("Creature Quest Starter Entry should not be empty", "Error");
                return;
            }
            if (textBox109.Text == "")
            {
                MessageBox.Show("Creature Quest Ender Entry should not be empty", "Error");
                return;
            }

            Clipboard.SetText(stringSQLShare);
            label_copied_to_clipboard.Visible = true;
        }

        // Execut Query - button
        private void button2_Click(object sender, EventArgs e)
        {
            GenerateSqlCode(sender, e);

            if (textBox1.Text == "")
            {
                MessageBox.Show("ID should not be empty", "Error");
                return;
            }
            if (textBox1.Text == "0")
            {
                MessageBox.Show("ID should not be 0", "Error");
                return;
            }
            if (textBox67.Text == "")
            {
                MessageBox.Show("Quest Title should not be empty", "Error");
                return;
            }
            if (textBox106.Text == "")
            {
                MessageBox.Show("Creature Quest Starter Entry should not be empty", "Error");
                return;
            }
            if (textBox109.Text == "")
            {
                MessageBox.Show("Creature Quest Ender Entry should not be empty", "Error");
                return;
            }

            MySqlConnection connection = new MySqlConnection("datasource=" + mainmenu.textbox_mysql_hostname.Text + ";port=" + mainmenu.textbox_mysql_port.Text + ";username=" + mainmenu.textbox_mysql_username.Text + ";password=" + mainmenu.textbox_mysql_pass.Text);
            string insertQuery = stringSQLShare;
            connection.Open();
            MySqlCommand command = new MySqlCommand(insertQuery, connection);

            try
            {
                
                if (command.ExecuteNonQuery() >= 1)
                {
                    label_query_executed_successfully.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        //All textBoxes
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox107.Text = textBox1.Text;
            textBox108.Text = textBox1.Text;

            label_query_executed_successfully.Visible = false;
            label_copied_to_clipboard.Visible = false;
            label_saved.Visible = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox68.Text = comboBox2.Text;

            if (comboBox2.SelectedIndex == 0/*All Races*/) textBox68.Text = "0";
           else if (comboBox2.SelectedIndex == 1/*Horde Quest*/) textBox68.Text = "690";
            else if (comboBox2.SelectedIndex == 2/*Alliance Quest*/) textBox68.Text = "1101";
            else if (comboBox2.SelectedIndex == 3/*Human*/) textBox68.Text = "1";
            else if (comboBox2.SelectedIndex == 4/*Orc*/) textBox68.Text = "2";
            else if (comboBox2.SelectedIndex == 5/*Dwarf*/) textBox68.Text = "4";
            else if (comboBox2.SelectedIndex == 6/*Night Elf*/) textBox68.Text = "8";
            else if (comboBox2.SelectedIndex == 7/*Undead*/) textBox68.Text = "16";
            else if (comboBox2.SelectedIndex == 8/*Tauren*/) textBox68.Text = "32";
            else if (comboBox2.SelectedIndex == 9/*Gnome*/) textBox68.Text = "64";
            else if (comboBox2.SelectedIndex == 10/*Troll*/) textBox68.Text = "128";
            else if (comboBox2.SelectedIndex == 11/*Blood Elf*/) textBox68.Text = "512";
            else if (comboBox2.SelectedIndex == 12/*Draenei*/) textBox68.Text = "1024";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0) textBox105.Text = "INSERT";
            else if (comboBox3.SelectedIndex == 1) textBox105.Text = "REPLACE";
        }


        // All Numbers textBoxes
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // allow one minus symbol
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }
    }
}