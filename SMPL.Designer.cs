namespace SMPL
{
   partial class SMPL
   {
      /// <summary>
      ///  Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      ///  Clean up any resources being used.
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
      ///  Required method for Designer support - do not modify
      ///  the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SMPL));
			this.mainPanel = new System.Windows.Forms.SplitContainer();
			this.leftPanel = new System.Windows.Forms.SplitContainer();
			this.topLeftTabs = new System.Windows.Forms.TabControl();
			this.sceneTab = new System.Windows.Forms.TabPage();
			this.sceneRightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.sceneRightClickMenuCreate = new System.Windows.Forms.ToolStripMenuItem();
			this.spriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sceneRightClickSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.sceneRightClickMenuResetView = new System.Windows.Forms.ToolStripMenuItem();
			this.sceneAngle = new System.Windows.Forms.HScrollBar();
			this.sceneZoom = new System.Windows.Forms.VScrollBar();
			this.sceneMousePos = new System.Windows.Forms.Label();
			this.sceneStatus = new System.Windows.Forms.FlowLayoutPanel();
			this.groupBoxGrid = new System.Windows.Forms.GroupBox();
			this.gridSpacing = new System.Windows.Forms.MaskedTextBox();
			this.gridThickness = new System.Windows.Forms.TrackBar();
			this.gameTab = new System.Windows.Forms.TabPage();
			this.bottomLeftTabs = new System.Windows.Forms.TabControl();
			this.logTab = new System.Windows.Forms.TabPage();
			this.outputStatus = new System.Windows.Forms.FlowLayoutPanel();
			this.command = new System.Windows.Forms.MaskedTextBox();
			this.outputActive = new System.Windows.Forms.CheckBox();
			this.log = new System.Windows.Forms.RichTextBox();
			this.toolTipGridThickness = new System.Windows.Forms.ToolTip(this.components);
			this.toolTipDisableLog = new System.Windows.Forms.ToolTip(this.components);
			this.toolTipGridSpacing = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
			this.mainPanel.Panel1.SuspendLayout();
			this.mainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.leftPanel)).BeginInit();
			this.leftPanel.Panel1.SuspendLayout();
			this.leftPanel.Panel2.SuspendLayout();
			this.leftPanel.SuspendLayout();
			this.topLeftTabs.SuspendLayout();
			this.sceneTab.SuspendLayout();
			this.sceneRightClickMenu.SuspendLayout();
			this.sceneStatus.SuspendLayout();
			this.groupBoxGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridThickness)).BeginInit();
			this.bottomLeftTabs.SuspendLayout();
			this.logTab.SuspendLayout();
			this.outputStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainPanel
			// 
			this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mainPanel, "mainPanel");
			this.mainPanel.Name = "mainPanel";
			// 
			// mainPanel.Panel1
			// 
			this.mainPanel.Panel1.Controls.Add(this.leftPanel);
			this.mainPanel.TabStop = false;
			// 
			// leftPanel
			// 
			this.leftPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.leftPanel, "leftPanel");
			this.leftPanel.Name = "leftPanel";
			// 
			// leftPanel.Panel1
			// 
			this.leftPanel.Panel1.Controls.Add(this.topLeftTabs);
			this.leftPanel.Panel1.MouseEnter += new System.EventHandler(this.OnMouseEnterScene);
			this.leftPanel.Panel1.MouseLeave += new System.EventHandler(this.OnMouseLeaveScene);
			this.leftPanel.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMoveScene);
			// 
			// leftPanel.Panel2
			// 
			this.leftPanel.Panel2.Controls.Add(this.bottomLeftTabs);
			this.leftPanel.TabStop = false;
			// 
			// topLeftTabs
			// 
			this.topLeftTabs.Controls.Add(this.sceneTab);
			this.topLeftTabs.Controls.Add(this.gameTab);
			resources.ApplyResources(this.topLeftTabs, "topLeftTabs");
			this.topLeftTabs.Name = "topLeftTabs";
			this.topLeftTabs.SelectedIndex = 0;
			this.topLeftTabs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownTopLeftTabs);
			// 
			// sceneTab
			// 
			this.sceneTab.BackColor = System.Drawing.Color.Black;
			this.sceneTab.ContextMenuStrip = this.sceneRightClickMenu;
			this.sceneTab.Controls.Add(this.sceneAngle);
			this.sceneTab.Controls.Add(this.sceneZoom);
			this.sceneTab.Controls.Add(this.sceneMousePos);
			this.sceneTab.Controls.Add(this.sceneStatus);
			resources.ApplyResources(this.sceneTab, "sceneTab");
			this.sceneTab.Name = "sceneTab";
			this.sceneTab.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDownScene);
			this.sceneTab.MouseEnter += new System.EventHandler(this.OnMouseEnterScene);
			this.sceneTab.MouseLeave += new System.EventHandler(this.OnMouseLeaveScene);
			this.sceneTab.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMoveScene);
			this.sceneTab.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpScene);
			// 
			// sceneRightClickMenu
			// 
			this.sceneRightClickMenu.BackColor = System.Drawing.Color.White;
			this.sceneRightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sceneRightClickMenuCreate,
            this.sceneRightClickSeparator1,
            this.sceneRightClickMenuResetView});
			this.sceneRightClickMenu.Name = "sceneRightClickMenu";
			resources.ApplyResources(this.sceneRightClickMenu, "sceneRightClickMenu");
			// 
			// sceneRightClickMenuCreate
			// 
			this.sceneRightClickMenuCreate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spriteToolStripMenuItem});
			this.sceneRightClickMenuCreate.Name = "sceneRightClickMenuCreate";
			resources.ApplyResources(this.sceneRightClickMenuCreate, "sceneRightClickMenuCreate");
			// 
			// spriteToolStripMenuItem
			// 
			this.spriteToolStripMenuItem.Name = "spriteToolStripMenuItem";
			resources.ApplyResources(this.spriteToolStripMenuItem, "spriteToolStripMenuItem");
			this.spriteToolStripMenuItem.Click += new System.EventHandler(this.OnSceneRightClickMenuCreateSprite);
			// 
			// sceneRightClickSeparator1
			// 
			this.sceneRightClickSeparator1.Name = "sceneRightClickSeparator1";
			resources.ApplyResources(this.sceneRightClickSeparator1, "sceneRightClickSeparator1");
			// 
			// sceneRightClickMenuResetView
			// 
			this.sceneRightClickMenuResetView.Name = "sceneRightClickMenuResetView";
			resources.ApplyResources(this.sceneRightClickMenuResetView, "sceneRightClickMenuResetView");
			this.sceneRightClickMenuResetView.Click += new System.EventHandler(this.OnSceneRightClickMenuResetView);
			// 
			// sceneAngle
			// 
			resources.ApplyResources(this.sceneAngle, "sceneAngle");
			this.sceneAngle.LargeChange = 1;
			this.sceneAngle.Name = "sceneAngle";
			this.sceneAngle.ValueChanged += new System.EventHandler(this.OnSceneRotate);
			// 
			// sceneZoom
			// 
			resources.ApplyResources(this.sceneZoom, "sceneZoom");
			this.sceneZoom.Maximum = 200;
			this.sceneZoom.Name = "sceneZoom";
			this.sceneZoom.Value = 10;
			this.sceneZoom.ValueChanged += new System.EventHandler(this.OnSceneZoom);
			// 
			// sceneMousePos
			// 
			resources.ApplyResources(this.sceneMousePos, "sceneMousePos");
			this.sceneMousePos.BackColor = System.Drawing.Color.Black;
			this.sceneMousePos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sceneMousePos.ForeColor = System.Drawing.Color.White;
			this.sceneMousePos.Name = "sceneMousePos";
			// 
			// sceneStatus
			// 
			this.sceneStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sceneStatus.Controls.Add(this.groupBoxGrid);
			resources.ApplyResources(this.sceneStatus, "sceneStatus");
			this.sceneStatus.Name = "sceneStatus";
			this.sceneStatus.Click += new System.EventHandler(this.OnSceneStatusClick);
			// 
			// groupBoxGrid
			// 
			this.groupBoxGrid.Controls.Add(this.gridSpacing);
			this.groupBoxGrid.Controls.Add(this.gridThickness);
			resources.ApplyResources(this.groupBoxGrid, "groupBoxGrid");
			this.groupBoxGrid.ForeColor = System.Drawing.Color.White;
			this.groupBoxGrid.Name = "groupBoxGrid";
			this.groupBoxGrid.TabStop = false;
			// 
			// gridSpacing
			// 
			this.gridSpacing.BackColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.gridSpacing, "gridSpacing");
			this.gridSpacing.ForeColor = System.Drawing.Color.White;
			this.gridSpacing.Name = "gridSpacing";
			this.toolTipGridSpacing.SetToolTip(this.gridSpacing, resources.GetString("gridSpacing.ToolTip"));
			this.gridSpacing.ValidatingType = typeof(int);
			this.gridSpacing.TextChanged += new System.EventHandler(this.OnGridSpacingChange);
			// 
			// gridThickness
			// 
			this.gridThickness.BackColor = System.Drawing.Color.Black;
			this.gridThickness.Cursor = System.Windows.Forms.Cursors.SizeWE;
			resources.ApplyResources(this.gridThickness, "gridThickness");
			this.gridThickness.LargeChange = 1;
			this.gridThickness.Name = "gridThickness";
			this.toolTipGridThickness.SetToolTip(this.gridThickness, resources.GetString("gridThickness.ToolTip"));
			this.gridThickness.Value = 2;
			// 
			// gameTab
			// 
			this.gameTab.BackColor = System.Drawing.Color.Black;
			this.gameTab.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.gameTab, "gameTab");
			this.gameTab.Name = "gameTab";
			// 
			// bottomLeftTabs
			// 
			this.bottomLeftTabs.Controls.Add(this.logTab);
			resources.ApplyResources(this.bottomLeftTabs, "bottomLeftTabs");
			this.bottomLeftTabs.HotTrack = true;
			this.bottomLeftTabs.Name = "bottomLeftTabs";
			this.bottomLeftTabs.SelectedIndex = 0;
			this.bottomLeftTabs.TabStop = false;
			// 
			// logTab
			// 
			this.logTab.BackColor = System.Drawing.Color.Black;
			this.logTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.logTab.Controls.Add(this.outputStatus);
			this.logTab.Controls.Add(this.log);
			resources.ApplyResources(this.logTab, "logTab");
			this.logTab.Name = "logTab";
			// 
			// outputStatus
			// 
			this.outputStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.outputStatus.Controls.Add(this.command);
			this.outputStatus.Controls.Add(this.outputActive);
			resources.ApplyResources(this.outputStatus, "outputStatus");
			this.outputStatus.Name = "outputStatus";
			// 
			// command
			// 
			this.command.AsciiOnly = true;
			this.command.BackColor = System.Drawing.Color.Black;
			this.command.ForeColor = System.Drawing.Color.White;
			resources.ApplyResources(this.command, "command");
			this.command.Name = "command";
			this.command.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnCommandKeyPress);
			// 
			// outputActive
			// 
			resources.ApplyResources(this.outputActive, "outputActive");
			this.outputActive.Cursor = System.Windows.Forms.Cursors.Hand;
			this.outputActive.ForeColor = System.Drawing.Color.White;
			this.outputActive.Name = "outputActive";
			this.toolTipDisableLog.SetToolTip(this.outputActive, resources.GetString("outputActive.ToolTip"));
			this.outputActive.UseVisualStyleBackColor = true;
			// 
			// log
			// 
			this.log.BackColor = System.Drawing.Color.Black;
			this.log.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.log, "log");
			this.log.ForeColor = System.Drawing.Color.White;
			this.log.Name = "log";
			this.log.TextChanged += new System.EventHandler(this.OnOutputTextChange);
			this.log.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnOutputKeyPress);
			// 
			// toolTipGridThickness
			// 
			this.toolTipGridThickness.AutomaticDelay = 0;
			this.toolTipGridThickness.AutoPopDelay = 0;
			this.toolTipGridThickness.BackColor = System.Drawing.Color.Black;
			this.toolTipGridThickness.ForeColor = System.Drawing.Color.White;
			this.toolTipGridThickness.InitialDelay = 0;
			this.toolTipGridThickness.ReshowDelay = 0;
			// 
			// toolTipDisableLog
			// 
			this.toolTipDisableLog.AutomaticDelay = 0;
			this.toolTipDisableLog.AutoPopDelay = 0;
			this.toolTipDisableLog.BackColor = System.Drawing.Color.Black;
			this.toolTipDisableLog.ForeColor = System.Drawing.Color.White;
			this.toolTipDisableLog.InitialDelay = 0;
			this.toolTipDisableLog.ReshowDelay = 0;
			// 
			// toolTipGridSpacing
			// 
			this.toolTipGridSpacing.AutomaticDelay = 0;
			this.toolTipGridSpacing.AutoPopDelay = 0;
			this.toolTipGridSpacing.BackColor = System.Drawing.Color.Black;
			this.toolTipGridSpacing.ForeColor = System.Drawing.Color.White;
			this.toolTipGridSpacing.InitialDelay = 0;
			this.toolTipGridSpacing.ReshowDelay = 0;
			// 
			// SMPL
			// 
			this.BackColor = System.Drawing.Color.Black;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.mainPanel);
			this.Name = "SMPL";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.mainPanel.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
			this.mainPanel.ResumeLayout(false);
			this.leftPanel.Panel1.ResumeLayout(false);
			this.leftPanel.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.leftPanel)).EndInit();
			this.leftPanel.ResumeLayout(false);
			this.topLeftTabs.ResumeLayout(false);
			this.sceneTab.ResumeLayout(false);
			this.sceneTab.PerformLayout();
			this.sceneRightClickMenu.ResumeLayout(false);
			this.sceneStatus.ResumeLayout(false);
			this.groupBoxGrid.ResumeLayout(false);
			this.groupBoxGrid.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridThickness)).EndInit();
			this.bottomLeftTabs.ResumeLayout(false);
			this.logTab.ResumeLayout(false);
			this.outputStatus.ResumeLayout(false);
			this.outputStatus.PerformLayout();
			this.ResumeLayout(false);

      }

		#endregion
		private SplitContainer mainPanel;
		private SplitContainer leftPanel;
		private Label sceneMousePos;
		private FlowLayoutPanel sceneStatus;
		private TabControl bottomLeftTabs;
		private TabPage logTab;
		private RichTextBox log;
		private FlowLayoutPanel outputStatus;
		private CheckBox outputActive;
		private TabControl topLeftTabs;
		private TabPage sceneTab;
		private TabPage gameTab;
		private VScrollBar sceneZoom;
		private HScrollBar sceneAngle;
		private TrackBar gridThickness;
		private ToolTip toolTipGridThickness;
		private ToolTip toolTipDisableLog;
		private MaskedTextBox gridSpacing;
		private ToolTip toolTipGridSpacing;
		private GroupBox groupBoxGrid;
		private MaskedTextBox command;
		private ContextMenuStrip sceneRightClickMenu;
		private ToolStripMenuItem sceneRightClickMenuCreate;
		private ToolStripMenuItem spriteToolStripMenuItem;
		private ToolStripSeparator sceneRightClickSeparator1;
		private ToolStripMenuItem sceneRightClickMenuResetView;
	}
}