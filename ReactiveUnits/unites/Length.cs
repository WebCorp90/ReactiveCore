 
// --------------------------------------------------------------------------------------------------------------------
// <auto-generated> 
//   This code was generated by a T4 template. 
//
//   Changes to this file may cause incorrect behavior and will be lost if 
//   the code is regenerated. 
// </auto-generated>
// <copyright file="Length.cs" company="Webcorp">
//   Copyright (c) 2015 Webcorp contributors
// </copyright>
// <summary>
//   Represents the length quantity.
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
    /// Represents the length quantity.
    /// </summary>
    
#if !CORE
    [Serializable]
#endif
	[DataContract]
	[TypeConverter(typeof(UnitTypeConverter<Length>))]
    public partial class Length : Unit<Length>
    {
        /// <summary>
        /// The backing field for the <see cref="Metre" /> property.
        /// </summary>
		[Unit("m", true)]
		private static readonly Length MetreField = new Length(1);

        /// <summary>
        /// The backing field for the <see cref="Decimetre" /> property.
        /// </summary>
		[Unit("dm")]
		private static readonly Length DecimetreField = new Length(1e-1);

        /// <summary>
        /// The backing field for the <see cref="Centimetre" /> property.
        /// </summary>
		[Unit("cm")]
		private static readonly Length CentimetreField = new Length(1e-2);

        /// <summary>
        /// The backing field for the <see cref="Millimetre" /> property.
        /// </summary>
		[Unit("mm")]
		private static readonly Length MillimetreField = new Length(1e-3);

        /// <summary>
        /// The backing field for the <see cref="Kilometre" /> property.
        /// </summary>
		[Unit("km")]
		private static readonly Length KilometreField = new Length(1e3);

        /// <summary>
        /// The backing field for the <see cref="Yard" /> property.
        /// </summary>
		[Unit("yd")]
		private static readonly Length YardField = new Length(0.9144);

        /// <summary>
        /// The backing field for the <see cref="Foot" /> property.
        /// </summary>
		[Unit("ft")]
		private static readonly Length FootField = new Length(0.3048);

        /// <summary>
        /// The backing field for the <see cref="Inch" /> property.
        /// </summary>
		[Unit("in")]
		private static readonly Length InchField = new Length(0.0254);

        /// <summary>
        /// The backing field for the <see cref="HundredthInch" /> property.
        /// </summary>
		[Unit("")]
		private static readonly Length HundredthInchField = new Length(2.54e-4);

        /// <summary>
        /// The backing field for the <see cref="Mile" /> property.
        /// </summary>
		[Unit("mi")]
		private static readonly Length MileField = new Length(1609.344);

        /// <summary>
        /// The backing field for the <see cref="NauticalMile" /> property.
        /// </summary>
		[Unit("nmi")]
		private static readonly Length NauticalMileField = new Length(1852);

        /// <summary>
        /// The backing field for the <see cref="Ångström" /> property.
        /// </summary>
		[Unit("Å")]
		private static readonly Length ÅngströmField = new Length(1e-10);

        /// <summary>
        /// The backing field for the <see cref="AstronomicalUnit" /> property.
        /// </summary>
		[Unit("AU")]
		private static readonly Length AstronomicalUnitField = new Length(149597871464);

        /// <summary>
        /// The backing field for the <see cref="LightYear" /> property.
        /// </summary>
		[Unit("ly")]
		private static readonly Length LightYearField = new Length(9.4607304725808e15);

		private readonly List<string> registeredSymbols;

		public override List<string> RegisteredSymbols=>registeredSymbols;
        /// <summary>
        /// The value.
        /// </summary>
        private double value;

		/// <summary>
        /// Initializes a new instance of the <see cref="Length"/> struct.
        /// </summary>
        public Length():this(0.0)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Length"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        public Length(double value)
        {
            this.value = value;
			registeredSymbols = new List<string>() { "m","dm","cm","mm","km","yd","ft","in","","mi","nmi","Å","AU","ly"};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Length"/> struct.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="unitProvider">
        /// The unit provider. 
        /// </param>
        public Length(string value, IUnitProvider unitProvider = null)
        {
            this.value = Parse(value, unitProvider ?? UnitProvider.Default).value;
			registeredSymbols = new List<string>() { "m","dm","cm","mm","km","yd","ft","in","","mi","nmi","Å","AU","ly"};
        }

        /// <summary>
        /// Gets the "m" unit.
        /// </summary>
		[Unit("m", true)]
		        public static Length Metre
        {
            get { return MetreField; }
        }

        /// <summary>
        /// Gets the "dm" unit.
        /// </summary>
		[Unit("dm")]
		        public static Length Decimetre
        {
            get { return DecimetreField; }
        }

        /// <summary>
        /// Gets the "cm" unit.
        /// </summary>
		[Unit("cm")]
		        public static Length Centimetre
        {
            get { return CentimetreField; }
        }

        /// <summary>
        /// Gets the "mm" unit.
        /// </summary>
		[Unit("mm")]
		        public static Length Millimetre
        {
            get { return MillimetreField; }
        }

        /// <summary>
        /// Gets the "km" unit.
        /// </summary>
		[Unit("km")]
		        public static Length Kilometre
        {
            get { return KilometreField; }
        }

        /// <summary>
        /// Gets the "yd" unit.
        /// </summary>
		[Unit("yd")]
		        public static Length Yard
        {
            get { return YardField; }
        }

        /// <summary>
        /// Gets the "ft" unit.
        /// </summary>
		[Unit("ft")]
		        public static Length Foot
        {
            get { return FootField; }
        }

        /// <summary>
        /// Gets the "in" unit.
        /// </summary>
		[Unit("in")]
		        public static Length Inch
        {
            get { return InchField; }
        }

        /// <summary>
        /// Gets the "" unit.
        /// </summary>
		[Unit("")]
		        public static Length HundredthInch
        {
            get { return HundredthInchField; }
        }

        /// <summary>
        /// Gets the "mi" unit.
        /// </summary>
		[Unit("mi")]
		        public static Length Mile
        {
            get { return MileField; }
        }

        /// <summary>
        /// Gets the "nmi" unit.
        /// </summary>
		[Unit("nmi")]
		        public static Length NauticalMile
        {
            get { return NauticalMileField; }
        }

        /// <summary>
        /// Gets the "Å" unit.
        /// </summary>
		[Unit("Å")]
		        public static Length Ångström
        {
            get { return ÅngströmField; }
        }

        /// <summary>
        /// Gets the "AU" unit.
        /// </summary>
		[Unit("AU")]
		        public static Length AstronomicalUnit
        {
            get { return AstronomicalUnitField; }
        }

        /// <summary>
        /// Gets the "ly" unit.
        /// </summary>
		[Unit("ly")]
		        public static Length LightYear
        {
            get { return LightYearField; }
        }

        /// <summary>
        /// Gets or sets the length as a string.
        /// </summary>
        /// <value>The string.</value>
        /// <remarks>
        /// This property is used for XML serialization.
        /// </remarks>
        //[XmlText]
        [DataMember]
		//[BsonSerializer(typeof(UnitSerializer))]
		//[BsonSerializer(typeof(LengthSerializer))]
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
        /// Gets or sets the value of the length in the base unit.
        /// </summary>
        public override double Value
        {
            get{
                return this.value;
            }

			set{
				throw new NotSupportedException("Length Doesn't support base value changing");
			}

        }

		 /// <summary>
        /// Gets if length is variable or not
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
        /// A <see cref="Length"/> that represents the quantity in <paramref name="input" />. 
        /// </returns>
        public static Length Parse(string input, IFormatProvider provider, IUnitProvider unitProvider)
        {
            if (unitProvider == null)
            {
                unitProvider = provider as IUnitProvider ?? UnitProvider.Default;
            }

            Length value;
            if (!unitProvider.TryParse(input, provider, out value))
            {
				return new  Length(0);
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
        /// A <see cref="Length"/> that represents the quantity in <paramref name="input" />. 
        /// </returns>
        public static Length Parse(string input, IFormatProvider provider = null)
        {
            var unitProvider = provider as IUnitProvider ?? UnitProvider.Default;

            Length value;
            if (!unitProvider.TryParse(input, provider, out value))
            {
				return new  Length(0);
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
        /// A <see cref="Length"/> that represents the quantity in <paramref name="input" />. 
        /// </returns>
        public static Length Parse(string input, IUnitProvider unitProvider)
        {
            if (unitProvider == null)
            {
                unitProvider = UnitProvider.Default;
            }

            Length value;
            if (!unitProvider.TryParse(input, unitProvider.Culture, out value))
            {
				return new  Length(0);
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
        public static bool TryParse(string input, IFormatProvider provider, IUnitProvider unitProvider, out Length result)
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
        /// The <see cref="Length"/> .
        /// </returns>
        public static Length ParseJson(string input)
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
        public static Length operator +(Length x, Length y)
        {
            double? value = 0;
            value = x?.Value ?? value;
            value = value + (y?.Value ?? 0);
           
            return new Length(value ?? 0);
            //return new Length(x.value + y.value);
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
        public static Length operator /(Length x, double y)
        {
            return new Length(x.value / y);
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
        public static double operator /(Length x, Length y)
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
        public static bool operator ==(Length x, Length y)
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
        public static bool operator >(Length x, Length y)
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
        public static bool operator >=(Length x, Length y)
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
        public static bool operator !=(Length x, Length y)
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
        public static bool operator <(Length x, Length y)
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
        public static bool operator <=(Length x, Length y)
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
        public static Length operator *(double x, Length y)
        {
            return new Length(x * y.value);
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
        public static Length operator *(Length x, double y)
        {
            return new Length(x.value * y);
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
        public static Length operator -(Length x, Length y)
        {
            double? value = 0;
            value = x?.Value ?? value;
            value = value - (y?.Value ?? 0);

            return new Length(value ?? 0);
            //return new Length(x.value - y.value);
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
        public static Length operator +(Length x)
        {
            return new Length(x.value);
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
        public static Length operator -(Length x)
        {
            return new Length(-x.value);
        }

        /// <summary>
        /// Compares this instance to the specified <see cref="Length"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="Length"/> . 
        /// </param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and the other value. 
        /// </returns>
        public override int CompareTo(Length other)
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
            return this.CompareTo((Length)obj);
        }

        /// <summary>
        /// Converts the quantity to the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>The amount of the specified unit.</returns>
		public override double ConvertTo(IUnit unit)
        {
            return this.ConvertTo((Length)unit);
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
        public double ConvertTo(Length unit)
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
            if (obj is Length)
            {
                return this.Equals((Length)obj);
            }

            return false;
        }

        /// <summary>
        /// Determines if the specified <see cref="Length"/> is equal to this instance.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="Length"/> . 
        /// </param>
        /// <returns>
        /// True if the values are equal. 
        /// </returns>
        public override bool Equals(Length other)
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
            if (!(x is Length))
            {
                throw new InvalidOperationException("Can only add quantities of the same types.");
            }

            return new Length(this.value + x.Value);
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
	public class LengthSerializer:SerializerBase<Length>{
		public override Length Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var up = UnitProvider.Default;
            IUnit result;
            if(up.TryGetUnit(typeof(Length), context.Reader.ReadString(), out result))
                return (Length)result;

            return base.Deserialize(context, args);
        } 
	}
#endif
	public enum LengthUnit{
		Metre,
		Decimetre,
		Centimetre,
		Millimetre,
		Kilometre,
		Yard,
		Foot,
		Inch,
		HundredthInch,
		Mile,
		NauticalMile,
		Ångström,
		AstronomicalUnit,
		LightYear
	}

}
