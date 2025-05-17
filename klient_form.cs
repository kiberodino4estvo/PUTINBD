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

namespace PUTINBD
{
    public partial class klient_form : Form
    {
        db dataBase = new db();

        int selectedRow;
        public klient_form()
        {
            InitializeComponent();
        }

        private void CreateColums()
        {
            dataGridView1.Columns.Add("ID_kliyenta", "id");
            dataGridView1.Columns.Add("FIO_ili_nazvanie_kompanii", "Название компании");
            dataGridView1.Columns.Add("Kontaktniye_dannyye", "Контактные данные");            
            dataGridView1.Columns.Add("Adres", "Адресс");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }
        private void ClearFields()
        {
            textBox_id.Text = "";
            textBox_name.Text = "";
            textBox_kont.Text = "";
            textBox_adr.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Klient";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetSqlConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void klient_form_Load(object sender, EventArgs e)
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

                textBox_id.Text = row.Cells[0].Value.ToString();
                textBox_name.Text = row.Cells[1].Value.ToString();
                textBox_kont.Text = row.Cells[2].Value.ToString();
                textBox_adr.Text = row.Cells[3].Value.ToString();
            }
        }

        private void butn_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void butn_new_Click(object sender, EventArgs e)
        {
            add_form_klient add_Form_Klient = new add_form_klient();
            add_Form_Klient.ShowDialog();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[4].Value = RowState.Deleted;
        }


        private void update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[4].Value;

                if (rowState == RowState.Existed)
                    continue;
                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delQuery = $"delete from Klient where ID_kliyenta = {id}";

                    var command = new SqlCommand(delQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();

                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var kont = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var adr = dataGridView1.Rows[index].Cells[3].Value.ToString(); ;                    

                    var changeQuery = $"update Klient set FIO_ili_nazvanie_kompanii = '{name}', Kontaktniye_dannyye = '{kont}', Adres = '{adr}' where ID_kliyenta = '{id}'";

                    var command = new SqlCommand(changeQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();
                }

            }
            dataBase.closeConnection();
        }

        private void Change()
        {
            var SelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;
            var name = textBox_name.Text;
            var kont = textBox_kont.Text;
            var adr = textBox_adr.Text;

            if (dataGridView1.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[SelectedRowIndex].SetValues(id, name, kont, adr);
                dataGridView1.Rows[SelectedRowIndex].Cells[4].Value = RowState.Modified;
            }
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

        private void btn_save_Click(object sender, EventArgs e)
        {
            update();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}