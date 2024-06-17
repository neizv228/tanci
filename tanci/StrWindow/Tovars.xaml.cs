using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using tanci.BDModel;

namespace tanci.StrWindow
{
    /// <summary>
    /// Логика взаимодействия для Tovars.xaml
    /// </summary>
    public partial class Tovars : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<chasi> _TovarList;
        private int ManufacturerFilterID = 0;
        private int SortType = 0;
        public List<Manufacturers> ManufacturerList { get; set; }
        public string[] SortList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию",
             "Мастер по возрастанию",
              "Мастер по убыванию",
                "Размер скидки по возрастанию",
              "Размер скидки по убыванию"

        };

        private void Invalidate(string ComponentName = "TovarList")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TovarList"));
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(ComponentName));
        }

        private string SearchFilter = "";

        public IEnumerable<chasi> TovarList
        {
            get
            {
                var Result = _TovarList;
                

                switch (SortType)
                {
                    case 0:
                        Result = chasiEntities.GetContext().chasi.ToList();
                        break;
                    case 1:
                        Result = chasiEntities.GetContext().chasi.OrderBy(p => p.TovarCost);
                        break;
                    case 2:
                        Result = chasiEntities.GetContext().chasi.OrderByDescending(p => p.TovarCost);
                        break;
                    case 3:
                        Result = chasiEntities.GetContext().chasi.OrderBy(p => p.TovarMaster);
                        break;
                    case 4:
                        Result = chasiEntities.GetContext().chasi.OrderByDescending(p => p.TovarMaster);
                        break;
                    case 5:
                        Result = chasiEntities.GetContext().chasi.OrderBy(p => p.TovarDiscountAmount);
                        break;
                    case 6:
                        Result = chasiEntities.GetContext().chasi.OrderByDescending(p => p.TovarDiscountAmount);
                        break;
                }
                if (SearchFilter != "")
                    Result = Result.Where(p =>
                    p.TovarName.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.TovarDescription.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                if (ManufacturerFilterID > 0)
                    Result = Result.Where(i => i.Manufacturers.ManufacturerID == ManufacturerFilterID);

                return Result.Take(100);
            }
            set
            {
                _TovarList = value;
                Invalidate();
            }
        }

        int UR { get; set; }
        public string UserFaio;

        public Tovars(int UserRoles, string UserFIO)
        
        {
            UR = UserRoles;
            UserFaio = UserFIO;
            InitializeComponent();
            DataContext = this;
            TovarList = chasiEntities.GetContext().chasi.ToList();
            ManufacturerList = chasiEntities.GetContext().Manufacturers.ToList();
            ManufacturerList.Insert(0, new Manufacturers { ManufacturerName = "Все типы" });
            FIOText.Text = UserFaio;
            if (UserRoles == 1)
            {
                TovarListView2.Visibility = Visibility.Hidden;
            }
            if (UserRoles == 2)
            {
                AddTovar.Visibility = Visibility.Hidden;
                TovarListView.Visibility = Visibility.Hidden;
               
                Update.Visibility = Visibility.Hidden;
                Wrapik.Height = 0;
            }
            if (UserRoles == 3)
            {
                AddTovar.Visibility = Visibility.Hidden;
                TovarListView.Visibility = Visibility.Hidden;
                Update.Visibility = Visibility.Hidden;
            }
        }

        private void SearchFilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchFilterTextBox.Text;
            Invalidate();
        }

        private void SortFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortType = SortTypeComboBox.SelectedIndex;
            Invalidate();
        }

   

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            TovarList = chasiEntities.GetContext().chasi.ToList();
            Tovars m = new Tovars(UR, UserFaio);
            m.Show();
            this.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            AddTovar add = new AddTovar((sender as Button).DataContext as chasi);
            add.ShowDialog();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var VelikiForRemoving = TovarListView.SelectedItems.Cast<chasi>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {VelikiForRemoving.Count()} элементы?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    chasiEntities.GetContext().chasi.RemoveRange(VelikiForRemoving);
                    chasiEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    TovarListView.ItemsSource = chasiEntities.GetContext().chasi.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void AddTovar_Click(object sender, RoutedEventArgs e)
        {
            AddTovar add = new AddTovar(null);
            add.ShowDialog();
        }

        private void DiscountAmountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ManufacturerFilterID = DiscountAmountComboBox.SelectedIndex;
            Invalidate();
        }
    }
}
