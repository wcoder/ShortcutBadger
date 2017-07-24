# ShortcutBadger ![version](http://img.shields.io/badge/original-v1.1.17-brightgreen.svg?style=flat) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.ShortcutBadger)](https://www.nuget.org/packages/Xamarin.ShortcutBadger/)

Port of [ShortcutBadger](https://github.com/leolin310148/ShortcutBadger) for Xamarin.Android

---

The ShortcutBadger makes your Android App show the count of unread messages as a badge on your App shortcut!

## Usage

NuGet

```
Install-Package Xamarin.ShortcutBadger
```

Add the codes below:
```csharp
int badgeCount = 1;
ShortcutBadger.ApplyCount(context, badgeCount);
```
If you want to remove the badge:
```csharp
ShortcutBadger.RemoveCount(context);
```
or
```csharp
ShortcutBadger.ApplyCount(context, 0);
```

## Supported launchers:<br/>

<table>
    <tr>
        <td width="130">
            <h3>Sony</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_sony.png"/>
        </td>
        <td width="130">
            <h3>Samsung</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_samsung.png"/>
        </td>
        <td width="130">
            <h3>LG</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_lg.png"/>
        </td>
        <td width="130">
            <h3>HTC</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_htc.png"/>
        </td>
    </tr>
    <tr>
        <td width="130">
            <h3>Xiaomi</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_xiaomi.png"/>
            <br>
        </td>
        <td width="130">
            <h3>ASUS</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_asus.png"/>
        </td>
        <td width="130">
            <h3>ADW</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_adw.png"/>
        </td>
        <td width="130">
            <h3>APEX</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_apex.png"/>
        </td>
    <tr>
        <td width="130">
            <h3>NOVA</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_nova.png"/>
        </td>
        <td width="130">
            <h3>Huawei</h3>
            <br>
            (Not Fully Support)
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_huawei.png"/>
            <br>
            (1.1.7+)
        </td>
        <td width="130">
            <h3>ZUK</h3>
            <br>
            <img src="https://raw.github.com/linwoain/ShortcutBadger/master/screenshots/ss_zuk.png"/>
            <br>
            (1.1.10+)
        </td>
        <td width="130">
            <h3>OPPO</h3>
            <br>
            (Not Fully Support)
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_oppo.png?raw=true"/>
            <br>
            (1.1.10+)
        </td>
    </tr>
    <tr>
        <td width="130">
            <h3>EverythingMe</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_evme.png"/>
        </td>
        <td width="130">
            <h3>ZTE</h3>
            <br>
            <img src="https://raw.github.com/leolin310148/ShortcutBadger/master/screenshots/ss_zte.png"/>
            <br>
            (1.1.17+)
        </td>
    </tr>
</table>

## About Xiaomi devices
Xiaomi devices required extra setup with notifications, please read [wiki](https://github.com/leolin310148/ShortcutBadger/wiki/Xiaomi-Device-Support)

## IsBadgeWorking?

A tool for to checking your device / launcher / android version and test/report if Shortcutbadger is working or not

LICENSE
===================================

        Copyright 2014-2017 Leo Lin, Yauheni Pakala

        Licensed under the Apache License, Version 2.0 (the "License");
        you may not use this file except in compliance with the License.
        You may obtain a copy of the License at

            http://www.apache.org/licenses/LICENSE-2.0

        Unless required by applicable law or agreed to in writing, software
        distributed under the License is distributed on an "AS IS" BASIS,
        WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
        See the License for the specific language governing permissions and
        limitations under the License.
