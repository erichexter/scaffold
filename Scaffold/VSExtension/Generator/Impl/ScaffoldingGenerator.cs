using System;
using System.Collections.Generic;
using EnvDTE;
using Flywheel.VSHelpers;

namespace Flywheel.Generator
{
	public class ScaffoldingGenerator : IScaffoldingGenerator
	{
		private readonly CodeWindowSelector _codeWindowSelector;
		private readonly SolutionExplorerSelector _solutionExplorerSelector;
		private readonly List<string> _templateFilenames = new List<string>();
		private readonly ITemplateRunner _templateRunner;
		private readonly IUserInterface _userInterface;
		private CodeType _model;
		private ModelType _modelType;
		private TemplateRunResult[] _results;

		public ScaffoldingGenerator(CodeWindowSelector codeWindowSelector, SolutionExplorerSelector solutionExplorerSelector,
		                            ITemplateRunner templateRunner, IUserInterface userInterface)
		{
			_codeWindowSelector = codeWindowSelector;
			_solutionExplorerSelector = solutionExplorerSelector;
			_templateRunner = templateRunner;
			_userInterface = userInterface;
		}

		public IScaffoldingGenerator ForModelSelectedInTheCodeWindow()
		{
			_model = _codeWindowSelector.GetSelectedClass();
			_modelType = _model.ModelType();
			return this;
		}

		public IScaffoldingGenerator SelectTemplate()
		{
			_templateFilenames.Clear();
			_templateFilenames.AddRange(_userInterface.DisplayTemplateSelection(_model.ProjectDirectory(), _modelType.Name));
			return this;
		}

		public IScaffoldingGenerator Generate()
		{
			_results = _templateRunner.RunTemplates(_templateFilenames, _modelType, _model);
			return this;
		}
		public IScaffoldingGenerator ForModelSelectedInTheSolutionExplorer()
		{
			_model = _solutionExplorerSelector.GetSelectedClass();
			_modelType = _model.ModelType();
			return this;
		}

		public IScaffoldingGenerator DisplayResults()
		{
			if(_results.Length>0)
				_userInterface.DisplayResults(_results);
			return this;
		}
	}
}