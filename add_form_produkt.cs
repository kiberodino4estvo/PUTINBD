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
    public partial class add_form_produkt : Form
    {
        db dataBase = new db();
        public add_form_produkt()
        {
            InitializeComponent();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();
            
            var nazv = textBox_nazv.Text;
            var price = textBox_price.Text;
            var edin = textBox_edin.Text;
            var data = textBox_data.Text;
            var srok = textBox_srok.Text;
            var id_post = textBox_id_post.Text;

            var addquery = $"insert into Produkt (Nazvanie_produkta, Tsena, Edinitsa_izmereniya, Data_proizvodstva, Srok_godnosti, ID_kachestva_produktsii) values('{nazv}','{price}','{edin}','{data}', '{srok}', '{id_post}')";

            var command = new SqlCommand(addquery, dataBase.GetSqlConnection());

            command.ExecuteNonQuery();

            MessageBox.Show("Запись успешно создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataBase.closeConnection();
        }
    }
}
