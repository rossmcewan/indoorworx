using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace IndoorWorx.Infrastructure
{    
    public class IoC
    {
        private static IUnityContainer unityContainer;

        private static bool initialized = false;
        public static void Initialize(IUnityContainer _unityContainer)
        {
            if (!initialized)
            {
                initialized = true;
                unityContainer = _unityContainer;
            }
        }

        public static void RegisterType<T1, T2>() where T2 : T1
        {
            unityContainer.RegisterType<T1, T2>();
        }

        public static void RegisterType<T1, T2>(string name) where T2 : T1
        {
            unityContainer.RegisterType<T1, T2>(name);
        }

        public static void RegisterInstance<T>(T instance)
        {
            unityContainer.RegisterInstance<T>(instance);
        }

        public static void RegisterInstace<T>(string name, T instance)
        {
            unityContainer.RegisterInstance<T>(name, instance);
        }

        public static T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return unityContainer.Resolve<T>(name);
        }

        public static object Resolve(Type type)
        {
            return unityContainer.Resolve(type);
        }

        public static object Resolve(Type type, string name)
        {
            return unityContainer.Resolve(type, name);
        }
    }
}
