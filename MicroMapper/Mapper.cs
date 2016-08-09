using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroMapper
{
    public class Mapper<TSource, TDestination>
    {
        private readonly TDestination _destination;
        private readonly MapperOptions _options;
        private readonly TSource _source;

        public Mapper(TSource source, TDestination destination)
        {
            _source = source;
            _destination = destination;
            _options = new MapperOptions();
        }

        public void Execute()
        {
            // get a list of properties from the source object
            var sourceProperties = GetSourceProperties();

            // get a list of properties from the destination object
            var destinationProperties = GetDestinationProperties();

            // filter out any destination properties that should be ignored
            destinationProperties = destinationProperties.Where(p => !_options.IgnoredProperties.Contains(p.Name)).ToArray();

            // for each property of destination that exists on source set the value on destination
            foreach (var property in destinationProperties)
            {
                // if we have already evaluated the destination value from a call to MapProperty then use that
                if (_options.PropertyMaps.ContainsKey(property.Name))
                {
                    property.SetValue(_destination, _options.PropertyMaps[property.Name]);
                }
                else if (sourceProperties.Any(prop => prop.Name.Equals(property.Name)))
                {
                    property.SetValue(_destination, sourceProperties.First(prop => prop.Name.Equals(property.Name)).GetValue(_source));
                }
            }
        }

        public Mapper<TSource, TDestination> Ignore<TPropertyType>(Expression<Func<TDestination, TPropertyType>> destinationProperty)
        {
            var destinationPropertyName = GetPropertyName(destinationProperty);
            _options.IgnoredProperties.Add(destinationPropertyName);
            return this;
        }

        public Mapper<TSource, TDestination> MapProperty<TPropertyType>(Expression<Func<TDestination, TPropertyType>> destinationProperty, Expression<Func<TSource, TPropertyType>> valueExpression)
        {
            // we need to capture the destination property that this is for and the expression to run to get the value
            var destinationPropertyName = GetPropertyName(destinationProperty);

            // we need to the expression and store the result with the destination property name to set in the Execute()
            var compiledValueExpression = valueExpression.Compile();
            var destinationValue = compiledValueExpression.Invoke(_source);

            _options.PropertyMaps.Add(destinationPropertyName, destinationValue);

            return this;
        }

        public Mapper<TSource, TDestination> ReadOnlyPublicPropertiesFromSource()
        {
            _options.ReadOnlyPublicPropertiesFromSource = true;
            return this;
        }

        public Mapper<TSource, TDestination> WriteOnlyPublicPropertiesOnDestination()
        {
            _options.WriteOnlyPublicPropertiesOnDestination = true;
            return this;
        }

        private PropertyInfo[] GetDestinationProperties()
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (!_options.WriteOnlyPublicPropertiesOnDestination)
            {
                bindingFlags = bindingFlags | BindingFlags.NonPublic;
            }

            return _destination.GetType().GetTypeInfo().GetProperties(bindingFlags);
        }

        private string GetPropertyName<TPropertyType>(Expression<Func<TDestination, TPropertyType>> propertyLambda)
        {
            var type = typeof(TDestination);

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");

            return propInfo.Name;
        }

        private PropertyInfo[] GetSourceProperties()
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (!_options.ReadOnlyPublicPropertiesFromSource)
            {
                bindingFlags = bindingFlags | BindingFlags.NonPublic;
            }

            return _source.GetType().GetTypeInfo().GetProperties(bindingFlags);
        }

        private class MapperOptions
        {
            internal List<string> IgnoredProperties { get; set; }
            internal Dictionary<string, object> PropertyMaps { get; set; }
            internal bool ReadOnlyPublicPropertiesFromSource { get; set; }
            internal bool WriteOnlyPublicPropertiesOnDestination { get; set; }

            public MapperOptions()
            {
                PropertyMaps = new Dictionary<string, object>();
                IgnoredProperties = new List<string>();
            }
        }
    }
}