using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinkInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(@"VRChat\shell\open\command", true);
            if (rkey != null)
            {
                Assembly myAssembly = Assembly.GetEntryAssembly();
                string path = myAssembly.Location;
                path = Path.GetDirectoryName(path) + @"\VRChatModeSwitcher.exe";
                rkey.SetValue("", "\"" + path + @""" ""%1"" %*");
            }
        }
    }
}
