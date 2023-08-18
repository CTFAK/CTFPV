using ColorPicker;
using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunSystemObject : PropertyPanel
    {
        public int OddOffset;

        // Sprite Structure
        public int Flash;
        public int Layer;
        public int ZOrder;
        public BitDict CreationFlags = new BitDict(new string[]
        {
            "Rambo",
            "Recalculate Surface",
            "Private",
            "Inactive",
            "To Hide",
            "To Kill",
            "Reactivate",
            "Hidden",
            "Collision Box",
            "No Save",
            "Fill Back",
            "Disabled",
            "Inactive Internal",
            "Owner Draw",
            "Owner Save",
            "Fade",
            "Obstacle",
            "Platform",
            "",
            "Background",
            "Scale Resample",
            "Rotate Antialiased",
            "No Hotspot",
            "Owner Collision Mask",
            "Update Collision",
            "True Object"
        });
        public Color BackgroundColor;
        public int Effect;
        public BitDict Flags = new BitDict(new string[]
        {
            "Hidden",
            "Inactive",
            "Sleeping",
            "Scale Resample",
            "Rotate Antialiased",
            "Visible"
        });
        public BitDict FadeCreateFlags = new BitDict(new string[]
        {
            "", "", "", "", "", "", "",
            "Hidden"
        });

        // Universal System Object Values
        public short PlayerCount;
        public BitDict SysFlags = new BitDict(new string[]
        {
            "1"
        });
        public int OldLevel;
        public int Level;
        public CRunValue Value;
        public int Width;
        public int Height;

        // Counter
        public double Minimum;
        public double Maximum;
        public short OldFrame;
        public byte Hidden;

        // String
        public string Text = string.Empty;
        public int Font;
        public Color Color1;
        public Color Color2;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;
            if (PV.CRunApp.ProductBuild >= 292)
                OddOffset = 680;
            else
                OddOffset = 506;

            // Sprite Structure
            Flash = PV.MemLib.ReadInt(parentPointer + ", 0x1CE");
            Layer = PV.MemLib.ReadInt(parentPointer + ", 0x1D6");
            ZOrder = PV.MemLib.ReadInt(parentPointer + ", 0x1DA");
            CreationFlags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x1DE");
            BackgroundColor = PV.MemLib.ReadColor(parentPointer + ", 0x1E2");
            Effect = PV.MemLib.ReadInt(parentPointer + ", 0x1E6");
            Flags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x1EE");
            FadeCreateFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x1F0");

            // Universal System Object Values
            PlayerCount = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x1F2");
            SysFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x1F4");

            // Counter
            OldLevel = PV.MemLib.ReadInt(parentPointer + ", 0x" + OddOffset.ToString("X"));
            Level = PV.MemLib.ReadInt(parentPointer + ", 0x" + (OddOffset + 4).ToString("X"));
            Value = new CRunValue();
            Value.ValueOffset = OddOffset + 8;
            Value.InitData(parentPointer);
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (OddOffset + 24).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (OddOffset + 28).ToString("X"));
            Minimum = PV.MemLib.ReadDouble(parentPointer + ", 0x" + (OddOffset + 32).ToString("X"));
            Maximum = PV.MemLib.ReadDouble(parentPointer + ", 0x" + (OddOffset + 40).ToString("X"));
            OldFrame = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (OddOffset + 48).ToString("X"));
            Hidden = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (OddOffset + 50).ToString("X"));

            // String
            Text = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + (OddOffset + 52).ToString("X") + ", 0x0");
            Font = PV.MemLib.ReadInt(parentPointer + ", 0x" + (OddOffset + 56).ToString("X"));
            Color1 = PV.MemLib.ReadColor(parentPointer + ", 0x" + (OddOffset + 60).ToString("X"));
            Color2 = PV.MemLib.ReadColor(parentPointer + ", 0x" + (OddOffset + 64).ToString("X"));
        }

        public override void RefreshData(string parentPointer)
        {

        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Sprite Structure
            TreeViewItem sprStructPanel = Templates.Tab();
            ((sprStructPanel.Header as Grid).Children[0] as Label).Content = "Sprite Structure";

            // Z Order
            TreeViewItem orderZPanel = Templates.Editbox();
            ((orderZPanel.Header as Grid).Children[0] as Label).Content = "Z Order";
            ((orderZPanel.Header as Grid).Children[1] as TextBox).Text = ZOrder.ToString();
            (orderZPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1DA";
            sprStructPanel.Items.Add(orderZPanel);

            // Creation Flags
            TreeViewItem creationFlagsPanel = Templates.Tab(true);
            ((creationFlagsPanel.Header as Grid).Children[0] as Label).Content = "Creation Flags";
            (creationFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1DE";

            foreach (string key in CreationFlags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem creationFlagPanel = Templates.Checkbox(false);
                ((creationFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((creationFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = CreationFlags[key];
                (creationFlagPanel.Tag as TagHeader).ParentFlags = CreationFlags;
                (creationFlagPanel.Tag as TagHeader).Flag = key;
                (creationFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1DE";
                creationFlagsPanel.Items.Add(creationFlagPanel);
            }

            sprStructPanel.Items.Add(creationFlagsPanel);

            panel.Add(sprStructPanel);

            // Sprite Structure
            TreeViewItem sysObjStructPanel = Templates.Tab();
            ((sysObjStructPanel.Header as Grid).Children[0] as Label).Content = "System Object Structure";

            // Player Count
            TreeViewItem plyrCntPanel = Templates.Editbox();
            ((plyrCntPanel.Header as Grid).Children[0] as Label).Content = "Player Count";
            ((plyrCntPanel.Header as Grid).Children[1] as TextBox).Text = PlayerCount.ToString();
            ((plyrCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (plyrCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F2";
            sysObjStructPanel.Items.Add(plyrCntPanel);

            // Flags
            TreeViewItem flagsPanel = Templates.Tab(true);
            ((flagsPanel.Header as Grid).Children[0] as Label).Content = "Flags";
            (flagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F4";

            foreach (string key in Flags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem flagPanel = Templates.Checkbox(false);
                ((flagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((flagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Flags[key];
                (flagPanel.Tag as TagHeader).ParentFlags = Flags;
                (flagPanel.Tag as TagHeader).Flag = key;
                (flagPanel.Tag as TagHeader).ActionType = 1;
                (flagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F4";
                flagsPanel.Items.Add(flagPanel);
            }

            // Width
            TreeViewItem widthPanel = Templates.Editbox();
            ((widthPanel.Header as Grid).Children[0] as Label).Content = "Width";
            ((widthPanel.Header as Grid).Children[1] as TextBox).Text = Width.ToString();
            (widthPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 24).ToString("X");
            sysObjStructPanel.Items.Add(widthPanel);

            // Height
            TreeViewItem heightPanel = Templates.Editbox();
            ((heightPanel.Header as Grid).Children[0] as Label).Content = "Height";
            ((heightPanel.Header as Grid).Children[1] as TextBox).Text = Height.ToString();
            (heightPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 28).ToString("X");
            sysObjStructPanel.Items.Add(heightPanel);

            panel.Add(sysObjStructPanel);

            sysObjStructPanel.Items.Add(flagsPanel);

            // Counter Structure
            TreeViewItem cntrStructPanel = Templates.Tab();
            ((cntrStructPanel.Header as Grid).Children[0] as Label).Content = "Counter Structure";

            // Old Level
            TreeViewItem oldLvlPanel = Templates.Editbox();
            ((oldLvlPanel.Header as Grid).Children[0] as Label).Content = "Minimum";
            ((oldLvlPanel.Header as Grid).Children[1] as TextBox).Text = OldLevel.ToString();
            (oldLvlPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + OddOffset.ToString("X");
            cntrStructPanel.Items.Add(oldLvlPanel);

            // Level
            TreeViewItem lvlPanel = Templates.Editbox();
            ((lvlPanel.Header as Grid).Children[0] as Label).Content = "Maximum";
            ((lvlPanel.Header as Grid).Children[1] as TextBox).Text = Level.ToString();
            (lvlPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 4).ToString("X");
            cntrStructPanel.Items.Add(lvlPanel);

            // Value
            TreeViewItem valPanel = Templates.Editbox();
            ((valPanel.Header as Grid).Children[0] as Label).Content = "Value";

            string valueText = string.Empty;
            switch (Value.Type)
            {
                case 0: // Int
                    valueText = (((int)Value.Value() * -1) - 1).ToString();
                    (valPanel.Tag as TagHeader).FlipCounter = true;
                    break;
                case 1: // String
                    valueText = Value.Value().ToString();
                    (valPanel.Tag as TagHeader).ActionType = 5;
                    break;
                case 2: // Double
                    valueText = Value.Value().ToString();
                    (valPanel.Tag as TagHeader).ActionType = 4;
                    break;
            }
            ((valPanel.Header as Grid).Children[1] as TextBox).Text = valueText;
            (valPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 16).ToString("X");

            cntrStructPanel.Items.Add(valPanel);

            // Old Frame
            TreeViewItem oldFrmPanel = Templates.Editbox();
            ((oldFrmPanel.Header as Grid).Children[0] as Label).Content = "Old Frame";
            ((oldFrmPanel.Header as Grid).Children[1] as TextBox).Text = OldFrame.ToString();
            (oldFrmPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 48).ToString("X");
            (oldFrmPanel.Tag as TagHeader).ActionType = 1;
            cntrStructPanel.Items.Add(oldFrmPanel);

            // Hidden
            TreeViewItem hiddenPanel = Templates.Editbox();
            ((hiddenPanel.Header as Grid).Children[0] as Label).Content = "Hidden";
            ((hiddenPanel.Header as Grid).Children[1] as TextBox).Text = Hidden.ToString();
            (hiddenPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 50).ToString("X");
            (hiddenPanel.Tag as TagHeader).ActionType = 0;
            cntrStructPanel.Items.Add(hiddenPanel);

            panel.Add(cntrStructPanel);

            // String Structure
            TreeViewItem strStructPanel = Templates.Tab();
            ((strStructPanel.Header as Grid).Children[0] as Label).Content = "String Structure";

            // Text
            TreeViewItem textPanel = Templates.Editbox();
            ((textPanel.Header as Grid).Children[0] as Label).Content = "Text";
            ((textPanel.Header as Grid).Children[1] as TextBox).Text = Text;
            (textPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 52).ToString("X") + ", 0x0";
            (textPanel.Tag as TagHeader).ActionType = 5;
            strStructPanel.Items.Add(textPanel);

            // Font
            TreeViewItem fontPanel = Templates.Editbox();
            ((fontPanel.Header as Grid).Children[0] as Label).Content = "Font";
            ((fontPanel.Header as Grid).Children[1] as TextBox).Text = Font.ToString();
            (fontPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 56).ToString("X");
            strStructPanel.Items.Add(fontPanel);

            // Color 1
            TreeViewItem primaryColorPanel = Templates.ColorPicker();
            ((primaryColorPanel.Header as Grid).Children[0] as Label).Content = "Primary Color";
            ((primaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_R = Color1.R;
            ((primaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_G = Color1.G;
            ((primaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_B = Color1.B;
            ((primaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.A = 255;
            ((primaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).ShowAlpha = false;
            (primaryColorPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 60).ToString("X");
            strStructPanel.Items.Add(primaryColorPanel);

            // Color 2
            TreeViewItem secondaryColorPanel = Templates.ColorPicker();
            ((secondaryColorPanel.Header as Grid).Children[0] as Label).Content = "Secondary Color";
            ((secondaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_R = Color2.R;
            ((secondaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_G = Color2.G;
            ((secondaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_B = Color2.B;
            ((secondaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.A = 255;
            ((secondaryColorPanel.Header as Grid).Children[1] as PortableColorPicker).ShowAlpha = false;
            (secondaryColorPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (OddOffset + 60).ToString("X");
            strStructPanel.Items.Add(secondaryColorPanel);

            panel.Add(strStructPanel);

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
