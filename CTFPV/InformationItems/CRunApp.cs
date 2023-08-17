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
using CTFPV.Miscellaneous;
using System.Collections.Generic;
using ColorPicker;
using ColorPicker.Models;
using CTFPV.InformationItems;

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
        public BitDict Flags = new BitDict(new string[]
        {
            "Maximized Border",
            "No Heading",
            "Panic",
            "Machine Independent Speed",
            "Stretch",
            "", "",
            "Menu Hidden",
            "Menu Bar",
            "Maximized",
            "Mix Samples",
            "Fullscreen at Start",
            "Togglable Fullscreen",
            "Protected",
            "Copyright",
            "OneFile"
        });
        public BitDict NewFlags = new BitDict(new string[]
        {
            "Samples Over Frames",
            "Relocate Files",
            "Run Frame",
            "Play Samples When Unfocused",
            "No Minimize Box",
            "No Maximize Box",
            "No Thick Frame",
            "Do Not Center Frame",
            "Don't Auto-Stop Screensaver",
            "Disable Close",
            "Hidden At Start",
            "XP Visual Theme Support",
            "VSync",
            "Run When Minimized",
            "MDI",
            "Run While Resizing"
        });
        public ushort Mode;
        public BitDict OtherFlags = new BitDict(new string[]
        {
            "Debugger Shortcuts",
            "Debugger Draw",
            "Debugger Draw VRam",
            "DontShareSubData",
            "Auto Image Filter",
            "Auto Sound Filter",
            "All In One",
            "Show Debugger",
            "Java Swing",
            "Java Applet",
            "", "", "", "",
            "Direct3D9or11",
            "Direct3D8or11"
        });
        public Size WindowSize;
        public System.Drawing.Color BorderColor;
        public int FramesPerSecond;

        // Information
        public string Name = string.Empty;
        public string FilePath = string.Empty;
        public string EditorFilePath = string.Empty;
        public string Copyright = string.Empty;
        public string About = string.Empty;
        public string TargetFilePath = string.Empty;
        public int FrameCount;

        // Objects
        public int ObjectInfoMaxIndex;
        public int ObjectInfoMaxHandle;
        public ushort[] ObjectInfoHandleToIndex = new ushort[0];
        public CRunObjectInfo[] ObjectInfos = new CRunObjectInfo[0];

        // Runtime Info
        public int RunState;
        public int StartFrame;
        public int NextFrame;
        public int CurrentFrame;
        public BitDict OldFlags = new BitDict(new string[]
        {
            ""
        });
        public BitDict AppRunFlags = new BitDict(new string[]
        {
            "Menu Initialized",
            "Menu Images Loaded",
            "In-Game Loop",
            "Pause Before Modal Loop"
        });

        // Globals
        public int GlobalValueCount;
        public CRunValue[] GlobalValues = new CRunValue[0];
        public int GlobalStringCount;
        public string[] GlobalStrings = new string[0];

        // Extended Header
        public BitDict Options = new BitDict(new string[]
        {
            "1"
        });
        public int BuildType;
        public BitDict BuildFlags = new BitDict(new string[]
        {
            "1"
        });

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // App Mini Header
            Header = PV.MemLib.ReadString(parentPointer + ", 0x0", length: 4, stringEncoding: Encoding.ASCII);
            Version = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x4");
            SubVersion = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x6");
            ProductVersion = PV.MemLib.ReadInt(parentPointer + ", 0x8");
            ProductBuild = PV.MemLib.ReadInt(parentPointer + ", 0xC");

            // App Header
            Flags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x14");
            NewFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x16");
            Mode = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x18");
            OtherFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x1A");
            WindowSize = new Size(PV.MemLib.Read2Byte(parentPointer + ", 0x1C"), PV.MemLib.Read2Byte(parentPointer + ", 0x1E"));
            BorderColor = PV.MemLib.ReadColor(parentPointer + ", 0x70");
            FramesPerSecond = PV.MemLib.ReadInt(parentPointer + ", 0x78");

            // Information
            Name = PV.MemLib.ReadString(parentPointer + ", 0x80, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            FilePath = PV.MemLib.ReadString(parentPointer + ", 0x84, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            EditorFilePath = PV.MemLib.ReadString(parentPointer + ", 0x88, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            Copyright = PV.MemLib.ReadString(parentPointer + ", 0x8C, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            About = PV.MemLib.ReadString(parentPointer + ", 0x90, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            TargetFilePath = PV.MemLib.ReadString(parentPointer + ", 0x94, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            FrameCount = PV.MemLib.ReadInt(parentPointer + ", 0xC4");

            // Objects
            ObjectInfoMaxIndex = PV.MemLib.ReadInt(parentPointer + ", 0x190");
            ObjectInfoMaxHandle = PV.MemLib.ReadInt(parentPointer + ", 0x194");
            ObjectInfoHandleToIndex = new ushort[ObjectInfoMaxHandle];
            ObjectInfos = new CRunObjectInfo[ObjectInfoMaxIndex];

            for (int i = 0; i < ObjectInfoMaxHandle; i++)
                ObjectInfoHandleToIndex[i] = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x198, 0x" + (i * 2).ToString("X"));

            for (int i = 0; i < ObjectInfoMaxIndex; i++)
            {
                ObjectInfos[i] = new CRunObjectInfo();
                ObjectInfos[i].InitData(parentPointer + ", 0x19C, 0x" + (i * 4).ToString("X"));
            }

            // Runtime Info
            RunState = PV.MemLib.ReadInt(parentPointer + ", 0x1E4");
            StartFrame = PV.MemLib.ReadInt(parentPointer + ", 0x1E8");
            NextFrame = PV.MemLib.ReadInt(parentPointer + ", 0x1EC");
            CurrentFrame = PV.MemLib.ReadInt(parentPointer + ", 0x1F0");
            OldFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x1F8");
            AppRunFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x1FA");

            // Globals
            GlobalValueCount = PV.MemLib.ReadInt(parentPointer + ", 0x264");
            GlobalValues = new CRunValue[GlobalValueCount];
            GlobalStringCount = PV.MemLib.ReadInt(parentPointer + ", 0x278");
            GlobalStrings = new string[GlobalStringCount];

            for (int i = 0; i < GlobalValueCount; i++)
            {
                GlobalValues[i] = new CRunValue();
                GlobalValues[i].ValueOffset = i * 16;
                GlobalValues[i].InitData(parentPointer + ", 0x268");
            }

            for (int i = 0; i < GlobalStringCount; i++)
                GlobalStrings[i] = PV.MemLib.ReadString(parentPointer + ", 0x27C, 0x" + (i * 4).ToString("X") + ", 0x0", length: 255, stringEncoding: Encoding.Unicode);

            // Extended Header
            Options.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x37C, 0x0");
            BuildType = PV.MemLib.ReadInt(parentPointer + ", 0x37C, 0x4");
            BuildFlags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x37C, 0x8");
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // App Header
            WindowSize = new Size(PV.MemLib.Read2Byte(parentPointer + ", 0x1C"), PV.MemLib.Read2Byte(parentPointer + ", 0x1E"));
            BorderColor = PV.MemLib.ReadColor(parentPointer + ", 0x70");
            FramesPerSecond = PV.MemLib.ReadInt(parentPointer + ", 0x78");

            // Objects
            for (int i = 0; i < ObjectInfoMaxIndex; i++)
                if (ObjectInfos[i] != null)
                    ObjectInfos[i].RefreshData(parentPointer + ", 0x19C, 0x" + (i * 4).ToString("X"));

            // Runtime Info
            RunState = PV.MemLib.ReadInt(parentPointer + ", 0x1E4");
            NextFrame = PV.MemLib.ReadInt(parentPointer + ", 0x1EC");
            CurrentFrame = PV.MemLib.ReadInt(parentPointer + ", 0x1F0");

            // Globals
            for (int i = 0; i < GlobalValueCount; i++)
                GlobalValues[i].RefreshData(parentPointer + ", 0x268");

            for (int i = 0; i < GlobalStringCount; i++)
                GlobalStrings[i] = PV.MemLib.ReadString(parentPointer + ", 0x27C, 0x" + (i * 4).ToString("X") + ", 0x0", length: 255, stringEncoding: Encoding.Unicode);
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // App Mini Header
            TreeViewItem appMiniHeaderPanel = Templates.Tab();
            ((appMiniHeaderPanel.Header as Grid).Children[0] as Label).Content = "App Mini-Header";

            // Header
            TreeViewItem headerPanel = Templates.Editbox();
            ((headerPanel.Header as Grid).Children[0] as Label).Content = "Header";
            ((headerPanel.Header as Grid).Children[1] as TextBox).Text = Header;
            ((headerPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (headerPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x0";
            appMiniHeaderPanel.Items.Add(headerPanel);

            // Version
            TreeViewItem versionPanel = Templates.Editbox();
            ((versionPanel.Header as Grid).Children[0] as Label).Content = "Version";
            ((versionPanel.Header as Grid).Children[1] as TextBox).Text = Version.ToString();
            ((versionPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (versionPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x4";
            appMiniHeaderPanel.Items.Add(versionPanel);

            // Sub-Version
            TreeViewItem subVersionPanel = Templates.Editbox();
            ((subVersionPanel.Header as Grid).Children[0] as Label).Content = "Sub-Version";
            ((subVersionPanel.Header as Grid).Children[1] as TextBox).Text = SubVersion.ToString();
            ((subVersionPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (subVersionPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x6";
            appMiniHeaderPanel.Items.Add(subVersionPanel);

            // Product Version
            TreeViewItem prodVersionPanel = Templates.Editbox();
            ((prodVersionPanel.Header as Grid).Children[0] as Label).Content = "Product Version";
            ((prodVersionPanel.Header as Grid).Children[1] as TextBox).Text = ProductVersion.ToString();
            ((prodVersionPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (prodVersionPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8";
            appMiniHeaderPanel.Items.Add(prodVersionPanel);

            // Product Build
            TreeViewItem prodBuildPanel = Templates.Editbox();
            ((prodBuildPanel.Header as Grid).Children[0] as Label).Content = "Product Build";
            ((prodBuildPanel.Header as Grid).Children[1] as TextBox).Text = ProductBuild.ToString();
            ((prodBuildPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (prodBuildPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xC";
            appMiniHeaderPanel.Items.Add(prodBuildPanel);

            panel.Add(appMiniHeaderPanel);

            // App Header
            TreeViewItem appHeaderPanel = Templates.Tab();
            ((appHeaderPanel.Header as Grid).Children[0] as Label).Content = "App Header";

            // Flags
            TreeViewItem flagsPanel = Templates.Tab(true);
            ((flagsPanel.Header as Grid).Children[0] as Label).Content = "Flags";
            (flagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x14";

            foreach (string key in Flags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem flagPanel = Templates.Checkbox(false);
                ((flagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((flagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Flags[key];
                (flagPanel.Tag as TagHeader).ParentFlags = Flags;
                (flagPanel.Tag as TagHeader).Flag = key;
                (flagPanel.Tag as TagHeader).ActionType = 1;
                (flagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x14";
                flagsPanel.Items.Add(flagPanel);
            }

            appHeaderPanel.Items.Add(flagsPanel);

            // New Flags
            TreeViewItem newFlagsPanel = Templates.Tab();
            ((newFlagsPanel.Header as Grid).Children[0] as Label).Content = "New Flags";
            (newFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x16";

            foreach (string key in NewFlags.Keys)
            {
                TreeViewItem newFlagPanel = Templates.Checkbox(false);
                ((newFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((newFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = NewFlags[key];
                (newFlagPanel.Tag as TagHeader).ParentFlags = NewFlags;
                (newFlagPanel.Tag as TagHeader).Flag = key;
                (newFlagPanel.Tag as TagHeader).ActionType = 1;
                (newFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x16";
                newFlagsPanel.Items.Add(newFlagPanel);
            }

            appHeaderPanel.Items.Add(newFlagsPanel);

            // Mode
            TreeViewItem modePanel = Templates.Editbox();
            ((modePanel.Header as Grid).Children[0] as Label).Content = "Mode";
            ((modePanel.Header as Grid).Children[1] as TextBox).Text = Mode.ToString();
            ((modePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (modePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x18";
            appHeaderPanel.Items.Add(modePanel);

            // Other Flags
            TreeViewItem otherFlagsPanel = Templates.Tab();
            ((otherFlagsPanel.Header as Grid).Children[0] as Label).Content = "Other Flags";
            (otherFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1A";

            foreach (string key in OtherFlags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem otherFlagPanel = Templates.Checkbox(false);
                ((otherFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((otherFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = OtherFlags[key];
                (otherFlagPanel.Tag as TagHeader).ParentFlags = OtherFlags;
                (otherFlagPanel.Tag as TagHeader).Flag = key;
                (otherFlagPanel.Tag as TagHeader).ActionType = 1;
                (otherFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1A";
                otherFlagsPanel.Items.Add(otherFlagPanel);
            }

            appHeaderPanel.Items.Add(otherFlagsPanel);

            // Window Width
            TreeViewItem windowWidthPanel = Templates.Editbox();
            ((windowWidthPanel.Header as Grid).Children[0] as Label).Content = "Window Width";
            ((windowWidthPanel.Header as Grid).Children[1] as TextBox).Text = WindowSize.Width.ToString();
            (windowWidthPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1C";
            (windowWidthPanel.Tag as TagHeader).ActionType = 1;
            appHeaderPanel.Items.Add(windowWidthPanel);

            // Window Height
            TreeViewItem windowHeightPanel = Templates.Editbox();
            ((windowHeightPanel.Header as Grid).Children[0] as Label).Content = "Window Height";
            ((windowHeightPanel.Header as Grid).Children[1] as TextBox).Text = WindowSize.Height.ToString();
            (windowHeightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1E";
            (windowHeightPanel.Tag as TagHeader).ActionType = 1;
            appHeaderPanel.Items.Add(windowHeightPanel);

            // Border Color
            TreeViewItem bColorPanel = Templates.ColorPicker();
            ((bColorPanel.Header as Grid).Children[0] as Label).Content = "Border Color";
            ((bColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_R = BorderColor.R;
            ((bColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_G = BorderColor.G;
            ((bColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_B = BorderColor.B;
            ((bColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.A = 255;
            ((bColorPanel.Header as Grid).Children[1] as PortableColorPicker).ShowAlpha = false;
            (bColorPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x70";
            appHeaderPanel.Items.Add(bColorPanel);

            // Frames Per Second
            TreeViewItem fpsPanel = Templates.Editbox();
            ((fpsPanel.Header as Grid).Children[0] as Label).Content = "FPS";
            ((fpsPanel.Header as Grid).Children[1] as TextBox).Text = FramesPerSecond.ToString();
            (fpsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x78";
            appHeaderPanel.Items.Add(fpsPanel);

            panel.Add(appHeaderPanel);

            // Information
            TreeViewItem infoPanel = Templates.Tab();
            ((infoPanel.Header as Grid).Children[0] as Label).Content = "Information";

            // Name
            TreeViewItem namePanel = Templates.Editbox();
            ((namePanel.Header as Grid).Children[0] as Label).Content = "Name";
            ((namePanel.Header as Grid).Children[1] as TextBox).Text = Name;
            ((namePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (namePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x80, 0x0";
            infoPanel.Items.Add(namePanel);

            // File Path
            TreeViewItem pathPanel = Templates.Editbox();
            ((pathPanel.Header as Grid).Children[0] as Label).Content = "File Path";
            ((pathPanel.Header as Grid).Children[1] as TextBox).Text = FilePath;
            ((pathPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (pathPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x84, 0x0";
            infoPanel.Items.Add(pathPanel);

            // Editor File Path
            TreeViewItem editorPathPanel = Templates.Editbox();
            ((editorPathPanel.Header as Grid).Children[0] as Label).Content = "Editor File Path";
            ((editorPathPanel.Header as Grid).Children[1] as TextBox).Text = EditorFilePath;
            ((editorPathPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (editorPathPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x88, 0x0";
            infoPanel.Items.Add(editorPathPanel);

            // Copyright
            TreeViewItem copyrightAbtPanel = Templates.Editbox();
            ((copyrightAbtPanel.Header as Grid).Children[0] as Label).Content = "Copyright";
            ((copyrightAbtPanel.Header as Grid).Children[1] as TextBox).Text = Copyright;
            ((copyrightAbtPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (copyrightAbtPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8C, 0x0";
            infoPanel.Items.Add(copyrightAbtPanel);

            // About
            TreeViewItem aboutPanel = Templates.Editbox();
            ((aboutPanel.Header as Grid).Children[0] as Label).Content = "About";
            ((aboutPanel.Header as Grid).Children[1] as TextBox).Text = About;
            ((aboutPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (aboutPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x90, 0x0";
            infoPanel.Items.Add(aboutPanel);

            // Target File Path
            TreeViewItem targetPathPanel = Templates.Editbox();
            ((targetPathPanel.Header as Grid).Children[0] as Label).Content = "Target File Path";
            ((targetPathPanel.Header as Grid).Children[1] as TextBox).Text = TargetFilePath;
            ((targetPathPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (targetPathPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x94, 0x0";
            infoPanel.Items.Add(targetPathPanel);

            // Frame Count
            TreeViewItem frameCntPanel = Templates.Editbox();
            ((frameCntPanel.Header as Grid).Children[0] as Label).Content = "Frame Count";
            ((frameCntPanel.Header as Grid).Children[1] as TextBox).Text = FrameCount.ToString();
            ((frameCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (frameCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xC4";
            infoPanel.Items.Add(frameCntPanel);

            panel.Add(infoPanel);

            // Runtime Info
            TreeViewItem runInfoPanel = Templates.Tab();
            ((runInfoPanel.Header as Grid).Children[0] as Label).Content = "Runtime Info";

            // Run State
            TreeViewItem runStatePanel = Templates.Editbox();
            ((runStatePanel.Header as Grid).Children[0] as Label).Content = "Run State";
            ((runStatePanel.Header as Grid).Children[1] as TextBox).Text = RunState.ToString();
            (runStatePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1E4";
            runInfoPanel.Items.Add(runStatePanel);

            // Start Frame
            TreeViewItem startFramePanel = Templates.Editbox();
            ((startFramePanel.Header as Grid).Children[0] as Label).Content = "Start Frame ID";
            ((startFramePanel.Header as Grid).Children[1] as TextBox).Text = StartFrame.ToString();
            (startFramePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1E8";
            runInfoPanel.Items.Add(startFramePanel);

            // Next Frame
            TreeViewItem nextFramePanel = Templates.Editbox();
            ((nextFramePanel.Header as Grid).Children[0] as Label).Content = "Next Frame ID";
            ((nextFramePanel.Header as Grid).Children[1] as TextBox).Text = NextFrame.ToString();
            ((nextFramePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (nextFramePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1EC";
            runInfoPanel.Items.Add(nextFramePanel);

            // Current Frame
            TreeViewItem curFramePanel = Templates.Editbox();
            ((curFramePanel.Header as Grid).Children[0] as Label).Content = "Current Frame ID";
            ((curFramePanel.Header as Grid).Children[1] as TextBox).Text = CurrentFrame.ToString();
            (curFramePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F0";
            runInfoPanel.Items.Add(curFramePanel);

            // Old Flags
            TreeViewItem oldFlagsPanel = Templates.Tab(true);
            ((oldFlagsPanel.Header as Grid).Children[0] as Label).Content = "Old Flags";
            (oldFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F8";
            runInfoPanel.Items.Add(oldFlagsPanel);

            // App Run Flags
            TreeViewItem appRunFlagsPanel = Templates.Tab(true);
            ((appRunFlagsPanel.Header as Grid).Children[0] as Label).Content = "App Run Flags";
            (appRunFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1FA";

            foreach (string key in AppRunFlags.Keys)
            {
                TreeViewItem appRunFlagPanel = Templates.Checkbox(false);
                ((appRunFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((appRunFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = AppRunFlags[key];
                (appRunFlagPanel.Tag as TagHeader).ParentFlags = AppRunFlags;
                (appRunFlagPanel.Tag as TagHeader).Flag = key;
                (appRunFlagPanel.Tag as TagHeader).ActionType = 1;
                (appRunFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1FA";
                appRunFlagsPanel.Items.Add(appRunFlagPanel);
            }

            runInfoPanel.Items.Add(appRunFlagsPanel);

            panel.Add(runInfoPanel);

            // Global Values
            TreeViewItem gblValsPanel = Templates.Tab(true);
            ((gblValsPanel.Header as Grid).Children[0] as Label).Content = $"Global Values ({GlobalValueCount})";
            (gblValsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x268";

            int gblValID = 0;
            foreach (var gblVal in GlobalValues)
            {
                // Global Value
                TreeViewItem gblValPanel = Templates.Editbox();
                ((gblValPanel.Header as Grid).Children[0] as Label).Content = "Global Value " + gblValID;
                ((gblValPanel.Header as Grid).Children[1] as TextBox).Text = gblVal.Value().ToString();
                (gblValPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x268, 0x" + ((gblValID++ * 16) + 8).ToString("X");

                switch (gblVal.Type)
                {
                    case 1:
                        (gblValPanel.Tag as TagHeader).ActionType = 5;
                        break;
                    case 2:
                        (gblValPanel.Tag as TagHeader).ActionType = 4;
                        break;
                }
                gblValsPanel.Items.Add(gblValPanel);
            }

            panel.Add(gblValsPanel);

            // Global Strings
            TreeViewItem gblStrsPanel = Templates.Tab(true);
            ((gblStrsPanel.Header as Grid).Children[0] as Label).Content = $"Global Strings ({GlobalStringCount})";
            (gblStrsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x27C";

            int gblStrID = 0;
            foreach (var gblStr in GlobalStrings)
            {
                // Global String
                TreeViewItem gblStrPanel = Templates.Editbox();
                ((gblStrPanel.Header as Grid).Children[0] as Label).Content = "Global String " + gblStrID;
                ((gblStrPanel.Header as Grid).Children[1] as TextBox).Text = gblStr;
                (gblStrPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x27C, 0x" + (gblStrID++ * 4).ToString("X") + ", 0x0";
                (gblStrPanel.Tag as TagHeader).ActionType = 5;
                gblStrsPanel.Items.Add(gblStrPanel);
            }

            panel.Add(gblStrsPanel);

            // Extended Header
            TreeViewItem extHeaderPanel = Templates.Tab(true);
            ((extHeaderPanel.Header as Grid).Children[0] as Label).Content = "Extended Header";
            (extHeaderPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x37C";

            // Options
            TreeViewItem optionsPanel = Templates.Tab(true);
            ((optionsPanel.Header as Grid).Children[0] as Label).Content = "Options";
            (optionsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x37C, 0x0";
            extHeaderPanel.Items.Add(optionsPanel);

            // Build Type
            TreeViewItem buildTypePanel = Templates.Editbox();
            ((buildTypePanel.Header as Grid).Children[0] as Label).Content = "Build Type";
            ((buildTypePanel.Header as Grid).Children[1] as TextBox).Text = BuildType.ToString();
            ((buildTypePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (buildTypePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x37C, 0x4";
            extHeaderPanel.Items.Add(buildTypePanel);

            // Build Flags
            TreeViewItem buildFlagsPanel = Templates.Tab(true);
            ((buildFlagsPanel.Header as Grid).Children[0] as Label).Content = "Build Flags";
            (buildFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x37C, 0x8";
            extHeaderPanel.Items.Add(buildFlagsPanel);

            panel.Add(extHeaderPanel);
            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

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

            TagHeader tag = new TagHeader();
            tag.Parent = this;
            tag.Pointer = latestParentPointer.Replace("base", $"\"{PV.MemLib.mProc.MainModule.ModuleName}\"");
            appItem.ContextMenu = tag.GetMenu();
            appItem.Tag = tag;
            appItem.Header = header;
            appItem.IsExpanded = true;
            appItem.Items.Add(PV.CRunFrame.GetListPanel());

            MenuItem close = new MenuItem();
            close.Header = "Close Application";
            close.Click += PV.CloseGame;
            appItem.ContextMenu.Items.Add(close);

            MenuItem restart = new MenuItem();
            restart.Header = "Restart Application";
            restart.Click += PV.RestartGame;
            appItem.ContextMenu.Items.Add(restart);

            MenuItem pause = new MenuItem();
            pause.Header = "Pause Application";
            pause.Click += PV.PauseGame;
            appItem.ContextMenu.Items.Add(pause);

            MenuItem restartfrm = new MenuItem();
            restartfrm.Header = "Restart Frame";
            restartfrm.Click += PV.RestartFrame;
            appItem.ContextMenu.Items.Add(restartfrm);

            MenuItem prevfrm = new MenuItem();
            prevfrm.Header = "Previous Frame";
            prevfrm.Click += PV.PreviousFrame;
            appItem.ContextMenu.Items.Add(prevfrm);

            MenuItem nextfrm = new MenuItem();
            nextfrm.Header = "Next Frame";
            nextfrm.Click += PV.NextFrame;
            appItem.ContextMenu.Items.Add(nextfrm);

            MenuItem jumper = new MenuItem();
            jumper.Header = "Jump to Frame";
            for (int i = 0; i < PV.CRunApp.FrameCount; i++)
            {
                MenuItem jump = new MenuItem();
                jump.Header = PV.FrameNames[i];
                jump.Tag = i;
                jump.Click += PV.JumpToFrame;
                jumper.Items.Add(jump);
            }
            appItem.ContextMenu.Items.Add(jumper);

            return appItem;
        }

        public void RefreshListPanel(ref TreeView treeView)
        {
            PV.CRunFrame.RefreshListPanel(ref treeView);
        }
    }
}
