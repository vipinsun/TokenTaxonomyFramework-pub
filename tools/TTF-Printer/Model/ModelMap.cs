using System;

namespace TTI.TTF.Taxonomy.Model
{
    public static class ModelMap
    {
        public static readonly string BaseFolder = "Base";
        public static readonly string BehaviorFolder = "Behaviors";
        public static readonly string BehaviorGroupFolder = "Behavior-groups";
        public static readonly string PropertySetFolder = "Property-sets";
        private static readonly string TokenTemplatesFolder = "Token-templates";
        public static readonly string TemplateFormulasFolder = TokenTemplatesFolder + FolderSeparator + "Formulas";
        public static readonly string TemplateDefinitionsFolder = TokenTemplatesFolder + FolderSeparator + "Definitions";
        public static readonly string SpecificationsFolder = "Specifications";
        public static readonly string FolderSeparator = @"\";
        
    }
}
