﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NexusTKMapEditor
{
    public partial class FormObject : Form
    {
        public static readonly FormObject FormInstance = new FormObject();

        private readonly List<int> selectedColumns = new List<int>();
        private int focusedColumn = -1;
        private int sizeModifier;
        private bool onTop = true;
        private bool isMouseDown = false;
        private bool showGrid;
        private readonly short penSelectOffsetY = 50;
        //ResizeFunctionality
        private int useableHeight = 0;
        private readonly int maxObjectWindowWidth;
        private readonly int maxTilesX = 50;

        public bool ShowGrid
        {
            get { return showGrid; }
            set { showGrid = value; this.Invalidate(); }
        }

        public FormObject()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(frmObject_MouseWheel);
            maxObjectWindowWidth = this.Width * 4;
            
            
            //this.Paint += frmObject_Paint;
        }
        private void FormObject_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            
            if (e.KeyCode == Keys.Down)
            {
                foreach (Form mdiChild in Parent.Controls)
                {
                    if (mdiChild is FormMap)
                    {
                        FormMap map = (FormMap)mdiChild;
                        map.WindowState = FormWindowState.Minimized;
                        map.Show();
                        map.WindowState = FormWindowState.Normal;
                        return;

                    }
                }
            }
        }
        public static FormObject GetFormInstance()
        {
            return FormInstance;
        }

        private void frmObject_Load(object sender, EventArgs e)
        {
            Reload(false);
            this.KeyPreview = true;
            
            MinimumSize = new Size(MinimumSize.Width + 10, MinimumSize.Height + 10);
            MaximumSize = new Size(maxObjectWindowWidth, MaximumSize.Height + 10);
            if (this.BackgroundImage == null) this.BackgroundImage = new Bitmap(maxTilesX * sizeModifier, 12 * sizeModifier);
            menuStrip.Visible = false;
            sb1.Maximum = TileManager.ObjectInfos.Length / 12 + 8;
            RenderObjectset();
        }

        private int GetObjectNumber(int x)
        {
            return sb1.Value * 12 + x;
        }
        private void CreateBackground(Graphics g)
        {
            int alternate = 0;
            for(int x = 0; x < maxTilesX; x++)
            {
                alternate += 1;
                for (int y = 0; y < 12; y++)
                {
                    alternate += 1;
                    if(alternate % 2 == 0)
                        g.FillRectangle(Brushes.DarkGray, x * sizeModifier, y * sizeModifier, sizeModifier, sizeModifier);
                    else
                        g.FillRectangle(Brushes.Gray, x * sizeModifier, y * sizeModifier, sizeModifier, sizeModifier);
                }
            }
        }
        private void RenderObjectset()
        {
            //Bitmap tSet = new Bitmap(432, 432);
            if (this.BackgroundImage == null) this.BackgroundImage = new Bitmap((12 * sizeModifier)*maxTilesX, 12 * sizeModifier);
            Graphics g = Graphics.FromImage(this.BackgroundImage);
            g.Clear(Color.Empty);
            //CreateBackground(g);
            g.Clear(ImageRenderer.Singleton.clearColor);
            for (int x = 0; x < maxTilesX; x++)
            {
                for (int y = 0; y < 12; y++)
                {
                    if (x == 0 && sb1.Value == 0) continue;

                    int objectInfoIndex = GetObjectNumber(x);
                    if (objectInfoIndex >= TileManager.ObjectInfos.Length)
                        continue;
                    int objectInfoHeight = TileManager.ObjectInfos[objectInfoIndex].Height;

                    if ((11 - y) >= objectInfoHeight) continue;
                    int tile = TileManager.ObjectInfos[objectInfoIndex].Indices[(y + objectInfoHeight) % 12];

                    if (tile < TileManager.Epf[1].max)
                    {
                        //Bitmap bitmap = ImageRenderer.Singleton.GetObjectBitmap(tile);
                        g.DrawImage(ImageRenderer.Singleton.GetObjectBitmap(tile), x * sizeModifier, y * sizeModifier - penSelectOffsetY);//, 36, 36);
                        //bitmap = null;
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Black, x * sizeModifier, y * sizeModifier - penSelectOffsetY, sizeModifier, sizeModifier);
                    }
                }
            }

            g.Dispose();
            this.Invalidate();
            //this.BackgroundImage = tSet;
            //picObjectset.Image = tSet;
            //Application.DoEvents();
            //tSet = null;
        }

        private void sb1_Scroll(object sender, ScrollEventArgs e)
        {
            selectedColumns.Clear();
            RenderObjectset();
        }

        private void frmObject_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (ShowGrid)
            {
                Pen penGrid = new Pen(Color.LightCyan, 1);
                for (int i = 0; i < 12; i++)
                {
                    int objectInfoIndex = GetObjectNumber(i);
                    int objectInfoHeight = TileManager.ObjectInfos[objectInfoIndex].Height;
                    e.Graphics.DrawRectangle(penGrid, i * sizeModifier, (12 * sizeModifier) - (objectInfoHeight * sizeModifier), sizeModifier, objectInfoHeight * sizeModifier);
                }
                penGrid.Dispose();
            }

            if (selectedColumns.Count > 0)
            {
                Pen pen = new Pen(Color.Red, 2);
                foreach (var selectedColumn in selectedColumns)
                {
                    int objectInfoIndex = GetObjectNumber(selectedColumn);
                    int objectInfoHeight = TileManager.ObjectInfos[objectInfoIndex].Height;
                    e.Graphics.DrawRectangle(pen, selectedColumn * sizeModifier, (12 * sizeModifier) - (objectInfoHeight * sizeModifier) - penSelectOffsetY, sizeModifier, objectInfoHeight * sizeModifier);
                }
                pen.Dispose();
            }

            if (focusedColumn >= 0)
            {
                Pen pen = new Pen(Color.Green, 2);
                int objectInfoIndex = GetObjectNumber(focusedColumn);
                if (objectInfoIndex >= TileManager.ObjectInfos.Length)
                    return;
                int objectInfoHeight = TileManager.ObjectInfos[objectInfoIndex].Height;
                e.Graphics.DrawRectangle(pen, focusedColumn * sizeModifier, (12 * sizeModifier) - (objectInfoHeight * sizeModifier) - penSelectOffsetY, sizeModifier, objectInfoHeight * sizeModifier);
                pen.Dispose();
            }
        }

        private void frmObject_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (sb1.Value - 1 >= sb1.Minimum) sb1.Value--;
            }
            else if (e.Delta < 0)
            {
                if (sb1.Value + 1 <= sb1.Maximum) sb1.Value++;
            }

            sb1_Scroll(null, null);
        }

        private void frmObject_MouseMove(object sender, MouseEventArgs e)
        {
            int newFocusedColumn = e.X / sizeModifier;
            bool refresh = newFocusedColumn != focusedColumn;

            if (refresh)
            {
                frmObject_MouseClick(sender, e);
                focusedColumn = newFocusedColumn;
                this.Invalidate();
                int objectNumber = GetObjectNumber(newFocusedColumn);
                if (objectNumber == -1) return;
                //int objectInfoHeight = TileManager.ObjectInfos[objectNumber].Indices.Length;
                toolStripStatusLabel.Text = string.Format("Object number: {0}", objectNumber);
            }
        }

        private void frmObject_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            
            int selectedColumn = e.X / sizeModifier;
            bool refresh = selectedColumn != focusedColumn;

            if (!isMouseDown)
            {
                if (ModifierKeys == Keys.Control)
                {
                    if (selectedColumns.Contains(selectedColumn)) selectedColumns.Remove(selectedColumn);
                    else selectedColumns.Add(selectedColumn);
                }
                else
                {
                    selectedColumns.Clear();
                    selectedColumns.Add(selectedColumn);
                }
                TileManager.ObjectSelection = GetSelection();
                TileManager.LastSelection = TileManager.SelectionType.Object;
            }
            
            isMouseDown = true;
            //RenderObjectset();
        }

        private void frmObject_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            int selectedColumn = e.X / sizeModifier;
            bool refresh = selectedColumn != focusedColumn;
            
            if (refresh && isMouseDown)
            {
                if (selectedColumns.Contains(selectedColumn)) selectedColumns.Remove(selectedColumn);
                else selectedColumns.Add(selectedColumn);
                TileManager.ObjectSelection = GetSelection();
                TileManager.LastSelection = TileManager.SelectionType.Object;
            }
        }

        private void frmObject_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isMouseDown = false;
        }

        private Dictionary<Point, int> GetSelection()
        {
            Dictionary<Point, int> dictionary = new Dictionary<Point, int>();
            if (selectedColumns.Count == 0) return dictionary;

            int xMin = selectedColumns[0];

            foreach (int selectedColumn in selectedColumns)
                if (xMin > selectedColumn) xMin = selectedColumn;

            foreach (int selectedColumn in selectedColumns)
                dictionary.Add(new Point(selectedColumn - xMin, 0), GetObjectNumber(selectedColumn));

            return dictionary;
        }

        private void findObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumberInputForm numberInputForm = new NumberInputForm(@"Enter object number");
            if(numberInputForm.ShowDialog(this) == DialogResult.OK)
            {
                NavigateToObject(numberInputForm.Number);
            }
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGrid = showGridToolStripMenuItem.Checked;
        }

        private void NavigateToObject(int number)
        {
            if (number < 0 || number >= TileManager.ObjectInfos.Length - 11) return;

            int sbIndex = number/12;
            int x = number - sbIndex * 12;

            sb1.Value = sbIndex;
            selectedColumns.Clear();
            selectedColumns.Add(x);
            TileManager.ObjectSelection = GetSelection();
            TileManager.LastSelection = TileManager.SelectionType.Object;
            RenderObjectset();
        }

        private void FormObject_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void Reload(bool render)
        {
            sizeModifier = ImageRenderer.Singleton.sizeModifier;
            //MinimumSize = new Size(MinimumSize.Width + 10, MinimumSize.Height + 10);
            //MaximumSize = new Size(maxObjectWindowWidth, MaximumSize.Height + 10);

            SetClientSizeCore((12 * sizeModifier) - 1, (12 * sizeModifier) + 39);
            MinimumSize = new Size(ClientSize.Width + 6, ClientSize.Height + 24);
            MaximumSize = new Size(maxObjectWindowWidth, ClientSize.Height + 24);
            this.BackgroundImage = null;

            if (render) RenderObjectset();
        }

        

        

        private void showOntopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            onTop = !onTop;
            if (onTop)
            {
                this.TopMost = true;
                
                showOntopToolStripMenuItem.Checked = true;
            }
            else { 
                this.TopMost = false;
                showOntopToolStripMenuItem.Checked = false;
            }
            

        }
    }
}
