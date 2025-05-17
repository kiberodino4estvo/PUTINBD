using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUTINBD
{
    public partial class MENU : Form
    {
        public MENU()
        {
            InitializeComponent();
        }

        private void btn_post_form_Click(object sender, EventArgs e)
        {
            post_form frm = new post_form();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btn_klient_form_Click(object sender, EventArgs e)
        {
            klient_form frm = new klient_form();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btn_produkt_form_Click(object sender, EventArgs e)
        {
            produkt_form frm = new produkt_form();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btn_zakaz_form_Click(object sender, EventArgs e)
        {
            zakaz_form frm = new zakaz_form();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button_form_kp_Click(object sender, EventArgs e)
        {
            kp_form frm = new kp_form();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button_logi_Click(object sender, EventArgs e)
        {
           logi frm = new logi();
            this.Hide();
            frm.ShowDialog();
            this.Show();

        }

        private void btn_zaprosi_Click(object sender, EventArgs e)
        {
            zaprosi frm = new zaprosi();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btm_prpost_Click(object sender, EventArgs e)
        {
            prpost frm = new prpost();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void btn_przak_Click(object sender, EventArgs e)
        {
            przak frm = new przak();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }
    }
}
