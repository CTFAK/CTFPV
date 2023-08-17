using ColorPicker;
using ColorPicker.Models;
using CTFPV.InformationItems;
using Encryption_Key_Finder.InformationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace CTFPV.Miscellaneous
{
    public class TagHeader
    {
        public PropertyPanel Parent;
        public int Handle;
        public string Pointer;

        public int ActionType = 2;
        public string OffValue = "0";
        public string OnValue = "1";
        public BitDict ParentFlags;
        public string Flag = string.Empty;
        public bool FlipCounter = false;

        public ContextMenu GetMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem copyPointer = new MenuItem();
            copyPointer.Header = "Copy Pointer";
            copyPointer.Click += CopyPointer_Click;
            menu.Items.Add(copyPointer);
            return menu;
        }

        private void CopyPointer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Pointer) && Pointer.Contains("base"))
                Clipboard.SetText(Pointer.Replace("base", $"\"{PV.MemLib.mProc.MainModule.ModuleName}\""));
        }

        public void CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            bool value = (sender as CheckBox).IsChecked == true;

            switch (ActionType)
            {
                case 0: // Byte
                    if (ParentFlags == null)
                        PV.MemLib.WriteMemory(Pointer, "byte", value ? OnValue : OffValue);
                    else
                    {
                        ParentFlags[Flag] = value;
                        PV.MemLib.WriteMemory(Pointer, "byte", ((byte)ParentFlags.flag).ToString());
                    }
                    break;
                case 1: // Short
                    if (ParentFlags == null)
                        PV.MemLib.WriteMemory(Pointer, "2bytes", value ? OnValue : OffValue);
                    else
                    {
                        ParentFlags[Flag] = value;
                        PV.MemLib.WriteBytes(Pointer, BitConverter.GetBytes((ushort)ParentFlags.flag));
                    }
                    break;
                case 2: // Int
                    if (ParentFlags == null)
                        PV.MemLib.WriteMemory(Pointer, "int", value ? OnValue : OffValue);
                    else
                    {
                        ParentFlags[Flag] = value;
                        PV.MemLib.WriteBytes(Pointer, BitConverter.GetBytes(ParentFlags.flag));
                    }
                    break;
                case 3: // Float
                    if (ParentFlags == null)
                        PV.MemLib.WriteMemory(Pointer, "float", value ? OnValue : OffValue);
                    break;
                case 4: // Double
                    if (ParentFlags == null)
                        PV.MemLib.WriteMemory(Pointer, "double", value ? OnValue : OffValue);
                    break;
            }
            CRunFrame.UpdateObjects = true;
        }

        public void EditBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Pointer))
                return;

            switch (ActionType)
            {
                case 0: // Byte
                    if (byte.TryParse((sender as TextBox).Text.Trim(), out var bvalue))
                        PV.MemLib.WriteMemory(Pointer, "byte", bvalue.ToString());
                    break;
                case 1: // Short
                    if (short.TryParse((sender as TextBox).Text.Trim(), out var svalue))
                        PV.MemLib.WriteMemory(Pointer, "2bytes", svalue.ToString());
                    break;
                case 2: // Int
                    if (int.TryParse((sender as TextBox).Text.Trim(), out var ivalue))
                        if (FlipCounter)
                            PV.MemLib.WriteMemory(Pointer, "int", ((ivalue * -1) - 1).ToString());
                        else
                            PV.MemLib.WriteMemory(Pointer, "int", ivalue.ToString());
                    break;
                case 3: // Float
                    if (float.TryParse((sender as TextBox).Text.Trim(), out var fvalue))
                        PV.MemLib.WriteMemory(Pointer, "float", fvalue.ToString());
                    break;
                case 4: // Double
                    if (double.TryParse((sender as TextBox).Text.Trim(), out var dvalue))
                        PV.MemLib.WriteMemory(Pointer, "double", dvalue.ToString());
                    break;
                case 5: // Unicode String
                    PV.MemLib.WriteMemory(Pointer, "string", (sender as TextBox).Text + (char)0, stringEncoding: Encoding.Unicode);
                    break;
                case 6: // Ascii String
                    PV.MemLib.WriteMemory(Pointer, "string", (sender as TextBox).Text + (char)0, stringEncoding: Encoding.ASCII);
                    break;
            }
            CRunFrame.UpdateObjects = true;
        }

        public void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Pointer))
                return;

            NotifyableColor color = (sender as PortableColorPicker).Color;
            switch (ActionType)
            {
                case 0: // BGRA
                    int bgra = BitConverter.ToInt32(
                        new byte[4]
                        {
                            (byte)color.RGB_B,
                            (byte)color.RGB_G,
                            (byte)color.RGB_R,
                            (byte)color.A
                        });
                    PV.MemLib.WriteMemory(Pointer, "int", bgra.ToString());
                    break;
                case 1: // ABGR
                    int abgr = BitConverter.ToInt32(
                        new byte[4]
                        {
                            (byte)color.A,
                            (byte)color.RGB_B,
                            (byte)color.RGB_G,
                            (byte)color.RGB_R
                        });
                    PV.MemLib.WriteMemory(Pointer, "int", abgr.ToString());
                    break;
                case 2: // RGBA
                    int rgba = BitConverter.ToInt32(
                        new byte[4]
                        {
                            (byte)color.RGB_R,
                            (byte)color.RGB_G,
                            (byte)color.RGB_B,
                            (byte)color.A
                        });
                    PV.MemLib.WriteMemory(Pointer, "int", rgba.ToString());
                    break;
                case 3: // ARGB
                    int argb = BitConverter.ToInt32(
                        new byte[4]
                        {
                            (byte)color.A,
                            (byte)color.RGB_R,
                            (byte)color.RGB_G,
                            (byte)color.RGB_B
                        });
                    PV.MemLib.WriteMemory(Pointer, "int", argb.ToString());
                    break;
            }
            CRunFrame.UpdateObjects = true;
        }
    }
}
