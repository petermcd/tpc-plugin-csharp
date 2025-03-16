namespace Password_Management.Platforms;

public class LinuxSu(string address, int port = 22, Boolean useLogon = false) : PlatformBase(address, port, useLogon), IPlatformType
{

    public void Change(Boolean useLogon = false)
    {
        Connect();
        var newPassword = FetchInput("Please enter the new password");
    }

    public void Verify()
    {
        var client = Connect();
        var whoAmIOutput = client.RunCommand("whoami");
        if (whoAmIOutput.Result.Trim() == this.LogonUsername)
        {
            Console.Out.WriteLine("Success logged in");
            Environment.Exit(0);
        }
        else
        {
            Console.Error.WriteLine("Login failed");
            Environment.Exit(1);
        }
        client.Disconnect();
    }
}
