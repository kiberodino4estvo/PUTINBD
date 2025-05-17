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
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class post_form : Form
    {
        db dataBase = new db();

        int selectedRow;

        public post_form()
        {
            InitializeComponent();
        }

    private void CreateColums()
        {
            dataGridView1.Columns.Add("ID_postavshika", "id");
            dataGridView1.Columns.Add("Nazvanie_kompanii", "Название компании");
            dataGridView1.Columns.Add("Kontaktnoe_litso", "Контактное лицо");
            dataGridView1.Columns.Add("Telefon", "Телефон");
            dataGridView1.Columns.Add("Email", "Емайл");
            dataGridView1.Columns.Add("Adres", "Адресс");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ClearFields()
        {
            textBox_id.Text = "";
            textBox_nazv.Text = "";
            textBox_litso.Text = "";
            textBox_tel.Text = "";
            textBox_email.Text = "";
            textBox_Adr.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Postavshik";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetSqlConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void Main_Load(object sender, EventArgs e)
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
                textBox_nazv.Text = row.Cells[1].Value.ToString();
                textBox_litso.Text = row.Cells[2].Value.ToString();
                textBox_tel.Text = row.Cells[3].Value.ToString();
                textBox_email.Text = row.Cells[4].Value.ToString();
                textBox_Adr.Text = row.Cells[5].Value.ToString();
            }
        }
        private void butn_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearFields();
        }

        private void butn_new_Click(object sender, EventArgs e)
        {
            Add_Form_Post add_Form_Post = new Add_Form_Post();
            add_Form_Post.ShowDialog();
        }


        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
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
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells [0].Value);
                    var delQuery = $"delete from Postavshik where ID_postavshika = {id}";

                    var command = new SqlCommand(delQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();

                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows [index].Cells [0].Value.ToString();
                    var nazv = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var litso = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var tel = dataGridView1.Rows[index].Cells[3].Value.ToString(); ;
                    var email = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var adr = dataGridView1.Rows[index].Cells[5].Value.ToString();

                    var changeQuery = $"update Postavshik set Nazvanie_kompanii = '{nazv}', Kontaktnoe_litso = '{litso}', Telefon = '{tel}', Email = '{email}', Adres = '{adr}' where ID_postavshika = '{id}'";

                    var command =  new SqlCommand(changeQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();
                }

            }
            dataBase.closeConnection();            
        }
        private void btn_del_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            update();
        }



        private void Change()
        {
            var SelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;   
            var nazv = textBox_nazv.Text;
            var litso = textBox_litso.Text;
            var tel = textBox_tel.Text;
            var email = textBox_email.Text;
            var adr = textBox_Adr.Text;

            if (dataGridView1.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[SelectedRowIndex].SetValues(id, nazv, litso, tel, email, adr);
                dataGridView1.Rows[SelectedRowIndex].Cells[6].Value = RowState.Modified;
            }
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

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
