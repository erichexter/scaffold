using System.Collections.Generic;

namespace Flywheel.UI
{
	public interface ITemplateRepository
	{
		IEnumerable<string> GetTemplateSets(string directory);
		IEnumerable<string> GetTemplatesForASet(string projectDirectory, string templateSet);
	}
}