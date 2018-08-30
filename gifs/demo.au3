#include <AutoItConstants.au3>

;Mouse coordinates are relative to window
;Opt("MouseCoordMode", 0)

;Title match mode = start
Opt("WinTitleMatchMode", 1)

$title = "log4net Configuration Editor"
$mouseSpeed = 25

Run("..\Source\Editor\bin\Release\Editor.exe")
WinWaitActive($title)
$pos = WinGetPos($title)
$mainWindowRightButtonsX = $pos[0] + $pos[2] - 70

;Click on Add Root
MouseClick($MOUSE_CLICK_LEFT, $mainWindowRightButtonsX, $pos[1] + 100, 1, $mouseSpeed)

WinWaitActive("Root Logger")
$pos = WinGetPos("Root Logger")

;Click on Save
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 210, $pos[1] + 325, 1, $mouseSpeed)

$pos = WinGetPos($title)

;Click on Add Appender
MouseClick($MOUSE_CLICK_LEFT, $mainWindowRightButtonsX, $pos[1] + 80, 1, $mouseSpeed)

;Click on console
MouseClick($MOUSE_CLICK_LEFT, $mainWindowRightButtonsX, $pos[1] + 105, 1, $mouseSpeed)

Sleep(500)

;Type appender name
Send("appenderName")

WinWaitActive("Console Appender")
$pos = WinGetPos("Console Appender")
$addFilterX = $pos[0] + $pos[2] - 60

;Click on Add (for filter)
MouseClick($MOUSE_CLICK_LEFT, $addFilterX, $pos[1] + 220, 1, $mouseSpeed)

;Click on Deny All
MouseClick($MOUSE_CLICK_LEFT, $addFilterX, $pos[1] + 250, 1, $mouseSpeed)

;Click on Add (for filter), again
MouseClick($MOUSE_CLICK_LEFT, $addFilterX, $pos[1] + 220, 1, $mouseSpeed)

;Click on Logger
MouseClick($MOUSE_CLICK_LEFT, $addFilterX, $pos[1] + 325, 1, $mouseSpeed)

Sleep(500)

;Type logger name
Send("loggerName")

$pos = WinGetPos("Logger Match Filter")

;Click on Save
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 210, $pos[1] + 100, 1, $mouseSpeed)

$pos = WinGetPos("Console Appender")

;Click on 'Up' to move logger filter above deny all
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 285, $pos[1] + 280, 1, $mouseSpeed)

Sleep(250)

;Enable root incoming ref
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 457, $pos[1] + 375, 1, $mouseSpeed)

Sleep(250)

;Click on Save
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 305, $pos[1] + 625, 1, $mouseSpeed)

SaveAndShow()

;Click on appender in grid
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 450, $pos[1] + 110, 1, $mouseSpeed)

;Click on Remove Ref
MouseClick($MOUSE_CLICK_LEFT, $mainWindowRightButtonsX, $pos[1] + 235, 1, $mouseSpeed)

SaveAndShow()

;Click on appender in grid
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 450, $pos[1] + 110, 1, $mouseSpeed)

;Ctrl + Click on root in grid so that both are selected
Send("{CTRLDOWN}")
MouseClick($MOUSE_CLICK_LEFT, $pos[0] + $pos[2] - 450, $pos[1] + 125, 1, $mouseSpeed)
Send("{CTRLUP}")

;Click on Remove
MouseClick($MOUSE_CLICK_LEFT, $mainWindowRightButtonsX, $pos[1] + 210, 1, $mouseSpeed)

SaveAndShow()

;TODO some of the above coordinates need to be relative to the LEFT of the window and not the right

Func SaveAndShow()
   $pos = WinGetPos($title)

   ;Click on Save (relative to center bottom)
   ;MouseClick($MOUSE_CLICK_LEFT, ($pos[0] + $pos[2] / 2) - 90, $pos[1] + $pos[3] - 25, 1, $mouseSpeed)
   ;TODO show
EndFunc