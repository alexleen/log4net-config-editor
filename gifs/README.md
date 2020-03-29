# How-to Generate Demo GIF
## AutoIt
>[AutoIt](https://www.autoitscript.com/site/) v3 is a freeware BASIC-like scripting language designed for automating the Windows GUI and general scripting. It uses a combination of simulated keystrokes, mouse movement and window/control manipulation in order to automate tasks in a way not possible or reliable with other languages.
## Getting Started
1. Run the log4net-config-editor in Release mode (script will launch release .exe)
3. Adjust sizing and close (so that when the app is launched by AutoIt, it's launched in the size & at the location you want)
   * Deleting the contents of `%userprofile%\AppData\Local\Editor` will remove saved settings - allowing you to clear out any previous size, location, and/or opened files
4. Open an XML editor in the background so that viewers can see what the tool is doing on save
5. Download the [AutoIt Script Editor](https://www.autoitscript.com/site/autoit-script-editor/)
5. Open [demo.au3](demo.au3) in the script editor and run - most of the click coordinates are releative, so window size and position shouldn't matter
6. Record with something like [ScreenToGif](https://www.screentogif.com/)