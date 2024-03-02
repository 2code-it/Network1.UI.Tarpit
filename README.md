# Network1.UI.Tarpit
UI elements for configuring and running a tarpit service.
[Code2.Net.TcpTarpit](https://github.com/2code-it/Code2.Net.TcpTarpit)

## Network1.UI.Tarpit.WinApp
Windows WPF app to run and configure a tarpit service with logging.


### Options
**View options** 
- Max connection entries
Maximum connections to display
- Max log entries
Maximum app log entries to display

**Tarpit options**
- Listen address
Listeners address, ipv4 or ipv6
- Ports
Ports specification, can be a range like 10-99 or individual ports, or both seperated by comma
- UseIPv4Only
Enable to use ipv4 only
- Write interval
Interval in milliseconds to write the specified (write size) amount of bytes
- Write size
Amount of bytes to write per write interval
- Update interval
Interval in seconds to receive connection updates
- Connection timeout
Timeout in seconds at which the connection is aborted
- Response file
Relative or full path to file which contains response data
- Response text
Response as plain text

**Connection log options**
- Enabled
Enable or disable connection logging
- Dateformat
Dateformat to use to format connection created date (yyyy-MM-ddTHH:mm:ss)
- Value delimiter
Delimiter char to seperate the values
- Directory path
Relative or full path to a log storage directory
- Filename prefix
Optional log file name prefix
- Filename date format
Filename suffix date format to store log lines per day (yyyyMMdd) or month (yyyyMM), etc

**Log file header**
created, source-endpoint, destination-endpoint, duration-seconds, bytes-sent