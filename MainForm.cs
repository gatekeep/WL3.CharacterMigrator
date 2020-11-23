/**
 * Wasteland 3 Character Migrator
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WL3.CharacterMigrator
{
    /// <summary>
    /// 
    /// </summary>
    public enum DifficultyLevels
    {
        Rookie = 0,
        Wastelander = 1,
        Ranger = 2,
        SuperJerk = 3
    } // public enum DifficultyLevels

    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly string localSaveGamePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            Path.Combine("My Games", "Wasteland3", "Save Games"));

        private Dictionary<string, XElement> sourcePCs = new Dictionary<string, XElement>();
        private List<string> sourceRosterPCs = new List<string>();

        private string sourceSavePath;
        private string sourceSaveFile;
        private SaveData sourceSave;

        private bool newGamePlus = false;

        private Dictionary<string, XElement> destPCs = new Dictionary<string, XElement>();
        private List<string> destRosterPCs = new List<string>();

        private string destSavePath;
        private string destSaveFile;
        private SaveData destSave;

        private DifficultyLevels currentDifficulty;
        private Dictionary<string, string> charactersToMigrate = new Dictionary<string, string>();
        private List<string> charactersToRoster = new List<string>();

        /**
         * Methods
         */

        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            difficultyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            // list view columns
            srcPcListView.Columns.Add("Display Name", -2, HorizontalAlignment.Left);
            srcPcListView.Columns.Add("Level", -2, HorizontalAlignment.Left);
            srcPcListView.Columns.Add("Rostered", -2, HorizontalAlignment.Left);
            dstPcListView.Columns.Add("Display Name", -2, HorizontalAlignment.Left);
            dstPcListView.Columns.Add("Level", -2, HorizontalAlignment.Left);
            dstPcListView.Columns.Add("Rostered", -2, HorizontalAlignment.Left);

            // hook events
            srcPcListView.ItemSelectionChanged += SrcPcListView_ItemSelectionChanged;
            difficultyComboBox.SelectedIndexChanged += DifficultyComboBox_SelectedIndexChanged;

            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Stream OpenStream(string fileName)
        {
            Assembly thisAssembly = Assembly.GetAssembly(typeof(MainForm));
            string directoryPath = Path.GetDirectoryName(fileName);
            directoryPath = directoryPath.Replace(Path.DirectorySeparatorChar, '.');
            if (!directoryPath.EndsWith("."))
                directoryPath += ".";
            if (directoryPath == ".")
                directoryPath = string.Empty;

            // so it seems the fucking god damn .NET vs Mono is different in this regard...
            // .NET will replace - with _ and Mono leaves them as-is when embedding files into the
            // executable assembly
            bool runningOnMono = Type.GetType("Mono.Runtime") != null;
            if (!runningOnMono && directoryPath.Contains("-"))
                directoryPath = directoryPath.Replace('-', '_');

            string resourceName = Path.GetFileName(fileName);

            string assemblyPath = thisAssembly.GetName().Name + ".Resource." + directoryPath + resourceName;
            List<string> embedTest = new List<string>(thisAssembly.GetManifestResourceNames());
            return thisAssembly.GetManifestResourceStream(assemblyPath);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Reset()
        {
            // 
            selectReplacementButton.Enabled = false;
            clearReplacementButton.Enabled = false;

            srcPcListView.SelectedItems.Clear();
            srcPcListView.Items.Clear();
            dstPcListView.SelectedItems.Clear();
            dstPcListView.Items.Clear();

            newGamePlus = false;
            newGameToolStripMenuItem.Enabled = false;

            sourcePCs = new Dictionary<string, XElement>();
            sourceRosterPCs = new List<string>();
            
            sourceSavePath = null;
            sourceSaveFile = null;
            sourceSave = null;


            destPCs = new Dictionary<string, XElement>();
            destRosterPCs = new List<string>();

            destSavePath = null;
            destSaveFile = null;
            destSave = null;

            charactersToMigrate = new Dictionary<string, string>();
            charactersToRoster = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="save"></param>
        /// <param name="pcList"></param>
        /// <param name="rosterPcList"></param>
        /// <param name="listView"></param>
        private void PopulateCharacters(SaveData save, ref Dictionary<string, XElement> pcList, ref List<string> rosterPcList, ref ListView listView)
        {
            // get active PC data from the XML source
            XElement pcs = save["pcs"];
            foreach (XElement pc in pcs.Elements())
            {
                if (pc != null)
                {
                    int companionId = Convert.ToInt32(pc.Element("companionId").Value);
                    if (companionId == -1)
                    {
                        string displayName = pc.Element("displayName").Value;
                        pcList.Add(displayName, pc);
                    }
                }
            }

            // get active PC data from the XML source
            XElement rosterPcs = save["rosterpcs"];
            foreach (XElement pc in rosterPcs.Elements())
            {
                if (pc != null)
                {
                    int companionId = Convert.ToInt32(pc.Element("companionId").Value);
                    if (companionId == -1)
                    {
                        string displayName = pc.Element("displayName").Value;
                        rosterPcList.Add(displayName);
                        pcList.Add(displayName, pc);
                    }
                }
            }

            // populate list view
            if (pcList.Count > 0)
            {
                foreach (KeyValuePair<string, XElement> pc in pcList)
                {
                    string displayName = pc.Key;
                    string level = pc.Value.Element("level").Value;
                    bool isRostered = rosterPcList.Contains(displayName);

                    listView.Items.Add(new ListViewItem(new string[] { displayName, level, (isRostered ? "True" : "False") }));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();

            //
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load Source Game Save";
            ofd.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            ofd.InitialDirectory = localSaveGamePath;

            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;

            sourceSaveFile = ofd.FileName;
            sourceSavePath = Path.GetDirectoryName(sourceSaveFile);
            sourceSave = SaveData.Load(ofd.FileName);

            PopulateCharacters(sourceSave, ref sourcePCs, ref sourceRosterPCs, ref srcPcListView);

            //
            ofd = new OpenFileDialog();
            ofd.Title = "Load Destination Game Save";
            ofd.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            ofd.InitialDirectory = localSaveGamePath;

            dr = ofd.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                newGameToolStripMenuItem.Enabled = true;
                destSavePath = localSaveGamePath;
                destSaveFile = null;

                return;
            }

            destSaveFile = ofd.FileName;
            destSavePath = Path.GetDirectoryName(destSaveFile);

            if (destSaveFile == sourceSaveFile)
            {
                MessageBox.Show("Cannot open the source as the destination save!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Reset();
                return;
            }

            destSave = SaveData.Load(ofd.FileName);

            PopulateCharacters(destSave, ref destPCs, ref destRosterPCs, ref dstPcListView);
            currentDifficulty = (DifficultyLevels)Convert.ToInt32(destSave["difficulty"].Value);
            difficultyComboBox.SelectedIndex = (int)currentDifficulty;

            selectReplacementButton.Enabled = true;
            clearReplacementButton.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Game Save";
            sfd.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (!newGamePlus && destSaveFile == null)
            {
                sfd.FileName = sourceSaveFile;
                sfd.InitialDirectory = sourceSavePath;

                DialogResult confirmDr = MessageBox.Show("This will overwrite your selected source game save! Are you sure?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (confirmDr == DialogResult.No)
                    return;
            }
            else
            {
                sfd.FileName = destSaveFile;
                sfd.InitialDirectory = destSavePath;
            }

            DialogResult dr = sfd.ShowDialog();

            if (dr == DialogResult.Cancel)
                return;

            int dataSize = 0;
            if (!newGamePlus && destSaveFile == null)
                sourceSave.Save(sfd.FileName, out dataSize);
            else
            {
                // update destination characters being migrated
                if (charactersToMigrate.Count > 0)
                {
                    Dictionary<string, string> reverseCharactersToMigrate = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> kvp in charactersToMigrate)
                        reverseCharactersToMigrate.Add(kvp.Value, kvp.Key);

                    IEnumerable<XElement> pcs = destSave["pcs"].Elements();
                    for (int i = 0; i < pcs.Count(); i++)
                    {
                        XElement pc = pcs.ElementAt(i);
                        if (pc != null)
                        {
                            int companionId = Convert.ToInt32(pc.Element("companionId").Value);
                            if (companionId == -1)
                            {
                                string displayName = pc.Element("displayName").Value;
                                if (reverseCharactersToMigrate.ContainsKey(displayName))
                                {
                                    string srcCharacterToReplaceWith = reverseCharactersToMigrate[displayName];
                                    pc.ReplaceWith(sourcePCs[srcCharacterToReplaceWith]);
                                    pc = pcs.ElementAt(i);

                                    XElement dstRotation = destPCs[displayName].Element("rotation");
                                    XElement rotation = pc.Element("rotation");
                                    rotation.ReplaceWith(dstRotation);

                                    XElement dstPosition = destPCs[displayName].Element("position");
                                    XElement position = pc.Element("position");
                                    position.ReplaceAll(dstPosition);
                                }
                            }
                        }
                    }
                }

                // update destination characters being rostered
                if (charactersToRoster.Count > 0)
                {
                    XElement rosterPCs = destSave["rosterpcs"];
                    foreach (string rosterCharacter in charactersToRoster)
                    {
                        XElement pc = sourcePCs[rosterCharacter];
                        rosterPCs.Add(pc);
                    }
                }

                // modify difficulty
                destSave["difficulty"].Value = ((int)currentDifficulty).ToString();
                if (currentDifficulty == DifficultyLevels.Ranger || currentDifficulty == DifficultyLevels.SuperJerk)
                    destSave["friendlyFire"].Value = "true";
                else
                    destSave["friendlyFire"].Value = "false";

                destSave.Save(sfd.FileName, out dataSize);
            }

            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGamePlus = true;
            Stream templateSave = OpenStream("Template Game Save.xml");

            destSave = SaveData.Load(templateSave);
            destSaveFile = "New Game.xml";

            PopulateCharacters(destSave, ref destPCs, ref destRosterPCs, ref dstPcListView);

            selectReplacementButton.Enabled = true;
            clearReplacementButton.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectReplacementButton_Click(object sender, EventArgs e)
        {
            if (srcPcListView.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Select a character to copy.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string srcPcName = srcPcListView.SelectedItems[0].Text;
            if (charactersToMigrate.ContainsKey(srcPcName) || charactersToRoster.Contains(srcPcName))
            {
                MessageBox.Show("Character already set to migrate or roster!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dstPcListView.SelectedItems.Count <= 0)
            {
                if (!charactersToRoster.Contains(srcPcName))
                {
                    charactersToRoster.Add(srcPcName);

                    string level = sourcePCs[srcPcName].Element("level").Value;
                    dstPcListView.Items.Add(new ListViewItem(new string[] { srcPcName, level }));
                }

                return;
            }

            string dstPcName = dstPcListView.SelectedItems[0].Text;
            if (destRosterPCs.Contains(dstPcName))
            {
                if (!charactersToRoster.Contains(srcPcName))
                {
                    charactersToRoster.Add(srcPcName);

                    string level = sourcePCs[srcPcName].Element("level").Value;
                    dstPcListView.Items.Add(new ListViewItem(new string[] { srcPcName, level }));
                }

                return;
            }

            if (!charactersToMigrate.ContainsKey(srcPcName))
            {
                charactersToMigrate.Add(srcPcName, dstPcName);

                // update destination list view
                foreach (ListViewItem item in dstPcListView.Items)
                {
                    if (item.Text.Contains(dstPcName))
                    {
                        item.Text = string.Format("{0} ({1})", srcPcName, dstPcName);
                        item.SubItems[1].Text = sourcePCs[srcPcName].Element("level").Value;
                        break;
                    }
                }
            }
            else
            {
                string origDstPcName = charactersToMigrate[srcPcName];
                charactersToMigrate[srcPcName] = dstPcName;

                // update destination list view
                foreach (ListViewItem item in dstPcListView.Items)
                {
                    if (item.Text.Contains(origDstPcName))
                    {
                        item.Text = string.Format("{0}", origDstPcName);
                        item.SubItems[1].Text = destPCs[origDstPcName].Element("level").Value;
                        break;
                    }
                }

                // update destination list view
                foreach (ListViewItem item in dstPcListView.Items)
                {
                    if (item.Text.Contains(dstPcName))
                    {
                        item.Text = string.Format("{0} ({1})", srcPcName, dstPcName);
                        item.SubItems[1].Text = sourcePCs[srcPcName].Element("level").Value;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearReplacementButton_Click(object sender, EventArgs e)
        {
            if (dstPcListView.SelectedItems.Count <= 0)
                return;

            string srcPcName = null;
            string dstPcName = dstPcListView.SelectedItems[0].Text;

            string[] splitName = dstPcName.Split('(', ')');
            if (splitName.Length > 1)
            {
                dstPcName = splitName[1];

                foreach (KeyValuePair<string, string> kvp in charactersToMigrate)
                {
                    if (kvp.Value == dstPcName)
                    {
                        srcPcName = kvp.Key;
                        break;
                    }
                }
            }

            if (srcPcName == null)
            {
                dstPcName = dstPcListView.SelectedItems[0].Text;

                // is this a character to be rostered?
                if (charactersToRoster.Contains(dstPcName))
                {
                    charactersToRoster.Remove(dstPcName);

                    // update destination list view
                    int idxToRemove = -1;
                    foreach (ListViewItem item in dstPcListView.Items)
                    {
                        if (item.Text.Contains(dstPcName))
                        {
                            idxToRemove = item.Index;
                            break;
                        }
                    }

                    dstPcListView.Items.RemoveAt(idxToRemove);
                }

                return;
            }

            // update destination list view
            foreach (ListViewItem item in dstPcListView.Items)
            {
                if (item.Text.Contains(dstPcName))
                {
                    item.Text = string.Format("{0}", dstPcName);
                    item.SubItems[1].Text = destPCs[dstPcName].Element("level").Value;
                    break;
                }
            }

            charactersToMigrate.Remove(srcPcName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SrcPcListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            dstPcListView.SelectedItems.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DifficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIdx = difficultyComboBox.SelectedIndex;
            currentDifficulty = (DifficultyLevels)selectedIdx;
        }
    } // public partial class MainForm : Form
} // namespace WL3.CharacterMigrator
