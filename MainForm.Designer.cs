/**
 * Wasteland 3 Character Migrator
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */

namespace WL3.CharacterMigrator
{
    partial class MainForm
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
            System.Windows.Forms.Label srcPcCharactersLabel;
            System.Windows.Forms.Label dstPcCharactersLabel;
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pcGroupBox = new System.Windows.Forms.GroupBox();
            this.clearReplacementButton = new System.Windows.Forms.Button();
            this.selectReplacementButton = new System.Windows.Forms.Button();
            this.dstPcListView = new System.Windows.Forms.ListView();
            this.srcPcListView = new System.Windows.Forms.ListView();
            srcPcCharactersLabel = new System.Windows.Forms.Label();
            dstPcCharactersLabel = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.pcGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // srcPcCharactersLabel
            // 
            srcPcCharactersLabel.AutoSize = true;
            srcPcCharactersLabel.Location = new System.Drawing.Point(6, 16);
            srcPcCharactersLabel.Name = "srcPcCharactersLabel";
            srcPcCharactersLabel.Size = new System.Drawing.Size(98, 13);
            srcPcCharactersLabel.TabIndex = 4;
            srcPcCharactersLabel.Text = "Source Characters:";
            // 
            // dstPcCharactersLabel
            // 
            dstPcCharactersLabel.AutoSize = true;
            dstPcCharactersLabel.Location = new System.Drawing.Point(301, 16);
            dstPcCharactersLabel.Name = "dstPcCharactersLabel";
            dstPcCharactersLabel.Size = new System.Drawing.Size(117, 13);
            dstPcCharactersLabel.TabIndex = 6;
            dstPcCharactersLabel.Text = "Destination Characters:";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.newGameToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(545, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Enabled = false;
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.newGameToolStripMenuItem.Text = "New Game+";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pcGroupBox
            // 
            this.pcGroupBox.Controls.Add(this.clearReplacementButton);
            this.pcGroupBox.Controls.Add(this.selectReplacementButton);
            this.pcGroupBox.Controls.Add(dstPcCharactersLabel);
            this.pcGroupBox.Controls.Add(this.dstPcListView);
            this.pcGroupBox.Controls.Add(srcPcCharactersLabel);
            this.pcGroupBox.Controls.Add(this.srcPcListView);
            this.pcGroupBox.Location = new System.Drawing.Point(12, 27);
            this.pcGroupBox.Name = "pcGroupBox";
            this.pcGroupBox.Size = new System.Drawing.Size(522, 343);
            this.pcGroupBox.TabIndex = 3;
            this.pcGroupBox.TabStop = false;
            // 
            // clearReplacementButton
            // 
            this.clearReplacementButton.Enabled = false;
            this.clearReplacementButton.Location = new System.Drawing.Point(223, 193);
            this.clearReplacementButton.Name = "clearReplacementButton";
            this.clearReplacementButton.Size = new System.Drawing.Size(75, 23);
            this.clearReplacementButton.TabIndex = 8;
            this.clearReplacementButton.Text = "<<";
            this.clearReplacementButton.UseVisualStyleBackColor = true;
            this.clearReplacementButton.Click += new System.EventHandler(this.clearReplacementButton_Click);
            // 
            // selectReplacementButton
            // 
            this.selectReplacementButton.Enabled = false;
            this.selectReplacementButton.Location = new System.Drawing.Point(223, 164);
            this.selectReplacementButton.Name = "selectReplacementButton";
            this.selectReplacementButton.Size = new System.Drawing.Size(75, 23);
            this.selectReplacementButton.TabIndex = 7;
            this.selectReplacementButton.Text = ">>";
            this.selectReplacementButton.UseVisualStyleBackColor = true;
            this.selectReplacementButton.Click += new System.EventHandler(this.selectReplacementButton_Click);
            // 
            // dstPcListView
            // 
            this.dstPcListView.FullRowSelect = true;
            this.dstPcListView.HideSelection = false;
            this.dstPcListView.Location = new System.Drawing.Point(304, 32);
            this.dstPcListView.MultiSelect = false;
            this.dstPcListView.Name = "dstPcListView";
            this.dstPcListView.ShowGroups = false;
            this.dstPcListView.Size = new System.Drawing.Size(211, 305);
            this.dstPcListView.TabIndex = 5;
            this.dstPcListView.UseCompatibleStateImageBehavior = false;
            this.dstPcListView.View = System.Windows.Forms.View.Details;
            // 
            // srcPcListView
            // 
            this.srcPcListView.FullRowSelect = true;
            this.srcPcListView.HideSelection = false;
            this.srcPcListView.Location = new System.Drawing.Point(6, 32);
            this.srcPcListView.MultiSelect = false;
            this.srcPcListView.Name = "srcPcListView";
            this.srcPcListView.ShowGroups = false;
            this.srcPcListView.Size = new System.Drawing.Size(211, 305);
            this.srcPcListView.TabIndex = 3;
            this.srcPcListView.UseCompatibleStateImageBehavior = false;
            this.srcPcListView.View = System.Windows.Forms.View.Details;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 381);
            this.Controls.Add(this.pcGroupBox);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(561, 420);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(561, 420);
            this.Name = "MainForm";
            this.Text = "Wasteland 3 Character Migrator";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.pcGroupBox.ResumeLayout(false);
            this.pcGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox pcGroupBox;
        private System.Windows.Forms.Button clearReplacementButton;
        private System.Windows.Forms.Button selectReplacementButton;
        private System.Windows.Forms.ListView dstPcListView;
        private System.Windows.Forms.ListView srcPcListView;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
    }
}

