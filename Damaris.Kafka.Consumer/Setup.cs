using Confluent.Kafka;
using Damaris.Kafka.Consumer.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Damaris.Kafka.Consumer
{
    [ExcludeFromCodeCoverage]
    public class Setup
    {

        public static void ConfigureCredentials<TSettings>(TSettings settings, ILogger logger) where TSettings : ClientConfig, IHasCredentials
        {
            TryDecompressResourceFiles(logger);
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrWhiteSpace(settings.SslCaLocation))
                settings.SslCaLocation = Path.Combine(currentDirectory, "ca.pem");

            if (settings.EnableSslCertificateVerification.GetValueOrDefault(true) &&
                settings.BootstrapServers.Split(',').All(s => IPAddress.TryParse(s.Split(':').First(), out _)))
            {
                logger.LogWarning("All bootstrap servers are IP addresses and SSL certificate verification might fail. " +
                                  "If so, consider setting 'EnableSslCertificateVerification' to 'false'.");
            }

            //if (settings.SaslMechanism == SaslMechanism.Gssapi)
            //{
            //    SetKerberosEnvironmentVariables(logger);

            //    if (string.IsNullOrWhiteSpace(settings.SaslKerberosPrincipal))
            //        settings.SaslKerberosPrincipal = settings.Username;

            //    if (string.IsNullOrWhiteSpace(settings.SaslKerberosKeytab) && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //    {
            //        var username = settings.Username.Split('@')[0];
            //        var realm = settings.Username.Split('@')[1];
            //        var keytabFilePath = Path.Combine(currentDirectory, "credentials.keytab");

            //        KeytabFile.Create(username, settings.Password, keytabFilePath, realm);
            //        settings.SaslKerberosKeytab = keytabFilePath;
            //    }
            //}
        }
        private static void SetKerberosEnvironmentVariables(ILogger logger)
        {
            const string KRB5_CONFIG = "KRB5_CONFIG";
            var krb5 = Environment.GetEnvironmentVariable(KRB5_CONFIG, EnvironmentVariableTarget.User);

            if (!string.IsNullOrEmpty(krb5))
                return;

            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "krb5.conf");
            Environment.SetEnvironmentVariable(KRB5_CONFIG, filePath, EnvironmentVariableTarget.User);
            logger.LogDebug($"Setting {nameof(KRB5_CONFIG)} = {Environment.GetEnvironmentVariable(KRB5_CONFIG, EnvironmentVariableTarget.User)}");
        }

        private static void TryDecompressResourceFiles(ILogger logger)
        {
            try
            {
                var assembly = Assembly.GetAssembly(typeof(Setup));
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
                foreach (var resourceName in assembly.GetManifestResourceNames())
                {
                    var fileName = resourceName.Replace(assembly.GetName().Name, string.Empty).Substring(1);
                    var targetFilePath = Path.Combine(currentDirectory, fileName);
                    if (File.Exists(targetFilePath))
                        continue;

                    var resource = assembly.GetManifestResourceStream(resourceName)!;
                    using var fs = new FileStream(targetFilePath, FileMode.Create);
                    resource.CopyTo(fs);
                    fs.Flush();

                    logger.LogInformation($"Decompressed \"{fileName}\"");
                }
            }
            catch (Exception e)
            {
                logger.LogCritical("Error when decompressing resource files. {0}", e);
                throw;
            }
        }
    }
}
