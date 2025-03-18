CREATE OR REPLACE PROCEDURE MESMGR.P_CQCM0029_Q (  
-- Date        By             Modification            
----------------------------------------------------------------------------------------------------------------------- 
--20220514    22194805       New auto-generated                                                            
/*********^*********^*********^*********^*********^*********^*********^*********^*********^*********^*********^********/  
    V_P_WORK_TYPE     IN VARCHAR2 DEFAULT 'Q',       -- WORK TYPE (Q:QUERY, N:INSERT, U:UPDATE, D:DELETE, META:GET DATA ETC)  
    V_P_COMPANY       IN VARCHAR2,   
    V_P_SUB_PLANT     IN VARCHAR2,
    V_P_DATE_F        IN VARCHAR2,
    V_P_DATE_T        IN VARCHAR2,
    V_P_ITEM          IN VARCHAR2,
    V_P_VIEW_TYPE     IN VARCHAR2,
    
    V_P_ERROR_CODE       OUT VARCHAR2,    -- Return error codes  
    V_P_ROW_COUNT        OUT NUMBER,      -- Record number of lines that run / return
    V_P_ERROR_NOTE       OUT NVARCHAR2,   -- Custom String  
    V_P_RETURN_STR       OUT NVARCHAR2,   -- Custom Return  
    V_P_ERROR_STR        OUT NVARCHAR2,   -- Error Messages 
    V_ERRORSTATE         OUT VARCHAR2,    -- Error number / severity / error status code line number of the error occurring routines
    V_ERRORPROCEDURE     OUT NVARCHAR2,   -- Error procedure / trigger
    CV_1                 OUT SYS_REFCURSOR,
    CV_2                 OUT SYS_REFCURSOR,
    CV_3                 OUT SYS_REFCURSOR,
    CV_4                 OUT SYS_REFCURSOR,
    CV_5                 OUT SYS_REFCURSOR,
    CV_6                 OUT SYS_REFCURSOR,
    CV_7                 OUT SYS_REFCURSOR,
    CV_8                 OUT SYS_REFCURSOR,
    CV_9                 OUT SYS_REFCURSOR--,
--    CV_10                OUT SYS_REFCURSOR,
--    CV_11                OUT SYS_REFCURSOR,
--    CV_12                OUT SYS_REFCURSOR,
--    CV_13                OUT SYS_REFCURSOR
    
) 
AS 

--    V_P_SQL CLOB;
--    LAST_PLANT_ID VARCHAR2(10);
--    V_P_PIVOT VARCHAR2(30000);
--    V_P_SUMCOL VARCHAR2(30000);
--    V_P_COL VARCHAR2(30000);
    /* Variable definitions used within the procedure */     
    V_P_MIN_DATE VARCHAR2(8);
    V_P_TO_DAY VARCHAR2(8);

