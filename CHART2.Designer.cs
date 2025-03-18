namespace VT.APPS.MES
{
    partial class CHART2
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
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chartFTT1 = new DevExpress.XtraCharts.ChartControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelEx30 = new JPlatform.Client.Controls8.PanelEx();
            this.memoFTT = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFTT1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx30)).BeginInit();
            this.panelEx30.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoFTT.Properties)).BeginInit();
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
            this.groupControl6.Controls.Add(this.panel1);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.chartFTT1);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 326);
            this.panel1.TabIndex = 11;
            // 
            // chartFTT1
            // 
            this.chartFTT1.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.AutoScaleBreaks.Enabled = true;
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("Tahoma", 7F);
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowRotate = false;
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.AllowStagger = false;
            xyDiagram1.AxisX.Label.ResolveOverlappingOptions.MinIndent = 1;
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
            this.chartFTT1.Diagram = xyDiagram1;
            this.chartFTT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartFTT1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Left;
            this.chartFTT1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.TopOutside;
            this.chartFTT1.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT1.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartFTT1.Legend.Font = new System.Drawing.Font("Tahoma", 7F);
            this.chartFTT1.Legend.Margins.Bottom = 0;
            this.chartFTT1.Legend.Margins.Left = 0;
            this.chartFTT1.Legend.Margins.Right = 0;
            this.chartFTT1.Legend.Margins.Top = 0;
            this.chartFTT1.Legend.MarkerSize = new System.Drawing.Size(20, 15);
            this.chartFTT1.Legend.UseCheckBoxes = true;
            this.chartFTT1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT1.Location = new System.Drawing.Point(0, 25);
            this.chartFTT1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartFTT1.Name = "chartFTT1";
            this.chartFTT1.Padding.Bottom = 0;
            this.chartFTT1.Padding.Left = 0;
            this.chartFTT1.Padding.Right = 0;
            this.chartFTT1.Padding.Top = 0;
            this.chartFTT1.PaletteName = "Flow";
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
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series2.Name = "Targetl (%)";
            lineSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            lineSeriesView1.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dash;
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.False;
            series2.View = lineSeriesView1;
            this.chartFTT1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartFTT1.Size = new System.Drawing.Size(736, 301);
            this.chartFTT1.TabIndex = 24;
            chartTitle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            chartTitle1.Indent = 0;
            chartTitle1.Text = "BY PLANT";
            chartTitle1.TextColor = System.Drawing.Color.Black;
            chartTitle1.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartFTT1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            this.chartFTT1.CustomDrawSeriesPoint += new DevExpress.XtraCharts.CustomDrawSeriesPointEventHandler(this.chartFTT1_CustomDrawSeriesPoint);
            this.chartFTT1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chartFTT1_MouseDoubleClick);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(736, 25);
            this.lblTitle.TabIndex = 23;
            this.lblTitle.Text = "FTT";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelEx30
            // 
            this.panelEx30.Controls.Add(this.memoFTT);
            this.panelEx30.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx30.Location = new System.Drawing.Point(0, 326);
            this.panelEx30.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelEx30.Name = "panelEx30";
            this.panelEx30.Size = new System.Drawing.Size(736, 49);
            this.panelEx30.TabIndex = 0;
            this.panelEx30.Visible = false;
            // 
            // memoFTT
            // 
            this.memoFTT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoFTT.EditValue = "";
            this.memoFTT.Location = new System.Drawing.Point(2, 2);
            this.memoFTT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoFTT.Name = "memoFTT";
            this.memoFTT.Size = new System.Drawing.Size(732, 45);
            this.memoFTT.TabIndex = 0;
            // 
            // CHART2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl6);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CHART2";
            this.Size = new System.Drawing.Size(736, 375);
            this.Load += new System.EventHandler(this.CHART2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartFTT1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx30)).EndInit();
            this.panelEx30.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoFTT.Properties)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraCharts.ChartControl chartFTT1;
        private System.Windows.Forms.Label lblTitle;
        private JPlatform.Client.Controls8.PanelEx panelEx30;
        private DevExpress.XtraEditors.MemoEdit memoFTT;
    }
}
