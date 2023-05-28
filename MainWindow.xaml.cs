using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace EurovGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=eurovizio;";
        private void LoadData()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                
                    connection.Open();

                    string query = "SELECT ev, eloado, cim, helyezes, pontszam FROM dal";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGrid.ItemsSource = dataTable.DefaultView;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt az adatok betöltése közben: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT Count(eloado) as \"Résztvevők\" from dal where orszag = \"Magyarország\"";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
            int count = 0;
            count = Convert.ToInt32(command.ExecuteScalar());
            MessageBox.Show($"{count}");
        }

        private void Fel4_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryText = "SELECT COUNT(*) FROM dal WHERE orszag = \"Magyarorszag\"";

                using (MySqlCommand command = new MySqlCommand(queryText, connection))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

                connection.Close();
            }
            int minHely = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryText = "SELECT Min(helyezes) FROM dal WHERE orszag = \"Magyarorszag\"";

                using (MySqlCommand command = new MySqlCommand(queryText, connection))
                {
                    minHely = Convert.ToInt32(command.ExecuteScalar());
                }

                connection.Close();
            }
            MessageBox.Show($"Magyar versenyzők száma:{count} \n A legjobb helyezés:{minHely}");
        }

        private void Fel5_Click(object sender, RoutedEventArgs e)
        {
            double nemetAtl = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryText = "SELECT AVG(pontszam) FROM dal WHERE orszag = \"Nemetorszag\"";

                using (MySqlCommand command = new MySqlCommand(queryText, connection))
                {
                    nemetAtl = Convert.ToInt32(command.ExecuteScalar());
                }

                connection.Close();
            }
            MessageBox.Show($"Németország átlagos pontszáma:{String.Format("{0:0.00}",nemetAtl)}");
        }

        private void Fel6_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryText = "SELECT eloado, cim from dal where cim like '%luck%'; ";

                using (MySqlCommand command = new MySqlCommand(queryText, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string eloado = reader["eloado"].ToString();
                            string cim = reader["cim"].ToString();

                            MessageBox.Show($"{eloado} - {cim}");
                        }
                    }
                }

                connection.Close();
            }
            
        }

        private void Fel7_Click(object sender, RoutedEventArgs e)
        {
            lbKiir.Items.Clear();
            string keresett = txtEloadoNev.Text;
            List<string> dalok = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryText = $"SELECT eloado, cim FROM dal WHERE eloado LIKE '%{keresett}%' ORDER BY eloado, cim";

                using (MySqlCommand command = new MySqlCommand(queryText, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Eloado = reader["eloado"].ToString();
                            string Cim = reader["cim"].ToString();

                            string tdalok = $"{Eloado} - {Cim}";
                            lbKiir.Items.Add(tdalok);
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
