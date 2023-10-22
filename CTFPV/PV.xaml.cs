using CTFPV.InformationItems;
using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
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
        public static string CurrentProcess = "";

        private Task runLoop;
        private int previousFrame;

        public static CRunApp CRunApp = new CRunApp();
        public static CRunFrame CRunFrame;
        public static CRunHeader CRunHeader;

        public static string[] FrameNames;
            
        public PV()
        {
            InitializeComponent();
        }

        private void CloseProgram(object sender, EventArgs e) => Environment.Exit(0);

        private void Loader_Click(object sender, RoutedEventArgs e)
        {
            Objects.Items.Clear();
            Properties.Items.Clear();
            MainPointer = 0;
            if (runLoop != null)
                runLoop.Dispose();
            runLoop = new Task(RunLoop);
            runLoop.Start();
        }

        public async void RunLoop()
        {
            await Create();
            if (MainPointer <= 0)
                return;
            previousFrame = -1;
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                stopwatch.Restart();
                //try
                {
                    CRunApp.RefreshData("base+" + MainPointer.ToString("X"));
                    CRunHeader.RefreshData("base+" + (MainPointer + 8).ToString("X"));
                    if (CRunApp.CurrentFrame != -1)
                    {
                        if (previousFrame != CRunApp.CurrentFrame)
                        {
                            CRunApp = new CRunApp();
                            CRunApp.InitData("base+" + MainPointer.ToString("X"));
                            CRunFrame = new CRunFrame();
                            CRunFrame.InitData("base+" + (MainPointer + 4).ToString("X"));
                            CRunHeader = new CRunHeader();
                            CRunHeader.InitData("base+" + (MainPointer + 8).ToString("X"));
                            FrameNames[CRunApp.CurrentFrame] = CRunFrame.Name;
                            CreateList();
                            previousFrame = CRunApp.CurrentFrame;
                        }
                        CRunFrame.RefreshData("base+" + (MainPointer + 4).ToString("X"));
                        RefreshList();
                    }
                }
                //catch { }
                Thread.Sleep((int)(1000.0 / CRunApp.FramesPerSecond) - (int)(stopwatch.ElapsedMilliseconds % (1000.0 / CRunApp.FramesPerSecond)));
            }
        }

        public static byte[][] Headers = new byte[][]
        {
            Encoding.ASCII.GetBytes("PAMU"),
            Encoding.ASCII.GetBytes("PAME"),
            new byte[]{201, 125, 39, 4}
        };

        public async Task Create()
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

            for (int i = 0; i < Headers.GetLength(0); i++)
            {
                await FindMV(Headers[i]);
                if (MainPointer > 0)
                    break;
            }
            if (MainPointer <= 0)
                return;

            //try
            {
                CRunApp = new CRunApp();
                CRunApp.InitData("base+" + MainPointer.ToString("X"));
            }
            //catch { }
            //try
            {
                CRunHeader = new CRunHeader();
                CRunHeader.InitData("base+" + (MainPointer + 8).ToString("X"));
            }
            //catch { }
            FrameNames = new string[CRunApp.FrameCount];
            for (int i = 0; i < CRunApp.FrameCount; i++)
                FrameNames[i] = "* Frame " + (i + 1);
        }

        public void CreateList()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                Objects.Items.Clear();
                Objects.Items.Add(CRunApp.GetListPanel());
            }));
        }

        public void RefreshList()
        {
            if (Objects.Items.Count == 0)
                return;
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                CRunApp.RefreshListPanel(ref Objects);
            }));
        }

        private PropertyPanel currentLPParent;
        public void UpdateListPanel(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = (TreeView)sender;
            TreeViewItem selItem = (TreeViewItem)treeView.SelectedItem;
            if (selItem == null) return;
            TagHeader tag = (TagHeader)selItem.Tag;

            currentLPParent = tag.Parent();
            List<TreeViewItem> panel = currentLPParent.GetPanel();
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                Properties.Items.Clear();
                foreach (TreeViewItem item in panel)
                    Properties.Items.Add(item);
            }));
        }

        public void RefreshListPanel()
        {
            if (currentLPParent == null) return;
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                currentLPParent.RefreshPanel(ref Properties);
            }));
        }

        public async Task FindMV(byte[] header)
        {
            var baseAddress = MemLib.mProc.MainModule.BaseAddress.ToInt64();

            string search = string.Empty;

            foreach (byte b in header)
            {
                if (b.ToString("X") == "0")
                    search += "00 ";
                else if (b.ToString("X").Length == 1)
                    search += $"0{b.ToString("X")} ";
                else
                    search += $"{b.ToString("X")} ";
            }

            IEnumerable<long> HeaderSearch = await MemLib.AoBScan(search.Trim(), true, true);
            
            long Header = 0;
            foreach (long result in HeaderSearch)
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
            string HeaderBytes = "";
            foreach (byte b in BitConverter.GetBytes((int)Header))
            {
                if (b.ToString("X") == "0")
                    HeaderBytes += "00 ";
                else if (b.ToString("X").Length == 1)
                    HeaderBytes += $"0{b.ToString("X")} ";
                else
                    HeaderBytes += $"{b.ToString("X")} ";
            }
            IEnumerable<long> PointerSearch = await MemLib.AoBScan(HeaderBytes.Trim(), true, true);

            long Pointer = 0;
            foreach (long pointer in PointerSearch)
            {
                long i = pointer - baseAddress;
                if (i > 1048576 || i < 0)
                    continue;
                byte[] output = MemLib.ReadBytes("base+" + i.ToString("X") + ", 0x0", 4);
                if (output.SequenceEqual(header))
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

        public static void CloseGame(object sender, RoutedEventArgs e)
        {
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "0");
        }

        public static void NextFrame(object sender, RoutedEventArgs e)
        {
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "1");
        }

        public static void PreviousFrame(object sender, RoutedEventArgs e)
        {
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "2");
        }

        public static void JumpToFrame(object sender, RoutedEventArgs e)
        {
            int FrameID = short.Parse((sender as MenuItem).Tag.ToString()) | unchecked((short)0x8000);

            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "3");
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x38", "int", FrameID.ToString());
        }

        public static void RestartGame(object sender, RoutedEventArgs e)
        {
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "4");
        }

        public static void PauseGame(object sender, RoutedEventArgs e)
        {
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "5");
        }

        public static void RestartFrame(object sender, RoutedEventArgs e)
        {
            MemLib.WriteMemory("base+" + (MainPointer + 8).ToString("X") + ", 0x30", "int", "101");
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

    public static class ByteFlag
    {
        public static bool GetFlag(uint flagbyte, int pos)
        {
            uint mask = (uint)(1 << pos);
            uint result = flagbyte & mask;
            return result == mask;
        }
    }
    public class BitDict
    {
        public string[] Keys;
        public uint flag { get; set; }

        public BitDict(string[] keys) => Keys = keys;
        public bool this[string key]
        {
            get => GetFlag(key);
            set => SetFlag(key, value);
        }

        public bool GetFlag(string key)
        {
            int pos = Array.IndexOf(Keys, key);
            if (pos >= 0)
                return (flag & ((uint)Math.Pow(2, pos))) != 0;
            return false;
        }

        public void SetFlag(string key, bool value)
        {
            if (value)
                flag |= (uint)Math.Pow(2, Array.IndexOf(Keys, key));
            else
                flag &= ~(uint)Math.Pow(2, Array.IndexOf(Keys, key));
        }

        public override string ToString()
        {
            Dictionary<string, bool> actualKeys = new Dictionary<string, bool>();
            foreach (var key in Keys)
                actualKeys[key] = this[key];
            return string.Join(";\n", actualKeys.Select(kv => kv.Key + "=" + kv.Value).ToArray());
        }
    }
}
