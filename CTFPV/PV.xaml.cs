using Encryption_Key_Finder.InformationItems;
using Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CTFPV
{
    public partial class PV : Window
    {
        public static Mem MemLib = new Mem();
        public static long MainPointer = 0;
        public static string BasePointer = "base";
        public static string CurrentProcess = "";

        private Task runLoop;
        private int previousFrame;

        public static CRunApp CRunApp;
            
        public PV()
        {
            InitializeComponent();
            CRunApp = new CRunApp();
        }

        private void CloseProgram(object sender, EventArgs e) => Environment.Exit(0);

        private void Loader_Click(object sender, RoutedEventArgs e)
        {
            Objects.Items.Clear();
            MainPointer = 0;
            if (runLoop != null)
                runLoop.Dispose();
            runLoop = new Task(RunLoop);
            runLoop.Start();
        }

        public async void RunLoop()
        {
            await CreateList();
            if (MainPointer <= 0)
                return;
            previousFrame = -1;
            while (true)
            {
                if (previousFrame != CRunApp.CurrentFrame)
                {
                    previousFrame = CRunApp.CurrentFrame;
                    RefreshList();
                }
                Thread.Sleep((int)(1000.0 / CRunApp.FramesPerSecond));
            }
        }

        public async Task CreateList()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                Objects.Items.Clear();
                MainPointer = 0;
                if (Processes.SelectedItem == null) return;
                CurrentProcess = Path.GetFileNameWithoutExtension((Processes.SelectedItem as ComboBoxItem).Content.ToString());
            }));

            if (!MemLib.OpenProcess(CurrentProcess + ".exe"))
                return;

            await FindMV();
            if (MainPointer <= 0)
                return;
            BasePointer = $"base+{MainPointer.ToString("X")}";

            CRunApp = new CRunApp();
            CRunApp.InitData();
        }

        public void RefreshList()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                Objects.Items.Clear();
                Objects.Items.Add(CRunApp.GetListPanel());
            }));
        }

        public async Task FindMV()
        {
            IEnumerable<long> BaseSearch = await MemLib.AoBScan(65536, 16777216 * 2, "4D 5A 90 00 03 00 00 00 04 00 00 00 FF FF 00 00 B8 00 00 00 00 00 00 00 40", true, false, true, false, "");
            if (BaseSearch.Count() == 0) 
                return;
            var baseAddress = BaseSearch.ToArray()[0];

            IEnumerable<long> PAMUSearch = await MemLib.AoBScan("50 41 4D 55", true, true);
            
            long Header = 0;
            foreach (long result in PAMUSearch)
            {
                int i = MemLib.ReadInt((result + 4).ToString("X"));
                if (i == 770)
                {
                    Header = result;
                    break;
                }
            }

            if (Header == 0)
                return;
            string PAMUBytesA = "";
            foreach (byte b in BitConverter.GetBytes((int)Header))
            {
                if (b.ToString("X") == "0")
                    PAMUBytesA += "00 ";
                else if (b.ToString("X").Length == 1)
                    PAMUBytesA += $"0{b.ToString("X")} ";
                else
                    PAMUBytesA += $"{b.ToString("X")} ";
            }
            IEnumerable<long> PointerSearch = await MemLib.AoBScan(PAMUBytesA.Trim(), true, true);

            long Pointer = 0;
            foreach (long pointer in PointerSearch)
            {
                long i = pointer - baseAddress;
                if (i > 1048576 || i < 0)
                    continue;
                string output = MemLib.ReadString("base+" + i.ToString("X") + ", 0x0", "", 4);
                if (output == "PAMU")
                    MainPointer = i;
            }
        }

        private void ProcessesSelected(object sender, EventArgs e)
        {
            Processes.Items.Clear();

            List<string> processes = new();
            foreach (Process process in Process.GetProcesses())
                if (process.BasePriority == 8)
                    processes.Add(process.ProcessName);
            GFG gg = new();
            processes.Sort(gg);
            foreach (string process in processes)
            {
                ComboBoxItem newCombo = new();
                newCombo.Content = process + ".exe";
                bool no = false;
                foreach (ComboBoxItem combo in Processes.Items)
                    if (combo.Content.ToString() == newCombo.Content.ToString())
                        no = true;
                if (!no)
                    Processes.Items.Add(newCombo);
            }
        }
    }

    public static class Extensions
    {
        public static BitmapImage ToImage(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }

    class GFG : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null || y == null)
                return 0;
            return x.CompareTo(y);
        }
    }
}
