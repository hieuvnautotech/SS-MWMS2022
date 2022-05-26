--call insert_lot_tam 
CREATE DEFINER=`shinsung`@`%` PROCEDURE `insert_lot_tam`(
	IN `mt_no` VARCHAR(200),
	IN `gr_qty` INT,
	IN `expore_dt` VARCHAR(14),
	IN `dt_of_receipt` VARCHAR(14),
	IN `exp_input_dt` VARCHAR(14),
	IN `lot_no` VARCHAR(200),
	IN `mt_type` VARCHAR(3),
	IN `UserID` VARCHAR(50),
	IN `_barcode` CHAR(1),
	IN `send_qty` INT,
	IN `today` VARCHAR(10),
	IN `time_now` VARCHAR(10)



)
LANGUAGE SQL
NOT DETERMINISTIC
CONTAINS SQL
SQL SECURITY DEFINER
COMMENT ''
BEGIN
DECLARE bien_first VARCHAR(500)  ;
DECLARE COUNT INT(20) DEFAULT 0;
DECLARE cut_duoi_lot INT(20) DEFAULT 0;
DECLARE v_mt_cd VARCHAR(500)  ;
SET bien_first=CONCAT(mt_no, '-CP-', today, time_now);

	 WHILE (COUNT  < send_qty) DO
			SET COUNT =COUNT +1;
	
	if (SELECT COUNT(*) FROM w_material_info_tam WHERE mt_cd LIKE CONCAT(bien_first,'%'))>0 then 
		SET cut_duoi_lot=(SELECT cast(substring(mt_cd, LENGTH(mt_cd)-5, 7) AS SIGNED ) FROM w_material_info_tam WHERE mt_cd LIKE CONCAT(bien_first,'%') ORDER BY cast(substring(mt_cd, LENGTH(mt_cd)-5, 7) AS SIGNED ) DESC LIMIT 1);
		SET cut_duoi_lot=cut_duoi_lot+1;
	ELSE 
		SET cut_duoi_lot=COUNT;
		
	END if;
	
		SET v_mt_cd = Func_Create_ID_6c(bien_first,cut_duoi_lot);
	
	if _barcode="Y" then 
		
INSERT INTO `w_material_info_tam` (`mt_type`, `mt_cd`, `mt_no`, `gr_qty`, `date`, `expiry_dt`, `dt_of_receipt`, `expore_dt`, `lot_no`, `mt_barcode`, `mt_qrcode`, `mt_sts_cd`, `use_yn`, `reg_id`, `reg_dt`, `chg_id`, `chg_dt`,`real_qty`) 
VALUES 
	(mt_type, v_mt_cd, mt_no,gr_qty,(SELECT REPLACE(REPLACE(REPLACE(NOW(),':',''),' ',''),'-','')),exp_input_dt,dt_of_receipt,expore_dt,lot_no,v_mt_cd,v_mt_cd,'000','Y',UserID,(SELECT NOW()),UserID,(SELECT NOW()),gr_qty);
	 
	 else
	 INSERT INTO `w_material_info_tam` (`mt_type`, `mt_cd`, `mt_no`, `gr_qty`, `date`, `expiry_dt`, `dt_of_receipt`, `expore_dt`, `lot_no`, `mt_barcode`, `mt_qrcode`, `mt_sts_cd`, `use_yn`, `reg_id`, `reg_dt`, `chg_id`, `chg_dt`,`real_qty`) 
VALUES 
	(mt_type, v_mt_cd, mt_no,gr_qty,(SELECT REPLACE(REPLACE(REPLACE(NOW(),':',''),' ',''),'-','')),exp_input_dt,dt_of_receipt,expore_dt,lot_no,null,null,'000','Y',UserID,(SELECT NOW()),UserID,(SELECT NOW()),gr_qty);
	 
	 END if;
	 END while;

END