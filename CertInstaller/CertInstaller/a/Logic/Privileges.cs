using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CertInstaller.a.Logic
{
    public class Privileges
    {
        /// <summary>
        /// A method for checking if we have administrator rights on the system to execute.
        /// We usually do because of the app manifest tells windows to load this program as administrator.
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            bool isAdministrator;
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                isAdministrator = false;
            }
            catch (Exception)
            {
                isAdministrator = false;
            }
            return isAdministrator;
        }
    }
}
