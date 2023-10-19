//------------------------------------------------------------------------------
// EAssetsExtension.cs
//
// Copyright 2020
//
// Created by Christian Koch
// 
//------------------------------------------------------------------------------

//#define UNITY_EXTENSIONS //Uncomment to use extensions of unity classes
//#define DEBUGGING //Uncomment to allow the Debug class to be called in the extensions
//#define THROWS_EXCEPTIONS //Uncomment to let the extensions throw exceptions (this can be unwanted in games due to performance)

#if DEBUGGING && UNITY_EXTENSIONS
using UnityEngine;
#endif

namespace Extensions
{
    #region Unity extensions
#if UNITY_EXTENSIONS
    /// <summary>
    /// This namespace contains extensions for classes located in the <see cref="UnityEngine"/> namespace (e.g. <see cref="UnityEngine.GameObject"/> or <see cref="UnityEngine.Vector3"/>)
    /// </summary>
    namespace UnityClassExtension
    {
        using UnityEngine;
        using System.Reflection;
        using Extensions.ClassExtensions;

        /// <summary>
        /// This class contains extensions for unity classes
        /// </summary>
        public static class UnityClassExtensions
        {
    #region MonoBehaviour
            public static string[] GetMembers(this MonoBehaviour monoBehaviour, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                return monoBehaviour.GetAllWritableMembers(flags);
            }
    #endregion

    #region GameObject
            /// <summary>
            /// Gets a component if attached, else attaches it and returns the reference
            /// </summary>
            /// <typeparam name="T">The component to get or add</typeparam>
            /// <param name="go">The gameobject you want to get a component from</param>
            /// <returns></returns>
            public static T AddAndGetComponent<T>(this GameObject go) where T : Component
            {
                return go.GetComponent<T>() ?? go.AddComponent<T>();

            }

            /// <summary>
            /// Returns the position of this GameObject
            /// </summary>
            /// <param name="go">The gameobject you want to use</param>
            /// <returns></returns>
            public static Vector3 GetPosition(this GameObject go)
            {
                return go.transform.position;
            }

            /// <summary>
            /// Returns the distance to a given Gameobject
            /// </summary>
            /// <param name="go">This gameobject</param>
            /// <param name="target">The gameobject you want to know the distance to</param>
            /// <returns></returns>
            public static float GetDistanceTo(this GameObject go, GameObject target)
            {
                return (target.GetPosition() - go.GetPosition()).magnitude;
            } 
    #endregion

    #region Vector3
            /// <summary>
            /// Set this vector to the zero vector
            /// </summary>
            /// <param name="vec">The vector you want to set</param>
            public static void SetToZero(this Vector3 vec)
            {
                vec = Vector3.zero;
            }

            /// <summary>
            /// Returns the sum of the 3 vector components
            /// </summary>
            /// <param name="vec">The vector you want to use</param>
            /// <returns></returns>
            public static float Sum(this Vector3 vec)
            {
                return vec.x + vec.y + vec.z;
            }
    #endregion
        }
    }
#endif
    #endregion

    #region Non-Unity extensions

    /// <summary>
    /// This namespace contains extensions for the <see cref="object"/> class regarding classes and structs
    /// </summary>
    namespace ClassExtensions
    {
        using System;
        using System.Reflection;
        using System.Collections.Generic;

        /// <summary>
        /// This class contains extensions for classes
        /// <para>Most of the content of this class are shorthand writings for field and property checking on a class/struct</para>
        /// </summary>
        public static class ClassExtensions
        {
            #region object

