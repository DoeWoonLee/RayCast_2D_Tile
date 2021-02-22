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
            this.KeyDown += OnKeyDown;
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
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            int moveX = 0;
            int moveY = 0;
            if (e.KeyCode == Keys.Left)
                moveX = -5;
            else if (e.KeyCode == Keys.Right)
                moveX = 5;

            if (e.KeyCode == Keys.Up)
                moveY = -5;
            else if (e.KeyCode == Keys.Down)
                moveY = 5;

            if(e.KeyCode == Keys.A)
            {
                m_Draw.AddJump(-1);
            }
            else if(e.KeyCode == Keys.S)
            {
                m_Draw.AddJump(1);
            }
            if (e.KeyCode == Keys.Q)
            {
                m_Draw.AddSpeed(-1);
            }
            else if (e.KeyCode == Keys.W)
            {
                m_Draw.AddSpeed(1);
            }
            if (e.KeyCode == Keys.Z)
            {
                m_Draw.AddGravity(-1);
            }
            else if (e.KeyCode == Keys.X)
            {
                m_Draw.AddGravity(1);
            }
            if (e.KeyCode == Keys.F)
            {
                m_Draw.FlipDirection();
            }
            m_Draw.MoveCurveStart(new Point(moveX, moveY));
            m_Draw.MakeCurveMark2();
            Invalidate(true);
        }
    }
}
