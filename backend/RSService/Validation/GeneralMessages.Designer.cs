﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RSService.Validation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class GeneralMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal GeneralMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RSService.Validation.GeneralMessages", typeof(GeneralMessages).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.Event.Authentication.
        /// </summary>
        internal static string Authentication {
            get {
                return ResourceManager.GetString("Authentication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.ConfigVar.Empty.
        /// </summary>
        internal static string ConfigVar {
            get {
                return ResourceManager.GetString("ConfigVar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.Event.Creation.
        /// </summary>
        internal static string Event {
            get {
                return ResourceManager.GetString("Event", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.Event.Edit.
        /// </summary>
        internal static string EventEdit {
            get {
                return ResourceManager.GetString("EventEdit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.Room.Creation.
        /// </summary>
        internal static string Room {
            get {
                return ResourceManager.GetString("Room", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.User.Creation.
        /// </summary>
        internal static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
        }
    }
}
