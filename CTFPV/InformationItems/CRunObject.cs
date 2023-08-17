using ColorPicker;
using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Xml.Linq;

namespace CTFPV.InformationItems
{
    public class CRunObject : PropertyPanel
    {
        // Object Header
        public short Number;
        public short Next;
        public int Size;
        public short FrmObjNumber;
        public short ObjInfoNumber;
        public short Type;
        public short CreationID;
        public string MovementName = string.Empty;
        public CRunObjectCommon ObjectCommon;

        // Object
        public int XPosition;
        public int YPosition;
        public int XHotspot;
        public int YHotspot;
        public int Width;
        public int Height;
        public Rectangle DisplayRect;
        public BitDict ObjectFlags = new BitDict(new string[]
        {
            "Display In Front",
            "Background",
            "Backsave",
            "Run Before Run In",
            "Movements",
            "Animations",
            "Tab Stop",
            "Window Process",
            "Values",
            "Sprites",
            "Internal Backsave",
            "Scrolling Independent",
            "Quick Display",
            "Never Kill",
            "Never Sleep",
            "Manual Sleep",
            "Text",
            "Don't Create at Start"
        });
        public BitDict Flags = new BitDict(new string[]
        {
            "Destroyed",
            "True Event",
            "Real Sprite",
            "Fade In",
            "Fade Out",
            "Owner Draw",
            "", "", "", "", "", "", "",
            "No Collision",
            "Float",
            "String"
        });
        public uint Layer;
        public BitDict CollisionFlags = new BitDict(new string[]
        {
            "", "", "", "",
            "Backdrops",
            "", "",
            "On Collide",
            "Quick Collision",
            "Quick Border",
            "Quick Sprite",
            "Quick Extension"
        });
        public string Identifier = string.Empty;

        // Animation/Movement Information
        public int CurrentPlayer;
        public int CurrentAnimation;
        public int CurrentFrame;
        public float XScale;
        public float YScale;
        public float Angle;
        public int CurrentDirection;
        public int CurrentSpeed;
        public int MinSpeed;
        public int MaxSpeed;
        public bool UpdateObject;

        // Movement Structure
        public int Acceleration;
        public int Decceleration;
        public int CollisionCount;
        public int StopSpeed;
        public int AccelValue;
        public int DeccelValue;
        public BitDict EventFlags = new BitDict(new string[]
        {
            "Goes In Playfield",
            "Goes Out Playfield",
            "Wrap"
        });
        public bool Moving;
        public bool Wrapping;
        public bool Reverse;
        public bool Bouncing;
        public int CurrentMovement;

        // Additional Structure
        public CRunActiveObject ActiveObject;
        public CRunSystemObject SystemObject;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Object Header
            Number = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x0");
            Next = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x2");
            Size = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x4");
            FrmObjNumber = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x10");
            ObjInfoNumber = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x12");
            Type = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x18");
            CreationID = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x1A");
            MovementName = PV.MemLib.ReadString(parentPointer + ", 0x38, 0x0", length: 255, stringEncoding: Encoding.Unicode);
            ObjectCommon = new CRunObjectCommon();
            ObjectCommon.InitData(parentPointer + ", 0x44");

            // Object
            XPosition = PV.MemLib.ReadInt(parentPointer + ", 0x4C");
            YPosition = PV.MemLib.ReadInt(parentPointer + ", 0x54");
            XHotspot = PV.MemLib.ReadInt(parentPointer + ", 0x58");
            YHotspot = PV.MemLib.ReadInt(parentPointer + ", 0x5C");
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x60");
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x64");
            DisplayRect = new Rectangle();
            DisplayRect.X = PV.MemLib.ReadInt(parentPointer + ", 0x68");
            DisplayRect.Y = PV.MemLib.ReadInt(parentPointer + ", 0x6C");
            DisplayRect.Width = PV.MemLib.ReadInt(parentPointer + ", 0x70") - DisplayRect.X;
            DisplayRect.Height = PV.MemLib.ReadInt(parentPointer + ", 0x74") - DisplayRect.Y;
            ObjectFlags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x78");
            Flags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x7C");
            Layer = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x84");
            CollisionFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x94");
            Identifier = PV.MemLib.ReadString(parentPointer + ", 0xB4", length: 4, stringEncoding: Encoding.ASCII);

