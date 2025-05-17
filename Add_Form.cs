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
    public partial class Add_Form_Post : Form
    {
        db dataBase = new db();
        public Add_Form_Post()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var nazv = textBox_nazv.Text;
            var litso = textBox_litso.Text;
            var tel = textBox_tel.Text;
            var email = textBox_email.Text;
            var adr = textBox_adr.Text;

            var addquery = $"insert into Postavshik (Nazvanie_kompanii, Kontaktnoe_litso, Telefon, Email, Adres) values('{nazv}','{litso}','{tel}','{email}','{adr}')";

            var command = new SqlCommand(addquery, dataBase.GetSqlConnection());

            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();
        }

        private void Add_Form_Post_Load(object sender, EventArgs e)
        {

        }
    }
}
