using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using System.Runtime;

namespace CFS.SkyNet.Common
{
    /// <summary>
    /// Helper for creating string enums.
    /// Code from here: http://code.google.com/p/stringenums4dotnet/
    /// License: Apache License 2.0: http://www.apache.org/licenses/LICENSE-2.0
    /// </summary>
    /// <code>
    /// public class MyStringEnum : StringEnumBase
    /// {
    ///    private MyStringEnum(string value) : base(value){}
    /// 
    ///    public static readonly MyStringEnum IsNeeded = new MyStringEnum("needed");
    ///    public static readonly MyStringEnum NotNeeded = new MyStringEnum("notneeded");
    /// }
    /// 
    /// Usage...
    /// if (myenumvar == MyStringEnum.IsNeeded) ....
    /// or
    /// Console.WriteLine(myenumvar); //produces "needed"
    /// </code>
    public abstract class StringEnumBase : IComparable<StringEnumBase>
    {
        #region Const name to value mapping
        private sealed class NameValueData
        {
            public string FieldName { get; set; }
            public string StringValue { get; set; }
            public StringEnumBase Value { get; set; }
        }

        private static readonly IDictionary<Type, IEnumerable<NameValueData>> TypeInfo = new Dictionary<Type, IEnumerable<NameValueData>>();
        private static readonly object LockObject = new object();
        private static IEnumerable<NameValueData> GetFieldDataForType(Type enumType)
        {
            if (TypeInfo.ContainsKey(enumType))
            {
                return TypeInfo[enumType];
            }
            lock (LockObject)
            {
                if (TypeInfo.ContainsKey(enumType))
                {
                    return TypeInfo[enumType];
                }

                var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
                var fieldData = fields.Select(f => new NameValueData
                {
                    FieldName = f.Name,
                    StringValue = f.GetValue(null).ToString(),
                    Value = (StringEnumBase) f.GetValue(null)
                }).ToList();
                TypeInfo[enumType] = fieldData;
                return fieldData;
            }
        }
        #endregion

        private readonly string _value;
        private readonly Lazy<string> _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringEnumBase"/> class.
        /// </summary>
        /// <param name="value">The name.</param>
        protected StringEnumBase(string value) : this()
        {
            _value = value;
        }

        private StringEnumBase()
        {
            _name = new Lazy<string>(() =>
            {
                var allEnumMembers = GetFieldDataForType(this.GetType());
                return allEnumMembers.First(f => ReferenceEquals(f.Value, this)).FieldName;
            });
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public sealed override string ToString()
        {
            return _value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public sealed override bool Equals(object obj)
        {
            if (obj is string)
                return _value.Equals(obj);
            if (obj is StringEnumBase)
                return _value == obj.ToString();
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public sealed override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(StringEnumBase first, string second)
        {
            if ((object)first == null)
            {
                return second == null;
            }
            return (second == first.ToString());
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(StringEnumBase first, string second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Gets the names for a particular constant base type by reflection.
        /// </summary>
        /// <param name="constantType">Type of the constant.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetNames(Type constantType)
        {
            if (constantType == null)
            {
                throw new ArgumentNullException("constantType");
            }

            //HACK - not equivalent
            if (!constantType.IsAssignableFrom(typeof(StringEnumBase)))
            //if (constantType.BaseType != typeof (StringEnumBase))
            {
                throw new ArgumentException("Must inherit from type StringConstantBase.", "constantType");
            }

            return GetFieldDataForType(constantType).Select(f => f.FieldName);
        }

        /// <summary>
        /// Gets a string enum by string value
        /// </summary>
        /// <typeparam name="T">Type of the string enum</typeparam>
        /// <param name="stringValue">The string value to lookup</param>
        /// <returns>The string enum</returns>
        /// <exception cref="ArgumentNullException">Thrown if the string parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown if the string value is not found</exception>
        public static T GetByValue<T>(string stringValue) where T : StringEnumBase
        {
            if (stringValue == null)
            {
                throw new ArgumentNullException("stringValue");
            }

            var possibleValues = GetFieldDataForType(typeof(T));
            var enumData = possibleValues.FirstOrDefault(e => e.StringValue == stringValue);
            if (enumData == null)
            {
                throw new ArgumentException("Value not found: " + stringValue, "stringValue");
            }
            return enumData.Value as T;
        }

        /// <summary>
        /// Gets a string enum by string value and enum type
        /// </summary>
        /// <param name="enumType">Type of the string enum</param>
        /// <param name="name">The string name of the type of the enum</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case.</param>
        /// <returns>The string enum</returns>
        public static object Parse(Type enumType, string name, bool ignoreCase = false)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!(typeof (StringEnumBase).IsAssignableFrom(enumType)))
            {
                throw new ArgumentException("Should inherit from StringEnumBase.", "enumType");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Should not be empty string.", "name");
            }

            var found = GetFieldDataForType(enumType).FirstOrDefault(t => Compare(t.FieldName, name, ignoreCase));

            if (found == null)
            {
                throw new ArgumentException("Requested name '" + name + "' was not found.");
            }

            return found.Value;
        }

        private static bool Compare(string fieldName, string name, bool ignoreCase)
        {
            var comparer = ignoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.CurrentCulture;
            return string.Compare(fieldName, name, comparer) == 0;
        }

        /// <summary>
        /// Attempts to parse a StringEnumBase, returning the result and a boolean indicating success
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum being parsed to</typeparam>
        /// <param name="value">The string value of the enum to parse</param>
        /// <param name="result">The result of the parse</param>
        /// <returns>Indication of successful parse</returns>
        public static Boolean TryParse<TEnum>(string value, out TEnum result) where TEnum : StringEnumBase
        {
            try
            {
                result = (TEnum)Parse(typeof (TEnum), value);
            }
            catch (Exception)
            {
                result = default(TEnum);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets all defined values for a string enum
        /// </summary>
        /// <typeparam name="T">The string enum</typeparam>
        /// <returns>The string values</returns>
        public static IEnumerable<string> GetAllValues<T>() where T : StringEnumBase
        {
            return GetFieldDataForType(typeof (T)).Select(f => f.StringValue);
        }
        
        /// <summary>
        /// Gets the name of the enum as a string
        /// </summary>
        /// <returns>The string name of the enum</returns>
        public string GetName()
        {
            return _name.Value;
        }

        public static implicit operator string(StringEnumBase value)
        {
            return value == null ? null : value.ToString();
        }

        /// <summary>
        /// Returns all defined members for type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAllDefinedMembers<T>()
            where T : StringEnumBase
        {
            return GetFieldDataForType(typeof(T))
                .Select(f => f.Value as T);
        }

        #region IComparable<StringStringEnumBase> Members

        public int CompareTo(StringEnumBase other)
        {
            return String.Compare(_value, other.ToString(), StringComparison.Ordinal);
        }

        #endregion

    }
}

