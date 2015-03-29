using System;

namespace Flywheel.UI
{
	public class ProjectConfiguration : IProjectConfiguration
	{
		#region IProjectConfiguration Members

		public string TemplateSetDirectory
		{
            get { return "Scaffolds"; }
			set { throw new NotImplementedException(); }
		}

		#endregion
	}
}