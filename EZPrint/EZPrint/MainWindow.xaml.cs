using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
//using GoogleCloudPrintServices.DTO;
//using GoogleCloudPrintServices;
//using GoogleCloudPrintServices.Support;
using GoogleCloudPrintServices.DTO;
using GoogleCloudPrintServices;
using GoogleCloudPrintServices.Support;
using System.IO;

namespace EZPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String selectedPrinter = "";
        string filepath;
        string[] allPrinters = new string[200];
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TestGetPrinters();
        }

        private void btnFirstTimeSetup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLoadPrinters_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var cloudPrint = new GoogleCloudPrint("");
                cloudPrint.UserName = txtUsername.Text + "%40ridgewood%2Ek12%2Enj%2Eus";
                cloudPrint.Password = pwdPassword.Password;
                var printers = cloudPrint.Printers;
                int x = 0;
                foreach (CloudPrinter printer in printers.printers)
                {

                    //name is the name of the printer on the system when added to google cloud print
                    //description is the comment of the printer in the Comment field on Windows. Mac shared printer was blank.

                    //Console.WriteLine(@"name:{0} description:{1}", printer.name, printer.description, printer.name);
                    //lstPrinters.Items.Add(String.Format(@"{0}", printer.name));

                    lstPrinters.Items.Add(printer.description);
                    allPrinters[x] = printer.description; 
                    x++;
                }
                //Console.ReadLine();

                //CloudPrintJob job = cloudPrint.PrintDocument(printers.printers[0].id, "Title", FileToByteArray(@"page.pdf"), @"application/pdf");
                //Console.WriteLine(job.message);
            }
            catch
            {
                MessageBox.Show("Could not access API. Please make sure you aren't using your personal account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF Files (.pdf)|*.pdf";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filepath = dlg.FileName;
                //textBox1.Text = filename;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cloudPrint = new GoogleCloudPrint("");
                cloudPrint.UserName = txtUsername.Text + "%40ridgewood%2Ek12%2Enj%2Eus";
                cloudPrint.Password = pwdPassword.Password;
                var printers = cloudPrint.Printers;
                int intPrinterIndex = 0;
                foreach (CloudPrinter printer in printers.printers)
                {

                    //name is the name of the printer on the system when added to google cloud print
                    //description is the comment of the printer in the Comment field on Windows. Mac shared printer was blank.

                    //Console.WriteLine(@"name:{0} description:{1}", printer.name, printer.description, printer.name);
                    //lstPrinters.Items.Add(String.Format(@"{0}", printer.name));
                    //lstPrinters.
                    //lstPrinters.Items.Add(printer.description);
                    if (selectedPrinter == printer.description)
                    {
                        //MessageBox.Show(printers.printers[intPrinterIndex].id);
                        CloudPrintJob job = cloudPrint.PrintDocument(printers.printers[intPrinterIndex].id, "PDF File", FileToByteArray(filepath), @"application/pdf");
                        MessageBox.Show("Print successful!");
                    }
                    intPrinterIndex++;
                }
                //Console.ReadLine();

                //CloudPrintJob job = cloudPrint.PrintDocument(printers.printers[0].id, "Title", FileToByteArray(@"page.pdf"), @"application/pdf");
                //Console.WriteLine(job.message);
                
            }
            catch
            {
                MessageBox.Show("Could not access API. Please make sure you aren't using your personal account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lstPrinters_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // http://stackoverflow.com/questions/15003095/getting-value-of-selected-item-in-list-box-as-string
            /*var selected = lstPrinters.SelectedValue;
            selectedPrinter = selected.ItemString;

            MessageBox.Show(selectedText);*/
            int index = lstPrinters.SelectedIndex;
            selectedPrinter = allPrinters[index];
            //MessageBox.Show(selectedPrinter);
        }
        // http://pastie.org/pastes/1733781/text
        public void TestGetPrinters()
        {
            var cloudPrint = new GoogleCloudPrint("");
            #region info
            cloudPrint.UserName = "blah@gmail.com";
            cloudPrint.Password = "password";
            #endregion
            //var printers = new cloudPrint.Printers();
            //cloudPrint.Printers x = new cloudPrint.printers();
            var printers = cloudPrint.Printers;

            var printerList = printers.printers;
            foreach (var p in printerList)
            {
                lstPrinters.Items.Add(p.name);
            }
            /*foreach (var p in printers)
            {
                
            }*/
        }





    }
}