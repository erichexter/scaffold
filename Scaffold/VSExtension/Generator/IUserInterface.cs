using System.Collections.Generic;

namespace Flywheel.Generator
{
	public interface IUserInterface
	{
		void DisplayResults(TemplateRunResult[] results);
		IEnumerable<string> DisplayTemplateSelection(string projectDirectory, string modelName);
	}
}