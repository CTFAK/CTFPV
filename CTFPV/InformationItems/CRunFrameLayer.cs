using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunFrameLayer : PropertyPanel
    {
        public int LayerOffset;

        // Information
        public string Name = string.Empty;

        // Offset
        public int OffsetX;
        public int OffsetY;
        public int OffsetDeltaX;
        public int OffsetDeltaY;

        // Backdrops
        public int BackdropCount;
        public CRunBackdrop[] Backdrops = new CRunBackdrop[0];

        // Ladders
        public int MaxLadders;
        public int LadderCount;
        public Rect[] Ladders = new Rect[0];

        // Z-Order
        public int MaxZOrder;

        // Frame Data
        public BitDict Options = new BitDict(new string[]
        {
            "X Coefficient",
            "Y Coefficient",
            "Don't Save Backdrop",
            "Wrap (Obsolete)",
            "Visible",
            "Wrap Horizontally",
            "Wrap Vertically",
            "", "", "", "", "", "", "", "", "",
            "Redraw",
            "To Hide",
            "To Show"
        });
        public float XCoefficient;
        public float YCoefficient;
        public int NumberOfBackdrops;
        public int FirstBackdropIndex;

        // Efects
        public int Effect;
        public int EffectParam;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Information
            Name = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + LayerOffset.ToString("X") + ", 0x0");

            // Offset
            OffsetX = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 4).ToString("X"));
            OffsetY = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 8).ToString("X"));
            OffsetDeltaX = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 12).ToString("X"));
            OffsetDeltaY = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 16).ToString("X"));

            // Backdrops
            BackdropCount = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 20).ToString("X"));
            Backdrops = new CRunBackdrop[BackdropCount];
            for (int i = 0; i < BackdropCount; i++)
            {
                Backdrops[i] = new CRunBackdrop();
                //Backdrops[i].InitData(parentPointer + ", 0x" + (LayerOffset + 24).ToString("X") + ", 0x" + (i * 4).ToString("X"));
            }

            // Ladders
            MaxLadders = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 28).ToString("X"));
            LadderCount = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 32).ToString("X"));
            Ladders = new Rect[LadderCount];
            for (int i = 0; i < LadderCount; i++)
            {
                Ladders[i] = new Rect();
                Ladders[i].X = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 36).ToString("X") + ", 0x0");
                Ladders[i].Y = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 36).ToString("X") + ", 0x4");
                Ladders[i].Size = new Size(PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 36).ToString("X") + ", 0x8") - Ladders[i].X,
                                           PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 36).ToString("X") + ", 0xC") - Ladders[i].Y);
            }

            // Z-Order
            MaxZOrder = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 40).ToString("X"));

            // Frame Data
            Options.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 44).ToString("X"));
            XCoefficient = PV.MemLib.ReadFloat(parentPointer + ", 0x" + (LayerOffset + 48).ToString("X"));
            YCoefficient = PV.MemLib.ReadFloat(parentPointer + ", 0x" + (LayerOffset + 52).ToString("X"));
            NumberOfBackdrops = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 56).ToString("X"));
            FirstBackdropIndex = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 60).ToString("X"));

            // Efects
            Effect = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 64).ToString("X"));
            EffectParam = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 68).ToString("X"));
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // Offset
            OffsetX = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 4).ToString("X"));
            OffsetY = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 8).ToString("X"));
            OffsetDeltaX = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 12).ToString("X"));
            OffsetDeltaY = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 16).ToString("X"));

            // Backdrops
            BackdropCount = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 20).ToString("X"));
            Backdrops = new CRunBackdrop[BackdropCount];
            //for (int i = 0; i < BackdropCount; i++)
                //Backdrops[i].RefreshData(parentPointer + ", 0x" + (LayerOffset + 24).ToString("X") + ", 0x" + (i * 4).ToString("X"));

            // Z-Order
            MaxZOrder = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 40).ToString("X"));

            // Frame Data
            XCoefficient = PV.MemLib.ReadFloat(parentPointer + ", 0x" + (LayerOffset + 48).ToString("X"));
            YCoefficient = PV.MemLib.ReadFloat(parentPointer + ", 0x" + (LayerOffset + 52).ToString("X"));
            NumberOfBackdrops = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 56).ToString("X"));
            FirstBackdropIndex = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 60).ToString("X"));

            // Efects
            Effect = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 64).ToString("X"));
            EffectParam = PV.MemLib.ReadInt(parentPointer + ", 0x" + (LayerOffset + 68).ToString("X"));
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // Name
            TreeViewItem namePanel = Templates.Editbox();
            ((namePanel.Header as Grid).Children[0] as Label).Content = "Name";
            ((namePanel.Header as Grid).Children[1] as TextBox).Text = Name;
            (namePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + LayerOffset.ToString("X") + ", 0x0";
            panel.Add(namePanel);

            // Offset
            TreeViewItem offsetPanel = Templates.Tab();
            ((offsetPanel.Header as Grid).Children[0] as Label).Content = "Offset";

            // X Offset
            TreeViewItem offsetXPanel = Templates.Editbox();
            ((offsetXPanel.Header as Grid).Children[0] as Label).Content = "X Offset";
            ((offsetXPanel.Header as Grid).Children[1] as TextBox).Text = OffsetX.ToString();
            ((offsetXPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (offsetXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 4).ToString("X");
            offsetPanel.Items.Add(offsetXPanel);

            // Y Offset
            TreeViewItem offsetYPanel = Templates.Editbox();
            ((offsetYPanel.Header as Grid).Children[0] as Label).Content = "Y Offset";
            ((offsetYPanel.Header as Grid).Children[1] as TextBox).Text = OffsetY.ToString();
            ((offsetYPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (offsetYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 8).ToString("X");
            offsetPanel.Items.Add(offsetYPanel);

            // X Delta Offset
            TreeViewItem offsetDeltaXPanel = Templates.Editbox();
            ((offsetDeltaXPanel.Header as Grid).Children[0] as Label).Content = "X Delta Offset";
            ((offsetDeltaXPanel.Header as Grid).Children[1] as TextBox).Text = OffsetDeltaX.ToString();
            ((offsetDeltaXPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (offsetDeltaXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 12).ToString("X");
            offsetPanel.Items.Add(offsetDeltaXPanel);

            // Y Offset
            TreeViewItem offsetDeltaYPanel = Templates.Editbox();
            ((offsetDeltaYPanel.Header as Grid).Children[0] as Label).Content = "Y Delta Offset";
            ((offsetDeltaYPanel.Header as Grid).Children[1] as TextBox).Text = OffsetDeltaY.ToString();
            ((offsetDeltaYPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (offsetDeltaYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 16).ToString("X");
            offsetPanel.Items.Add(offsetDeltaYPanel);

            panel.Add(offsetPanel);

            // Backdrops
            TreeViewItem backdropsPanel = Templates.Tab();
            ((backdropsPanel.Header as Grid).Children[0] as Label).Content = $"Backdrops ({BackdropCount})";
            (backdropsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 24).ToString("X");
            panel.Add(backdropsPanel);

            // Ladders
            TreeViewItem laddersPanel = Templates.Tab();
            ((laddersPanel.Header as Grid).Children[0] as Label).Content = $"Ladders ({LadderCount})";
            (laddersPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 36).ToString("X");

            int LadderID = 0;
            foreach (var ladder in Ladders)
            {
                TreeViewItem ladderPanel = Templates.Tab();
                ((ladderPanel.Header as Grid).Children[0] as Label).Content = "Ladder " + LadderID++;

                // Ladder Left
                TreeViewItem ladderLeftPanel = Templates.Editbox(false);
                ((ladderLeftPanel.Header as Grid).Children[0] as Label).Content = "Left";
                ((ladderLeftPanel.Header as Grid).Children[1] as TextBox).Text = ladder.Left.ToString();
                ladderPanel.Items.Add(ladderLeftPanel);

                // Ladder Top
                TreeViewItem ladderTopPanel = Templates.Editbox(false);
                ((ladderTopPanel.Header as Grid).Children[0] as Label).Content = "Top";
                ((ladderTopPanel.Header as Grid).Children[1] as TextBox).Text = ladder.Top.ToString();
                ladderPanel.Items.Add(ladderTopPanel);

                // Ladder Right
                TreeViewItem ladderRightPanel = Templates.Editbox(false);
                ((ladderRightPanel.Header as Grid).Children[0] as Label).Content = "Right";
                ((ladderRightPanel.Header as Grid).Children[1] as TextBox).Text = ladder.Right.ToString();
                ladderPanel.Items.Add(ladderRightPanel);

                // Ladder Bottom
                TreeViewItem ladderBottomPanel = Templates.Editbox(false);
                ((ladderBottomPanel.Header as Grid).Children[0] as Label).Content = "Bottom";
                ((ladderBottomPanel.Header as Grid).Children[1] as TextBox).Text = ladder.Bottom.ToString();
                ladderPanel.Items.Add(ladderBottomPanel);

                laddersPanel.Items.Add(ladderPanel);
            }

            panel.Add(laddersPanel);

            // Frame Data
            TreeViewItem frmDataPanel = Templates.Tab();
            ((frmDataPanel.Header as Grid).Children[0] as Label).Content = "Frame Data";

            // Max Z-Order
            TreeViewItem maxZOrderPanel = Templates.Editbox();
            ((maxZOrderPanel.Header as Grid).Children[0] as Label).Content = "Max Z-Order";
            ((maxZOrderPanel.Header as Grid).Children[1] as TextBox).Text = MaxZOrder.ToString();
            ((maxZOrderPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (maxZOrderPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 40).ToString("X");
            frmDataPanel.Items.Add(maxZOrderPanel);

            // Options
            TreeViewItem optionsPanel = Templates.Tab();
            ((optionsPanel.Header as Grid).Children[0] as Label).Content = "Options";
            (optionsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 44).ToString("X");

            foreach (string key in Options.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem optionPanel = Templates.Checkbox(false);
                ((optionPanel.Header as Grid).Children[0] as Label).Content = key;
                ((optionPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Options[key];
                (optionPanel.Tag as TagHeader).ParentFlags = Options;
                (optionPanel.Tag as TagHeader).Flag = key;
                (optionPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 44).ToString("X");
                optionsPanel.Items.Add(optionPanel);
            }

            frmDataPanel.Items.Add(optionsPanel);

            // X Coefficient
            TreeViewItem coeffXPanel = Templates.Editbox();
            ((coeffXPanel.Header as Grid).Children[0] as Label).Content = "X Coefficient";
            ((coeffXPanel.Header as Grid).Children[1] as TextBox).Text = XCoefficient.ToString();
            (coeffXPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 48).ToString("X");
            (coeffXPanel.Tag as TagHeader).ActionType = 3;
            frmDataPanel.Items.Add(coeffXPanel);

            // Y Coefficient
            TreeViewItem coeffYPanel = Templates.Editbox();
            ((coeffYPanel.Header as Grid).Children[0] as Label).Content = "Y Coefficient";
            ((coeffYPanel.Header as Grid).Children[1] as TextBox).Text = YCoefficient.ToString();
            (coeffYPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 52).ToString("X");
            (coeffYPanel.Tag as TagHeader).ActionType = 3;
            frmDataPanel.Items.Add(coeffYPanel);

            // Backdrop Count
            TreeViewItem backdropCountPanel = Templates.Editbox();
            ((backdropCountPanel.Header as Grid).Children[0] as Label).Content = "Backdrop Count";
            ((backdropCountPanel.Header as Grid).Children[1] as TextBox).Text = BackdropCount.ToString();
            ((backdropCountPanel.Header as Grid).Children[1] as TextBox).IsReadOnly = true;
            (backdropCountPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 56).ToString("X");
            frmDataPanel.Items.Add(backdropCountPanel);

            // Effect
            TreeViewItem effectPanel = Templates.Editbox();
            ((effectPanel.Header as Grid).Children[0] as Label).Content = "Effect";
            ((effectPanel.Header as Grid).Children[1] as TextBox).Text = Effect.ToString();
            (effectPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x" + (LayerOffset + 64).ToString("X");
            frmDataPanel.Items.Add(effectPanel);

            panel.Add(frmDataPanel);
            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
