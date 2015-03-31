using System.Collections.Generic;

namespace Flywheel.Generator
{
	public interface IUserInterface
	{
		void DisplayResults(TemplateRunResult[] results);
		ScaffoldSelection DisplayTemplateSelection(string projectDirectory, string modelName);
	}
}