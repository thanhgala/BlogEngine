using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

namespace FrameworkCore.Infrastructure.Common
{
    public static class Reflects
    {
        #region Public Methods and Operators

        /// <summary>
        /// Execute a generic method by method name, parameter types and  method generic argument types in instance.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="paramTypes">
        /// The method parameter types.
        /// </param>
        /// <param name="genericArgumentTypes">
        /// The method generic argument types.
        /// </param>
        /// <param name="methodParams">
        /// The method parameters for invoking
        /// </param>
        /// <returns>
        /// The invoking result.
        /// </returns>
        public static object ExecuteGenericMethod(
            this object instance,
            string methodName,
            Type[] paramTypes,
            Type[] genericArgumentTypes,
            params object[] methodParams)
        {
            MethodInfo methodInfo = GetGenericMethod(instance.GetType(), methodName, paramTypes, genericArgumentTypes);
            return methodInfo.Invoke(instance, methodParams);
        }

        /// <summary>
        /// Searches and returns attributes. The inheritance chain is not used to find the attributes.
        /// </summary>
        /// <typeparam name="T">
        /// The type of attribute to search for.
        /// </typeparam>
        /// <param name="type">
        /// The type which is searched for the attributes.
        /// </param>
        /// <returns>
        /// Returns all attributes.
        /// </returns>
        public static T[] GetCustomAttributes<T>(this Type type) where T : System.Attribute
        {
            return GetCustomAttributes(type, typeof(T), false).Select(arg => (T)arg).ToArray();
        }

        /// <summary>
        /// Searches and returns attributes.
        /// </summary>
        /// <typeparam name="T">
        /// The type of attribute to search for.
        /// </typeparam>
        /// <param name="type">
        /// The type which is searched for the attributes.
        /// </param>
        /// <param name="inherit">
        /// Specifies whether to search this member's inheritance chain to find the attributes. Interfaces will be searched, too.
        /// </param>
        /// <returns>
        /// Returns all attributes.
        /// </returns>
        public static T[] GetCustomAttributes<T>(this Type type, bool inherit) where T : System.Attribute
        {
            return GetCustomAttributes(type, typeof(T), inherit).Select(arg => (T)arg).ToArray();
        }

