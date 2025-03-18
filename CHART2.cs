using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using System.Collections;
using JPlatform.Client.JBaseForm8;
using JPlatform.Client.TKBaseForm8;
using System.Drawing.Drawing2D;
namespace VT.APPS.MES
{
    public partial class CHART2 : UserControl
    {
        double red, green, yelow, target;
        string v_p_item, v_p_date_f, v_p_date_t;
        bool check_label = false;
        bool legend = false;
        bool comment = false;
        CQCM0029 parrent;
        public string ChartName   // property
        {
            get { return lblTitle.Text; }   // get method
            set
            {
                lblTitle.Text = value;
                //groupControl6.Text = value;
            }  // set method
        }
        public CHART2()
        {
            InitializeComponent();
        }
        private void CHART2_Load(object sender, EventArgs e)
        {
            chartFTT1.CrosshairEnabled = DefaultBoolean.False;
            chartFTT1.RuntimeHitTesting = true;
            panel1.BackColor = Color.FromArgb(235, 236, 239);
        }
        private void groupControl6_CustomDrawCaption(object sender, DevExpress.XtraEditors.GroupCaptionCustomDrawEventArgs e)
        {
            LinearGradientBrush outerBrush = new LinearGradientBrush(e.CaptionBounds,
            Color.LightGray, Color.LightGray, LinearGradientMode.Vertical);
            using (outerBrush)
            {
                e.Cache.FillRectangle(outerBrush, e.CaptionBounds);
            }
            Rectangle innerRect = Rectangle.Inflate(e.CaptionBounds, -3, -3);
            LinearGradientBrush innerBrush = new LinearGradientBrush(innerRect, Color.LightGray,
              Color.LightGray, LinearGradientMode.Vertical);
            using (innerBrush)
            {
                e.Cache.FillRectangle(innerBrush, e.CaptionBounds);
            }
            StringFormat outStrFormat = new StringFormat();
            outStrFormat.Alignment = StringAlignment.Center;
            outStrFormat.LineAlignment = StringAlignment.Center;
            e.Cache.DrawString(e.Info.Caption, e.Info.AppearanceCaption.Font,
              e.Cache.GetSolidBrush(Color.Red), innerRect, outStrFormat);
            e.Handled = true;
        }
        public static string Plant_value { get; set; }
        public bool Legend_Show
        {
            get { return legend; }
            set
            {
                legend = value;
                if (legend == true)
                    chartFTT1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                else
                    chartFTT1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }
        }
        
