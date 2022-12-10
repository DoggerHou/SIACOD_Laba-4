using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_4
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }



    public class Circle
    {
        private const int RADIX = 40;
        private Point location;
        private bool detail;
        public int number;
        public List<int> vershin;
        public Circle(Point location, int number)
        {
            this.location = location;
            this.number = number;
            detail = false;
            vershin = new List<int>();
        }
        public void changeDetail_to(bool d)
        {
            detail = d;
        }
        public bool getDetail()
        {
            return detail;
        }
        public void OnPaint(PaintEventArgs e, List<Circle> s)
        {
            if (detail == false)
                e.Graphics.DrawEllipse(new Pen(Color.Black, 2f), location.X - RADIX / 2, location.Y - RADIX / 2, RADIX, RADIX);
            else
                e.Graphics.DrawEllipse(new Pen(Color.Red, 3f), location.X - RADIX / 2, location.Y - RADIX / 2, RADIX, RADIX);

            e.Graphics.DrawString(number.ToString(), new Font("Arial", 20, FontStyle.Regular),
                Brushes.Red, location.X + 5 - RADIX / 2, 3 + location.Y - RADIX / 2);

            foreach(var num in vershin)
            {
                e.Graphics.DrawLine(new Pen(Color.Blue, 3f), location,
                    s[num].location);
            }

        }
        public bool isPicked(MouseEventArgs e, bool controlUp)
        {
            if (Math.Pow(location.X - e.X, 2) + Math.Pow(location.Y - e.Y, 2) <= RADIX * RADIX
                & controlUp)
                return true;
            return false;
        }
        public void changeNumber(int number)
        {
            this.number = number;
        }


        public void addVershin(int n)
        {
            vershin.Add(n);
        }
    }
}
