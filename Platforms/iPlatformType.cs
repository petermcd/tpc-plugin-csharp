namespace Password_Management.Platforms;

public interface IPlatformType
{
    public void Change(Boolean useLogon = false);
    public void Logon();
    public void Reconcile();
    public void PreReconcile();
    public void Verify();
}