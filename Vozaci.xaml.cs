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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for Vozaci.xaml
    /// </summary>
    public partial class Vozaci : Window
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-L9PKUQO;Initial Catalog=AutobuskaStanica;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        int IDKorisnika;
        public Vozaci(int idKorisnika)
        {
            InitializeComponent();
            LoadDataGrid();
            this.IDKorisnika = idKorisnika;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtIdAutobusa.Text = dr["idAutobusa"].ToString();
                txtImePrezime.Text = dr["ime"].ToString();
                txtTelefon.Text = dr["brojTelefona"].ToString();
                txtAdresa.Text = dr["adresa"].ToString();
                txtidVozaca.Text = dr["id"].ToString();
            }

        }
        private void LoadDataGrid()
        {
            if (sqlCon.State == ConnectionState.Closed)
              sqlCon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Vozac.* FROM Vozac ";
            cmd.Connection = sqlCon;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable("Vozac");
            dataAdapter.Fill(dataTable);
            VozacDataGrid.ItemsSource = new DataView(dataTable);
            sqlCon.Close();
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

 

        private void BtnIzmeni_Click(object sender, RoutedEventArgs e)
        {

            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand command = new SqlCommand();
            string query = "UPDATE Vozac SET ime = @Ime, brojTelefona = @brojTelefona, adresa = @adresa, idAutobusa=@idAutobusa WHERE id = @id";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@ime", txtImePrezime.Text);
            sqlCmd.Parameters.AddWithValue("brojTelefona", txtTelefon.Text);
            sqlCmd.Parameters.AddWithValue("@adresa", txtAdresa.Text);
            sqlCmd.Parameters.AddWithValue("@idAutobusa", txtIdAutobusa.Text);
            sqlCmd.Parameters.AddWithValue("@id", txtidVozaca.Text);


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
            string query = "DELETE FROM Vozac WHERE id=@id";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@id", txtidVozaca.Text);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno obrisani");
                LoadDataGrid();
            }
            ponistiUnosTxt();
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void txtLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMW = new MainWindow();
            Visibility = Visibility.Hidden;
            objMW.Show();
        }

        private void ponistiUnosTxt()
        {
            txtIdAutobusa.Text = "";
            txtImePrezime.Text = "";
            txtTelefon.Text = "";
            txtAdresa.Text = "";
            txtidVozaca.Text = "";
        }

        private void BtnDodaj_Click (object sender, RoutedEventArgs e)
        {
        {
            sqlCon.Open();
            string query = "INSERT INTO Vozac (ime,brojTelefona, adresa,idAutobusa) VALUES(@ime, @brojTelefona, @adresa, @idAutobusa)";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@brojTelefona", txtTelefon.Text);
            sqlCmd.Parameters.AddWithValue("@adresa", txtAdresa.Text);
            sqlCmd.Parameters.AddWithValue("@ime", txtImePrezime.Text);
            sqlCmd.Parameters.AddWithValue("@idAutobusa", txtIdAutobusa.Text);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno upisani");
                LoadDataGrid();
            }
            ponistiUnosTxt();
        }
        }
    }
}
