# AutoCheckInOut
Automatically chekc in/out for PHP Timeclock using selenium and phantomjs.

# How to use
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
