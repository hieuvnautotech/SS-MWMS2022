ALTER TABLE materialbomADD COLUMN IsActive BIT NOT NULL DEFAULT '0' ;
CALL spFGWMMS_GetFGGeneralDetail('', '', '', '', '');
ALTER TABLE materialbom	ADD COLUMN ProductCode VARCHAR(12) NOT NULL ;
INSERT INTO materialbom
(MaterialID, BOMID,ProductCode,IsActive,CreateOn)

SELECT b.mtid, a.bid, a.style_no,1, NOW()
FROM d_bom_info AS a
JOIN d_material_info  AS b
ON a.mt_no = b.mt_no;


ALTER TABLE d_bom_info
	DROP COLUMN drawingName,
	DROP COLUMN description,
	DROP COLUMN cuttingSize;



ALTER TABLE d_bom_info
	ADD COLUMN IsActive BIT(1) NOT NULL DEFAULT b'0' AFTER del_yn;
	
CREATE TABLE shippingsdmaterial (
	id INT NULL,
	sd_no VARCHAR(100) NULL DEFAULT NULL,
	mt_no VARCHAR(300) NULL DEFAULT NULL,
	quantity DOUBLE NULL DEFAULT NULL,
	meter DOUBLE NULL DEFAULT NULL
)
COLLATE='utf8mb4_0900_ai_ci'
;

ALTER TABLE shippingsdmaterial
	ADD COLUMN reg_id VARCHAR(20) NULL DEFAULT NULL AFTER meter,
	ADD COLUMN reg_dt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP AFTER reg_id;
	
	
INSERT INTO shippingsdmaterial (sd_no,mt_no,quantity,meter,reg_id,reg_dt)

/*GetPickingScanPP_Count_MT_no*/
 SELECT abc.sd_no,abc.mt_no,COUNT(abc.mt_cd),sum(abc.gr_qty),'root',NOW()
FROM 
( 
	SELECT a.mt_no, a.mt_cd , a.mt_sts_cd,a.wmtid, a.gr_qty, a.sd_no
	FROM w_material_info_tam AS a 
	WHERE  a.lct_cd LIKE '002%' 
	UNION ALL 
	SELECT b.mt_no, b.mt_cd, b.mt_sts_cd ,b.wmtid, b.gr_qty, b.sd_no
	FROM w_material_info AS b 
	WHERE  b.lct_cd LIKE '002%'
) AS abc
 left JOIN w_material_info info ON info.wmtid = abc.wmtid
 WHERE (abc.sd_no != NULL or abc.sd_no != '')
GROUP BY abc.sd_no ;	
/*GetPickingScanPP_Count_MT_no*/



----create SP SPGetActualInfo
create PROCEDURE SPGetActualInfo(IN _at_no VARCHAR (300),
	IN _date VARCHAR (300),
	IN _processCode VARCHAR (300))
BEGIN
SELECT
		a.id_actual,
		a.type,
		a.name,
		a.date,
		a.item_vcd,
		( SELECT item_nm FROM qc_item_mt WHERE item_vcd = a.item_vcd LIMIT 1 ) AS QCName,
		a.defect,
		ifnull( a.actual, 0 ) actual,	bang3.actual_cn,bang3.actual_cd,(
		SELECT
			dt_nm 
		FROM
			comm_dt 
		WHERE
			dt_cd = a.don_vi_pr 
			AND mt_cd = 'COM032' 
			LIMIT 1 
		) AS RollName,
		a.reg_id,
		a.reg_dt,
		a.chg_id,
		a.chg_dt,
		b.target,
		a.level,
		bang2.mc_no 
	FROM
		w_actual a
		JOIN w_actual_primary b ON a.at_no = b.at_no
		LEFT JOIN (
		SELECT  
			TABLE1.NAME,
			TABLE1.mc_no,
			TABLE1.reg_dt,
			TABLE1.id_actual 
		FROM
			(
			SELECT
				mc.mc_no,
				ac.NAME,
				mc.reg_dt,
				ac.id_actual 
			FROM
				d_pro_unit_mc AS mc
				JOIN w_actual AS ac ON mc.id_actual = ac.id_actual 
			WHERE
				ac.id_actual IN ( SELECT actual.id_actual FROM w_actual actual JOIN w_actual_primary actualPrimary ON actual.at_no = actualPrimary.at_no WHERE actual.at_no = _at_no ) 
			) AS TABLE1
			RIGHT JOIN (
			SELECT
				MAX( mc1.reg_dt ) AS reg_dt,
				actual1.NAME 
			FROM
				d_pro_unit_mc AS mc1
				JOIN w_actual AS actual1 ON mc1.id_actual = actual1.id_actual 
			WHERE
				actual1.id_actual IN ( SELECT actual.id_actual FROM w_actual actual JOIN w_actual_primary actualPrimary ON actual.at_no = actualPrimary.at_no WHERE actual.at_no = _at_no ) 
			GROUP BY
				actual1.id_actual 
			) AS TABLE2 ON TABLE1.NAME = TABLE2.NAME 
			AND TABLE1.reg_dt = TABLE2.reg_dt 
		) AS bang2 ON a.id_actual = bang2.id_actual
