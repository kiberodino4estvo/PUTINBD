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
    public partial class add_form_klient : Form
    {
        db dataBase = new db();
        public add_form_klient()
        {
            InitializeComponent();
        }       
        private void button_save_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var name = textBox_name.Text;
            var kont = textBox_kont.Text;
            var adr = textBox_adr.Text;

            var addquery = $"insert into Klient (FIO_ili_nazvanie_kompanii, Kontaktniye_dannyye, Adres) values('{name}','{kont}','{adr}')";

            var command = new SqlCommand(addquery, dataBase.GetSqlConnection());

            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();
        }
    }
}
