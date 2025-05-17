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
    public partial class logi : Form
    {
        db dataBase = new db();
        private DataTable dataTable;
        private string connectionString = @"Data Source=PILOTPC\SQLEXPRESS;Initial Catalog=GLADKOVDB;Integrated Security=True;Encrypt=False";
        public logi()
        {
            InitializeComponent();
        }
        private void CreateColums()
        {
            dataGridView1.Columns.Add("ID", "ID");
            dataGridView1.Columns.Add("Tip_izmeneniya", "Тип изменения");
            dataGridView1.Columns.Add("Tablica", "Таблица");
            dataGridView1.Columns.Add("ID_zapisi", "ID Записи");
            dataGridView1.Columns.Add("Starie_dannye", "Старые данные");
            dataGridView1.Columns.Add("Novye_dannye", "Новые данные");
            dataGridView1.Columns.Add("Data_izmeneniya", "Дата изменения");
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            object idValue = record.GetValue(0);
            object tipIzmeneniyaValue = record.GetValue(1);
            object tablicaValue = record.GetValue(2);
            object idZapisiValue = record.GetValue(3);
            object starieDannieValue = record.GetValue(4);
            object novyeDannieValue = record.GetValue(5);
            object dataIzmeneniyaValue = record.GetValue(6);

            dgw.Rows.Add(
                idValue != DBNull.Value ? (int)idValue : 0, // Или другое значение по умолчанию
                tipIzmeneniyaValue != DBNull.Value ? (string)tipIzmeneniyaValue : string.Empty,
                tablicaValue != DBNull.Value ? (string)tablicaValue : string.Empty,
                idZapisiValue != DBNull.Value ? (int)idZapisiValue : 0,
                starieDannieValue != DBNull.Value ? (string)starieDannieValue : string.Empty,
                novyeDannieValue != DBNull.Value ? (string)novyeDannieValue : string.Empty,
                dataIzmeneniyaValue != DBNull.Value ? (DateTime)dataIzmeneniyaValue : DateTime.MinValue // Или другое значение по умолчанию
            );
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"select * from Izmeneniya";

            SqlCommand command = new SqlCommand(queryString, dataBase.GetSqlConnection());

            dataBase.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void logi_Load(object sender, EventArgs e)
        {
            CreateColums();
            RefreshDataGrid(dataGridView1);
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);            
        }       
    }
}
