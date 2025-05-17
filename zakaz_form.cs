using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUTINBD
{
    public partial class zakaz_form : Form
    {
        db dataBase = new db();

        int selectedRow;
        public zakaz_form()
        {
            InitializeComponent();
        }

        private void CreateColums()
        {
            dataGridView1.Columns.Add("ID_zakaza", "ID заказа");
            dataGridView1.Columns.Add("Data_zakaza", "Дата заказа");
            dataGridView1.Columns.Add("ID_kliyenta", "ID клиента");
            dataGridView1.Columns.Add("Status_zakaza", "Статус заказа");
            dataGridView1.Columns.Add("Summa_zakaza", "Сумма заказа");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }
        private void ClearFields()
        {
            textBox_id_z.Text = "";
            textBox_data.Text = "";
            textBox_id_k.Text = "";
            textBox_status.Text = "";
            textBox_sum.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetDateTime(1), record.GetInt32(2), record.GetString(3), record.GetDecimal(4), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Zakaz";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetSqlConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void zakaz_form_Load(object sender, EventArgs e)
        {
            CreateColums();
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_id_z.Text = row.Cells[0].Value.ToString();
                textBox_data.Text = row.Cells[1].Value.ToString();
                textBox_id_k.Text = row.Cells[2].Value.ToString();
                textBox_status.Text = row.Cells[3].Value.ToString();
                textBox_sum.Text = row.Cells[4].Value.ToString();
            }
        }
        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
        }
        private void update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[6].Value;

                if (rowState == RowState.Existed)
                    continue;
                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delQuery = $"delete from Zakaz where ID_zakaza = {id}";

                    var command = new SqlCommand(delQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();

                }

                if (rowState == RowState.Modified)
                {
                    var id_z = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var data = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var id_k = dataGridView1.Rows[index].Cells[2].Value.ToString(); ;
                    var status = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var sum = dataGridView1.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"update Zakaz set  Data_zakaza = '{data}', ID_kliyenta = '{id_k}', Status_zakaza = '{status}', Summa_zakaza = '{sum}' where ID_zakaza = '{id_z}'";

                    var command = new SqlCommand(changeQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();
                }

            }
            dataBase.closeConnection();
        }
        private void Change()
        {
            var SelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id_z = textBox_id_z.Text;
            var data = textBox_data.Text;
            var id_k = textBox_id_k.Text;
            var status = textBox_status.Text;
            var sum = textBox_sum.Text;

            if (dataGridView1.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[SelectedRowIndex].SetValues(id_z, data, id_k, status, sum);
                dataGridView1.Rows[SelectedRowIndex].Cells[5].Value = RowState.Modified;
            }
        }

        private void butn_new_Click(object sender, EventArgs e)
        {
            add_form_zakaz add_Form_Post = new add_form_zakaz();
            add_Form_Post.ShowDialog();
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void butn_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            update();
        }

        private void btn_change_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
