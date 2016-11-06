using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CertInstaller.a.Model;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

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
            var mycert = GenerateCert();
            Certificate cert = new Certificate();
            cert.Thumbprint = mycert.Thumbprint;
            cert.Name = mycert.FriendlyName;
            cert.Before = mycert.NotBefore;
            cert.Expires = mycert.NotAfter;
            cert.Success = true;



            mycert.FriendlyName = "Jabra Cert";
            // Options
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            int yearsToExpire = 2;

            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection col = x509Store.Certificates;

            //x509Store.Add(mycert);

            ////Create placeholder certs
            //X509Certificate2 certSelected = null;
            //Certificate cert = new Certificate();
            //cert.Success = false;

            //// Enumerate windows certificate store
            //x509Store.Open(OpenFlags.ReadOnly);
            //X509Certificate2Collection col = x509Store.Certificates;

            //// Select certificate
            //foreach (X509Certificate2 c in col)
            //{

            //    // Find a valid certificate that does not expire any time soon!
            //    bool before = c.NotBefore.Date < DateTime.Now;
            //    bool expire = c.NotAfter.Date > DateTime.Now.AddYears(yearsToExpire);
            //    if (before && expire)
            //    {
            //        certSelected = c;
            //        cert.Name = c.Subject;
            //        cert.Thumbprint = c.Thumbprint;
            //        cert.Before = c.NotBefore;
            //        cert.Expires = c.NotAfter;
            //        cert.Success = true;
            //        break;
            //    }
            //}

            // return cert
            return cert;
        }

        private static X509Certificate2 GenerateCert()
        {
            var keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            var keypair = keypairgen.GenerateKeyPair();

            var gen = new X509V3CertificateGenerator();
            var CN = new X509Name("CN=" + "Jabra test");
            var SN = BigInteger.ProbablePrime(120, new Random());

            gen.SetSerialNumber(SN);
            gen.SetSubjectDN(CN);
            gen.SetIssuerDN(CN);
            gen.SetNotAfter(DateTime.MaxValue);
            gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            gen.SetSignatureAlgorithm("MD5WithRSA");
            gen.SetPublicKey(keypair.Public);

            var newCert = gen.Generate(keypair.Private);

            return new X509Certificate2(DotNetUtilities.ToX509Certificate((Org.BouncyCastle.X509.X509Certificate)newCert));
        }


        public static List<Certificate> GetAllCerts(string name, string location)
        {
            List<Certificate> list = new List<Certificate>();
            StoreName sName = StoreName.My;
            StoreLocation sLocation = StoreLocation.LocalMachine;

            switch (name)
            {
                case "MY":
                    sName = StoreName.My;
                    break;
                case "ROOT":
                    sName = StoreName.Root;
                    break;
                case "ADDRESS BOOK":
                    sName = StoreName.AddressBook;
                    break;
                case "AUTH ROOT":
                    sName = StoreName.AuthRoot;
                    break;
                case "CERTIFICATE AUTHORITY":
                    sName = StoreName.CertificateAuthority;
                    break;
                case "DISALLOWED":
                    sName = StoreName.Disallowed;
                    break;
                case "TRUSTED PEOPLE":
                    sName = StoreName.TrustedPeople;
                    break;
                case "TRUSTED PUBLISHER":
                    sName = StoreName.TrustedPublisher;
                    break;
            }

            switch (location)
            {
                case "Local Machine":
                    sLocation = StoreLocation.LocalMachine;
                    break;
                case "Current User":
                    sLocation = StoreLocation.CurrentUser;
                    break;
            }


            X509Store x509Store = new X509Store(sName, sLocation);
            int yearsToExpire = 2;

            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection col = x509Store.Certificates;

            foreach (X509Certificate2 c in col)
            {
                Certificate cert = new Certificate();
                // Find a valid certificate that does not expire any time soon!
                bool before = c.NotBefore.Date < DateTime.Now;
                bool expire = c.NotAfter.Date > DateTime.Now.AddYears(yearsToExpire);
                if (before && expire)
                {
                    cert.Name = c.Subject;
                    cert.Thumbprint = c.Thumbprint;
                    cert.Before = c.NotBefore;
                    cert.Expires = c.NotAfter;
                    cert.Success = true;
                    list.Add(cert);
                }
            }
            return list;
        }
    }
}
