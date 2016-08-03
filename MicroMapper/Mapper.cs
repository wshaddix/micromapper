using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroMapper
{
    public class Mapper<TSource, TDestination>
    {
        private TDestination _destination;
        private MapperOptions _options;
        private TSource _source;

        public Mapper(TSource source, TDestination destination)
        {
            _source = source;
            _destination = destination;
            _options = new MapperOptions();
        }

        public void Execute()
        {
            // get a list of properties from the source object
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (_options.ReadNonPublicPropertiesOnSource)
            {
                bindingFlags = bindingFlags | BindingFlags.NonPublic;
            }

            var sourceProperties = _source.GetType().GetTypeInfo().GetProperties(bindingFlags);

            // get a list of properties from the destination object
            var destinationProperties = _destination.GetType().GetTypeInfo().GetProperties();

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

        public Mapper<TSource, TDestination> ReadNonPublicPropertiesOnSource()
        {
            _options.ReadNonPublicPropertiesOnSource = true;
            return this;
        }

        private string GetPropertyName<TPropertyType>(Expression<Func<TDestination, TPropertyType>> propertyLambda)
        {
            Type type = typeof(TDestination);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo.Name;
        }

        private class MapperOptions
        {
            internal Dictionary<string, object> PropertyMaps { get; set; }

            internal bool ReadNonPublicPropertiesOnSource { get; set; }

            public MapperOptions()
            {
                PropertyMaps = new Dictionary<string, object>();
            }
        }
    }
}