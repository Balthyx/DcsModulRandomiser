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

namespace DCSModuleRandomiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PROFIL_FILE = "JSON_Profiles/RProfil.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            OutputField.Text = Randomisator.Get(PROFIL_FILE);
            BtnReroll.Click += BtnReroll_Click;
        }

        private void BtnReroll_Click(object sender, RoutedEventArgs e)
        {
            OutputField.Text = Randomisator.Get(PROFIL_FILE, true);
        }
    }
}
