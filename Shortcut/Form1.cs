using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shortcut
{
    public partial class Form1 : Form
    {
        public Control myControl1;
        public Form1()
        {
            InitializeComponent();

            SetDoubleBuffered(tableLayoutPanel1);
        
        }

        // Обработчик нажатия кнопки построения поля
        private void button4_Click(object sender, EventArgs e)
        {
            int fieldSize;

            if (Int32.TryParse(textBox1.Text, out fieldSize))                    // проверка ввода целочисленных значений
            {
                if (fieldSize >= 10 && fieldSize <= 50)                          // проверка ввода значений в диапазоне от 10 до 50
                {
                    label12.Visible = false;         // В случае корректно введенных данных скрывается label с сообщением об ошибке
                    button5.Enabled = true;          // и активируется кнопка построения координат

                    tableLayoutPanel1.Controls.Clear();

                    //Создание таблицы заданных размеров

                    tableLayoutPanel1.ColumnCount = fieldSize;       
                    tableLayoutPanel1.RowCount = fieldSize;
                    float number = 100 / fieldSize;
                    tableLayoutPanel1.ColumnStyles[0].Width = number;
                    tableLayoutPanel1.RowStyles[0].Height = number;
                    {

                        for (int i = 0; i < fieldSize; i++)
                        {
                            for (int j = 0; j < fieldSize; j++)
                            {
                                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, number));
                                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, number));
                                tableLayoutPanel1.Controls.Add(new Label() { Text = "0" }, i, j);
                                tableLayoutPanel1.GetControlFromPosition(i, j).ForeColor = Color.White;
                                tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
                                
                                // Определение максимально возможного числа препятсвий на заданном поле
                                int maxObstacles;
                                if (fieldSize * fieldSize > 2000)
                                {
                                    maxObstacles = 2000;
                                }
                                else
                                {
                                    maxObstacles = fieldSize * fieldSize - 2;
                                }
                                label10.Visible = true;
                                label10.Text = "Максимальное количество препятствий " + maxObstacles.ToString();
                            }
                        }
                    }
                }

                else
                {
                    label12.Text = "Введите значение от 10 до 50";
                    label12.Visible = true;
                    button5.Enabled = false;  
                    label10.Visible = false;
                }
            }
                        
            else
            {
                label12.Text = "Введено некорректное значение";
                label12.Visible = true;
                button5.Enabled = false;
                label10.Visible = false;
            }

            
        }
        // Буферизация для увеличения быстродействия построения и отрисовки таблицы в TableLayoutPanel
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {

            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        // Обработчик кнопки построения начальной и конечной точки
        private void button5_Click(object sender, EventArgs e)
        {
            int startX1;
            int startY1;
            int startX2;
            int startY2;

            // проверка ввода целочисленных значений
            if (Int32.TryParse(textBox2.Text, out startX1) && 
                Int32.TryParse(textBox3.Text, out startY1) && 
                Int32.TryParse(textBox6.Text, out startX2) && 
                Int32.TryParse(textBox5.Text, out startY2))
            {
                // уменьшение индекса на один, для соответствия с нумерацией ячеек в tableLayoutPanel
                startX1--;
                startY1--;
                startX2--;
                startY2--;

                int fieldSize = Int32.Parse(textBox1.Text);
                
                // Проверка вхождения координат точек в заданные размеры поля
                if ((startX1 >= 0) &&( startX1 < fieldSize) &&
                    (startY1 >= 0) && (startY1 < fieldSize) &&
                    (startX2 >= 0) && (startX2 < fieldSize) &&
                    (startY2 >= 0) && (startY2 < fieldSize) )
                {
                    // Проверка ввода координат точек, недопущение их совпадения
                    if ((startX1 == startX2) && (startY1 == startY2))
                    {
                        label11.Visible = true;
                        label11.Text = "Координаты двух точек должны быть разными";
                        button2.Enabled = false;
                        
                    }
                    else
                    {
                        if (tableLayoutPanel1.GetControlFromPosition(startX1, startY1).ForeColor == Color.White)
                        {
                            // Задать ячейке начальной точки зеленый цвет и значение 1
                            tableLayoutPanel1.GetControlFromPosition(startX1, startY1).BackColor = Color.Green;
                            tableLayoutPanel1.GetControlFromPosition(startX1, startY1).Text = "1";
                            tableLayoutPanel1.GetControlFromPosition(startX1, startY1).ForeColor = Color.Green;

                        }
                        if (tableLayoutPanel1.GetControlFromPosition(startX2, startY2).ForeColor == Color.White)
                        {
                            // Задать ячейке конечной точки красный цвет
                            tableLayoutPanel1.GetControlFromPosition(startX2, startY2).ForeColor = Color.Red;
                            tableLayoutPanel1.GetControlFromPosition(startX2, startY2).BackColor = Color.Red;
                        }
                        label11.Visible = false;
                        button2.Enabled = true;
                        button6.Enabled = true;
                        
                    }
                }

                else
                {
                    label11.Visible = true; 
                    label11.Text = "Точка не может быть задана за пределами поля";
                    button2.Enabled = false;
                    button6.Enabled = false;

                }

            }
            else
            {
                label11.Visible = true;
                label11.Text = "Нужно ввести целые числа";
                button2.Enabled = false;
                button6.Enabled = false;

            }

        }

        // Обработчик нажатия кнопки построения препятствий
        private void button2_Click(object sender, EventArgs e)  
        {
            int maxObstacles;

            // Проверка ввода целочисленного значения
            if (Int32.TryParse(textBox4.Text, out maxObstacles))
            {
                // Очистка всей таблицы от ранее построенных препятствий 
                label13.Visible = false;
                int field = Int32.Parse(textBox1.Text);
                for (int i = 0; i < field; i++)
                {
                    for (int j = 0; j < field; j++)
                    {
                        // исключение из области чистки поля точек начала и конца маршрута
                        if ((tableLayoutPanel1.GetControlFromPosition(i, j).BackColor != Color.Green) &&
                           (tableLayoutPanel1.GetControlFromPosition(i, j).BackColor != Color.Red))
                        {
                            tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
                            tableLayoutPanel1.GetControlFromPosition(i, j).ForeColor = Color.White;
                        }
                    }
                }


                int square = field * field;
                int obstacles = Int32.Parse(textBox4.Text);
                if (square > 2000)
                {
                    // установление верхнего предела возможного количества препятствий (для больших полей)
                    if (obstacles > 2000)
                    {
                        label10.ForeColor = Color.Red;
                    }
                    else label10.ForeColor = Color.Black;
                }
                else
                {
                    // установление верхнего предела возможного количества препятствий
                    // возможное количество меньше общего количества ячеек таблицы на 2, исключая точки начала и конца маршрута
                    if (obstacles > square - 2)
                    {
                        label10.ForeColor = Color.Red;
                    }
                    else label10.ForeColor = Color.Black;
                }
                label10.Visible = true;

                Random rnd = new Random();

                if (label10.ForeColor != Color.Red)
                {
                    // Задание случайных координат для точек препятствий
                    for (int i = 0; i < obstacles; i++)
                    {
                        int obsX = rnd.Next(0, field);
                        int obsY = rnd.Next(0, field);
                        if (tableLayoutPanel1.GetControlFromPosition(obsX, obsY).ForeColor == Color.White)
                        {
                            tableLayoutPanel1.GetControlFromPosition(obsX, obsY).ForeColor = Color.Black;
                            tableLayoutPanel1.GetControlFromPosition(obsX, obsY).BackColor = Color.Black;
                        }
                        else i--;
                    }
                }
            }
            else label13.Visible = true;

            
        }

        private void button3_Click(object sender, EventArgs e) 
        {
            // Очистка поля, полей ввода значений, деактивация кнопок 
            tableLayoutPanel1.Controls.Clear();
            button6.Enabled = false;
            button2.Enabled = false;
            button5.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            button4.Enabled = true;

            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            label9.Visible = false;

            // повторное построение поля, по заданным в прошлый раз размерам
            int fieldfSize = Int32.Parse(textBox1.Text);
            float number = 100 / fieldfSize;

            for (int i = 0; i < fieldfSize; i++)
            {
                for (int j = 0; j < fieldfSize; j++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, number));
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, number));
                    tableLayoutPanel1.Controls.Add(new Label() { Text = "0" }, i, j);
                    tableLayoutPanel1.GetControlFromPosition(i, j).ForeColor = Color.White;
                    tableLayoutPanel1.GetControlFromPosition(i, j).BackColor = Color.White;
                }
            }
        }

       // Прокладывание маршрута
        private void button6_Click(object sender, EventArgs e) {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;

            int fieldfSize = Int32.Parse(textBox1.Text) - 1;
            int startX = Int32.Parse(textBox2.Text) - 1;
            int startY = Int32.Parse(textBox3.Text) - 1;
            int finishX = Int32.Parse(textBox6.Text) - 1;
            int finishY = Int32.Parse(textBox5.Text) - 1;
            int right, left, up, down;
            int currentX = startX;
            int currentY = startY;
            int step = 1; // переменная для расчета кол-ва шагов от начальной точки к конечной

            for (int k=0; k < fieldfSize * fieldfSize; k++)
            {
                if (tableLayoutPanel1.GetControlFromPosition(finishX, finishY).Text != "0") 
                    break;
                // цикл продолжается до тех пор пока ячейка с конечной точкой не примет какое-либо значение
                // которое будет равно количеству шагов от начальной ячейки

                for (int i = 0; i <= fieldfSize; i++)
                {
                    for (int j = 0; j <= fieldfSize; j++)
                    {
                        if (tableLayoutPanel1.GetControlFromPosition(i, j).Text == step.ToString())
                        {
                            currentX = i;
                            currentY = j;
                            //увеличение на единицу значения соседних клеток
                            // если у клетки еще нет ненулевого значения (значит еще не обрабатывалась)
                            // если она входит в границы заданного поля
                            right = currentX + 1;
                            

                            if ((right <= fieldfSize) && (tableLayoutPanel1.GetControlFromPosition(right, currentY).ForeColor != Color.Black) &&
                                (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(right, currentY).Text) == 0))
                            {
                                tableLayoutPanel1.GetControlFromPosition(right, currentY).Text = (step + 1).ToString();
                            }

                            left = currentX - 1;
                            if ((left >= 0) && (tableLayoutPanel1.GetControlFromPosition(left, currentY).ForeColor != Color.Black) &&
                                (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(left, currentY).Text) == 0))
                                {
                                 tableLayoutPanel1.GetControlFromPosition(left, currentY).Text = (step + 1).ToString();
                                }

                            up = currentY - 1;
                            if ((up >= 0) && (tableLayoutPanel1.GetControlFromPosition(currentX, up).ForeColor != Color.Black) &&
                                    (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(currentX, up).Text) == 0)) 
                                {
                                 tableLayoutPanel1.GetControlFromPosition(currentX, up).Text = (step + 1).ToString();
                                }

                            down = currentY + 1;
                            if ((down <= fieldfSize) && (tableLayoutPanel1.GetControlFromPosition(currentX, down).ForeColor != Color.Black) &&
                                    (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(currentX, down).Text) == 0)) 
                            {
                                tableLayoutPanel1.GetControlFromPosition(currentX, down).Text = (step + 1).ToString();
                            }
                        }
                    }
                }

                step++;
            }
            // если у конечной точки ненулевое значение значит маршрут найден
            // от нее к начальной точке идет закрашивание клеток по клеткам имеющим вес на единицу меньший, чем у предыдущей
            // то есть минимальное расстояние по клеткам между двумя заданными точками

            if (tableLayoutPanel1.GetControlFromPosition(finishX, finishY).Text != "0") 
            {
                currentX = finishX;
                currentY = finishY;

                int minSteps = Int32.Parse(tableLayoutPanel1.GetControlFromPosition(finishX, finishY).Text);
                 for ( int i = minSteps; i>2; i--)
                {
                    //проверка справа
                    if (currentX + 1 <= fieldfSize)
                    {
                        if (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(currentX + 1, currentY).Text) == i - 1)
                        {
                            tableLayoutPanel1.GetControlFromPosition(currentX + 1, currentY).ForeColor = Color.Yellow;
                            tableLayoutPanel1.GetControlFromPosition(currentX + 1, currentY).BackColor = Color.Yellow;
                            currentX = currentX + 1;
                            continue;
                        }
                    }
                    
                    //проверка слева
                    if (currentX - 1 >= 0)
                    {
                        if (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(currentX - 1, currentY).Text) == i - 1)
                        {
                            tableLayoutPanel1.GetControlFromPosition(currentX - 1, currentY).ForeColor = Color.Yellow;
                            tableLayoutPanel1.GetControlFromPosition(currentX - 1, currentY).BackColor = Color.Yellow;
                            currentX = currentX - 1;
                            continue;
                        }
                    }
                    
                    //проверка сверху
                    if (currentY -1 >=0 )
                    {
                        if (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(currentX, currentY - 1).Text) == i - 1)
                        {
                            tableLayoutPanel1.GetControlFromPosition(currentX, currentY - 1).ForeColor = Color.Yellow;
                            tableLayoutPanel1.GetControlFromPosition(currentX, currentY - 1).BackColor = Color.Yellow;
                            currentY = currentY - 1;
                            continue;
                        }
                    }

                    //проверка снизу
                    if (currentY + 1 <= fieldfSize)
                    {
                        if (Int32.Parse(tableLayoutPanel1.GetControlFromPosition(currentX, currentY + 1).Text) == i - 1)
                        {
                            tableLayoutPanel1.GetControlFromPosition(currentX, currentY +1).ForeColor = Color.Yellow;
                            tableLayoutPanel1.GetControlFromPosition(currentX, currentY + 1).BackColor = Color.Yellow;
                            currentY = currentY + 1;
                            continue;
                        }
                    }
                    
                }

                label9.Visible = true;
                label9.ForeColor = Color.Green;
                label9.Text = "Кратчайший путь найден!";

            }
            else
            // если у конечной точки значение в ячейке таблицы 0, значит добраться до этой точки нельзя, программа пишет об этом сообщение
            {
                label9.Visible = true;
                label9.ForeColor = Color.Red;
                label9.Text = "Нет доступных путей";

            }
        }
    }
}
