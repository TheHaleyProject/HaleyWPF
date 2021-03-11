using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("HaleyWPF")]
[assembly: AssemblyDescription("Contains some WPF controls & Helpers")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("TheHaleyProject")]
[assembly: AssemblyProduct("HaleyWPF")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//Should assembly version be always same? is it accepted?
[assembly: AssemblyVersion("2.0.9.0")]
[assembly: AssemblyFileVersion("2.0.9.0")]
[assembly: Guid("FECEB4DE-F5CA-4CB0-A18A-C48FAF964F38")]
[assembly: NeutralResourcesLanguage("en-GB")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: XmlnsPrefix("http://schemas.hpod9.com/haley/wpf", "hlyWPF")]

//FOR XAML NAMESPACES - WPF
//[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.ViewModels")]
//[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.Views")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.BaseControls")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.Enums")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.Events")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.Models")]