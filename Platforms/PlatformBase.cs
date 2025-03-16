namespace Password_Management.Platforms;

using System.Net.Sockets;
using Renci.SshNet;
using Renci.SshNet.Common;

public class PlatformBase()
{
    private readonly string _address = string.Empty;
    private readonly int _port = 22;
    protected string LogonUsername = string.Empty;
    protected string LogonPassword = string.Empty;
    protected string LocalUsername = string.Empty;
    protected string LocalPassword = string.Empty;
    protected string NewLocalPassword = string.Empty;
    protected Boolean UseLogon = false;
    protected SshClient? SshClient;
    
    protected PlatformBase(string address, int port = 22, Boolean useLogon = false) : this()
    {
        _address = address;
        _port = port;
        UseLogon = useLogon;
    }

    public void Logon()
    {
        Console.Error.WriteLine("Action not implemented.");
        Environment.Exit(1);
    }
    
    public void Reconcile()
    {
        Console.Error.WriteLine("Action not implemented.");
        Environment.Exit(1);
    }
    
    public void PreReconcile()
    {
        Console.Error.WriteLine("Action not implemented.");
        Environment.Exit(1);
    }
    
    protected SshClient Connect()
    {
        LogonUsername = FetchInput("Please enter the username");
        LogonPassword = FetchInput("Please enter the password");
        SshClient = new SshClient(_address, _port, LogonUsername, LogonPassword);
        
        try
        {
            SshClient.Connect();
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case SshAuthenticationException:
                    Console.Error.WriteLine("Could not authenticate to the SSH server.");
                    break;
                case SocketException:
                case SshConnectionException:
                case TimeoutException:
                    Console.Error.WriteLine("Could not connect to the SSH server.");
                    break;
                default:
                    Console.Error.WriteLine("An unknown error occured.");
                    break;
            }
            Environment.Exit(1);
            throw;
        }

        return SshClient;
    }
    
    protected static string FetchInput(string prompt)
    {
        Console.Out.Write($"{prompt}: ");
        var response = Console.In.ReadLine();
        if (string.IsNullOrEmpty(response))
        {
            Console.Out.WriteLine("No input received.");
            Environment.Exit(1);
            return "";
        }

        return response;
    }

    ~PlatformBase()
    {
        if (SshClient != null)
        {
            SshClient.Disconnect();
        }
    }
}