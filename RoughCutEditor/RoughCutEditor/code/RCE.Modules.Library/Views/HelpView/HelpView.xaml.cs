// <copyright file="HelpView.xaml.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: HelpView.xaml.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Library
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Window for displaying all the shortcut keys used in the application.
    /// Window for displaying all the shortcut keys used in the application.
    /// </summary>
    public partial class HelpView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpView"/> class.
        /// </summary>
        public HelpView()
        {
            InitializeComponent();
            KeyBoardCommands().ForEach(x => this.KeyBoardCommandsList.Items.Add(x));
        }

        /// <summary>
        /// List of all keyboar shortcuts.
        /// </summary>
        /// <returns><see cref="List{T}"/> of all keyboar shortcuts.</returns>
        private static List<KeyboardCommands> KeyBoardCommands()
        {
            return new List<KeyboardCommands>()
                {
                    // Global keyboard mappings.
                     // new KeyboardCommands() { KeyName = "Ctrl + 1", Description = "Show Library module" },
                     // new KeyboardCommands() { KeyName = "Ctrl + 2", Description = "Show MediaBin module" },
                     // new KeyboardCommands() { KeyName = "Ctrl + 3", Description = "Show Comment module" },
                     // new KeyboardCommands() { KeyName = "Ctrl + 4", Description = "Show Titles module" },
                     // new KeyboardCommands() { KeyName = "Ctrl + 5", Description = "Show LiveStreaming module" },
                     // new KeyboardCommands() { KeyName = "Ctrl + 6", Description = "Show Project Module" },
                     // new KeyboardCommands() { KeyName = "Ctrl + 7", Description = "Show Setting Module" },
                    new KeyboardCommands { KeyName = "Ctrl + S", Description = "Save the current project" },
                    new KeyboardCommands { KeyName = "Esc", Description = "Stops playback. Exits Full Screen in fullscreen mode" },
                    new KeyboardCommands { KeyName = "F12", Description = "Plays the timeline" },
                    new KeyboardCommands { KeyName = "F9 / Space", Description = "Toggles play and pause" },

                    // new KeyboardCommands(){KeyName = "F1", Description="Displays keyboard shortcuts and lik to help documentation"},
                    // Player keyboard mappings.
                    new KeyboardCommands { KeyName = "Home", Description = "Jumps to head of the time line. The first frame" },
                    
                    // new KeyboardCommands(){KeyName = "J", Description="Reverse" },
                    
                    // new KeyboardCommands(){KeyName = "L", Description="Fast Forward"},
                    new KeyboardCommands { KeyName = "End", Description = "Jumps to end of timeline" },
                    new KeyboardCommands { KeyName = "Q", Description = "Sets the player to loop" },
                    new KeyboardCommands { KeyName = "C", Description = "Adds a new comment at playhead position" },
                    new KeyboardCommands { KeyName = "Enter", Description = "Sets the player to full screen mode" },
                    
                    // new KeyboardCommands { KeyName = "M", Description = "Displays details about the clip or timeline" },
                    // new KeyboardCommands(){KeyName = "M", Description="Adds a marker type comment"},
                    // new KeyboardCommands(){KeyName = "Left Arrow and 1", Description="Step 1 Frame back"},
                    // new KeyboardCommands(){KeyName = "Right Arrow and 3", Description="Step 1 frame forward"},
                    new KeyboardCommands { KeyName = "Z", Description = "Mutes the audio" },
                    
                    // Timeline and thumbnail editing keyboard mappings.
                    new KeyboardCommands { KeyName = "I", Description = "Marks the inpoint" },
                    new KeyboardCommands { KeyName = "O", Description = "Marks the outpoint" },
                    new KeyboardCommands { KeyName = "Space", Description = "Toggles between play and pause" },
                    new KeyboardCommands { KeyName = "Delete and BackSpace", Description = "Deletes the currently selected clip" },
                    new KeyboardCommands { KeyName = "S", Description = "Splits the clip at the playhead" },
                    new KeyboardCommands { KeyName = "Ctrl + Z", Description = "Undo" },
                    new KeyboardCommands { KeyName = "Ctrl + Y", Description = "Redo" },
                    new KeyboardCommands { KeyName = "F", Description = "Toggle ripple mode on/off" },
                    new KeyboardCommands { KeyName = "Up Arrow", Description = "Zooms the timeline in on the playhead" },
                    new KeyboardCommands { KeyName = "Down Arrow", Description = "Zooms the timeline out on the playhead" },
                    
                    // new KeyboardCommands(){KeyName = "/", Description="Cuts the currenly selected item"},
                    // new KeyboardCommands(){KeyName = "*", Description="Inserts a new crossfade transition at the closet edit point."},
                    new KeyboardCommands { KeyName = "A", Description = "Adds the currently selected Media in media bin from the cursor position" },
                    new KeyboardCommands { KeyName = "Ctrl + X", Description = "Copy currently selected asset in media bin to clipboard." },
                    new KeyboardCommands { KeyName = "Ctrl + V", Description = "Paste the currently copied asset to the current folder in media bin." },
                };
        }
    }
}
