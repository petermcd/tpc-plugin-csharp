namespace Password_Management.Helpers;

public readonly struct ResponseTableItem(string responseRegex, string value)
{
    private string ResponseRegex { get; } = responseRegex;
    private string Value { get; } = value;
}