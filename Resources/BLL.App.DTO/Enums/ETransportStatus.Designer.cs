﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources.BLL.App.DTO.Enums {
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
    public class ETransportStatus {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ETransportStatus() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.BLL.App.DTO.Enums.ETransportStatus", typeof(ETransportStatus).Assembly);
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
        ///   Looks up a localized string similar to Accepted.
        /// </summary>
        public static string Accepted {
            get {
                return ResourceManager.GetString("Accepted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Canceled.
        /// </summary>
        public static string Canceled {
            get {
                return ResourceManager.GetString("Canceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delivered.
        /// </summary>
        public static string Delivered {
            get {
                return ResourceManager.GetString("Delivered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to On delivery.
        /// </summary>
        public static string OnDelivery {
            get {
                return ResourceManager.GetString("OnDelivery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rejected.
        /// </summary>
        public static string Rejected {
            get {
                return ResourceManager.GetString("Rejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Submitted.
        /// </summary>
        public static string Submitted {
            get {
                return ResourceManager.GetString("Submitted", resourceCulture);
            }
        }
    }
}
