using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold
{
    public class ScaffoldModel
    {
        public string ProjectNamespace { get; set; }
        public ModelType Model { get; set; }
    }

    public class ModelType
    {
        private readonly List<ModelProperty> _properties = new List<ModelProperty>();
        public string Name;
        public string Namespace;
        private List<ModelProperty> _methods = new List<ModelProperty>();
        private List<string> _bases = new List<string>();

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
            get
            {
                return _bases;
            }
        }
    }
    public class ModelProperty
    {
        public ModelType ModelType;
        public string Name;
    }
}

public class ScaffoldConfiguration
{
    public string name { get; set; }
    public Template[] templates { get; set; }
}

public class Template
{
    public string name { get; set; }
    public string destination { get; set; }
}
