<img src="logo.png" alt="Switcheroo" width="48px" height="48px"> Switcheroo  [![Build Status](https://img.shields.io/appveyor/ci/kvakulo/switcheroo.svg)](https://ci.appveyor.com/project/kvakulo/switcheroo) ![GPL License](https://img.shields.io/badge/license-GPL-brightgreen.svg)
==========

Switcheroo is for anyone who spends more time using a keyboard than a mouse.
Instead of alt-tabbing through a (long) list of open windows, Switcheroo allows
you to quickly switch to any window by typing in just a few characters of its title.

## Screenshot

<img src="screenshot.png" alt="Screenshot of Switcheroo in action" width="540px" height="372px">


Download
--------

**[Download Switcheroo here](https://github.com/kvakulo/Switcheroo/releases)**

If you use [Chocolatey](https://chocolatey.org), Switcheroo can be installed with `choco install switcheroo`.


Usage
-----

Action                         | Shortcut        | Remarks
------------------------------ | --------------- | ----------
Activate Switcheroo            | `Alt + Space`   | This shortcut can be customized in _Options_
Activate Switcheroo            | `Alt + Tab`     | Only works if enabled under _Options_
_When Switcheroo is open_      |                 |
Switch to selected window      | `Enter`         |
Close selected window          | `Ctrl + W`      |
Dismiss Switcheroo             | `Esc`           |



History
---------

Switcheroo was originally developed by [James Sulak](https://github.com/jsulak).

[Regin Larsen](https://github.com/kvakulo) took over the project in 2014 and is the current maintainer.

## V1.2
New feature/新功能

**Add a Hotkey to switch witching same process's different windows**  
 For example： After sitting Ctrl+Tab as the [HotKey for switching same process's different windows], when the user opened mutlitple MSWord windows, and one of them is activated, press Ctrl+Tab , one can switch to another window among those MSWord windows.

增加一组快捷键，用于切换当前程序的各个窗口
例如：设置Ctrl+tab为【当前程序切换快捷键】后，在打开若干个word文档，且当前某个wordk文档处于激活状态时，按下Ctrl+Tab就可以在word文档中切换

[HotKey for switching same process's different windows]
* When the process has only one window, nothing will happen when press the HotKey .
* When the process has only two windows, directly switch to the other one when press the HotKey .

[当前程序切换快捷键】
当前程序只有一个窗口时，按下快捷键不会做出任何反应。
当前程序有两个窗口时，按下快捷键会直接切换到另外一个窗口。
![image](https://user-images.githubusercontent.com/3627812/182014517-389c3a00-da0c-4dbc-9fd6-7d90f65a2797.png)

## V1.1
these abbreviation  is predefined  using ini file, so that it would be more easy for user to define their own abbreviation  for process

### ini file
```
[config]
Spliter=.
e=excel
f=explorer
w=word
p=powerpnt
```
left part of the equation mark is the abbreviation for the process name
right part of the equation mark is the process name

## V1
In **V1**, search by Chinese PinYin first letter is supported.
Add some prefined abbreviation for widely used software:
```
e. xx is to search Excel with the title of xx
w. xx is to search Word with the title of xx
p. xx is to search Powerpnt with the title of xx
f. xx is to search Explorer with the title of xx
```
the above mentioned abbreviation  is predefined in the program


How to contribute
-----------------

Please report any bug you encounter by [submitting an issue](https://github.com/kvakulo/Switcheroo/issues/new).

If you have an idea how to improve Switcheroo, then don't be shy to submit it as well.

Pull requests are greatly appreciated. If you plan a larger feature, then please get in contact, so we can coordinate the efforts.


License
-------

Switcheroo is open source and is licensed under the [GNU GPL v. 3](http://www.gnu.org/licenses/gpl.html).

```
Copyright 2014, 2015 Regin Larsen
Copyright 2009, 2010 James Sulak
 
Switcheroo is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Switcheroo is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
 
You should have received a copy of the GNU General Public License
along with Switcheroo.  If not, see <http://www.gnu.org/licenses/>.
```


Credits
-------

[HellBrick](https://github.com/HellBrick), [ovesen](https://github.com/ovesen), [philippotto](https://github.com/philippotto), [tarikguney](https://github.com/tarikguney), [holymoo](https://github.com/holymoo), [elig0n](https://github.com/elig0n) and [trond-snekvik](https://github.com/trond-snekvik) have contributed to Switcheroo.

Switcheroo makes use of these great open source projects:

* [Managed Windows API](http://mwinapi.sourceforge.net), Copyright © 2006 Michael Schier, GNU Lesser General Public License (LGPL)
* [PortableSettingsProvider](https://github.com/crdx/PortableSettingsProvider), Copyright © crdx, The MIT License (MIT)
