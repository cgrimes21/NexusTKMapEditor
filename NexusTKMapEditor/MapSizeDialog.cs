using System.Drawing;
using System.Windows.Forms;

namespace NexusTKMapEditor
{
    public partial class MapSizeDialog : Form
    {
        public Size MapSize { get; private set; }
       
        public MapSizeDialog(Size currentSize)
        {
            InitializeComponent();
            numericUpDownWidth.Value = new decimal(currentSize.Width);
            numericUpDownHeight.Value = new decimal(currentSize.Height);
        }

        private void MapSizeDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            MapSize = new Size((int)numericUpDownWidth.Value, (int)numericUpDownHeight.Value);
        }
    }
}