        /// <summary>
        /// Get all properties with all its parents
        /// </summary>
        public static PropertyInfo GetPropertyWithInherit(this Type type, string propertyName)
        {
            var typeList = new List<Type> { type };

            if (type.IsInterface)
            {
                typeList.AddRange(type.GetInterfaces());
            }

            return
                typeList.SelectMany(interfaceType => interfaceType.GetProperties()).FirstOrDefault(property => property.Name == propertyName);
        }

        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            var propertyInfos = new List<PropertyInfo>();
            if (type.IsInterface)
            {
                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface))
                        {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }
            else
            {
                propertyInfos.AddRange(type.GetProperties(BindingFlags.FlattenHierarchy
                                                          | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                var baseType = type.BaseType;
                while (baseType != null)
                {
                    propertyInfos.AddRange(baseType.GetProperties(BindingFlags.FlattenHierarchy
                                                                  | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                    baseType = baseType.BaseType;
                }

            }

            return propertyInfos.ToArray();
        }

        /// <summary>
        /// Get all properties with all its parents
        /// </summary>
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
            var typeList = new List<Type> { type };

            if (type.IsInterface)
            {
                typeList.AddRange(type.GetInterfaces());
            }

            return typeList.SelectMany(interfaceType => interfaceType.GetProperties()).ToArray();
        }

        /// <summary>
        /// Get generic method from passed information
        /// </summary>
        public static MethodInfo GetGenericMethod(
            Type type, string methodName, Type[] paramTypes, Type[] genericArgumentTypes)
        {
            MethodInfo methodInfo =
                type.GetMethods().First(
                    m =>
                    m.IsGenericMethod && m.Name == methodName
                    && m.GetGenericArguments().Length == genericArgumentTypes.Length && CompareParamType(m, paramTypes));

            return methodInfo.MakeGenericMethod(genericArgumentTypes);
        }

        /// <summary>
        ///     Creates an instance of the specified obj.
        /// </summary>
        /// <param name="type">The obj of object to create.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        ///     Supply obj name and it will create the object for you.
        /// </summary>
        /// <param name="typeName">name of the obj it will try to create</param>
        /// <param name="args">args to the ctor of the object</param>
        /// <returns>A new instance of the specified obj.</returns>
        public static object CreateInstanceOfType(string typeName)
        {
            Type type; // Type to be created.

            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }
            if (typeName.Length == 0)
            {
                throw new ArgumentException("typeName should not be empty.", "typeName");
            }

            new ReflectionPermission(PermissionState.Unrestricted).Assert();

            type = Type.GetType(typeName);
            if (type != null)
                return Activator.CreateInstance(type);

            type = FindType(typeName);
            if (type == null)
            {
                throw new Exception("FindType failed to find obj " + typeName);
            }

            // we let it throw exception when error occurs.
            // ============================================
            return Activator.CreateInstance(type, null);
        }

        /// <summary>
        ///     Supply obj name and it will create the object for you.
        /// </summary>
        /// <param name="typeName">name of the obj it will try to create</param>
        /// <param name="args">args to the ctor of the object</param>
        /// <returns>A new instance of the specified obj.</returns>
        public static object CreateInstanceOfType(string typeName, object[] args)
        {
            Type type; // Type to be created.

            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }
            if (typeName.Length == 0)
            {
                throw new ArgumentException("typeName should not be empty.", "typeName");
            }

            new ReflectionPermission(PermissionState.Unrestricted).Assert();

            type = Type.GetType(typeName);
            if (type != null)
                return Activator.CreateInstance(type, args, null);

            type = FindType(typeName);
            if (type == null)
            {
                throw new Exception("FindType failed to find obj " + typeName);
            }

            // we let it throw exception when error occurs.
            // ============================================
            return Activator.CreateInstance(type, args, null);
        }
        /// <summary>
        ///     Retrieves a obj given its name.
        /// </summary>
        /// <param name='typeName'>
        ///     Simple name, name with namespace, or obj name with
        ///     partial or fully qualified assembly to look for.
        /// </param>
        /// <returns>The specified Type.</returns>
        /// <remarks>
        ///     If an assembly is specified, then this method is equivalent
        ///     to calling Type.GetType(). Otherwise, all loaded assemblies
        ///     will be used to look for the obj (unlike Type.GetType(),
        ///     which would only look in the calling assembly and in mscorlib).
        /// </remarks>
        public static Type FindType(string typeName)
        {
            Type returnValue = null;
            string preferedType = "System.Windows";
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }

            // A comma indicates a obj with an assembly reference
            if (typeName.IndexOf(',') != -1)
            {
                return Type.GetType(typeName, true);
            }

            // A period indicates a namespace is present.
            bool hasNamespace = typeName.IndexOf('.') != -1;

