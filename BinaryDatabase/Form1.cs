using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryDatabase
{
    public partial class Form1 : Form
    {
        const string fileName = "data.dat";

        public Form1()
        {
            InitializeComponent();
            wczytajDaneToolStripMenuItem_Click(this,EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount>9)
            {
                //MessageBox.Show("Wprowadzono za dużo pozycji, zostanie skasowana pozycja nr 1");
                dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
                dataGridView1.Rows.Add(textBox1.Text, textBox2.Text, dateTimePicker1.Text, comboBox1.Text);
            }
            else
            {
                dataGridView1.Rows.Add(textBox1.Text, textBox2.Text, dateTimePicker1.Text, comboBox1.Text);
            }
        }


        void WriteValues()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    writer.Write(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    writer.Write(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    writer.Write(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    writer.Write(dataGridView1.Rows[i].Cells[3].Value.ToString());
                }
                
                 
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count>0)
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                dataGridView1.CurrentRow.Cells[0].Value = textBox1.Text;
                dataGridView1.CurrentRow.Cells[1].Value = textBox2.Text;
                dataGridView1.CurrentRow.Cells[2].Value = dateTimePicker1.Text;
                dataGridView1.CurrentRow.Cells[3].Value = comboBox1.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            
        }

        private void zapiszZmianyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteValues();
        }

        private void wczytajDaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (dataGridView1.SelectedCells.Count > 0)
            {              
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
           
               if (File.Exists(fileName))
               {
                   try
                   {
                       using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                       {
                           for (int i = 0; i < 10; i++)
                           {
                               dataGridView1.Rows.Add(reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString());
                           }
                       }
                   }
                   catch (EndOfStreamException)
                   {
                       return;
                   }
            }
        }

        private void importujDaneZTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All files (*.*)|*.*|Txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            while (dataGridView1.SelectedCells.Count > 0)
                            {
                                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                            }
                            System.IO.StreamReader file = new System.IO.StreamReader(myStream);
                            string read = "1";
                            for (int i = 0; i < 10; i++)
                            {
                                read = file.ReadLine();
                                if (read!=null)
                                {
                                    dataGridView1.Rows.Add(read, file.ReadLine(), file.ReadLine(), file.ReadLine());
                                } 
                            }
                            file.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd: Nie można było odczytać danych z dysku. Szczegóły: " + ex.Message);
                }
            }
        }

        private void eksportDoTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*|Txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(myStream))
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString());
                            sw.WriteLine(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            sw.WriteLine(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            sw.WriteLine(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        }

                    }
                    myStream.Close();
                }
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Zapisać zmiany w bazie?", "Zamykanie programu", MessageBoxButtons.YesNoCancel);
            if (dialog==DialogResult.Yes)
            {
                WriteValues();
            }
            if (dialog==DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