BEGIN 
 
    IF V_P_WORK_TYPE IN ( 'Q' )  THEN
    BEGIN
    ----------------------------------------------------------------------------------------------------------------------- 
    
        V_P_MIN_DATE := CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN TO_CHAR(SYSDATE - 30,'YYYYMMDD')
                                           WHEN 'WEEKLY' THEN TO_CHAR(TRUNC(SYSDATE-9*7,'IW'),'YYYYMMDD')
                                           WHEN 'MONTHLY' THEN TO_CHAR(ADD_MONTHS(SYSDATE,-9),'YYYYMM') || '01'
                                           WHEN 'YEARLY' THEN TO_CHAR(ADD_MONTHS(SYSDATE,-108),'YYYY') || '0101'
                                           ELSE TO_CHAR(SYSDATE,'YYYYMMDD') END;
        V_P_TO_DAY :=TO_CHAR(SYSDATE,'YYYYMMDD');
      -- Run the query   
        OPEN CV_1 FOR
           WITH DATE_SEQ AS(
                 SELECT SYS_DATE, 
                        CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN ROWNUM
                           WHEN 'WEEKLY' THEN DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD'))
                           WHEN 'MONTHLY' THEN DENSE_RANK () OVER (ORDER BY SUBSTR(SYS_DATE,1,6))
                           WHEN 'YEARLY' THEN DENSE_RANK () OVER (ORDER BY PLAN_YEAR )
                           ELSE 0 END DATE_SEQ,
                        CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN '('|| SUBSTR(SYS_DATE,5,2) ||'/' ||SUBSTR(SYS_DATE,-2) ||')'
                           WHEN 'WEEKLY' THEN 'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00')) 
                           --TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'MM/DD')
                           WHEN 'MONTHLY' THEN SUBSTR(SYS_DATE,1,4) ||'/' ||SUBSTR(SYS_DATE,5,2)
                           WHEN 'YEARLY' THEN SUBSTR(SYS_DATE,1,4)
                           ELSE ' ' END DATE_SHOW
                   FROM MWIPCALDEF
                  WHERE CALENDAR_ID ='VT'
                    AND SYS_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
               ORDER BY SYS_DATE
            )
            SELECT V_P_VIEW_TYPE VIEW_TYPE, DATE_SEQ, DATE_SHOW WORK_DATE, NVL(FCT, 'AVG') FCT, 
                   AVG(TOTAL_DEFECT) TOTAL_DEFECT, 100 - AVG(DEFECT_RATE) DEFECT_RATE,
                   GROUPING_ID(DATE_SEQ, DATE_SHOW, FCT) GRP_ID
              FROM (
                  SELECT FCT_GRP FCT,
                         DATE_SEQ, DATE_SHOW,
                         SUM(CASE WHEN DEFECT_CODE <> 'E000' THEN NVL(DEFECT_QTY,0) ELSE 0 END) TOTAL_DEFECT,
--                         SUM(DEFECTIVE_QTY) AUDIT_SIZE_CD ,COUNT(DISTINCT UPC_LABEL_ID)  DEFECTIVE_QTY, 
--                         ROUND(COUNT(DISTINCT UPC_LABEL_ID)/SUM(DEFECTIVE_QTY) *100,1) DEFECT_RATE
                         SUM(NVL(DEFECTIVE_QTY,0)) AUDIT_SIZE_CD,
                         COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) DEFECTIVE_QTY,
                         ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) /SUM(NVL(DEFECTIVE_QTY,0))*100,1) DEFECT_RATE 
                    FROM CEDCHFPMST A 
                         INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                         INNER JOIN DATE_SEQ C ON A.AUDIT_DATE = C.SYS_DATE
                    --WHERE DEFECT_CODE <> 'E000'
                   GROUP BY FCT_GRP, DATE_SEQ, DATE_SHOW
                 ) 
            GROUP BY ROLLUP((DATE_SEQ,DATE_SHOW),FCT)
            HAVING GROUPING_ID(DATE_SEQ,FCT) IN(0,1);
                         
            ----02--BY DEFECT TYPE --
        OPEN CV_2 FOR
           SELECT DEFECT_CODE ||'-'|| C.DATA_1 DEFECT_CODE, SUM(DEFECT_QTY) DEFECT_QTY       
             FROM CEDCHFPMST A 
                  INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                  INNER JOIN MGCMTBLDAT C ON B.FACTORY = C.FACTORY AND B.DEFECT_CODE = C.KEY_1
            WHERE A.FACTORY = 'VT'
              AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT)
              AND DEFECT_CODE <> 'E000'
              AND TABLE_NAME = 'CEDC_DEF_HFPA'
              AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
            GROUP BY DEFECT_CODE, C.DATA_1
            ORDER BY 1
             ;
             
            --03--TOP 5 MODEL -- 
        OPEN CV_3 FOR
           WITH DATE_SEQ AS(
                SELECT SYS_DATE, 
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN ROWNUM
                       WHEN 'WEEKLY' THEN DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD'))
                       WHEN 'MONTHLY' THEN DENSE_RANK () OVER (ORDER BY SUBSTR(SYS_DATE,1,6))
                       WHEN 'YEARLY' THEN DENSE_RANK () OVER (ORDER BY PLAN_YEAR )
                       ELSE 0 END DATE_SEQ,
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN SUBSTR(SYS_DATE,5,2) ||'/' ||SUBSTR(SYS_DATE,-2)
                       WHEN 'WEEKLY' THEN 'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00')) --TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'MM/DD')
                       WHEN 'MONTHLY' THEN SUBSTR(SYS_DATE,1,4) ||'/' ||SUBSTR(SYS_DATE,5,2)
                       WHEN 'YEARLY' THEN SUBSTR(SYS_DATE,1,4)
                       ELSE ' ' END DATE_SHOW
                FROM MWIPCALDEF
                WHERE CALENDAR_ID = 'VT'
                AND SYS_DATE BETWEEN /*V_P_MIN_DATE*/V_P_DATE_F  AND V_P_TO_DAY
                ORDER BY SYS_DATE
            ), TOP5MODEL AS(
                SELECT VIEW_TYPE, MODEL_DESC, TOTAL_DEFECT, ROW_SEQ
                FROM (
                      SELECT V_P_VIEW_TYPE VIEW_TYPE, MODEL_DESC, TOTAL_DEFECT, ROW_NUMBER()OVER(ORDER BY TOTAL_DEFECT DESC ) ROW_SEQ 
                        FROM (
                              SELECT MODEL_DESC, 
                                     SUM(CASE WHEN DEFECT_CODE<>'E000' THEN NVL(DEFECT_QTY,0) ELSE 0 END) TOTAL_DEFECT 
                                FROM CEDCHFPMST A 
                                     INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                                     INNER JOIN DATE_SEQ C ON A.AUDIT_DATE = C.SYS_DATE
                               WHERE A.FACTORY = 'VT'
                                 AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT)
                                 AND DEFECT_CODE <> 'E000'
                               GROUP BY MODEL_DESC 
                        )
                    ) WHERE ROW_SEQ<=5
            )
            SELECT VIEW_TYPE,A.MODEL_DESC, A.ROW_SEQ, A.TOTAL_DEFECT, 
                   LISTAGG(ROW_CNT ||'.' || DEFECT_NAME ||': ' || B.TOTAL_DEFECT ||'('|| ROUND(B.TOTAL_DEFECT*100/A.TOTAL_DEFECT,2) || '%)', '|') WITHIN GROUP (ORDER BY B.TOTAL_DEFECT DESC) COL_LIST 
              FROM TOP5MODEL A 
                   INNER JOIN (
                            SELECT MODEL_DESC, DEFECT_CODE, DATA_1 DEFECT_NAME, TOTAL_DEFECT, ROW_CNT
                              FROM (
                                    SELECT FACTORY, MODEL_DESC, DEFECT_CODE, TOTAL_DEFECT,
                                           ROW_NUMBER() OVER (PARTITION BY MODEL_DESC ORDER BY TOTAL_DEFECT DESC) AS ROW_CNT  
                                    FROM (
                                        SELECT A.FACTORY, A.MODEL_DESC, DEFECT_CODE, 
                                               SUM(CASE WHEN DEFECT_CODE <> 'E000' THEN NVL(DEFECT_QTY,0) ELSE 0 END) TOTAL_DEFECT 
                                          FROM CEDCHFPMST A INNER JOIN TOP5MODEL X ON A.MODEL_DESC = X.MODEL_DESC
                                               INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                                               INNER JOIN DATE_SEQ C ON A.AUDIT_DATE = C.SYS_DATE                     
                                         WHERE A.FACTORY = 'VT' 
                                           AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT)
                                           AND DEFECT_CODE <> 'E000'
                                      GROUP BY A.FACTORY, A.MODEL_DESC, DEFECT_CODE
                                ) A 
                            ) A INNER JOIN MGCMTBLDAT C ON 'VT' = C.FACTORY AND A.DEFECT_CODE = C.KEY_1 AND C.TABLE_NAME = 'CEDC_DEF_HFPA'
                            WHERE ROW_CNT <= 3
                        ) B ON A.MODEL_DESC = B.MODEL_DESC
            GROUP BY VIEW_TYPE, A.MODEL_DESC, A.ROW_SEQ, A.TOTAL_DEFECT
            ORDER BY VIEW_TYPE, A.ROW_SEQ
            ;
             
        -- 04--TOP 3 DEFECT 10 WEEEK --
        OPEN CV_4 FOR
         SELECT DATE_SHOW DATE_SHOW, MAX(DEFECT_CODE)DEFECT_CODE, DEFECT_QTY, DEF_SEQ, TOTAL_DEFECT, DEFECT_PERCENT
           FROM (
           WITH DATE_SEQ AS(
                SELECT SYS_DATE, 'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00'))  DATE_SHOW
                  FROM MWIPCALDEF
                 WHERE CALENDAR_ID = 'VT'
                   AND SYS_DATE BETWEEN  TO_CHAR(TRUNC(SYSDATE-9*7,'IW'),'YYYYMMDD') AND TO_CHAR(SYSDATE,'YYYYMMDD')
                 ORDER BY SYS_DATE
            ) 
            SELECT DATE_SHOW, DEFECT_CODE, DEFECT_QTY, DEF_SEQ, TOTAL_DEFECT,
                   ROUND((DEFECT_QTY /TOTAL_DEFECT)*100, 2) DEFECT_PERCENT
            FROM (
                  SELECT DATE_SHOW, DEFECT_CODE DEFECT_CODE, DEFECT_QTY, 
                         DENSE_RANK () OVER (PARTITION BY DATE_SHOW ORDER BY DEFECT_QTY DESC) DEF_SEQ
                         , ROUND(SUM(DEFECT_QTY) OVER(ORDER BY DATE_SHOW),2) TOTAL_DEFECT
                    FROM (
                           SELECT DATE_SHOW, DEFECT_CODE ||'-'|| D.DATA_1 DEFECT_CODE, 
                                  SUM(CASE WHEN DEFECT_CODE<>'E000' THEN NVL(DEFECT_QTY,0) ELSE 0 END) DEFECT_QTY 
                             FROM CEDCHFPMST A 
                                  INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                                  INNER JOIN DATE_SEQ C ON A.AUDIT_DATE = C.SYS_DATE
                                  INNER JOIN MGCMTBLDAT D ON B.FACTORY = D.FACTORY AND B.DEFECT_CODE = D.KEY_1
                            WHERE D.FACTORY = 'VT'
                              AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT)
                              AND DEFECT_CODE <> 'E000'
                              AND TABLE_NAME = 'CEDC_DEF_HFPA'
                            GROUP BY DATE_SHOW, DEFECT_CODE, D.DATA_1
                       )
               ) 
            WHERE DEF_SEQ <= 3)
            GROUP BY DATE_SHOW, DEFECT_QTY, DEF_SEQ, TOTAL_DEFECT, DEFECT_PERCENT
            ORDER BY DATE_SHOW, DEF_SEQ, DEFECT_CODE
             ;
            
             -----05--BY DEFECT HOURS pass rate -- % PAS RATE BY PLANT -- 
        OPEN CV_5 FOR     
           SELECT 'AVG' FCT,--NVL(FCT_GRP,'AVG') FCT, 
                  DECODE(NVL(V_P_SUB_PLANT,''), '', PLANT_ID, LINE_CODE) WORK_DATE,
                  ROUND(AVG(DEFECT_QTY),1) DEFECT_QTY,
                  ROUND(100 - AVG(DEFECT_RATE),1) DEFECT_RATE
             FROM (
                    SELECT /*CASE WHEN SUBSTR(A.AUDIT_SCAN_TIME,9,2) <='07' THEN '07'
                           WHEN SUBSTR(A.AUDIT_SCAN_TIME,9,2) >='15' THEN '15'
                           ELSE SUBSTR(A.AUDIT_SCAN_TIME,9,2) END Hours,FCT_GRP,*/
                           PLANT_ID, LINE_CODE,
                           ROUND(SUM(DEFECT_QTY)/COUNT(DISTINCT AUDIT_DATE),2) DEFECT_QTY,
                           --ROUND(COUNT(DISTINCT UPC_LABEL_ID)/SUM(DEFECTIVE_QTY) *100,1) DEFECT_RATE    
                           SUM(NVL(DEFECTIVE_QTY,0)) AUDIT_SIZE_CD,
                           COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) DEFECTIVE_QTY,
                           ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) /SUM(NVL(DEFECTIVE_QTY,0))*100,1) DEFECT_RATE     
                      FROM CEDCHFPMST A 
                           INNER JOIN  CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                     WHERE A.FACTORY = 'VT'
                       AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT)
                       AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                       --AND DEFECT_CODE <> 'E000'
                     GROUP BY PLANT_ID, LINE_CODE/*CASE WHEN SUBSTR(A.AUDIT_SCAN_TIME,9,2) <='07' THEN '07'
                                  WHEN SUBSTR(A.AUDIT_SCAN_TIME,9,2) >='15' THEN '15'
                                  ELSE SUBSTR(A.AUDIT_SCAN_TIME,9,2) END, FCT_GRP*/
            )
            GROUP BY DECODE(NVL(V_P_SUB_PLANT,''), '', PLANT_ID, LINE_CODE) --ROLLUP(HOURS, FCT_GRP)
            --HAVING GROUPING_ID(HOURS, FCT_GRP) IN (0,1)
            ORDER BY 2
            ;
             
             --------06--TOP 3 PLANT BY DEFECT RATE --
        OPEN CV_6 FOR
            WITH DATE_SEQ AS(
                SELECT SYS_DATE, 
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN ROWNUM
                       WHEN 'WEEKLY' THEN DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD'))
                       WHEN 'MONTHLY' THEN DENSE_RANK () OVER (ORDER BY SUBSTR(SYS_DATE,1,6))
                       WHEN 'YEARLY' THEN DENSE_RANK () OVER (ORDER BY PLAN_YEAR )
                       ELSE 0 END DATE_SEQ,
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN SUBSTR(SYS_DATE,5,2) ||'/' ||SUBSTR(SYS_DATE,-2)
                       WHEN 'WEEKLY' THEN 'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00')) --TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'MM/DD')
                       WHEN 'MONTHLY' THEN SUBSTR(SYS_DATE,1,4) ||'/' ||SUBSTR(SYS_DATE,5,2)
                       WHEN 'YEARLY' THEN SUBSTR(SYS_DATE,1,4)
                       ELSE ' ' END DATE_SHOW
                FROM MWIPCALDEF
                WHERE CALENDAR_ID = 'VT'
