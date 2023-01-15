using DCSModulRandomiser;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DCSModuleRandomiser.WindowsPanels
{
    /// <summary>
    /// Interaction logic for ProfilEditor.xaml
    /// </summary>
    public partial class ProfilEditor : Window
    {
        private string m_profilPath;
        public string ProfilPath
        {
            set
            {
                m_profilPath = value;
                InitializeComponent();
            }
            get => m_profilPath;
        }

        private DMRProfile m_dMRProfile;

        public ProfilEditor(string profil_path)
        {
                ProfilPath = profil_path;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            m_dMRProfile = Serializer.Deserialize(ProfilPath);

            LB_Profil.Content = ProfilPath;
            TB_MinDay.Text = m_dMRProfile.dayMin.ToString();
            TB_MaxDay.Text = m_dMRProfile.dayMax.ToString();
        }
    }
}
