using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace SMEVService.Helpers
{
    public class KeyService
    {
        public static X509Certificate2 Certificate()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, "‎‎9a 8c 33 ab f0 36 0f d6 6d 15 f2 cb 48 c4 b5 e5 e7 cd 2e 2c", true);
            string thumbprint = ConfigurationManager.AppSettings["thumbprint"];
            X509Certificate2Collection fcollection = collection.Find(X509FindType.FindByThumbprint, thumbprint, true);

            foreach (X509Certificate2 x509Certificate2 in collection)
            {
                if (thumbprint.Equals(x509Certificate2.Thumbprint, StringComparison.InvariantCultureIgnoreCase))
                {
                    return x509Certificate2;
                }
            }
            return null;
        }
    }
}