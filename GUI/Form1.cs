using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        bool drawing;//global peremennie dlja risovanika
        GraphicsPath currentPath;
        Point oldLocation;
        Pen currentPen;
        Color historyColor;
        public Form1()
        {
            InitializeComponent();
            drawing = false;//peremennaja otvetstvena za risovanie
            currentPen = new Pen(Color.Black);//inizilizoravili pero -4ernoe
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)//menu panel, button new
        {
            Bitmap pic = new Bitmap(699, 307);//razmeri PictureBox
            picDrawingSurface.Image = pic;

            //if user in one time want create 2 files, we make possibility to save first file
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save current picture before creating new?", "Alert", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No:break;
                    case DialogResult.Yes:saveToolStripMenuItem_Click(sender, e);break;
                    case DialogResult.Cancel: return;
                }
            }

        }

        

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)//menu panel, button save
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*png";
            SaveDlg.Title = "Save an Image File";
            SaveDlg.FilterIndex = 4;//by default choosed .png
            SaveDlg.ShowDialog();
            if (SaveDlg.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)SaveDlg.OpenFile();
                switch (SaveDlg.FilterIndex)
                {
                    case 1:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Png);
                        break;
                }
                fs.Close();
            }
           

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)//menu panel, button open
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;//by default choosed .jpg

            //download pic in PictureBox
            if (OP.ShowDialog() != DialogResult.Cancel)
                picDrawingSurface.Load(OP.FileName);
            picDrawingSurface.AutoSize = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)//ToolStrip, new
        {
            Bitmap pic = new Bitmap(699, 307);//razmeri PictureBox
            picDrawingSurface.Image = pic;

            //if user in one time want create 2 files, we make possibility to save first file
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save current picture before creating new?", "Alert", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)//ToolStrip, save
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*png";
            SaveDlg.Title = "Save an Image File";
            SaveDlg.FilterIndex = 4;//by default choosed .png
            SaveDlg.ShowDialog();
            if (SaveDlg.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)SaveDlg.OpenFile();
                switch (SaveDlg.FilterIndex)
                {
                    case 1:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Png);
                        break;
                }
                fs.Close();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)//ToolStrip, open
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;//by default choosed .jpg

            //download pic in PictureBox
            if (OP.ShowDialog() != DialogResult.Cancel)
                picDrawingSurface.Load(OP.FileName);
            picDrawingSurface.AutoSize = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)//ToolStrip, exit
        {
            Application.Exit();
        }

        private void picDrawingSurface_MouseDown(object sender, MouseEventArgs e)//MouseDown from PictureBox, otve4aet za nazatuju knopku, drawing=true or false
        {
            if (picDrawingSurface.Image == null)//if PictureBox not initialized,then shows message, the programs does't go wrong
            {
                MessageBox.Show("Create a new fail!");
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
                currentPen = new Pen(Color.Black);
            }
            if (e.Button == MouseButtons.Right)//LASTIK, esli nazata pravaja knopka,
            {
                historyColor = currentPen.Color;//historycolor zapisivaet tekushij zvet ?????pravilno, ???
                currentPen = new Pen(Color.White);//menaem tekushij zvet na belij
            }
        }

        private void picDrawingSurface_MouseUp(object sender, MouseEventArgs e)//MouseUp-otve4aet za otpushennuju knopku
        {
            drawing = false;
            try
            {
                currentPath.Dispose();
            }
            catch (Exception)
            { };
        }

        private void picDrawingSurface_MouseMove(object sender, MouseEventArgs e)//MouseMove-otve4aet za peremeshenie,risovanie
        {
            if (drawing)
            {
                Graphics g = Graphics.FromImage(picDrawingSurface.Image);
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(currentPen, currentPath);
                oldLocation = e.Location;
                g.Dispose();
                picDrawingSurface.Invalidate();
            }
        }

        
    }
}
