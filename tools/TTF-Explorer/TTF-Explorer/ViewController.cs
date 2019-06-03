using System;

using AppKit;
using Foundation;

namespace TTI.TTF.TTFExplorer
{
    public partial class ViewController : NSViewController
    {

        internal static Taxonomy.Model.Taxonomy Taxonomy { get; set; }

        public ViewController(IntPtr handle) : base(handle)
        {
          Taxonomy = TaxonomyContext.GetFullTaxonomy(new TaxonomyVersion { Version = "1.0" });
          _log.Info("Taxonomy Version: " + Taxonomy.Version + " loaded.");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
