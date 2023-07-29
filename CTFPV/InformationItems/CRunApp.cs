using CTFPV;
using CTFPV;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using Bitmap = System.Drawing.Bitmap;
using System.Windows;
using System.Windows.Media;
using ColorConverter = System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;
using Size = System.Windows.Size;

namespace Encryption_Key_Finder.InformationItems
{
    public class CRunApp : PropertyPanel
    {
        // App Mini Header
        public string Header = "PAMU";
        public ushort Version;
        public ushort SubVersion;
        public int ProductVersion;
        public int ProductBuild;

        // App Header
        public ushort Flags;
        public ushort NewFlags;
        public ushort Mode;
        public ushort OtherFlags;
        public Size WindowSize;
        public System.Drawing.Color BorderColor;
        public int FramesPerSecond;

        // Information
        public string Name = string.Empty;
        public string FilePath = string.Empty;

        // Frames
        public int FrameMaxIndex;
        public int FrameMaxHandle;
        public ushort[] FrameHandleToIndex;
        public int[] FrameOffsets;
        public string[] FramePasswords;

        // Objects
        public int ObjectInfoMaxIndex;
        public int ObjectInfoMaxHandle;
        public ushort[] ObjectInfoHandleToIndex;
        //public ObjectInfo[] ObjectInfos;

        // Runtime Info
        public int RunState;
        public int StartFrame;
        public int NextFrame;
        public int CurrentFrame;
        public ushort OldFlags;
        public ushort AppRunFlags;

        public override void InitData()
        {
            Header = PV.MemLib.ReadString(PV.BasePointer + ", 0x0", length: 4, stringEncoding: Encoding.ASCII);
            Version = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x4");
            SubVersion = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x6");
            ProductVersion = PV.MemLib.ReadInt(PV.BasePointer + ", 0x8");
            ProductBuild = PV.MemLib.ReadInt(PV.BasePointer + ", 0xC");

            Flags = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x14");
            NewFlags = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x16");
            Mode = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x18");
            OtherFlags = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x1A");
            WindowSize = new Size(PV.MemLib.Read2Byte(PV.BasePointer + ", 0x1C"), PV.MemLib.Read2Byte(PV.BasePointer + ", 0x1E"));
            BorderColor = System.Drawing.Color.FromArgb(PV.MemLib.ReadInt(PV.BasePointer + ", 0x70"));
            FramesPerSecond = PV.MemLib.ReadInt(PV.BasePointer + ", 0x78");

            Name = PV.MemLib.ReadString(PV.BasePointer + ", 0x80, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            FilePath = PV.MemLib.ReadString(PV.BasePointer + ", 0x84, 0x0", length: 255, stringEncoding: Encoding.Unicode);

            FrameMaxIndex = PV.MemLib.ReadInt(PV.BasePointer + ", 0xC4");
            FrameMaxHandle = PV.MemLib.ReadInt(PV.BasePointer + ", 0xC8");
            FrameHandleToIndex = new ushort[0];
            FrameOffsets = new int[0];
            FramePasswords = new string[0];

            if (PV.MemLib.ReadInt(PV.BasePointer + ", 0xCC") > 0)
            {
                FrameHandleToIndex = new ushort[FrameMaxHandle];
                for (int i = 0; i < FrameMaxHandle; i++)
                    FrameHandleToIndex[i] = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0xCC, 0x" + (i * 2).ToString("X"));
            }

            if (PV.MemLib.ReadInt(PV.BasePointer + ", 0xD0") > 0)
            {
                FrameOffsets = new int[FrameMaxIndex];
                for (int i = 0; i < FrameMaxIndex; i++)
                    FrameOffsets[i] = PV.MemLib.ReadInt(PV.BasePointer + ", 0xD0, 0x" + (i * 4).ToString("X"));
            }

            if (PV.MemLib.ReadInt(PV.BasePointer + ", 0xD4") > 0)
            {
                FramePasswords = new string[FrameMaxIndex];
                for (int i = 0; i < FrameMaxIndex; i++)
                    FramePasswords[i] = PV.MemLib.ReadString(PV.BasePointer + ", 0xD4, 0x" + (i * 4).ToString("X") + ", 0x0", length: 255, stringEncoding: Encoding.Unicode);
            }

            ObjectInfoMaxIndex = PV.MemLib.ReadInt(PV.BasePointer + ", 0x190");
            ObjectInfoMaxHandle = PV.MemLib.ReadInt(PV.BasePointer + ", 0x194");
            ObjectInfoHandleToIndex = new ushort[0];
            //ObjectInfos

            if (PV.MemLib.ReadInt(PV.BasePointer + ", 0x198") > 0)
            {
                ObjectInfoHandleToIndex = new ushort[ObjectInfoMaxHandle];
                for (int i = 0; i < ObjectInfoMaxHandle; i++)
                    ObjectInfoHandleToIndex[i] = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x198, 0x" + (i * 2).ToString("X"));
            }

            RunState = PV.MemLib.ReadInt(PV.BasePointer + ", 0x1E4");
            StartFrame = PV.MemLib.ReadInt(PV.BasePointer + ", 0x1E8");
            NextFrame = PV.MemLib.ReadInt(PV.BasePointer + ", 0x1EC");
            CurrentFrame = PV.MemLib.ReadInt(PV.BasePointer + ", 0x1F0");
            OldFlags = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x1F8");
            AppRunFlags = (ushort)PV.MemLib.Read2Byte(PV.BasePointer + ", 0x1FA");
        }

        public override void RefreshData()
        {

        }

        public override TreeView GetPanel()
        {
            TreeView panel = new TreeView();
            return panel;
        }

        public TreeViewItem GetListPanel()
        {
            TreeViewItem appItem = new TreeViewItem();
            Grid header = new Grid();

            Icon appIcon = Icon.ExtractAssociatedIcon(FilePath);
            Image icon = new Image();
            icon.Source = appIcon.ToBitmap().ToImage();
            icon.Width = 15;
            icon.Height = 15;
            icon.HorizontalAlignment = HorizontalAlignment.Left;
            header.Children.Add(icon);

            Label name = new Label();
            name.Content = Name;
            name.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE7FF"));
            name.HorizontalAlignment = HorizontalAlignment.Left;
            name.VerticalAlignment = VerticalAlignment.Center;
            name.HorizontalContentAlignment = HorizontalAlignment.Center;
            name.FontSize = 10;
            name.Padding = new Thickness(0);
            name.Margin = new Thickness(18, 0, 0, 0);
            header.Children.Add(name);

            appItem.Header = header;
            appItem.IsExpanded = true;
            return appItem;
        }
    }
}
