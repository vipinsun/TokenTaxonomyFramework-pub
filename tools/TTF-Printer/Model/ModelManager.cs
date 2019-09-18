using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using log4net;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.Model
{
	public class ModelManager
	{
		private static ILog _log;
		private static Model.Taxonomy Taxonomy { get; set; }

		public ModelManager(Model.Taxonomy taxonomy)
		{
			TypePrinters.Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			Taxonomy = taxonomy;
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
		
		

		public static TokenSpecification GetTokenSpecification(TokenTemplateId symbol)
		{
			_log.Info("Generating Token Specification from Token Token Template Id: " + symbol);
			var definition = GetTemplateDefinitionArtifact(new ArtifactSymbol
			{
				Id = symbol.DefinitionId
			});
	
			if (definition == null) return new TokenSpecification
			{
				SpecificationHash = "Token Specification for Template Definition: " + symbol.DefinitionId + " not found."
			};
			var formula = GetTemplateFormulaArtifact(new ArtifactSymbol
			{
				Id = definition.FormulaReference.Id
			});
			
			var spec = BuildSpecification(definition);
			if (formula.TemplateType != TemplateType.Hybrid) return spec;
			foreach (var c in definition.ChildTokens)
			{
				spec.ChildTokens.Add(BuildSpecification(c));
			}
			return spec;
		}

		private static TokenSpecification BuildSpecification(TemplateDefinition definition)
		{
			var formula = GetTemplateFormulaArtifact(new ArtifactSymbol
			{
				Id = definition.FormulaReference.Id
			});
			
			var validated = ValidateDefinition(formula, definition);
			if (!string.IsNullOrEmpty(validated))
			{
				_log.Error(validated);
				return new TokenSpecification
				{
					Artifact = new Artifact.Artifact
					{
						Name = validated
					}
				};
			}
			
			var retVal = new TokenSpecification
			{
				Artifact = definition.Artifact
			};
			retVal.Artifact.ArtifactSymbol.Type = ArtifactType.TokenTemplate;
			
			retVal.TokenBase = MergeBase(definition);
			
			var behaviorGroups = definition.BehaviorGroups.Select(tb => GetArtifactById<BehaviorGroup>(tb.Reference.Id)).ToList();
			foreach (var bg in definition.BehaviorGroups)
			{
				var behaviorGroup = behaviorGroups.SingleOrDefault(e => e.Artifact.ArtifactSymbol.Id == bg.Reference.Id);
				if (behaviorGroup == null) continue;
				
				var behaviorGroupSpec = new BehaviorGroupSpecification();
				foreach (var b in behaviorGroup.Behaviors)
				{
					var behavior = GetArtifactById<Behavior>(b.Reference.Id);
					if (behavior == null) continue;
					behaviorGroupSpec.Behaviors.Add(behavior.Artifact.ArtifactSymbol);
				}
				retVal.BehaviorGroups.Add(behaviorGroupSpec);
			}
			
			var propertySets = definition.PropertySets.Select(tb => GetArtifactById<PropertySet>(tb.Reference.Id)).ToList();
			var mergedPs = (from ps in definition.PropertySets let propertySet = propertySets.SingleOrDefault(e => e.Artifact.ArtifactSymbol.Id == ps.Reference.Id) 
				where propertySet != null select MergePropertySet(propertySet, ps)).ToList();

			var behaviors = definition.Behaviors.Select(tb => GetArtifactById<Behavior>(tb.Reference.Id)).ToList();
			var behaviorReferences = definition.Behaviors;
			foreach (var bgb in definition.BehaviorGroups)
			{
				foreach (var b in bgb.BehaviorArtifacts)
				{
					behaviors.Add(GetArtifactById<Behavior>(b.Reference.Id));
					behaviorReferences.Add(b);
				}
			}

			var (behaviorSpecifications, propSets) = BuildBehaviorSpecs(behaviors, behaviorReferences, mergedPs);
				
			retVal.Behaviors.AddRange(behaviorSpecifications);
			retVal.PropertySets.AddRange(propSets);
			retVal.SpecificationHash = TypePrinters.Utils.CalculateSha3Hash(retVal.ToByteString().ToBase64());
			return retVal;
		}

		private static (IEnumerable<BehaviorSpecification>, IEnumerable<PropertySetSpecification>) BuildBehaviorSpecs
			(IEnumerable<Behavior> behaviors, IEnumerable<BehaviorReference> references, IEnumerable<PropertySet> propertySets)
		{
			//check for invocationBindings
			var behaviorReferences = references.ToList();
			var propSets = propertySets.ToList();
			var behaviorsList = behaviors.ToList();

			var influencedBehaviors = 
				new List<(BehaviorReference, Behavior, BehaviorReference)>(); //influencingReference, influencedArtifact, influencedReference
			var influencedPropertySets = new List<(BehaviorReference, PropertySet)>(); //influencingReference, influencedArtifact

			//getting all the influenced behaviors matched with their influences
			foreach (var b in behaviorReferences)
			{
				if(b.InfluenceBindings.Count == 0)
					continue;
				foreach (var ib in b.InfluenceBindings)
				{
					var inb = behaviorsList.SingleOrDefault(e =>
						e.Artifact.ArtifactSymbol.Id == ib.InfluencedId);
					if (inb != null)
					{
						var influenced =
							behaviorReferences.SingleOrDefault(e => e.Reference.Id == ib.InfluencedId);
						if (influenced != null)
						{
							var influencedInvocation =
								behaviorsList.SingleOrDefault(e=>e.Artifact.ArtifactSymbol.Id == influenced.Reference.Id)?.Invocations
									.SingleOrDefault(e => e.Id == ib.InfluencedInvocationId);
							ib.InfluencedInvocation = influencedInvocation;
						}

						influencedBehaviors.Add((b, inb, influenced));
						continue;
					}

					var inp = propSets.SingleOrDefault(e => e.Artifact.ArtifactSymbol.Id == ib.InfluencedId);
					if(inp!= null)
						influencedPropertySets.Add((b, inp));
				}
			}

			//removing influenced behaviors and properties from straight merges into specs.
			foreach (var ib in influencedBehaviors)
			{
				behaviorReferences.Remove(ib.Item3);
			}

			foreach (var ibp in influencedPropertySets)
			{
				propSets.Remove(ibp.Item2);
			}
			
			var behaviorList = behaviorsList.ToList();

			var behaviorSpecs = (from b in behaviorReferences let behavior 
				= behaviorList.SingleOrDefault(e => e.Artifact.ArtifactSymbol.Id == b.Reference.Id) 
				select GetBehaviorSpecification(behavior, b)).ToList();
			
			foreach (var (influencingReference, influencedBehavior, influencedReference) in influencedBehaviors)
			{
				var behaviorSpec = new BehaviorSpecification
				{
					Constructor = influencedReference.Constructor,
					ConstructorType = influencedReference.ConstructorType,
					IsExternal = influencedReference.IsExternal,
					Artifact = influencedBehavior.Artifact,
					Properties = {influencedBehavior.Properties}
				};

				var invocationsCovered = new List<string>();
				foreach (var i in influencingReference.InfluenceBindings)
				{
					invocationsCovered.Add(i.InfluencedInvocationId);
					var invokeBinding = new InvocationBinding
					{
						Influence = new InvocationBinding.Types.Influence
						{
							InfluencedId = i.InfluencedId,
							InfluencedInvocationId = i.InfluencedInvocationId,
							InfluencingId = influencingReference.Reference.Id,
							InfluencingInvocationId = i.InfluencingInvocation.Id,
							InfluenceType = i.InfluenceType
						}
					};
					if (i.InfluenceType == InfluenceType.Intercept)
					{
						invokeBinding.InvocationStep = new InvocationBinding.Types.InvocationStep
						{
							Invocation = i.InfluencingInvocation,
							NextInvocation = new InvocationBinding.Types.InvocationStep
							{
								Invocation = i.InfluencedInvocation
							}
						};
					}
					else
					{
						invokeBinding.InvocationStep = new InvocationBinding.Types.InvocationStep
						{
							Invocation = i.InfluencingInvocation
						};
					}
					behaviorSpec.Invocations.Add(invokeBinding);
				}
				behaviorSpecs.Add(behaviorSpec);
				
				foreach (var ib in influencedBehavior.Invocations)
				{
					if (invocationsCovered.Contains(ib.Id)) continue;
					var invokeBinding = new InvocationBinding
					{
						InvocationStep = new InvocationBinding.Types.InvocationStep
						{
							Invocation = ib
						}
					};
					behaviorSpec.Invocations.Add(invokeBinding);
				}
			}
			
			var propSetSpecs = new List<PropertySetSpecification>();
			foreach (var (behaviorReference, propertySet) in influencedPropertySets)
			{
				var propSetSpec = new PropertySetSpecification
				{
					Artifact = propertySet.Artifact
				};
				foreach (var i in behaviorReference.InfluenceBindings)
				{
					foreach (var p in propertySet.Properties)
					{
						var propSpec = CreatePropertySpec(p);
						foreach (var ip in p.PropertyInvocations)
						{
							if (ip.Id == i.InfluencedInvocationId)
							{
								var invokeBinding = new InvocationBinding
								{
									Influence = new InvocationBinding.Types.Influence
									{
										InfluencedId = i.InfluencedId,
										InfluencedInvocationId = i.InfluencedInvocationId,
										InfluencingId = behaviorReference.Reference.Id,
										InfluencingInvocationId = i.InfluencingInvocation.Id,
										InfluenceType = i.InfluenceType
									}
								};
								if (i.InfluenceType == InfluenceType.Intercept)
								{
									invokeBinding.InvocationStep = new InvocationBinding.Types.InvocationStep
									{
										Invocation = i.InfluencingInvocation,
										NextInvocation = new InvocationBinding.Types.InvocationStep
										{
											Invocation = i.InfluencedInvocation
										}
									};
								}
								else
								{
									invokeBinding.InvocationStep = new InvocationBinding.Types.InvocationStep
									{
										Invocation = i.InfluencingInvocation
									};
								}
								propSpec.PropertyInvocations.Add(invokeBinding);
							}
							else //property in the set that is not influenced
							{
								var invokeBinding = new InvocationBinding
								{
									InvocationStep = new InvocationBinding.Types.InvocationStep
									{
										Invocation = ip
									}
								};
								propSpec.PropertyInvocations.Add(invokeBinding);
							}
							propSetSpec.Properties.Add(propSpec);
						}
					}
					propSetSpecs.Add(propSetSpec);
				}
			}
			return (behaviorSpecs, propSetSpecs);
		}

		private static PropertySpecification CreatePropertySpec(Property p)
		{
			var propSpec = new PropertySpecification
			{
				Name = p.Name,
				TemplateValue = p.TemplateValue,
				ValueDescription = p.ValueDescription
			};
			if (p.Properties.Count == 0)
				return propSpec;
			foreach (var pp in p.Properties)
			{
				propSpec = CreatePropertySpec(pp);
			}

			return propSpec;
		}

		private static PropertySet MergePropertySet(PropertySet ps, PropertySetReference psr)
		{
			foreach (var i in ps.Properties)
			{
				foreach (var m in psr.Properties)
				{
					if (i.Name == m.Name)
						i.MergeFrom(m);
				}
			}
			
			return ps;
		}

		private static BehaviorSpecification GetBehaviorSpecification(Behavior behavior, BehaviorReference behaviorReference)
		{
			var mutableBehaviors = behavior.Clone();
			foreach (var i in mutableBehaviors.Invocations)
			{
				foreach (var m in behaviorReference.Invocations)
				{
					if (i.Id != m.Id) continue;
					//i.MergeFrom(m);
					i.Description = m.Description;
					i.Request = m.Request;
					i.Response = m.Response;
					i.Name = m.Name;
				}
			}

			foreach (var p in mutableBehaviors.Properties)
			{
				foreach (var m in behaviorReference.Properties)
				{
					if (p.Name == m.Name)
						p.MergeFrom(m);
				}
			}

			var behaviorSpec = new BehaviorSpecification
			{
				Constructor = behaviorReference.Constructor,
				ConstructorType = behaviorReference.ConstructorType,
				IsExternal = behaviorReference.IsExternal,
				Artifact = mutableBehaviors.Artifact,
				Properties = {mutableBehaviors.Properties},
			};
			
			foreach (var ib in mutableBehaviors.Invocations)
			{
				var invokeBinding = new InvocationBinding
				{
					InvocationStep = new InvocationBinding.Types.InvocationStep
					{
						Invocation = ib
					}
				};
				behaviorSpec.Invocations.Add(invokeBinding);
			}
			
			return behaviorSpec;
		}

		private static Base MergeBase(TemplateDefinition validated)
		{
			var baseToken = GetArtifactById<Base>(validated.TokenBase.Reference.Id);
			baseToken.Artifact.Maps.MergeFrom(validated.Artifact.Maps);
			baseToken.Decimals = validated.TokenBase.Decimals;
			baseToken.Name = validated.TokenBase.Name;
			baseToken.Owner = validated.TokenBase.Owner;
			baseToken.Quantity = validated.TokenBase.Quantity;
			baseToken.Symbol = validated.TokenBase.Symbol;
			baseToken.ConstructorName = validated.TokenBase.ConstructorName;
			return baseToken;
		}

		private static string ValidateDefinition(TemplateFormula formula, TemplateDefinition definition)
		{
			if (definition.TokenBase.Reference.Id != formula.TokenBase.Base.Id)
			{
				return "Error validating definition id: " + definition.Artifact.ArtifactSymbol.Id
				                                          + " against its template id: " +
				                                          formula.Artifact.ArtifactSymbol.Id
				                                          + " the base token does not match";
			}

			foreach (var b in definition.Behaviors)
			{
				var tb = formula.Behaviors.SingleOrDefault(e => e.Behavior.Id == b.Reference.Id);
				if (tb == null)
				{
					return "Error validating definition id: " + definition.Artifact.ArtifactSymbol.Id
					                                          + " against its template id: " +
					                                          formula.Artifact.ArtifactSymbol.Id
					                                          + " the behavior " + b.Reference.Id
					                                          + " is not found.";
				}
			}
			
			foreach (var bg in definition.BehaviorGroups)
			{
				var tbg = formula.BehaviorGroups.SingleOrDefault(e => e.BehaviorGroup.Id == bg.Reference.Id);
				if (tbg == null)
				{
					return "Error validating definition id: " + definition.Artifact.ArtifactSymbol.Id
					                                          + " against its template id: " +
					                                          formula.Artifact.ArtifactSymbol.Id
					                                          + " the behaviorGroup " + bg.Reference.Id
					                                          + " is not found.";
				}
			}
			
			foreach (var ps in definition.PropertySets)
			{
				var tps = formula.PropertySets.SingleOrDefault(e => e.PropertySet.Id == ps.Reference.Id);
				if (tps == null)
				{
					return "Error validating definition id: " + definition.Artifact.ArtifactSymbol.Id
					                                          + " against its template id: " +
					                                          formula.Artifact.ArtifactSymbol.Id
					                                          + " the propertySet " + ps.Reference.Id
					                                          + " is not found.";
				}
			}

			return "";
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