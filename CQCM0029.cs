using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using JPlatform.Client.Library8.interFace;
using JPlatform.Client;
using JPlatform.Client.Controls8;
using JPlatform.Client.JBaseForm8;
using JPlatform.Client.TKBaseForm8;
using Miracom.CliFrx;
using DevExpress.XtraCharts;
using TK.TDSVinaFunctions;
namespace VT.APPS.MES
{
    public partial class CQCM0029 : TKBaseForm
    {
        private bool _isActivated = false;
        public CQCM0029()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //폼로드 이벤트시 공용버튼컨트롤에 대한 설정
            NewButton = false;
            DeleteButton = false;
            PreviewButton = false;
            PrintButton = false;
            // yymmdd_F.EditValue = CurrentDate("yyyy-mm-dd");
            yymmdd_F.EditValue = DateTime.Today.Month + "-01";
            yymmdd_T.EditValue = CurrentDate("yyyy-mm-dd");
            SetLookUpControl("PLANT");
            show_opt();
            TK.TDSVinaFunctions.TDSTool.CheckDEV(this, ServiceInfo.TK_AMES_MESMGR, SessionInfo.UserID);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this._isActivated = true;
        }

        public override void QueryClick()
        {
            if(chNikeSystem.Checked == true)
            {
                fnQRY_P_CQCM0029_Q("Q1");
                
            }
            else
            {
                fnQRY_P_CQCM0029_Q("Q");
            }
            
        }

