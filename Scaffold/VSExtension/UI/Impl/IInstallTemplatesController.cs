namespace Flywheel.UI.Impl
{
    public interface IInstallTemplatesController
    {
        string[] TemplateSets { get; }
        string ProjectName { get; }
        void Run();
    }
}