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
using System.IO;
using System.IO.Compression;
using DGII_contribuyentes.model;
namespace DGII_contribuyentes
{
    public partial class Form1 : Form
    {
        string[] listado_splitted;
        public Form1()
        {
            InitializeComponent();
        }

        #region eventos
        private void Form1_Load(object sender, EventArgs e)
        {
            load_grid();
        }
        private void btn_sicnronizar_Click(object sender, EventArgs e)
        {
            string url = Settings1.Default.url;
            /*Aqui utilizamos el metodo de descarga y le pasamos el url de donde vamos a obtener la informacion
             y despues le pasamos el directorio de donde vamos a poner el archivo a descargar + el nombre que
             va a tener el archibo*/
            descargar(url, Directory.GetCurrentDirectory() + "/tmp.zip");
            load_grid();

        }
#endregion
        #region metodos

        private void descargar(string url, string destino)
        {

            try
            {
                if (hay_internet())
                {
                    /*instanciamos al webclient para de esta manera utilizar el metodo por defecto
                     para poder hacer la descarga pasandole el destino tambien*/
                    WebClient cliente = new WebClient();
                    cliente.DownloadFile(url, destino);
                    descomprimir();
                    leer_datos();
                    
                    
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
            //Esto lo utilizamos para verificar si el ordenador tiene conexion a internet
            bool internet = NetworkInterface.GetIsNetworkAvailable();


            return internet;
        }

        private void descomprimir()
        {

            string _direccion = @"C:\Users\enman\OneDrive\Escritorio\C#\DGII_contribuyentes\DGII_contribuyentes\bin\Debug\TMP";
            if (Directory.Exists(_direccion))
            {
                
            }
            else
            {
                /*Utilizamos esta propiedad para poder descomprimir el archivo descargado, primero le pasamos la ubicacion del 
                 archivo y despues la ubicacion de donde lo queremos descompimir*/
                ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + "/tmp.zip", Directory.GetCurrentDirectory());
            }
            
            

        }

        private string[] leer_datos()
        {
         /*Aqui lo que hice fue que lei el texto con toda la informacion y lo puse en un array*/   
            string[] listado= File.ReadAllLines(Directory.GetCurrentDirectory() + "\\TMP\\DGII_RNC.txt");

            foreach (var item in listado)
            {/*Cuando el array entra aqui lo que hago es que lo divido segun las lineas | que tenga
                el documento porque asi se separa la informacion*/
                int contador = 0;
                listado_splitted = item.Split('|');

                foreach (var item2 in listado_splitted)
                {/*Como el array quedaba con muchos espacion en blanco lo que hice fue que lo meti dentro de
                    otor foreach para con esta condicion poner posicionar todos los campos que vamos a utilizar
                    en las 6 o 7 primeras filas segun correspondan*/
                    if (!string.IsNullOrWhiteSpace(item2))
                    {
                        listado_splitted[contador] = item2;
                        contador++;
                    }
                    
                }
                agregar_datos(listado_splitted);

            }
               
            
            return listado_splitted;
            
        }

        private void agregar_datos(string[] datos)
        {/*Algunas de nuestras empresas tienen 7 campos y otras 6 por es*/
            using (DGII_entities database = new DGII_entities())
            {
                dato clientes = new dato();
                if (!string.IsNullOrWhiteSpace(listado_splitted[6]))
                {
                    clientes.RNC = listado_splitted[0];
                    clientes.empresa = listado_splitted[1];
                    clientes.empresa2 = listado_splitted[2];
                    clientes.producto = listado_splitted[3];
                    clientes.fecha_fundacion = listado_splitted[4];
                    clientes.estado = listado_splitted[5];
                    clientes.situacion = listado_splitted[6];
                    database.datos.Add(clientes);
                    database.SaveChanges();

                }
                else
                {
                    clientes.RNC = listado_splitted[0];
                    clientes.empresa = listado_splitted[1];
                    clientes.empresa2 = "";
                    clientes.producto = listado_splitted[2];
                    clientes.fecha_fundacion = listado_splitted[3];
                    clientes.estado = listado_splitted[4];
                    clientes.situacion = listado_splitted[5];
                    database.datos.Add(clientes);
                    database.SaveChanges();
                }
            }
        }

        private void load_grid()
        {
            using (DGII_entities database = new DGII_entities())
            {
                var listado = from b in database.datos
                              select b;
                dataGridView1.DataSource = listado.ToList();
            }
        }
        #endregion


    }
}
