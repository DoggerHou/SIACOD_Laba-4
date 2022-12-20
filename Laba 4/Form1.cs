using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laba_4
{
    public partial class Form1 : Form
    {
        List<Circle> storage; //массив объектов(вершин)
        bool controlUp;//Зажата ли кнопка ctrl
        int[,] arr;//Матрица смежности
        int[,] arr2;//Матрица смежности
        int versh = -1;//для запоминания выбранной вершины
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            storage = new List<Circle>();
        }


        //Нажатие мыши
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //добавление ребер
            if (e.Button == MouseButtons.Right)
            {
                if (versh == -1)
                {
                    for (int i = 0; i < storage.Count; i++)
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
                    if ((toversh != -1) && (versh != toversh))
                    {
                        storage[versh].addVershin(toversh);
                        versh = -1;
                    }
                }
            }

            //Добавление вершин
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


        //Отрисовка формы
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var obj in storage)
                obj.OnPaint(e, storage);
        }

        //Нажатие кнопки "Получить матрицу"
        private void button1_Click(object sender, EventArgs e)
        {
            //Заполняем матрицу смежности
            for (int j = 0; j < storage.Count; j++)
            {
                var l = storage[j].vershin;
                for (int i = 0; i < l.Count; i++)
                {
                    arr[j, l[i]] = 1;
                }
            }

            for (int i = 0; i < storage.Count; i++)
                for (int j = 0; j < storage.Count; j++)
                    if (arr[i, j] == 1 || arr[j, i] == 1)
                    {
                        arr[j, i] = 1;
                        arr[i, j] = 1;
                    }

            //Заполняем DataGrid значениями из массива
            dataGridView1.RowCount = storage.Count + 1;
            dataGridView1.ColumnCount = storage.Count + 1;

            for (int i = 0; i < storage.Count; i++)
            {
                dataGridView1[0, i + 1].Value = i+1;
                dataGridView1[i + 1, 0].Value = i+1;
            }

            for (int i = 1; i <= storage.Count; i++)
                for (int j = 1; j <= storage.Count; j++)
                    dataGridView1[i, j].Value = arr[i - 1, j - 1];
        }


        //Обработка кнопки "Задание на РГР"
        private void button3_Click(object sender, EventArgs e)
        {
            arr2 = arr.Clone() as int[,];
            for (int i = 0; i < int.Parse(textBox3.Text) - 1;i++)
                arr2 = MultiplicationMatrixD(arr2, arr);

            for(int i = 0; i < storage.Count; i++)
            {
                if(arr2[int.Parse(textBox2.Text) - 1, i] >= 1)
                    textBox1.Text += (i + 1).ToString() + " ";
            }
        }


        //Обработка нажатий на клавиатуру
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


        private int[,] MultiplicationMatrixD(int[,] a, int[,] b)
        {
            int[,] r = new int[storage.Count, storage.Count];
            for (int i = 0; i < storage.Count; i++)
            {
                for (int j = 0; j < storage.Count; j++)
                {
                    for (int k = 0; k < storage.Count; k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return r;
        }
    }
}