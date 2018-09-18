﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperBasic.Utilities.Resources {
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
    public class DiagnosticsResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DiagnosticsResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SuperBasic.Utilities.Resources.DiagnosticsResources", typeof(DiagnosticsResources).Assembly);
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
        ///   Looks up a localized string similar to You can only assign submodules to events..
        /// </summary>
        public static string AssigningNonSubModuleToEvent {
            get {
                return ResourceManager.GetString("AssigningNonSubModuleToEvent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This expression must have a value to be used here..
        /// </summary>
        public static string ExpectedExpressionWithAValue {
            get {
                return ResourceManager.GetString("ExpectedExpressionWithAValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The label &apos;{0}&apos; is not defined in the same module..
        /// </summary>
        public static string GoToUndefinedLabel {
            get {
                return ResourceManager.GetString("GoToUndefinedLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This expression is not a valid statement..
        /// </summary>
        public static string InvalidExpressionStatement {
            get {
                return ResourceManager.GetString("InvalidExpressionStatement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library member &apos;{0}.{1}&apos; was available in older versions only, and has not been made available to SuperBasic yet..
        /// </summary>
        public static string LibraryMemberDeprecatedFromDesktop {
            get {
                return ResourceManager.GetString("LibraryMemberDeprecatedFromDesktop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library &apos;{0}&apos; has no member named &apos;{1}&apos;..
        /// </summary>
        public static string LibraryMemberNotFound {
            get {
                return ResourceManager.GetString("LibraryMemberNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library &apos;{0}&apos; was available in older versions only, and has not been made available to SuperBasic yet..
        /// </summary>
        public static string LibraryTypeDeprecatedFromDesktop {
            get {
                return ResourceManager.GetString("LibraryTypeDeprecatedFromDesktop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You cannot use the library &apos;{0}&apos; here since you used the library &apos;{1}&apos; in the same program. Are you writing a text-based or a graphics program?.
        /// </summary>
        public static string MultipleProgramKindsUsed {
            get {
                return ResourceManager.GetString("MultipleProgramKindsUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property &apos;{0}.{1}&apos; cannot be assigned to. It is ready only..
        /// </summary>
        public static string PropertyHasNoSetter {
            get {
                return ResourceManager.GetString("PropertyHasNoSetter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A label with the same name &apos;{0}&apos; is already defined..
        /// </summary>
        public static string TwoLabelsWithTheSameName {
            get {
                return ResourceManager.GetString("TwoLabelsWithTheSameName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A submodule with the same name &apos;{0}&apos; is already defined in the same program..
        /// </summary>
        public static string TwoSubModulesWithTheSameName {
            get {
                return ResourceManager.GetString("TwoSubModulesWithTheSameName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This expression returns a result. Did you mean to assign it to a variable?.
        /// </summary>
        public static string UnassignedExpressionStatement {
            get {
                return ResourceManager.GetString("UnassignedExpressionStatement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are passing &apos;{0}&apos; arguments to this method, while it expects &apos;{1}&apos; arguments..
        /// </summary>
        public static string UnexpectedArgumentsCount {
            get {
                return ResourceManager.GetString("UnexpectedArgumentsCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I was expecting to see &apos;{0}&apos; after this..
        /// </summary>
        public static string UnexpectedEndOfStream {
            get {
                return ResourceManager.GetString("UnexpectedEndOfStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This statement should go on a new line..
        /// </summary>
        public static string UnexpectedStatementInsteadOfNewLine {
            get {
                return ResourceManager.GetString("UnexpectedStatementInsteadOfNewLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I didn&apos;t expect to see &apos;{0}&apos; here. I was expecting &apos;{1}&apos; instead..
        /// </summary>
        public static string UnexpectedTokenFound {
            get {
                return ResourceManager.GetString("UnexpectedTokenFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I didn&apos;t expect to see &apos;{0}&apos; here. I was expecting the start of a new statement..
        /// </summary>
        public static string UnexpectedTokenInsteadOfStatement {
            get {
                return ResourceManager.GetString("UnexpectedTokenInsteadOfStatement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I don&apos;t understand this character &apos;{0}&apos;..
        /// </summary>
        public static string UnrecognizedCharacter {
            get {
                return ResourceManager.GetString("UnrecognizedCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This expression is not a valid array..
        /// </summary>
        public static string UnsupportedArrayBaseExpression {
            get {
                return ResourceManager.GetString("UnsupportedArrayBaseExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only use dot access with a library. Did you mean to use an existing library instead?.
        /// </summary>
        public static string UnsupportedDotBaseExpression {
            get {
                return ResourceManager.GetString("UnsupportedDotBaseExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This expression is not a valid submodule or method to be called..
        /// </summary>
        public static string UnsupportedInvocationBaseExpression {
            get {
                return ResourceManager.GetString("UnsupportedInvocationBaseExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This string is missing its right double quotes..
        /// </summary>
        public static string UnterminatedStringLiteral {
            get {
                return ResourceManager.GetString("UnterminatedStringLiteral", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value &apos;{0}&apos; is not a valid number..
        /// </summary>
        public static string ValueIsNotANumber {
            get {
                return ResourceManager.GetString("ValueIsNotANumber", resourceCulture);
            }
        }
    }
}