LEFT JOIN(
Select 
	p.id_actual,
	max(case when p.reg_dt = 'CN' then p.sl_tru_ng end) AS actual_cn,
	max(case when p.reg_dt = 'CD' then p.sl_tru_ng end) AS actual_cd
FROM
(SELECT
	a.id_actual,

	(CASE 
	WHEN ('08:00:00' <= DATE_FORMAT( CAST( a.reg_dt AS datetime ),'%H:%i:%s') AND   DATE_FORMAT( CAST( a.reg_dt AS datetime ),'%H:%i:%s')  <=  '20:00:00') THEN
		'CN'
	ELSE
		'CD'
END)  as reg_dt,

	
SUM(IFNULL( a.real_qty - d.sltrung, a.real_qty )) sl_tru_ng
FROM
	w_material_info AS a
	LEFT JOIN comm_dt b ON b.dt_cd = a.bbmp_sts_cd 
	AND b.mt_cd = 'MMS007'
	LEFT JOIN ( SELECT ( check_qty - ok_qty ) AS sltrung, ml_no FROM m_facline_qc WHERE ml_tims IS NULL ) d ON d.ml_no = a.mt_cd 
WHERE a.mt_type = 'CMT' 
	AND a.orgin_mt_cd IS NULL 
GROUP BY id_actual,reg_dt) p
group by p.id_actual) bang3 ON a.id_actual = bang3.id_actual  	
	WHERE
		a.at_no = _at_no 
		AND a.type = 'SX' 
	AND
	CASE
			
			WHEN _date = '' THEN
			'' = '' ELSE a.date = _date 
		END 
			AND a.name LIKE CONCAT( '%', _processCode, '%' );
END
----modify sp GetData_wActual_Primary
CREATE PROCEDURE GetData_wActual_Primary(
	IN _product VARCHAR(200),
	IN _productname VARCHAR(200),
	IN _model VARCHAR(200),
	IN _AtNo varchar(50),
	IN _RegStart varchar(30),
	IN _RegEnd varchar(30),
	IN _type VARCHAR(4)
)
BEGIN
if _type !='MMS' then 
SELECT max(a.id_actualpr) AS id_actualpr
		, max(a.at_no) AS at_no
		, max(a.type) AS type
		, SUM(a.target) AS totalTarget
		, max(a.target) AS target
		, max(a.product) AS product
		, max(a.md_cd) AS md_cd
		, max(a.remark) AS remark
		, max(a.style_nm) AS style_nm
		, max(a.process_count) AS process_count
		, sum(a.actual) AS actual
		, any_value(a.count_pr_w) AS count_pr_w
		, subquery.at_no AS poRun
	FROM viewactual_primary a
		left JOIN 
	(
		SELECT  b.at_no
		FROM d_pro_unit_staff AS a
		JOIN w_actual AS b
		ON a.id_actual = b.id_actual
		JOIN w_actual_primary AS c
		ON b.at_no = c.at_no
		WHERE 
			 (NOW() BETWEEN DATE_FORMAT(a.start_dt,'%Y-%m-%d %H:%i:%s') AND  DATE_FORMAT(a.end_dt,'%Y-%m-%d %H:%i:%s')) AND 
				c.finish_yn IN  ('N','Y')   AND b.type ='TIMS'
			 GROUP BY b.at_no
	) AS subquery 
	ON subquery.at_no = a.at_no
	WHERE (_AtNo='' OR a.at_no LIKE CONCAT('%',_AtNo,'%'))
	AND (_product='' OR  a.product LIKE CONCAT('%',_product,'%'))
	AND (_productname='' OR  a.style_nm LIKE CONCAT('%',_productname,'%'))
	AND (_model='' OR  a.md_cd LIKE CONCAT('%',_model,'%'))
	AND ((_RegStart='' AND  _RegEnd='') OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( _RegStart, '%Y/%m/%d' ) AND DATE_FORMAT( _RegEnd, '%Y/%m/%d' )))
	AND a.finish_yn IN  ('N','Y')
	GROUP BY a.at_no
	;
