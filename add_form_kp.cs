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
    public partial class add_form_kp : Form
    {
        db dataBase = new db();
        public add_form_kp()
        {
            InitializeComponent();
        }

        private void textBox_data_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var stan = textBox_stan.Text;

            var addquery = $"insert into Kachestvo_produktsii (Standart_kachestva) values('{stan}')";

            var command = new SqlCommand(addquery, dataBase.GetSqlConnection());

            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();
        }
    }
}
