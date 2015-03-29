//using Scaffolding.Exporter;
using Flywheel.Generator;
using Flywheel.UI.Impl;
using Flywheel.VSHelpers;

namespace Flywheel
{
	public interface IIoC
	{
		TInterface GetService<TInterface>();
		IShouldContextMenu CreateShouldContextMenu();
		IScaffoldingGenerator CreateScaffoldingGenerator();
	    IInstallTemplatesController CreateInstallTemplatesController();
//	    ExportGenerator CreateExportGenerator();
	}
}