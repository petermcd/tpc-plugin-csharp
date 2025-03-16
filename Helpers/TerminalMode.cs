namespace Password_Management.Helpers;

using Renci.SshNet;

public class TerminalMode : IDisposable
{
    private SshClient _sshClient;
    private Mode _modeInfo;
    
    public TerminalMode(SshClient sshClient, Mode modeInfo)
    {
        _modeInfo = modeInfo;
        _sshClient = sshClient;
    }
    public void Dispose()
    {
        _sshClient.RunCommand(Mode.FromCommand) ;
    }
}