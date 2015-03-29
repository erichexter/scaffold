using System;
using VSLangProj;

namespace Flywheel.Generator
{
	public interface IVisualStudioNewItemAttacher
	{
		bool AddFileToProject(string filename);
	    //bool AddFileToProject(string filename, prjBuildAction action);
	}
}