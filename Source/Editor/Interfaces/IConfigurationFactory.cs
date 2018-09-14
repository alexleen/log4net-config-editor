
namespace Editor.Interfaces
{
    public interface IConfigurationFactory
    {
        IConfigurationXml Create(string filename);
    }
}