        public bool Comment_Show
        {
            get { return comment; }
            set
            {
                comment = value;               
                panelEx30.Visible = comment;
               
            }
        }
        private void chartFTT1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Obtain hit information under the test point.
            ChartHitInfo hi = chartFTT1.CalcHitInfo(e.Location);
            // Obtain the series point under the test point.
            SeriesPoint point = hi.SeriesPoint;
            // Check whether the series point was clicked or not.
            if (v_p_item != "FTTFHI" && v_p_item != "FTTMAI")
            { 
                if (point != null  )
                {
                    string argument = "Argument: " + point.Argument.ToString();
                    string plant = point.Argument.ToString();                
                    parrent.runPopup(plant, v_p_date_f, v_p_date_t, v_p_item); 
                }
            }
        } 
        public void setData(string name, string date_f, string date_t, DataTable dt, CQCM0029 p,string opt )
        {
            v_p_item = name;
            v_p_date_f = date_f;
            v_p_date_t = date_t;
            parrent = p;
            //if(opt=="Y")
            //{
            //    chartFTT1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            //}
            //else
            //{
            //    chartFTT1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //}
            setData(dt);
        }
        public void setData(DataTable dt)
        {
            DataView vdt1 = new DataView(dt);
            vdt1.RowFilter = "ITEM_CD ='" + v_p_item + "'";
            DataTable dt1 = vdt1.ToTable();
            if (dt1.Rows.Count > 0)
            {
                //decimal dTotal = decimal.Parse(dt1.Compute("SUM(QTY)", "").ToString());
                chartFTT1.Series[0].Points.Clear();
                chartFTT1.Series[0].LegendText = chartFTT1.Series[0].ArgumentDataMember;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    SeriesPoint point = new SeriesPoint(dt1.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt1.Rows[i]["PASS_PER"].ToString()));
                    chartFTT1.Series[0].Points.Add(point);
                }
                chartFTT1.Series[1].Points.Clear();
                chartFTT1.Series[1].LegendText = chartFTT1.Series[0].ArgumentDataMember;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    SeriesPoint point = new SeriesPoint(dt1.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt1.Rows[i]["TARGET_QTY"].ToString()));
                    chartFTT1.Series[1].Points.Add(point);
                }
                memoFTT.Text = dt1.Rows[0]["COMMENTS"].ToString();
                lblTitle.Text = dt1.Rows[0]["ITEM_DES"].ToString() + " BY PLANT";
                chartFTT1.Titles[0].Text = dt1.Rows[0]["ITEM_DES"].ToString() + " BY PLANT";
                if (dt1.Rows[0]["ITEM_CD"].ToString() == "ISSUE" || dt1.Rows[0]["ITEM_CD"].ToString() == "STQC1") 
                {
                    //chartFTT1.Series[0].Name = dt1.Rows[0]["ITEM_CD"].ToString() + " (Case)";
                    chartFTT1.Legend.Visibility = DefaultBoolean.False;
                    chartFTT1.Series[1].Visible = false;
                    chartFTT1.Series[1].Visible = false;
                }
                else
                {
                    chartFTT1.Series[0].Name = dt1.Rows[0]["ITEM_CD"].ToString() + " - Actual (%)";
                    chartFTT1.Series[1].Name = dt1.Rows[0]["ITEM_CD"].ToString() + " - Target (%)";
                }
                target = double.Parse(dt1.Rows[0]["TARGET_QTY"].ToString());
                red = double.Parse(dt1.Rows[0]["RED_COLOR"].ToString()); 
                yelow = double.Parse(dt1.Rows[0]["YEL_COLOR"].ToString()); 
                green = double.Parse(dt1.Rows[0]["GRE_COLOR"].ToString()); 
            }
            else
            {
                chartFTT1.Series[0].Points.Clear();
            }
        }
        private void chartFTT1_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            // These changes will be applied to Bar Series only.
            BarDrawOptions drawOptions = e.SeriesDrawOptions as BarDrawOptions;
            if (drawOptions == null)
                return;
            // Get the fill options for the series point.
            drawOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Gradient;
            RectangleGradientFillOptions options =
            drawOptions.FillStyle.Options as RectangleGradientFillOptions;
            if (options == null)
                return;
            // Get the value at the current series point.
            double val = e.SeriesPoint[0];
            if (v_p_item == "FTTMAI" || v_p_item == "FTTFHI")
            {
                if (val >= red)
                {
                    //options.Color2 = Color.Red;
                    options.Color2 = Color.FromArgb(255, 100, 100);
                    //drawOptions.Color = Color.Red;
                    drawOptions.Color = Color.FromArgb(255, 100, 100);
                }
                // ... if the value is less than 5.5, then fill the bar with yellow colors.
                else if (val < red & val >= yelow)
                {
                    //options.Color2 = Color.Yellow;
                    options.Color2 = Color.FromArgb(255, 217, 102);
                    //drawOptions.Color = Color.Yellow;
                    drawOptions.Color = Color.FromArgb(255, 217, 102);
                }
                // ... if the value is greater, then fill the bar with red colors.
                else
                {
                    options.Color2 = Color.FromArgb(0, 176, 80);
                    // options.Color2 = Color.FromArgb(0, 192, 0);
                    drawOptions.Color = Color.FromArgb(0, 176, 80);
                    //drawOptions.Border.Color = Color.FromArgb(100, 155, 26, 0);
                }
            }
            else
            {
                // If the value is less than 2.5, then fill the bar with green colors.
                if (val <= red)
                {
                    //options.Color2 = Color.Red;
                    options.Color2 = Color.FromArgb(255, 100, 100);
                    //drawOptions.Color = Color.Red;
                    drawOptions.Color = Color.FromArgb(255, 100, 100);
                }
                // ... if the value is less than 5.5, then fill the bar with yellow colors.
                else if (val > red & val <= yelow)
                {
                    //options.Color2 = Color.Yellow;
                    options.Color2 = Color.FromArgb(255, 217, 102);
                    //drawOptions.Color = Color.Yellow;
                    drawOptions.Color = Color.FromArgb(255, 217, 102);
                }
                // ... if the value is greater, then fill the bar with red colors.
                else
                {
                    options.Color2 = Color.FromArgb(0, 176, 80);
                    // options.Color2 = Color.FromArgb(0, 192, 0);
                    drawOptions.Color = Color.FromArgb(0, 176, 80);
                    //drawOptions.Border.Color = Color.FromArgb(100, 155, 26, 0);
                }
            }
        }
    }
}
