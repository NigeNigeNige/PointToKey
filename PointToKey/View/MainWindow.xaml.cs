/*  PointToKey
    Copyright (C) 2015 Nigel Jones

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using PointToKey.Util;
using PointToKey.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;

namespace PointToKey
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        private MainWindowViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            
            vm = new MainWindowViewModel();
            vm.GenerateGridCommand = new GenericCommand(GenerateGrid);
            vm.LoadSettingsCommand = new GenericCommand(LoadSettings);
            vm.SaveSettingsCommand = new GenericCommand(SaveSettings);

            DataContext = vm;
            GenerateGrid();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        public void GenerateGrid()
        {
            //RemoveGridCells();
            
            //MainGrid.ColumnDefinitions.Clear();
            //MainGrid.RowDefinitions.Clear();
            //for (int col = 0; col < vm.ColumnCount; ++col)
            //{
            //    MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(vm.ColumnCount, GridUnitType.Star) });
            //}

            //var backgroundBrush = new SolidColorBrush(vm.CellBackgroundColor);
            //var borderBrush = new SolidColorBrush(vm.GridLineColor);
            //var textBrush = new SolidColorBrush(vm.TextColor);
            //var borderThickness = new Thickness(vm.GridLineWidth);
            //var innerMargin = new Thickness(vm.GridCellMarginX, vm.GridCellMarginY, vm.GridCellMarginX, vm.GridCellMarginY);

            //var middleRow = vm.RowCount / 2;
            //var middleCol = vm.ColumnCount / 2;

            //for (int row = 0; row < vm.RowCount; ++row)
            //{
            //    MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(vm.RowCount, GridUnitType.Star) });

            //    for (int col = 0; col < vm.ColumnCount; ++col)
            //    {
            //        var margin = innerMargin;
            //        if (col == 0)
            //        {
            //            margin.Left = 0;
            //        }
            //        else if (col == vm.ColumnCount - 1)
            //        {
            //            margin.Right = 0;
            //        }

            //        if (row == 0)
            //        {
            //            margin.Top = 0;
            //        }
            //        else if (row == vm.RowCount - 1)
            //        {
            //            margin.Bottom = 0;
            //        }

            //        var border = new Border()
            //        {
            //            Background = backgroundBrush,
            //            BorderBrush = borderBrush,
            //            BorderThickness = borderThickness,
            //            Margin = margin,
            //        };

            //        border.SetValue(Grid.RowProperty, row);
            //        border.SetValue(Grid.ColumnProperty, col);

            //        var cellPosition = new Point(col, row);
            //        border.Tag = cellPosition; 

            //        border.MouseEnter += border_MouseEnter;
            //        border.MouseLeave += border_MouseLeave;
            //        border.MouseLeftButtonDown += border_MouseLeftButtonDown;
            //        border.MouseLeftButtonUp += border_MouseLeftButtonUp;

            //        string displayText1 = string.Empty;
            //        string displayText2 = string.Empty;
            //        if (vm.CellSettings.ContainsKey(cellPosition))
            //        {
            //            var cellSetting = vm.CellSettings[cellPosition];
            //            displayText1 = cellSetting.DisplayText;
            //            displayText2 = cellSetting.CellAction.ToString();
            //        }

            //        border.Child = new StackPanel()
            //        {
            //            Orientation = Orientation.Vertical,
            //            Children = 
            //            {
            //                new TextBlock()
            //                {
            //                    Text = displayText1,
            //                    HorizontalAlignment = HorizontalAlignment.Center,
            //                    FontSize = vm.TextSize,
            //                    Foreground = textBrush
            //                },
            //                new TextBlock()
            //                {
            //                    Text = displayText2,
            //                    HorizontalAlignment = HorizontalAlignment.Center,
            //                    FontSize = vm.TextSize,
            //                    Foreground = textBrush
            //                },
            //            }
            //        };                

            //        MainGrid.Children.Add(border);
            //    }
            //}
        }

        

        void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (vm.RequireClicks)
            {
                // Activate the key
                vm.ActivateCell((Border)sender);
            }
        }

        void border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (vm.TestMode || vm.EditMode == false)
            {
                if (vm.RequireClicks)
                {
                    // Activate the key
                    vm.DeactivateCell((Border)sender);
                }
            }
            else
            {
                vm.EditCell((Border)sender);
            }
        }

        void border_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(vm.CellBackgroundColor);
            if (vm.RequireClicks == false)
            {
                vm.DeactivateCell((Border)sender);
            }
        }

        void border_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(vm.HighlightColor);
            if (vm.RequireClicks == false)
            {
                // Activate the key
                vm.ActivateCell((Border)sender);
            }
        }

        private void RemoveGridCells()
        {
            //foreach (var item in MainGrid.Children.OfType<Border>())
            //{
            //    item.MouseLeave -= border_MouseLeave;
            //    item.MouseEnter -= border_MouseEnter;
            //    item.MouseLeftButtonDown -= border_MouseLeftButtonDown;
            //    item.MouseLeftButtonUp -= border_MouseLeftButtonUp;
            //}
            //MainGrid.Children.Clear();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    e.Handled = true;
                    this.Close();
                    break;
            }
        }

        public void SaveSettings()
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = "Settings files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                OverwritePrompt = true,
                Title = "Save Settings",
                ValidateNames = true
            };
            var result = dialog.ShowDialog(this);
            if (result.HasValue && result.Value)
            {
                vm.SaveSettings(dialog.FileName);
            }
        }

        public void LoadSettings()
        {
            var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = "xml",
                Filter = "Settings files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 0,
                Title = "Load Settings",
                ValidateNames = true
            };
            var result = dialog.ShowDialog(this);
            if (result.HasValue && result.Value)
            {
                vm.LoadSettings(dialog.FileName);
            }
        }

        #region IDisposable
        public void Dispose()
        {
            RemoveGridCells();
        }
        #endregion


    }
}
