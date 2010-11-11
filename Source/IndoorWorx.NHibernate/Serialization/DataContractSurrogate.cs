using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.CodeDom;
using NHibernate.Collection;
using NHibernate.Proxy;
using System.Runtime.Serialization;

namespace IndoorWorx.NHibernate.Serialization
{
    public class DataContractSurrogate : IDataContractSurrogate
    {
        public DataContractSurrogate() { }

        public Type GetDataContractType(Type type)
        {
            // Serialize proxies as the base type
            if (typeof(INHibernateProxy).IsAssignableFrom(type))
            {
                type = type.GetType().BaseType;
            }

            // Serialize persistent collections as the collection interface type
            if (typeof(IPersistentCollection).IsAssignableFrom(type))
            {
                foreach (Type collInterface in type.GetInterfaces())
                {
                    if (collInterface.IsGenericType)
                    {
                        type = collInterface;
                        break;
                    }
                    else if (!collInterface.Equals(typeof(IPersistentCollection)))
                    {
                        type = collInterface;
                    }
                }
            }

            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            // Serialize proxies as the base type
            if (obj is INHibernateProxy)
            {
                // Getting the implementation of the proxy forces an initialization of the proxied object (if not yet initialized)
                //obj = (obj as INHibernateProxy).HibernateLazyInitializer.GetImplementation();
                try
                {
                    obj = (obj as INHibernateProxy).HibernateLazyInitializer.GetImplementation();

                }
                catch
                {
                    try
                    {
                        obj = Activator.CreateInstance(targetType);
                    }
                    catch
                    {
                        obj = null;
                    }
                }
            }

            // Serialize persistent collections as the collection interface type
            if (obj is IPersistentCollection)
            {
                IPersistentCollection persistentCollection = (IPersistentCollection)obj;
                //persistentCollection.ForceInitialization();
                obj = persistentCollection.Entries(null);//.Entries(); // This returns the "wrapped" collection
                if (obj == null)
                {
                    if (targetType.IsGenericType)
                    {
                        var genericArgs = targetType.GetGenericArguments().First();
                        var genericList = typeof(List<>).MakeGenericType(genericArgs);
                        obj = Activator.CreateInstance(genericList);
                    }
                }
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }

}
