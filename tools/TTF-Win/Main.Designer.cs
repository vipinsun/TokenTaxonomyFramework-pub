namespace TTF_Win
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.materialBlueGreyTheme1 = new Telerik.WinControls.Themes.MaterialBlueGreyTheme();
            this.radLayoutControl1 = new Telerik.WinControls.UI.RadLayoutControl();
            this.radListView1 = new Telerik.WinControls.UI.RadListView();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.layoutControlSplitterItem1 = new Telerik.WinControls.UI.LayoutControlSplitterItem();
            this.layoutControlItem1 = new Telerik.WinControls.UI.LayoutControlItem();
            this.layoutControlItem2 = new Telerik.WinControls.UI.LayoutControlItem();
            this.artifactBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).BeginInit();
            this.radLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.artifactBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLayoutControl1
            // 
            this.radLayoutControl1.Controls.Add(this.radListView1);
            this.radLayoutControl1.Controls.Add(this.radPageView1);
            this.radLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLayoutControl1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.layoutControlSplitterItem1,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.radLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.radLayoutControl1.Name = "radLayoutControl1";
            this.radLayoutControl1.Size = new System.Drawing.Size(1583, 910);
            this.radLayoutControl1.TabIndex = 0;
            this.radLayoutControl1.ThemeName = "MaterialBlueGrey";
            // 
            // radListView1
            // 
            this.radListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radListView1.GroupItemSize = new System.Drawing.Size(200, 36);
            this.radListView1.ItemSize = new System.Drawing.Size(200, 36);
            this.radListView1.Location = new System.Drawing.Point(4, 4);
            this.radListView1.Name = "radListView1";
            this.radListView1.Size = new System.Drawing.Size(212, 902);
            this.radListView1.TabIndex = 3;
            this.radListView1.ThemeName = "MaterialBlueGrey";
            this.radListView1.ItemMouseDoubleClick += new Telerik.WinControls.UI.ListViewItemEventHandler(this.radListView1_ItemMouseDoubleClick);
            // 
            // radPageView1
            // 
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(226, 4);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.Size = new System.Drawing.Size(1353, 902);
            this.radPageView1.TabIndex = 4;
            this.radPageView1.ThemeName = "MaterialBlueGrey";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).StripAlignment = Telerik.WinControls.UI.StripViewAlignment.Bottom;
            // 
            // layoutControlSplitterItem1
            // 
            this.layoutControlSplitterItem1.Bounds = new System.Drawing.Rectangle(218, 0, 4, 908);
            this.layoutControlSplitterItem1.Name = "layoutControlSplitterItem1";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AssociatedControl = this.radListView1;
            this.layoutControlItem1.Bounds = new System.Drawing.Rectangle(0, 0, 218, 908);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "layoutControlItem1";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AssociatedControl = this.radPageView1;
            this.layoutControlItem2.Bounds = new System.Drawing.Rectangle(222, 0, 1359, 908);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "layoutControlItem2";
            // 
            // artifactBindingSource
            // 
            this.artifactBindingSource.DataSource = typeof(TTI.TTF.Taxonomy.Model.Artifact.Artifact);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1583, 910);
            this.Controls.Add(this.radLayoutControl1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Main";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Token Taxonomy Framework Manager";
            this.ThemeName = "MaterialBlueGrey";
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).EndInit();
            this.radLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.artifactBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.Themes.MaterialBlueGreyTheme materialBlueGreyTheme1;
        private Telerik.WinControls.UI.RadLayoutControl radLayoutControl1;
        private Telerik.WinControls.UI.RadListView radListView1;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.LayoutControlSplitterItem layoutControlSplitterItem1;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem1;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.BindingSource artifactBindingSource;
    }
}