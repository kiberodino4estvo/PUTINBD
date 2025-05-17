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
    public partial class add_form_zakaz : Form
    {
        db dataBase = new db();
        public add_form_zakaz()
        {
            InitializeComponent();
        }

        private void add_form_zakaz_Load(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var id_p = textBox_id_p.Text;
            var data = textBox_data.Text;
            var id_k = textBox_id_k.Text;
            var status = textBox_status.Text;
            var sum = textBox_sum.Text;

            var addquery = $"insert into Zakaz (ID_produkta, Data_zakaza, ID_kliyenta, Status_zakaza, Summa_zakaza) values('{id_p}','{data}','{id_k}','{status}','{sum}')";

            var command = new SqlCommand(addquery, dataBase.GetSqlConnection());

            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();
        }
    }
}
