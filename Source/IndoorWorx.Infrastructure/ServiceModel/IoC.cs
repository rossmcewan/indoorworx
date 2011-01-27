using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace IndoorWorx.Infrastructure.ServiceModel
{
    public class IoC
    {
        public static IUnityContainer UnityContainer
        {
            get;
            set;
        }

        private static bool initialized = false;
        public static void Initialize(IUnityContainer _unityContainer)
        {
            if (!initialized)
            {
                initialized = true;
                UnityContainer = _unityContainer;
            }
        }

        public static void RegisterType<T1, T2>() where T2 : T1
        {
            UnityContainer.RegisterType<T1, T2>();
        }

        public static void RegisterType<T1, T2>(string name) where T2 : T1
        {
            UnityContainer.RegisterType<T1, T2>(name);
        }

        public static void RegisterInstance<T>(T instance)
        {
            UnityContainer.RegisterInstance<T>(instance);
        }

        public static void RegisterInstance<T>(string name, T instance)
        {
            UnityContainer.RegisterInstance<T>(name, instance);
        }

        public static T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return UnityContainer.Resolve<T>(name);
        }
    }
}
