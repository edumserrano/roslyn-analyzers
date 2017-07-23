.. _how-to-debug:

How to debug an analyzer
========================

Once you've created your :ref:`VSIX project <how-to-start>` make sure it is set as the default start up project by right clicking it and chosing the option Set as StartUp Project.
Also go to the VSIX project's properties and confirm that the Debug tab has the following:

* Under Start action the option to Start external program should be selected and the location should be where you have installed visual studio. Something like C:/Program Files (x86)/Microsoft Visual Studio/2017/Enterprise/Common7/IDE/devenv.exe.
* Under Start options the Command line arguments should be set to /rootsuffix Exp. See `Devenv command-line switches <https://docs.microsoft.com/en-us/visualstudio/extensibility/devenv-command-line-switches-for-vspackage-development>`_ for more info.

Once you've done this Start Debugging on Visual Studio which will launch the `experimental version of Visual Studio <https://docs.microsoft.com/en-us/visualstudio/extensibility/the-experimental-instance>`_. To confirm that your VSIX project with your analyzers is installed in this experimental version of Visual Studio go to Tools->Extensions and Updates and in the Installed extensions you will find the VSIX project that you launched. The Name and Description set in the source.extension.vsixmanifest file of the VSIX project will show up in the list.

.. image:: images/debug_installed_extensions.PNG

Now all you need to do is create a new solution to test your analyzer. Simply create a new project of the type of your chosing and add a code that should trigger the analyzer.

You can put breakpoints on the Visual Studio that has your analyzer code because it is debugging the experimental version of visual studio.

Reseting your experimental version of Visual Studio
---------------------------------------------------

If you're not careful with the ids and versions of your VSIX projects that you deploy onto the experimental version of Visual Studio you might get into a place where you can not debug properly because the VSIX fails to be installed.
If you think you're in that position you should either:

* **Launch a different experimental instance of Visual Studio**: In the Command line arguments instead of "/rootsuffix Exp" change the word Exp to anything else you would like, for instance "/rootsuffix Exp2" and a new experimental version of Visual Studio will be launched. Each time you chose a new word a new experimental isntance is launched and unless you reset them they remember the VSIX that were deployed to them.
* **Reset your experimental instance**: for that you will need to use the `CreateExpInstance utility <https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/createexpinstance-utility>`_.
