using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace project13
{
    public partial class MainForm : Form
    {
        Color paint;
        bool choose = false;
        bool draw = false;
        int x, y, lx, ly = 0;
        Item currItem;


        public MainForm()
        {
            InitializeComponent();
            
        }

        bool isTopPanelDragged = false;
        bool isLeftPanelDragged = false;
        bool isRightPanelDragged = false;
        bool isBottomPanelDragged = false;
        bool isTopBorderPanelDragged = false;
        

        bool isRightBottomPanelDragged = false;
        bool isLeftBottomPanelDragged = false;
        bool isRightTopPanelDragged = false;
        bool isLeftTopPanelDragged = false;

        bool isWindowMaximized = false;
        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;

        public enum Item
        {
            Rectangle, Line, Eraser, Text
        }       

        //********************************************************************
        // TopBorderPanel
        private void TopBorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopBorderPanelDragged = true;
            }
            else
            {
                isTopBorderPanelDragged = false;
            }
        }

        private void TopBorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y)
            {
                if (isTopBorderPanelDragged)
                {
                    if (this.Height < 50)
                    {
                        this.Height = 50;
                        isTopBorderPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                    }
                }
            }
        }

        private void TopBorderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopBorderPanelDragged = false;
        }
        //********************************************************************
        // TopPanel
        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopPanelDragged = true;
                Point pointStartPosition = this.PointToScreen(new Point(e.X, e.Y));
                offset = new Point();
                offset.X = this.Location.X - pointStartPosition.X;
                offset.Y = this.Location.Y - pointStartPosition.Y;
            }
            else
            {
                isTopPanelDragged = false;
            }
            if (e.Clicks == 2)
            {
                isTopPanelDragged = false;
                minMaxButton_Click(sender, e);
            }
        }

        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopPanelDragged)
            {
                Point newPoint = TopPanel.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(offset);
                this.Location = newPoint;

                if (this.Location.X > 2 || this.Location.Y > 2)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.Location = _normalWindowLocation;
                        this.Size = _normalWindowSize;
                        toolTip1.SetToolTip(minMaxButton, "Maximize");
                        minMaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
                        isWindowMaximized = false;
                    }
                }
            }
        }

        private void TopPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopPanelDragged = false;
            if (this.Location.Y <= 5)
            {
                if (!isWindowMaximized)
                {
                    _normalWindowSize = this.Size;
                    _normalWindowLocation = this.Location;

                    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                    this.Location = new Point(0, 0);
                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                    toolTip1.SetToolTip(minMaxButton, "Restore Down");
                    minMaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
                    isWindowMaximized = true;
                }
            }
        }


        //********************************************************************
        // LeftPanel
        private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Location.X <= 0 || e.X < 0)
            {
                isLeftPanelDragged = false;
                this.Location = new Point(10, this.Location.Y);
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    isLeftPanelDragged = true;
                }
                else
                {
                    isLeftPanelDragged = false;
                }
            }
        }

        private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftPanelDragged)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        isLeftPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
                    }
                }
            }
        }

        private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftPanelDragged = false;
        }


        //********************************************************************
        // RightPanel
        private void RightPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isRightPanelDragged = true;
            }
            else
            {
                isRightPanelDragged = false;
            }
        }

        private void RightPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightPanelDragged)
            {
                if (this.Width < 100)
                {
                    this.Width = 100;
                    isRightPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                    FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
                }
            }
        }

        private void RightPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isRightPanelDragged = false;
        }


        //********************************************************************
        // BottomPanel
        private void BottomPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isBottomPanelDragged = true;
            }
            else
            {
                isBottomPanelDragged = false;
            }
        }

        private void BottomPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBottomPanelDragged)
            {
                if (this.Height < 50)
                {
                    this.Height = 50;
                    isBottomPanelDragged = false;
                }
                else
                {
                    this.Height = this.Height + e.Y;
                }
            }
        }

        private void BottomPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isBottomPanelDragged = false;
        }


        //********************************************************************
        // RightBottomPanel 1
        private void RightBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = true;
        }

        private void RightBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomPanelDragged)
            {
                if (this.Width < 100 || this.Height < 50)
                {
                    this.Width = 100;
                    this.Height = 50;
                    isRightBottomPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                    this.Height = this.Height + e.Y;
                    FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
                }
            }
        }

        private void RightBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = false;
        }

        //********************************************************************
        // RightBottomPanel 2
        private void RightBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseDown(sender, e);
        }

        private void RightBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseMove(sender, e);
        }

        private void RightBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseUp(sender, e);
        }


        //********************************************************************
        // LeftBottomPanel 1
        private void LeftBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = true;
        }

        private void LeftBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftBottomPanelDragged || this.Height < 50)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        this.Height = 50;
                        isLeftBottomPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Height = this.Height + e.Y;
                        FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
                    }
                }
            }
        }

        private void LeftBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = false;
        }


        //********************************************************************
        // LeftBottomPanel 2
        private void LeftBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseDown(sender, e);
        }

        private void LeftBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseMove(sender, e);
        }

        private void LeftBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseUp(sender, e);
        }



        //********************************************************************
        // RightTopPanel 1
        private void RightTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = true;
        }

        private void RightTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y || e.X < this.Location.X)
            {
                if (isRightTopPanelDragged)
                {
                    if (this.Height < 50 || this.Width < 100)
                    {
                        this.Height = 50;
                        this.Width = 100;
                        isRightTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                        this.Width = this.Width + e.X;
                        FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
                    }
                }
            }
        }

        private void RightTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = false;
        }

        //********************************************************************
        // RightTopPanel 2
        private void RightTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseDown(sender, e);
        }

        private void RightTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseMove(sender, e);
        }

        private void RightTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseUp(sender, e);
        }


        //********************************************************************
        // LeftTopPanel 1
        private void LeftTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = true;
        }

        private void LeftTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X || e.Y < this.Location.Y)
            {
                if (isLeftTopPanelDragged)
                {
                    if (this.Width < 100 || this.Height < 50)
                    {
                        this.Width = 100;
                        this.Height = 100;
                        isLeftTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                        FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
                    }
                }
            }

        }

        private void LeftTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = false;
        }


        //********************************************************************
        // LeftTopPanel 2
        private void LeftTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseDown(sender, e);
        }

        private void LeftTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseMove(sender, e);
        }

        private void LeftTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseUp(sender, e);
        }

        //FormText
        private void FormText_MouseDown(object sender, MouseEventArgs e)
        {
            TopPanel_MouseDown(sender, e);
        }

        private void FormText_MouseMove(object sender, MouseEventArgs e)
        {
            TopPanel_MouseMove(sender, e);
        }

        private void FormText_MouseUp(object sender, MouseEventArgs e)
        {
            TopPanel_MouseUp(sender, e);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minMaxButton_Click(object sender, EventArgs e)
        {
            if (isWindowMaximized)
            {
                this.Location = _normalWindowLocation;
                this.Size = _normalWindowSize;
                toolTip1.SetToolTip(minMaxButton, "Maximize");
                minMaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
                isWindowMaximized = false;

                FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length * 2,
                                            9);
            }
            else
            {
                _normalWindowSize = this.Size;
                _normalWindowLocation = this.Location;

                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(0, 0);
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                toolTip1.SetToolTip(minMaxButton, "Restore Down");
                minMaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
                isWindowMaximized = true;

                FormText.Location = new Point((TopPanel.Width / 2) - FormText.Text.Length,9);
            }
        }

        private void minButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FontFamily[] family = FontFamily.Families;
            foreach(FontFamily font in family)
            {
                toolStripComboBox2.Items.Add(font.GetName(1).ToString());
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            x = e.X;
            y = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
            lx = e.X;
            ly = e.Y;
            if(currItem == Item.Line)
            {
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawLine(new Pen(new SolidBrush(paint)), new Point(x, y), new Point(lx, ly));
                g.Dispose();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Graphics g = pictureBox1.CreateGraphics();
                switch (currItem)
                {
                    case Item.Rectangle:
                        g.FillRectangle(new SolidBrush(paint), x, y, e.X - x, e.Y - y);
                        break;
                    case Item.Eraser:
                        g.FillRectangle(new SolidBrush(pictureBox1.BackColor), x, y, e.X - x, e.Y - y);
                        break;
                }
                g.Dispose();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            currItem = Item.Rectangle;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            currItem = Item.Line;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            currItem = Item.Eraser;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            currItem = Item.Text;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            if (currItem == Item.Text)
            {
                if (toolStripComboBox4.Text == "Regular")
                {
                    g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                }
                else if (toolStripComboBox4.Text == "Bold")
                {
                    g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                }
                else if (toolStripComboBox4.Text == "Underline")
                {
                    g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                }
                else if (toolStripComboBox4.Text == "Strikeout")
                {
                    g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                }
                else if (toolStripComboBox4.Text == "Italic")
                {
                    g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                }
                if (toolStripComboBox1.Text == "SE")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x + 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x + 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x + 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x + 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x + 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if (toolStripComboBox1.Text == "SW")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x - 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x - 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x - 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x - 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x - 5, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if (toolStripComboBox1.Text == "NE")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x + 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x + 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x + 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x + 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x + 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if (toolStripComboBox1.Text == "NW")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x - 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x - 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x - 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x - 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x - 5, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if (toolStripComboBox1.Text == "S")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x, y + 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if (toolStripComboBox1.Text == "N")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x, y - 5));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if (toolStripComboBox1.Text == "W")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x - 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x - 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x - 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x - 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x - 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                else if(toolStripComboBox1.Text == "E")
                {
                    if (toolStripComboBox4.Text == "Regular")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(Color.Gray), new PointF(x + 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Regular), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Bold")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(Color.Gray), new PointF(x + 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Bold), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Underline")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(Color.Gray), new PointF(x + 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Underline), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Strikeout")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(Color.Gray), new PointF(x + 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Strikeout), new SolidBrush(paint), new PointF(x, y));
                    }
                    else if (toolStripComboBox4.Text == "Italic")
                    {
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(Color.Gray), new PointF(x + 5, y));
                        g.DrawString(toolStripTextBox2.Text, new Font(toolStripComboBox2.Text, Convert.ToInt32(toolStripComboBox3.Text), FontStyle.Italic), new SolidBrush(paint), new PointF(x, y));
                    }
                }
                g.Dispose();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            pictureBox1.Image = null;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Png files|*.png|jpeg files|*jpg|bitmaps|*.bmp";
            if(o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = (Image)Image.FromFile(o.FileName).Clone();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Png files|*.png|jpeg files|*jpg|bitmaps|*.bmp";
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(s.FileName))
                {
                    File.Delete(s.FileName);
                }
                if (s.FileName.Contains(".jpg"))
                {
                    bmp.Save(s.FileName, ImageFormat.Jpeg);
                }
                else if (s.FileName.Contains(".png"))
                {
                    bmp.Save(s.FileName, ImageFormat.Png);
                }
                else if (s.FileName.Contains(".bmp"))
                {
                    bmp.Save(s.FileName, ImageFormat.Bmp);
                }
            }
        }

        private void red_Scroll(object sender, EventArgs e)
        {
            paint = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paint;
            redval.Text = "R: " + paint.R.ToString();
        }

        private void green_Scroll(object sender, EventArgs e)
        {
            paint = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paint;
            greenval.Text = "G: " + paint.G.ToString();
        }

        private void blue_Scroll(object sender, EventArgs e)
        {
            paint = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paint;
            blueval.Text = "B: " + paint.B.ToString();
        }

        private void alpha_Scroll(object sender, EventArgs e)
        {
            paint = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paint;
            alphaval.Text = "A: " + paint.A.ToString();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            choose = true;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (choose)
            {
                Bitmap bmp = (Bitmap)pictureBox2.Image.Clone();
                paint = bmp.GetPixel(e.X, e.Y);
                red.Value = paint.R;
                green.Value = paint.G;
                blue.Value = paint.B;
                alpha.Value = paint.A;
                redval.Text = paint.R.ToString();
                greenval.Text = paint.G.ToString();
                blueval.Text = paint.B.ToString();
                alphaval.Text = paint.A.ToString();
                pictureBox3.BackColor = paint;
            }
           
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            choose = false;
        }
    }
}

