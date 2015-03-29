using System.Collections.Generic;
using System.IO;
using EnvDTE;
using System.Linq;
namespace Flywheel.VSHelpers
{
	public static class CodeTypeExtensions
	{
		public static string ProjectNamespace(this CodeType codeType)
		{
			return codeType.ProjectItem.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString();
		}


		public static string ProjectDirectory(this CodeType codeType)
		{
			string path = Path.GetDirectoryName(codeType.ProjectItem.ContainingProject.FileName);
			if (!path.EndsWith("\\"))
				path += "\\";
			return path;
		}

		public static ModelType ModelType(this CodeType codeType)
		{
			var modeltype = new ModelType();
			modeltype.Name = codeType.Name;
			modeltype.Namespace = codeType.Namespace.FullName;
		    modeltype.Bases.AddRange( FindBases(codeType.Bases.Cast<CodeType>()));
			AddChildProperties(modeltype, codeType);
			return modeltype;
		}

        private static IEnumerable<string> FindBases(IEnumerable<CodeType> enumerable)
        {
            foreach (var basename in enumerable)
            {
                yield return basename.Name;
                foreach (var childbase in FindBases(basename.Bases.Cast<CodeType>()))
                {
                    yield return childbase;
                }

            }
        }


	    private static void AddChildProperties(ModelType modeltype, CodeType codeType)
		{
			foreach (CodeElement member in GetMembersAndBaseMembers(codeType))
			{
				if (member.Kind == vsCMElement.vsCMElementProperty)
				{
					var prop = (CodeProperty) member;
					modeltype.Properties.Add(new ModelProperty
					                         	{
					                         		Name = prop.Name,
                                                    
					                         		ModelType =
					                         			new ModelType {Name = prop.Type.AsString, Namespace = prop.Type.AsFullName}
					                         	});
				}
                else if (member.Kind == vsCMElement.vsCMElementFunction)
                {
                    var prop = (CodeFunction)member;
                    var property = new ModelProperty
                                       {
                                           Name = prop.Name,

                                           ModelType =
                                               new ModelType { Name = prop.Type.AsString, Namespace = prop.Type.AsFullName }
                                       };
                    if (prop.Type.TypeKind == vsCMTypeRef.vsCMTypeRefArray)
                        property.ModelType.Bases.AddRange(FindBases(prop.Type.ElementType. CodeType.Bases.Cast<CodeType>()));
                    else if (prop.Type.TypeKind == vsCMTypeRef.vsCMTypeRefCodeType)
                        property.ModelType.Bases.AddRange(FindBases(prop.Type.CodeType.Bases.Cast<CodeType>()));

                    modeltype.Methods.Add(property);
                }
            }
		}

	    private static IEnumerable<CodeElement> GetMembersAndBaseMembers(CodeType codeType)
	    {
	        return codeType.Members.Cast<CodeElement>().Union<CodeElement>(codeType.Bases.Cast<CodeType>().SelectMany<CodeType,CodeElement>(type => type.Members.Cast<CodeElement>()) );
	    }
	}
}