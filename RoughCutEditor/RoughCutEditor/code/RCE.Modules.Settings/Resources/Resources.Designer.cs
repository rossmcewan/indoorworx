﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RCE.Modules.Settings.Resources {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RCE.Modules.Settings.Resources.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to /RCE.Modules.Settings;component/images/icon_off.png.
        /// </summary>
        public static string HeaderIconOff {
            get {
                return ResourceManager.GetString("HeaderIconOff", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /RCE.Modules.Settings;component/images/icon_on.png.
        /// </summary>
        public static string HeaderIconOn {
            get {
                return ResourceManager.GetString("HeaderIconOn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        public static string HeaderInfo {
            get {
                return ResourceManager.GetString("HeaderInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project name should not be empty or less than 100 characters..
        /// </summary>
        public static string InvalidProjectName {
            get {
                return ResourceManager.GetString("InvalidProjectName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value should follow HH:MM:SS;FF pattern..
        /// </summary>
        public static string InvalidStartTimeCode {
            get {
                return ResourceManager.GetString("InvalidStartTimeCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value should be less than {0}..
        /// </summary>
        public static string InvalidStartTimeCodeValue {
            get {
                return ResourceManager.GetString("InvalidStartTimeCodeValue", resourceCulture);
            }
        }
    }
}
