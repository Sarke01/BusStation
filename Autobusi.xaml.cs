using System;
using System.Collections.Generic;
using System.Data;
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
using System.Configuration;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Autobusi.xaml
    /// </summary>
    public partial class Autobusi : Window
    {


        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-L9PKUQO;Initial Catalog=AutobuskaStanica;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void LoadDataGrid()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Autobus.* FROM Autobus";
            cmd.Connection = sqlCon;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable("Autobusi");
            dataAdapter.Fill(dataTable);
            AutobusiDataGrid.ItemsSource = new DataView(dataTable);
            sqlCon.Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtidAutobusa.Text = dr["idAutobusa"].ToString();
                txtMarka.Text = dr["marka"].ToString();
                txtBrojSedista.Text = dr["brojSedista"].ToString();
            }

        }
        int IDKorisnika;
        public Autobusi(int idKorisnika)
        {
            InitializeComponent();
            LoadDataGrid();
            this.IDKorisnika = idKorisnika;
        }

        

        private void ponistiUnosTxt()
        {
            txtidAutobusa.Text = "";
            txtMarka.Text = "";
            txtBrojSedista.Text = "";
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            sqlCon.Open();
            string query = "INSERT INTO Autobus (marka,brojSedista) VALUES(@marka,@brojSedista)";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@marka", txtMarka.Text);
            sqlCmd.Parameters.AddWithValue("@brojSedista", txtBrojSedista.Text);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno upisani");
                LoadDataGrid();
            }
            ponistiUnosTxt();
        }

        private void BtnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand command = new SqlCommand();
            string query = "UPDATE Autobus SET marka = @marka, brojSedista = @brojSedista WHERE idAutobusa = @idAutobusa";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@marka", txtMarka.Text);
            sqlCmd.Parameters.AddWithValue("brojSedista", txtBrojSedista.Text);
            sqlCmd.Parameters.AddWithValue("@idAutobusa", txtidAutobusa.Text);


            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno promenjeni");
                LoadDataGrid();
            }
            ponistiUnosTxt();
        }

        private void BtnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            string query = "DELETE FROM Autobus WHERE idAutobusa=@idAutobusa";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@idAutobusa", txtidAutobusa.Text);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno obrisani");
                LoadDataGrid();
            }
            ponistiUnosTxt();
        }

        private void txtIdStanice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtAdresa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Mesta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void txtStanice_Click(object sender, RoutedEventArgs e)
        {
            Stanica objMW = new Stanica(IDKorisnika);
            Visibility = Visibility.Hidden;
            objMW.Show();
        }

        private void txtVozaci_Click(object sender, RoutedEventArgs e)
        {
            Vozaci objMW = new Vozaci(IDKorisnika);
            Visibility = Visibility.Hidden;
            objMW.Show();
        }

        private void txtKarte_Click(object sender, RoutedEventArgs e)
        {
            WindowGlavni objMW = new WindowGlavni(IDKorisnika);
            Visibility = Visibility.Hidden;
            objMW.Show();
        }

        private void txtAutobusi_Click(object sender, RoutedEventArgs e)
        {
            Autobusi objMW = new Autobusi(IDKorisnika);
            Visibility = Visibility.Hidden;
            objMW.Show();
        }

        private void txtLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMW = new MainWindow();
            Visibility = Visibility.Hidden;
            objMW.Show();
        }
    }
}
