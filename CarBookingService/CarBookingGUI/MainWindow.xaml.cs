using CarBookingService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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

namespace CarBookingGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private HttpClient client = new HttpClient();
        public List<Car> carsValue = new List<Car>();



        public List<Car> Cars
        {
            get => carsValue;
            set
            {
                carsValue = value;
                OnPropertyChanged(nameof(Cars));
            }
        }

        public List<Car> filteredCarsValue = new List<Car>();

        public List<Car> FilteredCars
        {
            get => filteredCarsValue;
            set
            {
                filteredCarsValue = value;
                OnPropertyChanged(nameof(FilteredCars));
            }
        }

        public List<Car> SelectCarsValue = new List<Car>();

        public List<Car> SelectCars
        {
            get => SelectCarsValue;
            set
            {
                SelectCarsValue = value;
                OnPropertyChanged(nameof(SelectCars));
            }
        }

        public DateTime selectedDateValue;


        public DateTime SelectedDate
        {
            get => selectedDateValue;
            set
            {
                selectedDateValue = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }


        public DateTime BookingDateValue;
        public DateTime BookingDate
        {
            get => BookingDateValue;
            set
            {
                BookingDateValue = value;
                OnPropertyChanged(nameof(BookingDate));
            }
        }

        public Car SelectedCarValue;
        public Car SelectedCar
        {
            get => SelectedCarValue;
            set
            {
                SelectedCarValue = value;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }
        public MainWindow()
        {
            this.SelectedDate = DateTime.Now;
            this.BookingDate = DateTime.Now;

            FetchCars();

            InitializeComponent();
            //this.SelectedCar = this.Cars.FirstOrDefault();
            this.BookingBox.SelectedIndex = 0;
            this.CarGrid.AutoGeneratingColumn += DataGrid_OnAutoGeneratingColumn;
            this.BookDate.SelectedDateChanged += picker_ValueChanged;
            this.DataContext = this;

        }
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void FetchCars()
        {

            var response = client.GetAsync("http://localhost:8300/cars").Result;
            var result = JsonConvert.DeserializeObject<List<Car>>(response.Content.ReadAsStringAsync().Result);
            this.Cars = result;
            this.FilteredCars = result;
        }
        public void FetchFilteredCars(DateTime date)
        {
            var response = client.GetAsync("http://localhost:8300/cars/"+date.Date.ToString()).Result;
            var result = JsonConvert.DeserializeObject<List<Car>>(response.Content.ReadAsStringAsync().Result);
            this.FilteredCars = result;
        }
        public void FetchFilteredCarsDate(DateTime date)
        {
            var response = client.GetAsync("http://localhost:8300/cars/" + date.Date.ToString()).Result;
            var result = JsonConvert.DeserializeObject<List<Car>>(response.Content.ReadAsStringAsync().Result);
            this.SelectCars = result;
            this.SelectedCar = this.Cars.FirstOrDefault();
            this.BookingBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.FilteredCars = this.Cars;
        }

        private void Filter_Cars(object sender, RoutedEventArgs e)
        {
            FetchFilteredCars(this.SelectedDate.Date);
        }
        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "CarId" || e.PropertyName == "BookingDates")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void Book_Car(object sender, RoutedEventArgs e)
        {
            if(this.SelectedCar.BookingDates == null)
            {
                this.SelectedCar.BookingDates = new List<BookingDate>();
            }
            BookingDate bd = new BookingDate();
            bd.BookDate = this.BookingDate;
            var json = JsonConvert.SerializeObject(bd.BookDate.Date);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("http://localhost:8300/book/"+this.SelectedCar.CarId,data).Result;
            var result = JsonConvert.DeserializeObject<Car>(response.Content.ReadAsStringAsync().Result);
            FetchFilteredCars(this.SelectedDate);
            FetchFilteredCarsDate(this.BookingDate);
        }
        private void picker_ValueChanged(object sender, EventArgs e)
        {
            this.FetchFilteredCarsDate(this.BookingDate);
        }
    }
}