            // Animation/Movement Information
            CurrentPlayer = PV.MemLib.ReadInt(parentPointer + ", 0xC8");
            CurrentAnimation = PV.MemLib.ReadInt(parentPointer + ", 0xD8");
            CurrentFrame = PV.MemLib.ReadInt(parentPointer + ", 0xDC");
            XScale = PV.MemLib.ReadFloat(parentPointer + ", 0xE0");
            YScale = PV.MemLib.ReadFloat(parentPointer + ", 0xE4");
            Angle = PV.MemLib.ReadFloat(parentPointer + ", 0xE8");
            CurrentDirection = PV.MemLib.ReadInt(parentPointer + ", 0xEC");
            CurrentSpeed = PV.MemLib.ReadInt(parentPointer + ", 0xF0");
            MinSpeed = PV.MemLib.ReadInt(parentPointer + ", 0xF4");
            MaxSpeed = PV.MemLib.ReadInt(parentPointer + ", 0xF8");
            UpdateObject = PV.MemLib.ReadInt(parentPointer + ", 0xFC") == 1;

            // Movement Structure
            Acceleration = PV.MemLib.ReadInt(parentPointer + ", 0x130");
            Decceleration = PV.MemLib.ReadInt(parentPointer + ", 0x134");
            CollisionCount = PV.MemLib.ReadInt(parentPointer + ", 0x138");
            StopSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x140");
            AccelValue = PV.MemLib.ReadInt(parentPointer + ", 0x14C");
            DeccelValue = PV.MemLib.ReadInt(parentPointer + ", 0x150");
            EventFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x154");
            Moving = PV.MemLib.ReadInt(parentPointer + ", 0x16A") == 1;
            Wrapping = PV.MemLib.ReadInt(parentPointer + ", 0x16E") == 1;
            Reverse = PV.MemLib.ReadInt(parentPointer + ", 0x172") == 1;
            Bouncing = PV.MemLib.ReadInt(parentPointer + ", 0x176") == 1;
            CurrentMovement = PV.MemLib.ReadInt(parentPointer + ", 0x17A");

            switch (Identifier)
            {
                // Active
                case "SPRI":
                    ActiveObject = new CRunActiveObject();
                    ActiveObject.InitData(parentPointer);
                    break;

                // String and Coutner
                case "XTÿÿ":
                case "TE":
                case "TEXT":
                case "TRÿÿ":
                case "CNTR":
                case "LIVE":
                case "CN":
                    SystemObject = new CRunSystemObject();
                    SystemObject.InitData(parentPointer);
                    break;
            }
        }