--                AND SYS_DATE BETWEEN V_P_MIN_DATE  AND V_P_TO_DAY
                 AND SYS_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                ORDER BY SYS_DATE
            ) 
            SELECT * 
            FROM (
                    SELECT VIEW_TYPE, DECODE(NVL(V_P_SUB_PLANT,''), '', PLANT_ID, LINE_CODE) PLANT_ID, 
                           AUDIT_SIZE_CD, DEFECTIVE_QTY, 
                           DEFECT_RATE, ROW_NUMBER() OVER (ORDER BY DEFECT_RATE DESC) AS  ROW_SEQ 
                    FROM (
                           SELECT V_P_VIEW_TYPE VIEW_TYPE, PLANT_ID, LINE_CODE,
                                       --AUDIT_DATE,                               
                                       --NVL(DEFECTIVE_QTY,0)  AUDIT_SIZE_CD ,
                                       --CASE WHEN DEFECT_CODE<>'E000' THEN NVL(DEFECTIVE_QTY,0) ELSE 0 END  DEFECTIVE_QTY ,                                
                                       --CASE WHEN DEFECT_CODE<>'E000' THEN A.HFPA_ID ELSE NULL END   UPC_LABEL_ID     
                                    --SUM(NVL(DEFECTIVE_QTY,0))  AUDIT_SIZE_CD,
                                    --COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END )  DEFECTIVE_QTY,
                                    --ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) /SUM(NVL(DEFECTIVE_QTY,0))*100,1)  DEFECT_RATE 
                                    SUM(NVL(DEFECTIVE_QTY,0)) AUDIT_SIZE_CD,
                                    COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) DEFECTIVE_QTY,
                                    ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END ) /SUM(NVL(DEFECTIVE_QTY,0))*100,1) DEFECT_RATE   
                               FROM CEDCHFPMST A,
                                    CEDCHFPDTL B, DATE_SEQ C  
                              WHERE A.FACTORY = 'VT' 
                                AND A.FACTORY = B.FACTORY
                                AND A.HFPA_ID = B.HFPA_ID        
                                AND A.AUDIT_DATE = C.SYS_DATE
                                AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT)
                        GROUP BY PLANT_ID, LINE_CODE --,AUDIT_DATE
                        --ORDER BY DEFECT_RATE DESC
                    )
                 ) 
                 WHERE ROW_SEQ <= 3
                ;
             
            -- 07--TOP 5 DEFECT RATE BY TYPE-- 
        OPEN CV_7 FOR
        SELECT * FROM (
             SELECT DEFECT_CODE, AUDIT_SIZE_CD, DEFECTIVE_QTY, DEFECT_TOTAL,
                   ROUND (NVL (DEFECTIVE_QTY, 0) / DEFECT_TOTAL * 100, 1) DEFECT_RATE
              FROM (SELECT DEFECT_CODE, AUDIT_SIZE_CD, DEFECTIVE_QTY,
                           SUM (AUDIT_SIZE_CD) OVER () DEFECT_TOTAL,
                           ROW_NUMBER () OVER (ORDER BY AUDIT_SIZE_CD DESC) AS ROW_SEQ
                      FROM (  SELECT DEFECT_CODE || '-' || C.DATA_1 DEFECT_CODE,
                                     SUM (NVL (DEFECTIVE_QTY, 0)) AUDIT_SIZE_CD,
                                     COUNT ( DISTINCT CASE WHEN DEFECT_CODE <> 'E000'
                                                           THEN UPC_LABEL_ID
                                                           ELSE NULL END)
                                        DEFECTIVE_QTY
                                --     ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END) /SUM(NVL(DEFECTIVE_QTY,0))*100,1)  DEFECT_RATE
                                FROM CEDCHFPMST A
                                     INNER JOIN CEDCHFPDTL B
                                        ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                                     INNER JOIN MGCMTBLDAT C
                                        ON     B.FACTORY = C.FACTORY
                                           AND B.DEFECT_CODE = C.KEY_1
                               WHERE     C.FACTORY = 'VT'
                                     AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT) 
                                     AND DEFECT_CODE <> 'E000'
                                     AND TABLE_NAME = 'CEDC_DEF_HFPA'
                                     AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                            GROUP BY DEFECT_CODE, C.DATA_1
                            ORDER BY 2 DESC))
             WHERE ROW_SEQ <= 5
             ORDER BY 5 DESC)
             UNION ALL 
             SELECT * FROM (
                 WITH DATA_ALL AS(
                     SELECT DEFECT_CODE, AUDIT_SIZE_CD, DEFECTIVE_QTY, DEFECT_TOTAL,
                            ROUND (NVL (DEFECTIVE_QTY, 0) / DEFECT_TOTAL * 100, 1) DEFECT_RATE,
                            ROW_SEQ
                      FROM (SELECT DEFECT_CODE, AUDIT_SIZE_CD, DEFECTIVE_QTY,
                                   SUM (AUDIT_SIZE_CD) OVER () DEFECT_TOTAL,
                                   ROW_NUMBER () OVER (ORDER BY AUDIT_SIZE_CD DESC) AS ROW_SEQ
                              FROM (  SELECT DEFECT_CODE || '-' || C.DATA_1 DEFECT_CODE,
                                             SUM (NVL (DEFECTIVE_QTY, 0)) AUDIT_SIZE_CD,
                                             COUNT ( DISTINCT CASE WHEN DEFECT_CODE <> 'E000'
                                                                   THEN UPC_LABEL_ID
                                                                   ELSE NULL END)
                                                DEFECTIVE_QTY
                                        --     ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN UPC_LABEL_ID ELSE NULL END) /SUM(NVL(DEFECTIVE_QTY,0))*100,1)  DEFECT_RATE
                                        FROM CEDCHFPMST A
                                             INNER JOIN CEDCHFPDTL B
                                                ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID
                                             INNER JOIN MGCMTBLDAT C
                                                ON     B.FACTORY = C.FACTORY
                                                   AND B.DEFECT_CODE = C.KEY_1
                                       WHERE C.FACTORY = 'VT'
                                         AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT) 
                                         AND DEFECT_CODE <> 'E000'
                                         AND TABLE_NAME = 'CEDC_DEF_HFPA'
                                         AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                                    GROUP BY DEFECT_CODE, C.DATA_1
                                    ORDER BY 2 DESC))
                            WHERE ROW_SEQ >5
                            )
                            SELECT 'OTHER' DEFECT_CODE, 0 AUDIT_SIZE_CD, 0 DEFECTIVE_QTY, 0 DEFECT_TOTAL, SUM(DEFECT_RATE) DEFECT_RATE
                            FROM DATA_ALL
                          );
             
         ---------08--TOP 3 LINE CHANGE RATE HIGHTEST --
        OPEN CV_8 FOR
            SELECT PLANT_ID, LINE_CODE, W_1_DEFECT_RATE, W_DEFECT_RATE, DIF_RATE
            FROM (
                SELECT PLANT_ID, LINE_CODE, W_1_DEFECT_RATE, W_DEFECT_RATE, DIF_RATE, ROW_NUMBER() OVER (ORDER BY DIF_RATE DESC) AS RN 
                    FROM (
                        SELECT PLANT_ID,LINE_CODE, 
                            SUM(W_1_AUDIT_SIZE_CD) W_1_AUDIT_SIZE_CD ,
                            SUM(W_1_DEFECTIVE_QTY) W_1_DEFECTIVE_QTY, 
                            SUM(W_1_DEFECT_RATE) W_1_DEFECT_RATE, 
                            SUM(W_AUDIT_SIZE_CD) W_AUDIT_SIZE_CD, 
                            SUM(W_DEFECTIVE_QTY) W_DEFECTIVE_QTY, 
                            SUM(W_DEFECT_RATE) W_DEFECT_RATE,
                            ABS(SUM(W_DEFECT_RATE)- SUM(W_1_DEFECT_RATE)) DIF_RATE
                        FROM ( 
                               SELECT PLANT_ID, LINE_CODE, 
                                      --SUM(DEFECTIVE_QTY) W_1_AUDIT_SIZE_CD,
                                      --COUNT(DISTINCT UPC_LABEL_ID)  W_1_DEFECTIVE_QTY, 
                                      --ROUND(COUNT(DISTINCT UPC_LABEL_ID)/SUM(DEFECTIVE_QTY) *100,1) W_1_DEFECT_RATE,0 W_AUDIT_SIZE_CD,0 W_DEFECTIVE_QTY,0 W_DEFECT_RATE
                                      SUM(NVL(DEFECTIVE_QTY,0)) W_1_AUDIT_SIZE_CD,
                                      COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN A.UPC_LABEL_ID ELSE NULL END ) W_1_DEFECTIVE_QTY,
                                      ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN A.UPC_LABEL_ID ELSE NULL END ) /SUM(NVL(DEFECTIVE_QTY,0))*100,1) W_1_DEFECT_RATE
                                      ,0 W_AUDIT_SIZE_CD,0 W_DEFECTIVE_QTY,0 W_DEFECT_RATE
                                 FROM CEDCHFPMST A INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID 
                                WHERE A.FACTORY = 'VT'--DEFECT_CODE <> 'E000'
                                  AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT) 
                                  AND A.AUDIT_DATE BETWEEN TO_CHAR(TRUNC(SYSDATE-7,'IW'),'YYYYMMDD') AND TO_CHAR(TRUNC(SYSDATE-7,'IW')+6,'YYYYMMDD')
                                GROUP BY PLANT_ID,LINE_CODE
                                UNION ALL
                                SELECT PLANT_ID,LINE_CODE, 0,0,0, 
