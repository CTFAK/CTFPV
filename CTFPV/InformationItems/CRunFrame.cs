using ColorPicker;
using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CTFPV.InformationItems
{
    public class CRunFrame : PropertyPanel
    {
        // Frame Header
        public Size FrameSize;
        public System.Drawing.Color BackgroundColor;
        public BitDict Flags = new BitDict(new string[]
        {
            "Display Name",
            "Grab Desktop",
            "Keep Display",
            "", "",
            "Total Collision Mask",
            "Password",
            "",
            "Resize At Start",
            "Do Not Center",
            "Force Load On Call",
            "No Surface",
            "", "", "",
            "Timed Movements"
        });

        // Information
        public string Name = string.Empty;
        public int LevelLeft;
        public int LevelTop;
        public int LevelLastLeft;
        public int LevelLastTop;

        // Layers
        public int LayerCount;
        public CRunFrameLayer[] Layers = new CRunFrameLayer[0];

        // Rect
        public int RectLeft;
        public int RectTop;
        public int RectRight;
        public int RectBottom;

        // Frame Objects
        public int FrameObjectMaxIndex;
        public int FrameObjectMaxHandle;
        public ushort[] FrameObjectHandleToIndex = new ushort[0];
        public CRunFrameObject[] FrameObjects = new CRunFrameObject[0];

        // Function Indexes
        public int FrameObjectFranIndex;
        public int ObjectInfoFranIndex;

        // Transitions
        public byte FadeIn;
        public byte FadeOut;

        // Exit Code
        public int LevelQuit;

        // Other
        public int NumberOfPlayers;
        public int NumberOfPlayersReal;
        public int LevelLoopState;
        public int StartLevelX;
        public int StartLevelY;
        public ushort MaxObjects;
        public ushort MaxObjectInfos;
        public bool Fade;
        public int FadeTimerDelta;
        public ushort RandomSeed;
        public int MovementTimer;

        // Effects
        public byte LayerEffects;
        public int InkEffect;
        public int InkEffectParam;

        // Scale and Angle
        public float ScaleX;
        public float ScaleY;
        public float Scale;
        public float Angle;

        // Hotspot
        public int HotspotX;
        public int HotspotY;

        // Destination Point
        public int DestPointX;
        public int DestPointY;

        // Run Objects
        public static bool UpdateObjects = false;
        public List<CRunObject> Objects;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Frame Header
            FrameSize = new Size(PV.MemLib.ReadInt(parentPointer + ", 0x0"), PV.MemLib.ReadInt(parentPointer + ", 0x4"));
            BackgroundColor = PV.MemLib.ReadColor(parentPointer + ", 0x8");
            Flags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0xC");

            // Information
            Name = PV.MemLib.ReadString(parentPointer + ", 0x10, 0x0", stringEncoding: Encoding.Unicode);
            LevelLeft = PV.MemLib.ReadInt(parentPointer + ", 0x1C");
            LevelTop = PV.MemLib.ReadInt(parentPointer + ", 0x20");
            LevelLastLeft = PV.MemLib.ReadInt(parentPointer + ", 0x24");
            LevelLastTop = PV.MemLib.ReadInt(parentPointer + ", 0x28");

            // Layers
            LayerCount = PV.MemLib.ReadInt(parentPointer + ", 0x2C");
            Layers = new CRunFrameLayer[LayerCount];
            for (int i = 0; i < LayerCount; i++)
            {
                Layers[i] = new CRunFrameLayer();
                Layers[i].LayerOffset = i * 96;
                Layers[i].InitData(parentPointer + ", 0x30");
            }

            // Rect
            RectLeft = PV.MemLib.ReadInt(parentPointer + ", 0x34");
            RectTop = PV.MemLib.ReadInt(parentPointer + ", 0x38");
            RectRight = PV.MemLib.ReadInt(parentPointer + ", 0x3C");
            RectBottom = PV.MemLib.ReadInt(parentPointer + ", 0x40");

            // Frame Objects
            FrameObjectMaxIndex = PV.MemLib.ReadInt(parentPointer + ", 0x44");
            FrameObjectMaxHandle = PV.MemLib.ReadInt(parentPointer + ", 0x48");
            FrameObjectHandleToIndex = new ushort[FrameObjectMaxHandle];
            FrameObjects = new CRunFrameObject[FrameObjectMaxHandle];

            for (int i = 0; i < FrameObjectMaxHandle; i++)
            {
                FrameObjectHandleToIndex[i] = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x4C, 0x" + (i * 2).ToString("X"));
                FrameObjects[i] = new CRunFrameObject();
                FrameObjects[i].ObjectOffset = i * 36;
                FrameObjects[i].InitData(parentPointer + ", 0x50");
            }

            // Function Indexes
            FrameObjectFranIndex = PV.MemLib.ReadInt(parentPointer + ", 0x54");
            ObjectInfoFranIndex = PV.MemLib.ReadInt(parentPointer + ", 0x58");

            // Transitions
            FadeIn = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x60, 0x0");
            FadeOut = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x64, 0x0");

            // Exit Code
            LevelQuit = PV.MemLib.ReadInt(parentPointer + ", 0x74");

            // Other
            NumberOfPlayers = PV.MemLib.ReadInt(parentPointer + ", 0x8DC");
            NumberOfPlayersReal = PV.MemLib.ReadInt(parentPointer + ", 0x8E0");
            LevelLoopState = PV.MemLib.ReadInt(parentPointer + ", 0x8E4");
            StartLevelX = PV.MemLib.ReadInt(parentPointer + ", 0x8E8");
            StartLevelY = PV.MemLib.ReadInt(parentPointer + ", 0x8EC");
            MaxObjects = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x8F0");
            MaxObjectInfos = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x8F2");
            Fade = PV.MemLib.ReadInt(parentPointer + ", 0x900") == 1;
            FadeTimerDelta = PV.MemLib.ReadInt(parentPointer + ", 0x904");
            RandomSeed = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x968");
            MovementTimer = PV.MemLib.ReadInt(parentPointer + ", 0x96C");

            // Effects
            LayerEffects = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x970, 0x0");
            InkEffect = PV.MemLib.ReadInt(parentPointer + ", 0x974, 0x0");
            InkEffectParam = PV.MemLib.ReadInt(parentPointer + ", 0x974, 0x4");

            // Scale and Angle
            ScaleX = PV.MemLib.ReadFloat(parentPointer + ", 0x988");
            ScaleY = PV.MemLib.ReadFloat(parentPointer + ", 0x98C");
            Scale = PV.MemLib.ReadFloat(parentPointer + ", 0x990");
            Angle = PV.MemLib.ReadFloat(parentPointer + ", 0x994");

            // Hotspot
            HotspotX = PV.MemLib.ReadInt(parentPointer + ", 0x99C");
            HotspotY = PV.MemLib.ReadInt(parentPointer + ", 0x9A0");

            // Destination Point
            DestPointX = PV.MemLib.ReadInt(parentPointer + ", 0x9A4");
            DestPointY = PV.MemLib.ReadInt(parentPointer + ", 0x9A8");

            // Run Objects
            Objects = new List<CRunObject>();
            for (int i = 0; i < MaxObjects; i++)
            {
                string objPointer = parentPointer + ", 0x8D0, 0x" + (i * 8).ToString("X");
                if (PV.MemLib.ReadInt(objPointer) == 0)
                    continue;
                CRunObject obj = new CRunObject();
                obj.InitData(objPointer);
                Objects.Add(obj);
            }

            if (UpdateObjects)
                for (int i = 0; i < MaxObjects; i++)
                    PV.MemLib.WriteMemory(parentPointer + ", 0x8D0, 0x" + (i * 8).ToString("X") + ", 0xFC", "int", "1");
            UpdateObjects = false;
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Frame Header
            FrameSize = new Size(PV.MemLib.ReadInt(parentPointer + ", 0x0"), PV.MemLib.ReadInt(parentPointer + ", 0x4"));
            BackgroundColor = PV.MemLib.ReadColor(parentPointer + ", 0x8");

            // Information
            LevelLeft = PV.MemLib.ReadInt(parentPointer + ", 0x1C");
            LevelTop = PV.MemLib.ReadInt(parentPointer + ", 0x20");
            LevelLastLeft = PV.MemLib.ReadInt(parentPointer + ", 0x24");
            LevelLastTop = PV.MemLib.ReadInt(parentPointer + ", 0x28");

            // Layers
            for (int i = 0; i < LayerCount; i++)
            {
                Layers[i].LayerOffset = i * 96;
                Layers[i].RefreshData(parentPointer + ", 0x30");
            }

            // Rect
            RectRight = PV.MemLib.ReadInt(parentPointer + ", 0x3C");
            RectBottom = PV.MemLib.ReadInt(parentPointer + ", 0x40");

            // Frame Objects
            FrameObjectMaxIndex = PV.MemLib.ReadInt(parentPointer + ", 0x44");
            FrameObjectMaxHandle = PV.MemLib.ReadInt(parentPointer + ", 0x48");
            FrameObjectHandleToIndex = new ushort[FrameObjectMaxHandle];
            var NewFrameObjects = new CRunFrameObject[FrameObjectMaxHandle];

            for (int i = 0; i < FrameObjectMaxHandle; i++)
            {
                FrameObjectHandleToIndex[i] = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x4C, 0x" + (i * 2).ToString("X"));
                if (FrameObjects != null && FrameObjects.Length > i && FrameObjects[i] != null)
                {
                    FrameObjects[i].ObjectOffset = i * 36;
                    FrameObjects[i].RefreshData(parentPointer + ", 0x50");
                    NewFrameObjects[i] = FrameObjects[i];
                }
                else
                {
                    NewFrameObjects[i] = new CRunFrameObject();
                    NewFrameObjects[i].ObjectOffset = i * 36;
                    NewFrameObjects[i].InitData(parentPointer + ", 0x50");
                }
            }
            FrameObjects = NewFrameObjects;

            // Exit Code
            LevelQuit = PV.MemLib.ReadInt(parentPointer + ", 0x74");

            // Other
            NumberOfPlayers = PV.MemLib.ReadInt(parentPointer + ", 0x8DC");
            NumberOfPlayersReal = PV.MemLib.ReadInt(parentPointer + ", 0x8E0");
            LevelLoopState = PV.MemLib.ReadInt(parentPointer + ", 0x8E4");
            MaxObjects = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x8F0");
            MaxObjectInfos = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x8F2");
            Fade = PV.MemLib.ReadInt(parentPointer + ", 0x900") == 1;
            FadeTimerDelta = PV.MemLib.ReadInt(parentPointer + ", 0x904");

            // Effects
            LayerEffects = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x970, 0x0");
            InkEffect = PV.MemLib.ReadInt(parentPointer + ", 0x974, 0x0");
            InkEffectParam = PV.MemLib.ReadInt(parentPointer + ", 0x974, 0x4");

            // Scale and Angle
            ScaleX = PV.MemLib.ReadFloat(parentPointer + ", 0x988");
            ScaleY = PV.MemLib.ReadFloat(parentPointer + ", 0x98C");
            Angle = PV.MemLib.ReadFloat(parentPointer + ", 0x994");

            // Hotspot
            HotspotX = PV.MemLib.ReadInt(parentPointer + ", 0x99C");
            HotspotY = PV.MemLib.ReadInt(parentPointer + ", 0x9A0");

            // Destination Point
            DestPointX = PV.MemLib.ReadInt(parentPointer + ", 0x9A4");
            DestPointY = PV.MemLib.ReadInt(parentPointer + ", 0x9A8");

            // Run Objects
            Objects = new List<CRunObject>();
            for (int i = 0; i < MaxObjects; i++)
            {
                string objPointer = parentPointer + ", 0x8D0, 0x" + (i * 8).ToString("X");
                if (PV.MemLib.ReadInt(objPointer) == 0)
                    continue;
                CRunObject obj = new CRunObject();
                obj.InitData(objPointer);
                Objects.Add(obj);
            }

            if (UpdateObjects)
                for (int i = 0; i < MaxObjects; i++)
                    PV.MemLib.WriteMemory(parentPointer + ", 0x8D0, 0x" + (i * 8).ToString("X") + ", 0xFC", "int", "1");
            UpdateObjects = false;
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Frame Header
            TreeViewItem frmHeaderPanel = Templates.Tab();
            ((frmHeaderPanel.Header as Grid).Children[0] as Label).Content = "Frame Header";

            // Width
            TreeViewItem widthPanel = Templates.Editbox();
            ((widthPanel.Header as Grid).Children[0] as Label).Content = "Width";
            ((widthPanel.Header as Grid).Children[1] as TextBox).Text = FrameSize.Width.ToString();
            (widthPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x0";
            frmHeaderPanel.Items.Add(widthPanel);

            // Height
            TreeViewItem heightPanel = Templates.Editbox();
            ((heightPanel.Header as Grid).Children[0] as Label).Content = "Height";
            ((heightPanel.Header as Grid).Children[1] as TextBox).Text = FrameSize.Height.ToString();
            (heightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x4";
            frmHeaderPanel.Items.Add(heightPanel);

            // Background Color
            TreeViewItem bgColorPanel = Templates.ColorPicker();
            ((bgColorPanel.Header as Grid).Children[0] as Label).Content = "Background Color";
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_R = BackgroundColor.R;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_G = BackgroundColor.G;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_B = BackgroundColor.B;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.A = 255;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).ShowAlpha = false;
            (bgColorPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8";
            frmHeaderPanel.Items.Add(bgColorPanel);

            // Flags
            TreeViewItem flagsPanel = Templates.Tab(true);
            ((flagsPanel.Header as Grid).Children[0] as Label).Content = "Flags";
            (flagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xC";

            foreach (string key in Flags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem flagPanel = Templates.Checkbox(false);
                ((flagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((flagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Flags[key];
                (flagPanel.Tag as TagHeader).ParentFlags = Flags;
                (flagPanel.Tag as TagHeader).Flag = key;
                (flagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xC";
                flagsPanel.Items.Add(flagPanel);
            }

            frmHeaderPanel.Items.Add(flagsPanel);

            panel.Add(frmHeaderPanel);

            // Information
            TreeViewItem infoPanel = Templates.Tab();
            ((infoPanel.Header as Grid).Children[0] as Label).Content = "Information";

            // Name
            TreeViewItem namePanel = Templates.Editbox();
            ((namePanel.Header as Grid).Children[0] as Label).Content = "Name";
            ((namePanel.Header as Grid).Children[1] as TextBox).Text = Name;
            ((namePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (namePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x10, 0x0";
            infoPanel.Items.Add(namePanel);

            // Level Left
            TreeViewItem lvlLeftPanel = Templates.Editbox();
            ((lvlLeftPanel.Header as Grid).Children[0] as Label).Content = "Level Left";
            ((lvlLeftPanel.Header as Grid).Children[1] as TextBox).Text = LevelLeft.ToString();
            (lvlLeftPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1C";
            infoPanel.Items.Add(lvlLeftPanel);

            // Level Top
            TreeViewItem lvlTopPanel = Templates.Editbox();
            ((lvlTopPanel.Header as Grid).Children[0] as Label).Content = "Level Top";
            ((lvlTopPanel.Header as Grid).Children[1] as TextBox).Text = LevelTop.ToString();
            (lvlTopPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x20";
            infoPanel.Items.Add(lvlTopPanel);

            // Level Last Left
            TreeViewItem lvlLastLeftPanel = Templates.Editbox();
            ((lvlLastLeftPanel.Header as Grid).Children[0] as Label).Content = "Level Last Left";
            ((lvlLastLeftPanel.Header as Grid).Children[1] as TextBox).Text = LevelLastLeft.ToString();
            ((lvlLastLeftPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (lvlLastLeftPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x24";
            infoPanel.Items.Add(lvlLastLeftPanel);

            // Level Last Top
            TreeViewItem lvlLastTopPanel = Templates.Editbox();
            ((lvlLastTopPanel.Header as Grid).Children[0] as Label).Content = "Level Last Top";
            ((lvlLastTopPanel.Header as Grid).Children[1] as TextBox).Text = LevelLastTop.ToString();
            ((lvlLastTopPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (lvlLastTopPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x28";
            infoPanel.Items.Add(lvlLastTopPanel);

            panel.Add(infoPanel);

            // Layers
            TreeViewItem lyrsPanel = Templates.Tab();
            ((lyrsPanel.Header as Grid).Children[0] as Label).Content = $"Layers ({LayerCount})";
            (lyrsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x30";

            int LayerID = 0;
            foreach (var Layer in Layers)
            {
                TreeViewItem lyrPanel = Templates.Tab();
                ((lyrPanel.Header as Grid).Children[0] as Label).Content = $"[{LayerID++}] {Layer.Name}";
                (lyrPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x30, 0x" + (LayerID * 96).ToString("X");
                foreach (var lpanel in Layer.GetPanel())
                    lyrPanel.Items.Add(lpanel);
                lyrsPanel.Items.Add(lyrPanel);
            }

            panel.Add(lyrsPanel);

            // Rect
            TreeViewItem rectPanel = Templates.Tab();
            ((rectPanel.Header as Grid).Children[0] as Label).Content = "Frame Rectangle";

            // Rect Left
            TreeViewItem rectLeftPanel = Templates.Editbox();
            ((rectLeftPanel.Header as Grid).Children[0] as Label).Content = "Rect Left";
            ((rectLeftPanel.Header as Grid).Children[1] as TextBox).Text = RectLeft.ToString();
            (rectLeftPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x34";
            rectPanel.Items.Add(rectLeftPanel);

            // Rect Top
            TreeViewItem rectTopPanel = Templates.Editbox();
            ((rectTopPanel.Header as Grid).Children[0] as Label).Content = "Rect Top";
            ((rectTopPanel.Header as Grid).Children[1] as TextBox).Text = RectTop.ToString();
            (rectTopPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x38";
            rectPanel.Items.Add(rectTopPanel);

            // Rect Right
            TreeViewItem rectRightPanel = Templates.Editbox();
            ((rectRightPanel.Header as Grid).Children[0] as Label).Content = "Rect Right";
            ((rectRightPanel.Header as Grid).Children[1] as TextBox).Text = RectRight.ToString();
            (rectRightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x3C";
            rectPanel.Items.Add(rectRightPanel);

            // Rect Bottom
            TreeViewItem rectBottomPanel = Templates.Editbox();
            ((rectBottomPanel.Header as Grid).Children[0] as Label).Content = "Rect Bottom";
            ((rectBottomPanel.Header as Grid).Children[1] as TextBox).Text = RectBottom.ToString();
            (rectBottomPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x40";
            rectPanel.Items.Add(rectBottomPanel);

            panel.Add(rectPanel);

            // Frame Objects
            TreeViewItem frmObjsPanel = Templates.Tab();
            ((frmObjsPanel.Header as Grid).Children[0] as Label).Content = $"Frame Objects ({FrameObjectMaxIndex})";
            (frmObjsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x50";
            panel.Add(frmObjsPanel);

            // Transitions
            TreeViewItem transPanel = Templates.Tab();
            ((transPanel.Header as Grid).Children[0] as Label).Content = "Transitions";

            // Fade In
            TreeViewItem fadeInPanel = Templates.Editbox();
            ((fadeInPanel.Header as Grid).Children[0] as Label).Content = "Fade In";
            ((fadeInPanel.Header as Grid).Children[1] as TextBox).Text = FadeIn.ToString();
            (fadeInPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x60, 0x0";
            transPanel.Items.Add(fadeInPanel);

            // Fade Out
            TreeViewItem fadeOutPanel = Templates.Editbox();
            ((fadeOutPanel.Header as Grid).Children[0] as Label).Content = "Fade Out";
            ((fadeOutPanel.Header as Grid).Children[1] as TextBox).Text = FadeOut.ToString();
            (fadeOutPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x64, 0x0";
            transPanel.Items.Add(fadeOutPanel);

            panel.Add(transPanel);

            // Other
            TreeViewItem otherPanel = Templates.Tab();
            ((otherPanel.Header as Grid).Children[0] as Label).Content = "Other";

            // Level Quit
            TreeViewItem lvlQuitPanel = Templates.Editbox();
            ((lvlQuitPanel.Header as Grid).Children[0] as Label).Content = "Level Quit";
            ((lvlQuitPanel.Header as Grid).Children[1] as TextBox).Text = LevelQuit.ToString();
            (lvlQuitPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x74";
            otherPanel.Items.Add(lvlQuitPanel);

            // Number Of Players
            TreeViewItem plyrCntPanel = Templates.Editbox();
            ((plyrCntPanel.Header as Grid).Children[0] as Label).Content = "Player Count";
            ((plyrCntPanel.Header as Grid).Children[1] as TextBox).Text = NumberOfPlayers.ToString();
            ((plyrCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (plyrCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8DC";
            otherPanel.Items.Add(plyrCntPanel);

            // Level Loop State
            TreeViewItem loopSttPanel = Templates.Editbox();
            ((loopSttPanel.Header as Grid).Children[0] as Label).Content = "Level Loop State";
            ((loopSttPanel.Header as Grid).Children[1] as TextBox).Text = LevelLoopState.ToString();
            ((loopSttPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (loopSttPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8E4";
            otherPanel.Items.Add(loopSttPanel);

            // Start Level X
            TreeViewItem strtXPanel = Templates.Editbox();
            ((strtXPanel.Header as Grid).Children[0] as Label).Content = "Start Level X";
            ((strtXPanel.Header as Grid).Children[1] as TextBox).Text = StartLevelX.ToString();
            ((strtXPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (strtXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8E8";
            otherPanel.Items.Add(strtXPanel);

            // Start Level Y
            TreeViewItem strtYPanel = Templates.Editbox();
            ((strtYPanel.Header as Grid).Children[0] as Label).Content = "Start Level Y";
            ((strtYPanel.Header as Grid).Children[1] as TextBox).Text = StartLevelY.ToString();
            ((strtYPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (strtYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8EC";
            otherPanel.Items.Add(strtYPanel);

            // Max Objects
            TreeViewItem maxObjsPanel = Templates.Editbox();
            ((maxObjsPanel.Header as Grid).Children[0] as Label).Content = "Max Objects";
            ((maxObjsPanel.Header as Grid).Children[1] as TextBox).Text = MaxObjects.ToString();
            ((maxObjsPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (maxObjsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8F0";
            otherPanel.Items.Add(maxObjsPanel);

            // Fading
            TreeViewItem fadingPanel = Templates.Checkbox();
            ((fadingPanel.Header as Grid).Children[0] as Label).Content = "Fading";
            ((fadingPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Fade;
            (fadingPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x900";
            otherPanel.Items.Add(fadingPanel);

            // Random Seed
            TreeViewItem rngSeedPanel = Templates.Editbox();
            ((rngSeedPanel.Header as Grid).Children[0] as Label).Content = "Random Seed";
            ((rngSeedPanel.Header as Grid).Children[1] as TextBox).Text = RandomSeed.ToString();
            (rngSeedPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x968";
            (rngSeedPanel.Tag as TagHeader).ActionType = 1;
            otherPanel.Items.Add(rngSeedPanel);

            // Movement Timer
            TreeViewItem mvmtTimerPanel = Templates.Editbox();
            ((mvmtTimerPanel.Header as Grid).Children[0] as Label).Content = "Movement Timer";
            ((mvmtTimerPanel.Header as Grid).Children[1] as TextBox).Text = MovementTimer.ToString();
            (mvmtTimerPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x96C";
            otherPanel.Items.Add(mvmtTimerPanel);

            // Ink Effect
            TreeViewItem inkEfctPanel = Templates.Editbox();
            ((inkEfctPanel.Header as Grid).Children[0] as Label).Content = "Ink Effect";
            ((inkEfctPanel.Header as Grid).Children[1] as TextBox).Text = InkEffect.ToString();
            (inkEfctPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x974, 0x0";
            otherPanel.Items.Add(inkEfctPanel);

            panel.Add(otherPanel);

            // Scale and Angle
            TreeViewItem scaleAnglePanel = Templates.Tab();
            ((scaleAnglePanel.Header as Grid).Children[0] as Label).Content = "Scale and Angle";

            // Scale X
            TreeViewItem scaleXPanel = Templates.Editbox();
            ((scaleXPanel.Header as Grid).Children[0] as Label).Content = "X Scale";
            ((scaleXPanel.Header as Grid).Children[1] as TextBox).Text = ScaleX.ToString();
            (scaleXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x988";
            (scaleXPanel.Tag as TagHeader).ActionType = 3;
            scaleAnglePanel.Items.Add(scaleXPanel);

            // Scale Y
            TreeViewItem scaleYPanel = Templates.Editbox();
            ((scaleYPanel.Header as Grid).Children[0] as Label).Content = "Y Scale";
            ((scaleYPanel.Header as Grid).Children[1] as TextBox).Text = ScaleY.ToString();
            (scaleYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x98C";
            (scaleYPanel.Tag as TagHeader).ActionType = 3;
            scaleAnglePanel.Items.Add(scaleYPanel);

            // Scale
            TreeViewItem scalePanel = Templates.Editbox();
            ((scalePanel.Header as Grid).Children[0] as Label).Content = "Scale";
            ((scalePanel.Header as Grid).Children[1] as TextBox).Text = Scale.ToString();
            (scalePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x990";
            (scalePanel.Tag as TagHeader).ActionType = 3;
            scaleAnglePanel.Items.Add(scalePanel);

            // Angle
            TreeViewItem anglePanel = Templates.Editbox();
            ((anglePanel.Header as Grid).Children[0] as Label).Content = "Angle";
            ((anglePanel.Header as Grid).Children[1] as TextBox).Text = Angle.ToString();
            (anglePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x994";
            (anglePanel.Tag as TagHeader).ActionType = 3;
            scaleAnglePanel.Items.Add(anglePanel);

            panel.Add(scaleAnglePanel);

            // Points
            TreeViewItem pointsPanel = Templates.Tab();
            ((pointsPanel.Header as Grid).Children[0] as Label).Content = "Points";

            // Hotspot X
            TreeViewItem hotXPanel = Templates.Editbox();
            ((hotXPanel.Header as Grid).Children[0] as Label).Content = "Hotspot X";
            ((hotXPanel.Header as Grid).Children[1] as TextBox).Text = HotspotX.ToString();
            ((hotXPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (hotXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x99C";
            pointsPanel.Items.Add(hotXPanel);

            // Hotspot Y
            TreeViewItem hotYPanel = Templates.Editbox();
            ((hotYPanel.Header as Grid).Children[0] as Label).Content = "Hotspot Y";
            ((hotYPanel.Header as Grid).Children[1] as TextBox).Text = HotspotY.ToString();
            ((hotYPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (hotYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x9A0";
            pointsPanel.Items.Add(hotYPanel);

            // Destination X
            TreeViewItem destXPanel = Templates.Editbox();
            ((destXPanel.Header as Grid).Children[0] as Label).Content = "Destination X";
            ((destXPanel.Header as Grid).Children[1] as TextBox).Text = DestPointX.ToString();
            ((destXPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (destXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x9A4";
            pointsPanel.Items.Add(destXPanel);

            // Destination Y
            TreeViewItem destYPanel = Templates.Editbox();
            ((destYPanel.Header as Grid).Children[0] as Label).Content = "Destination Y";
            ((destYPanel.Header as Grid).Children[1] as TextBox).Text = DestPointY.ToString();
            ((destYPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (destYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x9A8";
            pointsPanel.Items.Add(destYPanel);

            panel.Add(pointsPanel);

            // Run Object Count
            TreeViewItem runObjCntPanel = Templates.Editbox();
            ((runObjCntPanel.Header as Grid).Children[0] as Label).Content = "Run Object Count";
            ((runObjCntPanel.Header as Grid).Children[1] as TextBox).Text = Objects.Count.ToString();
            ((runObjCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (runObjCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x8D0";
            panel.Add(runObjCntPanel);

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }

        public TreeViewItem GetListPanel()
        {
            TreeViewItem frmItem = new TreeViewItem();

            Label frmHeader = new Label();
            frmHeader.Content = $"Current Loaded Frame: '{Name}'";
            frmHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE7FF"));
            frmHeader.HorizontalAlignment = HorizontalAlignment.Left;
            frmHeader.VerticalAlignment = VerticalAlignment.Center;
            frmHeader.HorizontalContentAlignment = HorizontalAlignment.Center;
            frmHeader.FontSize = 10;
            frmHeader.Padding = new Thickness(0);
            frmHeader.Margin = new Thickness(0, 0, 0, 0);

            for (int i = 0; i < Objects.Count; i++)
            {
                CRunObjectInfo objInfo = PV.CRunApp.ObjectInfos[PV.CRunApp.ObjectInfoHandleToIndex[Objects[i].ObjInfoNumber]];
                if (objInfo.Type != 1 && Objects[i].Identifier == "")
                    continue;

                TreeViewItem objItem = new TreeViewItem();
                Label objHeader = new Label();
                if (objInfo.Type == 1)
                    objHeader.Content = objInfo.Name;
                else
                    objHeader.Content = $"[{Objects[i].Identifier}] {objInfo.Name}";
                objHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE7FF"));
                objHeader.HorizontalAlignment = HorizontalAlignment.Left;
                objHeader.VerticalAlignment = VerticalAlignment.Center;
                objHeader.HorizontalContentAlignment = HorizontalAlignment.Center;
                objHeader.FontSize = 10;
                objHeader.Padding = new Thickness(0);
                objHeader.Margin = new Thickness(0, 0, 0, 0);

                objItem.Header = objHeader;
                objItem.IsExpanded = true;
                TagHeader tag = new TagHeader();
                tag.Parent = Objects[i];
                tag.Handle = Objects[i].Number;
                tag.Pointer = Objects[i].latestParentPointer;
                objItem.ContextMenu = tag.GetMenu();
                objItem.Tag = tag;
                frmItem.Items.Add(objItem);
            }

            TagHeader frmtag = new TagHeader();
            frmtag.Parent = this;
            frmtag.Pointer = latestParentPointer;
            frmItem.ContextMenu = frmtag.GetMenu();
            frmItem.Tag = frmtag;
            frmItem.Header = frmHeader;
            frmItem.IsExpanded = true;
            return frmItem;
        }

        public void RefreshListPanel(ref TreeView treeView)
        {
            var frmItem = (treeView.Items[0] as TreeViewItem).Items[0] as TreeViewItem;
            var handles = new List<int>();
            foreach (TreeViewItem item in frmItem.Items)
                handles.Add((item.Tag as TagHeader).Handle);

            for (int i = 0; i < Objects.Count; i++)
            {
                if (handles.Contains(Objects[i].Number))
                {
                    handles.Remove(Objects[i].Number);
                    continue;
                }

                CRunObjectInfo objInfo = PV.CRunApp.ObjectInfos[PV.CRunApp.ObjectInfoHandleToIndex[Objects[i].ObjInfoNumber]];
                if (objInfo.Type != 1 && Objects[i].Identifier == "")
                    continue;

                TreeViewItem objItem = new TreeViewItem();
                Label objHeader = new Label();
                if (objInfo.Type == 1)
                    objHeader.Content = objInfo.Name;
                else
                    objHeader.Content = $"[{Objects[i].Identifier}] {objInfo.Name}";
                objHeader.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE7FF"));
                objHeader.HorizontalAlignment = HorizontalAlignment.Left;
                objHeader.VerticalAlignment = VerticalAlignment.Center;
                objHeader.HorizontalContentAlignment = HorizontalAlignment.Center;
                objHeader.FontSize = 10;
                objHeader.Padding = new Thickness(0);
                objHeader.Margin = new Thickness(0, 0, 0, 0);

                objItem.Header = objHeader;
                objItem.IsExpanded = true;
                TagHeader tag = new TagHeader();
                tag.Parent = Objects[i];
                tag.Handle = Objects[i].Number;
                tag.Pointer = Objects[i].latestParentPointer;
                objItem.ContextMenu = tag.GetMenu();
                objItem.Tag = tag;
                if (!frmItem.Items.Contains(objInfo))
                    frmItem.Items.Add(objItem);
            }

            foreach (int handle in handles)
                foreach (TreeViewItem item in frmItem.Items)
                    if ((item.Tag as TagHeader).Handle == handle)
                    {
                        frmItem.Items.Remove(item);
                        break;
                    }
        }
    }
}
