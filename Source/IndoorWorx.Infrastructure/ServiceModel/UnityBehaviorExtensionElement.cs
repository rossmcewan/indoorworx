using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using System.ServiceModel.Configuration;

namespace IndoorWorx.Infrastructure.ServiceModel
{
    /// <summary>
    /// Represents a configuration element that contains sub-elements that specify behavior extensions, which enable the user to customize service or endpoint behaviors.
    /// </summary>
    public class UnityBehaviorExtensionElement : BehaviorExtensionElement
    {
        /// <summary>
        /// The default path for the unity configuration.
        /// </summary>
        private const string DefaultUnityConfigurationSectionPath = "unity";

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityBehaviorExtensionElement"/> class.
        /// </summary>
        public UnityBehaviorExtensionElement()
        {
            this.UnityConfigurationSectionPath = DefaultUnityConfigurationSectionPath;
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" />.</returns>
        /// <value>The behavior type being used.</value>
        public override Type BehaviorType
        {
            get { return typeof(UnityServiceBehavior); }
        }

        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        /// <value>The name of the container.</value>
        [ConfigurationProperty("containerName")]
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets the unity configuration section path.
        /// </summary>
        /// <value>The unity configuration section path.</value>
        [ConfigurationProperty("unityConfigurationSectionPath", DefaultValue = DefaultUnityConfigurationSectionPath)]
        public string UnityConfigurationSectionPath { get; set; }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new UnityServiceBehavior(IoC.UnityContainer);

            UnityConfigurationSection unitySection = ConfigurationManager.GetSection(this.UnityConfigurationSectionPath) as UnityConfigurationSection;

            if (unitySection == null)
            {
                throw new ArgumentException("unitySection");
            }

            IUnityContainer container = new UnityContainer();

            if (this.ContainerName == null)
            {
                unitySection.Configure(container);
            }
            else
            {
                unitySection.Configure(container, this.ContainerName);
            }

            return new UnityServiceBehavior(container);
        }
    }
}