--                                     SUM(DEFECTIVE_QTY) W_AUDIT_SIZE_CD 
--                                     ,COUNT(DISTINCT UPC_LABEL_ID)  W_DEFECTIVE_QTY, 
--                                     ROUND(COUNT(DISTINCT UPC_LABEL_ID)/SUM(DEFECTIVE_QTY) *100,1) W_DEFECT_RATE
                                       SUM(NVL(DEFECTIVE_QTY,0)) W_AUDIT_SIZE_CD,
                                       COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN A.UPC_LABEL_ID ELSE NULL END ) W_DEFECTIVE_QTY,
                                       ROUND(COUNT(DISTINCT CASE WHEN DEFECT_CODE<>'E000' THEN A.UPC_LABEL_ID ELSE NULL END )/SUM(NVL(DEFECTIVE_QTY,0))*100,1) W_DEFECT_RATE
                                  FROM CEDCHFPMST A INNER JOIN CEDCHFPDTL B ON A.FACTORY = B.FACTORY AND A.HFPA_ID = B.HFPA_ID 
                                 WHERE A.FACTORY = 'VT' --DEFECT_CODE <> 'E000'
                                   AND (V_P_SUB_PLANT IS NULL OR A.PLANT_ID = V_P_SUB_PLANT) 
                                   AND A.AUDIT_DATE >= TO_CHAR(TRUNC(SYSDATE,'IW'),'YYYYMMDD')
                                 GROUP BY PLANT_ID, LINE_CODE
                            )
                        GROUP BY PLANT_ID, LINE_CODE
                        --ORDER BY DIF_RATE DESC
                     )
                )
                WHERE RN<=3
                ORDER BY W_1_DEFECT_RATE DESC
             ;
             
      V_P_ROW_COUNT := SQL%ROWCOUNT;

      IF V_P_ROW_COUNT > 0 THEN 
        V_P_ERROR_CODE := 'MSG0001';                                -- The inquiry was successfully
      ELSE 
        V_P_ERROR_CODE := 'MSG0006';                                -- There is no query that data.              
      END IF;

    END; -- The first return en 
    END IF; 
    ------------------------------------------------------------------------------------------------------    

    
    IF V_P_WORK_TYPE IN ( 'Q1' )  THEN
    BEGIN
    ----------------------------------------------------------------------------------------------------------------------- 
    
        V_P_MIN_DATE := CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN TO_CHAR(SYSDATE - 30,'YYYYMMDD')
                                           WHEN 'WEEKLY' THEN TO_CHAR(TRUNC(SYSDATE-9*7,'IW'),'YYYYMMDD')
                                           WHEN 'MONTHLY' THEN TO_CHAR(ADD_MONTHS(SYSDATE,-9),'YYYYMM') || '01'
                                           WHEN 'YEARLY' THEN TO_CHAR(ADD_MONTHS(SYSDATE,-108),'YYYY') || '0101'
                                           ELSE TO_CHAR(SYSDATE,'YYYYMMDD') END;
        V_P_TO_DAY :=TO_CHAR(SYSDATE,'YYYYMMDD');
        
      -- Run the query   
        OPEN CV_1 FOR
           WITH DATE_SEQ AS(
             SELECT SYS_DATE, 
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN ROWNUM
                       WHEN 'WEEKLY' THEN DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD'))
                       WHEN 'MONTHLY' THEN DENSE_RANK () OVER (ORDER BY SUBSTR(SYS_DATE,1,6))
                       WHEN 'YEARLY' THEN DENSE_RANK () OVER (ORDER BY PLAN_YEAR )
                       ELSE 0 END DATE_SEQ,
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN '('|| SUBSTR(SYS_DATE,5,2) ||'/' ||SUBSTR(SYS_DATE,-2) ||')'
                       WHEN 'WEEKLY' THEN 'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00')) 
                       --TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'MM/DD')
                       WHEN 'MONTHLY' THEN SUBSTR(SYS_DATE,1,4) ||'/' ||SUBSTR(SYS_DATE,5,2)
                       WHEN 'YEARLY' THEN SUBSTR(SYS_DATE,1,4)
                       ELSE ' ' END DATE_SHOW
               FROM MWIPCALDEF
              WHERE CALENDAR_ID = 'VT'
                AND SYS_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
              ORDER BY SYS_DATE
            )
             SELECT V_P_VIEW_TYPE VIEW_TYPE, DATE_SEQ, DATE_SHOW WORK_DATE, NVL(FCT_GRP, 'AVG') FCT,          --V_P_VIEW_TYPE='DAILY'        --=        --=
                    AVG(TOTAL_DEFECT_QTY) TOTAL_DEFECT, 100 - AVG(DEFECT_RATE) DEFECT_RATE,
                    GROUPING_ID(DATE_SEQ, DATE_SHOW, FCT_GRP) GRP_ID         --=        --=
               FROM (
                  SELECT B.DATE_SEQ, B.DATE_SHOW, A.FCT_GRP,
                         SUM(A.PROD_QTY) PROD_QTY, SUM(A.DEFECT_QTY_PRS) DEFECT_QTY_PRS, 
                         SUM(A.TOTAL_DEFECT_QTY) TOTAL_DEFECT_QTY, 
                         --ROUND(AVG(DEFECT_RATE), 1) DEFECT_RATE
                         ROUND( SUM(A.DEFECT_QTY_PRS) / SUM(A.AUDIT_QTY)*100 ,1) DEFECT_RATE
                    FROM MESMGR.CEDCHFPNIK A
                         INNER JOIN DATE_SEQ B ON A.AUDIT_DATE = B.SYS_DATE
                   WHERE A.FACTORY = 'VT'
                     AND A.HFPA_CMF_2 = '4402'
                GROUP BY A.FCT_GRP, B.DATE_SEQ, B.DATE_SHOW
                ) 
            GROUP BY ROLLUP((DATE_SEQ, DATE_SHOW), FCT_GRP)
            HAVING GROUPING_ID(DATE_SEQ, FCT_GRP) IN(0,1);
                         
            ----02--BY DEFECT TYPE --
        OPEN CV_2 FOR
          SELECT DEFECT_CODE ||'-'|| C.DATA_1  DEFECT_CODE, SUM(DEFECT_QTY) DEFECT_QTY    
            FROM (
                SELECT 'E001' DEFECT_CODE, NVL(SUM(E01), 0) DEFECT_QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND (V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT) 
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E002' DEFECT_CODE, NVL(SUM(E02), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E003' DEFECT_CODE, NVL(SUM(E03), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E004' DEFECT_CODE, NVL(SUM(E04), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E005' DEFECT_CODE, NVL(SUM(E05), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E006' DEFECT_CODE, NVL(SUM(E06), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E007' DEFECT_CODE, NVL(SUM(E07), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E008' DEFECT_CODE, NVL(SUM(E08), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E009' DEFECT_CODE, NVL(SUM(E09), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                UNION ALL
                SELECT 'E010' DEFECT_CODE, NVL(SUM(E10), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                UNION ALL
                SELECT 'E011' DEFECT_CODE, NVL(SUM(E11), 0) QTY
                  FROM MESMGR.CEDCHFPNIK A
                 WHERE FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T  
            ) A   
                INNER JOIN MGCMTBLDAT C ON  A.DEFECT_CODE = C.KEY_1 
            WHERE TABLE_NAME = 'CEDC_DEF_HFPA'
                  AND C.FACTORY = 'VT'
         GROUP BY DEFECT_CODE, C.DATA_1            
         ORDER BY 1
             ;
             
            --03--TOP 5 MODEL -- 
        OPEN CV_3 FOR
         WITH DATA_DEFECT AS(    
              SELECT *
                FROM (
                    SELECT MODEL_DESC, SUM(TOTAL_DEFECT_QTY) TOTAL_QTY, 
                           SUM(E01) E01, SUM(E02) E02, SUM(E03) E03, SUM(E04) E04, SUM(E05) E05, 
                           SUM(E06) E06, SUM(E07) E07, SUM(E08) E08, SUM(E09) E09, SUM(E10) E10, SUM(E11) E11
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND A.HFPA_CMF_2 = '4402'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT)) 
                       AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T  
                    GROUP BY MODEL_DESC
                    ORDER BY 2 DESC
                    )
                WHERE ROWNUM <= 5
               )       
        , DATA_ALL AS(
               SELECT DEFECT_CODE, C.DATA_1 DEFECT_NAME, MODEL_DESC, TOTAL_QTY, QTY, 
                      ROW_SEQ
               FROM (
                SELECT DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, SUM(QTY) QTY, 
                       ROW_NUMBER() OVER (PARTITION BY MODEL_DESC ORDER BY MODEL_DESC) AS ROW_SEQ
                 FROM (
                        SELECT 'E001' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E01), 0) QTY
                        FROM DATA_DEFECT
                        GROUP BY MODEL_DESC
                        UNION ALL
                        SELECT 'E002' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E02), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC 
                        UNION ALL
                        SELECT 'E003' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E03), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC 
                        UNION ALL
                        SELECT 'E004' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E04), 0) QTY
                        FROM DATA_DEFECT 
                           GROUP BY MODEL_DESC
                        UNION ALL
                        SELECT 'E005' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E05), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC
                        UNION ALL
                        SELECT 'E006' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E06), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC
                        UNION ALL
                        SELECT 'E007' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E07), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC 
                        UNION ALL
                        SELECT 'E008' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E08), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC 
                        UNION ALL
                        SELECT 'E009' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E09), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC
                        UNION ALL
                        SELECT 'E010' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E10), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC
                        UNION ALL
                        SELECT 'E011' DEFECT_CODE, MODEL_DESC, MAX(TOTAL_QTY) TOTAL_QTY, NVL(SUM(E11), 0) QTY
                        FROM DATA_DEFECT
                           GROUP BY MODEL_DESC 
                    ) 
                GROUP BY DEFECT_CODE, MODEL_DESC
                ORDER BY 2,3 DESC
                 ) A
                  INNER JOIN MGCMTBLDAT C ON  A.DEFECT_CODE = C.KEY_1 
                WHERE TABLE_NAME = 'CEDC_DEF_HFPA'
                      AND C.FACTORY = 'VT'
                      AND A.ROW_SEQ <=3
               ) 
           SELECT MODEL_DESC, TOTAL_QTY TOTAL_DEFECT,
                  LISTAGG(ROW_SEQ ||'.' ||DEFECT_NAME ||QTY ||'('|| ROUND((QTY/TOTAL_QTY)*100,2) || '%)', '|') WITHIN GROUP (ORDER BY MODEL_DESC DESC) COL_LIST
             FROM DATA_ALL
         GROUP BY MODEL_DESC, TOTAL_QTY
            ;
             
        -- 04--TOP 3 DEFECT 10 WEEEK --
        OPEN CV_4 FOR
          SELECT DATE_SHOW DATE_SHOW, MAX(DEFECT_CODE)DEFECT_CODE, DEFECT_QTY,
                 DEF_SEQ,TOTAL_DEFECT, DEFECT_PERCENT 
         FROM (         
           WITH DATE_SEQ AS(
                SELECT SYS_DATE,  'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00'))  DATE_SHOW
                  FROM MWIPCALDEF
                 WHERE CALENDAR_ID ='VT'
                   AND SYS_DATE BETWEEN  TO_CHAR(TRUNC(SYSDATE-9*7,'IW'),'YYYYMMDD') AND TO_CHAR(SYSDATE,'YYYYMMDD')
                 ORDER BY SYS_DATE
           ) ,
           DEFECT AS
           (
            SELECT AUDIT_DATE, DEFECT_CODE ||'-'|| C.DATA_1  DEFECT_CODE, SUM(DEFECT_QTY) DEFECT_QTY    
                FROM (
                    SELECT AUDIT_DATE,'E001' DEFECT_CODE, SUM(E01) DEFECT_QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                         AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                     GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E002' DEFECT_CODE, SUM(E02) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E003' DEFECT_CODE, SUM(E03) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E004' DEFECT_CODE, SUM(E04) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E005' DEFECT_CODE, SUM(E05) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E006' DEFECT_CODE, SUM(E06) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E007' DEFECT_CODE, SUM(E07) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                    GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E008' DEFECT_CODE, SUM(E08) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E009' DEFECT_CODE, SUM(E09) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E010' DEFECT_CODE, SUM(E10) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                   GROUP BY AUDIT_DATE
                    UNION ALL
                    SELECT AUDIT_DATE,'E011' DEFECT_CODE, SUM(E11) QTY
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
--                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T  
                   GROUP BY AUDIT_DATE
                    ) A   
                    INNER JOIN MGCMTBLDAT C ON  A.DEFECT_CODE = C.KEY_1 
                WHERE TABLE_NAME = 'CEDC_DEF_HFPA'
                      AND C.FACTORY = 'VT'
             GROUP BY AUDIT_DATE, DEFECT_CODE, C.DATA_1  
           )
            SELECT DATE_SHOW, DEFECT_CODE, DEFECT_QTY ,DEF_SEQ, TOTAL_DEFECT,
                 ROUND((DEFECT_QTY /TOTAL_DEFECT)*100, 2) DEFECT_PERCENT
            FROM (
                    SELECT DATE_SHOW, DEFECT_CODE  DEFECT_CODE, DEFECT_QTY
                           , DENSE_RANK () OVER (PARTITION BY DATE_SHOW ORDER BY DEFECT_QTY DESC) DEF_SEQ
                           , ROUND(SUM(DEFECT_QTY) OVER(ORDER BY DATE_SHOW),2) TOTAL_DEFECT
                    FROM (                           
                           SELECT DATE_SHOW, DEFECT_CODE,
                                  SUM(CASE WHEN DEFECT_CODE<>'E000' THEN NVL(DEFECT_QTY,0) ELSE 0 END)  DEFECT_QTY 
                             FROM DEFECT A  
                                  INNER JOIN DATE_SEQ C ON A.AUDIT_DATE = C.SYS_DATE
                            GROUP BY DATE_SHOW, DEFECT_CODE                                                                                    
                      )
              ) 
            WHERE DEF_SEQ <= 3
            )
            GROUP BY DATE_SHOW, DEFECT_QTY, DEF_SEQ, TOTAL_DEFECT, DEFECT_PERCENT
            ORDER BY DATE_SHOW, DEF_SEQ, DEFECT_CODE
             ;
            
        -----05--BY DEFECT HOURS pass rate -- % PAS RATE BY PLANT -- 
        OPEN CV_5 FOR     
           SELECT 'AVG' FCT, DECODE(NVL(V_P_SUB_PLANT,''), '', SUB_PLANT, LINE_CODE) WORK_DATE,
                   SUM(DEFECT_QTY_PRS) DEFECT_QTY, 
                  100 - ROUND(SUM(DEFECT_QTY_PRS)/SUM(AUDIT_QTY)*100, 1) DEFECT_RATE
             FROM MESMGR.CEDCHFPNIK A
            WHERE FACTORY = 'VT'
              AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
              AND A.HFPA_CMF_2 = '4402'
              AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
         GROUP BY DECODE(NVL(V_P_SUB_PLANT,''), '', SUB_PLANT, LINE_CODE) 
         ORDER BY 2
        ;
             
        --------06--TOP 3 PLANT BY DEFECT RATE -----------
        OPEN CV_6 FOR
            WITH DATE_SEQ AS(
                SELECT SYS_DATE, 
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN ROWNUM
                       WHEN 'WEEKLY' THEN DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD'))
                       WHEN 'MONTHLY' THEN DENSE_RANK () OVER (ORDER BY SUBSTR(SYS_DATE,1,6))
                       WHEN 'YEARLY' THEN DENSE_RANK () OVER (ORDER BY PLAN_YEAR )
                       ELSE 0 END DATE_SEQ,
                    CASE V_P_VIEW_TYPE WHEN 'DAILY' THEN SUBSTR(SYS_DATE,5,2) ||'/' ||SUBSTR(SYS_DATE,-2)
                       WHEN 'WEEKLY' THEN 'W'|| TRIM(TO_CHAR(DENSE_RANK () OVER (ORDER BY TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'YYYYMMDD')),'00')) --TO_CHAR(TRUNC(TO_DATE(SYS_DATE,'YYYYMMDD'),'IW'),'MM/DD')
                       WHEN 'MONTHLY' THEN SUBSTR(SYS_DATE,1,4) ||'/' ||SUBSTR(SYS_DATE,5,2)
                       WHEN 'YEARLY' THEN SUBSTR(SYS_DATE,1,4)
                       ELSE ' ' END DATE_SHOW
                FROM MWIPCALDEF
               WHERE CALENDAR_ID = 'VT'