            #region Field/Property Getter
            /// <summary>
            /// Returns a property of this object
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="propname">The name of the property you want returned</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will return null if the object is not a class or struct
            /// object value = o.GetPropertyValue("propName");
            /// 
            /// //value does now contain the value of the property or null
            /// 
            /// //do stuff with value here...
            /// </code>
            /// </example>
            public static object GetPropertyValue(this object obj, string propname, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetPropertyValue");
#else
                    return null;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a property
                    return default;

                if (propname != string.Empty)
                {
                    PropertyInfo info = obj.GetType().GetProperty(propname, flags);
                    //check if it is no indexer
                    if (info.GetIndexParameters() != null && info.GetIndexParameters().Length == 0)
                    {
                        return info.GetValue(obj, null);
                    }
                    else
                    {
                        return default;
                    }
                }
                else
                    return default;
            }

            /// <summary>
            /// Get a property of this object using its getter method. Returns null if it does not exist
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="propname">The name of the property</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will not do anything if the object is not a class or struct
            /// object value = o.GetPropertyValueUsingGetter("propName");
            /// 
            /// //Do something with the value here
            /// </code>
            /// </example>
            public static object GetPropertyValueUsingGetter(this object obj, string propname, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for SetPropertyValueUsingSetter");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a property
                    return default;

                PropertyInfo prop = obj.GetType().GetProperty(propname, flags);

                if (prop != null && prop.CanRead)
                {
                    return prop.GetMethod.Invoke(obj, null);
                }

                return default;
            }

            /// <summary>
            /// Returns a field of this object
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="fieldname">The name of the field you want to set</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will return null if the object is not a class or struct
            /// object value = o.GetFieldValue("fieldName");
            /// 
            /// //value does now contain the value of the field or null
            /// 
            /// //do stuff with value here...
            /// </code>
            /// </example>
            public static object GetFieldValue(this object obj, string fieldname, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetFieldValue");
#else
                    return null;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a field
                    return default;

                if (obj != null)
                {
                    return obj.GetType().GetField(fieldname, flags)?.GetValue(obj);
                }
                return default;
            }

            /// <summary>
            /// Returns a field/property of this object. Returns null if it does not exist
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="name">The name of the field/property you want to set</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will return null if the object is not a class or struct
            /// object value = o.TryGetValue("fieldName");
            /// 
            /// //value does now contain the value of the field/property or null
            /// 
            /// //do stuff with value here...
            /// </code>
            /// </example>
            public static object TryGetValue(this object obj, string name, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for TryGetValue");
#else
                    return null;
#endif
                }

                if (obj.GetType().GetField(name, flags)?.GetValue(obj) != null)
                {
                    return obj.GetFieldValue(name, flags);
                }
                else if (obj.GetType().GetProperty(name, flags)?.GetValue(obj) != null)
                {
                    return obj.GetPropertyValue(name, flags);
                }
                else
                {
                    return null;
                }

            }
            #endregion

            #region Field/Property Setter
            /// <summary>
            /// Set a property of this object
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="propname">The name of the property</param>
            /// <param name="value">The value you want to set it to</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will not do anything if the object is not a class or struct
            /// o.SetPropertyValue("propName",propValue);
            /// </code>
            /// </example>
            public static void SetPropertyValue(this object obj, string propname, object value, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for SetPropertyValue");
#else
                    return;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a property
                    return;

                PropertyInfo prop = obj.GetType().GetProperty(propname, flags);

                if (prop != null && value != null && prop.CanWrite)
                {
                    prop.SetValue(obj, value, null);
                }

            }

            /// <summary>
            /// Set a property of this object using its setter method
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="propname">The name of the property</param>
            /// <param name="value">The value you want to set it to</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will not do anything if the object is not a class or struct
            /// o.SetPropertyValueUsingSetter("propName",propValue);
            /// </code>
            /// </example>
            public static void SetPropertyValueUsingSetter(this object obj, string propname, object value, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for SetPropertyValueUsingSetter");
#else
                    return;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a property
                    return;

                PropertyInfo prop = obj.GetType().GetProperty(propname, flags);

                if (prop != null && value != null && prop.CanWrite)
                {
                    prop.SetMethod.Invoke(obj, new object[] { value });
                }
            }

