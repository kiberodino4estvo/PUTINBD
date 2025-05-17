using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;

namespace PUTINBD
{
    public partial class kp_form : Form
    {
        db dataBase = new db();

        int selectedRow;
        public kp_form()
        {
            InitializeComponent();
        }
        private void CreateColums()
        {
            dataGridView1.Columns.Add("ID_kachestva_produktsii", "ID качество продукции");
            dataGridView1.Columns.Add("Standart_kachestva", "Стандарт качества");                      
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }
        private void ClearFields()
        {
            textBox_id_kp.Text = "";
            textBox_stan.Text = "";
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), RowState.ModifiedNew);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Kachestvo_produktsii";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetSqlConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void kp_form_Load(object sender, EventArgs e)
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

                textBox_id_kp.Text = row.Cells[0].Value.ToString();
                textBox_stan.Text = row.Cells[1].Value.ToString();
            }
        }
        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[2].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[2].Value = RowState.Deleted;
        }
        private void update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[2].Value;

                if (rowState == RowState.Existed)
                    continue;
                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delQuery = $"delete from Kachestvo_produktsii where ID_kachestva_produktsii = {id}";

                    var command = new SqlCommand(delQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();

                }

                if (rowState == RowState.Modified)
                {
                    var id_kp = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var stan = dataGridView1.Rows[index].Cells[1].Value.ToString();

                    var changeQuery = $"update Kachestvo_produktsii set  Standart_kachestva = '{stan}' where ID_kachestva_produktsii = '{id_kp}'";

                    var command = new SqlCommand(changeQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();
                }

            }
            dataBase.closeConnection();
        }
        private void Change()
        {
            var SelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id_kp = textBox_id_kp.Text;
            var stan = textBox_stan.Text;

            if (dataGridView1.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[SelectedRowIndex].SetValues(id_kp, stan);
                dataGridView1.Rows[SelectedRowIndex].Cells[2].Value = RowState.Modified;
            }
        }

        private void butn_new_Click(object sender, EventArgs e)
        {
            add_form_kp add_Form_Post = new add_form_kp();
            add_Form_Post.ShowDialog();
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void btn_change_Click(object sender, EventArgs e)
        {
            Change();
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

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void textBox_rez_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
