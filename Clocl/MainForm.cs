using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using Clocl.Properties;

namespace Clocl
{
    public partial class MainForm : Form
    {

        ColorDialog backgroundColorDialog;
        ColorDialog foregroundColofDialog;
        FontDialog fontDialog;
        ChooseFont chooseFontDialog;
        public MainForm()
        {
            InitializeComponent();
            SetFontDirectory();
            this.TransparencyKey = Color.Empty;
            backgroundColorDialog = new ColorDialog();
            foregroundColofDialog = new ColorDialog();
            fontDialog = new FontDialog();

            chooseFontDialog = new ChooseFont();

            backgroundColorDialog.Color = Color.Black;
            foregroundColofDialog.Color = Color.Blue;
            labelTime.ForeColor = foregroundColofDialog.Color;
            labelTime.BackColor = backgroundColorDialog.Color;
            
            SetVisibility(false);
            this.Location = new Point
                (
                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - this.Width,
                    50
                );
            this.Text += $"location: {this.Location.X}x{this.Location.Y}";
            LoadSettings();
        }


        void GetLocation()
        {
            this.Location = new Point
                    (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - (labelTime.Width + 70),
                    50
                    );
        }

        void SetFontDirectory()
        {
            string location = Assembly.GetEntryAssembly().Location; //Получаем полный адрес исполняемого файла 
            string path = Path.GetDirectoryName(location);        //Из адреса извлекаем путь к файлу
            //MessageBox.Show(path);
            Directory.SetCurrentDirectory($"{path}\\..\\..\\Fonts");//переходим в каталог со шрифтами
            //MessageBox.Show(Directory.GetCurrentDirectory());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");
            if (cbShowDate.Checked)
            {
                labelTime.Text += $"\n{DateTime.Today.ToString("dd.MM.yyyy")}";
            }
            //GetLocation();
        }

        private void SetVisibility(bool visible)
        {
            this.TransparencyKey = visible ? Color.Empty : this.BackColor;
            this.FormBorderStyle = visible ? FormBorderStyle.Sizable : FormBorderStyle.None;
            this.ShowInTaskbar = visible;
            cbShowDate.Visible = visible;
            btnHideControls.Visible = visible;
            //labelTime.BackColor = visible ? Color.Empty : Color.Coral;
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
            if (foregroundColofDialog.ShowDialog(this) == DialogResult.OK)
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
            if (chooseFontDialog.ShowDialog(this) == DialogResult.OK)
            {
                labelTime.Font = chooseFontDialog.ChosenFont;
            }
        }

        private void labelTime_MouseDown(object sender, MouseEventArgs e)
        {
            labelTime.Capture = false; //захват мыши окном labelTime
            Message n = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);//подменяет значение для системы о зажатии левой кнопки мыши
            this.WndProc(ref n);

        }

        private void loadOnWindowsStartupToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            const string name = "clock";
            string Path = Assembly.GetExecutingAssembly().Location;
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            if (loadOnWindowsStartupToolStripMenuItem.Checked)
            {
                reg.SetValue(name, Path);
            }
            else { reg.DeleteValue(name); }
            reg.Flush();
            reg.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Topmost = topmostToolStripMenuItem.Checked;
            Settings.Default.ShowControl = showControlsToolStripMenuItem.Checked;
            Settings.Default.ShowDate = showDateToolStripMenuItem.Checked;
            Settings.Default.LoadOnWND = loadOnWindowsStartupToolStripMenuItem.Checked;
            Settings.Default.FColor = labelTime.ForeColor;
            Settings.Default.BColor = labelTime.BackColor;
            Settings.Default.Font = labelTime.Font;
            Settings.Default.Location = this.Location;
            Settings.Default.Save();
        }
        private void LoadSettings()
        {
            topmostToolStripMenuItem.Checked = Properties.Settings.Default.Topmost;
            showControlsToolStripMenuItem.Checked = Properties.Settings.Default.ShowControl;
            showDateToolStripMenuItem.Checked = Properties.Settings.Default.ShowDate;
            loadOnWindowsStartupToolStripMenuItem.Checked = Properties.Settings.Default.LoadOnWND;
            labelTime.ForeColor = Properties.Settings.Default.FColor;
            labelTime.BackColor = Properties.Settings.Default.BColor;
            labelTime.Font = Properties.Settings.Default.Font;
            this.Location = Properties.Settings.Default.Location;
        }
    }
}
