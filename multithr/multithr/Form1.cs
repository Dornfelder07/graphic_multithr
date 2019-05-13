using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace multithr
{
    public partial class Form1 : Form
    {
        bool isPaused = true;
        bool isEnd = true;
        bool stop = false;
        int i = 1;
        int n;

        public Form1()
        {
            var display_thread = new Thread(() =>
            {
                InitializeComponent();
            });

            display_thread.Start();
            display_thread.Join();
        }

        public int getNextFactorial(int number, int result)
        {
            return result*number;
        }

        public void timePrediction_thread()
        {
            double predictedTime = 0.5 * n;
            double timeToEnd = predictedTime - i * 0.5;

            while(true)
            {
                timeToEnd = predictedTime - i * 0.5;
                Thread.Sleep(734);
                this.BeginInvoke((Action)delegate ()
                {
                    label7.Text = timeToEnd.ToString();
                    if (i <= n)
                    {
                        progressBar1.Value = (int)(((double)i / (double)n) * 100);
                    }
                    else
                    {
                        progressBar1.Value = 100;
                    }
                });

                if(isEnd == true)
                {
                    break;
                }
            }
        }

        public void calculations_thread()
        {
            n = int.Parse(textBox1.Text);
            
            int result=1;
            isEnd = false;
            double time = 0;
            while (i <= n)
            {
                result = getNextFactorial(i, result);
                Console.Write("\n" + result);
                Thread.Sleep(500);
                time += 0.5;
                if(time%1 == 0)
                {
                    this.BeginInvoke((Action)delegate ()
                    {
                        label8.Text = time.ToString();
                    });
                }
                
                i++;

                while (isPaused == true)
                {
                    Thread.Sleep(100);
                }

                if(stop == true)
                {
                    break;
                }
            }

            if (!stop)
            {
                this.BeginInvoke((Action)delegate ()
                {
                    label3.Text = result.ToString();
                });
            }
            isEnd = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread calculations = new Thread(calculations_thread);
            Thread predicitons = new Thread(timePrediction_thread);
            isPaused = !isPaused;

            if(isPaused == false && isEnd == true)
            {
                calculations.Start();
            }
            predicitons.Start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            stop = true;
        }
    }
}
