﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
#if CORE
using System.Reflection;
#endif
namespace ReactiveHelpers.ComponentModel
{
    public static class TypeConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, ITypeConverter> _typeConverters = new ConcurrentDictionary<Type, ITypeConverter>();

        static TypeConverterFactory()
        {
            CreateDefaultConverters();
        }

        private static void CreateDefaultConverters()
        {
            _typeConverters.TryAdd(typeof(DateTime), new DateTimeConverter());
            _typeConverters.TryAdd(typeof(TimeSpan), new TimeSpanConverter());
            _typeConverters.TryAdd(typeof(bool), new BooleanConverter(
                new[] { "yes", "y", "on", "wahr" },
                new[] { "no", "n", "off", "falsch" }));

          /*  ITypeConverter converter = new ShippingOptionConverter(true);
            _typeConverters.TryAdd(typeof(IList<ShippingOption>), converter);
            _typeConverters.TryAdd(typeof(List<ShippingOption>), converter);
            _typeConverters.TryAdd(typeof(ShippingOption), new ShippingOptionConverter(false));

            converter = new ProductBundleDataConverter(true);
            _typeConverters.TryAdd(typeof(IList<ProductBundleItemOrderData>), converter);
            _typeConverters.TryAdd(typeof(List<ProductBundleItemOrderData>), converter);
            _typeConverters.TryAdd(typeof(ProductBundleItemOrderData), new ProductBundleDataConverter(false));*/
        }

        public static void RegisterConverter<T>(ITypeConverter typeConverter)
        {
            RegisterConverter(typeof(T), typeConverter);
        }

        public static void RegisterConverter(Type type, ITypeConverter typeConverter)
        {
            Contract.Requires<ArgumentNullException>(type != null, nameof(type));
            Contract.Requires<ArgumentNullException>(typeConverter != null, nameof(typeConverter));

            _typeConverters.TryAdd(type, typeConverter);
        }

        public static ITypeConverter RemoveConverter<T>(ITypeConverter typeConverter)
        {
            return RemoveConverter(typeof(T));
        }

        public static ITypeConverter RemoveConverter(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null, nameof(type));

            ITypeConverter converter = null;
            _typeConverters.TryRemove(type, out converter);
            return converter;
        }

        public static ITypeConverter GetConverter<T>()
        {
            return GetConverter(typeof(T));
        }

        public static ITypeConverter GetConverter(object component)
        {
            Contract.Requires<ArgumentNullException>(component != null, nameof(component));

            return GetConverter(component.GetType());
        }

        public static ITypeConverter GetConverter(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null, nameof(type));

            ITypeConverter converter;
            if (_typeConverters.TryGetValue(type, out converter))
            {
                return converter;
            }
#if CORE
            var isGenericType = type.GetTypeInfo().IsGenericType;
            var isSubclass= type.GetTypeInfo().IsSubclassOf(typeof(IEnumerable<>));
#else
            var isGenericType = type.IsGenericType;
            var isSubclass= type.IsSubclassOf(typeof(IEnumerable<>));
#endif

            if (isGenericType)
            {
                var definition = type.GetGenericTypeDefinition();

                // Nullables
                if (definition == typeof(Nullable<>))
                {
                    converter = new NullableConverter(type);
                    RegisterConverter(type, converter);
                    return converter;
                }

                // Sequence types
                var genericArgs = type.GetGenericArguments();
                var isEnumerable = genericArgs.Length == 1 && isSubclass;
                if (isEnumerable)
                {
                    converter = (ITypeConverter)Activator.CreateInstance(typeof(EnumerableConverter<>).MakeGenericType(genericArgs[0]), type);
                    RegisterConverter(type, converter);
                    return converter;
                }
            }

            // default fallback
            converter = new TypeConverterAdapter(TypeDescriptor.GetConverter(type));
            RegisterConverter(type, converter);
            return converter;
        }
    }

}
