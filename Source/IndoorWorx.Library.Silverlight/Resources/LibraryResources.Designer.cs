﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndoorWorx.Library.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class LibraryResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LibraryResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("IndoorWorx.Library.Resources.LibraryResources", typeof(LibraryResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Catalog.
        /// </summary>
        public static string CatalogHeader {
            get {
                return ResourceManager.GetString("CatalogHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading media ....
        /// </summary>
        public static string LoadingMediaProgressMessage {
            get {
                return ResourceManager.GetString("LoadingMediaProgressMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search....
        /// </summary>
        public static string SearchWatermark {
            get {
                return ResourceManager.GetString("SearchWatermark", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown item type {0}.
        /// </summary>
        public static string UnknownItemTypeErrorMessage {
            get {
                return ResourceManager.GetString("UnknownItemTypeErrorMessage", resourceCulture);
            }
        }
    }
}
