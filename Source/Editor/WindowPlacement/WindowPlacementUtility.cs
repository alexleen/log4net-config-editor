// Copyright © 2018 Alex Leendertsen

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Xml;
using System.Xml.Serialization;

// ReSharper disable InconsistentNaming

namespace Editor.WindowPlacement
{
    /// <summary>
    /// Handles window size and location persistence.
    /// </summary>
    /// <remarks>
    /// Stolen from https://blogs.msdn.microsoft.com/davidrickard/2010/03/08/saving-window-size-and-location-in-wpf-and-winforms/
    /// </remarks>
    public static class WindowPlacementUtility
    {
        private static readonly Encoding sEncoding = new UTF8Encoding();
        private static readonly XmlSerializer sSerializer = new XmlSerializer(typeof(WINDOWPLACEMENT));

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;

        private static void SetPlacement(IntPtr windowHandle, string placementXml)
        {
            if (string.IsNullOrEmpty(placementXml))
            {
                return;
            }

            byte[] xmlBytes = sEncoding.GetBytes(placementXml);

            try
            {
                WINDOWPLACEMENT placement;
                using (MemoryStream memoryStream = new MemoryStream(xmlBytes))
                {
                    placement = (WINDOWPLACEMENT)sSerializer.Deserialize(memoryStream);
                }

                placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                placement.flags = 0;
                placement.showCmd = placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : placement.showCmd;
                SetWindowPlacement(windowHandle, ref placement);
            }
            catch (InvalidOperationException)
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        public static void SetPlacement(this Window window, string placementXml)
        {
            SetPlacement(new WindowInteropHelper(window).Handle, placementXml);
        }

        private static string GetPlacement(IntPtr windowHandle)
        {
            GetWindowPlacement(windowHandle, out WINDOWPLACEMENT placement);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    sSerializer.Serialize(xmlTextWriter, placement);
                    byte[] xmlBytes = memoryStream.ToArray();
                    return sEncoding.GetString(xmlBytes);
                }
            }
        }

        public static string GetPlacement(this Window window)
        {
            return GetPlacement(new WindowInteropHelper(window).Handle);
        }
    }
}
