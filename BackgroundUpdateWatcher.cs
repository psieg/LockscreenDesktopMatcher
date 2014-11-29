/*
The MIT License (MIT)

Copyright (c) 2014 psieg

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockscreenDesktopMatcher
{
    class BackgroundUpdateWatcher
    {
        private ManagementEventWatcher watcher;

        public BackgroundUpdateWatcher()
        {
            string SID = WindowsIdentity.GetCurrent().User.ToString();
            WqlEventQuery query = new WqlEventQuery(
                          "SELECT * FROM RegistryValueChangeEvent WHERE " +
                     "Hive = 'HKEY_USERS'" +
                     "AND KeyPath = '" + SID + @"\\Control Panel\\Desktop' AND ValueName='TranscodedImageCache'");

            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += new EventArrivedEventHandler(UpdatePolicyFromUserBackground);
        }

        public void start()
        {
            watcher.Start();
            UpdatePolicyFromUserBackground(this, null);
        }
        
        public void stop()
        {
            watcher.Stop();
        }

        private void UpdatePolicyFromUserBackground(object sender, EventArrivedEventArgs e)
        {
            var v = Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "TranscodedImageCache", null);
            string s = System.Text.Encoding.Unicode.GetString((Byte[])v).Substring(12).Replace("\0", string.Empty);
            try
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Personalization", "LockScreenImage", s);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Failed to update Policy Override. Ensure this user can write to 'HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\Personalization'.", 
                    "LockScreen Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
