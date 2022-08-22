using Microsoft.Win32;
using System;
using System.Security.Cryptography.X509Certificates;

namespace SSL_Fix
{
    public class SSL
    {
        public static bool sslInstallStatus;
        public static bool sslKeyExists;
        public static void InstallCertificate()
        {
            var cerFileName = "B7AB3308D1EA4477BA1480125A6FBDA936490CBB.crt";
            X509Certificate2 cert = new X509Certificate2(cerFileName);
            X509Store store2 = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            X509Store store3 = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);
            X509Store store4 = new X509Store(StoreName.AuthRoot, StoreLocation.LocalMachine);
            
            try
            {
                store2.Open(OpenFlags.ReadWrite);
                Console.WriteLine("Installing Intermediate Cert");
                store2.Add(cert);
                Console.WriteLine("Intermediate Cert Installed");

                store3.Open(OpenFlags.ReadWrite);
                Console.WriteLine("Installing Root and Trusted Cert");
                store3.Add(cert);
                Console.WriteLine("Root and Trusted Cert Installed");

                store4.Open(OpenFlags.ReadWrite);
                Console.WriteLine("Installing 3rd Party Cert");
                store4.Add(cert);
                Console.WriteLine("3rd Party Cert Installed");

                Console.WriteLine("Done! The CA certificate was successfully. Press any key to close.");
                sslInstallStatus = true;
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                Console.WriteLine("Unable to install certificate\n Please run as Admin");
                sslInstallStatus = false;
            }
            store2.Close();
            store3.Close();
            store4.Close();
        }
        public static void checkSslExists()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\SystemCertificates\AuthRoot\Certificates\B7AB3308D1EA4477BA1480125A6FBDA936490CBB");
            if (key != null)
            {
                Console.WriteLine("SSL Cert is present");
                sslKeyExists = true;
            }
            else
            {
                Console.WriteLine("SSL Cert is NOT present");
                sslKeyExists = false;
            }
        }
    }
}
