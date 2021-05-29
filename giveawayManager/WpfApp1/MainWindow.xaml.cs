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
using WpfApplication1.BpmApi;
using WpfApplication1.ConstantStrings;
using WpfApplication1.GiveawayNamespace;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ItsmIncident caseInfo = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void findCaseButton_Click(object sender, RoutedEventArgs e)
        {
            ItsmIncident caseInfo = BpmClient.testGetInfoByCaseNumber(caseNumberTextBox.Text);
            if (caseInfo.Result.First().ServiceItem == ServiceItem.WORKPLACE_CONFIGURATION)
            {
                this.caseInfo = caseInfo;
                descriptionTextBox.Text = this.caseInfo.Result.First().Description;
            }
            else
            {
                MessageBox.Show("Обращение должно иметь тип \"" + ServiceItem.WORKPLACE_CONFIGURATION + "\"");
            }


        }

        private void createGiveawayButton_Click(object sender, RoutedEventArgs e)
        {
            int id = GiveawayProcessor.generateId();
            GiveawayProcessor.createGiveaway(caseInfo);
            MessageBox.Show("Выдача создана успешно. Инициатору обращение отправлено сообщение с кодом.");


        }
    }
}