            new ReflectionPermission(PermissionState.Unrestricted).Assert();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (hasNamespace)
                {
                    Type t = assembly.GetType(typeName);
                    if (t != null)
                    {
                        return t;
                    }
                }
                else
                {
                    // For obj names that are not fully qualified
                    // and don't even have namespaces, explicitly
                    // opt out of WinForms (avoids collision with WPF
                    // the more common case).
                    if (assembly.FullName.Contains("Forms"))
                    {
                        continue;
                    }
                    Type[] testTypes = SafeGetTypes(assembly);
                    foreach (Type type in testTypes)
                    {
                        if (type != null && type.Name == typeName)
                        {
                            if (type.FullName.Contains(preferedType))
                            {
                                return type;
                            }
                            if (returnValue == null)
                            {
                                returnValue = type;
                            }
                        }
                    }
                }
            }
            if (returnValue != null)
            {
                return returnValue;
            }
            throw new InvalidOperationException("Unable to find obj [" + typeName + "] in loaded assemblies");
        }

        /// <summary>
        ///     Retrieves types, guaranteeing that no exceptions will be thown.
        /// </summary>
        /// <param name="assembly">Assembly to get types from.</param>
        /// <returns>
        ///     The types in the specifies assembly, or a zero-length array if an
        ///     exception was thrown and no types could be accessed. If only some
        ///     types are loaded, these are returned, and the returned array
        ///     will have null elements.
        /// </returns>
        public static Type[] SafeGetTypes(Assembly assembly)
        {
            // AssemblySW assemblySW = AssemblySW.Wrap(assembly);
            Type[] testTypes = Type.EmptyTypes;
            try
            {
                Type[] types = assembly.GetTypes();
                testTypes = new Type[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    testTypes[i] = types[i];
                }
            }
            catch (ReflectionTypeLoadException re)
            {
                var sb = new StringBuilder();
                sb.Append("ReflectionTypeLoadException thrown while getting types for ");
                sb.Append(assembly.FullName);
                sb.Append(Environment.NewLine);
                sb.Append(re);
                sb.Append("Loader exceptions: ");
                sb.Append(re.LoaderExceptions.Length);
                sb.Append(Environment.NewLine);
                foreach (Exception exception in re.LoaderExceptions)
                {
                    sb.Append(exception);
                    sb.Append(Environment.NewLine);
                }

                return re.Types;
            }
            catch (Exception exception)
            {
            }
            return testTypes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies a function to every element of the list.
        /// </summary>
        /// <typeparam name="T">
        /// The item type.
        /// </typeparam>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        /// <param name="function">
        /// The function.
        /// </param>
        private static void Apply<T>(this IEnumerable<T> enumerable, Action<T> function)
        {
            foreach (T item in enumerable)
            {
                function(item);
            }
        }

        /// <summary>
        /// The compare param type.
        /// </summary>
        /// <param name="methodInfo">
        /// The method info.
        /// </param>
        /// <param name="paramTypes">
        /// The param types.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        private static bool CompareParamType(MethodInfo methodInfo, Type[] paramTypes)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (parameterInfos.Length != paramTypes.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Private helper for searching attributes.
        /// </summary>
        /// <param name="type">
        /// The type which is searched for the attribute.
        /// </param>
        /// <param name="attributeType">
        /// The type of attribute to search for.
        /// </param>
        /// <param name="inherit">
        /// Specifies whether to search this member's inheritance chain to find the attribute. Interfaces will be searched, too.
        /// </param>
        /// <returns>
        /// An array that contains all the custom attributes, or an array with zero elements if no attributes are defined.
        /// </returns>
        private static IEnumerable<object> GetCustomAttributes(Type type, Type attributeType, bool inherit)
        {
            if (!inherit)
            {
                return type.GetCustomAttributes(attributeType, false);
            }

            var attributeCollection = new Collection<object>();
            Type baseType = type;

            do
            {
                baseType.GetCustomAttributes(attributeType, true).Apply(attributeCollection.Add);
                baseType = baseType.BaseType;
            }
            while (baseType != null);

            foreach (Type interfaceType in type.GetInterfaces())
            {
                GetCustomAttributes(interfaceType, attributeType, true).Apply(attributeCollection.Add);
            }

            var attributeArray = new object[attributeCollection.Count];
            attributeCollection.CopyTo(attributeArray, 0);
            return attributeArray;
        }

        #endregion

        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propInfo = null;
            do
            {
                propInfo = type.GetProperty(propertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (propInfo == null && type != null);
            return propInfo;
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            PropertyInfo propInfo = GetPropertyInfo(objType, propertyName);
            if (propInfo == null)
                throw new ArgumentOutOfRangeException(nameof(propertyName),
                    $"Couldn't find property {propertyName} in type {objType.FullName}");
            return propInfo.GetValue(obj, null);
        }
    }
}
