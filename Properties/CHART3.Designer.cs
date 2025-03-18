namespace VT.APPS.MES
{
    partial class CHART3
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
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.panelEx26 = new JPlatform.Client.Controls8.PanelEx();
            this.chartFTT3 = new DevExpress.XtraCharts.ChartControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelEx30 = new JPlatform.Client.Controls8.PanelEx();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx26)).BeginInit();
            this.panelEx26.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFTT3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx30)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl6
            // 
            this.groupControl6.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl6.AppearanceCaption.ForeColor = System.Drawing.Color.Red;
            this.groupControl6.AppearanceCaption.Options.UseFont = true;
            this.groupControl6.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl6.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl6.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl6.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.groupControl6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.groupControl6.Controls.Add(this.panelEx26);
            this.groupControl6.Controls.Add(this.panelEx30);
            this.groupControl6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl6.Location = new System.Drawing.Point(0, 0);
            this.groupControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.ShowCaption = false;
            this.groupControl6.Size = new System.Drawing.Size(736, 375);
            this.groupControl6.TabIndex = 6;
            this.groupControl6.Text = "FTT BY PLANT";
            this.groupControl6.CustomDrawCaption += new DevExpress.XtraEditors.GroupCaptionCustomDrawEventHandler(this.groupControl6_CustomDrawCaption);
            // 
            // panelEx26
            // 
            this.panelEx26.Controls.Add(this.chartFTT3);
            this.panelEx26.Controls.Add(this.panel1);
            this.panelEx26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx26.Location = new System.Drawing.Point(0, 0);
            this.panelEx26.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelEx26.Name = "panelEx26";
            this.panelEx26.Size = new System.Drawing.Size(736, 375);
            this.panelEx26.TabIndex = 3;
            // 
            // chartFTT3
            // 
            this.chartFTT3.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.AutoScaleBreaks.Enabled = true;
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("Tahoma", 7F);
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            xyDiagram1.AxisX.LabelVisibilityMode = DevExpress.XtraCharts.AxisLabelVisibilityMode.AutoGeneratedAndCustom;
            xyDiagram1.AxisX.ScaleBreakOptions.SizeInPixels = 2;
            xyDiagram1.AxisX.Tickmarks.MinorVisible = false;
            xyDiagram1.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.VisualRange.Auto = false;
            xyDiagram1.AxisX.VisualRange.MaxValueSerializable = "9";
            xyDiagram1.AxisX.VisualRange.MinValueSerializable = "0";
            xyDiagram1.AxisX.WholeRange.Auto = false;
            xyDiagram1.AxisX.WholeRange.MaxValueSerializable = "9";
            xyDiagram1.AxisX.WholeRange.MinValueSerializable = "0";
            xyDiagram1.AxisY.Label.Font = new System.Drawing.Font("Tahoma", 7F);
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BorderVisible = false;
            this.chartFTT3.Diagram = xyDiagram1;
            this.chartFTT3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartFTT3.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Left;
            this.chartFTT3.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.TopOutside;
            this.chartFTT3.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT3.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartFTT3.Legend.Font = new System.Drawing.Font("Tahoma", 7F);
            this.chartFTT3.Legend.Margins.Bottom = 0;
            this.chartFTT3.Legend.Margins.Left = 0;
            this.chartFTT3.Legend.Margins.Right = 0;
            this.chartFTT3.Legend.Margins.Top = 0;
            this.chartFTT3.Legend.MarkerSize = new System.Drawing.Size(20, 15);
            this.chartFTT3.Legend.UseCheckBoxes = true;
            this.chartFTT3.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT3.Location = new System.Drawing.Point(2, 27);
            this.chartFTT3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartFTT3.Name = "chartFTT3";
            this.chartFTT3.PaletteName = "Flow";
            sideBySideBarSeriesLabel1.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            sideBySideBarSeriesLabel1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            sideBySideBarSeriesLabel1.Font = new System.Drawing.Font("Tahoma", 7F);
            sideBySideBarSeriesLabel1.LineLength = 2;
            sideBySideBarSeriesLabel1.Position = DevExpress.XtraCharts.BarSeriesLabelPosition.Top;
            sideBySideBarSeriesLabel1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.Label = sideBySideBarSeriesLabel1;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.Name = "Actual (%)";
            sideBySideBarSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(80)))));
            series1.View = sideBySideBarSeriesView1;
            this.chartFTT3.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartFTT3.Size = new System.Drawing.Size(732, 346);
            this.chartFTT3.TabIndex = 18;
            chartTitle1.Alignment = System.Drawing.StringAlignment.Far;
            chartTitle1.Font = new System.Drawing.Font("Tahoma", 10F);
            chartTitle1.Text = "Daily FTT Plant";
            chartTitle1.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT3.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            this.chartFTT3.CustomDrawSeriesPoint += new DevExpress.XtraCharts.CustomDrawSeriesPointEventHandler(this.chartFTT_CustomDrawSeriesPoint);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(732, 25);
            this.panel1.TabIndex = 17;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(732, 25);
            this.lblTitle.TabIndex = 22;
            this.lblTitle.Text = "FTT";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelEx30
            // 
            this.panelEx30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx30.Location = new System.Drawing.Point(0, 0);
            this.panelEx30.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelEx30.Name = "panelEx30";
            this.panelEx30.Size = new System.Drawing.Size(736, 375);
            this.panelEx30.TabIndex = 0;
            // 
            // CHART3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl6);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CHART3";
            this.Size = new System.Drawing.Size(736, 375);
            this.Load += new System.EventHandler(this.CHART2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx26)).EndInit();
            this.panelEx26.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFTT3)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelEx30)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private JPlatform.Client.Controls8.PanelEx panelEx26;
        private JPlatform.Client.Controls8.PanelEx panelEx30;
        private DevExpress.XtraCharts.ChartControl chartFTT3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
    }
}