            /// <summary>
            /// Set a field of this object
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="fieldname">The name of the field you want to set</param>
            /// <param name="value">The value you want to set it to</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will not do anything if the object is not a class or struct
            /// o.SetFieldValue("fieldName",fieldValue);
            /// </code>
            /// </example>
            public static void SetFieldValue(this object obj, string fieldname, object value, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for SetFieldValue");
#else
                    return;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a field
                    return;

                FieldInfo fieldInfo = obj.GetType().GetField(fieldname, flags);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(obj, value);
                }
            }

            /// <summary>
            /// Tries to set a field/property of this object
            /// <para>If it is a property, its setter method will be used to do so</para>
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="name">The name of the field/property you want to set</param>
            /// <param name="value">The value you want to set it to</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will not do anything if the object is not a class or struct
            /// o.TrySetValue("name",value);
            /// </code>
            /// </example>
            public static void TrySetValue(this object obj, string name, object value, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for TrySetValue");
#else
                    return;
#endif
                }

                //Try setting a field and a property
                if (obj.GetType().GetField(name) != null)
                {
                    obj.SetFieldValue(name, value, flags);
                }
                else if (obj.GetType().GetProperty(name) != null)
                {
                    //Get the set method
                    obj.GetType().GetProperty(name)?.SetMethod.Invoke(obj, new object[] { value });
                }
            }
            #endregion

            #region Field/Property Checker
            /// <summary>
            /// Checks if this object has a field with the given name
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="fieldname">The name of the field</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will return false if the object is not a class or struct
            /// if(o.HasField("fieldName")
            /// {
            ///     //Do something here if the field exists
            /// }
            /// </code>
            /// </example>
            public static bool HasField(this object obj, string fieldname, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for HasField");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a field
                    return false;

                FieldInfo info = obj.GetType().GetField(fieldname, flags);

                return info != null;
            }
            /// <summary>
            /// Checks if this object has a property with the given name
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="propname">The name of the property</param>
            /// <param name="flags">You can set the flags appropriate to your needs</param>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will return false if the object is not a class or struct
            /// if(o.HasProperty("propName")
            /// {
            ///     //Do something here if the property exists
            /// }
            /// </code>
            /// </example>
            public static bool HasProperty(this object obj, string propname, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for HasProperty");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a field
                    return false;

                PropertyInfo info = obj.GetType().GetProperty(propname, flags);

                return info != null;
            }
            #endregion

            #region Method information
            /// <summary>
            /// Invokes a method of this object if possible. Returns null if not
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="methodname">The name of the method to invoke</param>
            /// <param name="values">The values to pass to the method</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This will return null if the object is not a class or struct
            /// //Only use returnValue if anything is returned
            /// object returnValue = o.Invoke("methodName",inputs); //inputs can be any array of objects
            /// //If the inputs dont match any method, invoke will throw an error
            /// </code>
            /// </example>
            public static object Invoke(this object obj, string methodname, object[] values = null, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for Invoke");
#else
                    return null;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a method
                    return default;

                Type type = obj.GetType(); //Get type

                MethodInfo info = type.GetMethod(methodname, flags);
                if (info != null)
                    return info.Invoke(obj, values);

                return default;
            }

            /// <summary>
            /// Returns the method names of a class
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain all names of the public, non-static methods of object o
            /// string[] methodNames = o.GetMethodNames(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the methodnames here...
            /// </code>
            /// </example>
            public static string[] GetMethodNames(this object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetMethodNames");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have methods
                    return null;

                if (obj == null)
                    return default;

                BindingFlags bindingFlags = flags;

                List<string> arr = new List<string>();
                MethodInfo[] prop = obj.GetType().GetMethods(bindingFlags);

                for (int i = 0; i < prop.Length; i++)
                {
                    arr.Add(prop[i].Name);
                }

                return arr.ToArray();
            }

            /// <summary>
            /// Returns the number of method parameters
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="methodName">The name of the method you want to search for</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain the number of inputs of all public, non-static methods of object o
            /// int parameterCount = o.GetMethodParameterCount(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the parameterCount here...
            /// </code>
            /// </example>
            public static int GetMethodParameterCount(this object obj, string methodName, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetMethodParameterCount");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have methods
                    return -1;

                BindingFlags bindingFlags = flags;

                MethodInfo prop = obj.GetType().GetMethod(methodName);

                if (prop != null)
                    return prop.GetParameters().Length;
                else
                    return -1;
            }

            /// <summary>
            /// Returns the number of method parameters
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain the number of inputs of all public, non-static methods of object o
            /// int[] parameterCount = o.GetMethodsParametersCount(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the parameterCount here...
            /// </code>
            /// </example>
            public static int[] GetMethodsParameterCount(this object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetMethodsParameterCount");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have methods
                    return null;

                //if (obj == null)
                //return new int[] { -1 };

                BindingFlags bindingFlags = flags;

                List<int> methodParameterCount = new List<int>();
                MethodInfo[] prop = obj.GetType().GetMethods(bindingFlags);

                for (int i = 0; i < prop.Length; i++)
                {
                    methodParameterCount.Add(prop[i].GetParameters().Length);
                }

                return methodParameterCount.ToArray();
            }

            /// <summary>
            /// Returns the names of the method parameters
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="method">The name of the method</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns>The parameter names if any, else null</returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain the number of inputs of all public, non-static methods of object o
            /// string[] parameterNames = o.GetMethodParameterNames("methodname"); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the parameterNames here...
            /// </code>
            /// </example>
            public static string[] GetMethodParameterNames(this object obj, string method, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetMethodParameterNames");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have methods
                    return default;

                BindingFlags bindingFlags = flags;

                MethodInfo prop = obj.GetType().GetMethod(method, bindingFlags);

                if (prop != null) //Check for null
                {
                    int paramCount = prop.GetParameters().Length;

                    if (paramCount > 0) //Check for parameters
                    {
                        List<string> methodParameterNames = new List<string>();
                        foreach (ParameterInfo pi in prop.GetParameters())
                        {
                            methodParameterNames.Add(pi.Name);
                        }
                        return methodParameterNames.ToArray();
                    }
                }

                return default;
            }

            /// <summary>
            /// Returns the names and types of the method parameters
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="method">The name of the method</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns>The parameter names if any and their types, else null</returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain the number of inputs of all public, non-static methods of object o
            /// Dictionary[string,Type] parameters = o.GetMethodParameters("methodname"); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the parameters here...
            /// </code>
            /// </example>
            public static Dictionary<string, Type> GetMethodParameters(this object obj, string method, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetMethodParameters");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have methods
                    return default;

                BindingFlags bindingFlags = flags;

                MethodInfo prop = obj.GetType().GetMethod(method, bindingFlags);

                if (prop != null) //Check for null
                {
                    int paramCount = prop.GetParameters().Length;

                    if (paramCount > 0) //Check for parameters
                    {
                        Dictionary<string, Type> methodParameterNames = new Dictionary<string, Type>();
                        foreach (ParameterInfo pi in prop.GetParameters())
                        {
                            methodParameterNames.Add(pi.Name, pi.ParameterType);
                        }
                        return methodParameterNames;
                    }
                }

                return default;
            }

            /// <summary>
            /// Returns the type of the given method parameter
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="method">The name of the method</param>
            /// <param name="parameter">The parameter you want to check</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns>The parameter names if any and their types, else null</returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain the number of inputs of all public, non-static methods of object o
            /// Type parameterType = o.GetParameterType("methodname","parameterType"); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the parametertype here...
            /// </code>
            /// </example>
            public static Type GetMethodParameterType(this object obj, string method, string parameter, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetMethodParameterType");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have methods
                    return default;

                BindingFlags bindingFlags = flags;

                MethodInfo prop = obj.GetType().GetMethod(method, bindingFlags);

                if (prop != null) //Check for null
                {
                    int paramCount = prop.GetParameters().Length;

                    if (paramCount > 0) //Check for parameters
                    {
                        foreach (ParameterInfo pi in prop.GetParameters())
                        {
                            if (pi.Name.Equals(parameter))
                                return pi.ParameterType;
                        }
                    }
                }

                return default;
            }
            #endregion

            #region Get Field/Property Names
            /// <summary>
            /// Returns the field names of a class
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain all names of the public, non-static fields of the object o
            /// string[] fieldnames = o.GetFieldNames(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the fieldnames here...
            /// </code>
            /// </example>
            public static string[] GetFieldNames(this object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetFieldNames");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have fields
                    return null;

                if (obj == null)
                    return default;

                BindingFlags bindingFlags = flags;

                FieldInfo[] fields = obj.GetType().GetFields(bindingFlags);

                List<string> arr = new List<string>();
                for (int i = 0; i < fields.Length; i++)
                {
                    arr.Add(fields[i].Name);
                }

                return arr.ToArray();
            }

            /// <summary>
            /// Returns the property names of a class
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain all names of the public, non-static properties of the object o
            /// string[] propertyNames = o.GetPropertyNames(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the propertynames here...
            /// </code>
            /// </example>
            public static string[] GetPropertyNames(this object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetPropertyNames");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have properties
                    return null;

                if (obj == null)
                    return default;

                List<string> arr = new List<string>();
                PropertyInfo[] prop = obj.GetType().GetProperties(flags);

                for (int i = 0; i < prop.Length; i++)
                {
                    arr.Add(prop[i].Name);
                }

                return arr.ToArray();
            }

            /// <summary>
            /// Returns all properties that are writable
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain all names of the public, non-static properties of the object o
            /// //This method filters out all properties that are read only (e.g. have private setters)
            /// string[] propertyNames = o.GetWritablePropertyNames(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the propertynames here...
            /// </code>
            /// </example>
            public static string[] GetWritablePropertyNames(this object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetWritablePropertyNames");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have writable properties
                    return null;

                string[] str = obj.GetPropertyNames(flags);

                List<string> strlist = new List<string>();
                foreach (string s in str)
                {
                    if (!obj.PropertyIsReadOnly(s))
                    {
                        strlist.Add(s);
                    }
                }

                return strlist.ToArray();
            }

            /// <summary>
            /// Returns all writable attributes
            /// <para>Which is all fields and all writable properties that are part of the bindingflags</para>
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //This array will now contain all names of the public, non-static properties and fields of the object o
            /// //This method filters out all properties that are read only (e.g. have private setters)
            /// string[] attributeNames = o.GetAllWritableMembers(); //Returns null if object is neither class nor struct
            /// 
            /// //Do something with the membernames here...
            /// </code>
            /// </example>
            public static string[] GetAllWritableMembers(this object obj, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for GetAllWritableMembers");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have attributes
                    return null;

                List<string> attributes = new List<string>(obj.GetWritablePropertyNames(flags));
                attributes.AddRange(obj.GetFieldNames(flags));

                return attributes.ToArray();
            }

            /// <summary>
            /// Returns true if the property is read only
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="propertyName">The name of the property to check</param>
            /// <param name="flags">The flags you want to use</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //Returns true if the property with the given name is read-only (e.g. only has private setter)
            /// if(o.GetMethodAttributeCount("propertyName")) //Returns false if object is neither class nor struct
            /// {
            ///     //Do something here...
            /// }
            /// </code>
            /// </example>
            public static bool PropertyIsReadOnly(this object obj, string propertyName, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for PropertyIsReadOnly");
#else
                    return default;
#endif
                }

                if (!obj.GetType().IsClass && !obj.GetType().IsValueType) //A non class/non struct wont have a property
                    return false;

                PropertyInfo info = obj.GetType().GetProperty(propertyName, flags);
                if (info != null && info.GetIndexParameters().Length == 0)
                {
                    //If there is no set method or it is privat it is read only
                    return info.SetMethod == null || !info.SetMethod.IsPublic;
                }
                else if (info != null && info.GetIndexParameters().Length != 0)
                {
                    return true; //Indexers are also read-only since they need an input value
                }
                else
                    return false;
            }

            /// <summary>
            /// Returns true if this object is a class
            /// </summary>
            /// <param name="obj">This object</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //Returns true if the object is a class
            /// if(o.IsClass()) 
            /// {
            ///     //Do something here...
            /// }
            /// </code>
            /// </example>
            public static bool IsClass(this object obj)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for IsClass");
