ALTER TABLE `d_bom_info`
	ADD COLUMN `IsApply` CHAR(1) NOT NULL DEFAULT 'N' AFTER `del_yn`;
	
	
	materialbom;
	ALTER TABLE materialbom
ADD  UNIQUE (ProductCode,MaterialPrarent,MaterialNo);

ALTER TABLE `w_actual_primary`
	ADD COLUMN `IsApply` CHAR(1) NOT NULL DEFAULT 'N' AFTER `finish_yn`;
	
	GetData_wActual_Primary, any_value(a.IsApply) AS IsApply
	viewactual_primary



//ngày 23

tao sp: GetListMaterialForBom
view process_last_time : thêm `a`.`product` AS `product`,`a`.`actual` AS `actual`,