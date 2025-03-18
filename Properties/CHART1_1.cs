using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace VT.APPS.MES
{
    public partial class CHART1_1 : UserControl
    {
        double red, green, yelow, target;
        bool legend = false;
        bool comment = false;
        string v_p_item;
        public string ChartName   // property
        {
            get { return lblTitle.Text; }   // get method
            set
            {
                //groupControl1.Text = value;
                lblTitle.Text = value;
            }  // set method
        }

        public Color Panel_color   // property
        {
            get { return panel1.BackColor; }   // get method
            set
            {
                //groupControl1.Text = value;
                panel1.BackColor = value;
            }  // set method
        }

        public double ChartColorRed
        {
            get { return red; }
            set
            {
                red = value;
                if (v_p_item != "FTTMAI" && v_p_item != "FTTFHI")
                {
                    lblred.Text = "<=" + red.ToString();
                }                
                else
                {
                    lblred.Text = ">=" + red.ToString();
                }
            }
        }
        public double ChartColorYellow
        {
            get { return yelow; }
            set
            {
                yelow = value;
                if (v_p_item != "FTTMAI" || v_p_item != "FTTFHI")
                {
                    lblyellow.Text = "<=" + yelow.ToString();
                }
                else
                {
                    lblyellow.Text = ">=" + yelow.ToString();
                }
            }
        }
        public double ChartColorGreen
        {
            get { return green; }
            set
            {
                green = value;
                if (v_p_item != "FTTMAI" && v_p_item != "FTTFHI")
                {
                    lblgreen.Text = ">=" + green.ToString();
                }
                else
                {
                    lblgreen.Text = "<=" + green.ToString();
                }
            }
        }

        public double ChartColorTarget
        {
            get { return target; }
            set
            {
                target = value;
                lbltarget.Text = "Target: " + target.ToString();
                ChartColorTargetForeColor = Color.Red;
            }
        }
        public Color ChartColorTargetForeColor
        {
            get { return lbltarget.ForeColor; }
            set
            {
                lbltarget.ForeColor = value;
            }
        }

        public bool Legend_Show
        {
            get { return legend; }
            set
            {
                legend = value;
                if(legend==true)
                    chartFTT.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                else
                    chartFTT.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
                chartFTT.Refresh();
            }
        }

        //public bool Comment_Show
        //{
        //    get { return comment; }
        //    set
        //    {
        //        comment = value;
        //        if (legend == true)
        //            panelEx30.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
        //        else
        //            panelEx30.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        //    }
        //}

        public CHART1_1()
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(235, 236, 239);
        }

        private void groupControl1_CustomDrawCaption(object sender, DevExpress.XtraEditors.GroupCaptionCustomDrawEventArgs e)
        {
            LinearGradientBrush outerBrush = new LinearGradientBrush(e.CaptionBounds,Color.LightGray, Color.LightGray, LinearGradientMode.Vertical);
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

        public void setData(string name, string date_f, string date_t, DataTable dt)
        {
            //date_cnt = (DateTime.ParseExact(date_t, "yyyyMMdd", CultureInfo.InvariantCulture) - DateTime.ParseExact(date_f, "yyyyMMdd", CultureInfo.InvariantCulture)).TotalDays;

            v_p_item = name;
            setData(dt);
        }
        public void setData(DataTable dt)
        {
            DataView vdt1 = new DataView(dt);
            vdt1.RowFilter = "ITEM_CD ='" + v_p_item + "'";
            DataTable dt0 = vdt1.ToTable();
            panel1.BackColor = Color.FromArgb(235, 236, 239);
            if (dt0.Rows.Count > 0)
            {
                //decimal dTotal = decimal.Parse(dt1.Compute("SUM(QTY)", "").ToString());

                chartFTT.Series[0].Points.Clear();
                chartFTT.Series[0].LegendText = chartFTT.Series[0].ArgumentDataMember;
                for (int i = 0; i < dt0.Rows.Count; i++)
                {
                    SeriesPoint point = new SeriesPoint(dt0.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt0.Rows[i]["PASS_PER"].ToString()));
                    chartFTT.Series[0].Points.Add(point);
                }

                chartFTT.Series[1].Points.Clear();
                chartFTT.Series[1].LegendText = chartFTT.Series[0].ArgumentDataMember;
                for (int i = 0; i < dt0.Rows.Count; i++)
                {
                    SeriesPoint point = new SeriesPoint(dt0.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt0.Rows[i]["TARGET_QTY"].ToString()));
                    chartFTT.Series[1].Points.Add(point);
                }
                // label3.Text = dTotal.ToString("0,0") + " Prs";
                lblTitle.Text = dt0.Rows[0]["ITEM_DES"].ToString();
                if (dt0.Rows[0]["ITEM_CD"].ToString() == "ISSUE")
                {
                    chartFTT.Series[0].Name = dt0.Rows[0]["ITEM_CD"].ToString() + " (Case)";
                    chartFTT.Series[1].Visible = false;
                    lbltarget.Visible = false;
                }
                else
                {
                    chartFTT.Series[0].Name = dt0.Rows[0]["ITEM_CD"].ToString() + " - Actual (%)";
                    chartFTT.Series[1].Name = dt0.Rows[0]["ITEM_CD"].ToString() + " - Target (%)";
                }
                ChartColorTarget = double.Parse(dt0.Rows[0]["TARGET_QTY"].ToString());
                ChartColorRed = double.Parse(dt0.Rows[0]["RED_COLOR"].ToString());
                ChartColorYellow = double.Parse(dt0.Rows[0]["YEL_COLOR"].ToString());
                ChartColorGreen = double.Parse(dt0.Rows[0]["GRE_COLOR"].ToString());

                if (dt0.Rows.Count >= 20)
                {
                    //((XYDiagram)chartFTT.Diagram).AxisX.Label.Angle = -90;
                    chartFTT.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
                }
                else
                {
                    //((XYDiagram)chartFTT.Diagram).AxisX.Label.Angle = 0;
                    chartFTT.Series[0].Label.TextOrientation = TextOrientation.Horizontal;
                }
            }
            else
            {
                chartFTT.Series[0].Points.Clear();
            }
        }

        private void chartFTT_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
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
                // If the value is less than 2.5, then fill the bar with green colors.
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
                    drawOptions.Color = Color.FromArgb(255, 217, 102); ;
                }
                // ... if the value is greater, then fill the bar with red colors.
                else
                {
                    options.Color2 = Color.FromArgb(0, 176, 80);
                    //options.Color2 = Color.FromArgb(46, 222, 46);
                    drawOptions.Color = Color.FromArgb(0, 176, 80);
                    drawOptions.Border.Color = Color.FromArgb(0, 176, 80);
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
                    drawOptions.Color = Color.FromArgb(255, 217, 102); ;
                }
                // ... if the value is greater, then fill the bar with red colors.
                else
                {
                    options.Color2 = Color.FromArgb(0, 176, 80);
                    //options.Color2 = Color.FromArgb(46, 222, 46);
                    drawOptions.Color = Color.FromArgb(0, 176, 80);
                    drawOptions.Border.Color = Color.FromArgb(0, 176, 80);
                }
            }
        }
    }
}