ELSE 
SELECT max(a.id_actualpr) AS id_actualpr
		, max(a.at_no) AS at_no
		, max(a.type) AS type
		, SUM(a.target) AS totalTarget
		, max(a.target) AS target
		, max(a.product) AS product
		, max(a.md_cd) AS md_cd
		, max(a.remark) AS remark
		, max(a.style_nm) AS style_nm
		, max(a.process_count) AS process_count
		, sum(a.actual) AS actual
		, any_value(a.count_pr_w) AS count_pr_w
		, subquery.at_no AS poRun
	FROM viewactual_primary a
			left JOIN 
	(
		SELECT  b.at_no
		FROM d_pro_unit_staff AS a
		JOIN w_actual AS b
		ON a.id_actual = b.id_actual
		JOIN w_actual_primary AS c
		ON b.at_no = c.at_no
		WHERE 
			 (NOW() BETWEEN DATE_FORMAT(a.start_dt,'%Y-%m-%d %H:%i:%s') AND  DATE_FORMAT(a.end_dt,'%Y-%m-%d %H:%i:%s')) AND 
			 c.finish_yn ='N'	  AND b.type ='SX'
			 GROUP BY b.at_no
	) AS subquery 
	ON subquery.at_no = a.at_no
	WHERE (_AtNo='' OR a.at_no LIKE CONCAT('%',_AtNo,'%'))
	AND (_product='' OR  a.product LIKE CONCAT('%',_product,'%'))
	AND (_productname='' OR  a.style_nm LIKE CONCAT('%',_productname,'%'))
	AND (_model='' OR  a.md_cd LIKE CONCAT('%',_model,'%'))
	AND ((_RegStart='' AND  _RegEnd='') OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( _RegStart, '%Y/%m/%d' ) AND DATE_FORMAT( _RegEnd, '%Y/%m/%d' )))
	AND a.finish_yn IN  ('N','Y')
	AND a.finish_yn IN  ('N','YT')
	GROUP BY a.at_no
	;

END if;
END


----create sp GetAllFinishProducts
CREATE PROCEDURE GetAllFinishProducts(IN _product VARCHAR(200),
	IN _productname VARCHAR(200),
	IN _model VARCHAR(200),
	IN _AtNo varchar(50),
	IN _RegStart varchar(30),
	IN _RegEnd varchar(30),
	IN _type VARCHAR(4))
BEGIN
	if _type!= "MMS" then
  SELECT max(a.id_actualpr) AS id_actualpr
		, max(a.at_no) AS at_no
		, max(a.type) AS type
		, SUM(a.target) AS totalTarget

		, max(a.target) AS target
		, max(a.product) AS product
		, max(a.md_cd) AS md_cd
		, max(a.remark) AS remark

		, max(a.style_nm) AS style_nm
		, max(a.process_count) AS process_count
		, SUM(a.actual) AS actual
		, any_value(a.count_pr_w) AS count_pr_w
	FROM viewactual_primary a
	WHERE (_AtNo='' OR a.at_no LIKE CONCAT('%',_AtNo,'%'))
	AND (_productname='' OR a.style_nm LIKE CONCAT('%',_productname,'%'))
	AND (_model='' OR a.md_cd LIKE CONCAT('%',_model,'%'))
	AND (_product='' OR  a.product LIKE CONCAT('%',_product,'%'))
	AND ((_RegStart='' AND _RegEnd='' ) OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( _RegStart, '%Y/%m/%d' ) AND DATE_FORMAT( _RegEnd, '%Y/%m/%d' )))
	AND a.finish_yn IN ('YT')
	GROUP BY a.at_no;
	ELSE 
	
	  SELECT max(a.id_actualpr) AS id_actualpr
		, max(a.at_no) AS at_no
		, max(a.type) AS type
		, SUM(a.target) AS totalTarget
		, max(a.target) AS target
		, max(a.product) AS product
		, max(a.md_cd) AS md_cd
		, max(a.remark) AS remark
		, max(a.style_nm) AS style_nm
		, max(a.process_count) AS process_count
		, SUM(a.actual) AS actual
		, any_value(a.count_pr_w) AS count_pr_w
	FROM viewactual_primary a
	WHERE (_AtNo='' OR a.at_no LIKE CONCAT('%',_AtNo,'%'))
	AND (_productname='' OR a.style_nm LIKE CONCAT('%',_productname,'%'))
	AND (_model='' OR a.md_cd LIKE CONCAT('%',_model,'%'))
	AND (_product='' OR  a.product LIKE CONCAT('%',_product,'%'))
	AND ((_RegStart='' AND _RegEnd='' ) OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( _RegStart, '%Y/%m/%d' ) AND DATE_FORMAT( _RegEnd, '%Y/%m/%d' )))
	AND a.finish_yn IN ('Y')
	GROUP BY a.at_no;
	
	END if;
