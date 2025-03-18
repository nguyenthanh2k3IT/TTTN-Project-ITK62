namespace VT.APPS.MES
{
    partial class CHART1
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
        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.SideBySideBarSeriesView sideBySideBarSeriesView1 = new DevExpress.XtraCharts.SideBySideBarSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelEx3 = new JPlatform.Client.Controls8.PanelEx();
            this.chartFTT = new DevExpress.XtraCharts.ChartControl();
            this.panelEx1 = new JPlatform.Client.Controls8.PanelEx();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblred = new System.Windows.Forms.Label();
            this.lblyellow = new System.Windows.Forms.Label();
            this.lblgreen = new System.Windows.Forms.Label();
            this.lbltarget = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).BeginInit();
            this.panelEx3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFTT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.groupControl1.Controls.Add(this.panelEx3);
            this.groupControl1.Controls.Add(this.panelEx1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(459, 279);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "FTT";
            // 
            // panelEx3
            // 
            this.panelEx3.Controls.Add(this.chartFTT);
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx3.Location = new System.Drawing.Point(2, 26);
            this.panelEx3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(455, 251);
            this.panelEx3.TabIndex = 2;
            // 
            // chartFTT
            // 
            this.chartFTT.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("Tahoma", 7F);
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.MinIndent = 1;
            xyDiagram1.AxisX.ScaleBreakOptions.SizeInPixels = 1;
            xyDiagram1.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Label.Font = new System.Drawing.Font("Tahoma", 7F);
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BorderVisible = false;
            xyDiagram1.Margins.Bottom = 0;
            xyDiagram1.Margins.Left = 0;
            xyDiagram1.Margins.Right = 0;
            xyDiagram1.Margins.Top = 0;
            this.chartFTT.Diagram = xyDiagram1;
            this.chartFTT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartFTT.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Left;
            this.chartFTT.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.TopOutside;
            this.chartFTT.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartFTT.Legend.Font = new System.Drawing.Font("Tahoma", 7F);
            this.chartFTT.Legend.Margins.Bottom = 0;
            this.chartFTT.Legend.Margins.Left = 0;
            this.chartFTT.Legend.Margins.Right = 0;
            this.chartFTT.Legend.Margins.Top = 0;
            this.chartFTT.Legend.MarkerSize = new System.Drawing.Size(20, 15);
            this.chartFTT.Legend.UseCheckBoxes = true;
            this.chartFTT.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT.Location = new System.Drawing.Point(2, 2);
            this.chartFTT.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chartFTT.Name = "chartFTT";
            this.chartFTT.PaletteName = "Flow";
            sideBySideBarSeriesLabel1.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            sideBySideBarSeriesLabel1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            sideBySideBarSeriesLabel1.Font = new System.Drawing.Font("Tahoma", 7F);
            sideBySideBarSeriesLabel1.LineLength = 5;
            sideBySideBarSeriesLabel1.Position = DevExpress.XtraCharts.BarSeriesLabelPosition.Top;
            sideBySideBarSeriesLabel1.TextColor = System.Drawing.Color.Black;
            sideBySideBarSeriesLabel1.TextOrientation = DevExpress.XtraCharts.TextOrientation.BottomToTop;
            series1.Label = sideBySideBarSeriesLabel1;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.Name = "Actual (%)";
            sideBySideBarSeriesView1.Color = System.Drawing.Color.LimeGreen;
            series1.View = sideBySideBarSeriesView1;
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series2.Name = "Target (%)";
            lineSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.False;
            series2.View = lineSeriesView1;
            this.chartFTT.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartFTT.Size = new System.Drawing.Size(451, 247);
            this.chartFTT.TabIndex = 9;
            chartTitle1.Alignment = System.Drawing.StringAlignment.Far;
            chartTitle1.Font = new System.Drawing.Font("Tahoma", 10F);
            chartTitle1.Text = "Daily FTT Trend";
            chartTitle1.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            this.chartFTT.CustomDrawSeriesPoint += new DevExpress.XtraCharts.CustomDrawSeriesPointEventHandler(this.chartFTT_CustomDrawSeriesPoint);
            // 
            // panelEx1
            // 
            this.panelEx1.Controls.Add(this.lblTitle);
            this.panelEx1.Controls.Add(this.lblred);
            this.panelEx1.Controls.Add(this.lblyellow);
            this.panelEx1.Controls.Add(this.lblgreen);
            this.panelEx1.Controls.Add(this.lbltarget);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(2, 2);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(455, 24);
            this.panelEx1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(2, 2);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(154, 20);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "FTT";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblred
            // 
            this.lblred.BackColor = System.Drawing.Color.Red;
            this.lblred.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblred.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblred.ForeColor = System.Drawing.Color.White;
            this.lblred.Location = new System.Drawing.Point(271, 2);
            this.lblred.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblred.Name = "lblred";
            this.lblred.Size = new System.Drawing.Size(40, 20);
            this.lblred.TabIndex = 3;
            this.lblred.Text = "<=85";
            this.lblred.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblyellow
            // 
            this.lblyellow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(120)))));
            this.lblyellow.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblyellow.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblyellow.Location = new System.Drawing.Point(311, 2);
            this.lblyellow.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblyellow.Name = "lblyellow";
            this.lblyellow.Size = new System.Drawing.Size(38, 20);
            this.lblyellow.TabIndex = 2;
            this.lblyellow.Text = "<=89.9";
            this.lblyellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblgreen
            // 
            this.lblgreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            this.lblgreen.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblgreen.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblgreen.ForeColor = System.Drawing.Color.White;
            this.lblgreen.Location = new System.Drawing.Point(349, 2);
            this.lblgreen.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblgreen.Name = "lblgreen";
            this.lblgreen.Size = new System.Drawing.Size(37, 20);
            this.lblgreen.TabIndex = 1;
            this.lblgreen.Text = ">=90";
            this.lblgreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbltarget
            // 
            this.lbltarget.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbltarget.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.lbltarget.ForeColor = System.Drawing.Color.Red;
            this.lbltarget.Location = new System.Drawing.Point(386, 2);
            this.lbltarget.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbltarget.Name = "lbltarget";
            this.lbltarget.Size = new System.Drawing.Size(67, 20);
            this.lbltarget.TabIndex = 0;
            this.lbltarget.Text = "Target: 90";
            this.lbltarget.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHART1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CHART1";
            this.Size = new System.Drawing.Size(459, 279);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx3)).EndInit();
            this.panelEx3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFTT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private JPlatform.Client.Controls8.PanelEx panelEx3;
        private DevExpress.XtraCharts.ChartControl chartFTT;
        private JPlatform.Client.Controls8.PanelEx panelEx1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblred;
        private System.Windows.Forms.Label lblyellow;
        private System.Windows.Forms.Label lblgreen;
        private System.Windows.Forms.Label lbltarget;
    }
}
