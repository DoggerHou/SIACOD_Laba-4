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
        int versh = -1;//для запоминания выбранной вершины
        Stack<int> tempStack = new Stack<int>();//Временный стек
        List<int> resultList = new List<int>(); // Результирующий Список
        Queue<int> tempQueue = new Queue<int>();//Временная очередь
        List<int> rgr = new List<int>();//Для ргр
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            storage = new List<Circle>();
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
                dataGridView1[0, i + 1].Value = i;
                dataGridView1[i + 1, 0].Value = i;
            }

            for (int i = 1; i <= storage.Count; i++)
                for (int j = 1; j <= storage.Count; j++)
                    dataGridView1[i, j].Value = arr[i - 1, j - 1];
        }


        //Нажатие кнопки "Обход в глубину"
        private void button2_Click(object sender, EventArgs e)
        {
            dfs(0);   //Рекурсивный алгоритм
            //dfs2();    //Нерекурсивный алгоритм
            foreach (var obj in resultList)
                textBox1.Text += obj.ToString() + " ";
        }


        //Обработка кнопки "Задание на РГР"
        private void button3_Click(object sender, EventArgs e)
        {
            bfs(Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text));
            rgr.RemoveAll(item => item == Int32.Parse(textBox2.Text));
            foreach (var obj in rgr)
                textBox1.Text += obj.ToString() + " ";
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


        //Рекурсивный поиск в глубину
        private void dfs(int v)
        {
            resultList.Add(v);
            for (int i = 0; i < storage.Count; i++)
            {
                if (!resultList.Contains(i) && arr[v, i] == 1)
                    dfs(i);
            }
        }

        //НЕ рекурсивный поиск в глубину
        private void dfs2()
        {
            tempStack.Push(0);
            while (tempStack.Count > 0)
            {
                int v = tempStack.Pop();
                if (!resultList.Contains(v))
                {
                    resultList.Add(v);
                    for (int i = 0; i < storage.Count; i++)
                        if (arr[v, i] == 1 && !resultList.Contains(i))
                            tempStack.Push(i);
                }
            }
        }


        //Поиск в ширину для РГР
        private void bfs(int versh, int kol)
        {
            tempQueue.Enqueue(versh);
            int k = 0;
            while (tempQueue.Count > 0)
            {
                k++;
                if (k == kol)
                {
                    foreach (var vv in tempQueue)
                        for (int i = 0; i < storage.Count; i++)
                            if (arr[vv, i] == 1 && !rgr.Contains(i))
                                rgr.Add(i);
                }

                int v = tempQueue.Dequeue();
                if (!resultList.Contains(v))
                {
                    resultList.Add(v);
                    for (int i = 0; i < storage.Count; i++)
                    {
                        if (arr[v, i] == 1 && !resultList.Contains(i))
                            tempQueue.Enqueue(i);
                    }
                }
            }
        }
    }
}