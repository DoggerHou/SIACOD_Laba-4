using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_4
{
    public partial class Form1 : Form
    {
        List<Circle> storage;
        bool controlUp;
        int[,] arr;
        int versh = -1;
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            storage = new List<Circle>();
        }

        //Нажатие кнопки "Получить матрицу"
        private void button1_Click(object sender, EventArgs e)
        {
            //Заполняем DataGrid значениями из массива
            dataGridView1.RowCount = storage.Count + 1;
            dataGridView1.ColumnCount = storage.Count + 1;

            for(int i = 0;i< storage.Count; i++)
            {
                dataGridView1[0, i + 1].Value = i;
                dataGridView1[i + 1, 0].Value = i;
            }

            for (int i = 1; i <= storage.Count; i++)
                for (int j = 1; j <= storage.Count; j++)
                    dataGridView1[i, j].Value = arr[i - 1, j - 1];
        }

        //Отрисовка формы
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var obj in storage)
                obj.OnPaint(e, storage);
        }

        //Нажатие мыши
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(versh == -1)
                {
                    for(int i = 0; i < storage.Count; i++)
                        if (storage[i].isPicked(e, controlUp))
                        {
                            versh = i;
                            break;
                        }
                }
                else
                {
                    int toversh = -1;
                    for (int i = 0; i < storage.Count; i++)
                        if (storage[i].isPicked(e, controlUp))
                        {
                            toversh = i;
                            break;
                        }
                    if((toversh != -1) && (versh != toversh))
                    {
                        arr[versh, toversh] = 1;
                        arr[toversh, versh] = 1;
                        storage[versh].addVershin(toversh);
                        versh = -1;
                    }
                }
            }


            if (e.Button == MouseButtons.Left)
            {
                if (controlUp)
                    foreach (var obj in storage)
                    {
                        if (obj.isPicked(e, controlUp) && obj.getDetail() == false)
                            obj.changeDetail_to(true);
                        else if (obj.isPicked(e, controlUp) && obj.getDetail() == true)
                            obj.changeDetail_to(false);
                    }
                else
                    storage.Add(new Circle(e.Location, storage.Count));
                arr = new int[storage.Count, storage.Count];
            }
            pictureBox1.Invalidate();
        }

        //нажатие кнопки
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                controlUp = true;
            if (e.KeyCode == Keys.Delete)
            {
                for (int i = storage.Count - 1; i >= 0; i--)
                    if (storage[i].getDetail())
                        storage.Remove(storage[i]);
                for (int i = storage.Count - 1; i >= 0; i--)
                    storage[i].changeNumber(i);
            }
            pictureBox1.Invalidate();

        }

        //Отпустили кнопку
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            controlUp = false;
        }


    }
}
