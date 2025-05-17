using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUTINBD
{
    public partial class prpost : Form
    {
        private string connectionString = @"Data Source=PILOTPC\SQLEXPRESS;Initial Catalog=DBPUTIN;Integrated Security=True;Encrypt=False";
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        private string currentQuery;
        db dataBase = new db();
        public prpost()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData("select * from Produkt_Post");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData("select * from Produkt_Zakaz");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (adapter == null || dataTable == null)
            {
                MessageBox.Show("Нет данных для сохранения. Пожалуйста, загрузите данные сначала.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    adapter.SelectCommand = new SqlCommand(currentQuery, connection);
                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                    adapter.Update(dataTable);
                    MessageBox.Show("Таблица сохранена!");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Ошибка SQL: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
        private void LoadData(string query)
        {
            try
            {
                // Создаем соединение с базой данных
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(query, connection);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    currentQuery = query;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
