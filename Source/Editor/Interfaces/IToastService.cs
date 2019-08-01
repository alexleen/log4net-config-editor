namespace Editor.Interfaces
{
    public interface IToastService
    {
        void ShowError(string message);
        void ShowInformation(string message);
        void ShowSuccess(string message);
        void ShowWarning(string message);
    }
}
