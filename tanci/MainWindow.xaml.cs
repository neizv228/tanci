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
using tanci.BDModel;
using tanci.StrWindow;

namespace tanci
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<User> userObj;
        private string UserFIO;
        private string Privet = "Добро пожаловать!";
        public MainWindow()
        {
            InitializeComponent();
            userObj = chasiEntities.GetContext().User.ToList();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            var CurrentUsers = userObj.Where(user => user.UserLogin == log.Text && user.UserPassword == pass.Password).FirstOrDefault();
            UserFIO = $"Добро пожаловать, {CurrentUsers.UserSurname} {CurrentUsers.UserName} {CurrentUsers.UserPatronymic}!";
            if (CurrentUsers != null)
            {
                if (CurrentUsers.UserRole == 1)
                {
                    Tovars a = new Tovars(1, UserFIO);
                    a.Show();
                    this.Close();
                }
                else if (CurrentUsers.UserRole == 2)
                {
                    Tovars m = new Tovars(2, UserFIO);
                    m.Show();
                    this.Close();
                }
                else if (CurrentUsers.UserRole == 3)
                {
                    Tovars c = new Tovars(3, UserFIO);
                    c.Show();
                    this.Close();
                }
            }
            else MessageBox.Show("Вы ввели неправильный логин или пароль!", "Внимание!", MessageBoxButton.OK);
        }

        private void GuestEnter_Click(object sender, RoutedEventArgs e)
        {
            Tovars g = new Tovars(2, Privet);
            g.Show();
            this.Close();
        }
    }
}

