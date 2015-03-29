using System;
using System.Collections.Generic;

namespace Flywheel
{
	[Serializable]
	public class ModelType
	{
		private readonly List<ModelProperty> _properties = new List<ModelProperty>();
		public string Name;
		public string Namespace;
	    private List<ModelProperty> _methods= new List<ModelProperty>();
	    private List<string> _bases=new List<string>();

	    public List<ModelProperty> Properties
		{
			get { return _properties; }
		}

        public List<ModelProperty> Methods
        {
            get { return _methods; }
        }

	    public List<string> Bases
	    {
	        get {
	            return _bases;
	        }
	    }
	}
}