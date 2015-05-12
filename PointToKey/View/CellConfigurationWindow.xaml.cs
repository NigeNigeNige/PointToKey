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

using PointToKey.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PointToKey.View
{
    /// <summary>
    /// Interaction logic for CellConfigurationWindow.xaml
    /// </summary>
    public partial class CellConfigurationWindow : Window
    {
        private CellConfigurationViewModel vm;

        public CellConfigurationWindow(CellConfigurationViewModel vm)
        {
            InitializeComponent();

            this.vm = vm;
            DataContext = vm;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SetKey_Click(object sender, RoutedEventArgs e)
        {
            vm.WaitingForKey = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (vm.WaitingForKey)
            {
                e.Handled = true;
                vm.ReceivedKey(e.Key);
            }
        }
    }
}
