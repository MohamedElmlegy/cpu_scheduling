using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public struct process
    {
        public int id, WT, AT, BT, TAT, PR, ST, ET;
    };
    public partial class Form5 : Form
    {
        string type;
        String[] all_types;
        int quant;
        int no_prc;
        double avg_waiting_t;
        List<int> arrival;
        List<int> burst;
        List<int> id;
        List<int> priorty;
        Size form1_with_chart;
        public Form5()
        {
            all_types = new string[] { "FCFS", "SJF (Non Preemptive)", "SJF (Preemptive)", "Priority (Non Preemptive)", "Priority (Preemptive)", "Round Robin" };
            type = "";
            no_prc = 0;
            form1_with_chart = new Size(1062, 580);
            id = new List<int>();
            arrival = new List<int>();
            burst = new List<int>();
            priorty = new List<int>();
            InitializeComponent();
            label4.Hide();
            label6.Hide();
            textBox6.Hide();
            textBox7.Hide();

        }

        private void next_Click(object sender, EventArgs e)
        {
            switch (this.type)
            {
                case "Priority (Preemptive)":
                case "Priority (Non Preemptive)":

                    string input = textBox1.Text;
                    List<string> list_id = new List<string>(
                                               input.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));

                    input = textBox2.Text;
                    List<string> list_arrival = new List<string>(
                                               input.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));

                    input = textBox3.Text;
                    List<string> list_burst = new List<string>(
                                               input.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));
                    input = textBox6.Text;
                    List<string> list_priorty = new List<string>(
                                               input.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));
                    if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(textBox4.Text))
                    {
                        MessageBox.Show("the information you entered is incorrect or incomplete", "Missinginfo");

                    }
                    else
                    {
                        this.no_prc = int.Parse(textBox4.Text);
                        if (no_prc != list_id.Count || no_prc != list_arrival.Count || no_prc != list_burst.Count || no_prc != list_priorty.Count)
                        {
                            MessageBox.Show("the information you entered is incorrect or incomplete", "Missinginfo");
                        }
                        else if (have_dublicates(list_id))
                        {
                            MessageBox.Show("you have entered information for the same process twice\r\n please reenter the processes id correctly", "dublicate processes");
                            textBox1.Text = "";
                        }
                        else
                        {

                            /*if (this.Size.Height < form1_with_chart.Height || this.Size.Width < form1_with_chart.Width)
                            {
                                this.Size = form1_with_chart;
                            }*/
                            chart1.Series.Clear();
                            chart1.Show();
                            List<int> p_arrival = new List<int>();
                            List<int> p_burst = new List<int>();
                            List<int> p_id = new List<int>();
                            List<int> p_priorty = new List<int>();
                            p_id = slist_to_ilist(list_id);
                            p_arrival = slist_to_ilist(list_arrival);
                            p_burst = slist_to_ilist(list_burst);
                            p_priorty = slist_to_ilist(list_priorty);

                            switch (type)
                            {
                                case "Priority (Preemptive)":
                                    sort_process_by_arrival(p_id, p_arrival, p_burst, p_priorty);
                                    add_process_in_chart();
                                    int total = 0;
                                    for (int i = 0; i < this.no_prc; i++)
                                    {
                                        total += p_burst[i];
                                    }
                                    int[] arr = new int[total];

                                    preemptivePriority(arr, ref this.avg_waiting_t, this.no_prc, ilist_to_iarray(p_id), ilist_to_iarray(p_burst), ilist_to_iarray(p_arrival), ilist_to_iarray(p_priorty));
                                    grantt_chart_pri_PRE(arr);
                                    textBox5.Text = this.avg_waiting_t.ToString("0.00");
                                    break;

                                case "Priority (Non Preemptive)":

                                    add_process_in_chart();
                                    int[,] SET = new int[this.no_prc, 3];
                                    NPP(this.no_prc, ilist_to_iarray(p_id), ilist_to_iarray(p_arrival), ilist_to_iarray(p_burst), ilist_to_iarray(p_priorty), ref this.avg_waiting_t, SET);
                                    grantt_chart_pri(SET);
                                    textBox5.Text = this.avg_waiting_t.ToString("0.00");
                                    break;

                                default:
                                    break;


                            }
                        }
                    }
                    break;

                case "FCFS":
                case "SJF (Non Preemptive)":
                case "SJF (Preemptive)":
                    string input1 = textBox1.Text;
                    List<string> list_id1 = new List<string>(
                                               input1.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));

                    input1 = textBox2.Text;
                    List<string> list_arrival1 = new List<string>(
                                               input1.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));

                    input1 = textBox3.Text;
                    List<string> list_burst1 = new List<string>(
                                               input1.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));
                    if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(textBox4.Text))
                    {
                        MessageBox.Show("the information you entered is incorrect or incomplete", "Missinginfo");

                    }
                    else
                    {
                        this.no_prc = int.Parse(textBox4.Text);
                        if (no_prc != list_id1.Count || no_prc != list_arrival1.Count || no_prc != list_burst1.Count)
                        {
                            MessageBox.Show("the information you entered is incorrect or incomplete", "Missinginfo");
                        }
                        else if (have_dublicates(list_id1))
                        {
                            MessageBox.Show("you have entered information for the same process twice\r\n please reenter the processes id correctly", "dublicate processes");
                            textBox1.Text = "";
                        }
                        else
                        {

                            /*if (this.Size.Height < form1_with_chart.Height || this.Size.Width < form1_with_chart.Width)
                            {
                                this.Size = form1_with_chart;
                            }*/
                            chart1.Series.Clear();
                            chart1.Show();
                            List<int> p_arrival = new List<int>();
                            List<int> p_burst = new List<int>();
                            List<int> p_id = new List<int>();
                            p_id = slist_to_ilist(list_id1);
                            p_arrival = slist_to_ilist(list_arrival1);
                            p_burst = slist_to_ilist(list_burst1);
                            switch (type)
                            {
                                case "FCFS":
                                    sort_process_by_arrival(p_id, p_arrival, p_burst);
                                    add_process_in_chart();
                                    grantt_chart_fcfs(p_id, p_arrival, p_burst);
                                    textBox5.Text = this.avg_waiting_t.ToString("0.00");
                                    break;

                                case "SJF (Preemptive)":

                                    List<List<int>> entry_order = new List<List<int>>();
                                    sjf(true, ilist_to_darray(p_arrival), ilist_to_darray(p_burst), entry_order);
                                    add_process_in_chart();
                                    grantt_chart_SJF_PRE(entry_order);
                                    textBox5.Text = this.avg_waiting_t.ToString("0.00");
                                    break;
                                case "SJF (Non Preemptive)":

                                    List<List<int>> entry_order2 = new List<List<int>>();
                                    sjf(false, ilist_to_darray(p_arrival), ilist_to_darray(p_burst), entry_order2);
                                    add_process_in_chart();
                                    grantt_chart_SJF_PRE(entry_order2);
                                    textBox5.Text = this.avg_waiting_t.ToString("0.00");
                                    break;

                            }
                        }
                    }

                    break;
                case "Round Robin":
                    string input2 = textBox1.Text;
                    List<string> list_id2 = new List<string>(
                                               input2.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));

                    input2 = textBox2.Text;
                    List<string> list_arrival2 = new List<string>(
                                               input2.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));

                    input2 = textBox3.Text;
                    List<string> list_burst2 = new List<string>(
                                               input2.Split(new string[] { "\r\n" },
                                               StringSplitOptions.RemoveEmptyEntries));
                    if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(textBox4.Text) || String.IsNullOrEmpty(textBox7.Text))
                    {
                        MessageBox.Show("the information you entered is incorrect or incomplete", "Missinginfo");

                    }
                    else
                    {
                        this.no_prc = int.Parse(textBox4.Text);
                        this.quant = int.Parse(textBox7.Text);
                        if (no_prc != list_id2.Count || no_prc != list_arrival2.Count || no_prc != list_burst2.Count)
                        {
                            MessageBox.Show("the information you entered is incorrect or incomplete", "Missinginfo");
                        }
                        else if (have_dublicates(list_id2))
                        {
                            MessageBox.Show("you have entered information for the same process twice\r\n please reenter the processes id correctly", "dublicate processes");
                            textBox1.Text = "";
                        }
                        else
                        {

                            /*if (this.Size.Height < form1_with_chart.Height || this.Size.Width < form1_with_chart.Width)
                            {
                                this.Size = form1_with_chart;
                            }*/
                            chart1.Series.Clear();
                            chart1.Show();
                            List<int> p_arrival = new List<int>();
                            List<int> p_burst = new List<int>();
                            List<int> p_id = new List<int>();
                            p_id = slist_to_ilist(list_id2);
                            p_arrival = slist_to_ilist(list_arrival2);
                            p_burst = slist_to_ilist(list_burst2);

                            sort_process_by_arrival(p_id, p_arrival, p_burst);
                            add_process_in_chart();
                            List<List<int>> output = new List<List<int>>();
                            round_robin(output, p_id, p_arrival, p_burst, this.quant, this.no_prc, ref this.avg_waiting_t);
                            // where the last drawing function will be  ......private void round_robin_chart (List<List<int>> o)
                            textBox5.Text = this.avg_waiting_t.ToString("0.00");

                        }
                    }
                    break;

            }

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            s = s.Replace("\r\n", "");
            bool isNumber = long.TryParse(s, out long n_p);
            if (!isNumber && textBox1.Text != "")
            {
                if (textBox1.Text.Length >= 1)
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                }
                else
                {
                    textBox1.Text = "";
                }
                MessageBox.Show("you didnt enter a correct numper", "NUMBERS!?");
            }
            else
            {
                if (n_p < 0 && textBox1.Text != "")
                {
                    MessageBox.Show("pleese enter a positive number", "NEGATIVE PROCESSES?");
                    if (textBox1.Text.Length >= 1)
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                }


            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string s = textBox2.Text;
            s = s.Replace("\r\n", "");
            bool isNumber = long.TryParse(s, out long n_p);
            if (!isNumber && textBox2.Text != "")
            {
                if (textBox2.Text.Length >= 1)
                {
                    textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1, 1);
                }
                else
                {
                    textBox2.Text = "";
                }
                MessageBox.Show("you didnt enter a correct numper", "NUMBERS!?");
            }
            else
            {
                if (n_p < 0 && textBox2.Text != "")
                {
                    MessageBox.Show("pleese enter a positive number", "NEGATIVE PROCESSES?");
                    if (textBox2.Text.Length >= 1)
                    {
                        textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1, 1);
                    }
                    else
                    {
                        textBox2.Text = "";
                    }
                }


            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string s = textBox3.Text;
            s = s.Replace("\r\n", "");
            bool isNumber = long.TryParse(s, out long n_p);

            if (!isNumber && textBox3.Text != "")
            {
                if (textBox3.Text.Length >= 1)
                {
                    textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1, 1);
                }
                else
                {
                    textBox3.Text = "";
                }
                MessageBox.Show("you didnt enter a correct numper", "NUMBERS!?");
            }
            else
            {
                if (n_p < 0 && textBox3.Text != "")
                {
                    MessageBox.Show("pleese enter a positive number", "NEGATIVE PROCESSES?");
                    if (textBox3.Text.Length >= 1)
                    {
                        textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1, 1);
                    }
                    else
                    {
                        textBox3.Text = "";
                    }
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(textBox4.Text, out int n_p);

            if (!isNumber && textBox4.Text != "")
            {
                if (textBox4.Text.Length >= 1)
                {
                    textBox4.Text = textBox4.Text.Remove(textBox4.Text.Length - 1, 1);
                }
                else
                {
                    textBox4.Text = "";
                }
                MessageBox.Show("you didnt enter a correct numper", "NUMBERS!?");
            }
            else
            {
                if (n_p < 1 && textBox4.Text != "")
                {
                    MessageBox.Show("pleese enter a positive number", "NEGATIVE PROCESSES?");
                    if (textBox4.Text.Length >= 1)
                    {
                        textBox4.Text = textBox4.Text.Remove(textBox4.Text.Length - 1, 1);
                    }
                    else
                    {
                        textBox4.Text = "";
                    }
                }
            }

        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string s = textBox6.Text;
            s = s.Replace("\r\n", "");
            bool isNumber = long.TryParse(s, out long n_p);

            if (!isNumber && textBox6.Text != "")
            {
                if (textBox6.Text.Length >= 1)
                {
                    textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1, 1);
                }
                else
                {
                    textBox6.Text = "";
                }
                MessageBox.Show("you didnt enter a correct numper", "NUMBERS!?");
            }
            else
            {
                if (n_p < 0 && textBox6.Text != "")
                {
                    MessageBox.Show("pleese enter a positive number", "NEGATIVE PROCESSES?");
                    if (textBox6.Text.Length >= 1)
                    {
                        textBox6.Text = textBox6.Text.Remove(textBox6.Text.Length - 1, 1);
                    }
                    else
                    {
                        textBox6.Text = "";
                    }
                }
            }
        }
        private void add_process_in_chart()
        {
            chart1.Series.Add("Process");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeBar;
            chart1.Series[0].IsXValueIndexed = false;
            chart1.Series[0].Color = Color.DarkBlue;
            //chart1.ChartAreas[0].AxisY.MinorTickMark.Interval = chart1.ChartAreas[0].AxisY.MajorTickMark.Interval/2;

        }
        private Boolean have_dublicates(List<string> list)
        {
            if (list.Count != list.Distinct().Count())
            {
                return true;
            }
            return false;
        }
        private List<int> slist_to_ilist(List<string> s)
        {
            List<int> ilist = new List<int>();
            for (int i = 0; i < s.Count; i++)
            {
                ilist.Add(int.Parse(s[i]));
            }
            return ilist;
        }
        private void sort_process_by_arrival(List<int> p, List<int> ar, List<int> brst, List<int> priorty)
        {

            for (int i = 0; i < p.Count; i++)
            {
                int min = Int32.MaxValue;
                int min_indx = -1;
                for (int j = i; j < p.Count; j++)
                {
                    if (ar[j] < min)
                    {
                        min = ar[j];
                        min_indx = j;
                    }

                }
                int temp1 = p[i];
                p[i] = p[min_indx];
                p[min_indx] = temp1;
                int temp2 = ar[i];
                ar[i] = ar[min_indx];
                ar[min_indx] = temp2;
                int temp3 = brst[i];
                brst[i] = brst[min_indx];
                brst[min_indx] = temp3;
                int temp4 = priorty[i];
                priorty[i] = priorty[min_indx];
                priorty[min_indx] = temp4;
            }
        }
        private void sort_process_by_arrival(List<int> p, List<int> ar, List<int> brst)
        {

            for (int i = 0; i < p.Count; i++)
            {
                int min = Int32.MaxValue;
                int min_indx = -1;
                for (int j = i; j < p.Count; j++)
                {
                    if (ar[j] < min)
                    {
                        min = ar[j];
                        min_indx = j;
                    }

                }
                int temp1 = p[i];
                p[i] = p[min_indx];
                p[min_indx] = temp1;
                int temp2 = ar[i];
                ar[i] = ar[min_indx];
                ar[min_indx] = temp2;
                int temp3 = brst[i];
                brst[i] = brst[min_indx];
                brst[min_indx] = temp3;
            }
        }
        private int[] ilist_to_iarray(List<int> i)
        {
            int[] d = new int[i.Count];
            for (int j = 0; j < i.Count; j++)
            {
                d[j] = i[j];
            }
            return d;
        }

        private void select(int timeAxis, ref int nextArrival, ref int currentProcess, ref int currentBurst, int size, int[] burstTime, int[] arrivalTime, int[] priority)
        {
            Boolean cond = false;
            int higherPriority = Int32.MaxValue;
            int min_i = 0;

            // when last process arrives, so there is no need to be included as a new selection decision 
            if ((timeAxis == nextArrival) && (arrivalTime[size - 1] == nextArrival))
            {
                nextArrival = Int32.MaxValue;
            }


            for (int i = 0; i < size; i++)
            {
                // update the next arrival time to take a new selection decision
                if (higherPriority != Int32.MaxValue && arrivalTime[i] > timeAxis)
                {
                    nextArrival = arrivalTime[i];
                    cond = true;
                }
                else
                {
                    // select the first arrived process with the highest priority
                    if (burstTime[i] != 0 && priority[i] < higherPriority)
                    {
                        min_i = i;
                        higherPriority = priority[i];
                    }


                }

                if (cond) break;
            }

            currentProcess = min_i;
            currentBurst = burstTime[currentProcess];
        }
        private double[] ilist_to_darray(List<int> i)
        {
            double[] d = new double[i.Count];
            for (int j = 0; j < i.Count; j++)
            {
                d[j] = i[j];
            }
            return d;
        }
        private void preemptivePriority(int[] arry, ref double averageWaitingTime, int size, int[] processes, int[] burstTime, int[] arrivalTime, int[] priority)
        {
            int totalBurst = 0;
            int timeAxis = 0;
            int nextArrival = 0;
            int currentProcess = 0;
            int currentBurst = 0;
            int[] waiting = new int[50];  // limited to max 50 process


            //calculate total burst time
            for (int i = 0; i < size; i++)
            {
                totalBurst += burstTime[i];
            }

            //Initialize the waiting array
            for (int i = 0; i < size; i++)
            {
                waiting[i] = -(arrivalTime[i] + burstTime[i]);
            }


            while (timeAxis < totalBurst)
            {     //this condition violate when all process complete execution
                select(timeAxis, ref nextArrival, ref currentProcess, ref currentBurst, size, burstTime, arrivalTime, priority); //selection decision
                while (timeAxis < nextArrival && currentBurst != 0)
                {  // new selection decision taken when it complete or there is another process arrived
                    arry[timeAxis] = processes[currentProcess];
                    timeAxis++;
                    currentBurst--;
                }
                burstTime[currentProcess] = currentBurst;  // save the status of the currernt process before proceed to the decision 
                if (currentBurst == 0) waiting[currentProcess] += timeAxis;
            }

            //calculate average waiting time
            averageWaitingTime = 0;
            for (int i = 0; i < size; i++)
            {
                averageWaitingTime += waiting[i];
            }
            averageWaitingTime /= size;
        }
        private void grantt_chart_pri_PRE(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {

                chart1.Series[0].Points.AddXY(arr[i], i, i + 1);

            }
        }
        private void grantt_chart_pri(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {

                chart1.Series[0].Points.AddXY(arr[i, 0], arr[i, 1], arr[i, 2]);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;
            this.type = all_types[i];
            switch (type)
            {
                case "Priority (Preemptive)":
                case "Priority (Non Preemptive)":
                    textBox6.Show();
                    label4.Show();
                    textBox7.Hide();
                    label6.Hide();


                    break;
                case "FCFS":
                case "SJF (Non Preemptive)":
                case "SJF (Preemptive)":
                    label4.Hide();
                    label6.Hide();
                    textBox6.Hide();
                    textBox7.Hide();
                    break;
                case "Round Robin":
                    label6.Show();
                    textBox7.Show();
                    break;
                default:
                    break;

            }

        }
        private void grantt_chart_fcfs(List<int> p_id, List<int> p_arrival, List<int> p_burst)
        {

            int start = p_arrival[0];
            int[] wait = new int[p_id.Count];
            wait[0] = 0;
            double total_wt = 0;
            for (int i = 1; i < p_id.Count; i++)
            {
                if (p_burst[i - 1] + wait[i - 1] > p_arrival[i])
                {
                    wait[i] = p_burst[i - 1] + wait[i - 1];//- p_arrival[i];
                }
                else
                {
                    wait[i] = p_arrival[i];
                }
            }
            for (int i = 0; i < p_id.Count; i++)
            {
                total_wt = total_wt + wait[i] - p_arrival[i];
            }
            avg_waiting_t = total_wt / p_id.Count;
            for (int i = 0; i < p_id.Count; i++)
            {
                chart1.Series[0].Points.AddXY(p_id[i], wait[i], wait[i] + p_burst[i]);
            }
        }
        private void grantt_chart_SJF_PRE(List<List<int>> entry_order)
        {
            for (int i = 0; i < entry_order.Count; i++)
            {

                chart1.Series[0].Points.AddXY(entry_order[i][0], entry_order[i][1], entry_order[i][2]);

            }
        }
        private void sjf(bool preemptive, double[] arr_times, double[] burst_times, List<List<int>> entry_order)
        {
            double[] start_times = Enumerable.Repeat(-1.0, arr_times.Length).ToArray();
            double[] completion_times = Enumerable.Repeat(-1.0, arr_times.Length).ToArray();
            double[] turnaround_times = Enumerable.Repeat(-1.0, arr_times.Length).ToArray();
            double[] waiting_times = Enumerable.Repeat(-1.0, arr_times.Length).ToArray();
            double[] response_times = Enumerable.Repeat(-1.0, arr_times.Length).ToArray();

            double total_turnaround_time = 0;
            double total_waiting_time = 0;
            double total_response_time = 0;
            double total_idle_time = 0;
            double avg_turnaround_time = 0;
            double avg_waiting_time = 0;
            double avg_response_time = 0;
            int[] is_completed = new int[arr_times.Length];
            double[] burst_remaining = new double[arr_times.Length];
            Array.Copy(burst_times, 0, burst_remaining, 0, burst_times.Length);

            double current_time = 0;
            double completed = 0;
            double prev = 0;
            double quantum = 1;

            // [pid, start, end=start+1] >> pid starts from 1 not 0!
            // List<List<int>> entry_order = new List<List<int>>();

            while (completed != arr_times.Length)
            {
                int turn = -1;
                double min_rt = 10000000;
                foreach (var i in Enumerable.Range(0, arr_times.Length))
                {
                    if (arr_times[i] <= current_time && is_completed[i] == 0)
                    {
                        if (burst_remaining[i] < min_rt)
                        {
                            min_rt = burst_remaining[i];
                            turn = i;
                            continue;
                        }

                        if (burst_remaining[i] == min_rt && arr_times[i] < arr_times[turn])
                        {
                            min_rt = burst_remaining[i];
                            turn = i;
                        }
                    }
                }

                if (turn != -1)
                {
                    if (burst_remaining[turn] == burst_times[turn])
                    {
                        start_times[turn] = current_time;
                        total_idle_time += start_times[turn] - prev;
                    }
                    if (!preemptive)
                    {
                        quantum = burst_times[turn];
                    }
                    entry_order.Add(new List<int> { turn + 1, (int)current_time, (int)(current_time + quantum) });


                    burst_remaining[turn] -= quantum;
                    current_time += quantum;
                    prev = current_time;

                    if (burst_remaining[turn] == 0)
                    {
                        completion_times[turn] = current_time;
                        turnaround_times[turn] = (double)completion_times[turn] - arr_times[turn];
                        waiting_times[turn] = turnaround_times[turn] - burst_times[turn];
                        response_times[turn] = (double)start_times[turn] - arr_times[turn];

                        total_turnaround_time += turnaround_times[turn];
                        total_waiting_time += waiting_times[turn];
                        total_response_time += response_times[turn];

                        is_completed[turn] = 1;
                        completed += 1;
                    }

                }
                else
                {
                    current_time += quantum;
                }
            }

            avg_turnaround_time = total_turnaround_time / arr_times.Length;
            avg_waiting_time = total_waiting_time / arr_times.Length;
            avg_response_time = total_response_time / arr_times.Length;
            this.avg_waiting_t = avg_waiting_time;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
        private void NPP(int pn, int[] p, int[] AT, int[] BT, int[] PT, ref double AWT, int[,] SET)
        {

            process[] a = new process[pn];
            int check_ar = 0;
            int Cmp_time = 0;
            float Total_WT = 0, Total_TAT = 0, Avg_TAT;

            for (int i = 0; i < pn; i++)
            {
                a[i].id = p[i];
                a[i].AT = AT[i];
                a[i].BT = BT[i];
                a[i].PR = PT[i];

                // here we are checking that arrival time
                // of the process are same or different
                if (i == 0)
                    check_ar = a[i].AT;

                if (check_ar != a[i].AT)
                    check_ar = 1;
            }
            if (check_ar != 0)
            {
                for (int i = 0; i < pn; i++)
                {
                    for (int j = 0; j < pn - i - 1; j++)
                    {
                        if (a[j].AT > a[j + 1].AT)
                        {
                            swap(ref a[j].id, ref a[j + 1].id);
                            swap(ref a[j].AT, ref a[j + 1].AT);
                            swap(ref a[j].BT, ref a[j + 1].BT);
                            swap(ref a[j].PR, ref a[j + 1].PR);
                        }
                    }
                }
            }
            if (check_ar != 0)
            {
                a[0].WT = a[0].AT;
                a[0].TAT = a[0].BT - a[0].AT;
                // cmp_time for completion time
                Cmp_time = a[0].TAT;
                Total_WT = Total_WT + a[0].WT;
                Total_TAT = Total_TAT + a[0].TAT;

                //Starting and Ending time
                a[0].ET = a[0].TAT + a[0].AT;
                a[0].ST = a[0].ET - a[0].BT;
                for (int i = 1; i < pn; i++)
                {
                    int min = a[i].PR;
                    for (int j = i + 1; j < pn; j++)
                    {
                        if (min > a[j].PR && a[j].AT <= Cmp_time)
                        {
                            min = a[j].PR;
                            swap(ref a[i].id, ref a[j].id);
                            swap(ref a[i].AT, ref a[j].AT);
                            swap(ref a[i].BT, ref a[j].BT);
                            swap(ref a[i].PR, ref a[j].PR);
                        }
                    }
                    a[i].WT = Cmp_time - a[i].AT;
                    Total_WT = Total_WT + a[i].WT;
                    // completion time of the process
                    Cmp_time = Cmp_time + a[i].BT;

                    // Turn Around Time of the process
                    // compl-Arival
                    a[i].TAT = Cmp_time - a[i].AT;
                    Total_TAT = Total_TAT + a[i].TAT;

                    //Starting and Ending time
                    a[i].ET = a[i].TAT + a[i].AT;
                    a[i].ST = a[i].ET - a[i].BT;
                }
            }
            else
            {
                for (int i = 0; i < pn; i++)
                {
                    int min = a[i].PR;
                    for (int j = i + 1; j < pn; j++)
                    {
                        if (min > a[j].PR && a[j].AT <= Cmp_time)
                        {
                            min = a[j].PR;
                            swap(ref a[i].id, ref a[j].id);
                            swap(ref a[i].AT, ref a[j].AT);
                            swap(ref a[i].BT, ref a[j].BT);
                            swap(ref a[i].PR, ref a[j].PR);
                        }
                    }
                    a[i].WT = Cmp_time - a[i].AT;

                    // completion time of the process
                    Cmp_time = Cmp_time + a[i].BT;

                    // Turn Around Time of the process
                    // compl-Arrival
                    a[i].TAT = Cmp_time - a[i].AT;
                    Total_WT = Total_WT + a[i].WT;
                    Total_TAT = Total_TAT + a[i].TAT;

                    //Starting and Ending time
                    a[i].ET = a[i].TAT + a[i].AT;
                    a[i].ST = a[i].ET - a[i].BT;
                }
            }
            for (int i = 0; i < pn; i++)
            {
                SET[i, 0] = a[i].id;
                SET[i, 1] = a[i].ST;
                SET[i, 2] = a[i].ET;
            }
            AWT = Total_WT / pn;
            Avg_TAT = Total_TAT / pn;
        }
        private void swap(ref int b, ref int c)
        {
            int tem;
            tem = c;
            c = b;
            b = tem;
        }
        private void round_robin(List<List<int>> output, List<int> p_id, List<int> at, List<int> bt, int quant, int nop , ref double avg_wt)
        {

            // initlialize the variable name  
            int i, sum = 0, count = 0, y, wt = 0, tat = 0;
            int loopn = -1;
            List<int> temp = new List<int>();
            double  avg_tat;
            y = nop;
            for (i = 0; i < nop; i++)
            {

                temp[i] = bt[i]; // store the burst time in temp array  
            }

            for (sum = 0, i = 0; y != 0;)
            {
                loopn++;
                if (temp[i] <= quant && temp[i] > 0)
                {
                    sum = sum + temp[i];
                    output[loopn][0] = p_id[i];
                    output[loopn][1] = temp[i];
                    temp[i] = 0;
                    count = 1;
                }
                else if (temp[i] > 0)
                {
                    temp[i] = temp[i] - quant;
                    sum = sum + quant;
                    output[loopn][0] = p_id[i];
                    output[loopn][1] = quant;
                }
                else if (temp[i] == 0 && count != 1)
                {
                    output[loopn][0] = p_id[i];
                    output[loopn][1] = 0;
                }
                if (temp[i] == 0 && count == 1)
                {
                    y--; //decrement the process no.  
                    wt = wt + sum - at[i] - bt[i];
                    tat = tat + sum - at[i];
                    count = 0;
                }
                if (i == nop - 1)
                {
                    i = 0;
                }
                else if (at[i + 1] <= sum)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
            }
            avg_wt = wt * 1.0 / nop;
            avg_tat = tat * 1.0 / nop;

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(textBox7.Text, out int n_p);

            if (!isNumber && textBox7.Text != "")
            {
                if (textBox7.Text.Length >= 1)
                {
                    textBox7.Text = textBox7.Text.Remove(textBox7.Text.Length - 1, 1);
                }
                else
                {
                    textBox7.Text = "";
                }
                MessageBox.Show("you didnt enter a correct numper", "NUMBERS!?");
            }
            else
            {
                if (n_p < 1 && textBox7.Text != "")
                {
                    MessageBox.Show("pleese enter a positive number", "NEGATIVE TIME?");
                    if (textBox7.Text.Length >= 1)
                    {
                        textBox7.Text = textBox7.Text.Remove(textBox7.Text.Length - 1, 1);
                    }
                    else
                    {
                        textBox7.Text = "";
                    }
                }
            }
        }
        /*private void round_robin_chart (List<List<int>> o)
        {
            



        }*/
    }
}
