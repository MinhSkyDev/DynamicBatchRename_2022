using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace DynamicBatchRename
{
    /// <summary>
    /// Interaction logic for SavePreset.xaml
    /// </summary>
    public partial class SavePreset : Window, INotifyPropertyChanged
    {
        private string result;
        public string PresetPath { get; set; }
        private string PresetPath_temp { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public SavePreset(string result)
        {
            this.result = result.Remove(result.Length - 1);
            InitializeComponent();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
            string currentPath = Preset_textbox.Text;
            if (currentPath.Equals(PresetPath_temp))
            {
                MessageBox.Show("You haven't type the preset name!");
            }
            else
            {

                StreamWriter streamWriter = File.CreateText(PresetPath);
                streamWriter.Write(result);
                MessageBox.Show("Save preset successfully!");
                streamWriter.Close();
            }
            DialogResult = true;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
            {
                PresetPath = dialog.SelectedPath;
                PresetPath_temp = dialog.SelectedPath;
                //MessageBox.Show(folder);
                
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string current = Preset_textbox.Text;
            current += ".txt";
            PresetPath = PresetPath_temp + @"\" + current;
        }
    }
}
