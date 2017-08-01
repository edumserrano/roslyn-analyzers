.. _create-nuget-package:

Create a nuget package for your analyzer
========================================

If you've used the :ref:`default template <easy-way>` to create your analyzer project then you don't need to worry because it is already configured to produce a nuget package upon building. If you have followed :ref:`pro-way` then you have to do some work to create a nuget package that will work properly for distributing your analyzer.

See `analyzers conventions <https://docs.microsoft.com/en-us/nuget/schema/analyzers-conventions>`_ for official documentation about packaging your analyzers.

All that is explained below was discovered from looking at how the :ref:`default template <easy-way>` generates the nuget package. In short by analyzing the nuspec file and the scripts inside the tools folder.

Using a nuspec file
-------------------

If you are using a .NET Core or .NET Standard project for your analyzers consider using the support for creating NuGet packages present in the csproj config.

Nuspec file
~~~~~~~~~~~

There's lot of metadata that you can configure on the `nuspec file <https://docs.microsoft.com/en-us/nuget/schema/nuspec>`_ but to create nuget packages for analyzers you have to at least set the below metadata and files::

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

.. _nuget-powershell-scripts:

Nuget Powershell Scripts
~~~~~~~~~~~~~~~~~~~~~~~~

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

.. _note-about-localization-and-nuegt-install-script:

.. note:: There is a small difference between the scripts from the default template and the ones in the official documentation. The scripts from the default template don't just get all the dlls from the nuget package. It gets all the dlls except the resource ones as you shown by::
	
	foreach ($analyzerFilePath in Get-ChildItem -Path "$analyzersPath\*.dll" -Exclude *.resources.dll)

   I have not tested but this line of the powershell script should only impact projects that are :ref:`localized <tips-and-more-resources>`. Altough the official documentation in not excluding the resource files it is probably the correct thing to do but I have not tested it.

Using support from the csproj
-----------------------------

If you are using .NET Core or .NET Standard project for your analyzers then you can make use of the csproj support to create NuGet packages. Here is an example of a csproj file configured to create a NuGet package for a .NET Standard analyzer project::

	<Project Sdk="Microsoft.NET.Sdk">

  		<PropertyGroup>
			<TargetFramework>netstandard1.4</TargetFramework>
			<PackageTargetFallback>portable-net45+win8+wp8+wpa81</PackageTargetFallback>
			<IncludeBuildOutput>false</IncludeBuildOutput>
			<GeneratePackageOnBuild>True</GeneratePackageOnBuild>
  		</PropertyGroup>

  		<PropertyGroup>
			<PackageId>Id.Of.Your.Package</PackageId>
			<PackageVersion>1.0.0.0</PackageVersion>
			<Authors>YOUR_NAME</Authors>
			<PackageLicenseUrl>http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE</PackageLicenseUrl>
			<PackageProjectUrl>http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE</PackageProjectUrl>
			<PackageIconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</PackageIconUrl>
			<RepositoryUrl>http://REPOSITORY_URL_HERE_OR_DELETE_THIS_LINE</RepositoryUrl>
			<PackageRequireLicenseAcceptance>false</PackageRequireLicenseAcceptance>
			<Description>Description of the package</Description>
			<PackageReleaseNotes>Summary of changes made in this release of the package or delete this line.</PackageReleaseNotes>
			<PackageTags>tag1, tag2, tag3</PackageTags>
			<NoPackageAnalysis>true</NoPackageAnalysis>
  		</PropertyGroup>

  		<ItemGroup>
			<PackageReference Include="Microsoft.CodeAnalysis.CSharp.Workspaces" Version="2.2.0" PrivateAssets="all" />
			<PackageReference Update="NETStandard.Library" PrivateAssets="all" />
  		</ItemGroup>

		<ItemGroup>
			<None Update="tools\*.ps1" CopyToOutputDirectory="Always" Pack="true" PackagePath="tools" />
			<None Include="$(OutputPath)\$(AssemblyName).dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />
		</ItemGroup>

	</Project>


For more information on the csproj configuration see `additions to the csproj format <https://docs.microsoft.com/en-us/dotnet/core/tools/csproj>`_ and more specifically `NuGet metadata properties <https://docs.microsoft.com/en-us/dotnet/core/tools/csproj#nuget-metadata-properties>`_.

The parts that are worth calling out on this csproj config are:

* The **GeneratePackageOnBuild** property will determine if a NuGet package is created as part of building the project::

	<GeneratePackageOnBuild>True</GeneratePackageOnBuild>

* The **NoPackageAnalysis** will ignore warnings from building the NuGet package::

	<NoPackageAnalysis>true</NoPackageAnalysis>

* The **PackageReference** lines will make sure that the NuGet package created has no dependencies. Otherwise when consuming the NuGet you would be required to install Microsoft.CodeAnalysis.CSharp.Workspaces and NETStandard.Library::

	<ItemGroup>
		<PackageReference Include="Microsoft.CodeAnalysis.CSharp.Workspaces" Version="2.2.0" PrivateAssets="all" />
		<PackageReference Update="NETStandard.Library" PrivateAssets="all" />
	</ItemGroup>

* You also need to make sure the NuGet package will contain the **custom install and uninstall scripts** that are required to add/remove the analyzer dll in the correct way when the nuget package is installed/removed. You will need to :ref:`create a tools folder with the required scripts <nuget-powershell-scripts>` for this to work. The line responsible for this is::

	<None Update="tools\*.ps1" CopyToOutputDirectory="Always" Pack="true" PackagePath="tools" />

* By default a NuGet package will contain the assembly inside a lib folder. However NuGet for analyzers follow a different convention as explained in the section `analyzers path format <https://docs.microsoft.com/en-us/nuget/schema/analyzers-conventions#analyzers-path-format>`_. In summary for a C# analyzer **the assembly packaged in the NuGet should be in the folder analyzers/dotnet/cs**. The line responsible for this is::

	<None Include="$(OutputPath)\$(AssemblyName).dll" Pack="true" PackagePath="analyzers/dotnet/cs" Visible="false" />

.. note:: I have not tested packaging a localized analyzer so there might be an extra step necessary to make the localization work with the NuGet package. See :ref:`this note about the install scripts <note-about-localization-and-nuegt-install-script>`.




