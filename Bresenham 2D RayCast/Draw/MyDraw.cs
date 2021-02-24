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
        public BresenhamData(int _index, int _x, int _y, float normalX, float normalY)
        {
            index = _index;
            x = _x;
            y = _y;
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

        private bool m_HasClick = false;
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

            //if(m_HasClick)
            //{
            //    
            //    
            //    
            //    
            //    
            //    
            //    g.DrawLine(pen, m_StartPt, m_Mouse);
            //}

            float beforeX = 0f;
            float beforeY = 0f;

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
        public void MakeBresenhamList()
        {
            if (!m_HasClick)
                return;

            //m_BresenhamList.Clear();
          
            //MyCalculation(m_StartPt.X , m_StartPt.Y , m_Mouse.X , m_Mouse.Y );
            //MakeBresenhamListImpl(m_StartPt.X / m_TileCX, m_StartPt.Y / m_TileCY,
            //    m_Mouse.X / m_TileCX, m_Mouse.Y / m_TileCY );

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
        public int CollisionWithRectangle( ref Rectangle rect, int tileX, int tileY, ref int beforeCollision)
        {
            // 좌
            //if(2 != beforeCollision)
            //{
            //    float y0 = PhysicsCurveFomulaY(rect.X, jump, speed, gravity) + m_CurveStart.Y;
            //    if (tileY * m_TileCY < y0 && y0 < (tileY + 1) * m_TileCY)
            //    {
            //        return 1;
            //    }
            //}

            //float time = speed / m_TileCX;
            // 우
            if (1 != beforeCollision)
            {
                float y1 = PhysicsCurveFomulaY(rect.X + rect.Width, jump, speed, gravity) + m_CurveStart.Y;
                if (tileY * m_TileCY <= y1 && y1 <= (tileY + 1) * m_TileCY)
                {
                    return 2;
                }
            }
            float x0 = 0f, x1 = 0f;
            // 위
            if (4 != beforeCollision)
            {
                if (PhysicsCurveFomulaX(rect.Y - m_TileCY + rect.Height, jump, speed, gravity, ref x0, ref x1))
                {
                    x0 += m_CurveStart.X;
                    x1 += m_CurveStart.X;

                    if ((tileX * m_TileCX <= x0 && x0 <= (tileX + 1) * m_TileCX))
                   //     ||
                   //(tileX * m_TileCX <= x1 && x1 <= (tileX + 1) * m_TileCX))
                    {
                        return 3;
                    }
                }
            }

            if (3 != beforeCollision)
            {
                //아래
                if (false == PhysicsCurveFomulaX(rect.Y + rect.Height, jump, speed, gravity, ref x0, ref x1))
                    return 0;

                x0 += m_CurveStart.X;
                x1 += m_CurveStart.X;

                if (/*(tileX * m_TileCX <= x0 && x0 <= (tileX + 1) * m_TileCX) ||*/
                    (tileX * m_TileCX <= x1 && x1 <= (tileX + 1) * m_TileCX))
                {
                    return 4;
                }
            }


            return 0;
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
        public void MakeCurve()
        {
            m_BresenhamList.Clear();
            //x 0 에서 200 까지로 

            int startX = 0;
            int startY = 0;

            int tileX = m_CurveStart.X / m_TileCX;
            int tileY = m_CurveStart.Y / m_TileCY;

            int intervalX = m_CurveStart.X - tileX * m_TileCX;
            int intervalY = m_CurveStart.Y - tileY * m_TileCY;

            if(direction < 0)
            {
                intervalX = m_TileCX - intervalX;
                //intervalX = m_TileCY - intervalX;
            }

            Rectangle rect = new Rectangle();
            rect.X = startX * m_TileCX;
            rect.Y = startY * m_TileCY;
            rect.Width = m_TileCX - intervalX;
            rect.Height = m_TileCY - intervalY;

            int index = 0;

            int collision = 0;

            int deltaX = 0;
            while (index < 20)
            {
                collision = CollisionWithRectangle(ref rect, tileX + deltaX, tileY, ref collision);
                if(0 != collision)
                {
                    m_BresenhamList.Add(new BresenhamData(index, tileX + deltaX * (int)direction, tileY, 0f, 0f));
                }
                switch (collision)
                {
                    case 1: // 좌
                        startX--;
                        deltaX--;
                        break;
                    case 2: // 우
                        startX++;
                        deltaX++;
                        break;
                    case 3: // 위
                        startY--;
                        tileY--;
                        break;
                    case 4: // 아래
                        startY++;
                        tileY++;
                        break;
                    case 0:
                        return;
                }
                rect.X = startX * m_TileCX - intervalX ;
                rect.Y = startY * m_TileCY - intervalY;
                rect.Width = m_TileCX;
                rect.Height = m_TileCY;

                index++;
            }
           

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

            int tileOffsetX = (dirX > 0f ? 1 : -1) ;
            int tileOffsetY = (dirY > 0f ? 1 : -1) ;

            int tileX = x0 / m_TileCX;
            int tileY = y0 / m_TileCY;

            float curX = x0;
            float curY = y0;
            float t = 0f;
            int index = 0;

            float ddtX = dirSignX * m_TileCX / dirX;
            float ddtY = dirSignY * m_TileCX / dirY;

            float dtX = ((tileX + tileOffsetX) * m_TileCX - curX) / dirX;
            float dtY = ((tileY + tileOffsetY) * m_TileCY - curY) / dirY;

            float dt = 0f;

            while (Math.Abs(t) < length)
            {
                m_BresenhamList.Add(new BresenhamData(index++, tileX, tileY, 0f, 0f));

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
