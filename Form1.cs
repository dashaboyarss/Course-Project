using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсач3
{
    public partial class Form1 : Form
    {
        bool isCorrect = true; //флаг, проверяющий корректность всех введенных данных

        static bool ParseInput (string input)
        {
            bool isConverted;
            long result;
            isConverted = Int64.TryParse(input, out result);
            return isConverted;
        }

        static double NewTargetSum (long targetPeriod, long targetSum, double inflation)
        {
            long years = targetPeriod / 12; 
            long months = targetPeriod % 12;
            double newTargetSum = targetSum;
            for (int i = 0; i < years; i++)
            {
                newTargetSum = newTargetSum + (newTargetSum * inflation);
            }
            newTargetSum = newTargetSum + (newTargetSum * (inflation * ((double)months / 12)));
            return newTargetSum;
        }

        static double InvestmentIncome (double sum, double perCent, long period)
        {
            double startSum = sum;
            long years = period / 12;
            double months = period % 12;

            for (int i = 0; i < years; i++)
            {
                sum = sum + (sum * perCent);
            }
            sum = sum + (sum * (perCent * (months / 12)));
            return sum - startSum;
        }

        static double Payment(double sum, long period, string freq)
        {
            double payment = 0;

            if (freq == "раз в неделю")
            {
                payment = sum / period / 4;
            }
            else if (freq == "раз в 2 недели")
            {
                payment = sum / period / 2;
            }
            else if (freq == "раз в месяц")
            {
                payment = sum / period;
            }
            else if (freq == "раз в 3 месяца")
            {
                if (period % 3 == 0) payment = sum / (period / 3);
                else payment = sum / (period / 3 + 1);
            }
            else if (freq == "раз в полгода")
            {
                if (period % 6 == 0) payment = sum / (period / 6);
                else payment = sum / (period / 6 + 1);
            }
            else if (freq == "раз в год")
            {
                if (period % 12 == 0) payment = sum / (period / 12);
                else payment = sum / (period / 12 + 1);
            }
            return payment;
        }

        static double Step(string freq)
        {
            double step = 0;
            if (freq == "раз в неделю")
            {
                step = 0.25;
            }
            else if (freq == "раз в 2 недели")
            {
                step = 0.5;
            }
            else if (freq == "раз в месяц")
            {
                step = 1;
            }
            else if (freq == "раз в 3 месяца")
            {
                step = 3;
            }
            else if (freq == "раз в полгода")
            {
                step = 6;
            }
            else if (freq == "раз в год")
            {
                step = 12;
            }
            return step;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool isConverted;  //проверка на корректность ввода всех полей с числовыми значениями
            isConverted = ParseInput(textBox1.Text);
            if (!isConverted) 
            {
                MessageBox.Show("Некорректно введено значение поля Сумма цели!");
                isCorrect = false;
            }
            isConverted = ParseInput(textBox3.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Начальная сумма!");
                isCorrect = false;
            }
            isConverted = ParseInput(textBox6.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля По сколько вы готовы вкладывать!");
                isCorrect = false;
            }
            isConverted = ParseInput(textBox5.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Сумма на инвестиционном счету!");
                isCorrect = false;
            }
            isConverted = ParseInput(textBox2.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Желаемый срок достижения цели!");
                isCorrect = false;
            }
            isConverted = ParseInput(textBox8.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Текущий уровень инфляции (%)!");
                isCorrect = false;
            }
            if (comboBox1.Text != "раз в неделю" && comboBox1.Text != "раз в 2 недели" && comboBox1.Text != "раз в месяц" && comboBox1.Text != "раз в 3 месяца" && comboBox1.Text != "раз в полгода" && comboBox1.Text != "раз в год" )
            {
                MessageBox.Show("Некорректно заполнено поле частоты взносов! Выберите один из предложенных вариантов внесения взносов");
                isCorrect = false;
            }

            if (isCorrect)
            {
                long targetSum = Convert.ToInt64(textBox1.Text); //целевая сумма
                long startSum = Convert.ToInt64(textBox3.Text);//начальная сумма
                double investmentPerCent = Convert.ToDouble(textBox6.Text)/100; //процент дохода от инвестиций
                double investmentSum = Convert.ToDouble(textBox5.Text); //сумма в инвестициях
                long targetPeriod = Convert.ToInt64(textBox2.Text); //целевой срок наклопления
                double inflation = Convert.ToDouble(textBox8.Text)/100; //годовой процент инфляции

                double newTargetSum = NewTargetSum(targetPeriod, targetSum, inflation);
                richTextBox1.Text = Convert.ToString(newTargetSum);//заполняем поле "Стоимость цели с учетом инфляции"

                double investmentIncome = InvestmentIncome(investmentSum, investmentPerCent, targetPeriod);
                richTextBox2.Text = Convert.ToString(investmentIncome); //заполняем поле "Доход от инвестиций"

                double paymentSum = newTargetSum - investmentIncome - startSum; //сумма платежей
                string frequency = comboBox1.Text;
                double payment = Payment(paymentSum, targetPeriod, frequency);
                richTextBox3.Text = Convert.ToString(payment);
                                
                double endGraph = targetPeriod;
                double step = Step(comboBox1.Text);
                double x = 0;
                double y = startSum;
                this.chart.Series[0].Points.Clear();
                this.chart.Series[1].Points.Clear();
                while (x <= endGraph)
                {
                    this.chart.Series[0].Points.AddXY(x, y);
                    x += step;
                    y += payment;                
                }
                
                double endGraph1 = targetPeriod;                
                double x1 = 0;
                double step1 = 1;
                double y1 = targetSum;
                while(x1  <= endGraph1)
                {
                    this.chart.Series[1].Points.AddXY(x1, y1);
                    x1 += step1;
                    y1 += y1 * (inflation / 12);
                }
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
