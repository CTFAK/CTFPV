using CTFPV;
using CTFPV.InformationItems;
using CTFPV.Miscellaneous;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Windows.Controls;

namespace Encryption_Key_Finder.InformationItems
{
    public class CRunHeader : PropertyPanel
    {
        // Game Header
        public byte PlayerCount;
        public byte MousePlayers;
        public BitDict GameFlags = new BitDict(new string[]
        {
            "",
            "VBL Independent",
            "Limited Scroll",
            "",
            "First Loop Fade In",
            "Load On Call",
            "Real Game",
            "Play",
            "",
            "Initializing"
        });
        public int CurrentPlayer;

        // Jumper
        public short Quit;
        public short ScrollingQuit;
        public int QuitParameter;

        // Objects
        public int ObjectCount;
        public int MaxObjects;

        // Other
        public int WindowWidth;
        public int WindowHeight;
        public int WindowX;
        public int WindowY;
        public int Timer;
        public int MouseX;
        public int MouseY;
        public int MouseClientX;
        public int MouseClientY;
        public short MouseClick;
        public CRunMVHeader CRunMVHeader;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Game Header
            PlayerCount = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x26");
            MousePlayers = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x27");
            GameFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x28");
            CurrentPlayer = PV.MemLib.ReadInt(parentPointer + ", 0x2C");

            // Jumper
            Quit = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x30");
            ScrollingQuit = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x32");
            QuitParameter = PV.MemLib.ReadInt(parentPointer + ", 0x38");

            // Objects
            ObjectCount = PV.MemLib.ReadInt(parentPointer + ", 0x3C");
            MaxObjects = PV.MemLib.ReadInt(parentPointer + ", 0x40");

            // Window
            WindowWidth = PV.MemLib.ReadInt(parentPointer + ", 0xA8");
            WindowHeight = PV.MemLib.ReadInt(parentPointer + ", 0xAC");
            WindowX = PV.MemLib.ReadInt(parentPointer + ", 0xB0");
            WindowY = PV.MemLib.ReadInt(parentPointer + ", 0xB4");

            // Mouse
            MouseX = PV.MemLib.ReadInt(parentPointer + ", 0x150");
            MouseY = PV.MemLib.ReadInt(parentPointer + ", 0x154");
            MouseClientX = PV.MemLib.ReadInt(parentPointer + ", 0x158");
            MouseClientY = PV.MemLib.ReadInt(parentPointer + ", 0x15C");
            MouseClick = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x160");

            // Other
            Timer = PV.MemLib.ReadInt(parentPointer + ", 0xD4");
            CRunMVHeader = new CRunMVHeader();
            CRunMVHeader.InitData(parentPointer + ", 0x4F4");
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Game Header
            GameFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x28");
            CurrentPlayer = PV.MemLib.ReadInt(parentPointer + ", 0x2C");

            // Jumper
            Quit = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x30");
            ScrollingQuit = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x32");
            QuitParameter = PV.MemLib.ReadInt(parentPointer + ", 0x38");

            // Objects
            ObjectCount = PV.MemLib.ReadInt(parentPointer + ", 0x3C");

            // Window
            WindowWidth = PV.MemLib.ReadInt(parentPointer + ", 0xA8");
            WindowHeight = PV.MemLib.ReadInt(parentPointer + ", 0xAC");
            WindowX = PV.MemLib.ReadInt(parentPointer + ", 0xB0");
            WindowY = PV.MemLib.ReadInt(parentPointer + ", 0xB4");

            // Mouse
            MouseX = PV.MemLib.ReadInt(parentPointer + ", 0x150");
            MouseY = PV.MemLib.ReadInt(parentPointer + ", 0x154");
            MouseClientX = PV.MemLib.ReadInt(parentPointer + ", 0x158");
            MouseClientY = PV.MemLib.ReadInt(parentPointer + ", 0x15C");
            MouseClick = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x160");

            // Other
            Timer = PV.MemLib.ReadInt(parentPointer + ", 0xD4");
            CRunMVHeader.RefreshData(parentPointer + ", 0x4F4");
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Game Header
            TreeViewItem gameHeaderPanel = Templates.Tab();
            ((gameHeaderPanel.Header as Grid).Children[0] as Label).Content = "Game Header";

