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

    /// <summary>
    /// The main application window's viewmodel.
    /// </summary>
    internal class MainWindowViewModel : BaseViewModel
    {
        private IKeyboardInputSender KeyboardInputSender = new InputSimulatorInputSender();
        private IJoystickInputSender JoystickInputSender = new VJoyInputSender();

        private int columnCount = 3;

        /// <summary>
        /// Gets or sets the column count.
        /// </summary>
        /// <value>
        /// The column count.
        /// </value>
        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                // Values less than 1 are not valid
                if (value < 1) value = 1;

                if (SetValue(ref columnCount, value, "ColumnCount"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private int rowCount = 3;

        /// <summary>
        /// Gets or sets the row count.
        /// </summary>
        /// <value>
        /// The row count.
        /// </value>
        public int RowCount
        {
            get { return rowCount; }
            set
            {
                // Values less than 1 are not valid
                if (value < 1) value = 1;

                if (SetValue(ref rowCount, value, "RowCount"))
                {
                    OnGenerateGrid();
                }
            }
        }

        private Color cellBackgroundColor = Colors.Black;
        /// <summary>
        /// Gets or sets the colour of the cell background.
        /// </summary>
        /// <value>
        /// The colour of the cell background.
        /// </value>
        public Color CellBackgroundColor
        {
            get { return cellBackgroundColor; }
            set 
            {
                SetValue(ref cellBackgroundColor, value, "CellBackgroundColor");
            }
        }

        private Color highlightColor = Colors.Green;
        /// <summary>
        /// Gets or sets the colour of the highlight (when the mouse is over the cell).
        /// </summary>
        /// <value>
        /// The colour of the highlight.
        /// </value>
        public Color HighlightColor
        {
            get { return highlightColor; }
            set
            {
                SetValue(ref highlightColor, value, "HighlightColor");
            }
        }

        private Color gridLineColor = Colors.Teal;
        /// <summary>
        /// Gets or sets the colour of the grid lines (border).
        /// </summary>
        /// <value>
        /// The colour of the grid lines.
        /// </value>
        public Color GridLineColor
        {
            get { return gridLineColor; }
            set
            {
                SetValue(ref gridLineColor, value, "GridLineColor");
            }
        }

        private int gridLineWidth = 1;
        /// <summary>
        /// Gets or sets the width of the grid lines (border).
        /// </summary>
        /// <value>
        /// The width of the grid lines.
        /// </value>
        public int GridLineWidth
        {
            get { return gridLineWidth; }
            set
            {
                SetValue(ref gridLineWidth, value, "GridLineWidth");
            }
        }

        private Color textColor = Colors.White;
        /// <summary>
        /// Gets or sets the colour of the text inside the cells.
        /// </summary>
        /// <value>
        /// The colour of the text.
        /// </value>
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                SetValue(ref textColor, value, "TextColor");
            }
        }

        private int textSize = 14;
        /// <summary>
        /// Gets or sets the size of the text inside the cells.
        /// </summary>
        /// <value>
        /// The size of the text.
        /// </value>
        public int TextSize
        {
            get { return textSize; }
            set
            {
                SetValue(ref textSize, value, "TextSize");
            }
        }

        private Color marginColor = Colors.Black;
        /// <summary>
        /// Gets or sets the colour of the margins (space between the cells).
        /// </summary>
        /// <value>
        /// The colour of the margins.
        /// </value>
        public Color MarginColor
        {
            get { return marginColor; }
            set
            {
                SetValue(ref marginColor, value, "MarginColor");
            }
        }

        private int gridCellMarginX = 1;
        /// <summary>
        /// Gets or sets the horizontal component of the grid cell margin distance in pixels.
        /// </summary>
        /// <value>
        /// The grid cell margin X value in pixels.
        /// </value>
        public int GridCellMarginX
        {
            get { return gridCellMarginX; }
            set
            {
                SetValue(ref gridCellMarginX, value, "GridCellMarginX");
            }
        }

        private int gridCellMarginY = 1;
        /// <summary>
        /// Gets or sets the vertical component of the grid cell margin distance in pixels.
        /// </summary>
        /// <value>
        /// The grid cell margin Y in pixels.
        /// </value>
        public int GridCellMarginY
        {
            get { return gridCellMarginY; }
            set
            {
                SetValue(ref gridCellMarginY, value, "GridCellMarginY");
            }
        }

        private bool requireClicks = false;
        /// <summary>
        /// Gets or sets a value indicating whether the mouse button must be clicked for a cell to perform its associated action.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cells must be clicked; otherwise, <c>false</c>.
        /// </value>
        public bool RequireClicks
        {
            get { return requireClicks; }
            set { SetValue(ref requireClicks, value, "RequireClicks"); }
        }

        private bool editMode = true;
        /// <summary>
        /// Gets or sets a value indicating whether the form is in edit mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the form is in edit mode; otherwise, <c>false</c>.
        /// </value>
        public bool EditMode
        {
            get { return editMode; }
            set { SetValue(ref editMode, value, "EditMode"); }
        }

        private bool testMode = false;
        /// <summary>
        /// Gets or sets a value indicating whether test mode is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if test mode is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool TestMode
        {
            get { return testMode; }
            set { SetValue(ref testMode, value, "TestMode"); }
        }

        private bool fullscreenMode = false;
        /// <summary>
        /// Gets or sets a value indicating whether the form should take up all available screen space.
        /// </summary>
        /// <value>
        ///   <c>true</c> if fullscreen mode is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool FullscreenMode
        {
            get { return fullscreenMode; }
            set
            {
                if (SetValue(ref fullscreenMode, value, "FullscreenMode"))
                {
                    // The fullscreen mode changed
                    if (fullscreenMode)
                    {
                        if (GoFullscreenCommand != null)
                        {
                            // Make the window fullscreen
                            GoFullscreenCommand.Execute(null);
                        }
                    }
                    else if (LeaveFullscreenCommand != null)
                    {
                        // Return to the previous window state
                        LeaveFullscreenCommand.Execute(null);
                    }
                }
            }
        }

        private bool joystickAvailable = false;
        /// <summary>
        /// Gets or sets a value indicating whether a virtual joystick device is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a virtual joystick device is available; otherwise, <c>false</c>.
        /// </value>
        public bool JoystickAvailable
        {
            get { return joystickAvailable; }
            set { SetValue(ref joystickAvailable, value, "JoystickAvailable"); }
        }

        private SerializableDictionary<Point, CellSettings> CellSettings = new SerializableDictionary<Point, CellSettings>();

        /// <summary>
        /// The cells and their stored actions. This is bound to by the grid view.
        /// </summary>
        /// <value>
        /// The cells.
        /// </value>
        public IEnumerable<CellSettings> Cells
        {
            get
            {
                foreach (var cell in CellSettings.OrderBy(cs => cs.Key.Y).ThenBy(cs => cs.Key.X))
                {
                    yield return cell.Value;
                }
            }
        }

        #region Commands
        public ICommand LoadSettingsCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
        public ICommand GoFullscreenCommand { get; set; }
        public ICommand LeaveFullscreenCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            // Workout if a virtual joystick device is available 
            JoystickAvailable = JoystickInputSender.Initialise();

            // Generate the grid for the first time
            OnGenerateGrid();
        }
        #endregion

        private void OnGenerateGrid()
        {
            GenerateSampleGrid();

            OnPropertyChanged("Cells");
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
            var point = new Point(col, row);
            if (CellSettings.ContainsKey(point) == false)
            {
                CellSettings[point] = new CellSettings()
                {
                    CellAction = new CellAction()
                    {
                        ActionType = CellActionType.KeyDown,
                        KeyCode = Key.A,
                        StringEntryString = "test"
                    },
                    DisplayText = string.Format("col:{0}, row:{1}", col, row),
                    XPosition = col,
                    YPosition = row
                };
            }
        }

        /// <summary>
        /// Called when a cell is activated (clicked or mouseovered)
        /// </summary>
        /// <param name="cell">The cell that was activated.</param>
        /// <exception cref="Exception">Unknown CellActionType</exception>
        public void ActivateCell(Border cell)
        {
            // Cells can only activate if we are in test mode or if the user has left edit mode
            if (TestMode || EditMode == false)
            {
                var setting = (CellSettings)(cell.DataContext);

                // Work out what action to perform
                var action = setting.CellAction;
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

        /// <summary>
        /// Called when a cell is deactivated (the click button was released or the mouse left the cell's boundary)
        /// </summary>
        /// <param name="cell">The cell that was deactivated.</param>
        public void DeactivateCell(Border cell)
        {
            // Cells can only deactivate if we are in test mode or if the user has left edit mode
            if (TestMode || EditMode == false)
            {
                var setting = (CellSettings)(cell.DataContext);

                var action = setting.CellAction;
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

        public void EditCell(Border cell)
        {
            var setting = (CellSettings)(cell.DataContext);

            var editWindow = new CellConfigurationViewModel(setting);
            if (editWindow.ShowDialog())
            {
                // The user clicked OK

                //TODO: ?
                OnGenerateGrid();
            }
        }

        /// <summary>
        /// Saves the application's settings.
        /// </summary>
        /// <param name="filename">The name of a file which will contain the saved settings.</param>
        public void SaveSettings(string filename)
        {
            // Construct the settings object
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

            // Write the object to a file in XML format
            using (var writer = new StreamWriter(filename))
            {
                var serializer = new XmlSerializer(typeof(ClientSettings));
                serializer.Serialize(writer, settings);

                writer.Close();
            }
        }

        /// <summary>
        /// Loads the application's settings.
        /// </summary>
        /// <param name="filename">The name of the file that should be loaded.</param>
        public void LoadSettings(string filename)
        {
            ClientSettings clientSettings;

            // Populate the settings object from the supplied XML file
            using (var reader = new StreamReader(filename))
            {
                var serializer = new XmlSerializer(typeof(ClientSettings));
                clientSettings = (ClientSettings)serializer.Deserialize(reader);

                reader.Close();
            }

            // Set the application's properties based on the contents of the file
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
