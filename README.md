# log4net-config-editor
A WPF GUI for editing log4net configuration files.
## Status
Currently under construction. log4net configuration files can be quite large and complex (hence this editor), so it may take some time to fully support all of the available features. See Support section below for more information.
### Support
#### Appenders
| Appender                      | Supported |
|-------------------------------|-----------|
| AdoNetAppender                |           |
| AnsiColorTerminalAppender     |           |
| AspNetTraceAppender           |           |
| BufferingForwardingAppender   |           |
| ColoredConsoleAppender        |           |
| ConsoleAppender               | ✓         |
| DebugAppender                 |           |
| EventLogAppender              |           |
| FileAppender                  | ✓         |
| FileAppender.ExclusiveLock    |           |
| FileAppender.InterProcessLock |           |
| FileAppender.MinimalLock      |           |
| ForwardingAppender            |           |
| LocalSyslogAppender           |           |
| ManagedColoredConsoleAppender |           |
| MemoryAppender                |           |
| NetSendAppender               |           |
| OutputDebugStringAppender     |           |
| RemoteSyslogAppender          |           |
| RemotingAppender              |           |
| RollingFileAppender           |           |
| SmtpAppender                  |           |
| SmtpPickupDirAppender         |           |
| TelnetAppender                |           |
| TextWriterAppender            |           |
| TraceAppender                 |           |
| UdpAppender                   |           |
#### Filters
| Filter            | Supported | Notes                                 |
|-------------------|-----------|---------------------------------------|
| DenyAllFilter     |     ✓     |                                       |
| LevelMatchFilter  |     ✓     |                                       |
| LevelRangeFilter  |     ✓     |                                       |
| LoggerMatchFilter |     ✓     |                                       |
| MdcFilter         |           | Deprecated in favor of PropertyFilter |
| NdcFilter         |           | Deprecated in favor of PropertyFilter  |
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