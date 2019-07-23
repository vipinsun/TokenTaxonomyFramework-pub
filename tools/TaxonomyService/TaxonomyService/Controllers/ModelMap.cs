namespace TTI.TTF.Taxonomy.Controllers
{
    public static class ModelMap
    {
        public static readonly string BaseFolder = "base";
        public static readonly string BehaviorFolder = "behaviors";
        public static readonly string BehaviorGroupFolder = "behavior-groups";
        public static readonly string PropertySetFolder = "property-sets";
        public static readonly string TokenTemplatesFolder = "token-templates";
        public static readonly string TemplateFormulasFolder = TokenTemplatesFolder + TxService.FolderSeparator + "formulas";
        public static readonly string TemplateDefinitionsFolder = TokenTemplatesFolder + TxService.FolderSeparator + "definitions";
        public static readonly string ClassificationRoots = TemplateFormulasFolder + TxService.FolderSeparator + "classificationRoots";

    }
}