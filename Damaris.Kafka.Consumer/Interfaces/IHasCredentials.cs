namespace Damaris.Kafka.Consumer.Interfaces
{
    public interface IHasCredentials
    {
        /// <summary>
        /// The user name/service account. Including domain. for example service_account@AMER.DELL.COM
        /// *** IMPORTANT: It is case-sensitive
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password of Service Account.
        /// *** IMPORTANT: It is case-sensitive
        /// </summary>
        public string Password { get; set; }
    }
}
