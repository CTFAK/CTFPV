using CTFPV;
using CTFPV.InformationItems;
using CTFPV.Miscellaneous;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Windows.Controls;
using System.Windows.Input;

namespace Encryption_Key_Finder.InformationItems
{
    public class CRunMVHeader : PropertyPanel
    {
        // MV Header
        public short AppMode;
        public short ScreenMode;
        public BitDict Preferences = new BitDict(new string[]
        {
            "", "", "", "", "", "", "", "", "", "", "", "", "", "",
            "Music Enabled",
            "Samples Enabled"
        });
        public bool Fullscreen;
        public int LanguageID;

        public override void InitData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // MV Header
            AppMode = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x1C");
            ScreenMode = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x1E");
            Preferences.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x40");
            Fullscreen = PV.MemLib.ReadInt(parentPointer + ", 0x48") == 1;
            LanguageID = PV.MemLib.ReadInt(parentPointer + ", 0xFC");
        }

        public override void RefreshData(string parentPointer)
        {
            latestParentPointer = parentPointer;

            // MV Header
            Preferences.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x40");
            Fullscreen = PV.MemLib.ReadInt(parentPointer + ", 0x48") == 1;
        }

        public override List<TreeViewItem> GetPanel()
        {
            List<TreeViewItem> panel = new List<TreeViewItem>();

            // App Mode
            TreeViewItem appModePanel = Templates.Editbox();
            ((appModePanel.Header as Grid).Children[0] as Label).Content = "App Mode";
            ((appModePanel.Header as Grid).Children[1] as TextBox).Text = AppMode.ToString();
            (appModePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1C";
            (appModePanel.Tag as TagHeader).ActionType = 1;
            panel.Add(appModePanel);

            // Screen Mode
            TreeViewItem scrModePanel = Templates.Editbox();
            ((scrModePanel.Header as Grid).Children[0] as Label).Content = "Screen Mode";
            ((scrModePanel.Header as Grid).Children[1] as TextBox).Text = ScreenMode.ToString();
            (scrModePanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x1E";
            (scrModePanel.Tag as TagHeader).ActionType = 1;
            panel.Add(scrModePanel);

            // Preferences
            TreeViewItem prefsPanel = Templates.Tab(true);
            ((prefsPanel.Header as Grid).Children[0] as Label).Content = "Preferences";
            (prefsPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x40";

            foreach (string key in Preferences.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                TreeViewItem gameFlagPanel = Templates.Checkbox(false);
                ((gameFlagPanel.Header as Grid).Children[0] as Label).Content = key;
                ((gameFlagPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Preferences[key];
                (gameFlagPanel.Tag as TagHeader).ParentFlags = Preferences;
                (gameFlagPanel.Tag as TagHeader).Flag = key;
                (gameFlagPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x40";
                prefsPanel.Items.Add(gameFlagPanel);
            }

            panel.Add(prefsPanel);

            // Fullscreen
            TreeViewItem fullScrPanel = Templates.Checkbox();
            ((fullScrPanel.Header as Grid).Children[0] as Label).Content = "Fullscreen";
            ((fullScrPanel.Header as Grid).Children[1] as CheckBox).IsChecked = Fullscreen;
            (fullScrPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0x48";
            panel.Add(fullScrPanel);

            // Language ID
            TreeViewItem langIDPanel = Templates.Editbox();
            ((langIDPanel.Header as Grid).Children[0] as Label).Content = "Language ID";
            ((langIDPanel.Header as Grid).Children[1] as TextBox).Text = LanguageID.ToString();
            (langIDPanel.Tag as TagHeader).Pointer = latestParentPointer + ", 0xFC";
            panel.Add(langIDPanel);

            return panel;
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
