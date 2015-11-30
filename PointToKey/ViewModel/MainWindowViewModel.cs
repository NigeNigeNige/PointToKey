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

using PointToKey.Interface;
using PointToKey.Model;
using PointToKey.Util;
using PointToKey.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace PointToKey.ViewModel
{
    //TODO: Dispose of joystick object when closing app

    internal class MainWindowViewModel : BaseViewModel
    {
        private IKeyboardInputSender KeyboardInputSender = new InputSimulatorInputSender();
        private IJoystickInputSender JoystickInputSender = new VJoyInputSender();

        private int columnCount = 3;

        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                if (value < 1) value = 1;
                if (SetValue(ref columnCount, value, "ColumnCount"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private int rowCount = 3;

        public int RowCount
        {
            get { return rowCount; }
            set
            {
                if (value < 1) value = 1;
                if (SetValue(ref rowCount, value, "RowCount"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private Color cellBackgroundColor = Colors.Black;
        public Color CellBackgroundColor
        {
            get { return cellBackgroundColor; }
            set 
            {
                if (SetValue(ref cellBackgroundColor, value, "CellBackgroundColor"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private Color highlightColor = Colors.Green;
        public Color HighlightColor
        {
            get { return highlightColor; }
            set
            {
                if (SetValue(ref highlightColor, value, "HighlightColor"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private Color gridLineColor = Colors.Teal;
        public Color GridLineColor
        {
            get { return gridLineColor; }
            set
            {
                if (SetValue(ref gridLineColor, value, "GridLineColor"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private int gridLineWidth = 1;
        public int GridLineWidth
        {
            get { return gridLineWidth; }
            set
            {
                if (SetValue(ref gridLineWidth, value, "GridLineWidth"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private Color textColor = Colors.White;
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                if (SetValue(ref textColor, value, "TextColor"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private int textSize = 14;
        public int TextSize
        {
            get { return textSize; }
            set
            {
                if (SetValue(ref textSize, value, "TextSize"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private Color marginColor = Colors.Black;
        public Color MarginColor
        {
            get { return marginColor; }
            set
            {
                SetValue(ref marginColor, value, "MarginColor");
            }
        }

        private int gridCellMarginX = 1;
        public int GridCellMarginX
        {
            get { return gridCellMarginX; }
            set
            {
                if (SetValue(ref gridCellMarginX, value, "GridCellMarginX"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private int gridCellMarginY = 1;
        public int GridCellMarginY
        {
            get { return gridCellMarginY; }
            set
            {
                if (SetValue(ref gridCellMarginY, value, "GridCellMarginY"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private bool requireClicks = false;
        public bool RequireClicks
        {
            get { return requireClicks; }
            set { SetValue(ref requireClicks, value, "RequireClicks"); }
        }

        private bool editMode = true;
        public bool EditMode
        {
            get { return editMode; }
            set { SetValue(ref editMode, value, "EditMode"); }
        }

        private bool testMode = false;
        public bool TestMode
        {
            get { return testMode; }
            set { SetValue(ref testMode, value, "TestMode"); }
        }

        private bool joystickAvailable = false;
        public bool JoystickAvailable
        {
            get { return joystickAvailable; }
            set { SetValue(ref joystickAvailable, value, "JoystickAvailable"); }
        }

        public SerializableDictionary<Point, CellSettings> CellSettings = new SerializableDictionary<Point, CellSettings>();

        public IEnumerable<CellSettings> Cells
        {
            get
            {
                foreach (var cell in CellSettings)
                {
                    yield return cell.Value;
                }
            }
        }

        #region Commands
        public ICommand GenerateGridCommand { get; set; }
        public ICommand LoadSettingsCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            JoystickAvailable = JoystickInputSender.Initialise();

            OnGenerateGrid();
        }

        private void OnGenerateGrid()
        {
            if (!CellSettings.Any())
            {
                GenerateSampleGrid();
            }

            if (GenerateGridCommand != null)
            {
                GenerateGridCommand.Execute(null);
            }
        }
        
        private void GenerateSampleGrid()
        {
            for (int col = 0; col < columnCount; ++col)
            {
                for (int row = 0; row < rowCount; ++row)
                {
                    GenerateGridCell(col, row);
                }
            }
        }

        private void GenerateGridCell(int col, int row)
        {
            CellSettings[new Point(col, row)] = new CellSettings()
            {
                CellAction = new CellAction()
                {
                    ActionType = CellActionType.KeyDown,
                    KeyCode = Key.A,
                    StringEntryString = "blah"
                },
                DisplayText = string.Format("col:{0},row:{1}", col, row)
            };
        }

        public void ActivateCell(Border cell)
        {
            Debug.Assert(cell.Tag != null);

            if (TestMode || EditMode == false)
            {
                var cellPosition = (Point)cell.Tag;
                if (CellSettings.ContainsKey(cellPosition))
                {
                    var action = CellSettings[cellPosition].CellAction;
                    switch (action.ActionType)
                    {
                        case CellActionType.None:
                            break;
                        case CellActionType.KeyDown:
                            KeyboardInputSender.SendKeyDown(action.KeyCode);
                            break;
                        case CellActionType.KeyPress:
                            KeyboardInputSender.SendKeyPress(action.KeyCode);
                            break;
                        case CellActionType.StringEntry:
                            KeyboardInputSender.SendTextEntry(action.StringEntryString);
                            break;
                        case CellActionType.VJoyButtonsDown:
                            JoystickInputSender.SendButtonsDown(action.VJoyButtons);
                            break;
                        case CellActionType.VJoyButtonsPress:
                            JoystickInputSender.SendButtonsPress(action.VJoyButtons);
                            break;
                        default:
                            throw new Exception("Unknown CellActionType");
                    }
                }
            }
        }

        public void DeactivateCell(Border cell)
        {
            Debug.Assert(cell.Tag != null);

            if (TestMode || EditMode == false)
            {
                var cellPosition = (Point)cell.Tag;
                if (CellSettings.ContainsKey(cellPosition))
                {
                    var action = CellSettings[cellPosition].CellAction;
                    switch (action.ActionType)
                    {
                        case CellActionType.KeyDown:
                            KeyboardInputSender.SendKeyUp(action.KeyCode);
                            break;
                        case CellActionType.VJoyButtonsDown:
                            JoystickInputSender.SendButtonsUp(action.VJoyButtons);
                            break;
                    }
                }
            }
        }

        public void EditCell(Border cell)
        {
            Debug.Assert(cell.Tag != null);

            var cellPosition = (Point)cell.Tag;
            if (CellSettings.ContainsKey(cellPosition))
            {
                var cellSettings = CellSettings[cellPosition];
                var editWindow = new CellConfigurationViewModel(cellSettings);
                if (editWindow.ShowDialog())
                {
                    OnGenerateGrid();
                }
            }
        }

        public void SaveSettings(string filename)
        {
            var settings = new ClientSettings()
            {
                ColumnCount = this.ColumnCount,
                RowCount = this.RowCount,
                CellBackgroundColor = this.CellBackgroundColor,
                HighlightColor = this.HighlightColor,
                GridLineColor = this.GridLineColor,
                GridLineWidth = this.GridLineWidth,
                TextColor = this.TextColor,
                TextSize = this.TextSize,
                MarginColor = this.MarginColor,
                GridCellMarginX = this.GridCellMarginX,
                GridCellMarginY = this.GridCellMarginY,
                RequireClicks = this.RequireClicks,
                CellSettings = this.CellSettings,
            };

            using (var writer = new StreamWriter(filename))
            {
                var serializer = new XmlSerializer(typeof(ClientSettings));
                serializer.Serialize(writer, settings);

                writer.Close();
            }
        }

        public void LoadSettings(string filename)
        {
            ClientSettings clientSettings;

            using (var reader = new StreamReader(filename))
            {
                var serializer = new XmlSerializer(typeof(ClientSettings));
                clientSettings = (ClientSettings)serializer.Deserialize(reader);

                reader.Close();
            }

            ColumnCount = clientSettings.ColumnCount;
            RowCount = clientSettings.RowCount;
            CellBackgroundColor = clientSettings.CellBackgroundColor;
            HighlightColor = clientSettings.HighlightColor;
            GridLineColor = clientSettings.GridLineColor;
            GridLineWidth = clientSettings.GridLineWidth;
            TextColor = clientSettings.TextColor;
            TextSize = clientSettings.TextSize;
            MarginColor = clientSettings.MarginColor;
            GridCellMarginX = clientSettings.GridCellMarginX;
            GridCellMarginY = clientSettings.GridCellMarginY;
            RequireClicks = clientSettings.RequireClicks;
            CellSettings = clientSettings.CellSettings;
        }
    }
}
