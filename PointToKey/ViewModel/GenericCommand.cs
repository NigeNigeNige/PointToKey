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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PointToKey.ViewModel
{
    internal class GenericCommand : ICommand
    {
        public delegate void executeDelegate();
        public delegate bool canExecuteDelegate();

        private executeDelegate executeAction;
        private canExecuteDelegate canExecuteAction;

        public GenericCommand(executeDelegate executeAction, canExecuteDelegate canExecuteAction = null)
        {
            this.executeAction = executeAction;
            this.canExecuteAction = canExecuteAction;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteAction != null)
            {
                return (bool)canExecuteAction.DynamicInvoke();
            }
            return true;
        }
        
        public void Execute(object parameter)
        {
            if (executeAction != null)
            {
                executeAction.DynamicInvoke();
            }
        }


        public event EventHandler CanExecuteChanged;
    }
}
