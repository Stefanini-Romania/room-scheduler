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
    internal class UserMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RSService.Validation.UserMessages", typeof(UserMessages).Assembly);
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
        ///   Looks up a localized string similar to User.Email.IsNotStefaniniDomain.
        /// </summary>
        internal static string EmailWrongEmail {
            get {
                return ResourceManager.GetString("EmailWrongEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.Email.IsEmpty.
        /// </summary>
        internal static string EmptyEmail {
            get {
                return ResourceManager.GetString("EmptyEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.FirstName.IsEmpty.
        /// </summary>
        internal static string EmptyFirstName {
            get {
                return ResourceManager.GetString("EmptyFirstName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.LastName.IsEmpty.
        /// </summary>
        internal static string EmptyLastName {
            get {
                return ResourceManager.GetString("EmptyLastName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.Password.IsEmpty.
        /// </summary>
        internal static string EmptyPassword {
            get {
                return ResourceManager.GetString("EmptyPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.Name.IsEmpty.
        /// </summary>
        internal static string EmptyUsername {
            get {
                return ResourceManager.GetString("EmptyUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.UserRole.IsEmpty.
        /// </summary>
        internal static string EmptyUserRole {
            get {
                return ResourceManager.GetString("EmptyUserRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.Email.IsNotUnique.
        /// </summary>
        internal static string UniqueEmail {
            get {
                return ResourceManager.GetString("UniqueEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.Name.IsNotUnique.
        /// </summary>
        internal static string UniqueUsername {
            get {
                return ResourceManager.GetString("UniqueUsername", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.UserRole.NotFound.
        /// </summary>
        internal static string UserRoleNotFound {
            get {
                return ResourceManager.GetString("UserRoleNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.Password.IsTooShort.
        /// </summary>
        internal static string WeakPassword {
            get {
                return ResourceManager.GetString("WeakPassword", resourceCulture);
            }
        }
    }
}
