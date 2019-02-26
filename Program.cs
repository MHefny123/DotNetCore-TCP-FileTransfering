using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace FileTransfering
{
class Program
{

//settings
bool mode;//mode [true send] [false receive]
string output_PATH = @"Download\";//files gotten
int BytesPerRead = 1024;//bytes to read and then write

static void Main(string[] args)
{
Console.WriteLine("Hello World!");
Program p =new Program();
p.START();
}

public void START()//ENTRYPOINT
{
Console.Clear();
Console.Title = "File Transfer TCP/IP";
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.BackgroundColor = ConsoleColor.Black;

if (!Directory.Exists(output_PATH))
Directory.CreateDirectory(output_PATH);

SET_SETTING_MODE();
if (mode)
{
SendFile();
}
else
{
ReciveFile();
}
}


int Get_port()
{
AGEN:
Console.Clear();
Console.WriteLine(">Port?");
Console.Write("<");
string input = Console.ReadLine();
try
{
int port = Int32.Parse(input);
if (port <= 65534 && port > 1204)
{
return port;
}
else goto AGEN;
}
catch
{
goto AGEN;
}
}//gets user input and validates it as a port

private string _getip()
{
IPHostEntry host;
string localIP = "?";
host = Dns.GetHostEntry(Dns.GetHostName());
foreach (IPAddress ip in host.AddressList)
{
if (ip.AddressFamily == AddressFamily.InterNetwork)
{
localIP = ip.ToString();
}
}
return localIP;
}//gets this machine ip

private void SET_SETTING_MODE()
{
FIRST:
Console.Clear();
Console.WriteLine(">Do you whant to recive or send files [r/s]");
Console.Write("<");
string input = Console.ReadLine();
if (input.StartsWith("r"))
{
mode = false;
}
else if (input.StartsWith("s"))
{
mode = true;
}
else
goto FIRST;
}

void SendFile()
{
top:
Console.Clear();
Console.WriteLine(">FILE to upload?");
Console.Write("<");
string file = Console.ReadLine();
if (File.Exists(file))
{
AGEN:
Console.Clear();
Console.WriteLine(">IP to send to");
Console.Write("<");
string ip = Console.ReadLine();
try
{
IPAddress.Parse(ip);
}
catch
{
goto AGEN;
}
int port = Get_port();

//sending file
Console.Clear();
Console.WriteLine(">Connecting");
TcpClient soc = new TcpClient(ip, port);
Console.WriteLine(">Connected");
Console.WriteLine(">Sending File");
Console.WriteLine(">" + file);
Console.WriteLine(">To ip:" + ip + " port:" + port);
soc.Client.SendFile(file);
Console.WriteLine(">Done");
Console.WriteLine(">Closeing port");
soc.Close();
Console.WriteLine(">Done");

}
else
goto top;
}//send file to ip (tcp/ip)

void ReciveFile()
{

int port = Get_port();
Console.WriteLine(">Getting ip");
string ip = _getip();
Console.WriteLine(">Opening port:" + port);
TcpListener listener = new TcpListener(IPAddress.Any,100);//bypass Compiler
try
{
listener = new TcpListener(IPAddress.Parse(ip), port);
}
catch
{
Console.WriteLine(">ERRROR failed to open port");
Console.ReadLine();
Environment.Exit(0);
}
Console.WriteLine(">Port open");
Console.WriteLine(">Waiting for connection on");
Console.WriteLine(">ip :" + ip);
Console.WriteLine(">port:" + port);

//wait for sender
listener.Start();
while (true)
{

if (listener.Pending())
{
break;
}
}

// here you receive the file
using (var client = listener.AcceptTcpClient())
using (var stream = client.GetStream())
using (var output = File.Create(output_PATH + @"\Data"))
{

Console.WriteLine(">connected :" + client.Client.LocalEndPoint.ToString());
Console.WriteLine(">reciving Data");

// read the file in chunks of 1KB (as default)
var buffer = new byte[BytesPerRead];
int bytesRead;
while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
{
output.Write(buffer, 0, bytesRead);
}
}

Console.WriteLine(">File recived");
Console.WriteLine(">Closeing port");
listener.Stop();
Console.WriteLine(">Data at");
Console.WriteLine(">" + output_PATH + @"\Data");
Console.ReadLine();
}//reciver file from (SendFile) (tcp/ip)
}





}

