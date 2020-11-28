using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

namespace DGII_contribuyentes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region eventos
        private void descargar(string url, string destino)
        {

            try
            {
                if (hay_internet())
                {
                    WebClient cliente = new WebClient();
                    cliente.DownloadFile(url, destino);
                }
                else
                {
                    MessageBox.Show("No hay internet");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private bool hay_internet()
        {
            bool internet = NetworkInterface.GetIsNetworkAvailable();


            return internet;
        }
        #endregion

    }
}