END


--INSERT SP Lot_PO_custom
CREATE PROCEDURE `Lot_PO_custom`(IN `_po` VARCHAR(50))
BEGIN

DROP  TABLE IF EXISTS tempResult;
DROP  TABLE IF EXISTS tableQuantity;

CREATE TEMPORARY TABLE tempResult 
    SELECT a.mt_cd,a.mt_no,c.dt_nm AS process,
    GROUP_CONCAT(CONCAT('<b>', l.mc_no, '</b>','<br> <i>Start: ',CAST(l.start_dt AS DATETIME),'<br> End: ',CAST( l.end_dt AS DATETIME)) SEPARATOR ' <br> </i>') AS machine,
	GROUP_CONCAT(concat('<b>', k.uname, '</b>', '<i> <br> Start: ',CAST(k.start_dt AS DATETIME), ' <br> End: ', CAST(k.end_dt AS DATETIME)) SEPARATOR ' <br> </i>') AS congnhan_time,
	(CONCAT(q.width, 'MM x ', q.spec, 'M')) size,
            q.mt_nm,
            a.`expiry_dt`,
            a.`dt_of_receipt`,
            a.`expore_dt`,
            a.`lot_no`,
            a.gr_qty SLLD
	FROM w_material_info AS a
	JOIN w_actual		 AS b ON a.id_actual=b.id_actual
	JOIN comm_dt 		 AS c ON c.dt_cd=b.name 			AND c.mt_cd ='COM007'
	JOIN d_pro_unit_mc AS l ON l.id_actual=a.id_actual 
	JOIN 
		(
			SELECT k.start_dt,k.end_dt,k.id_actual,n.uname
			FROM d_pro_unit_staff AS k JOIN mb_info			 AS n ON n.userid=k.staff_id
		) AS k ON k.id_actual = a.id_actual
	LEFT JOIN d_material_info AS q ON q.mt_no =a.mt_no
	WHERE a.at_no=_po AND a.mt_type!="CMT"
	GROUP BY a.`mt_cd`
    ORDER BY b.`reg_dt` desc;

    CREATE TEMPORARY TABLE tableQuantity 
        SELECT `mt_no`, Sum(`SLLD`) AS quantity FROM tempResult t2 GROUP BY `mt_no`;

    SELECT  t.mt_cd,
            Concat( '<div style="text-align:center;">',t.`mt_no`, '<br>(', t2.quantity, ')', '</div>' ) as `mt_no`,
            t.process,
            t.machine,
            t.congnhan_time,
            t.size,
            t.mt_nm,
            t.`expiry_dt`,
            t.`dt_of_receipt`,
            t.`expore_dt`,
            t.`lot_no`,
            t.SLLD
    FROM tempResult t
    JOIN 
    tableQuantity t2 on t.`mt_no` = t2.`mt_no`;
    

    
    DROP table tempResult;
    DROP table tableQuantity;
END

----create sp ss_qms_ng
BEGIN
	SELECT DISTINCT a.product_cd AS ProductCode,a.shift AS Shift,SUM(a.check_qty) AS Total,SUM(b.check_qty) AS OK,SUM(a.check_qty-b.check_qty) 			as NG,b.date_ymd AS CreateOn
	FROM m_facline_qc a
	JOIN m_facline_qc_value b ON a.fq_no=b.fq_no
	JOIN w_material_info c ON c.product=a.product_cd
	WHERE MONTH(CAST(b.date_ymd AS DATE))=MONTH(date_ymd) AND YEAR(CAST(b.date_ymd AS DATE))=YEAR(date_ymd) AND a.product_cd=productCode
	GROUP BY a.product_cd,a.shift,b.date_ymd
	ORDER BY b.date_ymd;
END

----modify sp GetPickingScanPP_Count_MT_no
----modify sp sp_insertproduct_excel