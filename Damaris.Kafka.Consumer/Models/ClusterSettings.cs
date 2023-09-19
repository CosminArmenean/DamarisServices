using Confluent.Kafka;
using Damaris.Kafka.Consumer.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Text.Json;

namespace Damaris.Kafka.Consumer.Models
{
    /// <summary>
    /// Provides options that control the behavior of the Kafka subscriber
    /// </summary>
    /// <remarks>https://confluence.dell.com/display/DES/Consumers</remarks>
    /// <remarks>https://docs.confluent.io/platform/current/installation/configuration/consumer-configs.html</remarks>
    public class ClusterSettings : ConsumerConfig, IHasCredentials, ICloneable
    {
        private readonly IConfiguration _configuration;
        /// <inheritdoc/>
        public string Password { get; set; }

        /// <inheritdoc/>
        public string Username { get; set; }

        /// <summary>
        /// It have to be true, because of the lib flow. The lib uses automatic commit.
        /// </summary>
        public new bool EnableAutoCommit
        {
            get
            {
                return true;
            }

            set { }
        }

        /// <summary>
        /// It have to be false, because of the lib flow. The Consumer will call store offset after finish them work.
        /// </summary>
        public new bool EnableAutoOffsetStore
        {
            get
            {
                return false;
            }

            set { }
        }

        /// <summary>
        /// The topic to subscribe to.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Initialize a new <see cref="ClusterSettings"/> instance.
        /// </summary>
        public ClusterSettings()
        {
            

        }

        private ClusterSettings(IDictionary<string, string> config) : base(config)
        {
            config = LoadDefaultValues(_configuration);
        }

        public object Clone()
        {
            IConfiguration configuration;
            // Serialize all native Confluent settings to json and then deserialize them to a dictionary
            var json = JsonSerializer.Serialize(this);
            var pairs = JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(json);
            var dictionary = pairs.ToDictionary(pair => pair.Key, pair => pair.Value);

            // Apply the native settings plus the custom local settings
            return new ClusterSettings(dictionary)
            {
                Password = Password,
                Username = Username,
                Topic = Topic
            };
        }

        private Dictionary<string, string> LoadDefaultValues(IConfiguration configuration)
        {
            var settings = new Dictionary<string, string>();

            // Get the configuration section for ConsumerDefaultConfig
            var consumerDefaultConfig = configuration.GetSection("ConsumerDefaultConfig");

            // Get the "data" array from the configuration section
            var data = consumerDefaultConfig.Get<List<ConfigItem>>();

            // Loop through each item in the "data" array and add it to the dictionary
            foreach (var item in data)
            {
                settings[item.Name] = item.Value;
            }

            return settings;
        }
    }

    internal class ConfigItem {
        public  string Name { get; set; }
        public string Value { get; set; }
    }

}
