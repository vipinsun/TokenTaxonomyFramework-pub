using System.Collections.Generic;
using TTI.TTF.Taxonomy.Model.Artifact;

namespace TTI.TTF.UISandbox.Models
{
    public class ArtifactItem
    {
        public ArtifactType ArtifactType { get; set; }
        public string ArtifactId { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public bool IsExpanded { get; set; }
        public int? ParentId { get; set; } //ArtifactCategoryItem.CategoryId
        public bool HasChildren { get; set; }
        public List<ArtifactItem> Items { get; set; }
    }

}