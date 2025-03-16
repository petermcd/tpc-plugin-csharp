using System.Diagnostics;
using Password_Management.Helpers;

namespace Password_Management.Platforms;

public class LinuxSudo : PlatformBase, IPlatformType
{

    private List<Mode> _modes = new List<Mode>();

    public LinuxSudo(string address, int port = 22, Boolean useLogon = false) : base(address, port, useLogon)
    {
        var rootMode = new Mode("root", "sudo su -", "exit", "# ", new List<ResponseTableItem>());
    }
    
    public void Change(Boolean useLogon = false)
    {
        Connect();
        var newPassword = FetchInput("Please enter the new password");
    }
    
    public new void Logon()
    {
        Verify();
    }

    public void Verify()
    {
        Connect();
        Debug.Assert(SshClient != null, nameof(SshClient) + " != null");
        var whoAmIOutput = SshClient.RunCommand("whoami");
        if (whoAmIOutput.Result.Trim() == LogonUsername)
        {
            Console.Out.WriteLine("Successfully logged in");
            Environment.Exit(0);
        }
        else
        {
            Console.Error.WriteLine("Login failed");
            Environment.Exit(1);
        }
    }
}
