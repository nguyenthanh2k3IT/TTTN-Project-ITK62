using DevExpress.Utils;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using JPlatform.Client.ChildForm8;
using JPlatform.Client.Controls8;
using JPlatform.Client.JBaseForm8;
using JPlatform.Client.Library8.interFace;
using JPlatform.Client.TKBaseForm8;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace TK.TDSVinaFunctions
{
    public class TDSProc : BaseProcClass
    {
        private DataTable dtData;
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện để khởi tạo TDSProc khi các tham số có nhiều loại varchar, number,blob.       
        /// </summary>
        public TDSProc(string ProcName, params TDSProcParam[] lparam)
        {
            _ProcName = ProcName;
            dtData = new DataTable(_ProcName);
            foreach (TDSProcParam pr in lparam)
            {
                switch (pr.Type)
                {
                    case TDSProcParam.TypeDecimal:
                        _ParamInfo.Add(new ParamInfo(pr.getKey, "Number", 0, "Input", typeof(System.Decimal)));//Decimal
                        dtData.Columns.Add(pr.getKey, typeof(System.Decimal));
                        break;
                    case TDSProcParam.TypeString:
                        _ParamInfo.Add(new ParamInfo(pr.getKey, "Varchar2", 0, "Input", typeof(System.String)));
                        dtData.Columns.Add(pr.getKey, typeof(System.String));
                        break;
                    case TDSProcParam.TypeCLOB:
                        _ParamInfo.Add(new ParamInfo(pr.getKey, "Clob", 0, "Input", typeof(System.Byte[])));
                        dtData.Columns.Add(pr.getKey, typeof(System.Byte[]));
                        break;
                    case TDSProcParam.TypeBLOB:
                        _ParamInfo.Add(new ParamInfo(pr.getKey, "Blob", 0, "Input", typeof(System.Byte[])));
                        dtData.Columns.Add(pr.getKey, typeof(System.Byte[]));
                        break;
                    case TDSProcParam.TypeDateTime:
                        _ParamInfo.Add(new ParamInfo(pr.getKey, "Date", 0, "Input", typeof(System.DateTime)));
                        dtData.Columns.Add(pr.getKey, typeof(System.DateTime));
                        break;
                }
            }
            DataRow dr = dtData.Rows.Add();
            foreach (TDSProcParam pr in lparam)
            {
                switch (pr.Type)
                {
                    case TDSProcParam.TypeDecimal:
                        dr[pr.getKey] = pr.getNumberValue;
                        break;
                    case TDSProcParam.TypeString:
                        dr[pr.getKey] = pr.getStringValue;
                        break;
                    case TDSProcParam.TypeCLOB:
                        dr[pr.getKey] = pr.GetB_CLob;
                        break;
                    case TDSProcParam.TypeBLOB:
                        dr[pr.getKey] = pr.GetB_CLob;
                        break;
                    case TDSProcParam.TypeDateTime:
                        dr[pr.getKey] = pr.getDateTimeValue;
                        break;
                }
            }
            //_ParamInfo.Add(new ParamInfo("V_P_AMOUNT", "Number", 0, "Input",typeof(System.Decimal )));
            //_ParamInfo.Add(new ParamInfo("V_P_PIC", "Blob", 0, "Input",typeof(System.Byte[] )));
            //_ParamInfo.Add(new ParamInfo("V_E", "Date", 0, "Input",typeof(System.DateTime )));
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện để khởi tạo TDSProc khi các tham số đều là varchar.       
        /// </summary>
        public TDSProc(string ProcName, params string[] lstParam)
        {
            if (lstParam.Length % 2 == 1)
            {
                throw new Exception("Số lượng tham số không hợp lý!");
            }
            _ProcName = ProcName;
            dtData = new DataTable(_ProcName);
            int i = 0;
            while (i < lstParam.Length)
            {
                _ParamInfo.Add(new ParamInfo(lstParam[i], "Varchar2", 0, "Input", typeof(System.String)));
                dtData.Columns.Add(lstParam[i], typeof(System.String));
                i += 2;
            }
            i = 0;
            DataRow dr = dtData.Rows.Add();
            while (i < lstParam.Length)
            {
                dr[lstParam[i]] = lstParam[i + 1];
                i += 2;
            }
        }
        public TDSProc(string ProcName, string V_P_Param)
        {
            _ProcName = ProcName;
            dtData = new DataTable(_ProcName);
            foreach (string s in V_P_Param.Split(';'))
            {
                _ParamInfo.Add(new ParamInfo(s, "Varchar2", 0, "Input", typeof(System.String)));
                dtData.Columns.Add(s, typeof(System.String));
            }
            dtData.Rows.Add();
        }
        public void addParamString(string key, string value)
        {
            _ParamInfo.Add(new ParamInfo(key, "Varchar2", 0, "Input", typeof(System.String)));
            dtData.Columns.Add(key, typeof(System.String));
            dtData.Rows[0][key] = value;
        }
        public void setWorkType(string wtype)
        {
            dtData.Rows[0]["V_P_WORK_TYPE"] = wtype;
        }
        public void setStringParamValue(string key, string value)
        {
            dtData.Rows[0][key] = value;
        }
        
        public void setStringParamValue(int index, string value)
        {
            dtData.Rows[0][index] = value;
        }
        public DataTable TableParam
        {
            get
            {
                return dtData;
            }
        }
        public void Dispose()
        {
            dtData.Dispose();
            dtData = null;
        }
        public ResultSet ExecProc(JBaseForm frm, string AliasDB)
        {
            ResultSet rs = frm.CommonCallQuery(AliasDB, dtData, _ProcName, GetParamInfo());
            dtData.Dispose();
            dtData = null;
            return rs;
        }
    }
    public class TDSProcParam
    {
        public const int TypeString = 1;
        public const int TypeDecimal = 2;
        //public const int TypeDouble = 2;
        public const int TypeCLOB = 3;
        public const int TypeBLOB = 4;
        public const int TypeDateTime = 5;
        private string Key;
        private string Svalue;
        private byte[] bvalue;
        private DateTime dt;
        private Decimal num;
        private int type;
        public TDSProcParam(string key, string value)
        {
            Key = key; Svalue = value;// type = t;
            type = 1;
        }
        public TDSProcParam(string key, byte[] value, bool isClob = true)
        {
            Key = key; bvalue = value; //type = t; 
            type = isClob ? 3 : 4;
        }
        public TDSProcParam(string key, DateTime value)
        {
            Key = key; dt = value;// type = t;
            type = 5;
        }
        public TDSProcParam(string key, Decimal value)
        {
            Key = key; num = value; //type = t; 
            type = 2;
        }
        public int Type
        {
            get
            {
                return type;
            }
        }
        public string getKey
        {
            get
            {
                return Key;
            }
        }
        public decimal getNumberValue
        {
            get
            {
                return num;
            }
        }
        public String getStringValue
        {
            get
            {
                return Svalue;
            }
        }
        public DateTime getDateTimeValue
        {
            get
            {
                return dt;
            }
        }
        public byte[] GetB_CLob
        {
            get
            {
                return bvalue;
            }
        }
    }
    class TDSTool
    {
        private const string StrReplace = "TK-TDSVNT-T";
        public static Color TOTAL = Color.FromArgb(250, 191, 143);
        public static Color SUM = Color.LightGreen;
        public static Color SUB_SUM = Color.FromArgb(184, 204, 228);
        public static Color IN_PROGRESS = Color.FromArgb(255, 255, 105);
        public static Color BACKLOG = Color.FromArgb(255, 255, 105);
        public static Color COMPLETED = Color.FromArgb(85, 85, 255);
        public static Color FORE_COLOR = Color.FromArgb(255, 255, 255);
        public static string ConvertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string Trim(object str)
        {
            if (str == null)
                return "";
            else
                return str.ToString().Trim();
        }
        public static DataTable SplitDataTable(DataTable dt, string con)
        {
            return dt.Select(con).CopyToDataTable();
        }
        public static bool CheckBINDefault(string strDefault, string str2)
        {
            bool ok = true;
            for (int i = 0; i < strDefault.Length; i++)
            {
                if (strDefault[i] == '1' && str2[i] == '0')
                    return false;
            }
            return ok;
        }
        public static string DEC2BIN(int n, int nlength)
        {
            string str = "";
            while (n > 0)
            {
                str = (n % 2).ToString() + str;
                n = n / 2;
            }
            return (new string('0', nlength - str.Length) + str);
        }
        public static int BIN2DEC(string str)
        {
            int n = 0;
            int i = str.Length - 1;
            foreach (char c in str)
            {
                n = n + Convert.ToInt16(c - 48) * Convert.ToInt16(Math.Pow(2, i));
                i--;
            }
            return n;
        }
        public static string[] ListBINbyn(int n, int start = 0)
        {
            String nBin = "";
            string strmax = new String('1', n);
            int nmax = BIN2DEC(strmax) + 1;
            for (int i = start; i < nmax; i++)
            {
                nBin += DEC2BIN(i, n) + ";";
            }
            return nBin.Substring(0, nBin.Length - 1).Split(';');
        }
        public static string[] ListBINbyn(int start, string strDefault)
        {
            String nBin = "";
            int n = (strDefault.Replace("1", "").Length);
            string strmax = new String('1', n);
            int nmax = BIN2DEC(strmax) + 1;
            string strkq = "";
            int t;
            for (int i = start; i < nmax; i++)
            {
                strmax = DEC2BIN(i, n);
                strkq = "";
                t = 0;
                foreach (char c in strDefault)
                {
                    if (c == '1')
                    {
                        strkq += '1';
                    }
                    else
                    {
                        strkq += strmax[t];
                        t++;
                    }
                }
                nBin += strkq + ";";
            }
            return nBin.Substring(0, nBin.Length - 1).Split(';');
        }
        public static bool IsNumber(string pText)
        {
            Regex regex = null;
            try
            {
                regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$"); return regex.IsMatch(pText);
            }
            catch 
            {
                return regex.IsMatch(pText);
            }
        }
        public static string TrimLeftbyChar(String src, String strim)
        {
            while ((StrReplace + src).IndexOf(StrReplace + strim) >= 0)
            {
                src = (StrReplace + src).Replace(StrReplace + strim, "");
            }
            return src;
        }
        public static string TrimRightbyChar(String src, String strim)
        {
            while ((src + StrReplace).IndexOf(strim + StrReplace) >= 0)
            {
                src = (src + StrReplace).Replace(strim + StrReplace, "");
            }
            return src;
        }
        public static string TrimbyChar(String src, String left_right_trim)
        {
            return TrimbyChar(src, left_right_trim, left_right_trim);
        }
        public static string TrimbyChar(String src, String leftrim, string righttrim)
        {
            return TrimLeftbyChar(TrimRightbyChar(src, righttrim), leftrim);
        }
        /// <summary>
        /// Tùy chọn cho phần loading screen.
        /// </summary>
        public enum OptionsButton
        {
            Query,
            Save,
            Delete,
            Print,
            Preview,
            New,
            Add
        }
        /// <summary>
        /// Tuyen.
        /// Create date: 2019-07-09.
        /// </summary>
        /// <param name="frm">Form</param>
        /// <param name="ob">Button type</param>
        public static void BeginQuery(JBaseForm frm, OptionsButton ob = OptionsButton.Query, bool DisableControl = true)
        {
            //if (optButton.ToUpper() == "QUERY")
            //    frm.QueryButton = false;
            //else if (optButton.ToUpper() == "SAVE")
            //    frm.SaveButton = false;
            //else if (optButton.ToUpper() == "DELETE")
            //    frm.DeleteButton = false;
            //else if (optButton.ToUpper() == "PRINT")
            //    frm.PrintButton = false;
            //else if (optButton.ToUpper() == "PREVIEW")
            //    frm.PreviewButton = false;
            //else if (optButton.ToUpper() == "NEW")
            //    frm.NewButton = false;
            //else if (optButton.ToUpper() == "ADD")
            //    frm.AddButton = false;
            switch (ob)
            {
                case OptionsButton.Query:
                    frm.QueryButton = false;
                    break;
                case OptionsButton.Save:
                    frm.SaveButton = false;
                    break;
                case OptionsButton.Delete:
                    frm.DeleteButton = false;
                    break;
                case OptionsButton.Print:
                    frm.PrintButton = false;
                    break;
                case OptionsButton.Preview:
                    frm.PreviewButton = false;
                    break;
                case OptionsButton.New:
                    frm.NewButton = false;
                    break;
                case OptionsButton.Add:
                    frm.AddButton = false;
                    break;
            }
            DevExpress.XtraWaitForm.ProgressPanel pnProgress = new DevExpress.XtraWaitForm.ProgressPanel();
            frm.Controls.Add(pnProgress);
            pnProgress.Appearance.BackColor = System.Drawing.Color.Transparent;
            pnProgress.Appearance.Options.UseBackColor = true;
            pnProgress.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            pnProgress.Location = new System.Drawing.Point(frm.Width / 2 - 120, frm.Height / 2 - 60);
            pnProgress.Name = "pnProgress";
            pnProgress.Size = new System.Drawing.Size(246, 66);
            pnProgress.TabIndex = 10;
            pnProgress.Text = "pnProgress";
            pnProgress.BringToFront();
            pnProgress.Visible = true;
            //frm.Enabled = false;
            if (DisableControl)
            {
                foreach (Control ctr in frm.Controls)
                    ctr.Enabled = false;
            }
        }
        //public static void BeginQuery(JBaseForm frm, string optButton = "QUERY")
        //{
        //    if (optButton.ToUpper() == "QUERY")
        //        frm.QueryButton = false;
        //    else if (optButton.ToUpper() == "SAVE")
        //        frm.SaveButton = false;
        //    else if (optButton.ToUpper() == "DELETE")
        //        frm.DeleteButton = false;
        //    else if (optButton.ToUpper() == "PRINT")
        //        frm.PrintButton = false;
        //    else if (optButton.ToUpper() == "PREVIEW")
        //        frm.PreviewButton = false;
        //    else if (optButton.ToUpper() == "NEW")
        //        frm.NewButton = false;
        //    else if (optButton.ToUpper() == "ADD")
        //        frm.AddButton = false;
        //    DevExpress.XtraWaitForm.ProgressPanel pnProgress = new DevExpress.XtraWaitForm.ProgressPanel();
        //    frm.Controls.Add(pnProgress);
        //    pnProgress.Appearance.BackColor = System.Drawing.Color.Transparent;
        //    pnProgress.Appearance.Options.UseBackColor = true;
        //    pnProgress.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
        //    pnProgress.Location = new System.Drawing.Point(frm.Width / 2 - 120, frm.Height / 2 - 60);
        //    pnProgress.Name = "pnProgress";
        //    pnProgress.Size = new System.Drawing.Size(246, 66);
        //    pnProgress.TabIndex = 10;
        //    pnProgress.Text = "pnProgress";
        //    pnProgress.BringToFront();
        //    pnProgress.Visible = true;
        //    //frm.Enabled = false;
        //    foreach (Control ctr in frm.Controls)
        //        ctr.Enabled = false;
        //}
        /// <summary>
        /// Tuyen.
        /// Create date: 2019-07-09.
        /// </summary>
        /// <param name="frm">Form</param>
        /// <param name="ob">Button type</param>
        public static void EndQuery(JBaseForm frm, OptionsButton ob = OptionsButton.Query)
        {
            //frm.Enabled = true;
            frm.Controls.RemoveByKey("pnProgress");
            //if (optButton.ToUpper() == "QUERY")
            //    frm.QueryButton = true;
            //else if (optButton.ToUpper() == "SAVE")
            //    frm.SaveButton = true;
            //else if (optButton.ToUpper() == "DELETE")
            //    frm.DeleteButton = true;
            //else if (optButton.ToUpper() == "PRINT")
            //    frm.PrintButton = true;
            //else if (optButton.ToUpper() == "PREVIEW")
            //    frm.PreviewButton = true;
            //else if (optButton.ToUpper() == "NEW")
            //    frm.NewButton = true;
            //else if (optButton.ToUpper() == "ADD")
            //    frm.AddButton = true;
            // frm.Enabled = true;
            switch (ob)
            {
                case OptionsButton.Query:
                    frm.QueryButton = true;
                    break;
                case OptionsButton.Save:
                    frm.SaveButton = true;
                    break;
                case OptionsButton.Delete:
                    frm.DeleteButton = true;
                    break;
                case OptionsButton.Print:
                    frm.PrintButton = true;
                    break;
                case OptionsButton.Preview:
                    frm.PreviewButton = true;
                    break;
                case OptionsButton.New:
                    frm.NewButton = true;
                    break;
                case OptionsButton.Add:
                    frm.AddButton = true;
                    break;
            }
            foreach (Control ctr in frm.Controls)
                ctr.Enabled = true;
        }
        //public static void EndQuery(JBaseForm frm, string optButton = "QUERY")
        //{
        //    //frm.Enabled = true;
        //    frm.Controls.RemoveByKey("pnProgress");
        //    if (optButton.ToUpper() == "QUERY")
        //        frm.QueryButton = true;
        //    else if (optButton.ToUpper() == "SAVE")
        //        frm.SaveButton = true;
        //    else if (optButton.ToUpper() == "DELETE")
        //        frm.DeleteButton = true;
        //    else if (optButton.ToUpper() == "PRINT")
        //        frm.PrintButton = true;
        //    else if (optButton.ToUpper() == "PREVIEW")
        //        frm.PreviewButton = true;
        //    else if (optButton.ToUpper() == "NEW")
        //        frm.NewButton = true;
        //    else if (optButton.ToUpper() == "ADD")
        //        frm.AddButton = true;
        //    // frm.Enabled = true;
        //    foreach (Control ctr in frm.Controls)
        //        ctr.Enabled = true;
        //}
        public static ResultSet ExecProcNoParam(JBaseForm frm, string AliasDB, string ProcName, string worktype)
        {
            return new TDSProc(ProcName, "V_P_WORK_TYPE", worktype).ExecProc(frm, AliasDB);
        }
        public static ResultSet ExecProc(JBaseForm frm, string AliasDB, string ProcName, params string[] lsparam)
        {
            return new TDSProc(ProcName, lsparam).ExecProc(frm, AliasDB);
        }
        public static ResultSet ExecProc(JBaseForm frm, string AliasDB, string ProcName, params TDSProcParam[] lsparam)
        {
            return new TDSProc(ProcName, lsparam).ExecProc(frm, AliasDB);
        }
        /// <summary>
        /// Gọi câu thủ tục trực tiếp: EXEC SP_TENTHUTUC 'Q','tham số 1','',....
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="AliasDB"></param>
        /// <param name="ProcName"></param>
        /// <param name="lsparam"></param>
        /// <returns></returns>
        public static ResultSet ExecutePROC(JBaseForm frm, string AliasDB, string ProcName, string lsparam)
        {
            return frm.CommonExecutePROC(AliasDB, ProcName + "(" + lsparam + ");");
        }
        /// <summary>
        /// Tạo ra nút trong 1 ô: cú pháp của hàm xử lý sự kiện  void bt_Click(object sender, EventArgs e)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="btType"></param>
        /// <param name="bt_ClickMethod">void bt_Click(object sender, EventArgs e)</param>
        /// <returns></returns>
        public static RepositoryItemButtonEditEx CreateButtonInGrid(string name, GridControlEx grd, DevExpress.XtraEditors.Controls.ButtonPredefines btType, Action<object, EventArgs> bt_ClickMethod = null)
        {
            RepositoryItemButtonEditEx bt = new RepositoryItemButtonEditEx();
            bt.BeginInit();
            bt.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(btType) });//DevExpress.XtraEditors.Controls.ButtonPredefines.Search
            bt.Name = name;
            bt.Click += new System.EventHandler(bt_ClickMethod);
            grd.RepositoryItems.Add(bt);
            bt.EndInit();
            return bt;
        }
        /// <summary>
        /// Tạo ra combo trong 1 ô: cú pháp của hàm xử lý sự kiện  void cbo_EditValueChanged(object sender, EventArgs e)
        /// </summary>
        public static RepositoryItemLookUpEditEx CreateComboInGrid(string name, GridControlEx grd, DataTable dt, string displaymember, string valuemember, Action<object, EventArgs> cbo_ChangeMethod = null)
        {
            RepositoryItemLookUpEditEx cbo = new RepositoryItemLookUpEditEx();
            cbo.BeginInit();
            cbo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });//DevExpress.XtraEditors.Controls.ButtonPredefines.Search
            cbo.Name = name;
            cbo.DisplayMember = displaymember;
            cbo.ValueMember = valuemember;
            cbo.DataSource = dt;
            cbo.NullText = "";
            if (cbo_ChangeMethod != null) cbo.EditValueChanged += new System.EventHandler(cbo_ChangeMethod);
            grd.RepositoryItems.Add(cbo);
            cbo.EndInit();
            return cbo;
        }
        public static DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit CreateMemoEditIngrid()
        {
            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            memoEdit.AutoHeight = true;
            memoEdit.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            memoEdit.WordWrap = true;
            return memoEdit;
        }
        public static string[] GetDataFromClipboard()
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData == null) return null;
            if (iData.GetDataPresent(DataFormats.UnicodeText))
                return ((string)iData.GetData(DataFormats.UnicodeText)).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); ;
            return null;
        }
        public static bool SendEmail(string fromemail, string fromname, string[] toemail, string[] ccemail, string title, string content)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient("tkgw.t2group.co.kr");
                SmtpServer.Port = 25;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromemail, fromname);
                foreach (string s in toemail)
                {
                    mail.To.Add(new MailAddress(s));
                }
                foreach (string s in ccemail)
                {
                    mail.CC.Add(new MailAddress(s));
                }
                mail.Subject = title;
                mail.IsBodyHtml = true;
                mail.Body = content;
                SmtpServer.Send(mail);
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public static void CheckDEV(TKBaseForm frm, string user)
        {
            if (frm.Modal && "18010101;20140061".Contains(user))
            {
                Panel p = new Panel();
                frm.Controls.Add(p);
                p.Visible = true;
                p.BackColor = Color.LightCyan;
                p.Dock = DockStyle.Top;
                p.Height = 30;
                Button btQuery = new Button();
                btQuery.Text = "Query";
                p.Controls.Add(btQuery);
                btQuery.Visible = true;
                btQuery.Width = 50;
                btQuery.Height = 20;
                btQuery.Location = new Point(25, 5);
                btQuery.Click += (o, evt) =>
                {
                    frm.QueryClick();
                };
            }
        }
        public static void CheckDEV(TKBaseForm frm, string strDBAlias, string user)
        {
            #region ## Check if user develop ##
            string strSql = $@"SELECT F_CHECK_USER_DEV ('{user}') KEY_VALUE FROM DUAL";
            ResultSet rs = frm.CommonDirectSQL(strDBAlias, strSql);
            var value = rs.ResultDataSet.Tables[0].Rows[0].ItemArray[0].ToString();
            if (frm.Modal && value == user)
            {
                Panel p = new Panel();
                frm.Controls.Add(p);
                p.Visible = true;
                p.BackColor = Color.LightCyan;
                p.Dock = DockStyle.Top;
                p.Height = 30;
                Button btQuery = new Button();
                btQuery.Text = "Query";
                p.Controls.Add(btQuery);
                btQuery.Visible = true;
                btQuery.Width = 50;
                btQuery.Height = 20;
                btQuery.Location = new Point(25, 5);
                btQuery.Click += (o, evt) =>
                {
                    frm.QueryClick();
                };
                Button btPreview = new Button();
                btPreview.Text = "Preview";
                p.Controls.Add(btPreview);
                btPreview.Visible = true;
                btPreview.Width = 50;
                btPreview.Height = 20;
                btPreview.Location = new Point(75, 5);
                btPreview.Click += (o, evt) =>
                {
                    frm.PreviewClick();
                };
                Button btPrint = new Button();
                btPrint.Text = "Print";
                p.Controls.Add(btPrint);
                btPrint.Visible = true;
                btPrint.Width = 50;
                btPrint.Height = 20;
                btPrint.Location = new Point(125, 5);
                btPrint.Click += (o, evt) =>
                {
                    frm.PrintClick();
                };
            }
            #endregion
        }
        public static DialogResult ShowInputDialog(ref string input, string title)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();
            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.ClientSize = size;
            inputBox.Text = title;
            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);
            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);
            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);
            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;
            inputBox.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
        //public static ResultSet CallStoreProcedureNoneParam(string strDBAlias, string ProcName, TKBaseForm frm)
        //{
        //    DataTable dt = new DataTable(ProcName);
        //    dt.Columns.Add("V_P_WORK_TYPE", typeof(System.String));
        //    dt.Rows.Add("Q");
        //    DataTable dtinfo = new DataTable(ProcName);
        //    dtinfo.Columns.Add("name", typeof(System.String));
        //    dtinfo.Columns.Add("type", typeof(System.String));
        //    dtinfo.Columns.Add("size", typeof(System.String));
        //    dtinfo.Columns.Add("direction", typeof(System.String));
        //    dtinfo.Rows.Add("V_P_WORK_TYPE", "Varchar2", 0, "Input");
        //    return frm.CommonCallQuery(strDBAlias, dt, ProcName, dtinfo);
        //}
        //public static ResultSet CallStoreProcedureWithParam(string strDBAlias, string ProcName, TKBaseForm frm, ref DataTable dt1, ref DataTable dt2, params TDSProcParam[] listparam)
        //{
        //    DataTable dt = new DataTable(ProcName);
        //    dt.Columns.Add("V_P_WORK_TYPE", typeof(System.String));
        //    DataTable dtinfo = new DataTable(ProcName);
        //    dtinfo.Columns.Add("name", typeof(System.String));
        //    dtinfo.Columns.Add("type", typeof(System.String));
        //    dtinfo.Columns.Add("size", typeof(System.String));
        //    dtinfo.Columns.Add("direction", typeof(System.String));
        //    dtinfo.Rows.Add("V_P_WORK_TYPE", "Varchar2", 0, "Input");
        //    foreach (TDSProcParam pr in listparam)
        //    {
        //        if (pr.Type==TDSProcParam.TypeDecimal)
        //        {
        //            dt.Columns.Add(pr.getKey, typeof(System.Double));
        //            dtinfo.Rows.Add(pr.getKey, "NUMBER", 0, "Input");
        //        }
        //        else if (pr.Type == TDSProcParam.TypeDecimal)
        //        {
        //            dt.Columns.Add(pr.getKey, typeof(System.Int32));
        //            dtinfo.Rows.Add(pr.getKey, "NUMBER", 0, "Input");
        //        }
        //        else if (pr.Type == TDSProcParam.TypeString)
        //        {
        //            dt.Columns.Add(pr.getKey, typeof(System.String));
        //            dtinfo.Rows.Add(pr.getKey, "Varchar2", 0, "Input");
        //        }
        //    }
        //    DataRow dr=dt.Rows.Add();
        //    dr["V_P_WORK_TYPE"] = "Q";
        //    foreach (TDSProcParam pr in listparam)
        //    {
        //        if (pr.Type == TDSProcParam.TypeDecimal)
        //        {
        //            dr[pr.getKey] = (pr.getNumberValue);
        //        }
        //        else if (pr.Type == TDSProcParam.TypeDecimal)
        //        {
        //            dr[pr.getKey] = (pr.getNumberValue);
        //        }
        //        else if (pr.Type == TDSProcParam.TypeString)
        //        {
        //            dr[pr.getKey] = (pr.getStringValue);
        //        }
        //    }
        //    ResultSet rs = frm.CommonCallQuery(strDBAlias, dt, ProcName, dtinfo);
        //    dt1 = dt;
        //    dt2 = dtinfo;
        //    return rs;
        //}
    }
    class GridViewTDS
    {
        public static void SetGridView(GridViewEx grv, bool isReadOnly, bool isEditable, bool isMerge, bool isFilter, bool isMultiselect)
        {
            grv.OptionsBehavior.ReadOnly = isReadOnly;
            grv.OptionsBehavior.Editable = isEditable;
            grv.OptionsView.AllowCellMerge = isMerge;
            //grv.OptionsView.ShowAutoFilterRow = isFilter;
            grv.OptionsCustomization.AllowFilter = isFilter;
            grv.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.SmartTag;
            grv.OptionsSelection.MultiSelect = isMultiselect;
        }
        public static void GridColumnFixed(string Colname, GridViewEx grv, DevExpress.XtraGrid.Columns.FixedStyle fx)
        {
            Colname = Colname + ";";
            foreach (GridColumnEx gb in grv.Columns)
            {
                if (Colname.Contains(gb.FieldName + ";"))
                    gb.Fixed = fx;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CellMerge của GridView: 
        /// <para>Trong 1 dòng nếu giá trị ở các cột trong Colname giống nhau thì sẽ merge lại</para>       
        /// </summary>
        public static void CreateColumn(GridViewEx grv, DataTable dt)
        {
            grv.Columns.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                GridColumnEx grc = new GridColumnEx();
                grc.FieldName = dr["FIELDNAME"].ToString();
                grc.Name = "cl" + grc.FieldName;
                grc.Caption = dr["UP1"].ToString();
                grc.WordText = grc.Caption;
                grc.ShowCodeColumn = true;
                if (!dr["WIDTH"].ToString().Equals("0"))
                    grc.Width = Convert.ToInt16(dr["WIDTH"]);
                else if (dr["FIELDNAME"].ToString().Contains("CBD"))
                {
                    grc.Caption = "";
                    grc.BindingField = null;
                    grc.Width = 10;
                }
                grc.Visible = dr["ISVISIBLE"].ToString().Equals("1");
                grv.Columns.Add(grc);
            }
        }
        /// <summary>
        /// fromat du lieu trong grid
        /// </summary>
        public static void SetFormatData(GridViewEx grv, string fieldname, DevExpress.Utils.FormatType type, string stringFormat)
        {
            foreach (string s in fieldname.Split(';'))
            {
                if (grv.Columns[s] == null) continue;
                grv.Columns[s].DisplayFormat.FormatType = type;// DevExpress.Utils.FormatType.Numeric
                grv.Columns[s].DisplayFormat.FormatString = stringFormat;// "({0:P0})";//"{0:c}"
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// Tô màu dòng khi được chọn</para>
        /// </summary>
        public static void CellStyle_SelectRow(string[] ColnameException, string[] Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, DevExpress.Utils.HorzAlignment hDefault, string colLeft = "", string colRight = "", string colCenter = "")
        {
            if (e.RowHandle < 0) return;
            for (int i = 0; i < ColnameException.Length; i++)
            {
                if (grv.GetRowCellValue(e.RowHandle, ColnameException[i]) != null && grv.GetRowCellValue(e.RowHandle, ColnameException[i]).ToString().Equals(Colvalue[i]))
                    return;
            }
            GridViewInfo viewInfo = (GridViewInfo)grv.GetViewInfo();
            GridCellInfo cell = viewInfo.GetGridCellInfo(e.RowHandle, e.Column);
            if (e.RowHandle == grv.FocusedRowHandle)
            {
                e.Appearance.Assign(grv.GetViewInfo().PaintAppearance.GetAppearance("FocusedRow"));
                if (colLeft.Contains(e.Column.FieldName))
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (colRight.Contains(e.Column.FieldName))
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                else if (colCenter.Contains(e.Column.FieldName))
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                else
                    e.Appearance.TextOptions.HAlignment = hDefault;
                e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                //e.Appearance.Font = grv.Columns[e.Column.FieldName].AppearanceCell.Font; // new Font(gvwMain.Appearance.Row.Font.FontFamily, 9, FontStyle.Regular);
            }
            else if (cell != null && cell.IsMerged)
            {
                foreach (GridCellInfo ci in cell.MergedCell.MergedCells)
                {
                    if (ci.RowHandle == grv.FocusedRowHandle)
                    {
                        e.Appearance.Assign(grv.GetViewInfo().PaintAppearance.GetAppearance("FocusedRow"));
                        if (colLeft.Contains(e.Column.FieldName))
                        {
                            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                        }
                        else if (colRight.Contains(e.Column.FieldName))
                            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        else if (colCenter.Contains(e.Column.FieldName))
                            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        else
                            e.Appearance.TextOptions.HAlignment = hDefault;
                        e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        break;
                    }
                }
            }
        }
        public static void MergeCell(string Colname, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if ((Colname + ";").Contains(e.Column.FieldName + ";"))
            {
                //e.Merge = gvwMain.GetRowCellValue(e.RowHandle1, "COMP").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "COMP").ToString()) && gvwMain.GetRowCellValue(e.RowHandle1, "DEPT").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "DEPT").ToString());
                e.Merge = true;
                foreach (string s in TDSTool.TrimRightbyChar(Colname, ";").Split(';'))
                {
                    e.Merge = e.Merge & (grv.GetRowCellValue(e.RowHandle2, Colname) != null && grv.GetRowCellValue(e.RowHandle1, Colname) != null && grv.GetRowCellValue(e.RowHandle1, s).ToString().Equals(grv.GetRowCellValue(e.RowHandle2, s).ToString()));
                }
            }
            e.Handled = true;
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CellMerge của GridView: 
        /// <para>Trong 1 dòng nếu giá trị ở các nhóm cột trong Colname[i] giống nhau thì sẽ merge lại</para>
        /// <para>Colname[0]="FCT;PLANT;VSM"    =>Giá trị trong các cột này phải giống nhau giữa dòng trên và dòng dưới thì sẽ merge lại</para>
        /// <para>Colname[1]="ABC;DEF;GHY"    =>Giá trị trong các cột này phải giống nhau giữa dòng trên và dòng dưới thì sẽ merge lại</para>
        /// </summary>
        public static void MergeCell(string[] Colname, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            //String col;
            foreach (string col in Colname)
            {
                //col = TDSTool.TrimRightbyChar(cl, ";");
                if ((col + ";").Contains(e.Column.FieldName + ";"))
                {
                    //e.Merge = gvwMain.GetRowCellValue(e.RowHandle1, "COMP").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "COMP").ToString()) && gvwMain.GetRowCellValue(e.RowHandle1, "DEPT").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "DEPT").ToString());
                    e.Merge = true;
                    foreach (string s in TDSTool.TrimRightbyChar(col, ";").Split(';'))
                    {
                        e.Merge = e.Merge & (grv.GetRowCellValue(e.RowHandle1, s) != null && grv.GetRowCellValue(e.RowHandle2, s) != null && grv.GetRowCellValue(e.RowHandle1, s).ToString().Equals(grv.GetRowCellValue(e.RowHandle2, s).ToString()));
                    }
                }
                e.Handled = true;
            }
        }
        public static void AddMemoEdit(GridViewEx gb, string Column, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit)
        {
            if (gb.GridControl.RepositoryItems.Contains(memoEdit)) return;
            gb.GridControl.RepositoryItems.Add(memoEdit);
            foreach (string s in Column.Split(';'))
            {
                gb.Columns[s].ColumnEdit = memoEdit;
            }
            gb.OptionsView.RowAutoHeight = true;
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của GridView: 
        /// <para>Tô màu cho ô nào có giá trị Colname=Colvalue</para>
        /// </summary>
        public static void CellStyle_SetColor(string Colname, string Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor)
        {
            if (e.RowHandle < 0 || (Colname).Equals(e.Column.FieldName) == false) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của GridView: 
        /// <para>Trong 1 dòng Giá trị của AND(Colname[i...]=Colvalue[i...]) thì sẽ tô màu nền và màu chữ tất cả các ô ngoại trừ những ô có FieldName nằm trong ColException</para>
        /// </summary>
        public static void CellStyle_SetColorException(string[] Colname, string[] Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColException)
        {
            if (e.RowHandle < 0 || (ColException + ";").Contains(e.Column.FieldName + ";")) return;
            bool ok = false;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = (grv.GetRowCellValue(e.RowHandle, Colname[i]) != null && grv.GetRowCellValue(e.RowHandle, Colname[i]).ToString().Equals(Colvalue[i]));
                if (!ok) return;
            }
            if (ok)
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của GridView: 
        /// <para>Trong 1 dòng Giá trị của Colname=Colvalue thì sẽ tô màu nền và màu chữ tất cả các ô ngoại trừ những ô có FieldName nằm trong ColException</para>
        /// </summary>
        public static void CellStyle_SetColorException(string Colname, string Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColException)
        {
            if (e.RowHandle < 0 || (ColException + ";").Contains(e.Column.FieldName + ";")) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của GridView: 
        /// <para>Trong 1 dòng Giá trị của AND(Colname[i...]=Colvalue[i...]) thì sẽ tô màu nền và màu chữ tất cả các ô nằm trong ColInvalid</para>
        /// </summary>
        public static void CellStyle_SetColorInvalid(string[] Colname, string[] Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColInvalid)
        {
            if (e.RowHandle < 0 || (ColInvalid + ";").Contains(e.Column.FieldName + ";") == false) return;
            bool ok = false;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = (grv.GetRowCellValue(e.RowHandle, Colname[i]) != null && grv.GetRowCellValue(e.RowHandle, Colname[i]).ToString().Equals(Colvalue[i]));
                if (!ok) return;
            }
            if (ok)
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của GridView: 
        /// <para>Trong 1 dòng Giá trị của Colname=Colvalue thì sẽ tô màu nền và màu chữ tất cả các ô ằm trong ColInvalid</para>
        /// </summary>
        public static void CellStyle_SetColorInvalid(string Colname, string Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColInvalid)
        {
            if (e.RowHandle < 0 || (ColInvalid + ";").Contains(e.Column.FieldName + ";") == false) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowStyle của GridView: 
        /// <para>Trong 1 dòng Giá trị của Colname=Colvalue thì sẽ tô màu nền và màu chữ của dòng đó</para>
        /// </summary>
        public static void RowStyle_SetColor(string Colname, string Colvalue, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e, Color BackColor, Color ForeColor)
        {
            if (e.RowHandle < 0) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập màu nền hoặc màu chữ của 1 cột. Thông thường phải set ở hàm LOAD
        /// </summary>
        public static void SetColumnColor(string Colname, GridViewEx grv, Color BackColor, Color ForeColor)
        {
            Colname = Colname + ";";
            foreach (GridColumnEx cl in grv.Columns)
            {
                if (Colname.Contains(cl.FieldName + ";"))
                {
                    if (BackColor != Color.Empty) cl.AppearanceCell.BackColor = BackColor;
                    if (ForeColor != Color.Empty) cl.AppearanceCell.ForeColor = ForeColor;
                }
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của GridViewEx
        /// <para>Các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của GridViewEx
        /// <para>Giá trị tại cột Colname=value thì các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, string Colname, string value, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            if (grv.GetValue(e.RowHandle, Colname).ToString().Equals(value))
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của GridViewEx
        /// <para>Giá trị tại cột AND(Colname[i...]=value[i...]) thì các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, string[] Colname, string[] value, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
                ok = ok && (grv.GetValue(e.RowHandle, Colname[i]).ToString().Equals(value[i]));
            if (ok)
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của GridViewEx
        /// <para>Giá trị tại cột AND(Colname[i...]=value[i...]) && AND(ColnameExecption[i...] <> valueException[i...])  thì các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, string[] Colname, string[] value, string[] ColnameExecption, string[] valueException, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
                ok = ok && (grv.GetValue(e.RowHandle, Colname[i]).ToString().Equals(value[i]));
            for (int i = 0; i < ColnameExecption.Length; i++)
                ok = ok && (grv.GetValue(e.RowHandle, ColnameExecption[i]).ToString().Equals(valueException[i]) == false);
            if (ok)
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của GridViewEx
        /// <para>Giá trị tại cột Colname <> value thì các cột trong ColHasControl sẽ được hiển thị control</para>
        /// </summary>
        public static void SetControlInGridExceptionValue(string ColHasControl, string Colname, string value, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, GridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            if (grv.GetValue(e.RowHandle, Colname).ToString().Equals(value) == false)
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để Ẩn hiện các dòng trong lưới. Sử dụng trong sự kiện CustomRowFilter
        /// <para>value="TOTAL;SUM;SUB...."</para>
        /// <para>Colname="PLANT"</para>
        /// </summary>
        public static void ShowHideRowInGrid(string Colname, string value, GridViewEx grv, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e, bool isVisible = true, bool Only = false)
        {
            DataView dv = grv.DataSource as DataView;
            if ((value + ";").Contains(dv[e.ListSourceRow][Colname].ToString() + ";"))
            {
                e.Visible = isVisible;
                e.Handled = true;
            }
            else if (Only)
            {
                e.Visible = !isVisible;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để Ẩn hiện các dòng trong lưới. Sử dụng trong sự kiện CustomRowFilter
        /// <para>value[i]={"TOTAL;SUM;SUB....","abc;def",...}</para>
        /// <para>Colname[i..]={"PLANT","VSM",...}</para>
        /// </summary>
        public static void ShowHideRowInGrid(string[] Colname, string[] value, GridViewEx grv, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e, bool isVisible = true, bool Only = false)
        {
            DataView dv = grv.DataSource as DataView;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = ok & (value[i] + ";").Contains(dv[e.ListSourceRow][Colname[i]].ToString() + ";");
            }
            if (ok)
            {
                e.Visible = isVisible;
                e.Handled = true;
            }
            else if (Only)
            {
                e.Visible = !isVisible;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để Ẩn hiện các dòng trong lưới. Sử dụng trong sự kiện CustomRowFilter
        /// <para>value[i]={"TOTAL;SUM;SUB....","abc;def",...}</para>
        /// <para>Colname[i..]={"PLANT","VSM",...}</para>
        /// </summary>
        public static void ShowHideRowInGrid(string[] Colname, string[] value, string[] ColnameException, string[] valueException, GridViewEx grv, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e, bool isVisible = true, bool Only = false)
        {
            DataView dv = grv.DataSource as DataView;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = ok & (value[i] + ";").Contains(dv[e.ListSourceRow][Colname[i]].ToString() + ";");
            }
            for (int i = 0; i < ColnameException.Length; i++)
            {
                ok = ok & !(valueException[i] + ";").Contains(dv[e.ListSourceRow][ColnameException[i]].ToString() + ";");
            }
            if (ok)
            {
                e.Visible = isVisible;
                e.Handled = true;
            }
            else if (Only)
            {
                e.Visible = !isVisible;
            }
        }
        public static void RecallEvent_CustomRowCellEdit(GridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs> event_func)
        {
            grv.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(event_func);
        }
        public static void RecallEvent_RowCellStyle(GridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs> event_func)
        {
            grv.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(event_func);
        }
        public static void RecallEvent_RowStyle(GridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs> event_func)
        {
            grv.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(event_func);
        }
        public static void RecallEvent_MergeCell(GridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs> event_func)
        {
            grv.CellMerge += new DevExpress.XtraGrid.Views.Grid.CellMergeEventHandler(event_func);
        }
        public static void RecallEvent_CustomRowFilter(GridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs> event_func)
        {
            grv.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(event_func);
        }
    }
    class BandedGrid
    {
        private static DataTable dtBandedColumn = null;
        public static void ResetBandedGridHeader(BandedGridViewEx grv)
        {
            /* int i = 0;
            DataTable dt = dtBandedColumn.Copy();
            foreach (DataRow dr in dtBandedColumn.Rows)
            {
                for (i = 1; i <= Convert.ToInt16(dr["NUP"]); i++)
                {
                    if (dr["UP" + i].ToString().Contains("[mrbo]"))
                    {
                        dr["UP" + i] = "";
                    }
                }
            }*/
            CreateBandGrid(grv, dtBandedColumn);
            foreach (GridBandEx gb in grv.Bands)
            {
                CleartmpTextInBandgrid(gb);
            }
        }
        public static void ShowHideSizeBandedGrid(BandedGridViewEx grv, string levelHide, string colEX = "")
        {
            //CreateBandGrid(grv, dtBandedColumn);
            if (levelHide.Length == 0) return;
            DataTable dt = dtBandedColumn.Copy();
            levelHide = TDSTool.TrimRightbyChar(levelHide, ";");
            string sizeCol = colEX + ";SIZE_01;SIZE_02;SIZE_03;SIZE_04;SIZE_05;SIZE_06;SIZE_07;SIZE_08;SIZE_09;SIZE_10;SIZE_11;SIZE_12;SIZE_13;SIZE_14;SIZE_15;SIZE_16;SIZE_17;SIZE_18;SIZE_19;SIZE_20;SIZE_21;SIZE_22;SIZE_23;SIZE_24;SIZE_25;SIZE_26;SIZE_27;SIZE_28;SIZE_29;SIZE_30;SIZE_31;SIZE_32;SIZE_33;SIZE_34;SIZE_35;SIZE_36;SIZE_37;SIZE_38;SIZE_39;";
            String colUp1 = "1;2;3;4;5;";
            int nup = 0;
            int i = 0;
            foreach (string s in levelHide.Split(';'))
            {
                dt.Columns["UP" + s].ColumnName = "UP" + s + "[REMOVET]";
                colUp1 = colUp1.Replace(s + ";", "");
                nup++;
            }
            if (colUp1.Length > 0)
            {
                int coldR = Convert.ToInt16(colUp1.Split(';')[0]);
                if (coldR != 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                        {
                            dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                        }
                        else
                        {
                            dr["UP" + coldR] = dr[1];
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                        {
                            dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                        }
                    }
                }
                foreach (string s in levelHide.Split(';'))
                {
                    dt.Columns.Remove("UP" + s + "[REMOVET]");
                }
                i = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.StartsWith("UP"))
                    {
                        dc.ColumnName = "UP" + (i++);
                    }
                }
            }
            /*
            foreach (string s in levelHide.Split(';'))
            {
                dt.Columns.Remove("UP" + s);
                nup++;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                {
                    dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                }
            }
            i = 1;
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName.StartsWith("UP"))
                {
                    dc.ColumnName = "UP" + (i++);
                }
            }
            */
            CreateBandGrid(grv, dt);
            foreach (GridBandEx gb in grv.Bands)
            {
                CleartmpTextInBandgrid(gb);
            }
        }
        static void CleartmpTextInBandgrid(GridBandEx gb)
        {
            if (gb.Caption.Contains("[mrbo]") || gb.WordText.Contains("[mrbo]"))
            {
                gb.Caption = "";
                gb.WordText = "";
            }
            foreach (GridBandEx gbx in gb.Children)
            {
                CleartmpTextInBandgrid(gbx);
            }
        }
        public static void SetGrid(BandedGridViewEx grv, int nheight = 1, bool allowMerge = true, bool allowShowControlInGrid = true, bool readOnly = true)
        {
            grv.OptionsView.AllowCellMerge = true;
            grv.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            grv.OptionsBehavior.ReadOnly = readOnly;
            grv.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            grv.OptionsBehavior.Editable = !readOnly;
            grv.MinBandPanelRowCount = nheight;
        }
        public static void AddMemoEdit(BandedGridViewEx gb, string Column, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit)
        {
            if (gb.GridControl.RepositoryItems.Contains(memoEdit)) return;
            gb.GridControl.RepositoryItems.Add(memoEdit);
            foreach (string s in Column.Split(';'))
            {
                gb.Columns[s].ColumnEdit = memoEdit;
            }
            gb.OptionsView.RowAutoHeight = true;
        }
        public static void SetBorderGrid(BandedGridViewEx grv, Color borderColor)
        {
            if (borderColor != Color.Empty)
            {
                grv.Appearance.HorzLine.BackColor = borderColor;
                grv.Appearance.VertLine.BackColor = borderColor;
                grv.Appearance.HorzLine.BorderColor = borderColor;
                grv.Appearance.VertLine.BorderColor = borderColor;
            }
        }
        static GridBandEx CreateNewBandGrid(ref GridBandEx parent, string newgridbandname, string fieldName)
        {
            string name = TDSTool.ConvertToUnSign3(newgridbandname).Replace(" ", "_").Replace("-", "1") + parent.Index;
            foreach (GridBandEx gbc in parent.Children)
            {
                if (gbc.Name.Equals(name))
                    return gbc;
            }
            GridBandEx gb = new GridBandEx();
            gb.Caption = newgridbandname;
            gb.WordText = gb.Caption;
            gb.AppearanceHeader.Options.UseTextOptions = true;
            gb.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gb.Tag = fieldName;
            if (name.Length > 0) gb.Name = name;
            parent.Children.Add(gb);
            return gb;
        }
        static GridBandEx CreateNewBandGrid(GridBandExCollection parrent, string newgridbandname, string fieldName)
        {
            string name = TDSTool.ConvertToUnSign3(newgridbandname).Replace(" ", "_").Replace("-", "1");
            if (fieldName.Contains("CBD") && newgridbandname.Contains("CBD"))
            {
                GridBandEx gb = new GridBandEx();
                gb.Caption = "";
                gb.WordText = "";
                gb.MinWidth = 10;
                gb.Width = 10;
                //if (fieldName.Length > 0) gb.Name = fieldName;
                gb.Tag = fieldName;
                parrent.Add(gb);
                return gb;
            }
            else
            {
                foreach (GridBandEx gbc in parrent)
                {
                    if (gbc.Name.Equals(name))
                        return gbc;
                }
                GridBandEx gb = new GridBandEx();
                gb.Caption = newgridbandname;
                gb.WordText = gb.Caption;
                gb.AppearanceHeader.Options.UseTextOptions = true;
                gb.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                if (name.Length > 0) gb.Name = name;
                gb.Tag = fieldName;
                parrent.Add(gb);
                return gb;
            }
        }
        public static void BandedGridAddMemoEdit(BandedGridViewEx gb, string Column, DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit)
        {
            gb.GridControl.RepositoryItems.Add(memoEdit);
            foreach (string s in Column.Split(';'))
            {
                gb.Columns[s].ColumnEdit = memoEdit;
            }
            gb.OptionsView.RowAutoHeight = true;
        }
        static void CreateChildGridBand(ref GridBandEx parent, DataRow dr, int i, BandedGridColumnEx cl)
        {
            if (i > Convert.ToInt16(dr["nUp"]))
            {
                parent.Columns.Add(cl);
                return;
            }
            GridBandEx gbc = CreateNewBandGrid(ref parent, dr["Up" + i].ToString(), cl.Name);
            try
            {
                i++;
                //if (i<= LevelBand && dr["Up" + i].ToString().Length > 0)
                if (i <= Convert.ToInt16(dr["nUp"]))
                {
                    CreateChildGridBand(ref gbc, dr, i, cl);
                }
                else
                {
                    gbc.Columns.Add(cl);
                }
            }
            catch
            {
                //MessageBox.Show(dr.GetColumnError("Up" + i));
            }
        }
        /// <summary>
        /// fromat du lieu trong grid
        /// </summary>
        public static void SetFormatData(BandedGridViewEx grv, string fieldname, DevExpress.Utils.FormatType type, string stringFormat)
        {
            foreach (string s in fieldname.Split(';'))
            {
                if (grv.Columns[s] == null) continue;
                grv.Columns[s].DisplayFormat.FormatType = type;// DevExpress.Utils.FormatType.Numeric
                grv.Columns[s].DisplayFormat.FormatString = stringFormat;// "({0:P0})";//"{0:c}"
            }
        }
        /// <summary>
        /// Format dữ liệu trong grid theo Column index.
        /// Tuyen - 19199240.
        /// Date: 2019-05-30.
        /// </summary>
        /// <param name="grv">View</param>
        /// <param name="StartColumnIndex">Start column index.</param>
        /// <param name="ColumnCount">Total column format.</param>
        /// <param name="type">Format type.</param>
        /// <param name="stringFormat">String format type.</param>
        public static void SetFormatData(BandedGridViewEx grv, int StartColumnIndex, int ColumnCount, DevExpress.Utils.FormatType type, string stringFormat)
        {
            for (int i = StartColumnIndex; i < ColumnCount; i++)
            {
                if (grv.Columns[i] == null)
                    continue;
                grv.Columns[i].DisplayFormat.FormatType = type;
                grv.Columns[i].DisplayFormat.FormatString = stringFormat;
            }
        }
        public static void RowforLine(string Col, string ColValue, string ColBorder, string ColException, BandedGridViewEx grv, PaintEventArgs e, int nHeightLine)
        {
            if (grv.RowCount == 0) return;
            BandedGridViewEx view = grv.GridControl.FocusedView as BandedGridViewEx;
            GridViewInfo info = view.GetViewInfo() as GridViewInfo;
            //int nHeight = (nRowHeader + (grv.RowCount == 0 ? 0 : + grv.RowCount ));
            ColException = ColException + ";";
            ColBorder = ColBorder + ";";
            //Rectangle r = new Rectangle(info.GetGridCellInfo(0, grv.Columns[1]).Bounds.X, info.GetGridCellInfo(0, grv.Columns[1]).Bounds.Bottom - 2, info.GetGridCellInfo(0, grv.Columns[grv.Columns.Count - 1]).Bounds.Right, 3);
            //int i;
            //ArrayList ar = new ArrayList();
            Rectangle r;
            for (int i = 0; i < grv.RowCount; i++)
            {
                if (grv.GetValue(i, Col).ToString().Equals(ColValue))
                {
                    foreach (BandedGridColumnEx cl in grv.Columns)
                    {
                        if (!ColException.Contains(cl.FieldName) && (ColBorder == ";" || ColBorder.Contains(cl.FieldName)) && info.GetGridCellInfo(i, cl) != null)
                        {
                            if (info.GetGridCellInfo(i, cl).IsMerged)
                                r = info.GetGridCellInfo(i, cl).Bounds; //r = info.GetGridCellInfo(i, cl).MergedCell.Bounds;
                            else
                            {
                                r = info.GetGridCellInfo(i, cl).Bounds;
                            }
                            e.Graphics.FillRectangle(Brushes.Black, r.Left - 2, r.Bottom - 2, r.Width + 2, nHeightLine);
                        }
                    }
                }
            }
            /*
            foreach (BandedGridColumnEx cl in grv.Columns)
            {
                if (!ColException.Contains(cl.FieldName + ";"))
                {
                    try
                    {
                        int lastrow = 0;
                        for (i = grv.RowCount - 1; i > 0; i--)
                        {
                            if (view.IsRowVisible(i) == DevExpress.XtraGrid.Views.Grid.RowVisibleState.Visible)
                            {
                                lastrow = i;
                                break;
                            }
                        }
                        if (info.GetGridCellInfo(lastrow, cl) != null)
                        {
                            colRect = info.GetGridCellInfo(lastrow, cl).Bounds;
                            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Right - 1, nRowHeader * -1, nWidthLine, colRect.Bottom + nWidthLine));
                        }
                        else
                        {
                            colRect = info.ColumnsInfo[cl].Bounds;
                            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Right - 1, nRowHeader * -1, nWidthLine, colRect.Bottom - colRect.Bottom / (nRowHeader + 1)));
                        }
                        //e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Left, colRect.Bottom - 1, colRect.Width, 1));
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }*/
        }
        /// <summary>
        /// Hàm này được sử dụng để Ke duong vien duoi cua o. Sử dụng trong sự kiện CustomDrawCell của BanedeGridViewEx
        /// <para>Các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void RowForLine(string Col, string ColValue, string ColBorder, string ColException, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e, Brush BDColor)
        {
            if ((ColBorder != "" && !(ColBorder + ";").Contains(e.Column.FieldName)) || (ColException + ";").Contains(e.Column.FieldName)) return;
            Rectangle r;
            if (grv.GetValue(e.RowHandle, Col).ToString().Equals(ColValue))
            {
                r = e.Bounds;
                e.Graphics.FillRectangle(BDColor, r.X - 2, r.Bottom - 2, r.Width + 5, 3);
            }
            else
            {
                GridViewInfo viewInfo = (GridViewInfo)grv.GetViewInfo();
                GridCellInfo cell = viewInfo.GetGridCellInfo(e.RowHandle, e.Column);
                if ((cell != null && cell.IsMerged))
                {
                    r = cell.MergedCell.Bounds;
                    //e.Graphics.FillRectangle(BDColor, r.X - 2, r.Bottom - 2, r.Width + 5, 3);
                    //e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Right - 1, nRowHeader * -1, nWidthLine, colRect.Bottom + nWidthLine));
                    e.Graphics.FillRectangle(BDColor, new Rectangle(r.X - 2, r.Bottom - 2, r.Width + 5, 10));
                }
            }
        }
        public static void SetFormatAlign(BandedGridViewEx grv, string fieldname, DevExpress.Utils.HorzAlignment hAlign)
        {
            foreach (string s in fieldname.Split(';'))
            {
                if (grv.Columns[s] == null) continue;
                grv.Columns[s].AppearanceCell.TextOptions.HAlignment = hAlign;// "({0:P0})";//"{0:c}"
            }
        }
        public static void RecallEvent_CustomDrawBandHeader(BandedGridViewEx grv, Action<object, DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventArgs> event_func)
        {
            grv.CustomDrawBandHeader += new DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventHandler(event_func);
        }
        public static void RecallEvent_MergeCell(BandedGridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs> event_func)
        {
            grv.CellMerge += new DevExpress.XtraGrid.Views.Grid.CellMergeEventHandler(event_func);
        }
        /// <summary>
        /// Hàm này được sử dụng để tạo ra các Band trong BandedGrid: dựa vào câu thủ tục
        /// </summary>
        public static void CreateBandGrid(BandedGridViewEx bgv, string strDBAlias, String ProcName, string param, TKBaseForm frm, bool showColumnHeader = false)
        {
            ResultSet rs = TDSTool.ExecutePROC(frm, strDBAlias, ProcName, param);//TDSTool.CallStoreProcedureNoneParam(strDBAlias, ProcName, frm);
            CreateBandGrid(bgv, rs.ResultDataSet.Tables[0], showColumnHeader);
        }
        /// <summary>
        /// Hàm này được sử dụng để tạo ra các Band trong BandedGrid: dựa vào câu thủ tục
        /// </summary>
        public static void CreateBandGrid(BandedGridViewEx bgv, string strDBAlias, String ProcName, TKBaseForm frm, bool showColumnHeader = false)
        {
            CreateBandGrid(bgv, strDBAlias, ProcName, "Q", frm, showColumnHeader);
            //ResultSet rs = TDSTool.ExecutePROC(frm, strDBAlias, ProcName, "'Q'");//TDSTool.CallStoreProcedureNoneParam(strDBAlias, ProcName, frm);
            //CreateBandGrid(bgv, rs.ResultDataSet.Tables[0], showColumnHeader);
        }
        /// <summary>
        /// Hàm này được sử dụng để set Fieldname vào Tag trong Band
        /// </summary>
        public static void UpdateFieldNameInBand(BandedGridViewEx bgv)
        {
            System.Collections.ArrayList ar = new System.Collections.ArrayList();
            string fieldname = "";
            // GridBandEx gbex;
            foreach (GridBandEx gb in bgv.Bands)
            {
                fieldname = "";
                Dequi(gb, ref fieldname);
            }
        }
        static void Dequi(GridBandEx gb, ref string fieldname)
        {
            foreach (GridBandEx gbx in gb.Children)
            {
                if (gbx.HasChildren)
                {
                    Dequi(gbx, ref fieldname);
                }
                else
                {
                    if (gbx.Columns.Count > 0)
                    {
                        fieldname = gbx.Columns[0].FieldName;
                        gbx.Tag = fieldname;
                        gbx.Caption = fieldname;
                    }
                }
            }
            if (fieldname.Length > 0)
            {
                gb.Tag = fieldname;
                gb.Caption = fieldname;
            }
            else
            {
                if (gb.Columns.Count > 0)
                {
                    fieldname = gb.Columns[0].FieldName;
                    gb.Tag = fieldname;
                    gb.Caption = fieldname;
                }
            }
        }
        public static void HideBand(GridBandEx gb)
        {
            bool hide = false;
            if (gb.HasChildren)
            {
                foreach (GridBandEx gx in gb.Children)
                {
                    HideBand(gx);
                    hide = hide || gx.Visible;
                }
            }
            else
            {
                foreach (BandedGridColumnEx bc in gb.Columns)
                    hide = hide || bc.Visible;
            }
            gb.Visible = hide;
        }
        public static void HideBandbyFieldName(BandedGridViewEx bgv, bool isVisible, string FieldName)
        {
            foreach(string s in FieldName.Split(';'))
            {
                if (s.Trim() == "")
                    continue;
                HideBandbyFieldName(bgv, s, isVisible);
            }
        }
        public static void HideBandbyFieldName(BandedGridViewEx bgv, string FieldName, bool isVisible = true)
        {
            if (bgv.Columns[FieldName] == null)
                return;
            bgv.Columns[FieldName].Visible = isVisible;
            GridBand gb = (bgv.Columns[FieldName]).OwnerBand;
            while (gb.ParentBand != null)
            {
                gb = gb.ParentBand;
            }
            if (gb == null) return;
            bool hide = false;
            if (gb.HasChildren)
            {
                foreach (GridBandEx gx in gb.Children)
                {
                    HideBand(gx);
                    hide = hide || gx.Visible;
                }
            }
            else
            {
                foreach (BandedGridColumnEx bc in gb.Columns)
                    hide = hide || bc.Visible;
            }
            gb.Visible = hide;
        }
        public static void CreateBandGrid(BandedGridViewEx bgv, DataTable dtGridBandx, bool showColumnHeader = false,string filter="")
        {
            DataTable dtGridBand;
            if (filter == "")
                dtGridBand = dtGridBandx;
            else
            {
                DataView dv = new DataView(dtGridBandx);
                dv.RowFilter = filter;
                dv.Sort = "STT";
                dtGridBand = dv.ToTable();
            }
            bgv.OptionsView.ShowColumnHeaders = showColumnHeader;
            bgv.Bands.Clear();
            bgv.Columns.Clear();
            foreach (DataRow dr in dtGridBand.Rows)
            {
                if (dr["FIELDNAME"].ToString().Length > 0)
                {
                    BandedGridColumnEx cl = new BandedGridColumnEx();
                    cl.Name = dr["FIELDNAME"].ToString();
                    cl.BindingField = cl.Name;
                    cl.ColumnEditName = cl.Name;
                    if (!dr["WIDTH"].ToString().Equals("0"))
                        cl.Width = Convert.ToInt16(dr["WIDTH"]);
                    else if (dr["FIELDNAME"].ToString().Contains("CBD"))
                    {
                        cl.Caption = "";
                        cl.BindingField = null;
                        cl.ColumnEditName = null;
                        cl.Width = 10;
                    }
                    if (dtGridBand.Columns.Contains("ALIGN_DATA"))
                    {
                        cl.AppearanceCell.Options.UseTextOptions = true;
                        if (dr["ALIGN_DATA"].ToString().ToUpper().Equals("CENTER"))
                            cl.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        else if (dr["ALIGN_DATA"].ToString().ToUpper().Equals("RIGHT"))
                            cl.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        else if (dr["ALIGN_DATA"].ToString().ToUpper().Equals("LEFT"))
                            cl.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    }
                    if (dtGridBand.Columns.Contains("FORMAT_DATA") && dr["FORMAT_DATA"].ToString().Length > 0)
                    {
                        cl.DisplayFormat.FormatString = dr["FORMAT_DATA"].ToString();
                        cl.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                    }
                    if (dtGridBand.Columns.Contains("GROUP_FOOTER") && dr["GROUP_FOOTER"].ToString().Length > 0)
                    {
                        bgv.OptionsView.ShowFooter = dr["GROUP_FOOTER"].ToString().Length > 0;
                        //AutoFilterCondition atfCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Default;
                        //if (bgv.OptionsView.ShowAutoFilterRow)
                        //{
                        //    atfCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                        //}
                        //cl.OptionsFilter.AutoFilterCondition = atfCondition;
                        cl.SummaryItem.DisplayFormat = "{0:#,0}";
                        cl.SummaryItem.FieldName = cl.FieldName;
                        switch (TDSTool.Trim(dr["GROUP_FOOTER"]))
                        {
                            case "SUM":
                                cl.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum; break;
                            case "COUNT":
                                cl.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count; break;
                            case "MIN":
                                cl.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Min; break;
                            case "MAX":
                                cl.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Max; break;
                            case "AVG":
                                cl.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average; break;
                        }
                    }
                    bgv.Columns.Add(cl);
                    cl.Visible = dr["ISVISIBLE"].ToString().Equals("1");
                    GridBandEx gb = CreateNewBandGrid(bgv.Bands, dr["UP1"].ToString(), dr["FIELDNAME"].ToString());
                    // BandedGridViewEx gb1 = CreateNewBandGrid(bgv.Bands, dr["Up1"].ToString(), dr["FieldName"].ToString());
                    //gb.Columns.Add(cl);
                    CreateChildGridBand(ref gb, dr, 2, cl);
                    gb.Visible = cl.Visible;
                }
            }
            foreach (GridBandEx bx in bgv.Bands)
            {
                HideBand(bx);
            }
            if (dtBandedColumn == null)
            {
                int i = 1;
                dtBandedColumn = dtGridBand.Copy();
                foreach (DataColumn dc in dtBandedColumn.Columns)
                {
                    if (dc.DataType.Name.Equals("String"))
                        dc.MaxLength = 1000;
                }
                foreach (DataRow dr in dtBandedColumn.Rows)
                {
                    if (dr["UP2"].ToString().Length > 0 && Convert.ToInt16(dr["NUP"]) > 1)
                    {
                        for (i = 2; i <= Convert.ToInt16(dr["NUP"]); i++)
                        {
                            if (dr["UP" + i].ToString().Trim().Length == 0)
                            {
                                dr["UP" + i] = dr["UP1"] + "[mrbo]" + i;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để Fixed Band trong BandedGrid: 
        /// </summary>
        public static void BandedFixed(string Colname, BandedGridViewEx grv, DevExpress.XtraGrid.Columns.FixedStyle fx)
        {
            Colname = Colname + ";";
            foreach (GridBandEx gb in grv.Bands)
            {
                if (Colname.Contains(gb.Tag + ";"))
                    gb.Fixed = fx;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để ẩn hiện Band trong BandedGrid: 
        /// </summary>
        public static void BandedVisible(string Colname, BandedGridViewEx grv, bool isVisible = true)
        {
            Colname = Colname + ";";
            foreach (GridBandEx gb in grv.Bands)
            {
                if (Colname.Contains(gb.Tag + ";"))
                    gb.Visible = isVisible;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để ẩn hiện Band trong BandedGrid: 
        /// </summary>
        public static void BandedVisibleByFieldName(string FieldName, BandedGridViewEx grv, bool isVisible = true)
        {
            FieldName = FieldName + ";";
            GridBandEx gb;
            foreach (BandedGridColumnEx cl in grv.Columns)
            {
                if (cl.OwnerBand != null)
                {
                    if (FieldName.Contains(cl.FieldName + ";"))
                    {
                        gb = (GridBandEx)cl.OwnerBand;
                        while (gb.ParentBand != null)
                        {
                            gb = (GridBandEx)gb.ParentBand;
                        }
                        gb.Visible = isVisible;
                    }
                }
            }
        }
        public static void BandedVisibleByFieldName(string FieldNameHide, string FieldNameShow, BandedGridViewEx grv)
        {
            FieldNameHide = FieldNameHide + ";";
            FieldNameShow = FieldNameShow + ";";
            GridBandEx gb;
            foreach (BandedGridColumnEx cl in grv.Columns)
            {
                if (cl.OwnerBand != null)
                {
                    if (FieldNameHide.Contains(cl.FieldName + ";"))
                    {
                        gb = (GridBandEx)cl.OwnerBand;
                        while (gb.ParentBand != null)
                        {
                            gb = (GridBandEx)gb.ParentBand;
                        }
                        gb.Visible = false;
                    }
                    else if (FieldNameShow.Contains(cl.FieldName + ";"))
                    {
                        gb = (GridBandEx)cl.OwnerBand;
                        while (gb.ParentBand != null)
                        {
                            gb = (GridBandEx)gb.ParentBand;
                        }
                        gb.Visible = true;
                    }
                }
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để ẩn hiện Band trong BandedGrid nếu Data của cột đó = null: 
        /// search="=0"(số)
        /// search="=''"(chuỗi)
        /// </summary>
        public static void BandedHideIFNULL(string FieldName, DataTable dtGrid, BandedGridViewEx grv, string search, bool isHide = true)
        {
            if (dtGrid == null || dtGrid.Rows.Count == 0) return;
            //string col = "SIZE_01;SIZE_02;SIZE_03;SIZE_04;SIZE_05;SIZE_06;SIZE_07;SIZE_08;SIZE_09;SIZE_10;SIZE_11;SIZE_12;SIZE_13;SIZE_14;SIZE_15;SIZE_16;SIZE_17;SIZE_18;SIZE_19;SIZE_20;SIZE_21;SIZE_22;SIZE_23;SIZE_24;SIZE_25;SIZE_26;SIZE_27;SIZE_28;SIZE_29;SIZE_30;SIZE_31;SIZE_32;SIZE_33;SIZE_34;SIZE_35;SIZE_36;SIZE_37;SIZE_38;SIZE_39;";
            string colHide = "", colShow = "";
            FieldName = FieldName + ";";
            if (isHide)
            {
                foreach (DataColumn dc in dtGrid.Columns)
                {
                    if (FieldName.Contains(dc.ColumnName + ";"))
                    {
                        if (dtGrid.Select(dc.ColumnName + search).Length == dtGrid.Rows.Count)
                        {
                            colHide += dc.ColumnName + ";";
                            //ẩn đi
                        }
                        else
                        {
                            //hiện lên
                            colShow += dc.ColumnName + ";";
                        }
                    }
                }
                foreach (BandedGridColumnEx cl in grv.Columns)
                {
                    if (FieldName.Contains(cl.FieldName + ";") && dtGrid.Columns.Contains(cl.FieldName) == false && colHide.Contains(cl.FieldName) == false)
                    {
                        colHide += cl.FieldName + ";";
                    }
                }
                BandedVisibleByFieldName(colHide, colShow, grv);
            }
            else
            {
                BandedVisibleByFieldName(FieldName, grv, true);
            }
            //BandedVisibleByFieldName(colHide, grv, !isHide);
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CellMerge của BandedGrid: 
        /// <para>Trong 1 dòng nếu giá trị ở các cột trong Colname giống nhau giữa dòng trên và dòng dưới thì sẽ merge lại</para>
        /// </summary>
        public static void MergeCell(string Colname, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if ((Colname + ";").Contains(e.Column.FieldName + ";"))
            {
                //e.Merge = gvwMain.GetRowCellValue(e.RowHandle1, "COMP").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "COMP").ToString()) && gvwMain.GetRowCellValue(e.RowHandle1, "DEPT").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "DEPT").ToString());
                e.Merge = true;
                foreach (string s in TDSTool.TrimRightbyChar(Colname, ";").Split(';'))
                {
                    e.Merge = e.Merge & (grv.GetRowCellValue(e.RowHandle1, s) != null && grv.GetRowCellValue(e.RowHandle2, s) != null && grv.GetRowCellValue(e.RowHandle1, s).ToString().Equals(grv.GetRowCellValue(e.RowHandle2, s).ToString()));
                }
            }
            e.Handled = true;
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CellMerge của BandedGrid: 
        /// <para>Trong 1 dòng nếu giá trị ở các nhóm cột trong Colname[i] giống nhau thì sẽ merge lại</para>
        /// <para>Colname[0]="FCT;PLANT;VSM"    =>Giá trị trong các cột này phải giống nhau giữa dòng trên và dòng dưới thì sẽ merge lại</para>
        /// <para>Colname[1]="ABC;DEF;GHY"    =>Giá trị trong các cột này phải giống nhau giữa dòng trên và dòng dưới thì sẽ merge lại</para>
        /// </summary>
        public static void MergeCell(string[] Colname, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            foreach (string col in Colname)
            {
                if ((col + ";").Contains(e.Column.FieldName + ";"))
                {
                    //e.Merge = gvwMain.GetRowCellValue(e.RowHandle1, "COMP").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "COMP").ToString()) && gvwMain.GetRowCellValue(e.RowHandle1, "DEPT").ToString().Equals(gvwMain.GetRowCellValue(e.RowHandle2, "DEPT").ToString());
                    e.Merge = true;
                    foreach (string s in TDSTool.TrimRightbyChar(col, ";").Split(';'))
                    {
                        e.Merge = e.Merge & (grv.GetRowCellValue(e.RowHandle1, s) != null && grv.GetRowCellValue(e.RowHandle2, s) != null && grv.GetRowCellValue(e.RowHandle1, s).ToString().Equals(grv.GetRowCellValue(e.RowHandle2, s).ToString()));
                    }
                }
                e.Handled = true;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// <para>Tô màu cho ô nào có giá trị Colname=Colvalue</para>
        /// </summary>
        public static void CellStyle_SetColor(string Colname, string Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, DevExpress.Utils.HorzAlignment hAlign = DevExpress.Utils.HorzAlignment.Default)
        {
            if (e.RowHandle < 0 || Colname.Equals(e.Column.FieldName) == false) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
                if (hAlign != DevExpress.Utils.HorzAlignment.Default) e.Appearance.TextOptions.HAlignment = hAlign;
            }
        }
        public static void RecallEvent_RowCellStyle(BandedGridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs> event_func)
        {
            grv.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(event_func);
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// <para>Trong 1 dòng Giá trị của Colname=Colvalue thì sẽ tô màu nền và màu chữ tất cả các ô ngoại trừ những ô có FieldName nằm trong ColException</para>
        /// </summary>
        public static void CellStyle_SetColorException(string Colname, string Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColException = "", Font f = null)
        {
            if (e.RowHandle < 0 || (ColException + ";").Contains(e.Column.FieldName + ";")) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
                if (f != null)
                {
                    e.Appearance.Font = f;
                }
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// Tô màu dòng khi được chọn</para>
        /// </summary>
        public static void CellStyle_SelectRow(string[] ColnameException, string[] Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, DevExpress.Utils.HorzAlignment hDefault, string colLeft = "", string colRight = "", string colCenter = "")
        {
            if (e.RowHandle < 0) return;
            for (int i = 0; i < ColnameException.Length; i++)
            {
                if (grv.GetRowCellValue(e.RowHandle, ColnameException[i]) != null && grv.GetRowCellValue(e.RowHandle, ColnameException[i]).ToString().Equals(Colvalue[i]))
                    return;
            }
            colLeft = colLeft + ";";
            colRight = colRight + ";";
            colCenter = colCenter + ";";
            GridViewInfo viewInfo = (GridViewInfo)grv.GetViewInfo();
            GridCellInfo cell = viewInfo.GetGridCellInfo(e.RowHandle, e.Column);
            if (e.RowHandle == grv.FocusedRowHandle)
            {
                e.Appearance.Assign(grv.GetViewInfo().PaintAppearance.GetAppearance("FocusedRow"));
                if (colLeft.Contains(e.Column.FieldName + ";"))
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (colRight.Contains(e.Column.FieldName))
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                else if (colCenter.Contains(e.Column.FieldName))
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                else
                    e.Appearance.TextOptions.HAlignment = hDefault;
                e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                //e.Appearance.Font = grv.Columns[e.Column.FieldName].AppearanceCell.Font; // new Font(gvwMain.Appearance.Row.Font.FontFamily, 9, FontStyle.Regular);
            }
            else if (cell != null && cell.IsMerged)
            {
                foreach (GridCellInfo ci in cell.MergedCell.MergedCells)
                {
                    if (ci.RowHandle == grv.FocusedRowHandle)
                    {
                        e.Appearance.Assign(viewInfo.PaintAppearance.FocusedRow);
                        e.Appearance.TextOptions.HAlignment = grv.Columns[e.Column.FieldName].AppearanceCell.TextOptions.HAlignment;
                        e.Appearance.Font = grv.Columns[e.Column.FieldName].AppearanceCell.Font;
                        e.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                        break;
                    }
                }
            }
        }
        public static void BandedCheck(BandedGridViewEx grv, string strSearch, string formid, string strValue = "1;WO;PS;TD")
        {
            DataTable dt = (DataTable)grv.GridControl.DataSource;
            if (dt.Rows.Count == 0) return;
            string kq = "";
            string kqfalse = "";
            foreach (string str in strValue.Split(';'))
            {
                if (str == "1" || dt.Select(strSearch + " = '" + str + "'").Length > 0)
                {
                    kq += "1";
                }
                else
                {
                    kq += "0";
                }
                kqfalse += "0";
            }
            //kq = TDSTool.TrimRightbyChar(kq, ";");
            if (kq == kqfalse)
                kq = "";
            if (System.IO.File.Exists(Application.StartupPath + "\\Files\\" + formid + "\\" + formid + grv.Name + kq + ".xml"))
                grv.RestoreLayoutFromXml(Application.StartupPath + "\\Files\\" + formid + "\\" + formid + grv.Name + kq + ".xml", OptionsLayoutBase.FullLayout);
            grv.OptionsView.ShowColumnHeaders = true;
            grv.BestFitColumns();
            grv.OptionsView.ShowColumnHeaders = false;
        }
        
        public static void SaveLayout(BandedGridViewEx grv, string formID, string bandDefault = "1100", string strSearch = "GENDER", string colSzie = "SIZE_01;SIZE_02;SIZE_03;SIZE_04;SIZE_05;SIZE_06;SIZE_07;SIZE_08;SIZE_09;SIZE_10;SIZE_11;SIZE_12;SIZE_13;SIZE_14;SIZE_15;SIZE_16;SIZE_17;SIZE_18;SIZE_19;SIZE_20;SIZE_21;SIZE_22;SIZE_23;SIZE_24;SIZE_25;SIZE_26;SIZE_27;SIZE_28;SIZE_29;SIZE_30;SIZE_31;SIZE_32;SIZE_33;SIZE_34;SIZE_35;SIZE_36;SIZE_37;SIZE_38;SIZE_39")
        {
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\Files\\" + formID);
            //폴더 존재유무 확인
            if (di.Exists == false)
            {
                di.Create();
            }
            grv.SaveLayoutToXml(Application.StartupPath + "\\Files\\" + formID + "\\" + formID + grv.Name + ".xml", OptionsLayoutBase.FullLayout);
            colSzie = strSearch + ";" + colSzie;
            GridBandEx gbParent, gbChild;
            int i, nHeight;
            string tmpFieldName="";
            foreach (string str in TDSTool.ListBINbyn(0, bandDefault))
            {
                if (TDSTool.CheckBINDefault(bandDefault, str))
                {
                    grv.RestoreLayoutFromXml(Application.StartupPath + "\\Files\\" + formID + "\\" + formID + grv.Name + ".xml", OptionsLayoutBase.FullLayout);
                    foreach (string col in colSzie.Split(';'))
                    {
                        if (col.Trim().Length == 0) continue;
                        gbParent = grv.Columns[col].OwnerBand as GridBandEx;
                        nHeight = gbParent.BandLevel;
                        while (gbParent.ParentBand != null)
                            gbParent = gbParent.ParentBand as GridBandEx;
                        gbChild = gbParent;
                        for (i = 0; i < str.Length; i++)
                        {
                            if (str[i] == '1')
                            {
                                if (gbChild.HasChildren) gbChild = gbChild.Children[0] as GridBandEx;
                            }
                            else
                            {
                                if (gbChild.BandLevel == 0)
                                {
                                    gbParent = gbChild;
                                    if (gbChild.HasChildren) gbChild = gbChild.Children[0] as GridBandEx;
                                    grv.Bands.Remove(gbParent);
                                    grv.Bands.Add(gbChild);
                                    gbParent = gbChild;
                                }
                                else if (gbChild.BandLevel == nHeight)
                                {
                                    gbParent = gbChild.ParentBand as GridBandEx;
                                    gbParent.Columns.Clear();
                                    //gbParent.Columns.Add(gbChild.Columns[0]);    
                                    tmpFieldName = gbChild.Columns[0].FieldName;
                                    gbParent.Children.Remove(gbChild);
                                    gbChild = gbParent;
                                    gbParent.Columns.Add(grv.Columns[tmpFieldName]);
                                }
                                else
                                {
                                    if (gbChild.Children.Count == 0)
                                    {
                                        gbParent = gbChild.ParentBand as GridBandEx;
                                        gbParent.Columns.Clear();
                                        tmpFieldName = gbChild.Columns[0].FieldName;
                                        //gbParent.Columns.Add(gbChild.Columns[0]);                                        
                                        gbParent.Children.Remove(gbChild);
                                        gbChild = gbParent;
                                        gbParent.Columns.Add(grv.Columns[tmpFieldName]);
                                    }
                                    else
                                    {
                                        gbParent = gbChild.ParentBand as GridBandEx;
                                        GridBandEx childex = gbChild.Children[0] as GridBandEx;
                                        gbParent.Children.Remove(gbChild);
                                        gbParent.Children.Add(childex);
                                        gbChild = childex;
                                    }
                                }
                            }
                        }
                    }
                    grv.SaveLayoutToXml(Application.StartupPath + "\\Files\\" + formID + "\\" + formID + grv.Name + str + ".xml", OptionsLayoutBase.FullLayout);
                }
            }
            //PS_Remove(grv, strSearch, formID);
            //TD_Remove(grv, strSearch, formID);
            //Remove(grv, strSearch, formID);
            //grv.RestoreLayoutFromXml(Application.StartupPath + "\\Files\\" + formID + "\\" + formID + grv.Name + "_PS.xml", OptionsLayoutBase.FullLayout);
            grv.RestoreLayoutFromXml(Application.StartupPath + "\\Files\\" + formID + "\\" + formID + grv.Name + ".xml", OptionsLayoutBase.FullLayout);
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// <para>Trong 1 dòng Giá trị của AND(Colname[i]=Colvalue[i]) thì sẽ tô màu nền và màu chữ tất cả các ô ngoại trừ những ô có FieldName nằm trong ColException</para>
        /// </summary>
        public static void CellStyle_SetColorException(string[] Colname, string[] Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColException)
        {
            if (e.RowHandle < 0 || (ColException + ";").Contains(e.Column.FieldName + ";")) return;
            bool ok = false;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = (grv.GetRowCellValue(e.RowHandle, Colname[i]) != null && grv.GetRowCellValue(e.RowHandle, Colname[i]).ToString().Equals(Colvalue[i]));
                if (!ok) return;
            }
            if (ok)
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Sử dụng trong sự kiện RowCellStyle
        /// Tuyen - 19199240
        /// Date: 2019-06-03
        /// </summary>
        /// <param name="Colname"></param>
        /// <param name="Colvalue"></param>
        /// <param name="grv"></param>
        /// <param name="e"></param>
        /// <param name="BackColor"></param>
        /// <param name="ForeColor"></param>
        /// <param name="ColException"></param>
        /// <param name="FontBold"></param>
        public static void CellStyle_SetColorException(string[] Colname, string[] Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColException, bool FontBold = false)
        {
            if (e.RowHandle < 0 || (ColException + ";").Contains(e.Column.FieldName + ";")) return;
            bool ok = false;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = (grv.GetRowCellValue(e.RowHandle, Colname[i]) != null && grv.GetRowCellValue(e.RowHandle, Colname[i]).ToString().Equals(Colvalue[i]));
                if (!ok) return;
            }
            if (ok)
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
                e.Appearance.Font = new Font(grv.Appearance.Row.Font, FontStyle.Bold);
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowStyle của BandedGrid: 
        /// <para>Set màu cho những dòng có Colname thỏa mãn điều kiện với Colvalue.</para>
        /// </summary>
        public static void RowStyle_SetColor(string Colname, string Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e, Color BackColor, Color ForeColor)
        {
            if (e.RowHandle < 0) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        public static void RecallEvent_RowCellStyle(BandedGridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs> event_func)
        {
            grv.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(event_func);
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// <para>Trong 1 dòng Giá trị của AND(Colname[i]=Colvalue[i]) thì sẽ tô màu nền và màu chữ tất cả các ô nằm trong ColInvalid</para>
        /// </summary>
        public static void CellStyle_SetColorInvalid(string[] Colname, string[] Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColInvalid)
        {
            if (e.RowHandle < 0 || (ColInvalid + ";").Contains(e.Column.FieldName + ";") == false) return;
            bool ok = false;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = (grv.GetRowCellValue(e.RowHandle, Colname[i]) != null && grv.GetRowCellValue(e.RowHandle, Colname[i]).ToString().Equals(Colvalue[i]));
                if (!ok) return;
            }
            if (ok)
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowCellStyle của BandedGrid: 
        /// <para>Trong 1 dòng Giá trị của Colname=Colvalue thì sẽ tô màu nền và màu chữ tất cả các ô nằm trong ColInvalid</para>
        /// </summary>
        public static void CellStyle_SetColorInvalid(string Colname, string Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, Color BackColor, Color ForeColor, string ColInvalid)
        {
            if (e.RowHandle < 0 || (ColInvalid + ";").Contains(e.Column.FieldName + ";") == false) return;
            if (grv.GetRowCellValue(e.RowHandle, Colname) != null && grv.GetRowCellValue(e.RowHandle, Colname).ToString().Equals(Colvalue))
            {
                if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện RowStyle của BandedGrid: Set màu cho những dòng có Colname thỏa mãn điều kiện với Colvalue.
        /// <para>Set màu cho những dòng có AND(Colname[i..]=Colvalue[i...])</para>
        /// </summary>
        public static void RowStyle_SetColor(string[] Colname, string[] Colvalue, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e, Color BackColor, Color ForeColor)
        {
            if (e.RowHandle < 0) return;
            for (int i = 0; i < Colname.Length; i++)
            {
                if (grv.GetRowCellValue(e.RowHandle, Colname[i]) != null && grv.GetRowCellValue(e.RowHandle, Colname[i]).ToString().Equals(Colvalue[i]))
                {
                    if (BackColor != Color.Empty) e.Appearance.BackColor = BackColor;
                    if (ForeColor != Color.Empty) e.Appearance.ForeColor = ForeColor;
                }
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CustomDrawBandHeader của BandedGrid: Màu nền trên BandedGrid.
        /// </summary>
        public static void SetBandedBackColor(string Colname, BandedGridViewEx grv, DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventArgs e, Color cColor)
        {
            //set ApplyBaseDesign=false;
            if (e.Band == null) return;
            //Colname = Colname + ";";
            if ((Colname + ";").Contains(e.Band.Tag.ToString() + ";"))
            {
                e.Info.AllowColoring = true;
                e.Appearance.BackColor = cColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CustomDrawBandHeader của BandedGrid: màu chữ trên BandedGrid.
        /// </summary>
        public static void SetBandedForeColor(string Colname, BandedGridViewEx grv, DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventArgs e, Color cColor)
        {
            //set ApplyBaseDesign=false;
            if (e.Band == null) return;
            //Colname = Colname + ";";
            if ((Colname + ";").Contains(e.Band.Tag.ToString() + ";"))
            {
                e.Info.AllowColoring = true;
                e.Appearance.ForeColor = cColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng trong sự kiện CustomDrawBandHeader của BandedGrid: Màu nền và màu chữ trên BandedGrid.
        /// </summary>
        public static void SetBandedColor(string Colname, BandedGridViewEx grv, DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventArgs e, Color BackColor, Color ForeColor)
        {
            //set ApplyBaseDesign=false;
            if (e.Band == null) return;
            //Colname = Colname + ";";
            if (e.Band.Tag != null && (Colname + ";").Contains(e.Band.Tag.ToString() + ";"))
            {
                e.Info.AllowColoring = true;
                e.Appearance.ForeColor = ForeColor;
                e.Appearance.BackColor = BackColor;
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để Ẩn hiện các dòng trong lưới. Sử dụng trong sự kiện CustomRowFilter
        /// <para>value="TOTAL;SUM;SUB...."</para>
        /// <para>Colname="PLANT"</para>
        /// </summary>
        public static void ShowHideRowInGrid(string Colname, string value, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e, bool isVisible = true, bool Only = false)
        {
            DataView dv = grv.DataSource as DataView;
            if ((value + ";").Contains(dv[e.ListSourceRow][Colname].ToString() + ";"))
            {
                e.Visible = isVisible;
            }
            else if (Only)
            {
                e.Visible = !isVisible;
            }
            e.Handled = true;
        }
        /// <summary>
        /// Hàm này được sử dụng để Ẩn hiện các dòng trong lưới. Sử dụng trong sự kiện CustomRowFilter
        /// <para>value[i]={"TOTAL;SUM;SUB....","abc;def",...}</para>
        /// <para>Colname[i..]={"PLANT","VSM",...}</para>
        /// </summary>
        public static void ShowHideRowInGrid(string[] Colname, string[] value, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e, bool isVisible = true, bool Only = false)
        {
            DataView dv = grv.DataSource as DataView;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = ok & (value[i] + ";").Contains(dv[e.ListSourceRow][Colname[i]].ToString() + ";");
            }
            if (ok)
            {
                e.Visible = isVisible;
            }
            else if (Only)
            {
                e.Visible = !isVisible;
            }
            e.Handled = true;
        }
        /// <summary>
        /// Hàm này được sử dụng để Ẩn hiện các dòng trong lưới. Sử dụng trong sự kiện CustomRowFilter
        /// <para>value[i]={"TOTAL;SUM;SUB....","abc;def",...}</para>
        /// <para>Colname[i..]={"PLANT","VSM",...}</para>
        /// </summary>
        public static void ShowHideRowInGrid(string[] Colname, string[] value, string[] ColnameException, string[] valueException, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e, bool isVisible = true, bool Only = false)
        {
            DataView dv = grv.DataSource as DataView;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
            {
                ok = ok & (value[i] + ";").Contains(dv[e.ListSourceRow][Colname[i]].ToString() + ";");
            }
            for (int i = 0; i < ColnameException.Length; i++)
            {
                ok = ok & !(valueException[i] + ";").Contains(dv[e.ListSourceRow][ColnameException[i]].ToString() + ";");
            }
            if (ok)
            {
                e.Visible = isVisible;
            }
            else if (Only)
            {
                e.Visible = !isVisible;
            }
            e.Handled = true;
        }
        public static void RecallEvent_CustomRowFilter(BandedGridViewEx grv, Action<object, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs> event_func)
        {
            grv.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(event_func);
        }
        public static void SetColumnColor(string Colname, BandedGridViewEx grv, Color BackColor, Color ForeColor, Color BackColor2)
        {
            Colname = Colname + ";";
            foreach (BandedGridColumnEx cl in grv.Columns)
            {
                if (Colname.Contains(cl.FieldName + ";"))
                {
                    if (BackColor != Color.Empty) cl.AppearanceCell.BackColor = BackColor;
                    if (BackColor2 != Color.Empty) cl.AppearanceCell.BackColor2 = BackColor2;
                    if (ForeColor != Color.Empty) cl.AppearanceCell.ForeColor = ForeColor;
                }
            }
        }
        public static void ColumnforLine(string Colname, BandedGridViewEx grv, PaintEventArgs e, int nRowHeader, int nWidthLine)
        {
            //if (grv.Columns.Count <= 10) return;
            Colname = Colname + ";";
            BandedGridViewEx view = grv.GridControl.FocusedView as BandedGridViewEx;
            GridViewInfo info = view.GetViewInfo() as GridViewInfo;
            //int nHeight = (nRowHeader + (grv.RowCount == 0 ? 0 : + grv.RowCount ));
            Rectangle colRect;
            int i;
            foreach (BandedGridColumnEx cl in grv.Columns)
            {
                if (Colname.Contains(cl.FieldName + ";"))
                {
                    if (cl.Visible && cl.OwnerBand.Visible)
                    {
                        try
                        {
                            int lastrow = 0;
                            for (i = grv.RowCount - 1; i > 0; i--)
                            {
                                if (view.IsRowVisible(i) == DevExpress.XtraGrid.Views.Grid.RowVisibleState.Visible)
                                {
                                    lastrow = i;
                                    break;
                                }
                            }
                            if (info.GetGridCellInfo(lastrow, cl) != null)
                            {
                                colRect = info.GetGridCellInfo(lastrow, cl).Bounds;
                                e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Right - 1, nRowHeader * -1, nWidthLine, colRect.Bottom + nWidthLine));
                            }
                            else
                            {
                                colRect = info.ColumnsInfo[cl].Bounds;
                                e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Right - 1, nRowHeader * -1, nWidthLine, colRect.Bottom - colRect.Bottom / (nRowHeader + 1)));
                            }
                            //e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Left, colRect.Bottom - 1, colRect.Width, 1));
                        }
                        catch 
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }
                }/*
                else
                {
                    try
                    {
                        colRect = info.ColumnsInfo[cl].Bounds;
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Right - 1, 0, 1, colRect.Height * nHeight));                       
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(colRect.Left, colRect.Bottom - 1, colRect.Width, 1));
                    }
                    catch (Exception ex)
                    {
                    }
                }*/
            }
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của BanedeGridViewEx
        /// <para>Các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, DevExpress.XtraEditors.Repository.RepositoryItem control, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle < 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của BanedeGridViewEx
        /// <para>Giá trị tại cột Colname=value thì các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, string Colname, string value, DevExpress.XtraEditors.Repository.RepositoryItem control, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            if (grv.GetValue(e.RowHandle, Colname).ToString().Equals(value))
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của BanedeGridViewEx
        /// <para>Giá trị tại cột AND(Colname[i...]=value[i...]) thì các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, string[] Colname, string[] value, DevExpress.XtraEditors.Repository.RepositoryItem control, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
                ok = ok && (grv.GetValue(e.RowHandle, Colname[i]).ToString().Equals(value[i]));
            if (ok)
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của BanedeGridViewEx
        /// <para>Giá trị tại cột AND(Colname[i...]=value[i...]) && AND(ColnameExecption[i...] <> valueException[i...])  thì các cột trong ColHasControl sẽ hiển thị control</para>
        /// </summary>
        public static void SetControlInGrid(string ColHasControl, string[] Colname, string[] value, string[] ColnameExecption, string[] valueException, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            bool ok = true;
            for (int i = 0; i < Colname.Length; i++)
                ok = ok && (grv.GetValue(e.RowHandle, Colname[i]).ToString().Equals(value[i]));
            for (int i = 0; i < ColnameExecption.Length; i++)
                ok = ok && (grv.GetValue(e.RowHandle, ColnameExecption[i]).ToString().Equals(valueException[i]) == false);
            if (ok)
                e.RepositoryItem = control;
        }
        /// <summary>
        /// Hàm này được sử dụng để thiết lập control trong grid. Sử dụng trong sự kiện CustomRowCellEdit của BanedeGridViewEx
        /// <para>Giá trị tại cột Colname <> value thì các cột trong ColHasControl sẽ được hiển thị control</para>
        /// </summary>
        public static void SetControlInGridExceptionValue(string ColHasControl, string Colname, string value, DevExpress.XtraEditors.Repository.RepositoryItemTextEdit control, BandedGridViewEx grv, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle <= 0 || (ColHasControl + ";").Contains(e.Column.FieldName + ";") == false) return;
            if (grv.GetValue(e.RowHandle, Colname).ToString().Equals(value) == false)
                e.RepositoryItem = control;
        }
    }
    public class Rpt
    {
        public static void BandSizeRemoveGENDER(DataTable dt, DataTable dtData, bool DeleteGender = true, string colEX = "")
        {
            //CreateBandGrid(grv, dtBandedColumn);
            // DataTable dt = dtBandedColumn.Copy();
            string levelHide = "";
            if (dtData.Select("GENDER = 'WO'").Length == 0)
                levelHide = levelHide + "2;";
            if (dtData.Select("GENDER = 'PS'").Length == 0)
                levelHide = levelHide + "3;";
            if (dtData.Select("GENDER = 'TD'").Length == 0)
                levelHide = levelHide + "4";
            if (levelHide.Length == 0) return;
            levelHide = TDSTool.TrimRightbyChar(levelHide, ";");
            string sizeCol = colEX + ";SIZE_01;SIZE_02;SIZE_03;SIZE_04;SIZE_05;SIZE_06;SIZE_07;SIZE_08;SIZE_09;SIZE_10;SIZE_11;SIZE_12;SIZE_13;SIZE_14;SIZE_15;SIZE_16;SIZE_17;SIZE_18;SIZE_19;SIZE_20;SIZE_21;SIZE_22;SIZE_23;SIZE_24;SIZE_25;SIZE_26;SIZE_27;SIZE_28;SIZE_29;SIZE_30;SIZE_31;SIZE_32;SIZE_33;SIZE_34;SIZE_35;SIZE_36;SIZE_37;SIZE_38;SIZE_39;";
            String colUp1 = "1;2;3;4;5;";
            int nup = 0;
            int i = 0;
            foreach (string s in levelHide.Split(';'))
            {
                dt.Columns["UP" + s].ColumnName = "UP" + s + "[REMOVET]";
                colUp1 = colUp1.Replace(s + ";", "");
                nup++;
            }
            if (colUp1.Length > 0)
            {
                int coldR = Convert.ToInt16(colUp1.Split(';')[0]);
                if (coldR != 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                        {
                            dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                        }
                        else
                        {
                            dr["UP" + coldR] = dr[1];
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                        {
                            dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                        }
                    }
                }
                foreach (string s in levelHide.Split(';'))
                {
                    dt.Columns.Remove("UP" + s + "[REMOVET]");
                }
                i = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.StartsWith("UP"))
                    {
                        dc.ColumnName = "UP" + (i++);
                    }
                }
                if (DeleteGender)
                {
                    DataRow[] dr = dt.Select("FIELDNAME='GENDER'");
                    if (dr.Length > 0)
                        dt.Rows.Remove(dr[0]);
                }
            }
        }
        public static void BandSizeRemoveSize(DataTable dt, DataTable dtData)
        {
            string sizeCol = "SIZE_01;SIZE_02;SIZE_03;SIZE_04;SIZE_05;SIZE_06;SIZE_07;SIZE_08;SIZE_09;SIZE_10;SIZE_11;SIZE_12;SIZE_13;SIZE_14;SIZE_15;SIZE_16;SIZE_17;SIZE_18;SIZE_19;SIZE_20;SIZE_21;SIZE_22;SIZE_23;SIZE_24;SIZE_25;SIZE_26;SIZE_27;SIZE_28;SIZE_29;SIZE_30;SIZE_31;SIZE_32;SIZE_33;SIZE_34;SIZE_35;SIZE_36;SIZE_37;SIZE_38;SIZE_39";
            foreach (string s in sizeCol.Split(';'))
            {
                if (dtData.Select(s + " = 0").Length + dtData.Select(s + " is null ").Length == dtData.Rows.Count)
                {
                    DataRow[] dr = dt.Select("FIELDNAME='" + s + "'");
                    if (dr.Length > 0)
                        dt.Rows.Remove(dr[0]);
                    dtData.Columns.Remove(s);
                }
            }
        }
        public static void BandSizeRemoveGender_Size(DataTable dt, DataTable dtData, string GenderColumn = "GENDER", bool DeleteGender = false, string colEX = "")
        {
            string levelHide = "";
            if (colEX.Length > 0)
                colEX = TDSTool.TrimRightbyChar(colEX, ";") + ";";
            string sizeCol = colEX + "SIZE_01;SIZE_02;SIZE_03;SIZE_04;SIZE_05;SIZE_06;SIZE_07;SIZE_08;SIZE_09;SIZE_10;SIZE_11;SIZE_12;SIZE_13;SIZE_14;SIZE_15;SIZE_16;SIZE_17;SIZE_18;SIZE_19;SIZE_20;SIZE_21;SIZE_22;SIZE_23;SIZE_24;SIZE_25;SIZE_26;SIZE_27;SIZE_28;SIZE_29;SIZE_30;SIZE_31;SIZE_32;SIZE_33;SIZE_34;SIZE_35;SIZE_36;SIZE_37;SIZE_38;SIZE_39";
            //Remove size
            foreach (string s in sizeCol.Split(';'))
            {
                if (dtData.Select(s + " = 0").Length + dtData.Select(s + " is null ").Length == dtData.Rows.Count)
                {
                    DataRow[] dr = dt.Select("FIELDNAME='" + s + "'");
                    if (dr.Length > 0)
                        dt.Rows.Remove(dr[0]);
                    dtData.Columns.Remove(s);
                }
            }
            String colUp1 = "1;2;3;4;5;";
            //Remove Gender
            int nup = 0;
            int i = 0;
            if (dtData.Select(GenderColumn + " = 'WO'").Length == 0)
                levelHide = levelHide + "2;";
            if (dtData.Select(GenderColumn + " = 'PS'").Length == 0)
                levelHide = levelHide + "3;";
            if (dtData.Select(GenderColumn + " = 'TD'").Length == 0)
                levelHide = levelHide + "4";
            if (levelHide.Length == 0) return;
            levelHide = TDSTool.TrimRightbyChar(levelHide, ";");
            foreach (string s in levelHide.Split(';'))
            {
                dt.Columns["UP" + s].ColumnName = "UP" + s + "[REMOVET]";
                colUp1 = colUp1.Replace(s + ";", "");
                nup++;
            }
            if (colUp1.Length > 0)
            {
                int coldR = Convert.ToInt16(colUp1.Split(';')[0]);
                if (coldR != 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                        {
                            dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                        }
                        else
                        {
                            dr["UP" + coldR] = dr[1];
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sizeCol.Contains(dr["FIELDNAME"].ToString() + ";"))
                        {
                            dr["NUP"] = Convert.ToInt16(dr["NUP"]) - nup;
                        }
                    }
                }
                foreach (string s in levelHide.Split(';'))
                {
                    dt.Columns.Remove("UP" + s + "[REMOVET]");
                }
                i = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.StartsWith("UP"))
                    {
                        dc.ColumnName = "UP" + (i++);
                    }
                }
                if (DeleteGender)
                {
                    DataRow[] dr = dt.Select("FIELDNAME='GENDER'");
                    if (dr.Length > 0)
                        dt.Rows.Remove(dr[0]);
                }
            }
        } 
    }
}
