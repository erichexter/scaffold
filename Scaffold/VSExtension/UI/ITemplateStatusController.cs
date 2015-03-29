using System;

namespace Flywheel.Generator
{
	public interface ITemplateStatusController
	{
		void Run(TemplateRunResult[] results);
	}
}