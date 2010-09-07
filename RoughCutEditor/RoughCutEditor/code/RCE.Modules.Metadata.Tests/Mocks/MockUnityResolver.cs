// <copyright file="MockUnityResolver.cs" company="Microsoft Corporation">
// ===============================================================================
//
//
// Project: Microsoft Silverlight Rough Cut Editor
// FILES: MockUnityResolver.cs                     
//
// ===============================================================================
// Copyright 2010 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// </copyright>

namespace RCE.Modules.Metadata.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;

    public class MockUnityResolver : IUnityContainer
    {
        private readonly Dictionary<Type, object> bag = new Dictionary<Type, object>();

        public Dictionary<Type, object> Bag
        {
            get { return this.bag; }
        }

        IUnityContainer IUnityContainer.Parent
        {
            get { throw new System.NotImplementedException(); }
        }

        public T Resolve<T>()
        {
            return (T)this.bag[typeof(T)];
        }

        public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager) where TTo : TFrom
        {
            // ignore
            return this;
        }

        public IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            // ignore
            return this;
        }

        IUnityContainer IUnityContainer.RegisterType<T>(params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers)
        {
            return this;
        }

        IUnityContainer IUnityContainer.RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return this;
        }

        IUnityContainer IUnityContainer.RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType<T>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType<T>(string name, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType<T>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type t, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, string name, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type t, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type t, string name, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type t, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance<TInterface>(TInterface instance)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance<TInterface>(TInterface instance, LifetimeManager lifetimeManager)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance<TInterface>(string name, TInterface instance)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance<TInterface>(string name, TInterface instance, LifetimeManager lifetimeManager)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance(Type t, object instance)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance(Type t, object instance, LifetimeManager lifetimeManager)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance(Type t, string name, object instance)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new System.NotImplementedException();
        }

        T IUnityContainer.Resolve<T>(string name)
        {
            throw new System.NotImplementedException();
        }

        object IUnityContainer.Resolve(Type t)
        {
            throw new System.NotImplementedException();
        }

        object IUnityContainer.Resolve(Type t, string name)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<T> IUnityContainer.ResolveAll<T>()
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<object> IUnityContainer.ResolveAll(Type t)
        {
            throw new System.NotImplementedException();
        }

        T IUnityContainer.BuildUp<T>(T existing)
        {
            throw new System.NotImplementedException();
        }

        T IUnityContainer.BuildUp<T>(T existing, string name)
        {
            throw new System.NotImplementedException();
        }

        object IUnityContainer.BuildUp(Type t, object existing)
        {
            throw new System.NotImplementedException();
        }

        object IUnityContainer.BuildUp(Type t, object existing, string name)
        {
            throw new System.NotImplementedException();
        }

        void IUnityContainer.Teardown(object o)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.AddExtension(UnityContainerExtension extension)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.AddNewExtension<TExtension>()
        {
            throw new System.NotImplementedException();
        }

        TConfigurator IUnityContainer.Configure<TConfigurator>()
        {
            throw new System.NotImplementedException();
        }

        object IUnityContainer.Configure(Type configurationInterface)
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.RemoveAllExtensions()
        {
            throw new System.NotImplementedException();
        }

        IUnityContainer IUnityContainer.CreateChildContainer()
        {
            throw new System.NotImplementedException();
        }
       
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}