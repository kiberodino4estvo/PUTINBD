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
    public partial class produkt_form : Form
    {
        db dataBase = new db();

        int selectedRow;
        public produkt_form()
        {
            InitializeComponent();
        }
        private void CreateColums()
        {
            dataGridView1.Columns.Add("ID_produkta", "id");
            dataGridView1.Columns.Add("Nazvanie_produkta", "Название продукта");
            dataGridView1.Columns.Add("Tsena", "Цена");
            dataGridView1.Columns.Add("Edinitsa_izmereniya", "Единица измерения");
            dataGridView1.Columns.Add("Data_proizvodstva", "Дата производства");
            dataGridView1.Columns.Add("Srok_godnosti", "Срок годности");
            dataGridView1.Columns.Add("ID_kachestva_produktsii", "id Качества продукцтт");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }
        private void ClearFields()
        {
            textBox_id.Text = "";
            textBox_nazv.Text = "";
            textBox_price.Text = "";
            textBox_edin.Text = "";
            textBox_data.Text = "";
            textBox_srok.Text = "";
            textBox_id_post.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDecimal(2), record.GetString(3), record.GetDateTime(4), record.GetDateTime(5), record.GetInt32(6), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Produkt";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetSqlConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void produkt_form_Load(object sender, EventArgs e)
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
                textBox_price.Text = row.Cells[2].Value.ToString();
                textBox_edin.Text = row.Cells[3].Value.ToString();
                textBox_data.Text = row.Cells[4].Value.ToString();
                textBox_srok.Text = row.Cells[5].Value.ToString();
                textBox_id_post.Text = row.Cells[6].Value.ToString();
            }
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[7].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[7].Value = RowState.Deleted;
        }

        private void update()
        {
            dataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[7].Value;

                if (rowState == RowState.Existed)
                    continue;
                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delQuery = $"delete from Produkt where ID_produkta = {id}";

                    var command = new SqlCommand(delQuery, dataBase.GetSqlConnection());
                    command.ExecuteNonQuery();

                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var nazv = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var price = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var edin = dataGridView1.Rows[index].Cells[3].Value.ToString(); 
                    var data = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var srok = dataGridView1.Rows[index].Cells[5].Value.ToString();
                    var id_post = dataGridView1.Rows[index].Cells[6].Value.ToString();

                    var changeQuery = $"update Produkt set Nazvanie_produkta = '{nazv}', Tsena = '{price}', Edinitsa_izmereniya = '{edin}', Data_proizvodstva = '{data}', Srok_godnosti = '{srok}', ID_kachestva_produktsii = '{id_post}' where ID_produkta = '{id}'";

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
            var nazv = textBox_nazv.Text;
            var price = textBox_price.Text;
            var edin = textBox_edin.Text; 
            var data = textBox_data.Text;
            var srok = textBox_srok.Text;
            var id_post = textBox_id_post.Text;

            if (dataGridView1.Rows[SelectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[SelectedRowIndex].SetValues(id, nazv, price, edin, data, srok, id_post);
                dataGridView1.Rows[SelectedRowIndex].Cells[7].Value = RowState.Modified;
            }
        }

        private void butn_new_Click(object sender, EventArgs e)
        {
            add_form_produkt add_Form = new add_form_produkt();
            add_Form.ShowDialog();
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
    }
}
