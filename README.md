# log4net-config-editor
[![Build status](https://ci.appveyor.com/api/projects/status/boirqfr14whjdmlr/branch/master?svg=true)](https://ci.appveyor.com/project/alexleen/log4net-config-editor/branch/master)
[![codecov](https://codecov.io/gh/alexleen/log4net-config-editor/branch/master/graph/badge.svg)](https://codecov.io/gh/alexleen/log4net-config-editor)
[![BCH compliance](https://bettercodehub.com/edge/badge/alexleen/log4net-config-editor?branch=master)](https://bettercodehub.com/)
![Gitter](https://img.shields.io/gitter/room/alexleen/log4net-config-editor)

A WPF GUI for editing log4net configuration files. This tool can edit log4net configurations saved as XML (\*.xml) or saved inside other appication configuration files (\*.config). Features include:

- An intuitive user interface
- Data validation
- Contextual help
- Links to log4net documentation
- Open log files directly from the UI
- Copy/Paste log4net elements

![](https://github.com/alexleen/log4net-config-editor/blob/master/gifs/demo.gif?raw=true)
## Status
Currently under construction. log4net configuration files can be quite large and complex (hence this editor), so it may take some time to fully support all of the available features. See Support section below for more information.
### Support
#### Appenders
| Appender                      | Supported |
|-------------------------------|-----------|
| AdoNetAppender                |           |
| AnsiColorTerminalAppender     |           |
| AspNetTraceAppender           | ✓         |
| [AsyncForwardingAppender](https://github.com/cjbhaines/Log4Net.Async#asyncforwardingappender)       | ✓         |
| [AWS Appender](https://github.com/aws/aws-logging-dotnet#apache-log4net)   | ✓         |
| BufferingForwardingAppender   | ✓         |
| ColoredConsoleAppender        |           |
| ConsoleAppender               | ✓         |
| DebugAppender                 | ✓         |
| EventLogAppender              | ✓         |
| FileAppender                  | ✓         |
| ForwardingAppender            | ✓         |
| LocalSyslogAppender           | ✓         |
| ManagedColoredConsoleAppender | ✓         |
| MemoryAppender                | ✓         |
| NetSendAppender               | ✓         |
| OutputDebugStringAppender     | ✓         |
| [ParallelForwardingAppender](https://github.com/cjbhaines/Log4Net.Async#parallelforwardingappender) |           |
| RemoteSyslogAppender          | ✓         |
| RemotingAppender              | ✓         |
| RollingFileAppender           | ✓         |
| SmtpAppender                  | ✓         |
| SmtpPickupDirAppender         | ✓         |
| TelnetAppender                | ✓         |
| TextWriterAppender            | ✓         |
| TraceAppender                 | ✓         |
| UdpAppender                   | ✓         |
#### Filters
| Filter            | Supported | Notes                                 |
|-------------------|-----------|---------------------------------------|
| DenyAllFilter     |     ✓     |                                       |
| LevelMatchFilter  |     ✓     |                                       |
| LevelRangeFilter  |     ✓     |                                       |
| LoggerMatchFilter |     ✓     |                                       |
| MdcFilter         |           | Deprecated in favor of PropertyFilter |
| NdcFilter         |           | Deprecated in favor of PropertyFilter |
| PropertyFilter    |     ✓     |                                       |
| StringMatchFilter |     ✓     |                                       |
#### Layouts
| Layout                  | Supported |
|-------------------------|-----------|
| DynamicPatternLayout    |           |
| ExceptionLayout         |           |
| Layout2RawLayoutAdapter |           |
| PatternLayout           | ✓         |
| RawLayoutConverter      |           |
| RawPropertyLayout       |           |
| RawTimeStampLayout      |           |
| RawUtcTimeStampLayout   |           |
| SimpleLayout            | ✓         |
| XmlLayout               |           |
| XmlLayoutBase           |           |
| XmlLayoutSchemaLog4j    |           |
## Thanks!
[JetBrains Rider](https://www.jetbrains.com/rider/)  
[Oxygen Icons](https://github.com/pasnox/oxygen-icons-png)  
[AppVeyor](https://ci.appveyor.com/)  
[Codecov](https://codecov.io/)  
[Better Code](https://bettercodehub.com/)  
[ToastNotifications](https://github.com/rafallopatka/ToastNotifications)