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
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public ObservableCollection<Preset> presets = new ObservableCollection<Preset>();
        ObservableCollection<Rules> rules = new ObservableCollection<Rules>();
        PresetReader presetReader = PresetReader.getInstance();
        RulesFactory RuleFactory = RulesFactory.getInstance();

        public event PropertyChangedEventHandler? PropertyChanged;
        public Stack<IRenameRules> currentRules_stack = new Stack<IRenameRules>();

        public String PrototypeName { get; set; }

        //Use when Window_loaded
        private void loadPresetJson()
        {
            try
            {
                List<Preset> presets_list = new List<Preset>();

                using (StreamReader r = new StreamReader("presetState.json"))
                {
                    string json = r.ReadToEnd();
                    presets_list = JsonSerializer.Deserialize<List<Preset>>(json);
                }

                presets = new ObservableCollection<Preset>(presets_list);
                Presets.ItemsSource = presets;

                if(presets.Count != 0)
                {
                    Presets.SelectedItem = presets[0];
                }
            }
            catch
            {
                //eat exception
            }

        }

        //Use when Window_close
        private void writePresetJson()
        {
            List<Preset> presets_list = new List<Preset>(presets);
            string json = JsonSerializer.Serialize(presets_list);
            File.WriteAllText("presetState.json", json);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Left = DynamicBatchRename.Properties.Settings.Default.Left;
            Top = DynamicBatchRename.Properties.Settings.Default.Top;

            Width = DynamicBatchRename.Properties.Settings.Default.WindowWidth;
            Height = DynamicBatchRename.Properties.Settings.Default.WindowHeight;

            loadPresetJson();
        }

       

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DynamicBatchRename.Properties.Settings.Default.Left = (int)Left;
            DynamicBatchRename.Properties.Settings.Default.Top = (int)Top;

            DynamicBatchRename.Properties.Settings.Default.WindowWidth = (int)Width;
            DynamicBatchRename.Properties.Settings.Default.WindowHeight = (int)Height;

            DynamicBatchRename.Properties.Settings.Default.Save();
            writePresetJson();
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
            Presets.ItemsSource = presets;
            
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

       

        private void Add_Method_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearRules()
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

        private void updateRulesListView(IRenameRules current_rule)
        {
            try
            {
                foreach (Rules rule in rules)
                {
                    if (current_rule.getName().Equals(rule.rule.getName()))
                    {
                        ListViewItem ListIteminstance = ListRules.ItemContainerGenerator.ContainerFromItem(rule)
                        as ListViewItem;

                        ListIteminstance.Background = Brushes.LightGreen;
                    }
                }
            }
            catch
            {

            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            clearRules();
        }

        
        private void updatePreset(List<String> lines) {

            clearRules();

            foreach (String line in lines)
            {
                var line_split = line.Split(' ');
                string ruleName = line_split[0];
                ruleName = ruleName.Replace("\r", string.Empty);
                string data = "";
                for (int i = 1; i < line_split.Length; i++)
                {
                    if (i == line_split.Length - 1)
                    {
                        data += line_split[i];
                    }
                    else
                    {
                        data += line_split[i] + " ";
                    }
                }
                IRenameRules newRule = RuleFactory.createRules(ruleName);
                if(newRule == null)
                {
                    MessageBox.Show("This preset contains some of invalid rule!, Some of the valid rules will be added");
                    return;
                }
                newRule.parseData(data);
                

                string prototype = newRule.stringPrototype();

                updatePreviewPrototypeFromPreset(prototype, data);
                updateRulesListView(newRule);
                currentRules_stack.Push(newRule);
            }
            
        }

        private void updatePreviewPrototypeFromPreset(string prototype, string data)
        {
            string prototype_add = "";
            if (!prototype.Equals(""))
            {
                data = data.Replace(' ', ',');
                data = data.Replace("\n", string.Empty);
                data = data.Replace("\r", string.Empty);
                prototype_add = prototype[0] + data + prototype[1];
            }

            PrototypeName = prototype_add + PrototypeName;
            
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog preset_taker_dialog = new OpenFileDialog();
            preset_taker_dialog.Filter = "TXT files|*.txt";
            string allLines = "";
            FileInfo info = null;
            if (preset_taker_dialog.ShowDialog() == true)
            {
                FileStream fs = (FileStream) preset_taker_dialog.OpenFile();
                var readStream = new StreamReader(fs);
                allLines = readStream.ReadToEnd();
                info = new FileInfo(Path.GetFullPath(preset_taker_dialog.FileNames[0]));

            }
            else
            {
                return;
            }

            //Add preset into ObserveableList for easier control
            string preset_name = info.Name;
            string preset_path = info.FullName;
            Preset preset = new Preset(preset_name, preset_path);
            presets.Add(preset);
            Presets.SelectedItem= preset;

        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string current_prototype = new string(PrototypeName);
                Stack<IRenameRules> preview_stack = new Stack<IRenameRules>(currentRules_stack.Reverse());

                while (preview_stack.Count != 0)
                {
                    IRenameRules currentRules = (IRenameRules)preview_stack.Pop().Clone();
                    string rule_prototype = currentRules.stringPrototype();

                    if (rule_prototype != "")
                    {
                        char rule_prototype_first = rule_prototype[0];
                        char rule_prototype_second = rule_prototype[1];

                        int colon = current_prototype.IndexOf(rule_prototype_first) + 1;
                        int hash = current_prototype.IndexOf(rule_prototype_second, colon);
                        if (colon == 0 || hash == -1)
                        {
                            //Colon == 0 vi` -1 +1 = 0
                            MessageBox.Show("There are incorrections in the preview box!\n Please try to type it correctly");
                            break;
                        }
                        string result = current_prototype.Substring(colon, hash - colon);
                        current_prototype = current_prototype.Substring(hash + 1, current_prototype.Length - hash - 1);

                        currentRules.parseData(result);
                    }


                    foreach (text file in files)
                    {
                        file.NewName = currentRules.Rename(file.NewName);
                    }

                    foreach (text file in folders)
                    {
                        file.NewName = currentRules.Rename(file.NewName);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is an errors in parameters");
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

        

        private void Presets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Preset currentPreset = (Preset)Presets.SelectedItem;
                string path = currentPreset.uri;
                var readStream = new StreamReader(path);
                string allLines = readStream.ReadToEnd();
                List<String> lines_parsed = presetReader.parsePreset(allLines);
                updatePreset(lines_parsed);
            }
            catch
            {
                //eat exception
            }

        }

        private string removeBetween(string input, string start, string end)
        {
            string result = "";

            int start_index = input.LastIndexOf(start) + start.Length;
            int end_index = input.IndexOf(end, start_index);
            result = input.Remove(start_index -1, end_index - start_index +2);

            return result;
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
                ListIteminstance.Background = Brushes.White;

                //Thuật toán để loại bỏ một phần tử nằm ở giữa stack
                Stack<IRenameRules> temp = new Stack<IRenameRules>();

                IRenameRules currentRule = rules[currentRulesIndex].getRule();

                while (!currentRule.getName().Equals(currentRules_stack.Peek().getName()))
                {
                    temp.Push(currentRules_stack.Pop());
                }

                currentRules_stack.Pop();

                while(temp.Count != 0)
                {
                    currentRules_stack.Push(temp.Pop());
                }

                string stringPrototype = currentRule.stringPrototype();
                if(stringPrototype != "")
                {
                    PrototypeName = removeBetween(PrototypeName, 
                        Char.ToString(stringPrototype[0]), Char.ToString(stringPrototype[1]));
                }

            }

        }


       

        private void fileListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        

        private void ClearPreset_Click(object sender, RoutedEventArgs e)
        {
            Preset deletePreset = (Preset) Presets.SelectedItem;
            Presets.ItemsSource = null;
            presets.Remove(deletePreset);
            Presets.ItemsSource = presets;
            if(presets.Count != 0)
            {
                Presets.SelectedIndex = 0;
            }
        }

        

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            string result = "";

            try
            {
                result = PresetReader.getInstance().parsePreset(PrototypeName, currentRules_stack);
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is an error in parameters");
            }
            SavePreset savePreset_screen = new SavePreset(result);
           if(savePreset_screen.ShowDialog() == true)
            {

            }
            else
            {

            }

        }

        
    }
}
