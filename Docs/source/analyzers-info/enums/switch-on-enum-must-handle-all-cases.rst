.. the orphan tag avoids the build warning about the rst file not being present in any toc tree

:orphan:

.. _switch-on-enum-must-handle-all-cases:

Populate switch
===================================================

**Identifier**: ENUM0003

**Default Action**: Warning

**Rationale**: Add missing switch cases. A switch is considered incomplete if it is missing a possible value of the enum or the default case. This allows for more safe refactoring and avoids bugs by leaving incomplete switch statements.

.. image:: ../../images/analyzers/enums/switch-on-enum-must-handle-all-cases.png

