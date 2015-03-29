namespace Flywheel.Generator
{
	public interface IScaffoldingGenerator
	{
		IScaffoldingGenerator ForModelSelectedInTheCodeWindow();
		IScaffoldingGenerator SelectTemplate();
		IScaffoldingGenerator Generate();
		IScaffoldingGenerator ForModelSelectedInTheSolutionExplorer();
		IScaffoldingGenerator DisplayResults();
	}
}