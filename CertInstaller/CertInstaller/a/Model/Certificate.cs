using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertInstaller.a.Model
{
    /// <summary>
    /// The internal model we use for the info we need from the certificate.
    /// </summary>
    public class Certificate
    {
        public string Name { get; set; }
        public string Thumbprint { get; set; }
        public DateTime Before { get; set; }
        public DateTime Expires { get; set; }
        public bool Success { get; set; }
    }
}
