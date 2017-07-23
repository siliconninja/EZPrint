# EZPrint (Windows)

Another way to make school easier -- print any PDF file from your personal laptop to the school library (if they use Google Cloud Print for print authentication).

This uses an unofficial Google Cloud Print proxy written in C# to connect to the school's backend server and query a PDF file for printing.

This app is simple and has just one purpose: log in and load a file into the app.

This only works at RHS.

(EDIT 2017: It is no longer working. A newer library or authentication method is probably required now for Google Cloud Print. This is a great starting point/reference though.)

## Current Version

1.0: Open Source

## How to run

Build it using Visual Studio Community 2013/2017 or later with WPF (Windows Presentation Framework) installed.

No extra compiler arguments or options are necessary.

## Libraries Used

[CloudPrintProxy](https://github.com/klightspeed/CloudPrintProxy)

## License

[Creative Commons BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)
