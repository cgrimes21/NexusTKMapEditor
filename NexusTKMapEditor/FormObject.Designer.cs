﻿namespace NexusTKMapEditor
{
    partial class FormObject
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.sb1 = new System.Windows.Forms.HScrollBar();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOntopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormObject_KeyDown);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 389);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(434, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // sb1
            // 
            this.sb1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sb1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sb1.Location = new System.Drawing.Point(0, 372);
            this.sb1.Name = "sb1";
            this.sb1.Size = new System.Drawing.Size(434, 17);
            this.sb1.TabIndex = 3;
            this.sb1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sb1_Scroll);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(434, 24);
            this.menuStrip.TabIndex = 6;
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findObjectToolStripMenuItem});
            this.editToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // findObjectToolStripMenuItem
            // 
            this.findObjectToolStripMenuItem.Name = "findObjectToolStripMenuItem";
            this.findObjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findObjectToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.findObjectToolStripMenuItem.Text = "&Find Object";
            this.findObjectToolStripMenuItem.Click += new System.EventHandler(this.findObjectToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showGridToolStripMenuItem,
            this.showOntopToolStripMenuItem});
            this.viewToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.CheckOnClick = true;
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showGridToolStripMenuItem.Text = "Show &Grid";
            this.showGridToolStripMenuItem.Click += new System.EventHandler(this.showGridToolStripMenuItem_Click);
            // 
            // showOntopToolStripMenuItem
            // 
            this.showOntopToolStripMenuItem.Checked = true;
            this.showOntopToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOntopToolStripMenuItem.Name = "showOntopToolStripMenuItem";
            this.showOntopToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showOntopToolStripMenuItem.Text = "Show Ontop";
            this.showOntopToolStripMenuItem.Click += new System.EventHandler(this.showOntopToolStripMenuItem_Click);
            // 
            // FormObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(434, 411);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.sb1);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormObject";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Objects";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormObject_FormClosing);
            this.Load += new System.EventHandler(this.frmObject_Load);
            this.SizeChanged += new System.EventHandler(this.FormObject_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmObject_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmObject_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmObject_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmObject_MouseUp);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findObjectToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.HScrollBar sb1;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;

        #endregion

        private System.Windows.Forms.ToolStripMenuItem showOntopToolStripMenuItem;
    }
}