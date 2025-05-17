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

    public partial class zaprosi : Form
    {
        private string connectionString = "Data Source=PILOTPC\\SQLEXPRESS;Initial Catalog=PUTINDB;Integrated Security=True;Encrypt=False";
        public zaprosi()
        {
            InitializeComponent();
            LoadQueries();
        }
        private void LoadQueries()
        {
            // Добавляем запросы в ComboBox
            comboBoxQueries.Items.Add(new QueryItem("Получить заказы с их статусами:", "SELECT ID_zakaza, Status_zakaza FROM Zakaz;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить список всех заказов с именами клиентов:", "SELECT Z.ID_zakaza, K.FIO_ili_nazvanie_kompanii, Z.Data_zakaza, Z.Summa_zakaza\r\n   FROM Zakaz Z\r\n   JOIN Klient K ON Z.ID_kliyenta = K.ID_kliyenta;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить список всех продуктов с именем поставщика:", "SELECT P.Nazvanie_produkta, PS.Nazvanie_kompanii\r\n   FROM Produkt P\r\n   JOIN Postavshik PS ON P.ID_postavshika = PS.ID_postavshika;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить все заказы с информацией о продуктах и поставщиках:", "SELECT Z.ID_zakaza, P.Nazvanie_produkta, PS.Nazvanie_kompanii, Z.Data_zakaza, Z.Summa_zakaza\r\n   FROM Zakaz Z\r\n   JOIN Produkt P ON Z.ID_produkta = P.ID_produkta\r\n   JOIN Postavshik PS ON P.ID_postavshika = PS.ID_postavshika;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить качество продукции с информацией о продукте:", "SELECT KP.ID_kachestva_produktsii, P.Nazvanie_produkta, KP.Standart_kachestva, KP.Data_proverki\r\n   FROM Kachestvo_produktsii KP\r\n   JOIN Produkt P ON KP.ID_produkta = P.ID_produkta;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить список всех заказов с информацией о клиентах и статусах:", "SELECT Z.ID_zakaza, K.FIO_ili_nazvanie_kompanii, Z.Status_zakaza, Z.Summa_zakaza\r\n   FROM Zakaz Z\r\n   JOIN Klient K ON Z.ID_kliyenta = K.ID_kliyenta;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить количество заказов и общую сумму по каждому клиенту:", "SELECT K.FIO_ili_nazvanie_kompanii, COUNT(Z.ID_zakaza) AS Kolichestvo_zakazov, SUM(Z.Summa_zakaza) AS Obshchaya_summa\r\n   FROM Klient K\r\n   LEFT JOIN Zakaz Z ON K.ID_kliyenta = Z.ID_kliyenta\r\n   GROUP BY K.FIO_ili_nazvanie_kompanii;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить список продуктов, которые не были заказаны ни разу:", "SELECT P.Nazvanie_produkta\r\n   FROM Produkt P\r\n   LEFT JOIN Zakaz Z ON P.ID_produkta = Z.ID_produkta\r\n   WHERE Z.ID_zakaza IS NULL;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить среднюю сумму заказов по статусу заказа:", "SELECT Z.Status_zakaza, AVG(Z.Summa_zakaza) AS Srednyaya_summa\r\n   FROM Zakaz Z\r\n   GROUP BY Z.Status_zakaza;"));
            comboBoxQueries.Items.Add(new QueryItem("Получить клиентов, которые сделали заказы на сумму больше средней суммы заказа всех клиентов:", "SELECT K.FIO_ili_nazvanie_kompanii\r\n   FROM Klient K\r\n   JOIN Zakaz Z ON K.ID_kliyenta = Z.ID_kliyenta\r\n   GROUP BY K.FIO_ili_nazvanie_kompanii\r\n   HAVING SUM(Z.Summa_zakaza) > (\r\n       SELECT AVG(Summa_zakaza)\r\n       FROM Zakaz\r\n   );"));
            // Добавьте свои запросы здесь
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedItem = comboBoxQueries.SelectedItem as QueryItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запрос из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ExecuteQuery(selectedItem.Query);
        }
        private void ExecuteQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridViewResults.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string customQuery = textBoxCustomQuery.Text.Trim();
            if (string.IsNullOrEmpty(customQuery))
            {
                MessageBox.Show("Пожалуйста, введите свой SQL-запрос.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;               
            }

            ExecuteQuery(customQuery);
            textBoxCustomQuery.Text = "";
        }
        public class QueryItem
        {
            public string DisplayName { get; set; }
            public string Query { get; set; }

            public QueryItem(string displayName, string query)
            {
                DisplayName = displayName;
                Query = query;
            }

            public override string ToString()
            {
                return DisplayName; // Это то, что будет отображаться в ComboBox
            }
        }


    }
}
