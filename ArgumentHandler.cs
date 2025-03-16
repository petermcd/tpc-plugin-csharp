using System.Text;

namespace Password_Management;

using System.CommandLine;
using Platforms;

public class ArgumentHandler
{
    public ArgumentHandler(string[] args)
    {
        var useLogonOption = new Option<Boolean>
        (
            name: "--use-logon",
            description: "Require a logon account",
            getDefaultValue: () => false
        );
        
        var platformTypeOption = new Option<string>
        (
            name: "--platform-type",
            description: "Platform type"
        ).FromAmong("LinuxSu", "LinuxSudo");
        platformTypeOption.IsRequired = true;
        
        var addressOption = new Option<string>
        (
            name: "--address",
            description: "Address of the endpoint device"
        ) {IsRequired = true};
        
        var portOption = new Option<int>
        (
            name: "--port",
            description: "SSH Port",
            getDefaultValue: () => 22
        );

        var rootCommand = new RootCommand();
        
        var verifyCommand = new Command(name: "verify", description: "Verify a password.");
        rootCommand.AddCommand(verifyCommand);
       
        var changeCommand = new Command(name: "change", description: "Change a password.");
        changeCommand.AddOption(useLogonOption);
        rootCommand.AddCommand(changeCommand);
        
        var logonCommand = new Command(name: "logon", description: "Verify logon password.");
        rootCommand.AddCommand(logonCommand);
        
        var reconcileCommand = new Command(name: "reconcile", description: "Reconcile an account.");
        rootCommand.AddCommand(reconcileCommand);
        
        var preReconcileCommand = new Command(name: "pre-reconcile", description: "Perform preparation work prior to reconcile.");
        rootCommand.AddCommand(preReconcileCommand);
        
        portOption.AddValidator(result =>
            {
                if (result.GetValueForOption(portOption) < 1 || result.GetValueForOption(portOption) > 65535)
                {
                    result.ErrorMessage = "The port must be an number between 1 and 65535.";
                }
            });
        rootCommand.AddGlobalOption(platformTypeOption);
        rootCommand.AddGlobalOption(addressOption);
        rootCommand.AddGlobalOption(portOption);

        verifyCommand.SetHandler((platformType, addressOptionValue, portOptionValue) =>
            {
                var platformTypeType = Type.GetType(new StringBuilder().Append("Password_Management.Platforms.")
                    .Append(platformType)
                    .ToString());
                if (platformTypeType is null)
                {
                    Console.Error.WriteLine(new StringBuilder().Append("Unknown platform type ")
                        .Append(platformType)
                        .Append('.')
                        .ToString());
                    return;
                }
                var controller = (IPlatformType)Activator.CreateInstance(platformTypeType, addressOptionValue, portOptionValue, false)!;
                controller.Verify();
            },
            platformTypeOption, addressOption, portOption);

        changeCommand.SetHandler((platformType, addressOptionValue, portOptionValue, useLogonOptionValue) =>
            {
                var platformTypeType = Type.GetType(new StringBuilder().Append("Password_Management.Platforms.")
                    .Append(platformType)
                    .ToString());
                if (platformTypeType is null)
                {
                    Console.Error.WriteLine(new StringBuilder().Append("Unknown platform type ")
                        .Append(platformType)
                        .Append('.')
                        .ToString());
                    Environment.Exit(1);
                    return;
                }
                var controller = (IPlatformType)Activator.CreateInstance(platformTypeType, addressOptionValue, portOptionValue, useLogonOptionValue)!;
                controller.Change(useLogon: useLogonOptionValue);
            },
            platformTypeOption, addressOption, portOption, useLogonOption);
        try
        {
            rootCommand.Invoke(args);
        }
        catch (InvalidOperationException)
        {
            Console.Error.WriteLine("The application failed unexpectedly.");
            Environment.Exit(1);
            throw;
        }
    }
}