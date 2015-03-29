namespace Flywheel.UI
{
	public interface ITemplateSelectionController
	{
		string[] Templates { get; set; }
		string TemplateSet { get; set; }
		void Run(string modelName, string projectDirectory);
		void TemplateSetSelected();
		void EnterSelected();
		void EscapeSelected();
	}
}