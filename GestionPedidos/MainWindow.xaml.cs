using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace GestionPedidos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection mySQLConection;

        // Data extraction from database
        private void ShowClients()
        {
            try
            {
                string ask = "select * from Client";

                // run "ask" on database
                SqlDataAdapter AdaptTableToWindow = new SqlDataAdapter(ask, mySQLConection);

                // Storing data from server
                using (AdaptTableToWindow)
                {
                    DataTable ClientTable = new DataTable();

                    // fill data to ClientTable
                    AdaptTableToWindow.Fill(ClientTable);

                    // Selection table column to display
                    ClientList.DisplayMemberPath = "name";
                    // Selecting key
                    ClientList.SelectedValuePath = "id";
                    // Data source
                    ClientList.ItemsSource = ClientTable.DefaultView;
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowOrder()
        {
            try
            {
                string ask = "select * from [Order] O inner join Client C on C.id = O.cClient where C.id = @ClientID";

                SqlCommand SQLCommand = new SqlCommand(ask, mySQLConection);

                SqlDataAdapter AdaptTableToWindow = new SqlDataAdapter(SQLCommand);

                // Storing data from server
                using (AdaptTableToWindow)
                {

                    SQLCommand.Parameters.AddWithValue("@ClientID", ClientList.SelectedValue);
                    DataTable OrderTable = new DataTable();

                    // fill data to OrderTable
                    AdaptTableToWindow.Fill(OrderTable);

                    // Selection table column to display
                    OrderList.DisplayMemberPath = "DateOrder";
                    // Selecting key
                    OrderList.SelectedValuePath = "id";
                    // Data source
                    OrderList.ItemsSource = OrderTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowAllOrders()
        {
            try
            {
                string ask = "select *, concat(cClient, '    ', DateOrder, '    ', PayMEthod) as cocatColumn from [Order]";

                // run "ask" on database
                SqlDataAdapter AdaptTableToWindow = new SqlDataAdapter(ask, mySQLConection);

                // Storing data from server
                using (AdaptTableToWindow)
                {
                    DataTable AllOrdersTable = new DataTable();

                    // fill data to ClientTable
                    AdaptTableToWindow.Fill(AllOrdersTable);

                    // Selection table column to display
                    AllOrders.DisplayMemberPath = "cocatColumn";
                    // Selecting key
                    AllOrders.SelectedValuePath = "id";
                    // Data source
                    AllOrders.ItemsSource = AllOrdersTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            string myConection = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            // Establishing connection with database
            mySQLConection = new SqlConnection(myConection);

            ShowClients();

            ShowAllOrders();

        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowOrder();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ask = "delete from [Order] where id=@orderID";

                SqlCommand SQLcommand = new SqlCommand(ask, mySQLConection);

                mySQLConection.Open();

                SQLcommand.Parameters.AddWithValue("@orderID", AllOrders.SelectedValue);

                SQLcommand.ExecuteNonQuery();

                mySQLConection.Close();

                ShowAllOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string ask = "delete from [Client] where values id =@clientID";

                SqlCommand SQLcommand = new SqlCommand(ask, mySQLConection);

                mySQLConection.Open();

                SQLcommand.Parameters.AddWithValue("@clientID", OrderList.SelectedValue);

                SQLcommand.ExecuteNonQuery();

                mySQLConection.Close();

                ShowClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //add
            try
            {
                string ask = "insert into [Client] (name) values (@newClient)";

                SqlCommand SQLcommand = new SqlCommand(ask, mySQLConection);

                mySQLConection.Open();

                SQLcommand.Parameters.AddWithValue("@newClient", NewClient.Text);

                SQLcommand.ExecuteNonQuery();

                mySQLConection.Close();

                ShowClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