#else
                    return default;
#endif
                }

                return obj.GetType().IsClass;
            }
            #endregion

            #region Primitive Checks
            /// <summary>
            /// Checks if this object is a primitive/simple type
            /// <para>Primitive/Simple types are: regular primitives, enums, strings, decimals</para>
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //Returns true if the object is a either an enum, a string, a decimal or any other simple/primitive type (like int, char and float)
            /// //This also returns true if the object is a generic class with a primitive type parameter
            /// if(o.IsPrimitive()) 
            /// {
            ///     //Do something here...
            /// }
            /// </code>
            /// </example>
            public static bool IsPrimitive(this object obj)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for IsPrimitive");
#else
                    return default;
#endif
                }

                TypeInfo info = obj.GetType().GetTypeInfo();

                return info.IsPrimitive
                    || info.IsEnum
                    || obj.GetType().Equals(typeof(string))
                    || obj.GetType().Equals(typeof(decimal));
            }

            /// <summary>
            /// Checks if this object is a simple type
            /// </summary>
            /// <param name="obj">This object</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Create an object/get an existing object
            /// object o = someValue;
            /// 
            /// //Returns true if the object is a either an enum, a string, a decimal or any other simple/primitive type (like int, char and float)
            /// //This also returns true if the object is a generic class with a primitive type parameter
            /// if(o.IsSimple()) 
            /// {
            ///     //Do something here...
            /// }
            /// </code>
            /// </example>
            public static bool IsSimple(this object obj)
            {
                if (obj == null)
                {
#if THROWS_EXCEPTIONS
                    throw new ArgumentException("Object can not be null for IsSimple");
#else
                    return default;
#endif
                }

                return IsPrimitive(obj);
            }
            #endregion

            #endregion
        }
    }

    /// <summary>
    /// This namespace contains extensions for the <see cref="System.Type"/> class.
    /// </summary>
    namespace TypeExtensions
    {
        using System;
        using System.Collections.Generic;

        /// <summary>
        /// This class extends the type class with inheritance checking
        /// </summary>
        public static class TypeExtensions
        {
            #region Type
            /// <summary>
            /// Get the base class of a given object
            /// <para>This goes up to the highest possible class, that is directly beneath object in the hierarchy</para>
            /// </summary>
            /// <param name="obj">This object</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Get the type of any object
            /// Type t = someValue.GetType();
            /// 
            /// //Returns the first (non object!) class in the inheritance hierarchy of a given type
            /// Type baseClass = t.GetFirstBaseclass();
            /// 
            /// //Do something with the baseclass here...
            /// </code>
            /// </example>
            public static Type GetFirstBaseclass(this Type obj)
            {
                Type b = obj.BaseType;
                Type tmp = b;
                while (tmp != typeof(object))
                {
                    b = tmp;
                    tmp = b.BaseType;
                }

                return b;
            }

            /// <summary>
            /// Returns all base classes of this object
            /// </summary>
            /// <param name="obj">This object</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Get the type of any object
            /// Type t = someValue.GetType();
            /// 
            /// //Returns all baseClasses of this type object. This array does not contain the object type
            /// Type[] baseClasses = t.GetBaseClasses();
            /// 
            /// //Do something with the baseclasses here...
            /// </code>
            /// </example>
            public static Type[] GetBaseClasses(this Type obj)
            {
                List<Type> b = new List<Type>();
                Type tmp = obj.BaseType;
                while (tmp != typeof(object))
                {
                    b.Add(tmp);
                    tmp = tmp.BaseType;
                }

                return b.ToArray();
            }

            /// <summary>
            /// Returns all base classes as a list
            /// </summary>
            /// <param name="obj">This object</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Get the type of any object
            /// Type t = someValue.GetType();
            /// 
            /// //Returns all baseClasses of this type object as a list. This list does not contain the object type
            /// List{Type} baseClasses = t.GetBaseClassesAsList();
            /// 
            /// //Do something with the baseclasses here...
            /// </code>
            /// </example>
            public static List<Type> GetBaseClassesAsList(this Type obj)
            {
                List<Type> b = new List<Type>();
                Type tmp = obj.BaseType;
                while (tmp != typeof(object))
                {
                    b.Add(tmp);
                    tmp = tmp.BaseType;
                }

                return b;
            }

            /// <summary>
            /// Returns true if some class this class inheritet from is of the given type
            /// <para>HasAncestor(typeof(object)) should always return false since it is ignored as BaseType!</para>
            /// </summary>
            /// <param name="obj">This object</param>
            /// <param name="type">The type you want to check for</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Get the type of any object
            /// Type t = someValue.GetType();
            /// 
            /// //Returns true if the given type is an ancestor of the type t (if t inherited from it somewhere) 
            /// if(t.HasAncestor(someType))
            /// {
            ///     //Do something here...
            /// }
            /// </code>
            /// </example>
            public static bool HasAncestor(this Type obj, Type type)
            {
                //If one of the baseclasses matches return true
                return obj.GetBaseClassesAsList().Contains(type);
            }

            /// <summary>
            /// Checks if this type implements the given interface
            /// <para>This should also work for classes</para>
            /// </summary>
            /// <typeparam name="T">The interface to check</typeparam>
            /// <param name="obj">This type</param>
            /// <returns></returns>
            /// <example>
            /// <code>
            /// //Get the type of any object
            /// Type t = someValue.GetType();
            /// 
            /// //Returns true if the given type implements the interface of the given type
            /// if(t.ImplementsInterface{interfaceType})
            /// {
            ///     //Do something here...
            /// }
            /// </code>
            /// </example>
            public static bool ImplementsInterface<T>(this Type obj) where T : class
            {
                return typeof(T).IsAssignableFrom(obj);
            }
            #endregion
        }
    }

    /// <summary>
    /// This namespace contains extensions for primitve types (like <see cref="int"/>, <see cref="float"/> or <see cref="string"/>)
    /// </summary>
    namespace PrimitiveExtensions
    {
        /// <summary>
        /// This class extends the <see cref="string"/> type
        /// </summary>
        public static class StringExtensions
        {
            #region
            /// <summary>
            /// Returns true if the string is null or empty
            /// </summary>
            /// <param name="str">This string</param>
            /// <returns></returns>
            public static bool IsNullOrEmpty(this string str)
            {
                return str == null || str == string.Empty;
            }

            #endregion
        }
    }

    /// <summary>
    /// This namespace contains extensions for <see cref="System.Reflection.Assembly"/> class
    /// </summary>
    namespace AssembliesExtensions
    {
        using System;
        using System.Linq;
        using System.Reflection;
        using System.Collections.Generic;

        /// <summary>
        /// This class contains all extensions for the assembly class
        /// </summary>
        public static class TypeLoaderExtensions
        {
            /// <summary>
            /// Returns all loadable types from an assembly
            /// </summary>
            /// <param name="assembly">This assembly</param>
            /// <returns></returns>
            public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
            {
                if (assembly == null)
                    throw new ArgumentNullException("Assembly");
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    return e.Types.Where(t => t != null);
                }
            }
        }
    }

    #endregion
}

