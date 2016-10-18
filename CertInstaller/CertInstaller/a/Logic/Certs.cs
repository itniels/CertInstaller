using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CertInstaller.a.Model;

namespace CertInstaller.a.Logic
{
    class Certs
    {
        /// <summary>
        /// Loads up the windows certificate store and gets the first good-looking cetificate.
        /// </summary>
        /// <returns></returns>
        public static Certificate GetCert()
        {
            // Options
            X509Store x509Store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            int yearsToExpire = 2;
            
            // Create placeholder certs
            X509Certificate2 certSelected = null;
            Certificate cert = new Certificate();
            cert.Success = false;

            // Enumerate windows certificate store
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection col = x509Store.Certificates;

            // Select certificate
            foreach (X509Certificate2 c in col)
            {

                // Find a valid certificate that does not expire any time soon!
                bool before = c.NotBefore.Date < DateTime.Now;
                bool expire = c.NotAfter.Date > DateTime.Now.AddYears(yearsToExpire);
                if (before && expire)
                {
                    certSelected = c;
                    cert.Name = c.Subject;
                    cert.Thumbprint = c.Thumbprint;
                    cert.Before = c.NotBefore;
                    cert.Expires = c.NotAfter;
                    cert.Success = true;
                    break;
                }
            }

            // return cert
            return cert;
        }
    }
}
