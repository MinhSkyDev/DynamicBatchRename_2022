using IRenameRules_namepsace;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
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
using File = System.IO.File;
using Path = System.IO.Path;

namespace DynamicBatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<text> files = new ObservableCollection<text>();
        public ObservableCollection<text> folders = new ObservableCollection<text>();
        ObservableCollection<Rules> rules = new ObservableCollection<Rules>();
        PresetReader presetReader = PresetReader.getInstance();
        RulesFactory RuleFactory = RulesFactory.getInstance();

        public event PropertyChangedEventHandler? PropertyChanged;
        public Stack<IRenameRules> currentRules_stack = new Stack<IRenameRules>();

        public String PrototypeName { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Left = DynamicBatchRename.Properties.Settings.Default.Left;
            Top = DynamicBatchRename.Properties.Settings.Default.Top;

            Width = DynamicBatchRename.Properties.Settings.Default.WindowWidth;
            Height = DynamicBatchRename.Properties.Settings.Default.WindowHeight;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DynamicBatchRename.Properties.Settings.Default.Left = (int)Left;
            DynamicBatchRename.Properties.Settings.Default.Top = (int)Top;

            DynamicBatchRename.Properties.Settings.Default.WindowWidth = (int)Width;
            DynamicBatchRename.Properties.Settings.Default.WindowHeight = (int)Height;

            DynamicBatchRename.Properties.Settings.Default.Save();
        }
        public void loadRules()
        {
            string rule_path = AppDomain.CurrentDomain.BaseDirectory;
            rule_path = rule_path + @"Rules";
            var folderInfo = new DirectoryInfo(rule_path);
            var dllFiles = folderInfo.GetFiles("*.dll");

            foreach (var file in dllFiles)
            {
                var assembly = Assembly.LoadFrom(file.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass && typeof(IRenameRules).IsAssignableFrom(type))
                    {
                        IRenameRules rule = (IRenameRules)Activator.CreateInstance(type)!;
                        RuleFactory.RegisterRules(rule);
                        Rules rule_binding = new Rules(rule);
                        rules.Add(rule_binding);
                    }
                }
            }



        }

        public MainWindow()
        {
            InitializeComponent();
            fileListView.ItemsSource = files;
            folderListView.ItemsSource = folders;
            loadRules();
            ListRules.ItemsSource = rules;
            PrototypeName = "NAME";
            PrototypeRulesTextBox.DataContext = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            foreach(text file in files)
            {
                string currentFolder_path = file.path;
                string newPath = currentFolder_path + $"/{file.NewName}";
                if (File.Exists(newPath))
                {
                    file.Name = file.NewName;
                }
                else
                {
                    //eat else
                }
            }
            foreach (text file in folders)
            {
                string currentFolder_path = file.path;
                string newPath = currentFolder_path + $"/{file.NewName}";
                if (File.Exists(newPath))
                {
                    file.Name = file.NewName;
                }
                else
                {
                    //eat else
                }
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Method_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrototypeName = "NAME";
                currentRules_stack.Clear();
                foreach (Rules rule in rules)
                {
                    ListViewItem ListIteminstance = ListRules.ItemContainerGenerator.ContainerFromItem(rule)
                    as ListViewItem;

                    ListIteminstance.Background = Brushes.White;
                }
            }
            catch
            {
                //eat exception
            }
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            string current_prototype = new string(PrototypeName);
            Stack<IRenameRules> preview_stack = new Stack<IRenameRules>(currentRules_stack);

            while(preview_stack.Count != 0)
            {
                IRenameRules currentRules = preview_stack.Pop();
                string rule_prototype = currentRules.stringPrototype();

                if (rule_prototype != "")
                {
                    char rule_prototype_first = rule_prototype[0];
                    char rule_prototype_second = rule_prototype[1];

                    int colon = current_prototype.IndexOf(rule_prototype_first) + 1;
                    int hash = current_prototype.IndexOf(rule_prototype_second, colon);
                    string result = current_prototype.Substring(colon, hash - colon);
                    current_prototype = current_prototype.Substring(hash + 1, current_prototype.Length - hash - 1);

                    currentRules.parseData(result);
                }


                foreach(text file in files)
                {
                    file.NewName = currentRules.Rename(file.NewName);
                }

                foreach (text file in folders)
                {
                    file.NewName = currentRules.Rename(file.NewName);
                }

            }


        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    FileInfo info = new FileInfo(Path.GetFullPath(file));
                    string name = info.Name;
                    long size = info.Length;
                    DateTime status = info.LastWriteTime;
                    string location = info.DirectoryName;
                    text item = new text() { Name = name, NewName = name ,Size = size, Status = status, path = location };
                    bool containsItem = false;
                    if (files != null)
                    {
                        containsItem = files.Any(x => x.path == item.path && x.Name == item.Name);
                    }
                    if (!containsItem)
                        files.Add(item);
                }
            }
            fileListView.ItemsSource = files;
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
                foreach (string folder in dialog.SelectedPaths)
                {
                    selectFolders(folder);
                }

        }

        private void selectFolders(string filename)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(filename);

            FileInfo[] info = dirInfo.GetFiles("*.*");
            foreach (FileInfo fi in info)
            {
                string name = fi.Name;
                long size = fi.Length;
                DateTime status = fi.LastWriteTime;
                string location = fi.DirectoryName;
                text item = new text() { Name = name, NewName = name ,Size = size, Status = status, path = location };
                bool containsItem = files.Any(x => x.path == item.path && x.Name == item.Name);
                if (!containsItem)
                    folders.Add(item);
            }

            DirectoryInfo[] subDirectories = dirInfo.GetDirectories();
            foreach (DirectoryInfo directory in subDirectories)
            {
                selectFolders(directory.FullName);
            }
        }

        private void btnUncheckAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (text file in files)
            {
                file.isChecked = false;
            }

            foreach (text file in folders)
            {
                file.isChecked = false;
            }
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(text file in files)
            {
                file.isChecked = true;
            }

            foreach (text file in folders)
            {
                file.isChecked = true;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            files.Clear();
            folders.Clear();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void StartBatch_Click(object sender, RoutedEventArgs e)
        {

            foreach (text file in folders)
            {
                if (file.isChecked)
                {
                    string currentName_path = file.path +@"\"+file.Name;
                    string newName_path = file.path + @"\" + file.NewName;
                    File.Move(currentName_path, newName_path);
                }
            }

            foreach (text file in files)
            {
                if (file.isChecked)
                {
                    string currentName_path = file.path + @"\" + file.Name;
                    string newName_path = file.path + @"\" + file.NewName;
                    File.Move(currentName_path, newName_path);
                }
            }

            MessageBox.Show("Rename Successfully!");

        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Presets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Rule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RuleItem_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ListRules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int currentRulesIndex = ListRules.SelectedIndex;

            var dataObject = ListRules.Items[currentRulesIndex];
            ListViewItem ListIteminstance = ListRules.ItemContainerGenerator.ContainerFromItem(dataObject)
                as ListViewItem;

            if (ListIteminstance.Background != Brushes.LightGreen)
            {
                //this.Title = rules[currentRulesIndex].Name;
                PrototypeName = rules[currentRulesIndex].rule.stringPrototype() + PrototypeName;


                IRenameRules currentRules = rules[currentRulesIndex].getRule();
                string rule_name = rules[currentRulesIndex].Name;
                currentRules_stack.Push(currentRules);

                string message = $"Rules {rule_name} added successfully";
                MessageBox.Show(message);

                ListIteminstance.Background = Brushes.LightGreen;
            }
            else
            {
                MessageBox.Show("This rules has been added !");
            }

        }


        private void fileListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void fileListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
