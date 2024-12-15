using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorted_app
{
    public partial class Form1 : Form
    {
        private int[] arrayToSort;
        private int comparisonsBubble = 0;
        private int swapsBubble = 0;
        private long timeBubble = 0;
        private int comparisonsSelection = 0;
        private int swapsSelection = 0;
        private long timeSelection = 0;
        private int comparisonsInsertion = 0;
        private int swapsInsertion = 0;
        private long timeInsertion = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                int size = Convert.ToInt32(textBoxSize.Text);
                int minValue = Convert.ToInt32(textBoxMin.Text);
                int maxValue = Convert.ToInt32(textBoxMax.Text);
                
                if (size <= 0 || minValue > maxValue)
                {
                    MessageBox.Show("Некорректный ввод! Проверьте введённые данные.");
                    return;
                }

                arrayToSort = new int[size];
                Random random = new Random();

                for (int i = 0; i < size; i++)
                {
                    arrayToSort[i] = random.Next(minValue, maxValue + 1);
                }

                listBoxOriginal.Items.Clear();
                foreach (var item in arrayToSort)
                {
                    listBoxOriginal.Items.Add(item.ToString());
                }

                buttonStart.Enabled = true;

            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}");
            }
            catch (OverflowException ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}");
            }
        }

        private void BubbleSort(int[] arr)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int n = arr.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    comparisonsBubble++;
                    if (arr[j] > arr[j + 1])
                    {
                        Swap(ref arr[j], ref arr[j + 1]);
                        swapped = true;
                        swapsBubble++;
                    }
                }

                if (!swapped)
                {
                    break;
                }
            }

            stopwatch.Stop();
            timeBubble = stopwatch.ElapsedMilliseconds;

            Invoke(new Action(() =>
            {
                listBoxSorted.Items.AddRange(arr.Select(x => x.ToString()).ToArray());
                MessageBox.Show("Пузырьковая сортировка завершена!");
            }));
        }

        private void SelectionSort(int[] arr)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisonsSelection++;
                    if (arr[minIndex] > arr[j])
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    Swap(ref arr[i], ref arr[minIndex]);
                    swapsSelection++;
                }
            }

            stopwatch.Stop();
            timeSelection = stopwatch.ElapsedMilliseconds;

            Invoke(new Action(() =>
            {
                listBoxSorted.Items.AddRange(arr.Select(x => x.ToString()).ToArray());
                MessageBox.Show("Сортировка выбором завершена!");
            }));
        }

        private void InsertionSort(int[] arr)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key)
                {
                    comparisonsInsertion++;
                    arr[j + 1] = arr[j];
                    j--;
                    swapsInsertion++;
                }

                arr[j + 1] = key;
            }

            stopwatch.Stop();
            timeInsertion = stopwatch.ElapsedMilliseconds;

            Invoke(new Action(() =>
            {
                listBoxSorted.Items.AddRange(arr.Select(x => x.ToString()).ToArray());
                MessageBox.Show("Сортировка вставками завершена!");
            }));
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private void DisplayResults()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Результаты пузырьковой сортировки:");
            sb.AppendLine($"   Сравнений: {comparisonsBubble}");
            sb.AppendLine($"   Перестановок: {swapsBubble}");
            sb.AppendLine($"   Время: {timeBubble} мс");

            sb.AppendLine("\nРезультаты сортировки выбором:");
            sb.AppendLine($"   Сравнений: {comparisonsSelection}");
            sb.AppendLine($"   Перестановок: {swapsSelection}");
            sb.AppendLine($"   Время: {timeSelection} мс");

            sb.AppendLine("\nРезультаты сортировки вставками:");
            sb.AppendLine($"   Сравнений: {comparisonsInsertion}");
            sb.AppendLine($"   Перестановок: {swapsInsertion}");
            sb.AppendLine($"   Время: {timeInsertion} мс");

            textBoxResults.Text = sb.ToString();
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            
            listBoxSorted.Items.Clear();
            textBoxResults.Clear();

            
            Task taskBubble = Task.Run(() => BubbleSort(arrayToSort));
            Task taskSelection = Task.Run(() => SelectionSort(arrayToSort));
            Task taskInsertion = Task.Run(() => InsertionSort(arrayToSort));

            await Task.WhenAll(taskBubble, taskSelection, taskInsertion);

            DisplayResults();
        }
    }
}
