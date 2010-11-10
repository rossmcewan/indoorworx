using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;

namespace IndoorWorx.Library
{
    public class Module : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        #region IModule Members

        public void Initialize()
        {
        }

        #endregion
    }
}
