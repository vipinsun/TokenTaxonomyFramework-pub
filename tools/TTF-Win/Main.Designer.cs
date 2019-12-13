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
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Token Bases");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Behaviors");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Behavior Groups");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Property Sets");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Template Formulas");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Template Definitions");
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabArtifact = new System.Windows.Forms.TabPage();
            this.gbSymbol = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabType = new System.Windows.Forms.TabPage();
            this.artifactSymbolBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.visualDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.templateValidatedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.gbDefinition = new System.Windows.Forms.GroupBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabArtifact.SuspendLayout();
            this.gbSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.artifactSymbolBindingSource)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1456);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(2338, 42);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(79, 32);
            this.toolStripStatus.Text = "Ready";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2338, 40);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(72, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 44);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(85, 36);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(214, 44);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 40);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(2338, 1416);
            this.splitContainer1.SplitterDistance = 353;
            this.splitContainer1.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode7.Name = "nodeBases";
            treeNode7.Text = "Token Bases";
            treeNode8.Name = "nodeBehaviors";
            treeNode8.Text = "Behaviors";
            treeNode9.Name = "nodeBehaviorGroups";
            treeNode9.Text = "Behavior Groups";
            treeNode10.Name = "nodePropertySets";
            treeNode10.Text = "Property Sets";
            treeNode11.Name = "nodeFormulas";
            treeNode11.Text = "Template Formulas";
            treeNode12.Name = "nodeDefinitions";
            treeNode12.Text = "Template Definitions";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
            this.treeView1.Size = new System.Drawing.Size(353, 1416);
            this.treeView1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabArtifact);
            this.tabControl1.Controls.Add(this.tabType);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1981, 1416);
            this.tabControl1.TabIndex = 0;
            // 
            // tabArtifact
            // 
            this.tabArtifact.Controls.Add(this.flowLayoutPanel1);
            this.tabArtifact.Location = new System.Drawing.Point(8, 39);
            this.tabArtifact.Name = "tabArtifact";
            this.tabArtifact.Padding = new System.Windows.Forms.Padding(3);
            this.tabArtifact.Size = new System.Drawing.Size(1965, 1369);
            this.tabArtifact.TabIndex = 1;
            this.tabArtifact.Text = "Artifact Data";
            this.tabArtifact.UseVisualStyleBackColor = true;
            // 
            // gbSymbol
            // 
            this.gbSymbol.Controls.Add(this.dataGridView1);
            this.gbSymbol.Location = new System.Drawing.Point(3, 3);
            this.gbSymbol.Name = "gbSymbol";
            this.gbSymbol.Size = new System.Drawing.Size(1959, 140);
            this.gbSymbol.TabIndex = 0;
            this.gbSymbol.TabStop = false;
            this.gbSymbol.Text = "Artifact Symbol";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.visualDataGridViewTextBoxColumn,
            this.toolingDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.templateValidatedDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.artifactSymbolBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(1953, 110);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabType
            // 
            this.tabType.Location = new System.Drawing.Point(8, 39);
            this.tabType.Name = "tabType";
            this.tabType.Padding = new System.Windows.Forms.Padding(3);
            this.tabType.Size = new System.Drawing.Size(1965, 1369);
            this.tabType.TabIndex = 2;
            this.tabType.Text = "Type Data";
            this.tabType.UseVisualStyleBackColor = true;
            // 
            // artifactSymbolBindingSource
            // 
            this.artifactSymbolBindingSource.DataSource = typeof(TTI.TTF.Taxonomy.Model.Artifact.ArtifactSymbol);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 200;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.Width = 200;
            // 
            // visualDataGridViewTextBoxColumn
            // 
            this.visualDataGridViewTextBoxColumn.DataPropertyName = "Visual";
            this.visualDataGridViewTextBoxColumn.HeaderText = "Visual";
            this.visualDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.visualDataGridViewTextBoxColumn.Name = "visualDataGridViewTextBoxColumn";
            this.visualDataGridViewTextBoxColumn.Width = 200;
            // 
            // toolingDataGridViewTextBoxColumn
            // 
            this.toolingDataGridViewTextBoxColumn.DataPropertyName = "Tooling";
            this.toolingDataGridViewTextBoxColumn.HeaderText = "Tooling";
            this.toolingDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.toolingDataGridViewTextBoxColumn.Name = "toolingDataGridViewTextBoxColumn";
            this.toolingDataGridViewTextBoxColumn.Width = 200;
            // 
            // versionDataGridViewTextBoxColumn
            // 
            this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
            this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.versionDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
            this.versionDataGridViewTextBoxColumn.Width = 200;
            // 
            // templateValidatedDataGridViewCheckBoxColumn
            // 
            this.templateValidatedDataGridViewCheckBoxColumn.DataPropertyName = "TemplateValidated";
            this.templateValidatedDataGridViewCheckBoxColumn.HeaderText = "TemplateValidated";
            this.templateValidatedDataGridViewCheckBoxColumn.MinimumWidth = 10;
            this.templateValidatedDataGridViewCheckBoxColumn.Name = "templateValidatedDataGridViewCheckBoxColumn";
            this.templateValidatedDataGridViewCheckBoxColumn.Width = 200;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.gbSymbol);
            this.flowLayoutPanel1.Controls.Add(this.gbDefinition);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1959, 1363);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // gbDefinition
            // 
            this.gbDefinition.Location = new System.Drawing.Point(3, 149);
            this.gbDefinition.Name = "gbDefinition";
            this.gbDefinition.Size = new System.Drawing.Size(1953, 391);
            this.gbDefinition.TabIndex = 1;
            this.gbDefinition.TabStop = false;
            this.gbDefinition.Text = "Artifact Definition";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2338, 1498);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Main";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabArtifact.ResumeLayout(false);
            this.gbSymbol.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.artifactSymbolBindingSource)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabArtifact;
        private System.Windows.Forms.TabPage tabType;
        private System.Windows.Forms.GroupBox gbSymbol;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource artifactSymbolBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn visualDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn templateValidatedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox gbDefinition;
    }
}