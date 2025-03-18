using JPlatform.Client.JBaseForm8;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace VT.APPS.MES
{
    public class P_CQCM0029_Q : BaseProcClass
    {
        public P_CQCM0029_Q()
        {
            // Modify Code : Procedure Name
            _ProcName = "P_CQCM0029_Q"; //P_CQMS0002_Q   // PROCEDURE MESMGR.SP_QMS_DAILY
            ParamAdd();
        }
        private void ParamAdd()
        {
            // Modify Code : Procedure Parameter
            _ParamInfo.Add(new ParamInfo("V_P_WORK_TYPE", "Varchar2", 0, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("V_P_COMPANY"  , "Varchar2", 0, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("V_P_SUB_PLANT", "Varchar2", 0, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("V_P_DATE_F"   , "Varchar2", 0, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("V_P_DATE_T"   , "Varchar2", 0, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("V_P_ITEM"     , "Varchar2", 0, "Input", typeof(System.String)));
            _ParamInfo.Add(new ParamInfo("V_P_VIEW_TYPE", "Varchar2", 0, "Input", typeof(System.String)));
        }
        public DataTable SetParamData(DataTable dataTable, System.String V_P_WORK_TYPE, 
                                                           System.String V_P_COMPANY, 
                                                           System.String V_P_SUB_PLANT,
                                                           System.String V_P_DATE_F, 
                                                           System.String V_P_DATE_T, 
                                                           System.String V_P_ITEM, 
                                                           System.String V_P_VIEW_TYPE)
        {
            if (dataTable == null)
            {
                dataTable = new DataTable(_ProcName);
                foreach (ParamInfo pi in _ParamInfo)
                {
                    dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
                }
            }
            // Modify Code : Procedure Parameter
            object[] objData = new object[] {
                    V_P_WORK_TYPE,
                    V_P_COMPANY,
                    V_P_SUB_PLANT,
                    V_P_DATE_F,
                    V_P_DATE_T,
                    V_P_ITEM,
                    V_P_VIEW_TYPE
                };
            dataTable.Rows.Add(objData);
            return dataTable;
        }
        public string V_P_WORK_TYPE {get; set;}
        public string V_P_COMPANY { set; get; }
        public string V_P_SUB_PLANT { set; get; }
        public string V_P_DATE_F { set; get; }
        public string V_P_DATE_T { set; get; }
        public string V_P_ITEM { set; get; }
        public string V_P_VIEW_TYPE { set; get; }
    }
}