        private bool fnQRY_P_CQCM0029_Q(string strWorkType)
        {
            if (!ValidateControls(panMain))
                return false;
            try
            {
                // 비즈니스 로직 정보
                P_CQCM0029_Q cProc = new P_CQCM0029_Q();
                DataTable dtData = null;
                //string strWorkType = ""; 
                string strViewType = ""; // V_P_VIEW_TYPE
                //string DateF = "";
                //string DateT = "";
                //string coLor = "";
                if (radioD.Checked)
                {
                    //strWorkType = "Q";
                    strViewType = "DAILY";
                }
                else if (radioW.Checked)
                {
                    //strWorkType = "Q";
                    strViewType = "WEEKLY";
                }
                else if (radioM.Checked)
                {
                    //strWorkType = "Q_M";
                    strViewType = "MONTHLY";
                }
                else if (radioY.Checked)
                {
                    //strWorkType = "Q_Y";
                    strViewType = "YEARLY";
                }
                dtData = cProc.SetParamData(dtData,
                            strWorkType, //"Q", //strWorkType
                            MPGV.gsFactory,
                            cboPlant.EditValue.ToString().Trim(),
                            DateTime.Parse(yymmdd_F.Text).ToString("yyyyMMdd"),
                            DateTime.Parse(yymmdd_T.Text).ToString("yyyyMMdd"),
                            "",
                            strViewType
                            );
                ResultSet rs = CommonCallQuery(/*ServiceInfo.MESBizDB*/ ServiceInfo.TK_AMES_MESMGR, dtData, cProc.ProcName, cProc.GetParamInfo());

                if (rs != null)
                {
                    DataTable dt1 = rs.ResultDataSet.Tables[0];
                    DataTable dt2 = rs.ResultDataSet.Tables[1];
                    DataTable dt3 = rs.ResultDataSet.Tables[2];
                    DataTable dt4 = rs.ResultDataSet.Tables[3];
                    DataTable dt5 = rs.ResultDataSet.Tables[4];
                    DataTable dt6 = rs.ResultDataSet.Tables[5];
                    DataTable dt7 = rs.ResultDataSet.Tables[6];
                    DataTable dt8 = rs.ResultDataSet.Tables[7];

                    // ========= chart 1 ==================
                    if (dt1.Rows.Count > 0)
                    {
                        //DataView vdt1 = new DataView(dt);
                        //vdt1.RowFilter = "ITEM_CD ='" + v_p_item + "'";
                        //DataTable dt0 = vdt1.ToTable();
                        //decimal dTotal = decimal.Parse(dt1.Compute("SUM(QTY)", "").ToString());

                        //string Df_byFCT = "DEFECT BY FCT (";
                        //lblByFCT.Text = "% PASS RATE BY FCT(" + strViewType + ")";
                        if (strViewType == "DAILY")
                        {
                            lblByFCT.ForeColor = Color.FromArgb(0, 176, 80);
                        }
                        else if (strViewType == "WEEKLY")
                        {
                            lblByFCT.ForeColor = Color.FromArgb(255, 224, 192);
                        }
                        else if (strViewType == "MONTHLY")
                        {
                            lblByFCT.ForeColor = Color.FromArgb(255, 192, 128);
                        }
                        else
                        {
                            lblByFCT.ForeColor = Color.FromArgb(255, 255, 128);
                        }

                        DataView vdt1 = new DataView(dt1);
                        vdt1.RowFilter = "FCT= 'TK_VT1'";
                        DataTable dt_VT1 = vdt1.ToTable();

                        DataView vdt2 = new DataView(dt1);
                        vdt2.RowFilter = "FCT= 'TK_VT2'";
                        DataTable dt_VT2 = vdt2.ToTable();

                        DataView vdt3 = new DataView(dt1);
                        vdt3.RowFilter = "FCT= 'AVG'";
                        DataTable dt_AVG = vdt3.ToTable();

                        chart1.Series[0].Points.Clear();
                        chart1.Series[0].LegendText = chart1.Series[0].ArgumentDataMember;
                        for (int i = 0; i < dt_VT1.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(dt_VT1.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt_VT1.Rows[i]["DEFECT_RATE"].ToString()));
                            chart1.Series[0].Points.Add(point);
                        }

                        chart1.Series[1].Points.Clear();
                        chart1.Series[1].LegendText = chart1.Series[1].ArgumentDataMember;
                        for (int i = 0; i < dt_VT2.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(dt_VT2.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt_VT2.Rows[i]["DEFECT_RATE"].ToString()));
                            chart1.Series[1].Points.Add(point);
                        }

                        chart1.Series[2].Points.Clear();
                        chart1.Series[2].LegendText = chart1.Series[2].ArgumentDataMember;
                        for (int i = 0; i < dt_AVG.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(dt_AVG.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(dt_AVG.Rows[i]["DEFECT_RATE"].ToString()));
                            chart1.Series[2].Points.Add(point);
                        }
                    }
                    else
                    {
                        chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();
                        chart1.Series[2].Points.Clear();
                        chart1.Invalidate();
                    }

                    // ========================== chart 2 =============================
                    if (dt2.Rows.Count > 0)
                    {
                        lblByDaily.Text = "DEFECT QTY BY TYPE(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")";

                        chart2.Series[0].Points.Clear();
                        chart2.Series[0].LegendText = chart2.Series[0].ArgumentDataMember;
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(dt2.Rows[i]["DEFECT_CODE"].ToString(), decimal.Parse(dt2.Rows[i]["DEFECT_QTY"].ToString()));
                            chart2.Series[0].Points.Add(point);
                        }
                    }
                    else
                    {
                        chart2.Series[0].Points.Clear();
                    }

                    // ============================ chart 3 ============================
                    if (dt3.Rows.Count > 0)
                    {
                        //string Titles1 = "Top 3 Defect Type By ";
                        //Titles1 += strViewType;
                        lblTop5Model.Text = "TOP 5 DEFECT QTY BY MODEL(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")";
                        //if (strViewType == "DAILY")
                        //{
                        //    lblTop5Model.ForeColor = Color.FromArgb(0, 176, 80);
                        //}
                        //else if (strViewType == "WEEKLY")
                        //{
                        //    lblTop5Model.ForeColor = Color.FromArgb(255, 224, 192);
                        //}
                        //else if (strViewType == "MONTHLY")
                        //{
                        //    lblTop5Model.ForeColor = Color.FromArgb(255, 192, 128);
                        //}
                        //else
                        //{
                        //    lblTop5Model.ForeColor = Color.FromArgb(255, 255, 128);
                        //}

                        chart3.Series[0].Points.Clear();
                        chart3.Series[0].LegendText = chart3.Series[0].ArgumentDataMember;
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(dt3.Rows[i]["MODEL_DESC"].ToString(), decimal.Parse(dt3.Rows[i]["TOTAL_DEFECT"].ToString()));
                            chart3.Series[0].Points.Add(point);
                            point.ToolTipHint = dt3.Rows[i]["COL_LIST"].ToString().Replace("|", "\n");
                        }
                        chart3.Series[0].CrosshairLabelPattern = "{A}: {V} \n\n{HINT}";
                    }
                    else
                    {
                        chart3.Series[0].Points.Clear();
                    }

                    // ============== chart 4 =================           
                    if (dt4.Rows.Count > 0)
                    {
                        chart4.Series.Clear();

                        chart4.DataSource = dt4;
                        chart4.SeriesDataMember = "DEFECT_CODE";
                        chart4.SeriesTemplate.ArgumentDataMember = "DATE_SHOW";
                        chart4.SeriesTemplate.ValueDataMembers[0] = "DEFECT_PERCENT"; // "DEFECT_QTY";

                        // HIEN DUNG THU TU W1 -> 10

                        XYDiagram diagram = chart4.Diagram as XYDiagram;
                        //diagram.AxisX.Label.TextPattern = "{A} Uhr";
                        diagram.AxisX.QualitativeScaleComparer = new CaseInsensitiveComparer(); // Import for sorting in xAxis otherwise wrong sorting
                        diagram.AxisX.NumericScaleOptions.GridSpacing = 1;

                        foreach (Series series in chart4.Series)
                        {
                            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                            series.Label.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
                            series.Label.FillStyle.FillMode = FillMode.Empty;
                            series.Label.Font = new Font("Tahoma", 10, FontStyle.Bold); // | FontStyle.Italic

                            //if (series.Name == "Temperature")
                            //{
                            //    LineSeriesView seriesView = (series.View as LineSeriesView);
                            //    if (seriesView == null) break;
                            //    seriesView.AxisX = diagram.SecondaryAxesX[0];
                            //}
                        }

                        //chart4.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                        //    BarSeriesLabel seriesLabel = chart4.Series[0].Label as BarSeriesLabel;
                        //    seriesLabel.TextPattern = "{V}";

                        //    DataView vdt1 = new DataView(dt4);
                        //    vdt1.RowFilter = "DEF_SEQ ='1'";
                        //    DataTable dt_T1 = vdt1.ToTable();

                        //    DataView vdt2 = new DataView(dt4);
                        //    vdt2.RowFilter = "DEF_SEQ ='2'";
                        //    DataTable dt_T2 = vdt2.ToTable();

                        //    DataView vdt3 = new DataView(dt4);
                        //    vdt3.RowFilter = "DEF_SEQ ='3'";
                        //    DataTable dt_T3 = vdt3.ToTable();


                        //    for (int i = 0; i < dt_T1.Rows.Count; i++)
                        //    {
                        //        SeriesPoint point = new SeriesPoint(dt_T1.Rows[i]["DATE_SHOW"].ToString(), decimal.Parse(dt_T1.Rows[i]["DEFECT_QTY"].ToString()));
                        //        chart4.Series[0].Points.Add(point);
                        //    }
                        //    chart4.Series[0].LegendText = chart4.Series[0].ArgumentDataMember;

                        //    for (int i = 0; i < dt_T2.Rows.Count; i++)
                        //    {
                        //        SeriesPoint point = new SeriesPoint(dt_T2.Rows[i]["DATE_SHOW"].ToString(), decimal.Parse(dt_T2.Rows[i]["DEFECT_QTY"].ToString()));//( dt4.Columns[i].ColumnName, dt4.Rows[0][i].ToString());
                        //        chart4.Series[1].Points.Add(point);
                        //    }
                        //    chart4.Series[1].LegendText = chart4.Series[1].ArgumentDataMember;
                        //    for (int i = 0; i < dt_T3.Rows.Count; i++)
                        //    {
                        //        SeriesPoint point = new SeriesPoint(dt_T3.Rows[i]["DATE_SHOW"].ToString(), decimal.Parse(dt_T3.Rows[i]["DEFECT_QTY"].ToString()));//( dt4.Columns[i].ColumnName, dt4.Rows[0][i].ToString());
                        //        chart4.Series[2].Points.Add(point);
                        //    }
                        //    chart4.Series[2].LegendText = chart4.Series[2].ArgumentDataMember;
                    }
                    else
                    {
                        chart4.Series.Clear();
                    }


                    // ============ chart 5 ===================
                    if (dt5.Rows.Count > 0)
                    {
                        lblByHour.Text = cboPlant.EditValue.ToString().Trim() == ""?
                                        "% PASS RATE BY PLANT(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")" :
                                        "% PASS RATE (" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")";

                        DataView _vdt1 = new DataView(dt5);
                        _vdt1.RowFilter = "FCT ='TK_VT1'";
                        DataTable _dt_VT1 = _vdt1.ToTable();

                        DataView _vdt2 = new DataView(dt5);
                        _vdt2.RowFilter = "FCT ='TK_VT2'";
                        DataTable _dt_VT2 = _vdt2.ToTable();

                        DataView _vdt3 = new DataView(dt5);
                        _vdt3.RowFilter = "FCT ='AVG'";
                        DataTable _dt_AVG = _vdt3.ToTable();

                        chart5.Series[0].Points.Clear();
                        chart5.Series[0].LegendText = chart5.Series[0].ArgumentDataMember;
                        for (int i = 0; i < _dt_VT1.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(_dt_VT1.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(_dt_VT1.Rows[i]["DEFECT_RATE"].ToString()));
                            chart5.Series[0].Points.Add(point);
                        }

                        chart5.Series[1].Points.Clear();
                        chart5.Series[1].LegendText = chart5.Series[1].ArgumentDataMember;
                        for (int i = 0; i < _dt_VT2.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(_dt_VT2.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(_dt_VT2.Rows[i]["DEFECT_RATE"].ToString()));
                            chart5.Series[1].Points.Add(point);
                        }

                        chart5.Series[2].Points.Clear();
                        chart5.Series[2].LegendText = chart5.Series[2].ArgumentDataMember;
                        for (int i = 0; i < _dt_AVG.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(_dt_AVG.Rows[i]["WORK_DATE"].ToString(), decimal.Parse(_dt_AVG.Rows[i]["DEFECT_RATE"].ToString()));
                            chart5.Series[2].Points.Add(point);
                        }
                    }
                    else
                    {
                        chart5.Series[0].Points.Clear();
                        chart5.Series[1].Points.Clear();
                        chart5.Series[2].Points.Clear();
                    }

                    // ================== chart 6 ===================
                    if (dt6.Rows.Count > 0)
                    {
                        lblTopLine.Text = cboPlant.EditValue.ToString().Trim() == "" ?
                                        "TOP 3 PLANT DEFECT RATE\n(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")" :
                                        "TOP 3 LINE DEFECT RATE\n(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")";

                        //lblTopLine.Text = "TOP 3 PLANT DEFECT RATE\n(" + strViewType + " BASE REAL TIME)"; 
                        /* if (strViewType == "DAILY")
                         {
                             lblTopLine.ForeColor = Color.FromArgb(0, 176, 80);
                         }
                         else if (strViewType == "WEEKLY")
                         {
                             lblTopLine.ForeColor = Color.FromArgb(255, 224, 192);
                         }
                         else if (strViewType == "MONTHLY")
                         {
                             lblTopLine.ForeColor = Color.FromArgb(255, 192, 128);
                         }
                         else
                         {
                             lblTopLine.ForeColor = Color.FromArgb(255, 255, 128);
                         }*/

                        string P_A, P_B, P_C;

                        P_A = dt6.Rows[0]["PLANT_ID"].ToString() + "\nDEFECT RATE: " + Convert.ToString(dt6.Rows[0]["DEFECT_RATE"].ToString()) + "%";
                        P_B = dt6.Rows[1]["PLANT_ID"].ToString() + "\nDEFECT RATE: " + Convert.ToString(dt6.Rows[1]["DEFECT_RATE"].ToString()) + "%";
                        P_C = dt6.Rows[2]["PLANT_ID"].ToString() + "\nDEFECT RATE: " + Convert.ToString(dt6.Rows[2]["DEFECT_RATE"].ToString()) + "%";

                        txtPlant_1.Text = P_A;
                        txtPlant_2.Text = P_B;
                        txtPlant_3.Text = P_C;
                    }
                    else
                    {
                        txtPlant_1.Text = "PLANT_" + "\nDEFECT RATE: ";
                        txtPlant_2.Text = "PLANT_" + "\nDEFECT RATE: ";
                        txtPlant_3.Text = "PLANT_" + "\nDEFECT RATE: ";
                    }

                    // =============== chart 7 ====================
                    if (dt7.Rows.Count > 0)
                    {
                        lblTopCode.Text = cboPlant.EditValue.ToString().Trim() == "" ?
                                        "TOP 5 DEFECT RATE \n(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")" :
                                        "TOP 5 DEFECT RATE BY " + cboPlant.EditValue.ToString().Trim() + "\n(" + yymmdd_F.Text.ToString() + " ~ " + yymmdd_T.Text.ToString() + ")";

                        //lblTopCode.Text = "TOP 5 DEFECT RATE BY TYPE\n(" + strViewType + " BASE REAL TIME)";
                        /*if (strViewType == "DAILY")
                        {
                            lblTopCode.ForeColor = Color.FromArgb(0, 176, 80);
                        }
                        else if (strViewType == "WEEKLY")
                        {
                            lblTopCode.ForeColor = Color.FromArgb(255, 224, 192);
                        }
                        else if (strViewType == "MONTHLY")
                        {
                            lblTopCode.ForeColor = Color.FromArgb(255, 192, 128);
                        }
                        else
                        {
                            lblTopCode.ForeColor = Color.FromArgb(255, 255, 128);
                        }*/
                        //Series series1 = new Series("chart7", ViewType.Doughnut);

                        //series8.SeriesPointsSorting = SortingMode.None;
                        //series8.SeriesPointsSortingKey = SeriesPointKey.Argument;

                        chart7.Series[0].Points.Clear();
                        chart7.Series[0].LegendText = chart7.Series[0].ArgumentDataMember;
                        for (int i = 0; i < dt7.Rows.Count; i++)
                        {
                            SeriesPoint point = new SeriesPoint(dt7.Rows[i]["DEFECT_CODE"].ToString(), decimal.Parse(dt7.Rows[i]["DEFECT_RATE"].ToString()));
                            chart7.Series[0].Points.Add(point);
                        }
                    }
                    else
                    {
                        chart7.Series[0].Points.Clear();
                    }

                    // ================== chart 8 ===================
                    if (dt8.Rows.Count > 0)
                    {
                        string PV_A, PV_B, PV_C;

                        PV_A = dt8.Rows[0]["PLANT_ID"].ToString() + "(" + dt8.Rows[0]["LINE_CODE"].ToString() + ")" + "\n";
                        PV_A += "LAST WEEK: " + dt8.Rows[0]["W_1_DEFECT_RATE"].ToString() + "%" + "\n";
                        PV_A += "THIS WEEK: " + dt8.Rows[0]["W_DEFECT_RATE"].ToString() + "%";


                        PV_B = dt8.Rows[1]["PLANT_ID"].ToString() + "(" + dt8.Rows[1]["LINE_CODE"].ToString() + ")" + "\n";
                        PV_B += "LAST WEEK: " + dt8.Rows[1]["W_1_DEFECT_RATE"].ToString() + "%" + "\n";
                        PV_B += "THIS WEEK: " + dt8.Rows[1]["W_DEFECT_RATE"].ToString() + "%";


                        PV_C = dt8.Rows[2]["PLANT_ID"].ToString() + "(" + dt8.Rows[2]["LINE_CODE"].ToString() + ")" + "\n";
                        PV_C += "LAST WEEK: " + dt8.Rows[2]["W_1_DEFECT_RATE"].ToString() + "%" + "\n";
                        PV_C += "THIS WEEK: " + dt8.Rows[2]["W_DEFECT_RATE"].ToString() + "%";

                        txtP_LINE1.Text = PV_A;
                        txtP_LINE2.Text = PV_B;
                        txtP_LINE3.Text = PV_C;
                    }
                    else
                    {
                        txtP_LINE1.Text = "PLANT_" + "\nLAST WEEK: " + "\nTHIS WEEK: ";
                        txtP_LINE2.Text = "PLANT_" + "\nLAST WEEK: " + "\nTHIS WEEK: ";
                        txtP_LINE3.Text = "PLANT_" + "\nLAST WEEK: " + "\nTHIS WEEK: ";
                    }
                }
                cProc = null;
                return true;
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
                return false;
            }
        }

        public void runPopup(string _plant, string _datef, string _datet, string _item)
        {
            Hashtable ht = new Hashtable();
            ht.Add("FACTORY", MPGV.gsFactory);
            ht.Add("SUB_PLANT", _plant);
            ht.Add("V_P_DATE_F", _datef);
            ht.Add("V_P_DATE_T", _datet);
            ht.Add("V_P_ITEM", _item);
            //OpenPopupForm(@"\MES\VT.APPS.MES.CQMS0002_01.dll", ht);
            if (this.Modal)
            {
                OpenChildForm(@"\DEV\VT.APPS.MES.CQMS0002_01.dll", ht, OpenType.Modal);
            }
            else
            {
                OpenChildForm(@"\MES\VT.APPS.MES.CQMS0002_01.dll", ht, OpenType.Modal);
            }
        }

        private bool SetLookUpControl(string LookUpId)
        {
            try
            {
                ///조회조건
                string sCondition = string.Empty;
                string sPntConds = string.Empty;
                switch (LookUpId)
                {
                    case "PLANT":
                        //LookupDataInit(cboPlantId);
                            sPntConds = "FACTORY = '" + MPGV.gsFactory + "'   "
                                      + "AND TABLE_NAME = 'CCOM_PLANT'   "
                                      + "AND DATA_3 IN ('VT1', 'VT2')    "
                                      + "AND KEY_1 NOT IN ('NIKE_ID', 'PLANT_VR1', 'VT1-NMI1')   "
                                      + "AND DATA_5 = 'FG'    "
                                      //+ string.Format("AND DATA_3 LIKE  DECODE('{0}', '', '%', '{0}')", cboWoFCT.EditValue.ToString() == "" ? _PlantID : cboWoFCT.EditValue)
                                      + "ORDER BY KEY_1";
                            SetLookUp(cboPlant, "", "VIEW_GCM_CMN_ID", sPntConds, true);

                            cboPlant.Properties.Columns[1].Visible = false;
                            cboPlant.Properties.BestFit();
                        break;

                    //case "PLANT_NIKE":
                    //    DataTable dtPlant = new DataTable();
                    //    string SQL = string.Empty;

                    //    SQL = "SELECT SUBSTR(KEY_1, INSTR(KEY_1, '_') + 1) DATA  "
                    //        + "FROM MGCMLAGDAT   "
                    //        + "WHERE FACTORY = 'VT'   "
                    //        + "AND TABLE_NAME = 'CCOM_PLANT'   "
                    //        + "AND DATA_3 IN ('VT1', 'VT2')    "
                    //        + "AND KEY_1 NOT IN ('NIKE_ID', 'PLANT_VR1', 'VT1-NMI1')   "
                    //        + "AND DATA_5 = 'FG'    "
                    //        + "ORDER BY KEY_1 ";
                    //    ResultSet rs_etc = CommonDirectSQL(ServiceInfo.TK_AMES_MESMGR, SQL);
                    //    dtPlant = rs_etc.ResultDataSet.Tables[0];

                    //    cboPlant.Properties.DataSource = dtPlant;
                    //    cboPlant.Properties.DisplayMember = "DATA";
                    //    cboPlant.Properties.ValueMember = "DATA";

                    //break;
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
                return false;
            }
            return true;
        }

        private void show_opt()
        {
            //if (checkBox1.Checked == true)
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1";
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Legend_Show = true;
            //        if (s == "FTT" || s == "MA_RESULT" || s == "3IZ" || s == "MA1") // FTTMAI  || s == "HI" 
            //        {
            //            (tableLayoutPanel1.Controls["c" + s] as CHART1_1).Legend_Show = true;
            //        }
            //    }
            //}
            //else
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1";
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Legend_Show = false;
            //        if (s == "FTT" || s == "MA_RESULT" || s == "3IZ" || s == "MA1") // FTTMAI  || s == "HI" 
            //        {
            //            (tableLayoutPanel1.Controls["c" + s] as CHART1_1).Legend_Show = false;
            //        }
            //    }
            //}
            //if (checkBox2.Checked == true)
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1";
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Comment_Show = true;
            //    }
            //}
            //else
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1";
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Comment_Show = false;
            //    }
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked==true) 
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1";
            //    foreach (string s in lst.Split(';'))
            //    {      
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Legend_Show=true;
            //        if (s == "FTT" || s == "MA_RESULT" || s == "3IZ" || s == "MA1") // FTTMAI  || s == "HI" 
            //        {
            //            (tableLayoutPanel1.Controls["c" + s] as CHART1_1).Legend_Show=true;
            //        }                                      
            //    }
            //}
            //else 
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1"; //FTTFHI    HI     FTTMAI        
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Legend_Show = false;
            //        if (s == "FTT" || s == "MA_RESULT" || s == "3IZ" || s == "MA1") // FTTMAI  || s == "HI" 
            //        {
            //            (tableLayoutPanel1.Controls["c" + s] as CHART1_1).Legend_Show = false;
            //        }                  
            //    }
            //}
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked == true)
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1";//FTTMAI HI FTTFHI
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Comment_Show = true;                  
            //    }
            //}
            //else
            //{
            //    string lst = "3IZ;FTT;MA_RESULT;ISSUE;MA1;STQC1"; //FTTMAI HI FTTFHI
            //    foreach (string s in lst.Split(';'))
            //    {
            //        (tableLayoutPanel1.Controls["c" + s + "1"] as CHART2).Comment_Show = false;
            //    }
            //}
        }

        private void chNikeSystem_CheckedChanged(object sender, EventArgs e)
        {
            //if (chNikeSystem.Checked == true)
            //{
            //    SetLookUpControl("PLANT_NIKE");
            //}
            //else
            //{
            //    SetLookUpControl("PLANT");
            //}
        }
    }
}
