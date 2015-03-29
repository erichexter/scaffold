using System.Collections.Generic;

namespace Flywheel.Generator
{
	public class TemplateRunResult
	{
	    public int BuildAction{ get; set;}
	    public RunResult Result { get; set; }
		public string TemplateFilename { get; set; }
		public List<Error> Errors { get; set; }
		public List<Error> Warnings { get; set; }
		public string OutputFilename { get; set; }
	}
}