        public override void RefreshData(string parentPointer)
        {

        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Object Header
            TreeViewItem objHeaderPanel = Templates.Tab();
            ((objHeaderPanel.Header as Grid).Children[0] as Label).Content = "Object Header";

            // Handle
            TreeViewItem hndlPanel = Templates.Editbox();
            ((hndlPanel.Header as Grid).Children[0] as Label).Content = "Handle";
            ((hndlPanel.Header as Grid).Children[1] as TextBox).Text = Number.ToString();
            ((hndlPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (hndlPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x0";
            objHeaderPanel.Items.Add(hndlPanel);

            // Size
            TreeViewItem sizePanel = Templates.Editbox();
            ((sizePanel.Header as Grid).Children[0] as Label).Content = "Size";
            ((sizePanel.Header as Grid).Children[1] as TextBox).Text = Size.ToString();
            ((sizePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (sizePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x4";
            objHeaderPanel.Items.Add(sizePanel);

            // Frame Object Handle
            TreeViewItem frmObjHndlPanel = Templates.Editbox();
            ((frmObjHndlPanel.Header as Grid).Children[0] as Label).Content = "Frame Object Handle";
            ((frmObjHndlPanel.Header as Grid).Children[1] as TextBox).Text = FrmObjNumber.ToString();
            ((frmObjHndlPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (frmObjHndlPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x10";
            objHeaderPanel.Items.Add(frmObjHndlPanel);

            // Object Info Handle
            TreeViewItem objInfoHndlPanel = Templates.Editbox();
            ((objInfoHndlPanel.Header as Grid).Children[0] as Label).Content = "Object Info Handle";
            ((objInfoHndlPanel.Header as Grid).Children[1] as TextBox).Text = ObjInfoNumber.ToString();
            ((objInfoHndlPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (objInfoHndlPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x12";
            objHeaderPanel.Items.Add(objInfoHndlPanel);

            // Type
            TreeViewItem typePanel = Templates.Editbox();
            ((typePanel.Header as Grid).Children[0] as Label).Content = "Type";
            ((typePanel.Header as Grid).Children[1] as TextBox).Text = Type.ToString();
            ((typePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (typePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x18";
            objHeaderPanel.Items.Add(typePanel);

            // Movement Name
            TreeViewItem mvmtNamePanel = Templates.Editbox();
            ((mvmtNamePanel.Header as Grid).Children[0] as Label).Content = "Movement Name";
            ((mvmtNamePanel.Header as Grid).Children[1] as TextBox).Text = MovementName;
            (mvmtNamePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x38, 0x0";
            (mvmtNamePanel.Tag as TagHeader).ActionType = 5;
            objHeaderPanel.Items.Add(mvmtNamePanel);

            // Object Common
            TreeViewItem objCommonPanel = Templates.Tab();
            ((objCommonPanel.Header as Grid).Children[0] as Label).Content = "Object Common";
            (objCommonPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x44";
            foreach (var ocpanel in ObjectCommon.GetPanel())
                objCommonPanel.Items.Add(ocpanel);
            objHeaderPanel.Items.Add(objCommonPanel);

            panel.Add(objHeaderPanel);

            // Object
            TreeViewItem objPanel = Templates.Tab();
            ((objPanel.Header as Grid).Children[0] as Label).Content = "Object";

            // X Position
            TreeViewItem posXPanel = Templates.Editbox();
            ((posXPanel.Header as Grid).Children[0] as Label).Content = "X Position";
            ((posXPanel.Header as Grid).Children[1] as TextBox).Text = XPosition.ToString();
            (posXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x4C";
            objPanel.Items.Add(posXPanel);

            // Y Position
            TreeViewItem posYPanel = Templates.Editbox();
            ((posYPanel.Header as Grid).Children[0] as Label).Content = "Y Position";
            ((posYPanel.Header as Grid).Children[1] as TextBox).Text = YPosition.ToString();
            (posYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x54";
            objPanel.Items.Add(posYPanel);

            // X Hotspot
            TreeViewItem hotXPanel = Templates.Editbox();
            ((hotXPanel.Header as Grid).Children[0] as Label).Content = "X Hotspot";
            ((hotXPanel.Header as Grid).Children[1] as TextBox).Text = XHotspot.ToString();
            (hotXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x58";
            objPanel.Items.Add(hotXPanel);

            // Y Hotspot
            TreeViewItem hotYPanel = Templates.Editbox();
            ((hotYPanel.Header as Grid).Children[0] as Label).Content = "Y Hotspot";
            ((hotYPanel.Header as Grid).Children[1] as TextBox).Text = YHotspot.ToString();
            (hotYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x5C";
            objPanel.Items.Add(hotYPanel);

            // Width
            TreeViewItem widthPanel = Templates.Editbox();
            ((widthPanel.Header as Grid).Children[0] as Label).Content = "Width";
            ((widthPanel.Header as Grid).Children[1] as TextBox).Text = Width.ToString();
            (widthPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x60";
            objPanel.Items.Add(widthPanel);

            // Height
            TreeViewItem heightPanel = Templates.Editbox();
            ((heightPanel.Header as Grid).Children[0] as Label).Content = "Height";
            ((heightPanel.Header as Grid).Children[1] as TextBox).Text = Height.ToString();
            (heightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x64";
            objPanel.Items.Add(heightPanel);

            // Display Rect
            TreeViewItem displayRectPanel = Templates.Tab();
            ((displayRectPanel.Header as Grid).Children[0] as Label).Content = "Display Rectangle";

            // Display Rect Left
            TreeViewItem displayRectLeftPanel = Templates.Editbox();
            ((displayRectLeftPanel.Header as Grid).Children[0] as Label).Content = "Display Rect Left";
            ((displayRectLeftPanel.Header as Grid).Children[1] as TextBox).Text = DisplayRect.Left.ToString();
            (displayRectLeftPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x68";
            displayRectPanel.Items.Add(displayRectLeftPanel);

            // Display Rect Top
            TreeViewItem displayRectTopPanel = Templates.Editbox();
            ((displayRectTopPanel.Header as Grid).Children[0] as Label).Content = "Display Rect Top";
            ((displayRectTopPanel.Header as Grid).Children[1] as TextBox).Text = DisplayRect.Top.ToString();
            (displayRectTopPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x6C";
            displayRectPanel.Items.Add(displayRectTopPanel);

            // Display Rect Right
            TreeViewItem displayRectRightPanel = Templates.Editbox();
            ((displayRectRightPanel.Header as Grid).Children[0] as Label).Content = "Display Rect Right";
            ((displayRectRightPanel.Header as Grid).Children[1] as TextBox).Text = DisplayRect.Right.ToString();
            (displayRectRightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x70";
            displayRectPanel.Items.Add(displayRectRightPanel);

            // Display Rect Bottom
            TreeViewItem displayRectBottomPanel = Templates.Editbox();
            ((displayRectBottomPanel.Header as Grid).Children[0] as Label).Content = "Display Rect Bottom";
            ((displayRectBottomPanel.Header as Grid).Children[1] as TextBox).Text = DisplayRect.Bottom.ToString();
            (displayRectBottomPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x74";
            displayRectPanel.Items.Add(displayRectBottomPanel);

            objPanel.Items.Add(displayRectPanel);

            // Object Flags
            TreeViewItem objFlagsPanel = Templates.Tab(true);
            ((objFlagsPanel.Header as Grid).Children[0] as Label).Content = "Object Flags";
            (objFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x78";

            foreach (string key in ObjectFlags.Keys)
            {
                TreeViewItem objFlagPanel = Templates.Checkbox(false);
                ((objFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((objFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = ObjectFlags[key];
                (objFlagPanel.Tag as TagHeader).ParentFlags = ObjectFlags;
                (objFlagPanel.Tag as TagHeader).Flag = key;
                (objFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x78";
                objFlagsPanel.Items.Add(objFlagPanel);
            }

            objPanel.Items.Add(objFlagsPanel);

            // Flags
            TreeViewItem flagsPanel = Templates.Tab(true);
            ((flagsPanel.Header as Grid).Children[0] as Label).Content = "Flags";
            (flagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x7C";

            foreach (string key in Flags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem flagPanel = Templates.Checkbox(false);
                ((flagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((flagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Flags[key];
                (flagPanel.Tag as TagHeader).ParentFlags = Flags;
                (flagPanel.Tag as TagHeader).Flag = key;
                (flagPanel.Tag as TagHeader).ActionType = 1;
                (flagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x7C";
                flagsPanel.Items.Add(flagPanel);
            }

            objPanel.Items.Add(flagsPanel);

            // Layer
            TreeViewItem lyrPanel = Templates.Editbox();
            ((lyrPanel.Header as Grid).Children[0] as Label).Content = "Layer";
            ((lyrPanel.Header as Grid).Children[1] as TextBox).Text = Layer.ToString();
            (lyrPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x84";
            objPanel.Items.Add(lyrPanel);

            // Collision Flags
            TreeViewItem colFlagsPanel = Templates.Tab(true);
            ((colFlagsPanel.Header as Grid).Children[0] as Label).Content = "Collision Flags";
            (colFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x94";

            foreach (string key in CollisionFlags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem colFlagPanel = Templates.Checkbox(false);
                ((colFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((colFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = CollisionFlags[key];
                (colFlagPanel.Tag as TagHeader).ParentFlags = CollisionFlags;
                (colFlagPanel.Tag as TagHeader).Flag = key;
                (colFlagPanel.Tag as TagHeader).ActionType = 1;
                (colFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x94";
                colFlagsPanel.Items.Add(colFlagPanel);
            }

            objPanel.Items.Add(colFlagsPanel);

            // Identifier
            TreeViewItem idPanel = Templates.Editbox();
            ((idPanel.Header as Grid).Children[0] as Label).Content = "Identifier";
            ((idPanel.Header as Grid).Children[1] as TextBox).Text = Identifier;
            ((idPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (idPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xB4";
            objPanel.Items.Add(idPanel);

            panel.Add(objPanel);

            // Animation/Movement Information
            TreeViewItem animMvntInfoPanel = Templates.Tab();
            ((animMvntInfoPanel.Header as Grid).Children[0] as Label).Content = "Animation/Movement Information";

            // Current Player
            TreeViewItem curPlyrPanel = Templates.Editbox();
            ((curPlyrPanel.Header as Grid).Children[0] as Label).Content = "Current Player";
            ((curPlyrPanel.Header as Grid).Children[1] as TextBox).Text = CurrentPlayer.ToString();
            (curPlyrPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xC8";
            animMvntInfoPanel.Items.Add(curPlyrPanel);

            // Current Animation
            TreeViewItem curAnimPanel = Templates.Editbox();
            ((curAnimPanel.Header as Grid).Children[0] as Label).Content = "Current Animation";
            ((curAnimPanel.Header as Grid).Children[1] as TextBox).Text = CurrentAnimation.ToString();
            (curAnimPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xD8";
            animMvntInfoPanel.Items.Add(curAnimPanel);

            // Current Frame
            TreeViewItem curFrmPanel = Templates.Editbox();
            ((curFrmPanel.Header as Grid).Children[0] as Label).Content = "Current Frame";
            ((curFrmPanel.Header as Grid).Children[1] as TextBox).Text = CurrentFrame.ToString();
            (curFrmPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xDC";
            animMvntInfoPanel.Items.Add(curFrmPanel);

            // X Scale
            TreeViewItem scaleXPanel = Templates.Editbox();
            ((scaleXPanel.Header as Grid).Children[0] as Label).Content = "X Scale";
            ((scaleXPanel.Header as Grid).Children[1] as TextBox).Text = XScale.ToString();
            (scaleXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xE0";
            (scaleXPanel.Tag as TagHeader).ActionType = 3;
            animMvntInfoPanel.Items.Add(scaleXPanel);

            // Y Scale
            TreeViewItem scaleYPanel = Templates.Editbox();
            ((scaleYPanel.Header as Grid).Children[0] as Label).Content = "Y Scale";
            ((scaleYPanel.Header as Grid).Children[1] as TextBox).Text = YScale.ToString();
            (scaleYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xE4";
            (scaleYPanel.Tag as TagHeader).ActionType = 3;
            animMvntInfoPanel.Items.Add(scaleYPanel);

            // Angle
            TreeViewItem anglePanel = Templates.Editbox();
            ((anglePanel.Header as Grid).Children[0] as Label).Content = "Angle";
            ((anglePanel.Header as Grid).Children[1] as TextBox).Text = Angle.ToString();
            (anglePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xE8";
            (anglePanel.Tag as TagHeader).ActionType = 3;
            animMvntInfoPanel.Items.Add(anglePanel);

            // Current Direction
            TreeViewItem curDirPanel = Templates.Editbox();
            ((curDirPanel.Header as Grid).Children[0] as Label).Content = "Current Direction";
            ((curDirPanel.Header as Grid).Children[1] as TextBox).Text = CurrentDirection.ToString();
            (curDirPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xEC";
            animMvntInfoPanel.Items.Add(curDirPanel);

            // Current Speed
            TreeViewItem curSpdPanel = Templates.Editbox();
            ((curSpdPanel.Header as Grid).Children[0] as Label).Content = "Current Speed";
            ((curSpdPanel.Header as Grid).Children[1] as TextBox).Text = CurrentSpeed.ToString();
            (curSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xF0";
            animMvntInfoPanel.Items.Add(curSpdPanel);

            // Minimum Speed
            TreeViewItem minSpdPanel = Templates.Editbox();
            ((minSpdPanel.Header as Grid).Children[0] as Label).Content = "Minimum Speed";
            ((minSpdPanel.Header as Grid).Children[1] as TextBox).Text = MinSpeed.ToString();
            (minSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xF4";
            animMvntInfoPanel.Items.Add(minSpdPanel);

            // Maximum Speed
            TreeViewItem maxSpdPanel = Templates.Editbox();
            ((maxSpdPanel.Header as Grid).Children[0] as Label).Content = "Maximum Speed";
            ((maxSpdPanel.Header as Grid).Children[1] as TextBox).Text = MaxSpeed.ToString();
            (maxSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xF8";
            animMvntInfoPanel.Items.Add(maxSpdPanel);

            // Update Object 11
            TreeViewItem updObjPanel = Templates.Checkbox();
            ((updObjPanel.Header as Grid).Children[0] as Label).Content = "Update Object";
            ((updObjPanel.Header as Grid).Children[1] as CheckBox).IsChecked = UpdateObject;
            (updObjPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xFC";
            animMvntInfoPanel.Items.Add(updObjPanel);

            panel.Add(animMvntInfoPanel);

            // Movement Structure
            TreeViewItem mvntStructPanel = Templates.Tab();
            ((mvntStructPanel.Header as Grid).Children[0] as Label).Content = "Movement Structure";

            // Acceleration
            TreeViewItem accPanel = Templates.Editbox();
            ((accPanel.Header as Grid).Children[0] as Label).Content = "Acceleration";
            ((accPanel.Header as Grid).Children[1] as TextBox).Text = Acceleration.ToString();
            (accPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x130";
            mvntStructPanel.Items.Add(accPanel);

            // Decceleration
            TreeViewItem decPanel = Templates.Editbox();
            ((decPanel.Header as Grid).Children[0] as Label).Content = "Decceleration";
            ((decPanel.Header as Grid).Children[1] as TextBox).Text = Decceleration.ToString();
            (decPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x134";
            mvntStructPanel.Items.Add(decPanel);

            // Collision Count
            TreeViewItem colCntPanel = Templates.Editbox();
            ((colCntPanel.Header as Grid).Children[0] as Label).Content = "Collision Count";
            ((colCntPanel.Header as Grid).Children[1] as TextBox).Text = CollisionCount.ToString();
            ((colCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (colCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x138";
            mvntStructPanel.Items.Add(colCntPanel);

            // Stop Speed
            TreeViewItem stopSpdPanel = Templates.Editbox();
            ((stopSpdPanel.Header as Grid).Children[0] as Label).Content = "Stop Speed";
            ((stopSpdPanel.Header as Grid).Children[1] as TextBox).Text = StopSpeed.ToString();
            (stopSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x140";
            mvntStructPanel.Items.Add(stopSpdPanel);

            // Acceleration Value
            TreeViewItem accValPanel = Templates.Editbox();
            ((accValPanel.Header as Grid).Children[0] as Label).Content = "Acceleration Value";
            ((accValPanel.Header as Grid).Children[1] as TextBox).Text = AccelValue.ToString();
            ((accValPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (accValPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x14C";
            mvntStructPanel.Items.Add(accValPanel);

            // Decceleration Value
            TreeViewItem decValuePanel = Templates.Editbox();
            ((decValuePanel.Header as Grid).Children[0] as Label).Content = "Decceleration Value";
            ((decValuePanel.Header as Grid).Children[1] as TextBox).Text = DeccelValue.ToString();
            ((decValuePanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (decValuePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x150";
            mvntStructPanel.Items.Add(decValuePanel);

            // Event Flags
            TreeViewItem evntFlagsPanel = Templates.Tab(true);
            ((evntFlagsPanel.Header as Grid).Children[0] as Label).Content = "Event Flags";
            (evntFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x154";

            foreach (string key in EventFlags.Keys)
            {
                TreeViewItem evntFlagPanel = Templates.Checkbox(false);
                ((evntFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((evntFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = EventFlags[key];
                (evntFlagPanel.Tag as TagHeader).ParentFlags = EventFlags;
                (evntFlagPanel.Tag as TagHeader).Flag = key;
                (evntFlagPanel.Tag as TagHeader).ActionType = 1;
                (evntFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x154";
                evntFlagsPanel.Items.Add(evntFlagPanel);
            }

            objPanel.Items.Add(evntFlagsPanel);

            // Moving
            TreeViewItem movingPanel = Templates.Checkbox();
            ((movingPanel.Header as Grid).Children[0] as Label).Content = "Moving";
            ((movingPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Moving;
            (movingPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x16A";
            mvntStructPanel.Items.Add(movingPanel);

            // Wrapping
            TreeViewItem wrappingPanel = Templates.Checkbox();
            ((wrappingPanel.Header as Grid).Children[0] as Label).Content = "Wrapping";
            ((wrappingPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Wrapping;
            (wrappingPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x16E";
            mvntStructPanel.Items.Add(wrappingPanel);

            // Reverse
            TreeViewItem reversePanel = Templates.Checkbox();
            ((reversePanel.Header as Grid).Children[0] as Label).Content = "Reverse";
            ((reversePanel.Header as Grid).Children[1] as CheckBox).IsChecked = Reverse;
            (reversePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x172";
            mvntStructPanel.Items.Add(reversePanel);

            // Bouncing
            TreeViewItem bouncingPanel = Templates.Checkbox();
            ((bouncingPanel.Header as Grid).Children[0] as Label).Content = "Bouncing";
            ((bouncingPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Bouncing;
            (bouncingPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x176";
            mvntStructPanel.Items.Add(bouncingPanel);

            // Current Movement
            TreeViewItem curMvmtPanel = Templates.Editbox();
            ((curMvmtPanel.Header as Grid).Children[0] as Label).Content = "Current Movement";
            ((curMvmtPanel.Header as Grid).Children[1] as TextBox).Text = CurrentMovement.ToString();
            (curMvmtPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x17A";
            mvntStructPanel.Items.Add(curMvmtPanel);

            panel.Add(mvntStructPanel);

            if (Type <= 32)
            {
                // Additional Structure
                TreeViewItem extraStructPanel = Templates.Tab();
                ((extraStructPanel.Header as Grid).Children[0] as Label).Content = (ActiveObject == null ? "System" : "Active") + " Object Structure";

                if (ActiveObject != null)
                    foreach (var apanel in ActiveObject.GetPanel())
                        extraStructPanel.Items.Add(apanel);

                if (SystemObject != null)
                    foreach (var spanel in SystemObject.GetPanel())
                        extraStructPanel.Items.Add(spanel);

                panel.Add(extraStructPanel);
            }

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
