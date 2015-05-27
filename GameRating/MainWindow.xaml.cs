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
using System.Configuration;
using RiotSharp;
using System.Data;
using RiotSharp.SummonerEndpoint;

namespace GameRating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RiotApi api;
        private int historyIndex = 0;
        private List<AnalysedMatch> pMatches;

        public MainWindow()
        {
            pMatches = new List<AnalysedMatch>();
            InitializeComponent();
            loadRegions(ConfigurationManager.AppSettings["defaultRegion"]);
            initConnection();
            SummonerInputBox.Focus();
            SummonerInputBox.SelectAll();
        }

        private void initConnection()
        {
            keyLabel.Content = ConfigurationManager.AppSettings["ApiKey"];
            api = RiotApi.GetInstance(ConfigurationManager.AppSettings["ApiKey"]);
        }

        private void loadRegions(string defaultSStr)
        {
            Region defaultSetting = parseString2Region(defaultSStr);
            var regions = Enum.GetValues(typeof(Region));
            foreach(Region reg in regions)
            {
                regionBox.Items.Add(reg.ToString().ToUpper());
                if(reg == defaultSetting)
                    regionBox.SelectedItem = reg.ToString().ToUpper();
            }
        }

        private Region parseString2Region(string regionString)
        {
            regionString = regionString.ToLower();
            Region region = new Region();
            if (!Enum.TryParse<Region>(regionString, out region))
                throw new Exception();
            return region;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            historyIndex = 0;
            pMatches.Clear();
            load();
        }

        private void load()
        {
            Analyser analyser = new Analyser(api, StaticRiotApi.GetInstance(ConfigurationManager.AppSettings["ApiKey"]));
            List<AnalysedMatch> matches = analyser.getAnalysedMatchHistory(SummonerInputBox.Text, parseString2Region(regionBox.SelectedItem.ToString()),historyIndex);
            matches.Reverse();
            pMatches.AddRange(matches);
            historyView.ItemsSource = null;
            historyView.ItemsSource = pMatches;
        }

        private void SummonerInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            { 
                pMatches.Clear();
                historyIndex = 0;
                load();
            }
        }

        private void ldMoreBt_Click(object sender, RoutedEventArgs e)
        {
            historyIndex++;
            load();
        }
    }
}
