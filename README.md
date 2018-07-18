# AutoCheckInOut
Automatically chekc in/out for PHP Timeclock using selenium and phantomjs.

# How to use
- Get binary from [Release](https://github.com/n696395/AutoCheckInOut/releases)

- Edit config.xml
``` xml
<root>
	<url>http://localhost/timeclock1/timeclock.php</url>
	<name>YourName</name>
	<delay>15</delay><!-- Minute -->
</root>
```

- Run program using command line
    - Check In : `./AutoCheckIn.exe In`
    - Check Out : `./AutoCheckIn.exe Out`
- Add to task scheduler
	- Run `./Install.bat`
- Remove from task scheduler
	- Run `./UnInstall.bat`
# TODO
- [x] Support task schedule
- [ ] Combine with Google Calendar or others
