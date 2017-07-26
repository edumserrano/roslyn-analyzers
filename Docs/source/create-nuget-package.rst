.. _create-nuget-package:

Create nuget package for your analyzer
======================================

If you've used the :ref:`default template <easy-way>` to create your analyzer project then you don't need to worry because it is already configured to produce a nuget package upon building. If you have followed :ref:`pro-way` then you have to do some work to create a nuget package that will work properly for distributing your analyzer.

See `analyzers conventions <https://docs.microsoft.com/en-us/nuget/schema/analyzers-conventions>`_ for official documentation about packaging your analyzers.

All that is explained below was discovered from looking at how the :ref:`default template <easy-way>` generates the nuget package. In short by analyzing the nuspec file and the scripts inside the tools folder.

The nuspec file
---------------

There's lot of metadata that you can configure on the `nuspec file <https://docs.microsoft.com/en-us/nuget/schema/nuspec>`_ but the part that needs to be modified to create nuget packages for analyzers is::

	<?xml version="1.0" encoding="utf-8"?>
		<package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd">
  			<metadata>
    			...
    			<frameworkAssemblies>
      				<frameworkAssembly assemblyName="System" targetFramework="" />
    			</frameworkAssemblies>
    			<developmentDependency>true</developmentDependency>
  			</metadata>
  		<!-- The convention for analyzers is to put language agnostic dlls in analyzers\portable50 and language specific analyzers in either analyzers\portable50\cs or analyzers\portable50\vb -->
  		<files>
    		<file src="*.dll" target="analyzers\dotnet\cs" exclude="**\Microsoft.CodeAnalysis.*;**\System.Collections.Immutable.*;**\System.Reflection.Metadata.*;**\System.Composition.*" />
    		<file src="tools\*.ps1" target="tools\" />
  		</files>
	</package>

As explained in the section `analyzers path format <https://docs.microsoft.com/en-us/nuget/schema/analyzers-conventions#analyzers-path-format>`_ you can use more complex folder structures to target different languages. The line from the above config that is setting the framework to dotnet and the language to C# is::
	
	<file src="*.dll" target="analyzers\dotnet\cs" exclude="**\Microsoft.CodeAnalysis.*;**\System.Collections.Immutable.*;**\System.Reflection.Metadata.*;**\System.Composition.*" />

The second file line::

	<file src="tools\*.ps1" target="tools\" />

Makes sure the nuget package will contain the custom install and uninstall scripts that are required to add/remove the analyzer dll in the correct way when the nuget package is installed/removed.

Nuget Powershell Scripts
------------------------

You will need to create a folder in the root directory of your analyzer project and add two scripts to it:

	* install.ps1
	* uninstall.ps1

The scripts that I've used were a copy of the ones produced by the :ref:`default template <easy-way>` to create an analyzer project. However you can use the ones `from the official documentation <https://docs.microsoft.com/en-us/nuget/schema/analyzers-conventions#install-and-uninstall-scripts>`_.

They both do the same which is add all the dll files from your nuget package into the project's analyzer references::

	foreach ($analyzerFilePath in Get-ChildItem $analyzersPath -Filter *.dll)
	{
		if($project.Object.AnalyzerReferences)
		{
			$project.Object.AnalyzerReferences.Add($analyzerFilePath.FullName)
		}
	}

The uninstall script does the reverse by removing the analyzer dlls from the project's analyzer references.

.. note:: There is a small difference between the scripts from the default template and the ones in the official documentation. The scripts from the default template don't just get all the dlls from the nuget package. It gets all the dlls except the resource ones as you shown by::
	
	foreach ($analyzerFilePath in Get-ChildItem -Path "$analyzersPath\*.dll" -Exclude *.resources.dll)

   Although I have not tested this should only impact projects that are :ref:`localized <tips-and-more-resources>`. Altough the official documentation in not excluding the resource files it is probably the correct thing to do but I have not tested it.





