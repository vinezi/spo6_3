using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace spo6_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TreeViewDrivers();
        }

        void TreeViewDrivers()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Tag = drive;
                item.Header = drive.ToString();
                item.Items.Add("*");
                treeView1.Items.Add(item);
                item.Expanded += new RoutedEventHandler(item_DirExpanded);
            }
        }

        void item_DirExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();
            DirectoryInfo dir;
            if (item.Tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)item.Tag;
                dir = drive.RootDirectory;
            }
            else dir = (DirectoryInfo)item.Tag;
            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Tag = subDir;
                    newItem.Header = subDir.ToString();
                    newItem.Items.Add("*");
                    item.Items.Add(newItem);
                    newItem.Expanded += new RoutedEventHandler(Item_FileExpanded);
                }
            }
            catch{ }
        }

        void Item_FileExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();
            DirectoryInfo driv;
            if (item.Tag is DriveInfo)
            {
                DriveInfo dr = (DriveInfo)item.Tag;
                driv = dr.RootDirectory;
            }
            else driv = (DirectoryInfo)item.Tag;
            try
            {
                listBox1.Items.Clear();
                foreach (FileInfo fi in driv.GetFiles())
                {
                    TreeViewItem newItem1 = new TreeViewItem();
                    newItem1.Tag = fi;
                    newItem1.Header = fi.ToString();
                    newItem1.Items.Add("*");
                    item.Items.Add(newItem1);
                    listBox1.Items.Add(fi);
                }
            }
            catch { }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fullPath = path.Text + "\\";
            DirectoryInfo di = new DirectoryInfo(fullPath);
            FileInfo[] fi = di.GetFiles();
            if (listBox1.SelectedItem != null)
            {
                foreach (FileInfo f in fi)
                {
                    if (f.Name == listBox1.SelectedItem.ToString())
                    {
                        listBox2.Items.Clear();
                        foreach (var item in f.Attributes.ToString().Split(' '))
                        {
                            listBox2.Items.Add(item.TrimEnd(','));
                        }
                    }
                }
            }
            else
                listBox2.Items.Clear();
        }

        public string GetFullPath(TreeViewItem node)
        {
            if (node == null)
                throw new ArgumentNullException();

            var result = Convert.ToString(node.Header);

            for (var i = GetParentItem(node); i != null; i = GetParentItem(i))
                result = i.Header + "\\" + result;
            path.Text = result.Remove(2, 1);
            return result;
        }

        static TreeViewItem GetParentItem(TreeViewItem item)
        {
            for (var i = VisualTreeHelper.GetParent(item); i != null; i = VisualTreeHelper.GetParent(i))
                if (i is TreeViewItem)
                    return (TreeViewItem)i;
            return null;
        }

        private void treeView1_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            GetFullPath(item);
        }
    }
}