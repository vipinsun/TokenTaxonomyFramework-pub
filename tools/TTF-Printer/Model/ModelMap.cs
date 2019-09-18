using TTI.TTF.Taxonomy.TypePrinters;

namespace TTI.TTF.Taxonomy.Model
{
    public static class ModelMap
    {
        public static readonly string BaseFolder = "base";
        public static readonly string BehaviorFolder = "behaviors";
        public static readonly string BehaviorGroupFolder = "behavior-groups";
        public static readonly string PropertySetFolder = "property-sets";
        public static readonly string TokenTemplatesFolder = "token-templates";
        public static readonly string TemplateFormulasFolder;
        public static readonly string TemplateDefinitionsFolder;
        public static readonly string SpecificationsFolder;
        internal static string Latest { get; private set; }
        public static string FolderSeparator { get; private set; }

        static ModelMap()
        {
            FolderSeparator = Os.IsWindows() ? "\\" : "/";
            TemplateFormulasFolder = TokenTemplatesFolder + FolderSeparator + "formulas";
            TemplateDefinitionsFolder = TokenTemplatesFolder + FolderSeparator + "definitions";
            SpecificationsFolder = "specifications";
            Latest =  "latest" + FolderSeparator;
        }
    }
}
