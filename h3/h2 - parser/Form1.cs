using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace h2___parser
{
    public partial class Form1 : Form
    {

        Int64 range1 = 0, range2 = 0, range3 = 0;
        int limit1 = 500, limit2 = 1000;
        int column = 3;
        bool skipHeader = true;

        DataTable dt; 

        public Form1()
        {
            InitializeComponent();
            warning(false);
            invalidRange(false);
            start1.Text = "0"; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Open a CSV file";
            openFileDialog1.Filter = "CSV file|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            

            if (result == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                textBox6.Text = fileName; 
                bindData(fileName);
            }
        }

        private void bindData(string filePath)
        {
            dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                // First line to create header
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }
                // Print how many IP address have been analyzed
                textBox5.Text = (lines.Length - 1).ToString(); // the header line must not be counted 

                // For Data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            warning(true);
            try
            {
                int e1 = Int32.Parse(end1.Text);
                start2.Text = e1.ToString(); 
            }
            catch(Exception ex)
            {
                warning(true);
                invalidRange(true); 
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            warning(true);
            try
            {
                int e2 = Int32.Parse(end2.Text);
                last.Text = e2.ToString();
            }
            catch (Exception ex)
            {
                warning(true);
                invalidRange(true);
            }
        }


        private void calculateBtn_Click(object sender, EventArgs e)
        {
            value1.Clear();
            value2.Clear();
            value3.Clear();
            int s1=0, e1=0, s2=0, e2=0, last=0, r1=0, r2=0, r3=0; 
            try
            {
                s1 = 0;
                e1 = Int32.Parse(end1.Text);
                s2 = e1;
                e2 = Int32.Parse(end2.Text);
                last = e2;
            }
            catch (Exception ex)
            {
                invalidRange(true);
                return;
            }
            
            if(!(s1 < e1 && s2 < e2))
            {
                invalidRange(true);
                return; 
            }

           
            int col = 2;
            foreach(DataRow row in dt.Rows)
            {
                int v = Int32.Parse(row["\"Count\""].ToString());
                if (v >= s1 && v < e1) r1++;
                else if (v >= s2 && v < e2) r2++;
                else r3++;
            }

            value1.Text = r1.ToString();
            value2.Text = r2.ToString();
            value3.Text = r3.ToString(); 

            warning(false);
            invalidRange(false);


        }


        private void warning(Boolean t)
        {
            if (t)
            {
                calculateBtn.BackColor = Color.Red;
                label9.ForeColor = Color.Red;
            }
            else
            {
                calculateBtn.BackColor = Color.White;
                label9.ForeColor = Color.Transparent;
            }
        }

        private void invalidRange(Boolean t)
        {
            if (t) label15.ForeColor = Color.Red;
            else label15.ForeColor = Color.Transparent;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

