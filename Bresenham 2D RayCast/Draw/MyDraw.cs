using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Bresenham_2D_RayCast
{
    public struct BresenhamData
    {
        public BresenhamData(int _index, int _x, int _y)
        {
            index = _index;
            x = _x;
            y = _y;
        }
        public int index;
        public int x;
        public int y;
    }

    public class MyDraw
    {
        private Pen pen = new Pen(Color.Black);
        private Brush brush = new SolidBrush(Color.Green);

        private int m_TileCX = 32;
        private int m_TileCY = 32;
        private int m_TileX = 40;
        private int m_TileY = 40;

        private bool m_HasClick = false;
        private Point m_StartPt;
        private Point m_Mouse;

        List<BresenhamData> m_BresenhamList = null;
        public MyDraw()
        {
            m_BresenhamList = new List<BresenhamData>();
        }
 

        public void Render(Graphics g)
        {
            // Draw Y
            for(int x = 0; x < m_TileX; ++x)
            {
                g.DrawLine(pen, x * m_TileCX, 0f, x * m_TileCX, m_TileCY * m_TileY);
            }

            // Draw X
            for (int y = 0; y < m_TileY; ++y)
            {
                g.DrawLine(pen, 0f, y * m_TileCY, m_TileCX * m_TileX, y * m_TileCY);
            }

            if(m_HasClick)
            {
                foreach(BresenhamData bresh in m_BresenhamList)
                {
                    float centerX = (float)(bresh.x * m_TileCX);
                    float centerY = (float)(bresh.y * m_TileCY);

                    g.FillRectangle(brush, centerX + 1, centerY + 1, m_TileCX - 1, m_TileCY - 1);
                }

                g.DrawLine(pen, m_StartPt, m_Mouse);

            }

        }
        public void MakeBresenhamList()
        {
            if (!m_HasClick)
                return;

            m_BresenhamList.Clear();
            MyCalculation(m_StartPt.X , m_StartPt.Y , m_Mouse.X , m_Mouse.Y );
            //MakeBresenhamListImpl(m_StartPt.X / m_TileCX, m_StartPt.Y / m_TileCY,
            //    m_Mouse.X / m_TileCX, m_Mouse.Y / m_TileCY );

        }
        private double Equation(double x, double a, double x1, double y1)
        {
            return a * (x - x1) + y1;
        }
        private void MyCalculation(int x0, int y0, int x1, int y1)
        {

            float dx = x1 - x0;
            float dy = y1 - y0;
            //if (dx == 0f || dy == 0f)
            //    return;

            float length = (float)Math.Sqrt((double)(dx * dx + dy * dy));
            float dirX = dx / length;
            float dirY = dy / length;

            int dirSignX = dirX > 0f ? 1 : -1;
            int dirSignY = dirY > 0f ? 1 : -1;

            int tileOffsetX = (dirX > 0f ? 1 : 0) ;
            int tileOffsetY = (dirY > 0f ? 1 : 0) ;

            int tileX = x0 / m_TileCX;
            int tileY = y0 / m_TileCY;

            float curX = x0;
            float curY = y0;
            float t = 0f;
            int index = 0;

            float dtX = ((tileX + tileOffsetX) * m_TileCX - curX) / dirX;
            float dtY = ((tileY + tileOffsetY) * m_TileCY - curY) / dirY;

            float ddtX = dirSignX * m_TileCX / dirX;
            float ddtY = dirSignY * m_TileCX / dirY;
            while (Math.Abs(t) < length)
            {
                m_BresenhamList.Add(new BresenhamData(index++, tileX, tileY));

                float dt = 0f;
                if (dtX < dtY )
                {
                    tileX  += dirSignX;
                    dt = dtX;
                    t += dt;
                    dtX = dtX + ddtX - dt;
                    dtY = dtY - dt;
                }
                else
                {
                    tileY += dirSignY;
                    dt = dtY;
                    t += dt;
                    dtX = dtX - dt;
                    dtY = dtY + ddtY - dt;

                }
            }
           
            
            //float dirX = 

        }
        private void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
        private void MakeBresenhamListImpl(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            int index = 0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x += 1)
            {
                if (steep) m_BresenhamList.Add(new BresenhamData(index, y, x));
                else m_BresenhamList.Add(new BresenhamData(index, x, y));
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
                index++;
            }
        }

        public Point StartPt
        {
            set
            {
                m_HasClick = true;
                m_StartPt = value;
            }
        }
        public Point Mouse
        {
            set
            {
                m_Mouse = value;
            }
        }

    }
}
