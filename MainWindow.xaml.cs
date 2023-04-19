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
using System.Data.SqlClient;
using System.Data;


namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int IDKorisnika = -1;

        public MainWindow()
        {
          InitializeComponent();
        }



        private void btnUlogujSe_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-L9PKUQO;Initial Catalog=AutobuskaStanica;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                String query = "select Putnik.id " +
                    "from Putnik " +
                    "join Korisnik on Korisnik.id=Putnik.id " +
                    "where Korisnik.korisnickoIme =@korisnickoIme " +
                    "and lozinka =@lozinka";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@korisnickoIme ", txtKorisnickoIme.Text);
                sqlCmd.Parameters.AddWithValue("@lozinka", txtLozinka.Password);
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.Read())
                    IDKorisnika = reader.GetInt32(0);                   
  
                if (IDKorisnika!=-1)
                {
                    WindowGlavni objMW = new WindowGlavni(IDKorisnika);
                    Visibility = Visibility.Hidden;
                    objMW.Show();
                }
                else
                {
                    MessageBox.Show("Username or password is incorect!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void BtnNazad_Click (object sender, RoutedEventArgs e)
        {
            MainWindow objStanica = new MainWindow();
            Visibility = Visibility.Hidden;
            objStanica.Show();
        }

        private void BtnRegistracija_Click (object sender, RoutedEventArgs e)
        {
            Registracija registracija = new Registracija();
            Visibility = Visibility.Hidden;
            registracija.Show();
        }

        private void txtLozinka_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
