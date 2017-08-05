Analyzers and Code Fixes for C#
===============================

|build-status| |nuget| |docs| 

This repository started as a learning experience about the Roslyn API. Hopefully it will grow to hold many more analyzers.

Installing
=================================================

Installation is performed via NuGet::
    
    PM> Install-Package Roslyn.Analyzers

Building
=================================================

This repository adheres to the `F5 manifesto <http://www.khalidabuhakmeh.com/the-f5-manifesto-for-net-developers>`_ so you should be able to clone, open in Visual Studio and build.

Documentation
=================================================

For documentation go `here <http://roslyn-analyzers.readthedocs.io/en/latest/>`_.

List of Analyzers
=================

For list of analyzers go `here <http://roslyn-analyzers.readthedocs.io/en/latest/analyzers-in-the-repo.html>`_.

Licence
=================================================

This project is licensed under the `MIT license <https://github.com/edumserrano/roslyn-analyzers/blob/master/Licence>`_.


.. |build-status| image:: https://eduardomserrano.visualstudio.com/_apis/public/build/definitions/e575bb72-927b-4cb5-aabf-df6415768b5b/31/badge
    :alt: build status
    :scale: 100%
    :target: https://eduardomserrano.visualstudio.com/_apis/public/build/definitions/e575bb72-927b-4cb5-aabf-df6415768b5b/31/badge

.. |docs| image:: https://readthedocs.org/projects/roslyn-analyzers/badge/?version=latest
    :alt: Documentation Status
    :scale: 100%
    :target: http://roslyn-analyzers.readthedocs.io/en/latest/?badge=latest
    
.. |nuget| image:: https://img.shields.io/nuget/v/Roslyn.Analyzers.svg?style=flat
    :alt: nuget package
    :scale: 100%
    :target: https://www.nuget.org/packages/Roslyn.Analyzers/
