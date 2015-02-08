using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace Risk_VTHacks
{
    public class MyRect : 
    {
        public MyRect()
        {
           
        }


    }

    public partial class Form1 : Form
    {

        Dictionary<
        public Form1()
        {

            InitializeComponent();
           for(int x = 0; x < 8; x ++)
           {
               for(int y = 0; y  < 8 ; y++)
               {



               }
            
           }
            
            // this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
            this.rectangleShape7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Click_MouseDown);
        }

        private void Click_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    var x = e.X;
                    var y = e.Y;
                    MessageBox.Show(this, String.Format("{0} {1}", e.X, e.Y));
                    break;
                case MouseButtons.Right:
                    MessageBox.Show(this, "Right Button Click");
                    break;
                case MouseButtons.Middle:
                    break;
                default:
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
