using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Bresenham_2D_RayCast
{
    public partial class Form1 : Form
    {
        private MyDraw m_Draw = null;
        public Form1()
        {
            InitializeComponent();
            m_Draw = new Bresenham_2D_RayCast.MyDraw();



            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            this.MouseMove += OnMouseMove;
            this.MouseClick += OnMouseClick;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            m_Draw.Render(e.Graphics);
        }
        private void OnMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_Draw.StartPt = new Point(e.X, e.Y);
        }
        private void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_Draw.Mouse = new Point(e.X,e.Y);
            m_Draw.MakeBresenhamList();
            Invalidate(true);
        }
    }
}
