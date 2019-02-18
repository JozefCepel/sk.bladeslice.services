using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.ServiceModel.Channels;

namespace WebEas.Client.Encoder
{
    public class MessageVersionConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type
        /// to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
        /// that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type
        /// you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(string) == sourceType)
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified
        /// type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
        /// that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the
        /// type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (typeof(InstanceDescriptor) == destinationType)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified
        /// context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
        /// that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to
        /// use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.
        /// </exception>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string messageVersion = (string)value;
                MessageVersion retval = null;
                switch (messageVersion)
                {
                    case ConfigurationStrings.Soap11WSAddressing10:
                        retval = MessageVersion.Soap11WSAddressing10;
                        break;
                    case ConfigurationStrings.Soap12WSAddressing10:
                        retval = MessageVersion.Soap12WSAddressing10;
                        break;
                    case ConfigurationStrings.Soap11WSAddressingAugust2004:
                        retval = MessageVersion.Soap11WSAddressingAugust2004;
                        break;
                    case ConfigurationStrings.Soap12WSAddressingAugust2004:
                        retval = MessageVersion.Soap12WSAddressingAugust2004;
                        break;
                    case ConfigurationStrings.Soap11:
                        retval = MessageVersion.Soap11;
                        break;
                    case ConfigurationStrings.Soap12:
                        retval = MessageVersion.Soap12;
                        break;
                    case ConfigurationStrings.None:
                        retval = MessageVersion.None;
                        break;
                    case ConfigurationStrings.Default:
                        retval = MessageVersion.Default;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("messageVersion");
                }

                return retval;
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified
        /// context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
        /// that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If
        /// null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the
        /// <paramref name="value" /> parameter to.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" />
        /// parameter is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.
        /// </exception>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (typeof(string) == destinationType && value is MessageVersion)
            {
                string retval = null;
                MessageVersion messageVersion = (MessageVersion)value;
                if (messageVersion == MessageVersion.Default)
                {
                    retval = ConfigurationStrings.Default;
                }
                else if (messageVersion == MessageVersion.Soap11WSAddressing10)
                {
                    retval = ConfigurationStrings.Soap11WSAddressing10;
                }
                else if (messageVersion == MessageVersion.Soap12WSAddressing10)
                {
                    retval = ConfigurationStrings.Soap12WSAddressing10;
                }
                else if (messageVersion == MessageVersion.Soap11WSAddressingAugust2004)
                {
                    retval = ConfigurationStrings.Soap11WSAddressingAugust2004;
                }
                else if (messageVersion == MessageVersion.Soap12WSAddressingAugust2004)
                {
                    retval = ConfigurationStrings.Soap12WSAddressingAugust2004;
                }
                else if (messageVersion == MessageVersion.Soap11)
                {
                    retval = ConfigurationStrings.Soap11;
                }
                else if (messageVersion == MessageVersion.Soap12)
                {
                    retval = ConfigurationStrings.Soap12;
                }
                else if (messageVersion == MessageVersion.None)
                {
                    retval = ConfigurationStrings.None;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("messageVersion");
                }
                return retval;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}