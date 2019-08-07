using System;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;

namespace TTI.TTF.Taxonomy.Controllers
{
    public static class ModelMap
    {
        public static readonly string BaseFolder = "base";
        public static readonly string BehaviorFolder = "behaviors";
        public static readonly string BehaviorGroupFolder = "behavior-groups";
        public static readonly string PropertySetFolder = "property-sets";
        private static readonly string TokenTemplatesFolder = "token-templates";
        public static readonly string TemplateFormulasFolder = TokenTemplatesFolder + TxService.FolderSeparator + "formulas";
        public static readonly string TemplateDefinitionsFolder = TokenTemplatesFolder + TxService.FolderSeparator + "definitions";

        public static string GetBaseFolderName(TokenType tokenType, TokenUnit tokenUnit)
        {
            switch (tokenUnit)
            {
                case TokenUnit.Fractional:
                    switch (tokenType)
                    {
                        case TokenType.Fungible:
                            return "fractional-fungible";
                        case TokenType.NonFungible:
                            return "fractional-non-fungible";
                    }
                    break;
                case TokenUnit.Whole:
                    switch (tokenType)
                    {
                        case TokenType.Fungible:
                            return "whole-fungible";
                        case TokenType.NonFungible:
                            return "whole=non-fungible";
                    }
                    break;
                case TokenUnit.Singleton:
                    return "singleton";
            }
            return null;
        }
    }
}