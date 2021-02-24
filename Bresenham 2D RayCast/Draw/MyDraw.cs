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
        public BresenhamData(int _index, int _x, int _y, float _normalX, float _normalY)
        {
            index = _index;
            x = _x;
            y = _y;
            normalX = _normalX;
            normalY = _normalY ;
        }
        public int index;
        public int x;
        public int y;
        public float normalX;
        public float normalY;
    }

    public class MyDraw
    {
        private Pen pen = new Pen(Color.Black);
        private Brush brush = new SolidBrush(Color.Green);

        private int m_TileCX = 32;
        private int m_TileCY = 32;
        private int m_TileX = 40;
        private int m_TileY = 40;

        //private bool m_HasClick = false;
        private Point m_StartPt;
        private Point m_Mouse;
        private Point m_CurveStart;

        private float speed = 150f;
        private float gravity = -20f;
        private float jump = -100f;
        private float direction = 1;
        List<BresenhamData> m_BresenhamList = null;
        public MyDraw()
        {
            m_BresenhamList = new List<BresenhamData>();
            m_CurveStart = new Point(0, 0);
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


            foreach (BresenhamData bresh in m_BresenhamList)
            {
                float centerX = (float)(bresh.x * m_TileCX);
                float centerY = (float)(bresh.y * m_TileCY);
                g.FillRectangle(brush, centerX + 1, centerY + 1, m_TileCX - 1, m_TileCY - 1);
            }

            float beforeX = 0f;
            float beforeY = 0f;

            // Draw Curve
            for (int x = 0; x < 300; ++x)
            {
                float y = PhysicsCurveFomulaY(x, jump, speed, gravity);

                g.DrawLine(pen, beforeX * direction + m_CurveStart.X,
                    beforeY + m_CurveStart.Y,
                    m_CurveStart.X  + x * direction,
                    y + m_CurveStart.Y);

                beforeX = x;
                beforeY = y;
            }


        }
       
        private float PhysicsCurveFomulaY(float x, float jump, float speed, float gravity)
        {
            float t = (x  / speed);
            return (-t * t * gravity) / 2 + t * jump;
        }
        private bool PhysicsCurveFomulaX(float y, float jump, float speed, float gravity, ref float x1, ref float x2)
        {
            //y 는 로컬 y
            // t == (s + sqrt(s*s  - 2gy)) / g
            float rootTemp = jump * jump - 2f * gravity * y;
            //if (rootTemp < 0f)
            //    return false;
            rootTemp = (float)Math.Sqrt((double)rootTemp);

            // val = t
            float t1 = (jump + rootTemp) / gravity;
            float t2 = (jump - rootTemp) / gravity;

           x1 = t1 * speed ;
           x2 = t2 * speed ;

            return true;

        }
        
        public void MakeCurveMark2()
        {
            m_BresenhamList.Clear();
            int index = 0;

            int tileX = m_CurveStart.X / m_TileCX;
            int tileY = m_CurveStart.Y / m_TileCX;

            float intervalX = direction > 0 ? m_CurveStart.X - tileX * m_TileCX : (tileX + 1) * m_TileCX - m_CurveStart.X;
            float intervalY = m_CurveStart.Y - tileY * m_TileCX;

            // 첫번째 타일의 절편
            float x = 0f;
            float y = 0f;

            float dtx = m_TileCX - intervalX;
            float dty = -intervalY;
            float rangeY = -intervalY;
            float rangeX = -intervalX;

            int tileXDelta = 0;

            float normalX = 0f;
            float normalY = 0f;
            while (index < 20)
            {
                m_BresenhamList.Add(new BresenhamData(index++, tileX + tileXDelta * (int)direction, tileY,normalX, normalY));
                // 오른쪽
                y = PhysicsCurveFomulaY(dtx, jump, speed, gravity);
                if(rangeY <= y && y <= rangeY + m_TileCY)
                {
                    rangeX += m_TileCX;
                    dtx += m_TileCX;
                    tileXDelta++;
                    continue;
                }
                // 위
                float rootTemp = jump * jump - 2f * gravity * (dty);
                float t = 0f;
                if(rootTemp > 0f)
                {
                    rootTemp = (float)Math.Sqrt((double)rootTemp);
                     t = (jump + rootTemp) / gravity;
                    x = t * speed;
                    if (rangeX <= x && x <= rangeX + m_TileCX)
                    {
                        rangeY -= m_TileCY;
                        dty -= m_TileCY;
                        tileY--;
                        continue;
                    }
                }
               
                // 아래
                rootTemp = jump * jump - 2f * gravity * (dty + m_TileCY);
                if(rootTemp > 0f)
                {
                    rootTemp = (float)Math.Sqrt((double)rootTemp);
                    t = (jump - rootTemp) / gravity;
                    x = t * speed;
                    if (rangeX <= x && x <= rangeX + m_TileCX)
                    {
                        rangeY += m_TileCY;
                        dty += m_TileCY;
                        tileY++;
                        continue;
                    }
                }
                break;
            }
        }



        public Point StartPt
        {
            set
            {
                //m_HasClick = true;
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
        public void AddJump(int addedJump)
        {
            jump += addedJump;
        }
        public void AddSpeed(int addedSpeed)
        {
            speed += addedSpeed;
        }
        public void AddGravity(int addedGravity)
        {
            gravity += addedGravity;
        }
        public void FlipDirection()
        {
            direction *= -1;
        }
        public void MoveCurveStart(Point move)
        {
            m_CurveStart.X += move.X;
            m_CurveStart.Y += move.Y;

        }
    }
}
