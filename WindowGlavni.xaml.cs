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
using System.Data;
using System.Data.SqlClient;

namespace AutobuskaStanica
{
    /// <summary>
    /// Interaction logic for WindowGlavni.xaml
    /// </summary>
    public partial class WindowGlavni : Window
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-L9PKUQO;Initial Catalog=AutobuskaStanica;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        int IDKorisnika;
        List<string> vremePolaska = new List<string>();
        List<string> MestaComboBox = new List<string>();
        public WindowGlavni(int idKorisnika)
        {
            InitializeComponent();
            ComboBoxMesta();
            popuniComboBox();
            LoadDataGrid();
            this.IDKorisnika = idKorisnika;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                cmbMestoPolaska.SelectedItem = dr["mestoPolaska"].ToString();
                cmbDestinacija.Text = dr["naziv"].ToString();
                txtbrojKarte.Text=dr["brojKarte"].ToString();
                string[] vreme=dr["vremePolaska"].ToString().Split(" ");
                cmbVreme.SelectedItem = vreme[1];
                datum.Text = vreme[0];

            }

        }
        private void LoadDataGrid()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select k.cena,k.brojKarte,p.ime,k.idAutobusa,mp.naziv as mestoPolaska,md.naziv,k.vremePolaska " +
                "from Karta k " +
                "join putnik p on p.id = K.idPutnika " +
                "join mesto mp on mp.id = k.MestoPolaska " +
                "join mesto md on md.id = k.MestoDolaska ";

            cmd.Connection = sqlCon;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable("Karta");
            dataAdapter.Fill(dataTable);
            KarteGrid.ItemsSource = new DataView(dataTable);
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
            cmbMestoPolaska.ItemsSource = MestaComboBox;
            cmbDestinacija.ItemsSource = MestaComboBox;


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
            datum.Text ="";
            cmbVreme.SelectedIndex = 0;
        }

        private int vratiIdMesta(string odabir)
        {
            string query = "select mesto.id from mesto where mesto.naziv =@mesto ";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            if(odabir=="polazak")
            sqlCmd.Parameters.AddWithValue("@mesto", cmbMestoPolaska.SelectedItem);
            else
                sqlCmd.Parameters.AddWithValue("@mesto", cmbDestinacija.SelectedItem);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            int idMesta= -1;
            if (reader.Read())
                idMesta = reader.GetInt32(0);
            reader.Close();

            return idMesta;
        }

        private string formatiranjeDatuma()
        {
            string[] datumPolaska = datum.Text.Split("/");
            string vremePolaska = (string)cmbVreme.SelectedItem;
            string dan = datumPolaska[1];
            string mesec = datumPolaska[0];
            string godina = datumPolaska[2];
            string terminPolaska = godina + "-" + mesec + "-" + dan + " " + vremePolaska;

            return terminPolaska;
        }

        private void popuniComboBox()
        {
            for (int i = 6; i < 22; i++)
                for (int k = 0, j = 0; j < 2; j++, k += 30)
                {
                    if(k==0)                      
                    vremePolaska.Add(i.ToString() + ":" + k.ToString() + "0" + ":" + "00");
                    else
                        vremePolaska.Add(i.ToString() + ":" + k.ToString() + ":" + "00");

                }
            cmbVreme.ItemsSource = vremePolaska;
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            string datum=formatiranjeDatuma();
            if (sqlCon.State == ConnectionState.Closed) 
                sqlCon.Open();
            string queryZaAutobus = "select top 1 idAutobusa " +
                                    "from Autobus " +
                                    "order by NEWID() ";
            SqlCommand sqlCmd2 = new SqlCommand(queryZaAutobus, sqlCon);
            SqlDataReader reader = sqlCmd2.ExecuteReader();
            int idAutobusa=-1;
            if (reader.Read())
                idAutobusa = reader.GetInt32(0);
            reader.Close();
            Random random = new Random();
            float broj = random.Next(3,7);
            float cena=(float)broj*100;
            string query = "INSERT INTO Karta (cena,idPutnika,idAutobusa,mestoPolaska,mestoDolaska,vremePolaska) VALUES(@cena, @idPutnika, @idAutobusa,@mestoPolaska,@mestoDolaska,@vremePolaska)";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@cena",cena);
            sqlCmd.Parameters.AddWithValue("@idPutnika",IDKorisnika);
            sqlCmd.Parameters.AddWithValue("@mestoPolaska",vratiIdMesta("polazak"));
            sqlCmd.Parameters.AddWithValue("@mestoDolaska", vratiIdMesta("dolazak"));
            sqlCmd.Parameters.AddWithValue("@vremePolaska", datum);
            sqlCmd.Parameters.AddWithValue("@idAutobusa",idAutobusa);
            int provera = sqlCmd.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno upisani");
                LoadDataGrid();
            }

        }

        private void BtnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            string datum = formatiranjeDatuma();
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand command = new SqlCommand();
            string query = "UPDATE Karta SET mestoPolaska = @mestoPolaska, mestoDolaska = @mestoDolaska,vremePolaska=@vremePolaska WHERE brojKarte = @brojKarte";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@mestoPolaska", vratiIdMesta("polazak"));
            sqlCmd.Parameters.AddWithValue("mestoDolaska", vratiIdMesta("dolazak"));
            sqlCmd.Parameters.AddWithValue("@vremePolaska", datum);
            sqlCmd.Parameters.AddWithValue("@brojKarte",txtbrojKarte.Text);

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
            string query = "DELETE FROM Karta WHERE brojKarte=@brojKarte";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@brojKarte",txtbrojKarte.Text);
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

        private void cmbVreme_SelectionChanged()
        {

        }
    }
}
