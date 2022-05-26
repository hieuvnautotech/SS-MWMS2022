/**[author_action]*/

truncate table [ShinsungReal].[dbo].[author_action];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[author_action] ON;
INSERT INTO [ShinsungReal].[dbo].[author_action]([mn_cd]
           ,[url_link]
           ,[id_button]
           ,[type]
           ,[name_table]
           ,[sts_action]
           ,[re_mark]
           ,create_id
           ,[create_date]
           ,[change_id]
           ,[change_date])
Select 
	mn_cd,
        url_link,
        id_button,
        type,
        name_table,
        sts_action,
        re_mark,
        'admin',
        getdate(),
        'admin',
        getdate()
From [ShinsungTest].[autodb-2129].[author_action]
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[author_action] OFF;
/*[author_info]*/

truncate table [ShinsungReal].[dbo].[author_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[author_info] ON;
INSERT INTO [ShinsungReal].[dbo].[author_info]
           ([at_cd]
           ,[at_nm]
           ,[role]
           ,[use_yn]
           ,reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[re_mark])
Select 
	at_cd,
        at_nm,
        role,
        use_yn,
        (case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id,
        reg_dt,
        chg_id,
        chg_dt,
        re_mark
From [ShinsungTest].[autodb-2129].[author_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[author_info] OFF;


/*[author_menu_info]*/
truncate table [ShinsungReal].[dbo].[author_menu_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[author_menu_info] ON;
INSERT INTO [ShinsungReal].[dbo].[author_menu_info]
           ([at_cd]
           ,[mn_cd]
           ,[mn_nm]
           ,[url_link]
           ,[re_mark]
           ,[use_yn]
           ,[st_yn]
           ,[ct_yn]
           ,[mt_yn]
           ,[del_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
		   ,[role]
           ,[nameen]
           ,[namevi]
           ,[namekr]
		   
		   )
Select 
	 at_cd
      ,mn_cd
      ,mn_nm
      ,url_link
      ,re_mark
      ,use_yn
      ,st_yn
      ,ct_yn
      ,mt_yn
      ,del_yn
      ,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id
      ,reg_dt
      ,chg_id
      ,chg_dt
	  ,''
	  ,nameen
	  ,namevi
	  ,namekr
From [ShinsungTest].[autodb-2129].[author_menu_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[author_menu_info] OFF;
/*[buyer_info]*/

truncate table [ShinsungReal].[dbo].[buyer_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[buyer_info] ON;
INSERT INTO [ShinsungReal].[dbo].[buyer_info]
           ([buyer_cd]
           ,[buyer_nm]
           ,[ceo_nm]
           ,[manager_nm]
           ,[brd_nm]
           ,[logo]
           ,[phone_nb]
           ,[cell_nb]
           ,[fax_nb]
           ,[e_mail]
           ,[address]
           ,[web_site]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[stampid]
           ,reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	  buyer_cd, buyer_nm, ceo_nm, manager_nm, brd_nm, logo, phone_nb, cell_nb, fax_nb, e_mail, 
address, web_site, re_mark, use_yn, del_yn, stampid,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS  reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[buyer_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[buyer_info] off;


/*[comm_dt]*/
truncate table [ShinsungReal].[dbo].[comm_dt];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[comm_dt] ON;
INSERT INTO [ShinsungReal].[dbo].[comm_dt]
           ([mt_cd]
           ,[dt_cd]
           ,[dt_nm]
           ,[dt_kr]
           ,[dt_vn]
           ,[dt_exp]
           ,[up_cd]
           ,[val1]
           ,[val1_nm]
           ,[val2]
           ,[val2_nm]
           ,[val3]
           ,[val3_nm]
           ,[val4]
           ,[val4_nm]
           ,[dt_order]
           ,[use_yn]
           ,[del_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[unit])
Select 
	  mt_cd, dt_cd, dt_nm, dt_kr, dt_vn, dt_exp, up_cd, val1, val1_nm, val2, val2_nm, val3, val3_nm, val4, val4_nm, 
dt_order, use_yn, del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt, unit 
From [ShinsungTest].[autodb-2129].[comm_dt];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[comm_dt] OFF;
/*[comm_mt]*/

truncate table [ShinsungReal].[dbo].[comm_mt];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[comm_mt] ON;
INSERT INTO [ShinsungReal].[dbo].[comm_mt]
           (mt_id,
		   [div_cd]
           ,[mt_cd]
           ,[mt_nm]
           ,[mt_exp]
           ,[memo]
           ,[use_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	  mt_id, div_cd, mt_cd, mt_nm, mt_exp,memo, use_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[comm_mt];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[comm_mt] off;

/*[d_bobbin_info]*/
truncate table [ShinsungReal].[dbo].[d_bobbin_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_bobbin_info] ON;
INSERT INTO [ShinsungReal].[dbo].[d_bobbin_info]
           ([mc_type]
           ,[bb_no]
           ,[mt_cd]
           ,[bb_nm]
           ,[purpose]
           ,[barcode]
           ,[re_mark]
           ,[use_yn]
           ,[count_number]
           ,[del_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	  mc_type, bb_no, mt_cd, bb_nm, purpose, barcode, re_mark, use_yn,(case when count_number='' or count_number is null then 0 else count_number end) AS  count_number, del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[d_bobbin_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_bobbin_info] OFF;



/*[d_bobbin_lct_hist]*/
truncate table [ShinsungReal].[dbo].[d_bobbin_lct_hist];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_bobbin_lct_hist] ON;
INSERT INTO [ShinsungReal].[dbo].[d_bobbin_lct_hist]
           ([mc_type]
           ,[bb_no]
           ,[mt_cd]
           ,[bb_nm]
           --,[start_dt]
           --,[end_dt]
           ,[use_yn]
           ,[del_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	 mc_type, bb_no, mt_cd, bb_nm
	 --, start_dt, end_dt
	 , use_yn, del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id,convert(datetime, reg_dt,121) reg_dt, chg_id, convert(datetime, chg_dt,121) chg_dt
From [ShinsungTest].[autodb-2129].[d_bobbin_lct_hist];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_bobbin_lct_hist] OFF;

/*[d_bom_info]*/
truncate table [ShinsungReal].[dbo].[d_bom_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_bom_info] ON;
INSERT INTO [ShinsungReal].[dbo].[d_bom_info]
           ([bom_no]
           ,[style_no]
           ,[mt_no]
           ,[need_time]
           ,[cav]
           ,[need_m]
           ,[buocdap]
           ,[del_yn]
           ,[isapply]
           ,[IsActive]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	 bom_no, style_no, mt_no, need_time, cav, need_m, buocdap, del_yn, isapply, IsActive,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[d_bom_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_bom_info] OFF;

/*[d_machine_info]*/
truncate table [ShinsungReal].[dbo].[d_machine_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_machine_info] ON;
INSERT INTO [ShinsungReal].[dbo].[d_machine_info]
           ([mc_type]
           ,[mc_no]
           ,[mc_nm]
           ,[purpose]
           ,[color]
           ,[barcode]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	mc_type, 
	mc_no, 
	mc_nm, 
	purpose, 
	color, 
	barcode, 
	re_mark, 
	use_yn, 
	del_yn, 
	(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, 
	reg_dt, 
	chg_id, 
	chg_dt 
From [ShinsungTest].[autodb-2129].[d_machine_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_machine_info] OFF;





/*[d_material_info]*/
truncate table [ShinsungReal].[dbo].[d_material_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_material_info] ON;
INSERT INTO [ShinsungReal].[dbo].[d_material_info]
           ([mt_type]
           ,[mt_no]
           ,[mt_cd]
           ,[mt_no_origin]
           ,[mt_nm]
           ,[mf_cd]
           ,[gr_qty]
           ,[unit_cd]
           ,[bundle_qty]
           ,[bundle_unit]
           ,[sp_cd]
           ,[s_lot_no]
           ,[item_vcd]
           ,[qc_range_cd]
           ,[width]
           ,[width_unit]
           ,[spec]
           ,[spec_unit]
           ,[area]
           ,[area_unit]
           ,[thick]
           ,[thick_unit]
           ,[stick]
           ,[stick_unit]
           ,[consum_yn]
           ,[price]
           ,[tot_price]
           ,[price_unit]
           ,[price_least_unit]
           ,[photo_file]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[barcode]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	mt_type, mt_no, mt_cd, mt_no_origin, mt_nm, mf_cd, gr_qty, unit_cd, bundle_qty, bundle_unit, sp_cd, s_lot_no, item_vcd, qc_range_cd,
width, width_unit, spec, spec_unit, area, area_unit, thick, thick_unit, stick, stick_unit, consum_yn, price, tot_price, price_unit, price_least_unit,
photo_file, re_mark, use_yn, del_yn, barcode,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[d_material_info];
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_material_info] OFF;


/*[d_model_info]*/
truncate table [ShinsungReal].[dbo].[d_model_info];
INSERT INTO [ShinsungReal].[dbo].[d_model_info]
           ([md_cd]
           ,[md_nm]
           ,[use_yn]
           ,[del_yn]
           ,reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])

Select 
	md_cd, md_nm, use_yn, del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS  reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[d_model_info];




/*[d_mold_info]*/

truncate table [ShinsungReal].[dbo].[d_mold_info];
INSERT INTO [ShinsungReal].[dbo].[d_mold_info]
           ([md_no]
           ,[md_nm]
           ,[purpose]
           ,[barcode]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])

Select 
	md_no,md_nm,purpose,barcode,re_mark,use_yn,del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id,reg_dt,chg_id,chg_dt
From [ShinsungTest].[autodb-2129].[d_mold_info];



/*[d_pro_unit_mc]*/

truncate table [ShinsungReal].[dbo].[d_pro_unit_mc];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_pro_unit_mc] ON;
INSERT INTO [ShinsungReal].[dbo].[d_pro_unit_mc]
           (pmid,[id_actual]
           ,[start_dt]
           ,[end_dt]
           ,[remark]
           ,[mc_no]
           ,[use_yn]
           ,[del_yn]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	pmid, id_actual, CONVERT(DATETIME, STUFF(STUFF(STUFF(start_dt,13,0,':'),11,0,':'),9,0,' ')) start_dt,
	CONVERT(DATETIME, STUFF(STUFF(STUFF(end_dt,13,0,':'),11,0,':'),9,0,' ')) end_dt,
	remark, mc_no, use_yn, del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, 
	reg_dt, chg_id, chg_dt 
From [ShinsungTest].[autodb-2129].[d_pro_unit_mc];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_pro_unit_mc] OFF;


/*[product_routing]*/

truncate table [ShinSungReal].[dbo].[product_routing];
SET IDENTITY_INSERT [ShinSungReal].[dbo].[product_routing] ON;
INSERT INTO [ShinSungReal].[dbo].[product_routing]
           ([style_no]
      ,[process_code]
      ,[IsApply]
      ,[process_name]
      ,[description]
      ,[reg_dt]
      ,[reg_id]
      ,[chg_id]
      ,[chg_dt]
     )
Select 
	style_no
	,process_code
	,IsApply
	, process_name
	, description
	, reg_dt
	, reg_id
	, chg_id
	, chg_dt
From [ShinsungTest].[autodb-2129].[product_routing];
SET IDENTITY_INSERT [ShinSungReal].[dbo].[product_routing] off;

/*[d_rounting_info]*/
truncate table [ShinsungReal].[dbo].[d_rounting_info];
INSERT INTO [ShinsungReal].[dbo].[d_rounting_info]
           ([style_no]
		   ,[process_code]
           ,[name]
           ,[level]
           ,[don_vi_pr]
           ,[type]
           ,[item_vcd]
           ,[description]
		   ,[IsFinish]
           ,[reg_dt]
           ,reg_id
           ,[chg_id]
           ,[chg_dt])
Select 
	style_no, process_code,name, level, don_vi_pr, type, item_vcd, description,IsFinish, reg_dt, (case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[d_rounting_info];



/*[d_style_info]*/

truncate table [ShinsungReal].[dbo].[d_style_info];
INSERT INTO [ShinsungReal].[dbo].[d_style_info]
           ([style_no]
           ,[style_nm]
           ,[md_cd]
           ,[prj_nm]
           ,[ssver]
           ,[part_nm]
           ,[standard]
           ,[cust_rev]
           ,[order_num]
           ,[pack_amt]
           ,[cav]
           ,[bom_type]
           ,[tds_no]
           ,[item_vcd]
           ,[qc_range_cd]
           ,[stamp_code]
           ,[expiry_month]
           ,[expiry]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[drawingname]
           ,[loss]
           ,[Description]
           ,[productType])
Select 
	 style_no, 
	 style_nm,
	 md_cd,
	 prj_nm, 
	 ssver,
	 part_nm,
	 standard, 
	 cust_rev, 
	 order_num,
	 (case when pack_amt='' or pack_amt is null then 0 else pack_amt end) AS pack_amt,
	 cav, 
	 bom_type,
	 tds_no,
	 item_vcd,
	 qc_range_cd,
	 stamp_code,
	 expiry_month,
	 expiry, 
 use_yn, 
 del_yn, 
 (case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, 
 reg_dt,
 chg_id, 
 chg_dt,
 drawingname,
 loss,
 Description,
 productType
From [ShinsungTest].[autodb-2129].[d_style_info];




/*[department_info]*/

truncate table [ShinsungReal].[dbo].[department_info];
INSERT INTO [ShinsungReal].[dbo].[department_info]
           ([depart_cd]
           ,[depart_nm]
           ,[up_depart_cd]
           ,[level_cd]
           ,[re_mark]
           ,[use_yn]
           ,[order_no]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[mn_full])
Select 
	  depart_cd, depart_nm, up_depart_cd, level_cd, re_mark, use_yn, order_no, del_yn, (case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt, mn_full
From [ShinsungTest].[autodb-2129].[department_info];




/*[ExportToMachine]*/
truncate table [ShinsungReal].[dbo].[ExportToMachine];
INSERT INTO [ShinsungReal].[dbo].[ExportToMachine]
           ([ExportCode]
           ,[ProductCode]
           ,[ProductName]
           ,[MachineCode]
           ,[IsFinish]
           ,[Description]
           ,[CreateId]
           ,[CreateDate]
           ,[ChangeId]
           ,[ChangeDate])
Select 
	      ExportCode,
    ProductCode,
    ProductName,
    MachineCode,
    IsFinish,
    Description,
    (case when CreateId='' or CreateId is null then 'admin' else CreateId end) AS CreateId,
    CreateDate,
    ChangeId,
    ChangeDate
From [ShinsungTest].[autodb-2129].[ExportToMachine];









/*[generalfg]*/
truncate table [ShinsungReal].[dbo].[generalfg];
INSERT INTO [ShinsungReal].[dbo].[generalfg]
           ([buyer_qr]
           ,[product_code]
           ,[at_no]
           ,[type]
           ,[md_cd]
           ,[dl_no]
           ,[qty]
           ,[lot_no]
           ,[status]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	   buyer_qr, product_code, at_no, type, md_cd, dl_no, qty, lot_no, sts_cd, use_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[generalfg];



/*[lct_info]*/

truncate table [ShinsungReal].[dbo].[lct_info];
INSERT INTO [ShinsungReal].[dbo].[lct_info]
           ([lct_cd]
           ,[lct_nm]
           ,[up_lct_cd]
           ,[level_cd]
           ,[index_cd]
           ,[shelf_cd]
           ,[order_no]
           ,[real_use_yn]
           ,[re_mark]
           ,[use_yn]
           ,[lct_rfid]
           ,[lct_bar_cd]
           ,[sf_yn]
           ,[is_yn]
           ,[mt_yn]
           ,[mv_yn]
           ,[ti_yn]
           ,[fg_yn]
           ,[rt_yn]
           ,[ft_yn]
           ,[wp_yn]
           ,[nt_yn]
           ,[pk_yn]
           ,[manager_id]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[mn_full]
           ,[sap_lct_cd]
           ,[userid]
           ,[selected])
Select 
	  lct_cd, lct_nm, up_lct_cd, level_cd, index_cd, shelf_cd, order_no, real_use_yn, re_mark, use_yn, lct_rfid, lct_bar_cd, 
 sf_yn, is_yn, mt_yn, mv_yn, ti_yn, fg_yn, rt_yn, ft_yn, wp_yn, nt_yn, pk_yn, manager_id, reg_id, reg_dt, chg_id, chg_dt, mn_full, sap_lct_cd, userid, selected
From [ShinsungTest].[autodb-2129].[lct_info]



/*[m_board]*/
truncate table [ShinsungReal].[dbo].[m_board];
INSERT INTO [ShinsungReal].[dbo].[m_board]
           ([title]
           ,[content]
           ,[viewcnt]
           ,[replycnt]
           ,[div_cd]
           ,[start_dt]
           ,[end_dt]
           ,[widthsize]
           ,[heightsize]
           ,[back_color]
           ,[order_no]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
	  [title]
      ,[content]
      ,[viewcnt]
      ,[replycnt]
      ,[div_cd]
      ,[start_dt]
      ,[end_dt]
      ,[widthsize]
      ,[heightsize]
      ,[back_color]
      ,[order_no]
      ,[del_yn]
      ,[reg_id]
      ,[reg_dt]
      ,[chg_id]
      ,[chg_dt]
From [ShinsungTest].[autodb-2129].[m_board]




/*[m_facline_qc]*/
truncate table [ShinsungReal].[dbo].[m_facline_qc];

INSERT INTO [ShinsungReal].[dbo].[m_facline_qc]
           ([fq_no]
           ,[ml_no]
           ,[ml_tims]
           ,[product_cd]
           ,[shift]
           ,[at_no]
           ,[work_dt]
           ,[item_vcd]
           ,[item_nm]
           ,[item_exp]
           ,[check_qty]
           ,[ok_qty]
		   ,[ng_qty]
		   ,[remain_qty]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
	  fq_no,
	  ml_no,
	  ml_tims, 
	  product_cd, 
	  shift, 
	  at_no,
	  work_dt, 
	  item_vcd, 
	  item_nm, 
	  item_exp,
(case when check_qty='' or check_qty is null then 0 else check_qty end) check_qty,(case when ok_qty='' or ok_qty is null then 0 else ok_qty end) ok_qty,
(case when ng_qty='' or ng_qty is null then 0 else ng_qty end) ng_qty,(case when remain_qty='' or remain_qty is null then 0 else remain_qty end) remain_qty, reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[m_facline_qc]









/*[m_facline_qc_value]*/

truncate table [ShinsungReal].[dbo].[m_facline_qc_value];

INSERT INTO [ShinsungReal].[dbo].[m_facline_qc_value]
           ([fq_no]
           ,[product]
           ,[at_no]
           ,[shift]
           ,[item_vcd]
           ,[check_id]
           ,[check_cd]
           ,[check_value]
           ,[check_qty]
           ,[date_ymd]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
fq_no, product,at_no, shift, item_vcd, check_id, check_cd, check_value, check_qty, date_ymd, reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[m_facline_qc_value]














/*[manufac_info]*/

truncate table [ShinsungReal].[dbo].[manufac_info];

INSERT INTO [ShinsungReal].[dbo].[manufac_info]
           ([mf_cd]
           ,[mf_nm]
           ,[brd_nm]
           ,[logo]
           ,[phone_nb]
           ,[web_site]
           ,[address]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
 mf_cd, mf_nm, brd_nm, logo, phone_nb, web_site, address, re_mark, use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[manufac_info]








/*[materialbom]*/

truncate table [ShinsungReal].[dbo].[materialbom];

INSERT INTO [ShinsungReal].[dbo].[materialbom]
           ([ProductCode]
           ,[MaterialPrarent]
           ,[MaterialNo]
           ,[CreateId]
           ,[CreateDate]
           ,[ChangeId]
           ,[ChangeDate])
	Select 
 ProductCode, MaterialPrarent, MaterialNo, CreateId, CreateDate, ChangeId, ChangeDate
From [ShinsungTest].[autodb-2129].[materialbom];




/*[mb_author_info]*/
truncate table [ShinsungReal].[dbo].[mb_author_info];

INSERT INTO [ShinsungReal].[dbo].[mb_author_info]
           ([userid]
           ,[at_cd]
           ,[re_mark]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
      userid
      ,at_cd
      ,re_mark
      ,use_yn
      ,reg_id
      ,reg_dt
      ,chg_id
      ,chg_dt
From [ShinsungTest].[autodb-2129].[mb_author_info];
















/*[mb_info]*/

truncate table [ShinsungReal].[dbo].[mb_info];

INSERT INTO [ShinsungReal].[dbo].[mb_info]
           ([userid]
           ,[uname]
           ,[nick_name]
           ,[upw]
           ,[grade]
           ,[depart_cd]
           ,[gender]
           ,[position_cd]
           ,[tel_nb]
           ,[cel_nb]
           ,[e_mail]
           ,[sms_yn]
           ,[join_dt]
           ,[birth_dt]
           ,[scr_yn]
           ,[mail_yn]
           ,[join_ip]
           ,[join_domain]
           ,[ltacc_dt]
           ,[ltacc_domain]
           ,[mbout_dt]
           ,[mbout_yn]
           ,[accblock_yn]
           ,[session_key]
           ,[session_limit]
           ,[memo]
           ,[del_yn]
           ,[check_yn]
           ,[rem_me]
           ,[barcode]
           ,[mbjoin_dt]
           ,[log_ip]
           ,[lct_cd]
           , reg_id
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[re_mark])
	Select 
   userid
  ,uname
  ,nick_name
  ,upw
  ,grade
  ,depart_cd
  ,gender
  ,position_cd
  ,tel_nb
  ,cel_nb
  ,e_mail
  ,sms_yn
  ,CONVERT(DATETIME, STUFF(STUFF(STUFF(join_dt,13,0,':'),11,0,':'),9,0,' ')) join_dt
  ,CONVERT(DATETIME, STUFF(STUFF(STUFF(birth_dt,13,0,':'),11,0,':'),9,0,' ')) birth_dt
  ,scr_yn
  ,mail_yn
  ,join_ip
  ,join_domain
  ,ltacc_dt
  ,ltacc_domain
  ,mbout_dt
  ,mbout_yn
  ,accblock_yn
  ,session_key
  ,session_limit
  ,memo
  ,del_yn
  ,check_yn
  ,rem_me
  ,barcode
  ,mbjoin_dt
  ,log_ip
  ,lct_cd
  ,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id
  ,reg_dt
  ,chg_id
  ,chg_dt
  ,re_mark
From [ShinsungTest].[autodb-2129].[mb_info];








/*[menu_info]*/
truncate table [ShinsungReal].[dbo].[menu_info];


INSERT INTO [ShinsungReal].[dbo].[menu_info]
           ([mn_cd]
           ,[mn_nm]
           ,[up_mn_cd]
           ,[level_cd]
           ,[url_link]
           ,[re_mark]
           ,[col_css]
           ,[sub_yn]
           ,[order_no]
           ,[use_yn]
           ,[mn_full]
           ,[mn_cd_full]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
		    ,[nameen]
           ,[namevi]
           ,[namekr]
		    ,[selected])
	Select 
   mn_cd
      ,mn_nm
      ,up_mn_cd
      ,level_cd
      ,url_link
      ,re_mark
      ,col_css
      ,sub_yn
      ,(case when order_no='' or order_no is null then 0 else order_no end) order_no
      ,use_yn
      ,mn_full
      ,mn_cd_full
      ,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id
      ,reg_dt
      ,chg_id
      ,chg_dt
	   ,nameen
	  ,namevi
	  ,namekr
      ,selected
	 
From [ShinsungTest].[autodb-2129].[menu_info];






/*[notice_board]*/
truncate table [ShinsungReal].[dbo].[notice_board];


INSERT INTO [ShinsungReal].[dbo].[notice_board]
           ([title]
           ,[content]
           ,[mn_cd]
           ,[viewcnt]
           ,[replycnt]
           ,[div_cd]
           ,[lng_cd]
           ,[start_dt]
           ,[end_dt]
           ,[widthsize]
           ,[heightsize]
           ,[back_color]
           ,[order_no]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
   [title]
      ,[content]
      ,[mn_cd]
      ,[viewcnt]
      ,[replycnt]
      ,[div_cd]
      ,[lng_cd]
      ,[start_dt]
      ,[end_dt]
      ,[widthsize]
      ,[heightsize]
      ,[back_color]
      ,[order_no]
      ,[del_yn]
      ,[reg_id]
      ,[reg_dt]
      ,[chg_id]
      ,[chg_dt]
From [ShinsungTest].[autodb-2129].[notice_board];











/*[product_material]*/
--SET IDENTITY_INSERT [ShinsungReal].[dbo].[product_material]  off
truncate table [ShinsungReal].[dbo].[product_material];

SET IDENTITY_INSERT [ShinsungReal].[dbo].[product_material] ON;
INSERT INTO [ShinsungReal].[dbo].[product_material]
           (id
		   ,[style_no]
		   ,[process_code]
           ,[level]
           ,[name]
           ,[mt_no]
           ,[need_time]
           ,[cav]
           ,[need_m]
           ,[buocdap]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select 
   id,style_no,process_code, level, name, mt_no, need_time, cav, need_m, buocdap, isActive, reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[product_material];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[product_material]  off




/*[product_material_detail]*/
truncate table [ShinsungReal].[dbo].[product_material_detail];


INSERT INTO [ShinsungReal].[dbo].[product_material_detail]
           ([ProductCode]
		   ,[process_code]
           ,[level]
		   ,[name]
           ,[MaterialParent]
           ,[MaterialNo]
           ,[CreateId]
           ,[ChangeId]
           ,[CreateDate]
           ,[ChangeDate])
	Select ProductCode,process_code, level,name, MaterialPrarent, MaterialNo, CreateId, ChangeId, CreateDate,  ChangeDate
From [ShinsungTest].[autodb-2129].[product_material_detail];







/*([item_type]*/

truncate table [ShinsungReal].[dbo].[qc_item_mt];


INSERT INTO [ShinsungReal].[dbo].[qc_item_mt]
           ([item_type]
           ,[item_vcd]
           ,[item_cd]
           ,[ver]
           ,[item_nm]
           ,[item_exp]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
	Select item_type, item_vcd, item_cd, ver, item_nm, item_exp, use_yn, del_yn,(case when reg_id='' or reg_id is null then 'admin' else reg_id end) AS reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[qc_item_mt];






/*[inventory_products]*/
Truncate table [ShinsungReal].[dbo].[inventory_products];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[inventory_products] ON;
INSERT INTO [ShinsungReal].[dbo].[inventory_products]
           (materialid
		   ,[id_actual]
           ,[material_code]
           ,[recei_wip_date]
           ,[picking_date]
           ,[sd_no]
           ,[ex_no]
           ,[lct_sts_cd]
           ,[mt_no]
           ,[mt_type]
           ,[gr_qty]
           ,[real_qty]
          ,[bb_no]
           ,[orgin_mt_cd]
           ,[recei_date]
           ,[expiry_date]
           ,[export_date]
           ,[date_of_receipt]
           ,[lot_no]
           ,[from_lct_cd]
           ,[location_code]
           ,[status]
           ,[create_id]
           ,[create_date]
           ,[change_id]
           ,[change_date]
           ,[ExportCode]
           ,[LoctionMachine]
           ,[shipping_wip_dt]
           ,[ShippingToMachineDatetime]
           ,[return_date]
		   )
Select 
	   wmtid, 
	  (case when id_actual='' or id_actual is null then 0 else id_actual end)   id_actual,
	   mt_cd, 
	  (case when rece_wip_dt='' or rece_wip_dt is null then null else CONVERT(DATETIME,rece_wip_dt,121) end) rece_wip_dt,
	 (case when picking_dt='' or picking_dt is null then null else CONVERT(DATETIME,picking_dt,121) end) picking_dt,
	   sd_no, 
	   ex_no, 
	   lct_sts_cd, 
	   mt_no, 
	   mt_type, 
	   (case when gr_qty='' or gr_qty is null then 0 else gr_qty end)  gr_qty,
	   (case when real_qty='' or real_qty is null then 0 else real_qty end)real_qty, 
	   bb_no, 
	   orgin_mt_cd,
(case when recevice_dt='' or recevice_dt is null then null else CONVERT(DATE,recevice_dt,120) end)  recevice_dt,
(case when expiry_dt='' or expiry_dt is null then null else CONVERT(DATE,expiry_dt,120)  end)  expiry_dt,
(case when expore_dt='' or expore_dt is null then null else CONVERT(DATE,expore_dt,120) end)  expore_dt,
(case when dt_of_receipt='' or dt_of_receipt is null then null else CONVERT(DATE,dt_of_receipt,120)  end)  dt_of_receipt,
   lot_no, 
   from_lct_cd, 
   lct_cd, 
   mt_sts_cd, 
   reg_id, 
   reg_dt, 
   chg_id, 
   chg_dt, 
    ExportCode, 
    LoctionMachine,
	shipping_wip_dt,
    ShippingToMachineDatetime,
	(case when return_date='' or return_date is null then null else  CONVERT(DATETIME, STUFF(STUFF(STUFF(return_date,13,0,':'),11,0,':'),9,0,' '))   end)  return_date
From [ShinsungTest].[autodb-2129].w_material_info
where wmtid not in (SELECT wmtid FROM [ShinsungTest].[autodb-2129].w_material_info WHERE id_actual IN( select id_actual from [ShinsungTest].[autodb-2129].w_actual WHERE type = 'TIMS') OR mt_cd LIKE '%TIMS-NG%')
and wmtid not in (SELECT wmtid FROM [ShinsungTest].[autodb-2129].w_material_info WHERE id_actual IN( select id_actual from [ShinsungTest].[autodb-2129].w_actual WHERE type = 'SX'))
and  ExportCode is not null or sd_no is not null
--and ExportCode='EP3764'
SET IDENTITY_INSERT [ShinsungReal].[dbo].[inventory_products] off;







/* [qc_itemcheck_dt] */
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[qc_itemcheck_dt]
INSERT INTO [ShinsungReal].[dbo].[qc_itemcheck_dt]
           (
           [item_vcd]
           ,[check_id]
           ,[check_cd]
           ,[defect_yn]
           ,[check_name]
           ,[order_no]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
    Select item_vcd, check_id, check_cd, defect_yn, check_name, order_no, re_mark, use_yn, del_yn,
	 (Case When reg_id = '' OR reg_id IS NULL then  '' end) as reg_id , reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[qc_itemcheck_dt]
GO





/* [shippingsdmaterial]  */
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[shippingsdmaterial]
INSERT INTO [ShinsungReal].[dbo].[shippingsdmaterial]
           (
           [sd_no]
           ,[mt_no]
           ,[quantity]
           ,[meter]
           ,[reg_id]
           ,[reg_dt])
 SELECT sd_no, mt_no, quantity, meter,(Case When reg_id = '' OR reg_id IS NULL then  'root' else reg_id end)  reg_id, reg_dt
 From [ShinsungTest].[autodb-2129].[shippingsdmaterial]
GO


/*[stamp_detail]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[stamp_detail]
INSERT INTO [ShinsungReal].[dbo].[stamp_detail]
           (
           [buyer_qr]
           ,[stamp_code]
           ,[product_code]
		   ,[ssver]
           ,[vendor_code]
           ,[vendor_line]
           ,[label_printer]
           ,[is_sample]
           ,[pcn]
           ,[lot_date]
           ,[serial_number]
           ,[machine_line]
           ,[shift]
           ,[standard_qty]
           ,[is_sent]
           ,[box_code]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
 Select buyer_qr, stamp_code, product_code,ssver, vendor_code, vendor_line, label_printer, is_sample, pcn, lot_date, serial_number, 
machine_line, shift, standard_qty, is_sent, box_code,(Case When reg_id = '' OR reg_id IS NULL then  'root' else reg_id end) reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[stamp_detail]
Go




/*[stamp_master]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[stamp_master]
INSERT INTO [ShinsungReal].[dbo].[stamp_master]
           (
           [stamp_code]
           ,[stamp_name])
   select stamp_code, stamp_name from [ShinsungTest].[autodb-2129].[stamp_master]

GO





/*[supplier_info]*/
USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[supplier_info]
INSERT INTO [ShinsungReal].[dbo].[supplier_info]
           (
           [sp_cd]
           ,[sp_nm]
           ,[bsn_tp]
           ,[phone_nb]
           ,[cell_nb]
           ,[fax_nb]
           ,[e_mail]
           ,[web_site]
           ,[address]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
    Select sp_cd, sp_nm, bsn_tp, phone_nb, cell_nb, fax_nb, e_mail, web_site, address, re_mark, use_yn, del_yn, 
reg_id, reg_dt, chg_id, chg_dt 
	From [ShinsungTest].[autodb-2129].[supplier_info]
GO



/*[version_app] */
USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[version_app]
INSERT INTO [ShinsungReal].[dbo].[version_app]
           (
           [type]
           ,[name_file]
           ,[version]
           ,[chg_dt]
          )

   select type, name_file, version, chg_dt
    from [ShinsungTest].[autodb-2129].[version_app]
GO



/*[w_actual]*/

USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[w_actual]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_actual] ON;
INSERT INTO [ShinsungReal].[dbo].[w_actual]
           ([id_actual]
           ,[at_no]
           ,[type]
	,[product]
           ,[actual]
           ,[defect]
           ,[name]
           ,[level]
           ,[date]
           ,[don_vi_pr]
           ,[item_vcd]
           ,[description]
           ,[IsFinish]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
   Select id_actual, at_no, type, product, actual, defect, name, level, date, don_vi_pr, item_vcd, description, IsFinished, reg_id, reg_dt, chg_id, chg_dt 
From [ShinsungTest].[autodb-2129].[w_actual]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_actual] off;
GO



/*[w_actual_primary]*/
USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[w_actual_primary]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_actual_primary] ON;
INSERT INTO [ShinsungReal].[dbo].[w_actual_primary]

           ([id_actualpr]
           ,[at_no]
           ,[type]
           ,[target]
           ,[product]
		   ,[process_code]
           ,[remark]
           ,[finish_yn]
           ,[isapply]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           )
Select id_actualpr, at_no, type, target, product,process_code, remark, finish_yn, IsApply,reg_id, reg_dt, chg_id, chg_dt 
From  [ShinsungTest].[autodb-2129].[w_actual_primary]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_actual_primary] off;
GO






/*[w_box_mapping]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[w_box_mapping]
INSERT INTO [ShinsungReal].[dbo].[w_box_mapping]
           (
           [bx_no]
           ,[buyer_cd]
           ,[mt_cd]
           ,[gr_qty]
           ,[product]
           ,[type]
           ,[mapping_dt]
           ,[status]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           )
    Select  bx_no, buyer_cd, mt_cd, gr_qty, product, type, mapping_dt, sts As status, 
use_yn, del_yn, reg_id, reg_dt, chg_id, chg_dt 
From [ShinsungTest].[autodb-2129].[w_box_mapping]
GO




/* [w_dl_info] */
USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[w_dl_info]
INSERT INTO [ShinsungReal].[dbo].[w_dl_info]
           ([dl_no]
           ,[dl_nm]
           ,[status]
           ,[work_dt]
           ,[lct_cd]
           ,[remark]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
          )
Select dl_no, dl_nm, dl_sts_cd As Status, work_dt, lct_cd, remark, use_yn, reg_id, 
reg_dt, chg_id, chg_dt From [ShinsungTest].[autodb-2129].[w_dl_info]
GO



/* [w_ex_info] */
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[w_ex_info]
INSERT INTO [ShinsungReal].[dbo].[w_ex_info]
           ([ex_no]
           ,[ex_nm]
           ,[status]
           ,[work_dt]
           ,[lct_cd]
           ,[alert]
           ,[remark]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
		)
Select ex_no, ex_nm, ex_sts_cd As status, work_dt, lct_cd, alert, remark, use_yn,(Case When reg_id = '' OR reg_id IS NULL then  'root' else reg_id end)   reg_id , reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[w_ex_info]
GO




/* [w_material_down] */
USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[w_material_down]
INSERT INTO [ShinsungReal].[dbo].[w_material_down]
           ([mt_cd]
           ,[gr_qty]
           ,[gr_down]
           ,[reason]
           ,[status_now]
           ,[bb_no]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           )
    Select mt_cd, gr_qty, gr_down, reason, sts_now As Status, bb_no, use_yn, reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[w_material_down]
GO



/*[w_material_info_memo]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[w_material_info_memo]
INSERT INTO [ShinsungReal].[dbo].[w_material_info_memo]

           ([md_cd]
           ,[style_no]
           ,[style_nm]
           ,[mt_cd]
           ,[width]
           ,[width_unit]
           ,[spec]
           ,[spec_unit]
           ,[sd_no]
           ,[lot_no]
           ,[status]
           ,[memo]
           ,[month_excel]
           ,[receiving_dt]
           ,[tx]
           ,[total_m]
           ,[total_m2]
           ,[total_ea]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
          )
     Select md_cd, style_no, style_nm, mt_cd, width, width_unit, spec, spec_unit, sd_no, lot_no, sts_cd, memo, month_excel, receiving_dt, Tx, total_m, total_m2, total_ea, use_yn, reg_id, reg_dt, chg_id, chg_dt
 From [ShinsungTest].[autodb-2129].[w_material_info_memo]
GO


	

	
/* [w_material_info_mms] */
 
USE [ShinsungReal]
--GOUSE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[w_material_info_mms]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_info_mms] ON;
INSERT INTO [dbo].[w_material_info_mms]
           (wmtid
		   ,[id_actual]
           ,[material_code]
           ,[material_type]
           ,[gr_qty]
           ,[real_qty]
           ,[number_divide]
           ,[id_actual_oqc]
           ,[status]
           ,[mt_no]
           ,[bb_no]
           ,[sd_no]
           ,[expiry_date]
           ,[export_date]
           ,[date_of_receipt]
           ,[rece_wip_dt]
           ,[recevice_dt_tims]
           ,[lot_no]
           ,[location_code]
           --,[location_number]
           ,[from_lct_code]
           ,[to_lct_code]
           ,[receipt_date]
           ,[reg_date]
           ,[reg_id]
           ,[chg_date]
           ,[chg_id]
           --,[bbmp_sts_cd]
           ,[orgin_mt_cd]
           ,[ExportCode]
           ,[sts_update]
	   ,[ShippingToMachineDatetime]
	   ,[description]
		   )
     SELECT
		wmtid,
        id_actual, 
        mt_cd, 
        mt_type, 
        (case when gr_qty='' or gr_qty is null then 0 else gr_qty end) gr_qty, 
		(case when real_qty='' or real_qty is null then 0 else real_qty end) real_qty, 
        0 ,
       (case when id_actual_oqc='' or id_actual_oqc is null then 0 else id_actual_oqc end) id_actual_oqc,
		mt_sts_cd,
        mt_no, 
        bb_no, 
        sd_no,
	(case when expiry_dt = '' or expiry_dt is null then null else CONVERT(DATE,expiry_dt,120) end) As expiry_dt,
	(case when expore_dt = '' or expore_dt is null then null else CONVERT(DATEtime,expore_dt,120) end) As expore_dt,
	 (case when dt_of_receipt = '' or dt_of_receipt is null then null else CONVERT(DATEtime,dt_of_receipt,120) end) As dt_of_receipt,
	 (case when rece_wip_dt = '' or rece_wip_dt is null then null else CONVERT(DATEtime,rece_wip_dt,120) end) As rece_wip_dt,
	 (case when recevice_dt_tims = '' or recevice_dt_tims is null then null else CONVERT(DATEtime,recevice_dt_tims,120) end) As recevice_dt_tims,
        lot_no,
		lct_cd, 
        -- null,
        from_lct_cd, 
        to_lct_cd,
	(case when recevice_dt = '' or recevice_dt is null then null else CONVERT(DATEtime,recevice_dt,120) end) As recevice_dt, 
	convert(datetime,reg_dt,121),
	 reg_id, 
	convert(datetime,chg_dt,121),
        chg_id,
        --bbmp_sts_cd,
        orgin_mt_cd,
	ExportCode,
	sts_update,
	ShippingToMachineDatetime,
	description
       
FROM [ShinsungTest].[autodb-2129].w_material_info
WHERE id_actual IN( select id_actual from [ShinsungTest].[autodb-2129].w_actual WHERE type = 'SX') --and sd_no='SD4268'
and  (ExportCode is null or ExportCode ='') and (sd_no is null or sd_no='')
or mt_cd  LIKE '%-MMS-NG%'
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_info_mms] Off;
GO





/* [qc_itemcheck_mt] */
Truncate table [ShinsungReal].[dbo].[qc_itemcheck_mt]
INSERT INTO [ShinsungReal].[dbo].[qc_itemcheck_mt]
           (
			[item_vcd]
           ,[check_id]
           ,[check_type]
           ,[check_subject]
           ,[min_value]
           ,[max_value]
           ,[range_type]
           ,[order_no]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select item_vcd, check_id, check_type, check_subject, min_value, max_value, range_type, order_no, re_mark, use_yn, del_yn, 
(Case When reg_id = '' OR reg_id IS NULL then  '' end) as reg_id 
,reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[qc_itemcheck_mt]
GO



/* [w_material_mapping_mms] */ 
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[w_material_mapping_mms]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_mapping_mms] ON;
INSERT INTO [ShinsungReal].[dbo].[w_material_mapping_mms]
           ([wmmId]
	   ,[mt_lot]
           ,[mt_cd]
           ,[mt_no]
           ,[mapping_dt]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_date]
           ,[chg_id]
           ,[chg_date]
           )
Select wmmid,mt_lot,mt_cd,mt_no,
(case when mapping_dt = '' or mapping_dt is null then null 
				else CONVERT(DATETIME, STUFF(STUFF(STUFF(mapping_dt,13,0,':'),11,0,':'),9,0,' ')) end) As mapping_dt,
use_yn,del_yn,(Case When reg_id = '' OR reg_id IS NULL then  '' end)  reg_id,reg_dt,chg_id,chg_dt 
From [ShinsungTest].[autodb-2129].[w_material_mapping] 
Where [ShinsungTest].[autodb-2129].[w_material_mapping].wmmid
NOT IN (SELECT wmmid FROM [ShinsungTest].[autodb-2129].[w_material_mapping] WHERE mt_lot NOT LIKE '%ROT%' AND mt_lot NOT LIKE '%STA%')
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_mapping_mms] Off;
GO



/* [w_material_info_tam] */ 
USE [ShinsungReal]
GO

Truncate table [ShinsungReal].[dbo].[w_material_info_tam]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_info_tam] On;
INSERT INTO [ShinsungReal].[dbo].[w_material_info_tam]
           ([wmtid]
		   ,[id_actual]
           ,[id_actual_oqc]
           ,[staff_id]
           ,[staff_id_oqc]
           ,[mt_type]
           ,[mt_cd]
           ,[mt_no]
           ,[gr_qty]
           ,[real_qty]
           ,[sp_cd]
           ,[rd_no]
           ,[sd_no]
           ,[ext_no]
           ,[ex_no]
           ,[dl_no]
           ,[picking_dt]
           ,[recevice_dt]
           ,[date]
           ,[return_date]
           ,[alert_ng]
           ,[expiry_dt]
           ,[dt_of_receipt]
           ,[expore_dt]
           ,[recevice_dt_tims]
           ,[lot_no]
           ,[mt_barcode]
           ,[mt_qrcode]
           ,[status]
           ,[bb_no]
           ,[bbmp_sts_cd]
           ,[lct_cd]
           ,[lct_sts_cd]
           ,[from_lct_cd]
           ,[to_lct_cd]
           ,[output_dt]
           ,[input_dt]
           ,[buyer_qr]
           ,[orgin_mt_cd]
           ,[remark]
           ,[sts_update]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           )

      Select wmtid,id_actual, id_actual_oqc, staff_id, staff_id_oqc, mt_type, mt_cd, mt_no, gr_qty, real_qty, sp_cd, rd_no, sd_no, ext_no, ex_no, dl_no, 
	picking_dt, 
	(case when recevice_dt = '' or recevice_dt is null then null else CONVERT(DATE,recevice_dt,120) end) As recevice_dt,  
	(case when date = '' or date is null then null else CONVERT(DATETIME, STUFF(STUFF(STUFF(date,13,0,':'),11,0,':'),9,0,' ')) end) As date, return_date, alert_NG,
	 (case when expiry_dt = '' or expiry_dt is null then null else CONVERT(DATE,expiry_dt,120) end) As expiry_dt,
	  (case when dt_of_receipt = '' or dt_of_receipt is null then null else CONVERT(DATE,dt_of_receipt,120) end) As dt_of_receipt,
	 (case when expore_dt = '' or expore_dt is null then null else CONVERT(DATE,expore_dt,120) end) As expore_dt,
	  (case when recevice_dt_tims = '' or recevice_dt_tims is null then null else CONVERT(DATE,recevice_dt_tims,120) end) As recevice_dt_tims,
 lot_no, mt_barcode, mt_qrcode, mt_sts_cd, bb_no, bbmp_sts_cd, lct_cd, lct_sts_cd, from_lct_cd, to_lct_cd, output_dt, input_dt, buyer_qr, orgin_mt_cd,
remark, sts_update, use_yn, reg_id, 
reg_dt, 
chg_id,
 chg_dt
From [ShinsungTest].[autodb-2129].[w_material_info_tam]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_info_tam] off;
GO





/*[w_material_info_tims]*/
truncate table [ShinsungReal].[dbo].[w_material_info_tims];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_info_tims] On;
INSERT INTO [ShinsungReal].[dbo].[w_material_info_tims]
           (wmtid
		   ,[at_no]
           ,[product]
           ,[id_actual]
           ,[id_actual_oqc]
           ,[ext_no]
           ,[staff_id]
           ,[staff_id_oqc]
           ,[material_code]
           ,[recevice_dt]
           ,[buyer_qr]
           ,[material_type]
           ,[gr_qty]
           ,[output_dt]
           ,[real_qty]
           ,[number_divide]
           ,[status]
           ,[sts_update]
           --,[lct_sts_cd]
           --,[mt_sts_cd]
           ,[mt_no]
           ,[bb_no]
           ,[dl_no]
           ,[sd_no]
           ,[return_date]
           ,[input_dt]
           --,[expiry_dt]
           --,[expore_dt]
           --,[date_of_receipt]
           --,[rd_no]
           --,[lot_no]
           ,[location_code]
           --,[bbmp_sts_cd]
           ,[from_lct_code]
           ,[orgin_mt_cd]
           ,[to_lct_code]
           ,[use_yn]
           ,[alert_ng]
           ,[remark]
           ,[receipt_date]
           ,[end_production_dt]
           ,[reg_date]
           ,[reg_id]
           ,[chg_date]
           ,[chg_id]
           ,[location_number]
           ,[ExportCode])
Select 
	  wmtid,
	  at_no,
	  product,
	  id_actual,
	  id_actual_oqc,
	  ex_no,
	  staff_id,
	  staff_id_oqc,
mt_cd,
(case when recevice_dt='' or recevice_dt is null then null else CONVERT(DATE,recevice_dt,120) end)  recevice_dt,
buyer_qr,
mt_type,
(case when gr_qty='' or gr_qty is null then 0 else gr_qty end)  gr_qty,
output_dt,
(case when real_qty='' or real_qty is null then 0 else real_qty end)  real_qty,
0,
mt_sts_cd,
sts_update,
--lct_sts_cd,
--mt_sts_cd,
mt_no,
bb_no,
dl_no,
sd_no,
return_date,
input_dt,
--(case when expiry_dt='' or expiry_dt is null then null else CONVERT(DATE,expiry_dt,120)  end)  expiry_dt,
--(case when expore_dt='' or expore_dt is null then null else CONVERT(DATE,expore_dt,120) end)  expore_dt,
--(case when dt_of_receipt='' or dt_of_receipt is null then null else CONVERT(DATE,dt_of_receipt,120)  end)  dt_of_receipt,
--rd_no,
--lot_no,
lct_cd,
--bbmp_sts_cd,
from_lct_cd,
orgin_mt_cd,
to_lct_cd,
use_yn,
alert_NG,
remark,
(case when recevice_dt='' or recevice_dt is null then null else CONVERT(DATE,recevice_dt,120)  end) recevice_dt,
(case when end_production_dt='' or end_production_dt is null then null else CONVERT(DATEtime,end_production_dt,120)  end)  end_production_dt,
reg_dt,
(case when reg_id='' or reg_id is null then 'root' else reg_id end) reg_id,
chg_dt,
(case when chg_id='' or chg_id is null then 'root' else chg_id end) chg_id,
lct_cd,
ExportCode
From [ShinsungTest].[autodb-2129].w_material_info
WHERE id_actual IN( select id_actual from [ShinsungTest].[autodb-2129].w_actual WHERE type = 'TIMS') OR mt_cd LIKE '%TIMS-NG%'
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_info_tims] Off;


/*[w_material_mapping_tims]*/
truncate table [ShinsungReal].[dbo].[w_material_mapping_tims];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_mapping_tims] On;
INSERT INTO [ShinsungReal].[dbo].[w_material_mapping_tims]
           (wmmid,[mt_lot]
           ,[mt_cd]
           ,[mt_no]
           ,[mapping_dt]
           ,[bb_no]
           ,[ext_no]
           ,[expiry_date]
           ,[export_date]
           ,[date_of_receipt]
           ,[lot_no]
           ,[remark]
           ,[sts_share]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
           ,[status])
Select 
	wmmid,mt_lot,mt_cd,mt_no,CONVERT(DATETIME, STUFF(STUFF(STUFF(mapping_dt,13,0,':'),11,0,':'),9,0,' ')) mapping_dt,bb_no,null ext_no,null expiry_dt,null expore_dt,'' date_of_receipt,
null lot_no, remark, sts_share, use_yn, del_yn,(case when reg_id='' or reg_id is null then 'root' else reg_id end)  reg_id, reg_dt, chg_id, chg_dt, '' status 
From [ShinsungTest].[autodb-2129].[w_material_mapping]
WHERE mt_lot NOT LIKE '%ROT%' AND mt_lot NOT LIKE '%STA%'; -- 213776
SET IDENTITY_INSERT [ShinsungReal].[dbo].[w_material_mapping_tims] Off;







/*[w_policy_mt]*/
truncate table [ShinsungReal].[dbo].[w_policy_mt];
INSERT INTO [ShinsungReal].[dbo].[w_policy_mt]
           ([policy_code]
           ,[policy_name]
           ,[policy_start_dt]
           ,[policy_end_dt]
           ,[work_starttime]
           ,[work_endtime]
           ,[lunch_start_time]
           ,[lunch_end_time]
           ,[dinner_start_time]
           ,[dinner_end_time]
           ,[work_hour]
           ,[use_yn]
           ,[last_yn]
           ,[re_mark]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
	policy_code, 
	policy_name,
	CONVERT(DATETIME, STUFF(STUFF(STUFF(policy_start_dt,13,0,':'),11,0,':'),9,0,' ')) policy_start_dt ,
	CONVERT(DATETIME, STUFF(STUFF(STUFF(policy_end_dt,13,0,':'),11,0,':'),9,0,' ')) policy_end_dt, 
concat(substring(work_starttime,1,2),':',substring(work_starttime,3,2)) work_starttime, 
concat(substring(work_endtime,1,2),':',substring(work_endtime,3,2)) work_endtime, 
concat(substring(lunch_start_time,1,2),':',substring(lunch_start_time,3,2)) lunch_start_time, 
concat(substring(lunch_end_time,1,2),':',substring(lunch_end_time,3,2)) lunch_end_time, 
concat(substring(dinner_start_time,1,2),':',substring(dinner_start_time,3,2)) dinner_start_time, 
concat(substring(dinner_end_time,1,2),':',substring(dinner_end_time,3,2)) dinner_end_time, 
 work_hour, 
 use_yn, 
 last_yn, 
 re_mark, 
 reg_id, 
 reg_dt,
 chg_id, 
 chg_dt
From [ShinsungTest].[autodb-2129].[w_policy_mt]


/*[w_product_qc]*/
truncate table [ShinsungReal].[dbo].[w_product_qc];
INSERT INTO [ShinsungReal].[dbo].[w_product_qc]
           ([pq_no]
           ,[ml_no]
           ,[work_dt]
           ,[item_vcd]
           ,[check_qty]
           ,[ok_qty]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
pq_no,ml_no,work_dt,item_vcd,check_qty,ok_qty,reg_id,reg_dt,chg_id,chg_dt
From [ShinsungTest].[autodb-2129].[w_product_qc]


/*[w_product_qc_value]*/
truncate table [ShinsungReal].[dbo].[w_product_qc_value];
INSERT INTO [ShinsungReal].[dbo].[w_product_qc_value]
           ([pq_no]
           ,[item_vcd]
           ,[check_id]
           ,[check_cd]
           ,[check_value]
           ,[check_qty]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
pq_no,item_vcd,check_id,check_cd,check_value,check_qty,reg_id,reg_dt,chg_id,chg_dt
From [ShinsungTest].[autodb-2129].[w_product_qc_value]


/*[w_rd_info]*/
truncate table [ShinsungReal].[dbo].[w_rd_info];

INSERT INTO [ShinsungReal].[dbo].[w_rd_info]
           ([rd_no]
           ,[rd_nm]
           ,[status]
           ,[lct_cd]
           ,[receiving_dt]
           ,[remark]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
[rd_no]
           ,[rd_nm]
           ,rd_sts_cd
           ,[lct_cd]
           ,[receiving_dt]
           ,[remark]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
From [ShinsungTest].[autodb-2129].[w_rd_info]





/*[w_sd_info]*/
truncate table [ShinsungReal].[dbo].[w_sd_info];

INSERT INTO [ShinsungReal].[dbo].[w_sd_info]
           ([sd_no]
           ,[sd_nm]
           ,[status]
           ,[product_cd]
           ,[lct_cd]
           ,[alert]
           ,[remark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
[sd_no]
           ,[sd_nm]
           ,sd_sts_cd
           ,[product_cd]
           ,[lct_cd]
           ,[alert]
           ,[remark]
           ,[use_yn]
           ,[del_yn]
           ,(case when reg_id='' or reg_id is null then 'root' else reg_id end) [reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
From [ShinsungTest].[autodb-2129].[w_sd_info]
--where sd_no='SD1465'
--select * from [ShinsungReal].[dbo].[w_rd_info] where [ShinsungReal].[dbo].[w_rd_info].[sd_no]='SD1645'







/*[w_vt_dt]*/
truncate table [ShinsungReal].[dbo].[w_vt_dt];

INSERT INTO [ShinsungReal].[dbo].[w_vt_dt]
           ([vn_cd]
           ,[mt_cd]
           ,[wmtid]
           ,[id_actual]
           ,[id_actual_oqc]
           ,[staff_id]
           ,[staff_id_oqc]
           ,[machine_id]
           ,[mt_type]
           ,[mt_no]
           ,[gr_qty]
           ,[real_qty]
           ,[staff_qty]
           ,[sp_cd]
           ,[rd_no]
           ,[sd_no]
           ,[ext_no]
           ,[ex_no]
           ,[dl_no]
           ,[recevice_dt]
           ,[date]
           ,[return_date]
           ,[alert_ng]
           ,[expiry_dt]
           ,[dt_of_receipt]
           ,[expore_dt]
           ,[recevice_dt_tims]
           ,[rece_wip_dt]
           ,[picking_dt]
           ,[shipping_wip_dt]
           ,[end_production_dt]
           ,[lot_no]
           ,[mt_barcode]
           ,[mt_qrcode]
           ,[status]
           ,[bb_no]
           ,[bbmp_sts_cd]
           ,[lct_cd]
           ,[lct_sts_cd]
           ,[from_lct_cd]
           ,[to_lct_cd]
           ,[output_dt]
           ,[input_dt]
           ,[buyer_qr]
           ,[orgin_mt_cd]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
[vn_cd]
           ,[mt_cd]
           ,[wmtid]
           ,[id_actual]
           ,[id_actual_oqc]
           ,[staff_id]
           ,[staff_id_oqc]
           ,[machine_id]
           ,[mt_type]
           ,[mt_no]
           ,[gr_qty]
           ,[real_qty]
           ,[staff_qty]
           ,[sp_cd]
           ,[rd_no]
           ,[sd_no]
           ,[ext_no]
           ,[ex_no]
           ,[dl_no]
           ,[recevice_dt]
           ,[date]
           ,[return_date]
           ,[alert_ng]
           ,[expiry_dt]
           ,[dt_of_receipt]
           ,[expore_dt]
           ,[recevice_dt_tims]
           ,[rece_wip_dt]
           ,[picking_dt]
           ,[shipping_wip_dt]
           ,[end_production_dt]
           ,[lot_no]
           ,[mt_barcode]
           ,[mt_qrcode]
           ,mt_sts_cd
           ,[bb_no]
           ,[bbmp_sts_cd]
           ,[lct_cd]
           ,[lct_sts_cd]
           ,[from_lct_cd]
           ,[to_lct_cd]
           ,[output_dt]
           ,[input_dt]
           ,[buyer_qr]
           ,[orgin_mt_cd]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
From [ShinsungTest].[autodb-2129].[w_vt_dt]





/*[w_vt_mt]*/
truncate table [ShinsungReal].[dbo].[w_vt_mt];

INSERT INTO [ShinsungReal].[dbo].[w_vt_mt]
           ([vn_cd]
           ,[vn_nm]
           ,[start_dt]
           ,[end_dt]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
[vn_cd]
           ,[vn_nm]
           ,[start_dt]
           ,[end_dt]
           ,[re_mark]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
From [ShinsungTest].[autodb-2129].[w_vt_mt]



/*[d_pro_unit_staff]*/
truncate table [ShinsungReal].[dbo].[d_pro_unit_staff];
SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_pro_unit_staff] On;

INSERT INTO [ShinsungReal].[dbo].[d_pro_unit_staff]
           (psid,[staff_id]
           ,[actual]
           ,[defect]
           ,[id_actual]
           ,[staff_tp]
           ,[start_dt]
           ,[end_dt]
           ,[use_yn]
           ,[del_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt])
Select 
psid, staff_id
,(case when actual='' or actual is null then 0 else actual end) actual
,(case when defect='' or defect is null then 0 else defect end) defect, id_actual, staff_tp
--,start_dt
, CONVERT(DATETIME, STUFF(STUFF(STUFF(start_dt,13,0,':'),11,0,':'),9,0,' ')) 
,CONVERT(DATETIME, STUFF(STUFF(STUFF(end_dt,13,0,':'),11,0,':'),9,0,' '))
--,end_dt
,use_yn, del_yn,(case when reg_id='' or reg_id is null then 'ROOT' else reg_id end)  reg_id, reg_dt, chg_id, chg_dt
From [ShinsungTest].[autodb-2129].[d_pro_unit_staff]
SET IDENTITY_INSERT [ShinsungReal].[dbo].[d_pro_unit_staff] Off;



/*[w_material_info_memo]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[w_material_info_memo]
INSERT INTO [ShinsungReal].[dbo].[w_material_info_memo]

           ([md_cd]
           ,[style_no]
           ,[style_nm]
           ,[mt_cd]
           ,[width]
           ,[width_unit]
           ,[spec]
           ,[spec_unit]
           ,[sd_no]
           ,[lot_no]
           ,[status]
           ,[memo]
           ,[month_excel]
           ,[receiving_dt]
           ,[tx]
           ,[total_m]
           ,[total_m2]
           ,[total_ea]
           ,[use_yn]
           ,[reg_id]
           ,[reg_dt]
           ,[chg_id]
           ,[chg_dt]
          )
     Select md_cd, style_no, style_nm, mt_cd, width, width_unit, spec, spec_unit, sd_no, lot_no, sts_cd, memo, month_excel, receiving_dt, Tx, total_m, total_m2, total_ea, use_yn, reg_id, reg_dt, chg_id, chg_dt
 From [ShinsungTest].[autodb-2129].[w_material_info_memo]
GO


/*[shippingfgsorting]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[shippingfgsorting]
INSERT INTO [ShinsungReal].[dbo].[shippingfgsorting]

            ([ShippingCode]
           ,[ProductCode]
           ,[ProductName]
           ,[IsFinish]
           ,[Description]
           ,[CreateId]
           ,[CreateDate]
           ,[ChangeId]
           ,[ChangeDate]
          )
     Select ShippingCode, ProductCode, ProductName, IsFinish, Description, CreateId, CreateDate, ChangeId, ChangeDate
 From [ShinsungTest].[autodb-2129].[shippingfgsorting]
GO


/*[shippingfgsortingdetail]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[shippingfgsortingdetail]
INSERT INTO [ShinsungReal].[dbo].[shippingfgsortingdetail]

           ([ShippingCode]
           ,[buyer_qr]
           ,[productCode]
           ,[lot_no]
           ,[Model]
           ,[location]
           ,[Quantity]
           ,[CreateId]
           ,[CreateDate]
           ,[ChangeId]
           ,[ChangeDate]
          )
     Select ShippingCode, buyer_qr, productCode, lot_no, Model, location, Quantity,CreateId, CreateDate, ChangeId,ChangeDate
 From [ShinsungTest].[autodb-2129].[shippingfgsortingdetail]
GO

/*[shippingtimssorting]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[shippingtimssorting]
INSERT INTO [ShinsungReal].[dbo].[shippingtimssorting]

           ([ShippingCode]
           ,[ProductCode]
           ,[ProductName]
           ,[IsFinish]
           ,[Description]
           ,[CreateId]
           ,[CreateDate]
           ,[ChangeId]
           ,[ChangeDate]
          )
     Select ShippingCode, ProductCode, ProductName, IsFinish, Description, CreateId, CreateDate, ChangeId,ChangeDate
 From [ShinsungTest].[autodb-2129].[shippingtimssorting]
GO
/*[shippingtimssortingdetail]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[shippingtimssortingdetail]
INSERT INTO [ShinsungReal].[dbo].[shippingtimssortingdetail]

            ([ShippingCode]
           ,[buyer_qr]
           ,[productCode]
           ,[lot_no]
           ,[Model]
           ,[location]
           ,[Quantity]
           ,[CreateId]
           ,[CreateDate]
           ,[ChangeId]
           ,[ChangeDate]
          )
     Select ShippingCode, buyer_qr,ProductCode, lot_no, Model, location, Quantity,CreateId, CreateDate, ChangeId,ChangeDate
 From [ShinsungTest].[autodb-2129].[shippingtimssortingdetail]
GO
/*[[language]]*/
USE [ShinsungReal]
GO
Truncate table [ShinsungReal].[dbo].[language]
INSERT INTO [ShinsungReal].[dbo].[language]

            ([router]
      ,[keyname]
      ,[en]
      ,[vi]
      ,[kr]
          )
     Select router, keyname,en, vi, kr
 From [ShinsungTest].[autodb-2129].[language]
GO
