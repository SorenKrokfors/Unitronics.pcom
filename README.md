# Unitronics.pcom
Implements parts of the Unitronics PCOM protocol in .NET Standard 2.0

#Usage
Create the CommunicationMessage and add Read and WriteOperations to the message
when done use GetMessage() to get the byte array to send to the PLC


#PCOMTCPclient
use th PCOMTcpClient to communicate with the PLC.
use SendAndReceive Method to send the byte array to the PLC and get a Byte array to parse back.
