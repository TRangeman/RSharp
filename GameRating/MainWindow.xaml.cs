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
using CustomUtil;

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
            Logger.directory = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            InitializeComponent();
            loadRegions(ConfigurationManager.AppSettings["defaultRegion"]);
            SummonerInputBox.Focus();
            SummonerInputBox.SelectAll();
        }

        private void initConnection()
        {
            updateLabel(ConfigurationManager.AppSettings["ApiKey"]);
            api = RiotApi.GetInstance(ConfigurationManager.AppSettings["ApiKey"], 10);
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

        private async void load()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            toggleWorking(true);
            try
            {
                Analyser analyser = new Analyser(api, StaticRiotApi.GetInstance(ConfigurationManager.AppSettings["ApiKey"]));
                List<AnalysedMatch> matches = await analyser.getAnalysedMatchHistory(SummonerInputBox.Text, parseString2Region(regionBox.SelectedItem.ToString()), historyIndex);
                matches.Reverse();
                pMatches.AddRange(matches);
                historyView.ItemsSource = null;
                historyView.ItemsSource = pMatches;
                this.Width = historyView.ActualWidth + 150;
            } catch(Exception ex)
            {
                if (ex.Message == "ErrorSumm")
                    MessageBox.Show("Summoner not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    MessageBox.Show(@"An unexpected error occurred!" + Environment.NewLine + "Please inform the creator of this application." + Environment.NewLine + "The logfile can be found at:" + Environment.NewLine + System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"TeamRating.log", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.logError(ex.StackTrace.ToString() + Environment.NewLine + ex.Message);
                }
            }
            finally
            {
                toggleWorking(false);
                Mouse.OverrideCursor = null;
            }
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

        private void updateLabel(string text)
        {
            keyLabel.Content = text;
            MainWindow1.MinWidth = this.keyLabel.ActualWidth + this.loadHistoryBt.ActualWidth + this.ldMoreBt.ActualWidth + 25;
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            initConnection();
        }

        private void toggleWorking(bool working)
        {
            working = !working;
            this.ldMoreBt.IsEnabled = working;
            this.loadHistoryBt.IsEnabled = working;
        }
    }
}
