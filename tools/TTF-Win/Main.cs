using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using TTF_Win.Controller;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTF_Win
{
    public partial class Main : Telerik.WinControls.UI.RadForm
    {
        private readonly Taxonomy _taxonomy = TaxonomyServices.Taxonomy;
        public Main()
        {
            InitializeComponent();
            radListView1.EnableGrouping = true;
            radListView1.ShowGroups = true;
            var groupByValue = new GroupDescriptor(new SortDescriptor("ArtifactType", ListSortDirection.Descending));
            radListView1.GroupDescriptors.Add(groupByValue);

            /*
            radListView1.Groups.Add(new ListViewDataItemGroup("Bases"));
            radListView1.Groups.Add(new ListViewDataItemGroup("Behaviors"));
            radListView1.Groups.Add(new ListViewDataItemGroup("Behavior Groups"));
            radListView1.Groups.Add(new ListViewDataItemGroup("Property Sets"));
            radListView1.Groups.Add(new ListViewDataItemGroup("Template Formulas"));
            radListView1.Groups.Add(new ListViewDataItemGroup("Template Definitions"));
            */

            radListView1.Columns.Add("ArtifactName", "Name");
            radListView1.Columns.Add("ArtifactType", "Type");
            radListView1.Columns.Add("Version", "Version");
            radListView1.Columns.Add(new ListViewDetailColumn("Id", "Id"));

            IEnumerable<Base> bases = _taxonomy.BaseTokenTypes.Values;
            IEnumerable<Behavior> behaviors = _taxonomy.Behaviors.Values;
            IEnumerable<BehaviorGroup> behaviorGroups = _taxonomy.BehaviorGroups.Values;
            IEnumerable<PropertySet> propSets = _taxonomy.PropertySets.Values;
            IEnumerable<TemplateFormula> formulas = _taxonomy.TemplateFormulas.Values;
            IEnumerable<TemplateDefinition> definitions = _taxonomy.TemplateDefinitions.Values;

            foreach (var b in bases)
            {
                var item = new ListViewDataItem();
                radListView1.Items.Add(item);

                item[0] = b.Artifact.Name;
                item[1] = b.Artifact.ArtifactSymbol.Type;
                item[2] = b.Artifact.ArtifactSymbol.Version;
                item[3] = b.Artifact.ArtifactSymbol.Id;
            }

            foreach (var b in behaviors)
            {
                var item = new ListViewDataItem();
                radListView1.Items.Add(item);

                item[0] = b.Artifact.Name;
                item[1] = b.Artifact.ArtifactSymbol.Type;
                item[2] = b.Artifact.ArtifactSymbol.Version;
                item[3] = b.Artifact.ArtifactSymbol.Id;
            }

            foreach (var b in behaviorGroups)
            {
                var item = new ListViewDataItem();
                radListView1.Items.Add(item);

                item[0] = b.Artifact.Name;
                item[1] = b.Artifact.ArtifactSymbol.Type;
                item[2] = b.Artifact.ArtifactSymbol.Version;
                item[3] = b.Artifact.ArtifactSymbol.Id;
            }

            foreach (var p in propSets)
            {
                var item = new ListViewDataItem();
                radListView1.Items.Add(item);

                item[0] = p.Artifact.Name;
                item[1] = p.Artifact.ArtifactSymbol.Type;
                item[2] = p.Artifact.ArtifactSymbol.Version;
                item[3] = p.Artifact.ArtifactSymbol.Id;
            }

            foreach (var f in formulas)
            {
                var item = new ListViewDataItem();
                radListView1.Items.Add(item);

                item[0] = f.Artifact.Name;
                item[1] = f.Artifact.ArtifactSymbol.Type;
                item[2] = f.Artifact.ArtifactSymbol.Version;
                item[3] = f.Artifact.ArtifactSymbol.Id;
            }

            foreach (var d in definitions)
            {
                var item = new ListViewDataItem();
                radListView1.Items.Add(item);

                item[0] = d.Artifact.Name;
                item[1] = d.Artifact.ArtifactSymbol.Type;
                item[2] = d.Artifact.ArtifactSymbol.Version;
                item[3] = d.Artifact.ArtifactSymbol.Id;
            }
        }

        
        private void radListView1_ItemMouseDoubleClick(object sender, ListViewItemEventArgs e)
        {
            var selectedArtifact = e.Item;

            var id = selectedArtifact["Id"].ToString();
            var type = (ArtifactType)Enum.Parse(typeof(ArtifactType), selectedArtifact["ArtifactType"].ToString());
            
            switch (type)
            {
                case ArtifactType.Base:
                    var selectedBase = GetSelectedArtifact<Base>(id, type);
                    PopulateBaseControls(selectedBase);
                    PopulateArtifactControls(selectedBase.Artifact);
                    break;
                case ArtifactType.Behavior:
                    var selectedBehavior = GetSelectedArtifact<Behavior>(id, type);
                    PopulateBehaviorControls(selectedBehavior);
                    PopulateArtifactControls(selectedBehavior.Artifact);
                    break;
                case ArtifactType.BehaviorGroup:
                    var selectedBehaviorGroup = GetSelectedArtifact<BehaviorGroup>(id, type);
                    PopulateBehaviorGroupControls(selectedBehaviorGroup);
                    PopulateArtifactControls(selectedBehaviorGroup.Artifact);
                    break;
                case ArtifactType.PropertySet:
                    var selectedPropertySet = GetSelectedArtifact<PropertySet>(id, type);
                    PopulatePropertySetControls(selectedPropertySet);
                    PopulateArtifactControls(selectedPropertySet.Artifact);
                    break;
                case ArtifactType.TemplateFormula:
                    var selectedFormula = GetSelectedArtifact<TemplateFormula>(id, type);
                    PopulateFormulaControls(selectedFormula);
                    PopulateArtifactControls(selectedFormula.Artifact);
                    break;
                case ArtifactType.TemplateDefinition:
                    var selectedDefinition = GetSelectedArtifact<TemplateDefinition>(id, type);
                    PopulateDefinitionControls(selectedDefinition);
                    PopulateArtifactControls(selectedDefinition.Artifact);
                    break;
   
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void PopulateDefinitionControls(TemplateDefinition selectedDefinition)
        {
            
        }

        private void PopulateFormulaControls(TemplateFormula selectedFormula)
        {
      
        }

        private void PopulatePropertySetControls(PropertySet selectedPropertySet)
        {
           
        }

        private void PopulateBehaviorGroupControls(BehaviorGroup selectedBehaviorGroup)
        {
            
        }

        private void PopulateBehaviorControls(Behavior selectedBehavior)
        {
            
        }

        private void PopulateBaseControls(Base selectedBase)
        {
            
        }

        private T GetSelectedArtifact<T>(string id, ArtifactType artifactType) where T : new()
        {
            dynamic artifactObject;
            switch (artifactType)
            {
                case ArtifactType.Base:
                    artifactObject =
                        TaxonomyServices.Taxonomy.BaseTokenTypes.Values.SingleOrDefault(e =>
                            e.Artifact.ArtifactSymbol.Id == id);
                    break;
                case ArtifactType.Behavior:
                    artifactObject =
                        TaxonomyServices.Taxonomy.Behaviors.Values.SingleOrDefault(e =>
                            e.Artifact.ArtifactSymbol.Id == id);
                    break;
                case ArtifactType.BehaviorGroup:
                    artifactObject =
                        TaxonomyServices.Taxonomy.BehaviorGroups.Values.SingleOrDefault(e =>
                            e.Artifact.ArtifactSymbol.Id == id);
                    break;
                case ArtifactType.PropertySet:
                    artifactObject =
                        TaxonomyServices.Taxonomy.PropertySets.Values.SingleOrDefault(e =>
                            e.Artifact.ArtifactSymbol.Id == id);
                    break;
                case ArtifactType.TemplateFormula:
                    artifactObject =
                        TaxonomyServices.Taxonomy.TemplateFormulas.Values.SingleOrDefault(e =>
                            e.Artifact.ArtifactSymbol.Id == id);
                    break;
                case ArtifactType.TemplateDefinition:
                    artifactObject =
                        TaxonomyServices.Taxonomy.TemplateDefinitions.Values.SingleOrDefault(e =>
                            e.Artifact.ArtifactSymbol.Id == id);
                    break;
                case ArtifactType.TokenTemplate:
                    artifactObject = TaxonomyServices.Taxonomy.TemplateDefinitions.Values.SingleOrDefault(e =>
                        e.Artifact.ArtifactSymbol.Id == id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
            }

            if (artifactObject != null) return (T)Convert.ChangeType(artifactObject, typeof(T));
            return new T();
            
        }

        private void PopulateArtifactControls(Artifact artifact)
        {
            var artifactSymbolControl = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.ArtifactSymbol
            };

            var artifactControl = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact
            };

            var artifactAliases = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Aliases
            };

            var rootArtifactPage = new RadPageViewPage {Text = "Artifact"};
            rootArtifactPage.Controls.Add(artifactControl);
            rootArtifactPage.Controls.Add(new RadLabel{Text = "Artifact Symbol:"});
            rootArtifactPage.Controls.Add(artifactSymbolControl);
            rootArtifactPage.Controls.Add(new RadLabel { Text = "Aliases:" });
            rootArtifactPage.Controls.Add(artifactAliases);
            radPageView1.Pages.Add(rootArtifactPage);


            var artifactBizDesc = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.ArtifactDefinition
            };


            var artifactAnalogyControl = new RadDataLayout
            {
                Dock = DockStyle.Bottom,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.ArtifactDefinition.Analogies
            };
            artifactBizDesc.Controls.Add(artifactAnalogyControl);

            var bizArtifactPage = new RadPageViewPage { Text = "Business Definition" };
            bizArtifactPage.Controls.Add(artifactBizDesc);
            
            radPageView1.Pages.Add(bizArtifactPage);


            var artifactSymbolDependency = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Dependencies
            };


            var artifactIncompatible = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.IncompatibleWithSymbols
            };


            var artifactSymbolInfluence = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.InfluencedBySymbols
            };


            var artifactFiles = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.ArtifactFiles
            };

            var artifactMaps = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Maps
            };

            var artifactMapRefs = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Maps.CodeReferences
            };

            var artifactMapRef2 = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Maps.ImplementationReferences
            };


            var artifactMapRes = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Maps.Resources
            };

            artifactMaps.Controls.Add(artifactMapRefs);
            artifactMaps.Controls.Add(artifactMapRef2);
            artifactMaps.Controls.Add(artifactMapRes);

            var artifactContributors = new RadDataLayout
            {
                Dock = DockStyle.Fill,
                ItemDefaultHeight = 26,
                ColumnCount = 2,
                FlowDirection = FlowDirection.TopDown,
                AutoSizeLabels = true,
                DataSource = artifact.Contributors
            };

        }
    }
}