            // Player Count
            TreeViewItem plyrCntPanel = Templates.Editbox();
            ((plyrCntPanel.Header as Grid).Children[0] as Label).Content = "Player Count";
            ((plyrCntPanel.Header as Grid).Children[1] as TextBox).Text = PlayerCount.ToString();
            ((plyrCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (plyrCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x26";
            gameHeaderPanel.Items.Add(plyrCntPanel);

            // Mouse Players
            TreeViewItem mousePlyrsPanel = Templates.Editbox();
            ((mousePlyrsPanel.Header as Grid).Children[0] as Label).Content = "Players using Mouse";
            ((mousePlyrsPanel.Header as Grid).Children[1] as TextBox).Text = MousePlayers.ToString();
            ((mousePlyrsPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (mousePlyrsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x27";
            gameHeaderPanel.Items.Add(mousePlyrsPanel);

            // Game Flags
            TreeViewItem gameFlagsPanel = Templates.Tab(true);
            ((gameFlagsPanel.Header as Grid).Children[0] as Label).Content = "Game Flags";
            (gameFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x28";

            foreach (string key in GameFlags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem gameFlagPanel = Templates.Checkbox(false);
                ((gameFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((gameFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = GameFlags[key];
                (gameFlagPanel.Tag as TagHeader).ParentFlags = GameFlags;
                (gameFlagPanel.Tag as TagHeader).Flag = key;
                (gameFlagPanel.Tag as TagHeader).ActionType = 1;
                (gameFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x28";
                gameFlagsPanel.Items.Add(gameFlagPanel);
            }

            gameHeaderPanel.Items.Add(gameFlagsPanel);

            // Current Player
            TreeViewItem curPlyrPanel = Templates.Editbox();
            ((curPlyrPanel.Header as Grid).Children[0] as Label).Content = "Current Player";
            ((curPlyrPanel.Header as Grid).Children[1] as TextBox).Text = CurrentPlayer.ToString();
            (curPlyrPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x2C";
            gameHeaderPanel.Items.Add(curPlyrPanel);

            panel.Add(gameHeaderPanel);

            // Jumper
            TreeViewItem jumperPanel = Templates.Tab();
            ((jumperPanel.Header as Grid).Children[0] as Label).Content = "Jumper";

            // Quit
            TreeViewItem quitPanel = Templates.Editbox();
            ((quitPanel.Header as Grid).Children[0] as Label).Content = "Quit";
            ((quitPanel.Header as Grid).Children[1] as TextBox).Text = Quit.ToString();
            (quitPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x30";
            (quitPanel.Tag as TagHeader).ActionType = 1;
            jumperPanel.Items.Add(quitPanel);

            // Scrolling Quit
            TreeViewItem scrollingQuitPanel = Templates.Editbox();
            ((scrollingQuitPanel.Header as Grid).Children[0] as Label).Content = "Scrolling Quit";
            ((scrollingQuitPanel.Header as Grid).Children[1] as TextBox).Text = ScrollingQuit.ToString();
            (scrollingQuitPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x32";
            (scrollingQuitPanel.Tag as TagHeader).ActionType = 1;
            jumperPanel.Items.Add(scrollingQuitPanel);

            // Quit Parameter
            TreeViewItem quitParamPanel = Templates.Editbox();
            ((quitParamPanel.Header as Grid).Children[0] as Label).Content = "Quit Parameter";
            ((quitParamPanel.Header as Grid).Children[1] as TextBox).Text = QuitParameter.ToString();
            (quitParamPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x38";
            jumperPanel.Items.Add(quitParamPanel);

            panel.Add(jumperPanel);

            // Objects
            TreeViewItem objsPanel = Templates.Tab();
            ((objsPanel.Header as Grid).Children[0] as Label).Content = "Objects";

            // Object Count
            TreeViewItem objCntPanel = Templates.Editbox();
            ((objCntPanel.Header as Grid).Children[0] as Label).Content = "Object Count";
            ((objCntPanel.Header as Grid).Children[1] as TextBox).Text = ObjectCount.ToString();
            ((objCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (objCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x3C";
            objsPanel.Items.Add(objCntPanel);

            // Max Objects
            TreeViewItem objMaxPanel = Templates.Editbox();
            ((objMaxPanel.Header as Grid).Children[0] as Label).Content = "Max Objects";
            ((objMaxPanel.Header as Grid).Children[1] as TextBox).Text = MaxObjects.ToString();
            (objMaxPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x40";
            objsPanel.Items.Add(objMaxPanel);

            panel.Add(objsPanel);

            // Window
            TreeViewItem wndPanel = Templates.Tab();
            ((wndPanel.Header as Grid).Children[0] as Label).Content = "Window";

            // Window Width
            TreeViewItem wndWidthPanel = Templates.Editbox();
            ((wndWidthPanel.Header as Grid).Children[0] as Label).Content = "Window Width";
            ((wndWidthPanel.Header as Grid).Children[1] as TextBox).Text = WindowWidth.ToString();
            (wndWidthPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xA8";
            wndPanel.Items.Add(wndWidthPanel);

            // Window Height
            TreeViewItem wndHeightPanel = Templates.Editbox();
            ((wndHeightPanel.Header as Grid).Children[0] as Label).Content = "Window Height";
            ((wndHeightPanel.Header as Grid).Children[1] as TextBox).Text = WindowHeight.ToString();
            (wndHeightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xAC";
            wndPanel.Items.Add(wndHeightPanel);

            // Window X
            TreeViewItem wndXPanel = Templates.Editbox();
            ((wndXPanel.Header as Grid).Children[0] as Label).Content = "Window X";
            ((wndXPanel.Header as Grid).Children[1] as TextBox).Text = WindowX.ToString();
            (wndXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xB0";
            wndPanel.Items.Add(wndXPanel);

            // Window Y
            TreeViewItem wndYPanel = Templates.Editbox();
            ((wndYPanel.Header as Grid).Children[0] as Label).Content = "Window Y";
            ((wndYPanel.Header as Grid).Children[1] as TextBox).Text = WindowY.ToString();
            (wndYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xB4";
            wndPanel.Items.Add(wndYPanel);

            panel.Add(wndPanel);

            // Timer
            TreeViewItem timerPanel = Templates.Editbox();
            ((timerPanel.Header as Grid).Children[0] as Label).Content = "Timer";
            ((timerPanel.Header as Grid).Children[1] as TextBox).Text = Timer.ToString();
            (timerPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xD4";
            panel.Add(timerPanel);

            // Mouse
            TreeViewItem mousePanel = Templates.Tab();
            ((mousePanel.Header as Grid).Children[0] as Label).Content = "Mouse";

            // Mouse X
            TreeViewItem mouseXPanel = Templates.Editbox();
            ((mouseXPanel.Header as Grid).Children[0] as Label).Content = "Mouse X";
            ((mouseXPanel.Header as Grid).Children[1] as TextBox).Text = MouseX.ToString();
            (mouseXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x150";
            mousePanel.Items.Add(mouseXPanel);

            // Mouse Y
            TreeViewItem mouseYPanel = Templates.Editbox();
            ((mouseYPanel.Header as Grid).Children[0] as Label).Content = "Mouse Y";
            ((mouseYPanel.Header as Grid).Children[1] as TextBox).Text = MouseY.ToString();
            (mouseYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x154";
            mousePanel.Items.Add(mouseYPanel);

            // Mouse Client X
            TreeViewItem mouseClientXPanel = Templates.Editbox();
            ((mouseClientXPanel.Header as Grid).Children[0] as Label).Content = "Mouse Client X";
            ((mouseClientXPanel.Header as Grid).Children[1] as TextBox).Text = MouseClientX.ToString();
            (mouseClientXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x158";
            mousePanel.Items.Add(mouseClientXPanel);

            // Mouse Client Y
            TreeViewItem mouseClientYPanel = Templates.Editbox();
            ((mouseClientYPanel.Header as Grid).Children[0] as Label).Content = "Mouse Client Y";
            ((mouseClientYPanel.Header as Grid).Children[1] as TextBox).Text = MouseClientY.ToString();
            (mouseClientYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x15C";
            mousePanel.Items.Add(mouseClientYPanel);

            // Mouse Click
            TreeViewItem mouseClickPanel = Templates.Editbox();
            ((mouseClickPanel.Header as Grid).Children[0] as Label).Content = "Mouse Click";
            ((mouseClickPanel.Header as Grid).Children[1] as TextBox).Text = MouseClick.ToString();
            (mouseClickPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x160";
            (mouseClickPanel.Tag as TagHeader).ActionType = 1;
            mousePanel.Items.Add(mouseClickPanel);

            panel.Add(mousePanel);

            // CRunMVHeader
            TreeViewItem mvHeaderPanel = Templates.Tab(true);
            ((mvHeaderPanel.Header as Grid).Children[0] as Label).Content = "MV Header";
            (mvHeaderPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x4F4";

            foreach (var item in CRunMVHeader.GetPanel())
                mvHeaderPanel.Items.Add(item);

            panel.Add(mvHeaderPanel);

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
