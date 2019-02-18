using System;
using System.Linq;
using System.ServiceModel.Configuration;

namespace WebEas.Client.Encoder
{
    /// <summary>
    /// Empty Action Behaviour Element
    /// </summary>
    public class EmptyActionBehaviourElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>The type of behavior.</returns>
        /// <value></value>
        public override Type BehaviorType
        {
            get
            {
                return typeof(EmptyActionBehaviour);
            }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new EmptyActionBehaviour();
        }
    }
}