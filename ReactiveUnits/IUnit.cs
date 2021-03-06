﻿using System;
using System.Collections.Generic;
#if !CORE
using System.ComponentModel.DataAnnotations.Schema;
#endif
namespace Webcorp.unite
{


    public interface IUnit:IComparable,IFormattable
    {
        double Value { get; set; }

        double ConvertTo(IUnit unit);

        IUnit MultiplyBy(double x);

        IUnit Add(IUnit x);

        List<string> RegisteredSymbols { get; }

        bool VariableValue { get; }
    }

    public interface IUnit<T> : IUnit, IEquatable<T>, IComparable<T>
    {

    }

#pragma warning disable CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
#pragma warning disable CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
#if !CORE
    [Serializable]
    [ComplexType]
#endif

    public abstract class Unit<T> : IUnit<T>
#pragma warning restore CS0661 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.GetHashCode()
#pragma warning restore CS0660 // Le type définit l'opérateur == ou l'opérateur != mais ne se substitue pas à Object.Equals(object o)
    {
        public abstract List<string> RegisteredSymbols {get; }

        public abstract double Value { get; set; }

        public abstract bool VariableValue { get; }

        public abstract IUnit Add(IUnit x);

        public abstract int CompareTo(T other);

        public abstract int CompareTo(object obj);

        public abstract double ConvertTo(IUnit unit);

        public abstract bool Equals(T other);

        public abstract IUnit MultiplyBy(double x);

        public abstract string ToString(string format, IFormatProvider formatProvider);

        

        public static bool operator >(Unit<T> a1, Unit<T> a2)
        {
            return a1.CompareTo(a2)>0;
        }

        public static bool operator <(Unit<T> a1, Unit<T> a2)
        {
            return a1.CompareTo(a2) < 0;
        }

        public static bool operator ==(Unit<T> a1, Unit<T> a2)
        {
            return a1.CompareTo(a2) == 0;
        }

        public static bool operator !=(Unit<T> a1, Unit<T> a2)
        {
            return a1.CompareTo(a2) != 0;
        }
    }
}
