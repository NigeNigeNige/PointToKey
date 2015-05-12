using PointToKey.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

namespace PointToKey.Model
{
    [Serializable]
    public class ClientSettings
    {
        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public Color CellBackgroundColor { get; set; }

        public Color HighlightColor { get; set; }

        public Color GridLineColor { get; set; }

        public int GridLineWidth { get; set; }

        public Color TextColor { get; set; }

        public int TextSize { get; set; }

        public Color MarginColor { get; set; }

        public int GridCellMarginX { get; set; }

        public int GridCellMarginY { get; set; }

        public bool RequireClicks { get; set; }

        public SerializableDictionary<Point, CellSettings> CellSettings { get; set; }

    }
}
