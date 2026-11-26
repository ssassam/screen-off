# Screen Off

A tiny Windows taskbar button that locks your PC and turns the screen off.

![icon](ScreenOff.ico)

## What it does

1. Click it: a 10-second countdown appears full-screen.
2. Press **Esc** or click anywhere to cancel.
3. At zero, Windows **locks** and the **monitor turns off**.
4. Press **Enter** (or any key) to wake the screen, then unlock as usual.

## Install

Download `ScreenOff.exe` from [Releases](../../releases), right-click it → **Show more options** → **Pin to taskbar**.

## Build from source

No SDK needed — uses the C# compiler that ships with Windows (.NET Framework 4):

```
build.cmd
```

This produces `ScreenOff.exe` with the moon icon embedded.

## Customize

Change `CountdownSeconds` in `ScreenOff.cs` and rebuild.

## License

MIT
