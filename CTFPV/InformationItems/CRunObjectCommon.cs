using ColorPicker;
using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunObjectCommon : PropertyPanel
    {
        // Object Common
        public int StructureSize;
        public ushort MovementOffset;
        public ushort AnimationOffset;
        public ushort Version;
        public ushort CounterOffset;
        public ushort DataOffset;
        public BitDict Flags = new BitDict(new string[]
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
        public short[] Qualifiers = new short[0];
        public ushort ExtensionOffset;
        public ushort ValuesOffset;
        public ushort StringsOffset;
        public BitDict NewFlags = new BitDict(new string[]
        {
            "Don't Save Backdrop",
            "Solid Backdrop",
            "Collision Box",
            "Visible at Start"
        });
        public BitDict Preferences = new BitDict(new string[]
        {
            "Backsave",
            "Scrolling Independent",
            "Quick Display",
            "Sleep",
            "Load On Call",
            "Global",
            "Back Effects",
            "Kill",
            "Ink Effects",
            "Transitions",
            "Fine Collisions"
        });
        public string Identifier = string.Empty;
        public System.Drawing.Color BackgroundColor;
        public int FadeInOffset;
        public int FadeOutOffset;

        // Parsed Offsets
        public CRunOCMovements CRunOCMovements;
        public CRunOCAnimations CRunOCAnimations;
        public CRunOCCounter CRunOCCounter;
        public CRunOCExtension CRunOCExtension;
        public CRunOCAltValues CRunOCAltValues;
        public CRunOCFade CRunOCFadeIn;
        public CRunOCFade CRunOCFadeOut;

        // Parsed Data Offset
        public CRunOCString CRunOCString;
        public CRunOCCounters CRunOCCounters;
        public CRunOCSubApp CRunOCSubApp;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Object Common
            StructureSize = PV.MemLib.ReadInt(parentPointer + ", 0x0");
            MovementOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x4");
            AnimationOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x6");
            Version = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x8");
            CounterOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0xA");
            DataOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0xC");
            Flags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x10");
            Qualifiers = new short[8];
            for (int i = 0; i < 8; i++)
                Qualifiers[i] = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (20 + (i * 2)).ToString("X"));
            ExtensionOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x24");
            ValuesOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x26");
            StringsOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x28");
            NewFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x2A");
            Preferences.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x2C");
            Identifier = PV.MemLib.ReadString(parentPointer + ", 0x2E", length: 4, stringEncoding: Encoding.ASCII);
            BackgroundColor = PV.MemLib.ReadColor(parentPointer + ", 0x32");
            FadeInOffset = PV.MemLib.ReadInt(parentPointer + ", 0x36");
            FadeOutOffset = PV.MemLib.ReadInt(parentPointer + ", 0x3A");

            // Parsed Offsets
            if (MovementOffset > 0)
            {
                CRunOCMovements = new CRunOCMovements();
                CRunOCMovements.MovementOffset = MovementOffset;
                CRunOCMovements.InitData(parentPointer);
            }

            if (AnimationOffset > 0)
            {
                CRunOCAnimations = new CRunOCAnimations();
                CRunOCAnimations.AnimationOffset = AnimationOffset;
                CRunOCAnimations.InitData(parentPointer);
            }

            if (CounterOffset > 0)
            {
                CRunOCCounter = new CRunOCCounter();
                CRunOCCounter.CounterOffset = CounterOffset;
                CRunOCCounter.InitData(parentPointer);
            }

            if (ExtensionOffset > 0)
            {
                CRunOCExtension = new CRunOCExtension();
                CRunOCExtension.ExtensionOffset = ExtensionOffset;
                CRunOCExtension.InitData(parentPointer);
            }

            if (ValuesOffset > 0 || StringsOffset > 0)
            {
                CRunOCAltValues = new CRunOCAltValues();
                CRunOCAltValues.ValuesOffset = ValuesOffset;
                CRunOCAltValues.StringsOffset = StringsOffset;
                CRunOCAltValues.InitData(parentPointer);
            }

            if (FadeInOffset > 0)
            {
                CRunOCFadeIn = new CRunOCFade();
                CRunOCFadeIn.FadeOffset = FadeInOffset;
                CRunOCFadeIn.InitData(parentPointer);
            }

            if (FadeOutOffset > 0)
            {
                CRunOCFadeOut = new CRunOCFade();
                CRunOCFadeOut.FadeOffset = FadeOutOffset;
                CRunOCFadeOut.InitData(parentPointer);
            }

            // Parsed Data Offset
            if (DataOffset > 0)
            {
                switch (Identifier)
                {
                    // String
                    case "XTÿÿ":
                    case "TE":
                    case "TEXT":
                        CRunOCString = new CRunOCString();
                        CRunOCString.StringOffset = DataOffset;
                        CRunOCString.InitData(parentPointer);
                        break;

                    // Counter
                    case "TRÿÿ":
                    case "CNTR":
                    case "LIVE":
                    case "CN":
                        CRunOCCounters = new CRunOCCounters();
                        CRunOCCounters.CountersOffset = DataOffset;
                        CRunOCCounters.InitData(parentPointer);
                        break;

                    //Sub-Application
                    case "CCA ":
                        CRunOCSubApp = new CRunOCSubApp();
                        CRunOCSubApp.SubAppOffset = DataOffset;
                        CRunOCSubApp.InitData(parentPointer);
                        break;
                }
            }
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Object Common
            MovementOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x4");
            AnimationOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x6");
            CounterOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0xA");
            DataOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0xC");
            Flags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x10");
            ExtensionOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x24");
            ValuesOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x26");
            StringsOffset = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x28");
            NewFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x2A");
            Preferences.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x2C");
            BackgroundColor = PV.MemLib.ReadColor(parentPointer + ", 0x32");

            // Parsed Offsets
            if (CRunOCMovements == null && MovementOffset > 0)
            {
                CRunOCMovements = new CRunOCMovements();
                CRunOCMovements.MovementOffset = MovementOffset;
                CRunOCMovements.InitData(parentPointer);
            }

            if (CRunOCAnimations == null && AnimationOffset > 0)
            {
                CRunOCAnimations = new CRunOCAnimations();
                CRunOCAnimations.AnimationOffset = AnimationOffset;
                CRunOCAnimations.InitData(parentPointer);
            }

            if (CRunOCCounter != null)
            {
                CRunOCCounter.CounterOffset = CounterOffset;
                CRunOCCounter.RefreshData(parentPointer);
            }
            else if (CounterOffset > 0)
            {
                CRunOCCounter = new CRunOCCounter();
                CRunOCCounter.CounterOffset = CounterOffset;
                CRunOCCounter.InitData(parentPointer);
            }

            if (CRunOCExtension == null && ExtensionOffset > 0)
            {
                CRunOCExtension = new CRunOCExtension();
                CRunOCExtension.ExtensionOffset = ExtensionOffset;
                CRunOCExtension.InitData(parentPointer);
            }

            if (CRunOCAltValues != null)
            {
                CRunOCAltValues.ValuesOffset = ValuesOffset;
                CRunOCAltValues.StringsOffset = StringsOffset;
                CRunOCAltValues.RefreshData(parentPointer);
            }
            else if (ValuesOffset > 0 || StringsOffset > 0)
            {
                CRunOCAltValues = new CRunOCAltValues();
                CRunOCAltValues.ValuesOffset = ValuesOffset;
                CRunOCAltValues.StringsOffset = StringsOffset;
                CRunOCAltValues.InitData(parentPointer);
            }

            if (CRunOCFadeIn == null && FadeInOffset > 0)
            {
                CRunOCFadeIn = new CRunOCFade();
                CRunOCFadeIn.FadeOffset = FadeInOffset;
                CRunOCFadeIn.InitData(parentPointer);
            }

            if (CRunOCFadeOut == null && FadeOutOffset > 0)
            {
                CRunOCFadeOut = new CRunOCFade();
                CRunOCFadeOut.FadeOffset = FadeOutOffset;
                CRunOCFadeOut.InitData(parentPointer);
            }

            // Parsed Data Offset
            if (DataOffset > 0)
            {
                switch (Identifier)
                {
                    // String
                    case "XTÿÿ":
                    case "TE":
                    case "TEXT":
                        if (CRunOCString != null)
                        {
                            CRunOCString.StringOffset = DataOffset;
                            CRunOCString.RefreshData(parentPointer);
                        }
                        else
                        {
                            CRunOCString = new CRunOCString();
                            CRunOCString.StringOffset = DataOffset;
                            CRunOCString.InitData(parentPointer);
                        }
                        break;

                    // Counter
                    case "TRÿÿ":
                    case "CNTR":
                    case "LIVE":
                    case "CN":
                        if (CRunOCCounters != null)
                        {
                            CRunOCCounters.CountersOffset = DataOffset;
                            CRunOCCounters.RefreshData(parentPointer);
                        }
                        else
                        {
                            CRunOCCounters = new CRunOCCounters();
                            CRunOCCounters.CountersOffset = DataOffset;
                            CRunOCCounters.InitData(parentPointer);
                        }
                        break;

                    //Sub-Application
                    case "CCA ":
                        if (CRunOCSubApp != null)
                        {
                            CRunOCSubApp.SubAppOffset = DataOffset;
                            CRunOCSubApp.RefreshData(parentPointer);
                        }
                        else
                        {
                            CRunOCSubApp = new CRunOCSubApp();
                            CRunOCSubApp.SubAppOffset = DataOffset;
                            CRunOCSubApp.InitData(parentPointer);
                        }
                        break;
                }
            }
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Flags
            TreeViewItem flagsPanel = Templates.Tab(true);
            ((flagsPanel.Header as Grid).Children[0] as Label).Content = "Flags";
            (flagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x10";

            foreach (string key in Flags.Keys)
            {
                TreeViewItem flagPanel = Templates.Checkbox(false);
                ((flagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((flagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Flags[key];
                (flagPanel.Tag as TagHeader).ParentFlags = Flags;
                (flagPanel.Tag as TagHeader).Flag = key;
                flagsPanel.Items.Add(flagPanel);
            }

            panel.Add(flagsPanel);

            // Qualifiers
            TreeViewItem qualsPanel = Templates.Tab();
            ((qualsPanel.Header as Grid).Children[0] as Label).Content = "Qualifers";
            for (int i = 0; i < 8; i++)
            {
                TreeViewItem qualPanel = Templates.Editbox();
                ((qualPanel.Header as Grid).Children[0] as Label).Content = "Qualifier " + i;
                ((qualPanel.Header as Grid).Children[1] as TextBox).Text = Qualifiers[i].ToString();
                (qualPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (20 + (i * 2)).ToString("X");
                (qualPanel.Tag as TagHeader).ActionType = 1;
                qualsPanel.Items.Add(qualPanel);
            }
            panel.Add(qualsPanel);

            // New Flags
            TreeViewItem newFlagsPanel = Templates.Tab(true);
            ((newFlagsPanel.Header as Grid).Children[0] as Label).Content = "New Flags";
            (newFlagsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x2A";

            foreach (string key in NewFlags.Keys)
            {
                TreeViewItem newFlagPanel = Templates.Checkbox(false);
                ((newFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((newFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = NewFlags[key];
                (newFlagPanel.Tag as TagHeader).ParentFlags = NewFlags;
                (newFlagPanel.Tag as TagHeader).Flag = key;
                (newFlagPanel.Tag as TagHeader).ActionType = 1;
                newFlagsPanel.Items.Add(newFlagPanel);
            }

            panel.Add(newFlagsPanel);

            // Preferences
            TreeViewItem prefsPanel = Templates.Tab();
            ((prefsPanel.Header as Grid).Children[0] as Label).Content = "Preferences";
            (prefsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x2C";

            foreach (string key in Preferences.Keys)
            {
                TreeViewItem prefPanel = Templates.Checkbox(false);
                ((prefPanel.Header as Grid).Children[0] as Label).Content = key;
                ((prefPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Preferences[key];
                (prefPanel.Tag as TagHeader).ParentFlags = NewFlags;
                (prefPanel.Tag as TagHeader).Flag = key;
                (prefPanel.Tag as TagHeader).ActionType = 1;
                prefsPanel.Items.Add(prefPanel);
            }

            panel.Add(prefsPanel);

            // Identifier
            TreeViewItem idPanel = Templates.Editbox();
            ((idPanel.Header as Grid).Children[0] as Label).Content = "Identifier";
            ((idPanel.Header as Grid).Children[1] as TextBox).Text = Identifier;
            ((idPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (idPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x2E";
            panel.Add(idPanel);

            // Background Color
            TreeViewItem bgColorPanel = Templates.ColorPicker();
            ((bgColorPanel.Header as Grid).Children[0] as Label).Content = "Background Color";
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_R = BackgroundColor.R;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_G = BackgroundColor.G;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.RGB_B = BackgroundColor.B;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).Color.A = 255;
            ((bgColorPanel.Header as Grid).Children[1] as PortableColorPicker).ShowAlpha = false;
            (bgColorPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x32";
            panel.Add(bgColorPanel);

            if (CRunOCString != null)
            {
                // String Common
                TreeViewItem strCommonPanel = Templates.Tab();
                ((strCommonPanel.Header as Grid).Children[0] as Label).Content = "String Common";
                panel.Add(strCommonPanel);
            }

            if (CRunOCCounters != null)
            {
                // Counter Common
                TreeViewItem cntrCommonPanel = Templates.Tab();
                ((cntrCommonPanel.Header as Grid).Children[0] as Label).Content = "Counter Common";
                panel.Add(cntrCommonPanel);
            }

            if (CRunOCSubApp != null)
            {
                // Sub-App Common
                TreeViewItem subAppCommonPanel = Templates.Tab();
                ((subAppCommonPanel.Header as Grid).Children[0] as Label).Content = "Sub-App Common";
                panel.Add(subAppCommonPanel);
            }

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
