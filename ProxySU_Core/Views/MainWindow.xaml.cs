﻿using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using ProxySU_Core.Models;
using ProxySU_Core.ViewModels;
using ProxySU_Core.ViewModels.Developers;
using ProxySU_Core.Views;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace ProxySU_Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string RecordPath = @"Data\Record.json";

        public ObservableCollection<RecordViewModel> Records { get; set; }

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            Records = new ObservableCollection<RecordViewModel>();

            if (File.Exists(RecordPath))
            {
                var recordsJson = File.ReadAllText(RecordPath, Encoding.UTF8);
                var records = JsonConvert.DeserializeObject<List<Record>>(recordsJson);
                records.ForEach(item =>
                {
                    Records.Add(new RecordViewModel(item));
                });
            }


            DataContext = this;
        }


        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/proxysu/ProxySU");
        }

        private void LaunchCoffeeSite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/proxysu/ProxySU");
        }

        private void ChangeLanguage(object sender, SelectionChangedEventArgs e)
        {
            var selection = cmbLanguage.SelectedValue as ComboBoxItem;

            if (selection.Name == "zh_cn")
            {
                ChangeLanguage("zh_cn");
            }
            else if (selection.Name == "en")
            {
                ChangeLanguage("en");
            }
        }

        private void ChangeLanguage(string culture)
        {
            ResourceDictionary resource = new ResourceDictionary();

            if (string.Equals(culture, "zh_cn", StringComparison.OrdinalIgnoreCase))
            {
                resource.Source = new Uri(@"Resources\Languages\zh_cn.xaml", UriKind.Relative);
            }

            else if (string.Equals(culture, "en", StringComparison.OrdinalIgnoreCase))
            {
                resource.Source = new Uri(@"Resources\Languages\en.xaml", UriKind.Relative);
            }

            Application.Current.Resources.MergedDictionaries[0] = resource;
        }


        private void AddHost(object sender, RoutedEventArgs e)
        {
            var hostWindow = new RecordEditorWindow(new Record());
            hostWindow.ShowDialog();
        }

        private void EditHost(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem is RecordViewModel project)
            {
                var hostWindow = new RecordEditorWindow(project.record);
                hostWindow.ShowDialog();
            }
        }


        private void DeleteHost(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem is RecordViewModel project)
            {
                var result = MessageBox.Show($"您确认删除主机{project.Host.Tag}吗？", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Records.Remove(project);
                }
            }

        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            var project = DataGrid.SelectedItem as Record;
            if (project == null)
            {
                DialogManager.ShowMessageAsync(this, "提示", "请选择一个服务器");
            }

            TerminalWindow terminalWindow = new TerminalWindow(project);
            terminalWindow.Show();
        }
    }
}