--                AND SYS_DATE BETWEEN V_P_MIN_DATE  AND V_P_TO_DAY
                 AND SYS_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
               ORDER BY SYS_DATE
            ) 
           SELECT * 
            FROM (
                SELECT V_P_VIEW_TYPE VIEW_TYPE, 
                       DECODE(NVL(V_P_SUB_PLANT,''), '', A.SUB_PLANT, A.LINE_CODE) PLANT_ID, 
                       SUM(A.AUDIT_QTY) AUDIT_SIZE_CD,
                       SUM(A.DEFECT_QTY_PRS) DEFECTIVE_QTY,
                       ROUND(SUM(A.DEFECT_QTY_PRS)/SUM(A.AUDIT_QTY)*100, 1) DEFECT_RATE
--                       ROUND(AVG(A.DEFECT_RATE), 1) DEFECT_RATE
                  FROM MESMGR.CEDCHFPNIK A, DATE_SEQ B
                 WHERE A.FACTORY = 'VT'
                   AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                   AND A.HFPA_CMF_2 = '4402'
                   AND A.AUDIT_DATE = B.SYS_DATE 
              GROUP BY A.SUB_PLANT, A.LINE_CODE
              ORDER BY DEFECT_RATE DESC
              )
            WHERE ROWNUM <=3
                ;
             
            -- 07--TOP 5 DEFECT RATE BY TYPE-- 
        OPEN CV_7 FOR
        
             WITH DATA_ALL AS(                
                    SELECT DEFECT_CODE ||'-'|| C.DATA_1 DEFECT_CODE, 
                           SUM(AUDIT_SIZE_CD), SUM(DEFECTIVE_QTY), 
                           SUM(DEFECT_QTY_TOTAL), 
                           ROUND(SUM(DEFECT_QTY_TOTAL)/SUM(AUDIT_SIZE_CD)*100, 1) DEFECT_RATE
--                           ROUND(AVG(DEFECT_RATE), 1) DEFECT_RATE
                    FROM (
                        SELECT DEFECT_CODE, AUDIT_SIZE_CD, DEFECTIVE_QTY, DEFECT_QTY_TOTAL, DEFECT_RATE
                          FROM (
                             SELECT 'E001' DEFECT_CODE,  
                                   CASE WHEN E01 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E01 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E01 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E01 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                            UNION ALL
                            SELECT 'E002' DEFECT_CODE,  
                                   CASE WHEN E02 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E02 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E02 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E02 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E003' DEFECT_CODE,  
                                   CASE WHEN E03 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E03 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E03 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E03 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                            UNION ALL
                            SELECT 'E004' DEFECT_CODE,  
                                   CASE WHEN E04 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E04 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E04 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E04 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E005' DEFECT_CODE,  
                                   CASE WHEN E05 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E05 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E05 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E05 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E006' DEFECT_CODE,  
                                   CASE WHEN E06 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E06 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E06 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E06 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E007' DEFECT_CODE,  
                                   CASE WHEN E07 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E07 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E07 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E07 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E008' DEFECT_CODE,  
                                   CASE WHEN E08 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E08 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E08 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E08 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E009' DEFECT_CODE,  
                                   CASE WHEN E09 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E09 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E09 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E09 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E010' DEFECT_CODE,  
                                   CASE WHEN E10 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E10 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E10 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E10 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T 
                             UNION ALL
                            SELECT 'E011' DEFECT_CODE,  
                                   CASE WHEN E11 <> 0 THEN AUDIT_QTY ELSE 0 END AUDIT_SIZE_CD,
                                   CASE WHEN E11 <> 0 THEN DEFECT_QTY_PRS ELSE 0 END DEFECTIVE_QTY,
                                   CASE WHEN E11 <> 0 THEN TOTAL_DEFECT_QTY ELSE 0 END DEFECT_QTY_TOTAL,
                                   CASE WHEN E11 <> 0 THEN DEFECT_RATE ELSE '0' END DEFECT_RATE
                              FROM MESMGR.CEDCHFPNIK A
                             WHERE FACTORY = 'VT'
                               AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                               AND A.HFPA_CMF_2 = '4402'
                               AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T      
                         )
                       WHERE AUDIT_SIZE_CD <>0
                    ) A   
                        INNER JOIN MGCMTBLDAT C ON  A.DEFECT_CODE = C.KEY_1 
                    WHERE C.TABLE_NAME = 'CEDC_DEF_HFPA'
                          AND C.FACTORY = 'VT'
                 GROUP BY DEFECT_CODE, C.DATA_1 
                 ORDER BY 1
                 )
             SELECT *
             FROM DATA_ALL
             WHERE ROWNUM <= 5 
            ;
             
         ---------08--TOP 3 LINE CHANGE RATE HIGHTEST --
        OPEN CV_8 FOR
            SELECT PLANT_ID, LINE_CODE, SUM(W_1_DEFECT_RATE) W_1_DEFECT_RATE, 
                   SUM(W_DEFECT_RATE) W_DEFECT_RATE, AVG(DIF_DATE) DIF_DATE,
                   ROW_NUMBER() OVER (ORDER BY PLANT_ID, LINE_CODE DESC) AS RN 
            FROM (   
                    SELECT SUB_PLANT PLANT_ID, LINE_CODE,       
                           ROUND(SUM(DEFECT_QTY_PRS)/SUM(AUDIT_QTY)*100,2) W_1_DEFECT_RATE,      
                           0 W_DEFECT_RATE, 
                           ROUND(AVG(DEFECT_RATE), 1) DIF_DATE
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
                       AND AUDIT_DATE BETWEEN TO_CHAR(TRUNC(SYSDATE-7,'IW'),'YYYYMMDD') AND TO_CHAR(TRUNC(SYSDATE-7,'IW')+6,'YYYYMMDD')
                    GROUP BY SUB_PLANT, LINE_CODE         
                    UNION ALL
                    SELECT SUB_PLANT PLANT_ID, LINE_CODE,       
                           0 W_1_DEFECT_RATE, 
                           DECODE(SUM(AUDIT_QTY), 0, 0, ROUND(SUM(DEFECT_QTY_PRS)/SUM(AUDIT_QTY)*100,2)) W_DEFECT_RATE,     
                           ROUND(AVG(DEFECT_RATE), 1) DIF_DATE
                      FROM MESMGR.CEDCHFPNIK A
                     WHERE FACTORY = 'VT'
                       AND ((V_P_SUB_PLANT IS NULL OR 'PLANT_'||SUB_PLANT = V_P_SUB_PLANT))
                       AND A.HFPA_CMF_2 = '4402'
                       AND AUDIT_DATE >= TO_CHAR(TRUNC(SYSDATE,'IW'),'YYYYMMDD')
--                       AND AUDIT_DATE BETWEEN V_P_DATE_F AND V_P_DATE_T
                    GROUP BY SUB_PLANT, LINE_CODE
            --        ORDER BY 1, 2   
            )
            GROUP BY PLANT_ID, LINE_CODE
            ORDER BY 4 DESC
             ;
             
      V_P_ROW_COUNT := SQL%ROWCOUNT;

      IF V_P_ROW_COUNT > 0 THEN 
        V_P_ERROR_CODE := 'MSG0001';                                -- The inquiry was successfully
      ELSE 
        V_P_ERROR_CODE := 'MSG0006';                                -- There is no query that data.              
      END IF;

    END; -- The first return en 
    END IF; 
    ------------------------------------------------------------------------------------------------------ 
     
    ------------------------------------------------------------------------------------------------------         
    EXCEPTION WHEN OTHERS THEN
    BEGIN
      V_P_ERROR_CODE := CASE SUBSTR(V_P_WORK_TYPE,1,1)
                             WHEN 'Q' THEN 'E000006'                 -- When a query error occurred. 
                             WHEN 'N' THEN 'E000008'                 -- The registration error occurred.
                             WHEN 'U' THEN 'E000009'                 -- The modified when the error occurred.
                             WHEN 'D' THEN 'E000010'                 -- An error occurred while deleting.
                                      ELSE 'ERR0000'
                        END;
      V_ERRORSTATE := CAST(SQLCODE AS VARCHAR2) || '|' || CAST(NULL AS VARCHAR2) || '|' || CAST(NULL AS VARCHAR2) || '|' || CAST(NULL AS VARCHAR2);
      V_ERRORPROCEDURE := 'PRC=' || CAST(NULL AS VARCHAR2);
      V_P_ERROR_STR := SQLERRM;
    END;

END;
/