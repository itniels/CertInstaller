using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertInstaller.a.Logic
{
    public class IO
    {
        /// <summary>
        /// A simple Filesystem IO method to check if the file we need is there or not.
        /// </summary>
        /// <returns></returns>
        public static bool foundMSI()
        {
            return File.Exists("JabraWebSocketServiceSetup.msi");
        }
    }
}
