.. _nuget-packages:

Nuget packages required to develop analyzers
=============================================

When creating the analyzers and test projects you will have to reference nugets that contain the Roslyn APIs and not only enable the development of the Analyzers and Code Fix Providers as well as integrate them with Visual Studio.

To develop analyzers and code fixes you only need to referece:

* `Microsoft.CodeAnalysis.CSharp.Workspaces <https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp.Workspaces>`_.

All the required packages are dependencies of Microsoft.CodeAnalysis.CSharp.Workspaces.

Microsoft.CodeAnalysis.CSharp.Workspaces package
-------------------------------------------------

Although one might think that the latest versions of the packages is what we should use in this case it is not that simple. After asking around on the  `Gitter channel for Roslyn <https://gitter.im/dotnet/roslyn>`_ I found out that the version of **Microsoft.CodeAnalysis.CSharp.Workspaces** is actually related with the version of **Visual Studio**. Sadly this mapping does not exist anywhere but I was told the pattern works like this:

* 1.0 -> Visual Studio 2015.0 (Version 14.0)
* 1.1 -> Visual Studio 2015.1 (Version 14.1)
* etc 
* 2.0 -> Visual Studio 2017.0 (Version 15.0)
* 2.1 -> Visual Studio 2017.1 (Version 15.1)
* 2.2 -> Visual Studio 2017.2 (Version 15.2)
* etc

The major and minor versions of Microsoft.CodeAnalysis.CSharp.Workspaces are directly related with the major and minor version of Visual Studio. The build part of the version is used for improvements/bug fixes. This is why the :ref:`default template <easy-way>` for creating Roslyn analyzers uses version 1.0.1 of Microsoft.CodeAnalysis.CSharp.Workspaces. It ensures compatibility with Visual Studio 2015.

It is up to you to chose which version you want to support and there might be a analyzers that you probably won't be able to do whilst at the same time keeping support for Visual Studio 2015. I'm thinking about analyzers for `new C# 7.0 language features <https://blogs.msdn.microsoft.com/dotnet/2017/03/09/new-features-in-c-7-0/>`_ that came out with Visual Studio 2017. If you're doing analyzers for those then you probably need to use at least version 2.0 of Microsoft.CodeAnalysis.CSharp.Workspaces.
