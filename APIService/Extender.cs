using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace APIService
{
    public static class Extender
    {
        public static string AssemblyVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.FileVersion;
            }
        }

        public static string ExecutingAssemblyName
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
        }

        public static string PathCombinedXml
        {
            get
            {
                return Path.Combine(AppContext.BaseDirectory, $"{ExecutingAssemblyName}.xml");
            }
        }

        public static Assembly AssemblyDetails {
            get
            {
                return Assembly.GetExecutingAssembly();
            }
        }

        public static FileVersionInfo AssemblyInfo
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(AssemblyDetails.Location);
            }
        }
    }
}
