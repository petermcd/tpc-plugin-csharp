namespace Password_Management.Helpers;

public readonly struct Mode(
    string name,
    string toCommand,
    string fromCommand,
    string promptRegex,
    List<ResponseTableItem> responseTable)
{
    public string Name { get; } = name;
    public string ToCommand { get; } = toCommand;
    public string FromCommand { get; } = fromCommand;
    public string PromptRegex { get; } = promptRegex;
    public List<ResponseTableItem> ResponseTable { get; } = responseTable;
}