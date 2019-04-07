using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
namespace Eigenvalues
{
    public partial class Eigenvalues : Form
    {
        private Matrix<double> A,B,E;
        private Vector<double> X, Xp,e;
        private double eps;
        public Eigenvalues()
        {
            InitializeComponent();
            InitializeDataGridView(2, 2);
        }
        private void InitializeDataGridView(int rows, int columns)
        {
            dataGridView1.ColumnCount = columns;
            dataGridView1.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;


            for (int i = 0; i < columns; ++i)
            {
                dataGridView1.Columns[i].Width = 64;
                dataGridView1.Columns[i].Name = (i + 1).ToString();
            }

            dataGridView1.RowCount = rows + 2;
            dataGridView1.RowHeadersVisible = true;

            DataGridViewCellStyle rowHeaderStyle = new DataGridViewCellStyle();
            rowHeaderStyle.BackColor = Color.Beige;
            rowHeaderStyle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            dataGridView1.RowHeadersDefaultCellStyle = rowHeaderStyle;

            //f = new Class26BasedSys();
            for (int i = 0; i < rows; ++i)
            {
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                dataGridView1.Rows[i].ReadOnly = false;
            }
            dataGridView1.Rows[rows].HeaderCell.Value = "";
            dataGridView1.Rows[rows].ReadOnly = true;
            dataGridView1.Rows[rows + 1].HeaderCell.Value = "X0";

            dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dataGridView1.RowHeadersWidth = 64;
            numericUpDownColumns.Value = columns;
            numericUpDownRows.Value = rows;
        }

        private void buttonSize_Click(object sender, EventArgs e)
        {
            InitializeDataGridView((int)numericUpDownRows.Value, (int)numericUpDownColumns.Value);
        }

        

        private void InitMatrics()
        {
            int rows = dataGridView1.RowCount - 2,
               columns = dataGridView1.ColumnCount;

            double[,] a = new double[rows, columns];
            double[,] b = new double[rows, columns];
            double[] v = new double[columns];

            for (int i = 0; i < rows-1; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    a[i, j] = Double.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                    b[i, j] = 0;
                }
                b[i, i] = 1;
                v[i] = Double.Parse(dataGridView1.Rows[rows].Cells[i].Value.ToString());
            }
            Xp = Vector<double>.Build.Dense(columns, (i) => v[i]);
            A = Matrix<double>.Build.Dense(rows - 1, columns, (i, j) => a[i, j]);
            E = Matrix<double>.Build.Dense(rows - 1, columns, (i, j) => b[i, j]);
        }

        private double MaxEigenvalue(Matrix<double>A_, Vector<double> xp, ref Vector<double>x, ref Vector<double>e_)
        {
            double u, up;

            e = xp / xp.Norm(2);
            x = A_ * e;
            u = x.DotProduct(e);

            do
            {
                xp = x;
                up = u;
                e = xp / xp.Norm(2);
                x = A_ * e;
                u = x.DotProduct(e);
            }
            while (Math.Abs(u - up) > eps);

            return u;
        }

        private void Execute()
        {
            InitMatrics();
            double l_B=0, l_A = MaxEigenvalue(A, Xp, ref X, ref e);
            B = l_A * E - A;
            if (A.IsSymmetric())
            {
                l_B = MaxEigenvalue(B, Xp, ref X, ref e);
                
                   labelMin.Text = "Min eigenvalue : " + (l_A - l_B);
            }
           
                labelMax.Text = "Max eigenvalue : " + l_A;
        } 


        private void button1_Click(object sender, EventArgs e)
        {
            if (!Double.TryParse(textBoxAccuracy.Text, out eps))
                MessageBox.Show("Enter correct Acuracy");
            else
                Execute();
            
        }
    }
}
