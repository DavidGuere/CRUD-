using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionPedidos
{
    /// <summary>
    /// Interaction logic for UpdateClient.xaml
    /// </summary>
    public partial class UpdateClient : Window
    {
        private int clientID;

        SqlConnection openConection;
        public UpdateClient(int arg)
        {
            clientID = arg;

            string myConection = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            openConection = new SqlConnection(myConection);

            InitializeComponent();
        }

        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ask = "update [Client] set name=@new_name where id=" + clientID;

                SqlCommand SQLcommand = new SqlCommand(ask, openConection);

                openConection.Open();

                SQLcommand.Parameters.AddWithValue("@new_name", clientToUpdate.Text);

                SQLcommand.ExecuteNonQuery();

                openConection.Close();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
