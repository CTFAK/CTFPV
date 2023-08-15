using ColorPicker;
using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunActiveObject : PropertyPanel
    {
        // Animation Structure
        public bool CurrentAnimationStopped;
        public int CurrentAnimation;
        public int CurrentDirection;
        public int CurrentSpeed;
        public int MinSpeed;
        public int MaxSpeed;
        public int Repeats;
        public int Looping;
        public int CurrentFrame;
        public int CurrentAnimationFrameCount;

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

        // Alterable Values
        public CRunValue[] AlterableValues;
        public int AlterableFlags;
        public string[] AlterableStrings;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Animation Structure
            CurrentAnimationStopped = PV.MemLib.ReadInt(parentPointer + ", 0x1DA") == 1;
            CurrentAnimation = PV.MemLib.ReadInt(parentPointer + ", 0x1DE");
            CurrentDirection = PV.MemLib.ReadInt(parentPointer + ", 0x1E6");
            CurrentSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x1F2");
            MinSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x1F6");
            MaxSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x1FA");
            Repeats = PV.MemLib.ReadInt(parentPointer + ", 0x20A");
            Looping = PV.MemLib.ReadInt(parentPointer + ", 0x20E");
            CurrentFrame = PV.MemLib.ReadInt(parentPointer + ", 0x212");
            CurrentAnimationFrameCount = PV.MemLib.ReadInt(parentPointer + ", 0x216");

            // Sprite Structure
            Flash = PV.MemLib.ReadInt(parentPointer + ", 0x21E");
            Layer = PV.MemLib.ReadInt(parentPointer + ", 0x226");
            ZOrder = PV.MemLib.ReadInt(parentPointer + ", 0x22A");
            CreationFlags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x22E");
            BackgroundColor = PV.MemLib.ReadColor(parentPointer + ", 0x232");
            Effect = PV.MemLib.ReadInt(parentPointer + ", 0x236");
            Flags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x23E");
            FadeCreateFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x240");

            // Alterable Values
            AlterableValues = new CRunValue[26];
            for (int i = 0; i < 26; i++)
            {
                CRunValue val = new CRunValue();
                val.ValueOffset = i * 16;
                val.InitData(parentPointer + ", 0x232");
                AlterableValues[i] = val;
            }
            AlterableFlags = PV.MemLib.ReadInt(parentPointer + ", 0x29A");
            AlterableStrings = new string[26];
            for (int i = 0; i < 26; i++)
                AlterableStrings[i] = PV.MemLib.ReadString(parentPointer + ", 0x" + ((i * 4) + 696).ToString("X") + ", 0x0", length: 255, stringEncoding: Encoding.Unicode);
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Animation Structure
            CurrentAnimationStopped = PV.MemLib.ReadInt(parentPointer + ", 0x1DA") == 1;
            CurrentAnimation = PV.MemLib.ReadInt(parentPointer + ", 0x1DE");
            CurrentDirection = PV.MemLib.ReadInt(parentPointer + ", 0x1E6");
            CurrentSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x1F2");
            MinSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x1F6");
            MaxSpeed = PV.MemLib.ReadInt(parentPointer + ", 0x1FA");
            Repeats = PV.MemLib.ReadInt(parentPointer + ", 0x20A");
            Looping = PV.MemLib.ReadInt(parentPointer + ", 0x20E");
            CurrentFrame = PV.MemLib.ReadInt(parentPointer + ", 0x212");
            CurrentAnimationFrameCount = PV.MemLib.ReadInt(parentPointer + ", 0x216");

            // Sprite Structure
            ZOrder = PV.MemLib.ReadInt(parentPointer + ", 0x22A");

            // Alterable Values
            AlterableValues = new CRunValue[26];
            for (int i = 0; i < 26; i++)
            {
                CRunValue val = new CRunValue();
                val.ValueOffset = i * 16;
                val.InitData(parentPointer + ", 0x232");
                AlterableValues[i] = val;
            }
            AlterableFlags = PV.MemLib.ReadInt(parentPointer + ", 0x29A");
            AlterableStrings = new string[26];
            for (int i = 0; i < 26; i++)
                AlterableStrings[i] = PV.MemLib.ReadString(parentPointer + ", 0x" + ((i * 4) + 696).ToString("X") + ", 0x0", length: 255, stringEncoding: Encoding.Unicode);
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Animation Structure
            TreeViewItem animStructPanel = Templates.Tab();
            ((animStructPanel.Header as Grid).Children[0] as Label).Content = "Animation Structure";

            // Current Animation Stopped
            TreeViewItem curAnimStoppedPanel = Templates.Checkbox();
            ((curAnimStoppedPanel.Header as Grid).Children[0] as Label).Content = "Current Animation Stopped";
            ((curAnimStoppedPanel.Header as Grid).Children[1] as CheckBox).IsChecked = CurrentAnimationStopped;
            (curAnimStoppedPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1DA";
            animStructPanel.Items.Add(curAnimStoppedPanel);

            // Current Animation
            TreeViewItem curAnimPanel = Templates.Editbox();
            ((curAnimPanel.Header as Grid).Children[0] as Label).Content = "Current Animation";
            ((curAnimPanel.Header as Grid).Children[1] as TextBox).Text = CurrentAnimation.ToString();
            (curAnimPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1DE";
            animStructPanel.Items.Add(curAnimPanel);

            // Current Direction
            TreeViewItem curDirPanel = Templates.Editbox();
            ((curDirPanel.Header as Grid).Children[0] as Label).Content = "Current Direction";
            ((curDirPanel.Header as Grid).Children[1] as TextBox).Text = CurrentDirection.ToString();
            (curDirPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1E6";
            animStructPanel.Items.Add(curDirPanel);

            // Current Speed
            TreeViewItem curSpdPanel = Templates.Editbox();
            ((curSpdPanel.Header as Grid).Children[0] as Label).Content = "Current Speed";
            ((curSpdPanel.Header as Grid).Children[1] as TextBox).Text = CurrentSpeed.ToString();
            (curSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F2";
            animStructPanel.Items.Add(curSpdPanel);

            // Minimum Speed
            TreeViewItem minSpdPanel = Templates.Editbox();
            ((minSpdPanel.Header as Grid).Children[0] as Label).Content = "Minimum Speed";
            ((minSpdPanel.Header as Grid).Children[1] as TextBox).Text = MinSpeed.ToString();
            (minSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1F6";
            animStructPanel.Items.Add(minSpdPanel);

            // Maximum Speed
            TreeViewItem maxSpdPanel = Templates.Editbox();
            ((maxSpdPanel.Header as Grid).Children[0] as Label).Content = "Maximum Speed";
            ((maxSpdPanel.Header as Grid).Children[1] as TextBox).Text = MaxSpeed.ToString();
            (maxSpdPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1FA";
            animStructPanel.Items.Add(maxSpdPanel);

            // Repeats
            TreeViewItem repeatsPanel = Templates.Editbox();
            ((repeatsPanel.Header as Grid).Children[0] as Label).Content = "Repeats";
            ((repeatsPanel.Header as Grid).Children[1] as TextBox).Text = Repeats.ToString();
            (repeatsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x20A";
            animStructPanel.Items.Add(repeatsPanel);

            // Looping
            TreeViewItem loopingPanel = Templates.Editbox();
            ((loopingPanel.Header as Grid).Children[0] as Label).Content = "Looping";
            ((loopingPanel.Header as Grid).Children[1] as TextBox).Text = Looping.ToString();
            (loopingPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x20E";
            animStructPanel.Items.Add(loopingPanel);

            // Current Frame
            TreeViewItem curFrmPanel = Templates.Editbox();
            ((curFrmPanel.Header as Grid).Children[0] as Label).Content = "Current Frame";
            ((curFrmPanel.Header as Grid).Children[1] as TextBox).Text = CurrentFrame.ToString();
            (curFrmPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x212";
            animStructPanel.Items.Add(curFrmPanel);

            // Current Animation Frame Count
            TreeViewItem curAnimFrmCntPanel = Templates.Editbox();
            ((curAnimFrmCntPanel.Header as Grid).Children[0] as Label).Content = "Current Animation Frame Count";
            ((curAnimFrmCntPanel.Header as Grid).Children[1] as TextBox).Text = CurrentAnimationFrameCount.ToString();
            ((curAnimFrmCntPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (curAnimFrmCntPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x216";
            animStructPanel.Items.Add(curAnimFrmCntPanel);

            panel.Add(animStructPanel);

            // Sprite Structure
            TreeViewItem sprStructPanel = Templates.Tab();
            ((sprStructPanel.Header as Grid).Children[0] as Label).Content = "Sprite Structure";

            // Flash
            TreeViewItem flashPanel = Templates.Editbox();
            ((flashPanel.Header as Grid).Children[0] as Label).Content = "Flash";
            ((flashPanel.Header as Grid).Children[1] as TextBox).Text = Flash.ToString();
            (flashPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x21E";
            //sprStructPanel.Items.Add(flashPanel);

            // Layer
            TreeViewItem layerPanel = Templates.Editbox();
            ((layerPanel.Header as Grid).Children[0] as Label).Content = "Layer";
            ((layerPanel.Header as Grid).Children[1] as TextBox).Text = Layer.ToString();
            (layerPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x226";
            sprStructPanel.Items.Add(layerPanel);

            // Z Order
            TreeViewItem orderZPanel = Templates.Editbox();
            ((orderZPanel.Header as Grid).Children[0] as Label).Content = "Z Order";
            ((orderZPanel.Header as Grid).Children[1] as TextBox).Text = ZOrder.ToString();
            (orderZPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x22A";
            sprStructPanel.Items.Add(orderZPanel);

            // Creation Flags
            TreeViewItem creationFlagsPanel = Templates.Tab(true);
            ((creationFlagsPanel.Header as Grid).Children[0] as Label).Content = "Creation Flags";
            (creationFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x22E";

            foreach (string key in CreationFlags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem creationFlagPanel = Templates.Checkbox(false);
                ((creationFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((creationFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = CreationFlags[key];
                (creationFlagPanel.Tag as TagHeader).ParentFlags = CreationFlags;
                (creationFlagPanel.Tag as TagHeader).Flag = key;
                creationFlagsPanel.Items.Add(creationFlagPanel);
            }

            sprStructPanel.Items.Add(creationFlagsPanel);

            // Background Color
            TreeViewItem bgColorPanel = Templates.ColorPicker();
            ((bgColorPanel.Header as Grid).Children[0] as Label).Content = "Background Color";
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_R = BackgroundColor.R;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_G = BackgroundColor.G;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_B = BackgroundColor.B;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.A = 255;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).ShowAlpha = false;
            (bgColorPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x232";
            sprStructPanel.Items.Add(bgColorPanel);

            // Effect
            TreeViewItem effectPanel = Templates.Editbox();
            ((effectPanel.Header as Grid).Children[0] as Label).Content = "Effect";
            ((effectPanel.Header as Grid).Children[1] as TextBox).Text = Effect.ToString();
            (effectPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x236";
            sprStructPanel.Items.Add(effectPanel);

            // Flags
            TreeViewItem flagsPanel = Templates.Tab(true);
            ((flagsPanel.Header as Grid).Children[0] as Label).Content = "Flags";
            (flagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x23E";

            foreach (string key in Flags.Keys)
            {
                TreeViewItem flagPanel = Templates.Checkbox(false);
                ((flagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((flagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Flags[key];
                (flagPanel.Tag as TagHeader).ParentFlags = Flags;
                (flagPanel.Tag as TagHeader).Flag = key;
                flagsPanel.Items.Add(flagPanel);
            }

            sprStructPanel.Items.Add(flagsPanel);

            // Flags
            TreeViewItem fadeCreateFlagsPanel = Templates.Tab(true);
            ((fadeCreateFlagsPanel.Header as Grid).Children[0] as Label).Content = "Fade Create Flags";
            (fadeCreateFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x240";

            foreach (string key in FadeCreateFlags.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem fadeCreateFlagPanel = Templates.Checkbox(false);
                ((fadeCreateFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((fadeCreateFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = FadeCreateFlags[key];
                (fadeCreateFlagPanel.Tag as TagHeader).ParentFlags = FadeCreateFlags;
                (fadeCreateFlagPanel.Tag as TagHeader).Flag = key;
                fadeCreateFlagsPanel.Items.Add(fadeCreateFlagPanel);
            }

            sprStructPanel.Items.Add(fadeCreateFlagsPanel);

            panel.Add(sprStructPanel);

            // Alterables
            TreeViewItem altsPanel = Templates.Tab();
            ((altsPanel.Header as Grid).Children[0] as Label).Content = "Alterables";

            // Alterable Values
            TreeViewItem altValsPanel = Templates.Tab(true);
            ((altValsPanel.Header as Grid).Children[0] as Label).Content = "Alterable Values";
            (altValsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x232";
            altsPanel.Items.Add(altValsPanel);

            // Alterable Flags
            TreeViewItem altFlagsPanel = Templates.Tab(true);
            ((altFlagsPanel.Header as Grid).Children[0] as Label).Content = "Alterable Flags";
            (altFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x29A";
            altsPanel.Items.Add(altFlagsPanel);

            // Alterable String
            TreeViewItem altStrsPanel = Templates.Tab();
            ((altStrsPanel.Header as Grid).Children[0] as Label).Content = "Alterable Strings";
            (altStrsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x2B8";
            altsPanel.Items.Add(altStrsPanel);

            panel.Add(altsPanel);

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
