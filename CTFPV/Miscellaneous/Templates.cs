using ColorPicker;
using ColorPicker.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CTFPV.Miscellaneous
{
    public static class Templates
    {
        public static TreeViewItem Checkbox(bool contextMenu = true) => Generic(0, contextMenu);
        public static TreeViewItem Editbox(bool contextMenu = true) => Generic(1, contextMenu);
        public static TreeViewItem ColorPicker(bool contextMenu = true) => Generic(2, contextMenu);
        private static TreeViewItem Generic(int type, bool contextMenu)
        {
            TreeViewItem item = new TreeViewItem();
            Grid grid = new Grid();
            Label label = new Label();

            grid.Width = 250;

            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Padding = new Thickness(0, 0, 0, 1);
            label.FontSize = 10;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE7FF"));
            label.Content = "Test Test TEst C#";
            grid.Children.Add(label);
            TagHeader tag = new TagHeader();

            switch (type)
            {
                case 0:
                    CheckBox checkBox = new CheckBox();
                    checkBox.HorizontalAlignment = HorizontalAlignment.Right;
                    checkBox.Style = Application.Current.Resources["CheckBoxStyle1"] as Style;
                    checkBox.Click += tag.CheckBox_Clicked;
                    grid.Children.Add(checkBox);
                    break;
                case 1:
                    TextBox editBox = new TextBox();
                    editBox.HorizontalAlignment = HorizontalAlignment.Right;
                    editBox.HorizontalContentAlignment = HorizontalAlignment.Right;
                    editBox.Width = 140;
                    editBox.Style = Application.Current.Resources["TextBoxStyle1"] as Style;
                    editBox.TextChanged += tag.EditBox_TextChanged;
                    grid.Children.Add(editBox);
                    break;
                case 2:
                    PortableColorPicker colorPicker = new PortableColorPicker();
                    colorPicker.HorizontalAlignment = HorizontalAlignment.Right;
                    colorPicker.Width = 15;
                    colorPicker.Height = 15;
                    colorPicker.ColorState = new ColorState();
                    colorPicker.ColorChanged += tag.ColorPicker_ColorChanged;
                    grid.Children.Add(colorPicker);
                    break;
            }

            item.ContextMenu = tag.GetMenu();
            item.Tag = tag;
            item.Header = grid;
            return item;
        }

        public static TreeViewItem Tab(bool contextMenu = false)
        {
            TreeViewItem item = new TreeViewItem();
            Grid grid = new Grid();
            Label label = new Label();

            grid.Width = 250;

            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Padding = new Thickness(0, 0, 0, 1);
            label.FontSize = 10;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDE7FF"));
            label.Content = "Test Test TEst C#";
            grid.Children.Add(label);
            TagHeader tag = new TagHeader();

            if (contextMenu)
                item.ContextMenu = tag.GetMenu();
            item.Tag = tag;
            item.Header = grid;
            return item;
        }
    }
}
