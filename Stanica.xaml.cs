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
    /// Interaction logic for Stanica.xaml
    /// </summary>
    public partial class Stanica : Window
    {

        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-L9PKUQO;Initial Catalog=AutobuskaStanica;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        List<string> MestaComboBox=new List<string>();
        int IDKorisnika;
        public Stanica(int idKorisnika)
        {

            InitializeComponent();
            ComboBoxMesta();
            DataContext = this;
            LoadDataGrid();
            this.IDKorisnika = idKorisnika;

        }
        private void LoadDataGrid()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Stanica.*,Mesto.naziv FROM Stanica,Mesto where Stanica.idMesta=Mesto.id";
            cmd.Connection = sqlCon;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable("Stanica");
            dataAdapter.Fill(dataTable);
            StanicaDataGrid.ItemsSource = new DataView(dataTable);
            sqlCon.Close();
        }


        private void ComboBoxMesta()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            string query = "select naziv from mesto";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            SqlDataReader reader = sqlCmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                MestaComboBox.Add((string)reader.GetValue(i));
                
            }
            reader.Close();
            Mesta.ItemsSource = MestaComboBox;

        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtIdStanice2.Text = dr["idStanice"].ToString();
                txtAdresa.Text = dr["adresa"].ToString();
                Mesta.SelectedItem=dr["naziv"].ToString();
            }

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

        private void ponistiUnosTxt()
        {
            txtAdresa.Text = "";
            Mesta.SelectedIndex= 1;
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            string query2 = "select mesto.id from mesto where naziv =@Grad";
            SqlCommand sqlCmd2 = new SqlCommand(query2, sqlCon);
            sqlCmd2.CommandType = CommandType.Text;
            sqlCmd2.Parameters.AddWithValue("@Grad", (string)Mesta.SelectedItem);
            SqlDataReader sqlDataReader = sqlCmd2.ExecuteReader();
            int grad=-1;
            if (sqlDataReader.Read())
               grad=(int)sqlDataReader.GetValue(0);
            sqlDataReader.Close();
            string query = "INSERT INTO Stanica (adresa,idMesta) VALUES(@adresa, @idMesta)";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@adresa", txtAdresa.Text);
            sqlCmd.Parameters.AddWithValue("@idMesta",grad);
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
            string query2 = "select mesto.id from mesto where naziv =@Grad";
            SqlCommand sqlCmd2 = new SqlCommand(query2, sqlCon);
            sqlCmd2.CommandType = CommandType.Text;
            sqlCmd2.Parameters.AddWithValue("@Grad", (string)Mesta.SelectedItem);
            SqlDataReader sqlDataReader = sqlCmd2.ExecuteReader();
            int grad = -1;
            if (sqlDataReader.Read())
                grad = (int)sqlDataReader.GetValue(0);
            sqlDataReader.Close();
            SqlCommand command = new SqlCommand();
            string query = "UPDATE Stanica SET idMesta = @IdMesta, adresa = @adresa WHERE idStanice = @idStanice";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("idMesta", Mesta.SelectedIndex + 1);
            sqlCmd.Parameters.AddWithValue("@adresa", txtAdresa.Text);
            sqlCmd.Parameters.AddWithValue("@idStanice", txtIdStanice2.Text);


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
            string query = "DELETE FROM Stanica WHERE idStanice=@idStanice";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@idStanice", txtIdStanice2.Text);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno obrisani");
                LoadDataGrid();
            }
            ponistiUnosTxt();
        }

        private void txtLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMW = new MainWindow();
            Visibility = Visibility.Hidden;
            objMW.Show();
        }

        private void txtIdStanice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtAdresa_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}
