 
// --------------------------------------------------------------------------------------------------------------------
// <auto-generated> 
//   This code was generated by a T4 template. 
//
//   Changes to this file may cause incorrect behavior and will be lost if 
//   the code is regenerated. 
// </auto-generated>
// <copyright file="FuelConsumption.cs" company="Webcorp">
//   Copyright (c) 2015 Webcorp contributors
// </copyright>
// <summary>
//   Represents the fuel consumption quantity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Webcorp.unite
{
    using System;
	using System.ComponentModel;
    using System.Globalization;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
#if REACTIVE_CORE
	using ReactiveCore;
#endif
#if MONGO
	using MongoDB.Bson.Serialization.Attributes;
	using MongoDB.Bson.Serialization.Serializers;
    using MongoDB.Bson.Serialization;
#endif

    /// <summary>
    /// Represents the fuel consumption quantity.
    /// </summary>
    
#if !CORE
    [Serializable]
#endif
	[DataContract]
	[TypeConverter(typeof(UnitTypeConverter<FuelConsumption>))]
    public partial class FuelConsumption : Unit<FuelConsumption>
    {
        /// <summary>
        /// The backing field for the <see cref="LitrePer100Kilometre" /> property.
        /// </summary>
		[Unit("l/100km", true)]
		private static readonly FuelConsumption LitrePer100KilometreField = new FuelConsumption(1e5);

		private readonly List<string> registeredSymbols;

		public override List<string> RegisteredSymbols=>registeredSymbols;
        /// <summary>
        /// The value.
        /// </summary>
        private double value;

		/// <summary>
        /// Initializes a new instance of the <see cref="FuelConsumption"/> struct.
        /// </summary>
        public FuelConsumption():this(0.0)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuelConsumption"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        public FuelConsumption(double value)
        {
            this.value = value;
			registeredSymbols = new List<string>() { "l/100km"};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuelConsumption"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="unitProvider">
        /// The unit provider. 
        /// </param>
        public FuelConsumption(string value, IUnitProvider unitProvider = null)
        {
            this.value = Parse(value, unitProvider ?? UnitProvider.Default).value;
			registeredSymbols = new List<string>() { "l/100km"};
        }

        /// <summary>
        /// Gets the "l/100km" unit.
        /// </summary>
		[Unit("l/100km", true)]
		        public static FuelConsumption LitrePer100Kilometre
        {
            get { return LitrePer100KilometreField; }
        }

        /// <summary>
        /// Gets or sets the fuel consumption as a string.
        /// </summary>
        /// <value>The string.</value>
        /// <remarks>
        /// This property is used for XML serialization.
        /// </remarks>
        //[XmlText]
        [DataMember]
		//[BsonSerializer(typeof(UnitSerializer))]
		//[BsonSerializer(typeof(FuelConsumptionSerializer))]
        public string FValue
        {
            get
            {
                // Use round-trip format
                return this.ToString("R", CultureInfo.InvariantCulture);
            }

            set
            {
			#if REACTIVE_CORE
				this.RaiseAndSetIfChanged(ref this.value, Parse(value, CultureInfo.InvariantCulture).value);
			#else
                this.value = Parse(value, CultureInfo.InvariantCulture).value;
			#endif
            }
        }

        /// <summary>
        /// Gets or sets the value of the fuel consumption in the base unit.
        /// </summary>
        public override double Value
        {
            get{
                return this.value;
            }

			set{
				this.value = value;
			}

        }

		 /// <summary>
        /// Gets if fuel consumption is variable or not
        /// </summary>
		public override bool VariableValue { get {return false; } }

        /// <summary>
        /// Converts a string representation of a quantity in a specific culture-specific format with a specific unit provider.
        /// </summary>
        /// <param name="input">
        /// A string that contains the quantity to convert. 
        /// </param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="input" />. If not specified, the culture of the default <see cref="UnitProvider" /> is used. 
        /// </param>
        /// <param name="unitProvider">
        /// The unit provider. If not specified, the default <see cref="UnitProvider" /> is used.
        /// </param>
        /// <returns>
        /// A <see cref="FuelConsumption"/> that represents the quantity in <paramref name="input" />. 
        /// </returns>
        public static FuelConsumption Parse(string input, IFormatProvider provider, IUnitProvider unitProvider)
        {
            if (unitProvider == null)
            {
                unitProvider = provider as IUnitProvider ?? UnitProvider.Default;
            }

            FuelConsumption value;
            if (!unitProvider.TryParse(input, provider, out value))
            {
				return new  FuelConsumption(0);
                //throw new FormatException("Invalid format.");
            }

            return value;
        }

        /// <summary>
        /// Converts a string representation of a quantity in a specific culture-specific format.
        /// </summary>
        /// <param name="input">
        /// A string that contains the quantity to convert. 
        /// </param>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information about <paramref name="input" />. If not specified, the culture of the default <see cref="UnitProvider" /> is used. 
        /// </param>
        /// <returns>
        /// A <see cref="FuelConsumption"/> that represents the quantity in <paramref name="input" />. 
        /// </returns>
        public static FuelConsumption Parse(string input, IFormatProvider provider = null)
        {
            var unitProvider = provider as IUnitProvider ?? UnitProvider.Default;

            FuelConsumption value;
            if (!unitProvider.TryParse(input, provider, out value))
            {
				return new  FuelConsumption(0);
                //throw new FormatException("Invalid format.");
            }

            return value;
        }

        /// <summary>
        /// Converts a string representation of a quantity with a specific unit provider.
        /// </summary>
        /// <param name="input">
        /// A string that contains the quantity to convert. 
        /// </param>
        /// <param name="unitProvider">
        /// The unit provider. If not specified, the default <see cref="UnitProvider" /> is used.
        /// </param>
        /// <returns>
        /// A <see cref="FuelConsumption"/> that represents the quantity in <paramref name="input" />. 
        /// </returns>
        public static FuelConsumption Parse(string input, IUnitProvider unitProvider)
        {
            if (unitProvider == null)
            {
                unitProvider = UnitProvider.Default;
            }

            FuelConsumption value;
            if (!unitProvider.TryParse(input, unitProvider.Culture, out value))
            {
				return new  FuelConsumption(0);
                //throw new FormatException("Invalid format.");
            }

            return value;
        }

        /// <summary>
        /// Tries to parse the specified string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="provider">The format provider.</param>
        /// <param name="unitProvider">The unit provider.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if the string was parsed, <c>false</c> otherwise.</returns>
        public static bool TryParse(string input, IFormatProvider provider, IUnitProvider unitProvider, out FuelConsumption result)
        {
            if (unitProvider == null)
            {
                unitProvider = provider as IUnitProvider ?? UnitProvider.Default;
            }

            return unitProvider.TryParse(input, provider, out result);
        }

        /// <summary>
        /// Parses the specified JSON string.
        /// </summary>
        /// <param name="input">The JSON input.</param>
        /// <returns>
        /// The <see cref="FuelConsumption"/> .
        /// </returns>
        public static FuelConsumption ParseJson(string input)
        {
            return Parse(input, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="x">
        /// The first value. 
        /// </param>
        /// <param name="y">
        /// The second value. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator +(FuelConsumption x, FuelConsumption y)
        {
            return new FuelConsumption(x.value + y.value);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator /(FuelConsumption x, double y)
        {
            return new FuelConsumption(x.value / y);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static double operator /(FuelConsumption x, FuelConsumption y)
        {
            return x.value / y.value;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static bool operator ==(FuelConsumption x, FuelConsumption y)
        {
            return x.value.Equals(y.value);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static bool operator >(FuelConsumption x, FuelConsumption y)
        {
            return x.value > y.value;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static bool operator >=(FuelConsumption x, FuelConsumption y)
        {
            return x.value >= y.value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static bool operator !=(FuelConsumption x, FuelConsumption y)
        {
            return !x.value.Equals(y.value);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static bool operator <(FuelConsumption x, FuelConsumption y)
        {
            return x.value < y.value;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static bool operator <=(FuelConsumption x, FuelConsumption y)
        {
            return x.value <= y.value;
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator *(double x, FuelConsumption y)
        {
            return new FuelConsumption(x * y.value);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator *(FuelConsumption x, double y)
        {
            return new FuelConsumption(x.value * y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator -(FuelConsumption x, FuelConsumption y)
        {
            return new FuelConsumption(x.value - y.value);
        }

        /// <summary>
        /// Implements the unary plus operator.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator +(FuelConsumption x)
        {
            return new FuelConsumption(x.value);
        }

        /// <summary>
        /// Implements the unary minus operator.
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <returns>
        /// The result of the operator. 
        /// </returns>
        public static FuelConsumption operator -(FuelConsumption x)
        {
            return new FuelConsumption(-x.value);
        }

        /// <summary>
        /// Compares this instance to the specified <see cref="FuelConsumption"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="FuelConsumption"/> . 
        /// </param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the other value. 
        /// </returns>
        public override int CompareTo(FuelConsumption other)
        {
            return this.value.CompareTo(other.value);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the 
        /// current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: 
        /// Value Meaning Less than zero This instance is less than <paramref name="obj" />. Zero This instance is equal to 
        /// <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="obj" />.
        /// </returns>
        public override int CompareTo(object obj)
        {
            return this.CompareTo((FuelConsumption)obj);
        }

        /// <summary>
        /// Converts the quantity to the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>The amount of the specified unit.</returns>
		public override double ConvertTo(IUnit unit)
        {
            return this.ConvertTo((FuelConsumption)unit);
        }

        /// <summary>
        /// Converts to the specified unit.
        /// </summary>
        /// <param name="unit">
        /// The unit. 
        /// </param>
        /// <returns>
        /// The value in the specified unit. 
        /// </returns>
        public double ConvertTo(FuelConsumption unit)
        {
            return this.value / unit.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance. 
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is FuelConsumption)
            {
                return this.Equals((FuelConsumption)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines if the specified <see cref="FuelConsumption"/> is equal to this instance.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="FuelConsumption"/> . 
        /// </param>
        /// <returns>
        /// True if the values are equal. 
        /// </returns>
        public override bool Equals(FuelConsumption other)
        {
            return this.value.Equals(other.value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Multiplies by the specified number.
        /// </summary>
        /// <param name="x">The number.</param>
        /// <returns>The new quantity.</returns>
        public override IUnit MultiplyBy(double x)
        {
            return this * x;
        }

        /// <summary>
        /// Adds the specified quantity.
        /// </summary>
        /// <param name="x">The quantity to add.</param>
        /// <returns>The sum.</returns>
        public override IUnit Add(IUnit x)
        {
            if (!(x is FuelConsumption))
            {
                throw new InvalidOperationException("Can only add quantities of the same types.");
            }

            return new FuelConsumption(this.value + x.Value);
        }

        /// <summary>
        /// Sets the value from the specified string.
        /// </summary>
        /// <param name="s">
        /// The s. 
        /// </param>
        /// <param name="provider">
        /// The provider. 
        /// </param>
        public void SetFromString(string s, IUnitProvider provider)
        {
            this.value = Parse(s, provider).value;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public override string ToString()
        {
            return this.ToString(null, UnitProvider.Default);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">
        /// The format provider. 
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            var unitProvider = formatProvider as IUnitProvider ?? UnitProvider.Default;

            return this.ToString(null, formatProvider, unitProvider);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">
        /// The format. 
        /// </param>
        /// <param name="formatProvider">
        /// The format provider. 
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public override string ToString(string format, IFormatProvider formatProvider = null)
        {
            var unitProvider = formatProvider as IUnitProvider ?? UnitProvider.Default;

            return this.ToString(format, formatProvider, unitProvider);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">
        /// The format. 
        /// </param>
        /// <param name="formatProvider">
        /// The format provider. 
        /// </param>
        /// <param name="unitProvider">
        /// The unit provider. 
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public  string ToString(string format, IFormatProvider formatProvider, IUnitProvider unitProvider)
        {
            if (unitProvider == null)
            {
                unitProvider = formatProvider as IUnitProvider ?? UnitProvider.Default;
            }

            return unitProvider.Format(format, formatProvider, this);
        }
    }
#if MONGO
	public class FuelConsumptionSerializer:SerializerBase<FuelConsumption>{
		public override FuelConsumption Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var up = UnitProvider.Default;
            IUnit result;
            if(up.TryGetUnit(typeof(FuelConsumption), context.Reader.ReadString(), out result))
                return (FuelConsumption)result;

            return base.Deserialize(context, args);
        } 
	}
#endif
	public enum FuelConsumptionUnit{
		LitrePer100Kilometre
	}

}
