using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GUIClient
{
    public partial class MainWindow : Window
    {
        private RestClient client;

        public MainWindow()
        {
            InitializeComponent();
            client = new RestClient("http://localhost:5001/api/business");
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(IndexNum.Text);
            var request = new RestRequest($"values/{index}", Method.Get);
            var response = client.Execute(request);

            var dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(response.Content);

            FNameBox.Text = dataIntermed.fName;
            LNameBox.Text = dataIntermed.lName;
            BalanceBox.Text = dataIntermed.bal.ToString("C");
            AcctNoBox.Text = dataIntermed.acct.ToString();
            PinBox.Text = dataIntermed.pin.ToString("D4");
        }

        private void GetTotalButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("count", Method.Get);
            var response = client.Execute(request);

            TotalNum.Text = response.Content;
        }

        private void Searchbut_Click(object sender, RoutedEventArgs e)
        {
            var mySearch = new SearchData { searchStr = Searchyboi.Text };
            var request = new RestRequest("search", Method.Post);
            request.AddJsonBody(mySearch);

            var response = client.Execute(request);

            var dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(response.Content);

            FNameBox.Text = dataIntermed.fName;
            LNameBox.Text = dataIntermed.lName;
            BalanceBox.Text = dataIntermed.bal.ToString("C");
            AcctNoBox.Text = dataIntermed.acct.ToString();
            PinBox.Text = dataIntermed.pin.ToString("D4");
        }

        private void GetImageButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("image", Method.Get);
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(response.RawBytes);
                image.EndInit();
                ImageBox.Source = image;
            }
        }
    }
}