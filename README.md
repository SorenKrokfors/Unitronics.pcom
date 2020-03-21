# Unitronics.pcom
Implements parts of the Unitronics PCOM protocol in .NET Standard 2.0

# History
This library is in production and I have implemented the parts that I have needed, some of the read and write operation classes are implemented but never used. I use MI,XB,MB. It is also possible to read and write to the RTC of the PLC.

# Usage
Create a instance of the CommunicationMessage class,add Read and WriteOperations to the message
when done use GetMessage() to get the byte array to send to the PLC


# PCOMTCPClient
use th PCOMTcpClient to communicate with the PLC.
use SendAndReceive Method to send the byte array to the PLC and get a Byte array to parse the read operations with CommunicationMessage.Parse method.

# Future
Compile a nuget package of the library and publish it on nuget.org
