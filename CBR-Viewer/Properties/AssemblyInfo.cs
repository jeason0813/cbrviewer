using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CBReader")]
[assembly: AssemblyDescription("CBReader is a small application to visualize .cbr or .cbz type comic ebooks. I saw a very nice one on codeplex (wfpbookreader.codeplex.com). It looks very nice and has many features, but I decided to build a small and simple one that can do much less.\r\n\r\n" +
    "\r\n" +
    "Inspirations came from (in random order): \r\n\r\n" +
    "Microsoft Developer Network [http://msdn.microsoft.com] \r\n\r\n" +
    "C.B.R [http://www.codeproject.com/Articles/294452/C-B-R]\r\n\r\n" + 
    "Sacha Barber [http://www.codeproject.com/Articles/37366/Styling-A-ScrollViewer-Scrollbar-In-WPF]\r\n\r\n" +
    "immortalus [http://www.codeproject.com/Articles/437237/WPF-Grid-Column-and-Row-Hiding]\r\n\r\n" +
    "WPF Minimal Button Styling [http://gregandora.wordpress.com/2011/02/06/wpf-minimal-button-styling/]\r\n\r\n" +    
	"WPF Loading Wait Adorner (Jeremy Hutchinson) [http://www.codeproject.com/Articles/57984/WPF-Loading-Wait-Adorner] \r\n\r\n" +
	"7-Zip [http://www.7-zip.org/] \r\n\r\n" +
    "MVVM Light Toolkit [http://www.galasoft.ch/mvvm/] \r\n\r\n" +
    "NLog advanced .NET Logging [http://nlog-project.org/] \r\n\r\n" +
    "")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Gazillion-Bytes")]
[assembly: AssemblyProduct("CBReader")]
[assembly: AssemblyCopyright("©2012-2017 \nErik Molenaar")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// In order to begin building localizable applications, set 
// <UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
// inside a <PropertyGroup>.  For example, if you are using US english
// in your source files, set the <UICulture> to en-US.  Then uncomment
// the NeutralResourceLanguage attribute below.  Update the "en-US" in
// the line below to match the UICulture setting in the project file.
[assembly: NeutralResourcesLanguage("en-US",
    UltimateResourceFallbackLocation.MainAssembly)]

// ResourceDictionaryLocation.None, where theme specific resource dictionaries are located
// (used if a resource is not found in the page, 
// or application resource dictionaries)
// ResourceDictionaryLocation.SourceAssembly where the generic resource dictionary is located
// (used if a resource is not found in the page, 
// app, or any theme specific resource dictionaries)
[assembly: ThemeInfo(ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
[assembly: AssemblyVersion("1.7.41.513")]
[assembly: AssemblyFileVersion("1.7.41.513")]
