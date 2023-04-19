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
    /// Interaction logic for Registracija.xaml
    /// </summary>
    public partial class Registracija : Window
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-L9PKUQO;Initial Catalog=AutobuskaStanica;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        List<string> MestaComboBox = new List<string>();
        public Registracija()
        {
            InitializeComponent();
            ComboBoxMesta();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objMW = new MainWindow();
            Visibility = Visibility.Hidden;
            objMW.Show();
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
            cmbMesta.ItemsSource = MestaComboBox;

        }

            
        private void ponistiUnosTxt()
        {
            txtKorisnickoIme.Text = "";
            txtLozinka.Text = "";
            txtEmail.Text = "";
            txtBrojTelefona.Text = "";
            txtDrugoMesto.Text = "";
            cmbMesta.SelectedItem = null;

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ponistiUnosTxt();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            string query = "INSERT INTO Korisnik (korisnickoIme,lozinka) VALUES(@korisnickoIme, @lozinka) ";
            string query2 = "SELECT COUNT(*) FROM Korisnik where korisnickoIme=@korisnickoIme";
            string query3 = "SELECT id FROM Korisnik where korisnickoIme=@korisnickoIme ";
            string query4 = "INSERT INTO Putnik(id,ime,brojTelefona,email) VALUES (@idKorisnikPutnik,@korisnickoIme,@brojTelefona,@email)";

            SqlCommand sqlCmd2 = new SqlCommand(query2, sqlCon);
            sqlCmd2.CommandType = CommandType.Text;
            sqlCmd2.Parameters.AddWithValue("@korisnickoIme", txtKorisnickoIme.Text);
            int provera2 = (int)sqlCmd2.ExecuteScalar();
           
            if (provera2 == 1)
            {
                MessageBox.Show("Korisnicko ime je zauzeto");
            }
            else
            {
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@korisnickoIme", txtKorisnickoIme.Text);
                sqlCmd.Parameters.AddWithValue("@lozinka", txtLozinka.Text);
                int provera = sqlCmd.ExecuteNonQuery();
                //if (provera == 1)
                //{
                //    MessageBox.Show("Uspesno ste registrovani");
                //}
            }

            SqlCommand sqlCmd3 = new SqlCommand(query3, sqlCon);
            sqlCmd3.CommandType = CommandType.Text;
            sqlCmd3.Parameters.AddWithValue("@korisnickoIme", txtKorisnickoIme.Text);
            int idKorisnikPutnik = (int)sqlCmd3.ExecuteScalar();

            SqlCommand sqlCmd4 = new SqlCommand(query4, sqlCon);
            sqlCmd4.CommandType = CommandType.Text;
            sqlCmd4.Parameters.AddWithValue("@idKorisnikPutnik", idKorisnikPutnik);
            sqlCmd4.Parameters.AddWithValue("@korisnickoIme", txtKorisnickoIme.Text);
            sqlCmd4.Parameters.AddWithValue("@brojTelefona", txtBrojTelefona.Text);
            sqlCmd4.Parameters.AddWithValue("@email", txtEmail.Text);
            int provera4 = sqlCmd4.ExecuteNonQuery();
            if (provera4 == 1)
            {
                MessageBox.Show("Uspesno ste registrovani");
            }

            ponistiUnosTxt();

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            string query = "INSERT INTO Mesto (naziv) VALUES(@naziv) ";

            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@naziv", txtDrugoMesto.Text);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Uspesno ste dodali mesto");
                Registracija objMW = new Registracija();
                Visibility = Visibility.Hidden;
                objMW.Show();
            }
            ComboBoxMesta();
        }

    }
 
 }

