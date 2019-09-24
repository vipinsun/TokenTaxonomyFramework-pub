using System;
using System.Linq;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using log4net;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.Model
{
	public static class ModelManager
	{
		private static ILog _log;
		internal static Taxonomy Taxonomy { get; set; }

		static ModelManager()
		{
			TypePrinters.Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
		
		public static Base GetBaseArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBaseArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<Base>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.BaseTokenTypes
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new Base();
		}

		public static Behavior GetBehaviorArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<Behavior>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.Behaviors
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new Behavior();
		}

		public static BehaviorGroup GetBehaviorGroupArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorGroupArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<BehaviorGroup>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.BehaviorGroups
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new BehaviorGroup();
		}

		public static PropertySet GetPropertySetArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetPropertySetArtifact Symbol: " + symbol);
			if (!string.IsNullOrEmpty(symbol.Id))
			{
				return GetArtifactById<PropertySet>(symbol.Id);
			}

			if (!string.IsNullOrEmpty(symbol.Tooling))
			{
				return Taxonomy.PropertySets
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == symbol.Tooling).Value;
			}

			return new PropertySet();
		}

		public static TemplateFormula GetTemplateFormulaArtifact(ArtifactSymbol formula)
		{
			_log.Info("GetTemplateFormulaArtifact: " + formula);
			if (!string.IsNullOrEmpty(formula.Id))
			{
				return GetArtifactById<TemplateFormula>(formula.Id);
			}

			if (!string.IsNullOrEmpty(formula.Tooling))
			{
				return Taxonomy.TemplateFormulas
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == formula.Tooling).Value;
			}

			return new TemplateFormula();
		}
		
		public static TemplateDefinition GetTemplateDefinitionArtifact(ArtifactSymbol formula)
		{
			_log.Info("GetTemplateDefinitionArtifact: " + formula);
			if (!string.IsNullOrEmpty(formula.Id))
			{
				return GetArtifactById<TemplateDefinition>(formula.Id);
			}

			if (!string.IsNullOrEmpty(formula.Tooling))
			{
				return Taxonomy.TemplateDefinitions
					.SingleOrDefault(e => e.Value.Artifact.ArtifactSymbol.Tooling == formula.Tooling).Value;
			}

			return new TemplateDefinition();
		}

		public static bool AddOrUpdateInMemoryArtifact(ArtifactType type, Any artifact)
		{
			switch (type)
			{
				case ArtifactType.Base:
					var baseType = artifact.Unpack<Base>();
					try
					{
						Taxonomy.BaseTokenTypes.Remove(baseType.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.Tooling, baseType);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + baseType.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.Tooling, baseType);
					}
					return true;
				case ArtifactType.Behavior:
					var behavior = artifact.Unpack<Behavior>();
					try
					{
						Taxonomy.Behaviors.Remove(behavior.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.Tooling, behavior);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + behavior.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.Tooling, behavior);
					}
					return true;
				case ArtifactType.BehaviorGroup:
					var behaviorGroup = artifact.Unpack<BehaviorGroup>();
					try
					{
						Taxonomy.BehaviorGroups.Remove(behaviorGroup.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.Tooling, behaviorGroup);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + behaviorGroup.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.Tooling, behaviorGroup);
					}
					return true;
				case ArtifactType.PropertySet:
					var propertySet = artifact.Unpack<PropertySet>();
					try
					{
						Taxonomy.PropertySets.Remove(propertySet.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.Tooling, propertySet);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + propertySet.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.Tooling, propertySet);
					}
					return true;
				case ArtifactType.TemplateFormula:
					var templateFormula = artifact.Unpack<TemplateFormula>();
					try
					{
						Taxonomy.TemplateFormulas.Remove(templateFormula.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.TemplateFormulas.Add(templateFormula.Artifact.ArtifactSymbol.Tooling, templateFormula);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + templateFormula.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.TemplateFormulas.Add(templateFormula.Artifact.ArtifactSymbol.Tooling, templateFormula);
					}
					return true;
				case ArtifactType.TemplateDefinition:
					var templateDefinition = artifact.Unpack<TemplateDefinition>();
					try
					{
						Taxonomy.TemplateDefinitions.Remove(templateDefinition.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.TemplateDefinitions.Add(templateDefinition.Artifact.ArtifactSymbol.Tooling, templateDefinition);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + templateDefinition.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.TemplateDefinitions.Add(templateDefinition.Artifact.ArtifactSymbol.Tooling, templateDefinition);
					}
					return true;

				default:
					return false;
			}
		}
		
		internal static string GetArtifactFolderNameBySymbol(ArtifactType artifactType, string tooling)
		{
			_log.Info("GetArtifactFolderNameBySymbol: " + artifactType +": " + tooling);
			try
			{
				switch (artifactType)
				{
					case ArtifactType.Base:
						var baseFolder = Taxonomy.BaseTokenTypes.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return baseFolder.Value.Artifact.Name;
					case ArtifactType.Behavior:
						var behaviorFolder = Taxonomy.Behaviors.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return behaviorFolder.Value.Artifact.Name;
					case ArtifactType.BehaviorGroup:
						var behaviorGroupFolder = Taxonomy.BehaviorGroups.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return behaviorGroupFolder.Value.Artifact.Name;
					case ArtifactType.PropertySet:
						var propertySetFolder = Taxonomy.PropertySets.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return propertySetFolder.Value.Artifact.Name;
					case ArtifactType.TemplateFormula:
						var tokenTemplateFolder = Taxonomy.TemplateFormulas.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return tokenTemplateFolder.Value.Artifact.Name;
					case ArtifactType.TemplateDefinition:
						var templateDefinitionFolder = Taxonomy.TemplateDefinitions.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return templateDefinitionFolder.Value.Artifact.Name;
					case ArtifactType.TokenTemplate:
                              return "";
					default:
						throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
				}
			}
			catch (Exception)
			{
				_log.Info("No matching artifact folder of type: " + artifactType + " with symbol: " + tooling);
				return "";
			}
		}

		public static TokenTemplate GetTokenTemplate(TokenTemplateId request)
		{
			var definition = Taxonomy.TemplateDefinitions.SingleOrDefault(e => e.Key == request.DefinitionId)
				.Value;
			if (definition == null) return null;
			var formula = Taxonomy.TemplateFormulas
				.SingleOrDefault(e => e.Key == definition.FormulaReference.Id).Value;
			var t = new TokenTemplate
			{
				Definition = definition,
				Formula = formula
			};
			return t;
		}

		public static T GetArtifactById<T>(string id)
		{
			if (typeof(T) == typeof(Base))
			{
				Taxonomy.BaseTokenTypes.TryGetValue(id, out var tokenBase);
				return (T) Convert.ChangeType(tokenBase, typeof(T));
			}

			if (typeof(T) == typeof(Behavior))
			{
				Taxonomy.Behaviors.TryGetValue(id, out var behavior);
				return (T) Convert.ChangeType(behavior, typeof(T));
			}

			if (typeof(T) == typeof(BehaviorGroup))
			{
				Taxonomy.BehaviorGroups.TryGetValue(id, out var behaviorGroup);
				return (T) Convert.ChangeType(behaviorGroup, typeof(T));
			}

			if (typeof(T) == typeof(PropertySet))
			{
				Taxonomy.PropertySets.TryGetValue(id, out var propSet);
				return (T) Convert.ChangeType(propSet, typeof(T));
			}

			if (typeof(T) == typeof(TemplateFormula))
			{
				Taxonomy.TemplateFormulas.TryGetValue(id, out var formula);
				return (T) Convert.ChangeType(formula, typeof(T));
			}

			if (typeof(T) != typeof(TemplateDefinition)) return (T) Convert.ChangeType(new object(), typeof(T));
			Taxonomy.TemplateDefinitions.TryGetValue(id, out var definition);
			return (T) Convert.ChangeType(definition, typeof(T));
		}
	}
}