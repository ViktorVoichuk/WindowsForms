using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clocl
{
    public partial class MainForm : Form
    {
        ColorDialog backgroundColorDialog;
        ColorDialog foregroundColofDialog;
        FontDialog fontDialog;
        public MainForm()
        {
            InitializeComponent();
            this.TransparencyKey = Color.Empty;
            backgroundColorDialog = new ColorDialog();
            foregroundColofDialog = new ColorDialog();
            fontDialog = new FontDialog();
            SetVisibility(false);
            this.Location = new Point
                (
                System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - this.Width,
                50
                );
            this.Text += $"location: {this.Location.X}x{this.Location.Y}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");
            if (cbShowDate.Checked)
            {
                labelTime.Text += $"\n{DateTime.Today.ToString("dd.MM.yyyy")}";
            }
        }

        private void SetVisibility(bool visible)
        {
            this.TransparencyKey = visible ? Color.Empty : this.BackColor;
            this.FormBorderStyle = visible ? FormBorderStyle.Sizable : FormBorderStyle.None;
            this.ShowInTaskbar = visible;
            cbShowDate.Visible = visible;
            btnHideControls.Visible = visible;
            labelTime.BackColor = visible ? Color.Empty : Color.Coral;
        }

        private void btnHideControls_Click(object sender, EventArgs e)
        {
            //SetVisibility(false);
            showControlsToolStripMenuItem.Checked = false;
            notifyIconSystenTray.ShowBalloonTip(3, "Важная информация", "Для того чтобы вернуть как было, нужно ткнуть 2 раза по часам или по этой иконке", ToolTipIcon.Info);
        }

        private void labelTime_DoubleClick(object sender, EventArgs e)
        {
            //SetVisibility(true);
            showControlsToolStripMenuItem.Checked = true;
        }

        private void notifyIconSystenTray_MouseMove(object sender, MouseEventArgs e)
        {
            notifyIconSystenTray.Text = "Current time:\n" + labelTime.Text;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void topmostToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = topmostToolStripMenuItem.Checked;
        }

        private void showControlsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibility(((ToolStripMenuItem)sender).Checked);
        }

        private void showDateToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            cbShowDate.Checked = ((ToolStripMenuItem)sender).Checked;
        }

        private void cbShowDate_CheckedChanged(object sender, EventArgs e)
        {
            showDateToolStripMenuItem.Checked = ((CheckBox)sender).Checked;
        }

        private void notifyIconSystenTray_DoubleClick(object sender, EventArgs e)
        {
            if (!this.TopMost)
            {
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        private void foregroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(foregroundColofDialog.ShowDialog(this) == DialogResult.OK)
            {
                labelTime.ForeColor = foregroundColofDialog.Color;
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backgroundColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                labelTime.BackColor = backgroundColorDialog.Color;
            }
        }

        private void fontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                labelTime.Font = fontDialog.Font;
            }
        }

    }
}
