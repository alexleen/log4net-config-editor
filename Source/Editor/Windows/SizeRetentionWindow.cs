// Copyright © 2018 Alex Leendertsen

using System;
using System.ComponentModel;
using System.Windows;
using Editor.Properties;
using Editor.WindowPlacement;

namespace Editor.Windows
{
    public class SizeRetentionWindow : Window
    {
        private readonly string mSettingName;

        /// <summary>
        /// Constructs a window that retains its size.
        /// </summary>
        /// <param name="settingName">Name of the setting where location information is stored.</param>
        protected SizeRetentionWindow(string settingName)
        {
            mSettingName = settingName;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            this.SetPlacement((string)Settings.Default[mSettingName]);
            base.OnSourceInitialized(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.Default[mSettingName] = this.GetPlacement();
            Settings.Default.Save();
            base.OnClosing(e);
        }
    }
}
