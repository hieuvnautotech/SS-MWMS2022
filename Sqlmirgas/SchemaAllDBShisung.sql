
USE [ShinsungSql]

GO
/****** Object:  UserDefinedTableType [dbo].[UT_StringList]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE TYPE [dbo].[UT_StringList] AS TABLE(
	[StringValue] [varchar](100) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[Find_Remain]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
CREATE FUNCTION [dbo].[Find_Remain]
(
	-- Add the parameters for the function here
	@Mt_Cd varchar(50),
	@Origin varchar(50),
	@Reg_Date datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Remain int;
	SET @Remain = 0;

	-- Add the T-SQL statements to compute the return value here
	If (@Origin IS NULL) 
		Begin
			SET @Remain = (SELECT Top 1 gr_qty FROM w_material_info_mms WHERE orgin_mt_cd = @Mt_Cd ORDER BY reg_date DESC);
		End
	Else
		Begin
			SET @Remain = (SELECT Top 1 a.gr_qty FROM w_material_info_mms AS a WHERE a.orgin_mt_cd= @Mt_Cd AND a.reg_date> @Reg_Date ORDER BY a.reg_date DESC);
		End
	Return @Remain
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNumberTra_Wip]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
CREATE FUNCTION [dbo].[GetNumberTra_Wip]
(
	-- Add the parameters for the function here
	@Mt_Cd varchar(50)
)
RETURNS int
AS

BEGIN
DECLARE @Remain  int;
SET @Remain = 0;

IF (@Mt_Cd = '' OR @Mt_Cd IS NULL) 

	SET @Remain = 0

ELSE  

	SET @Remain =  ISNULL((SELECT COUNT(materialid) FROM inventory_products WHERE material_code LIKE CONCAT(@Mt_Cd,'-TRA','%')),0) + 1;

  
RETURN @Remain;
END
GO
/****** Object:  UserDefinedFunction [dbo].[TongSoCuonNVL]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[TongSoCuonNVL]
(
	@MaterialNo VARCHAR(200),
	@LocationCode VARCHAR(50)
)
RETURNS Float
AS
BEGIN
	DECLARE @TongSoCuon  FLOAT;
	SET @TongSoCuon = (SELECT sum(abc.Socuon) 
						FROM (
								SELECT /*a.material_code, a.gr_qty, a.mt_no , b.spec, b.bundle_unit, a.status, a.ExportCode,*/
								CASE Max(b.bundle_unit) When 'Roll' THEN ISNULL(ROUND(SUM(a.gr_qty)/MAX(b.spec), 2),0) ELSE ISNULL(Round(Sum(a.gr_qty),2),0) 
								END AS Socuon

								FROM inventory_products AS a
								 JOIN d_material_info AS b
								ON a.mt_no = b.mt_no
								WHERE a.mt_no = @MaterialNo 
								AND  a.location_code = @LocationCode
								  AND a.mt_type !='CMT' AND a.mt_type != '' AND a.mt_type IS NOT NULL AND a.active = 1  AND a.status = '001' AND (a.ExportCode IS NULL OR a.ExportCode = '')
							)AS abc);

  RETURN @TongSoCuon;
	

END

GO
/****** Object:  UserDefinedFunction [dbo].[TongSoMetNVL]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
CREATE FUNCTION [dbo].[TongSoMetNVL]
(
	@MaterialNo VARCHAR(500),
	@LocationCode VARCHAR(50)
)
RETURNS FLOAT
AS
BEGIN
	DECLARE @TongSoMet  FLOAT;
	SET @TongSoMet = (SELECT sum(abc.SoMet) 
					FROM (
					SELECT (ISNULL(SUM(a.gr_qty),0)) AS SoMet
					FROM inventory_products AS a
						JOIN d_material_info AS b
					ON a.mt_no = b.mt_no
					WHERE  a.location_code = @LocationCode
					AND a.mt_no = @MaterialNo
					AND a.mt_type !='CMT' AND a.mt_type != '' AND a.mt_type IS NOT NULL AND a.active = 1  AND a.status = '001' AND (a.ExportCode IS NULL OR a.ExportCode = '') 
						) As abc);
  
  RETURN @TongSoMet;

END
GO
/****** Object:  Table [dbo].[w_actual]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_actual](
	[id_actual] [bigint] IDENTITY(1,1) NOT NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[actual] [float] NOT NULL,
	[defect] [float] NOT NULL,
	[name] [varchar](10) NULL,
	[level] [int] NULL,
	[date] [datetime2](7) NULL,
	[don_vi_pr] [varchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[description] [varchar](500) NULL,
	[IsFinish] [int] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime] NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_w_actual_1] PRIMARY KEY CLUSTERED 
(
	[id_actual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[process_last_time]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE VIEW [dbo].[process_last_time] AS
SELECT 
        a.product AS product,
        a.actual AS actual,
        a.id_actual AS id_actual,
        a.at_no AS at_no,
        a.type AS type,
        a.name AS name,
        a.level AS level,
        a.date AS date,
        a.don_vi_pr AS don_vi_pr,
        a.item_vcd AS item_vcd,
        a.defect AS defect,
        a.reg_id AS reg_id,
        a.reg_dt AS reg_dt,
        a.chg_id AS chg_id,
        a.chg_dt AS chg_dt
    FROM
        w_actual a
    WHERE
        ((a.type = 'SX')
            AND (a.level = (SELECT 
                MAX(k.level)
            FROM
                w_actual k
            WHERE
                ((k.type = 'SX')
                    AND (a.at_no = k.at_no))
            GROUP BY k.at_no)))
GO
/****** Object:  Table [dbo].[w_actual_primary]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_actual_primary](
	[id_actualpr] [int] IDENTITY(1,1) NOT NULL,
	[at_no] [varchar](18) NULL,
	[type] [varchar](50) NULL,
	[target] [int] NOT NULL,
	[product] [varchar](50) NULL,
	[process_code] [varchar](10) NULL,
	[remark] [nvarchar](200) NULL,
	[finish_yn] [char](4) NULL,
	[isapply] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_actual_primary] PRIMARY KEY CLUSTERED 
(
	[id_actualpr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[at_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_style_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_style_info](
	[sid] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](100) NULL,
	[style_nm] [varchar](200) NULL,
	[md_cd] [varchar](50) NULL,
	[prj_nm] [varchar](100) NULL,
	[ssver] [varchar](50) NULL,
	[part_nm] [varchar](200) NULL,
	[standard] [varchar](200) NULL,
	[cust_rev] [varchar](50) NULL,
	[order_num] [varchar](50) NULL,
	[pack_amt] [int] NOT NULL,
	[cav] [varchar](50) NULL,
	[bom_type] [varchar](50) NULL,
	[tds_no] [varchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[qc_range_cd] [varchar](6) NULL,
	[stamp_code] [varchar](6) NULL,
	[expiry_month] [varchar](6) NULL,
	[expiry] [nvarchar](100) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[drawingname] [varchar](100) NULL,
	[loss] [varchar](100) NULL,
	[Description] [varchar](200) NULL,
	[active] [bit] NULL,
	[productType] [char](1) NULL,
 CONSTRAINT [pk_d_style_info] PRIMARY KEY CLUSTERED 
(
	[sid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_bom_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_bom_info](
	[bid] [int] IDENTITY(1,1) NOT NULL,
	[bom_no] [varchar](100) NULL,
	[style_no] [varchar](200) NULL,
	[mt_no] [varchar](200) NULL,
	[need_time] [float] NULL,
	[cav] [int] NULL,
	[need_m] [float] NULL,
	[buocdap] [float] NULL,
	[del_yn] [char](1) NOT NULL,
	[isapply] [char](1) NULL,
	[IsActive] [bit] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [pk_d_bom_info] PRIMARY KEY CLUSTERED 
(
	[bid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[atm_mms]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE VIEW [dbo].[atm_mms] AS
SELECT 
    v.defect AS defect,
    v.actual AS actual,
    v.at_no AS at_no,
    Max(v.id_actual) AS id_actual,
    v.product AS product,
    (SELECT d_style_info.style_nm FROM d_style_info  WHERE(d_style_info.style_no = v.product)) AS product_nm,
    v.m_lieu AS m_lieu,
    v.need_m AS need_m,
    v.reg_dt AS reg_dt,
	(case when v.m_lieu=0 then 0 else CONVERT(DECIMAL(10,2),(((v.actual * v.need_m) / v.m_lieu) * 100)) end )HS,
    --CONVERT(DECIMAL(10,2),(((v.actual * v.need_m) / v.m_lieu) * 100)) AS HS,
    v.remark AS remark
FROM(
SELECT 
    (SELECT SUM(m.defect) FROM w_actual m WHERE ((m.type = 'SX') AND (m.at_no = a.at_no))) AS defect,
    (SELECT SUM(m.actual) FROM  w_actual m WHERE ((m.at_no = a.at_no) AND (a.name = m.name))) AS actual,
	a.at_no AS at_no,
	a.id_actual AS id_actual,
	b.product AS product,
	0 AS m_lieu,
    (SELECT top 1 d_bom_info.need_m  FROM d_bom_info  WHERE (d_bom_info.style_no = b.product)) AS need_m,
    (SELECT top 1 d_style_info.style_nm FROM d_style_info WHERE (d_style_info.style_no = b.product)) AS product_nm,
	convert(datetime,b.reg_dt,121)reg_dt,
    b.remark AS remark
    FROM w_actual a
    JOIN w_actual_primary b ON (a.at_no = b.at_no)
    WHERE
            a.id_actual IN 
			(SELECT process_last_time.id_actual FROM process_last_time)
) v
--where v.at_no='PO20210627-004'
GROUP BY v.defect,v.actual,v.at_no,v.product,v.m_lieu,v.need_m,v.reg_dt,v.remark
GO
/****** Object:  Table [dbo].[d_material_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_material_info](
	[mtid] [int] IDENTITY(1,1) NOT NULL,
	[mt_type] [varchar](20) NULL,
	[mt_no] [varchar](300) NOT NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no_origin] [varchar](500) NULL,
	[mt_nm] [nvarchar](300) NULL,
	[mf_cd] [varchar](20) NULL,
	[gr_qty] [int] NULL,
	[unit_cd] [varchar](6) NULL,
	[bundle_qty] [int] NULL,
	[bundle_unit] [varchar](6) NULL,
	[sp_cd] [nvarchar](50) NULL,
	[s_lot_no] [varchar](100) NULL,
	[item_vcd] [varchar](20) NULL,
	[qc_range_cd] [varchar](100) NULL,
	[width] [varchar](20) NULL,
	[width_unit] [varchar](3) NULL,
	[spec] [varchar](20) NULL,
	[spec_unit] [varchar](6) NULL,
	[area] [varchar](20) NULL,
	[area_unit] [varchar](3) NULL,
	[thick] [varchar](20) NULL,
	[thick_unit] [varchar](3) NULL,
	[stick] [varchar](20) NULL,
	[stick_unit] [varchar](6) NULL,
	[consum_yn] [char](1) NULL,
	[price] [varchar](20) NULL,
	[tot_price] [varchar](20) NULL,
	[price_unit] [varchar](10) NULL,
	[price_least_unit] [varchar](6) NULL,
	[photo_file] [varchar](100) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[barcode] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_material_info] PRIMARY KEY CLUSTERED 
(
	[mtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product_material]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product_material](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](100) NULL,
	[process_code] [int] NULL,
	[level] [int] NOT NULL,
	[name] [nvarchar](50) NULL,
	[mt_no] [varchar](200) NULL,
	[need_time] [float] NOT NULL,
	[cav] [int] NOT NULL,
	[need_m] [float] NOT NULL,
	[buocdap] [float] NOT NULL,
	[use_yn] [varchar](5) NULL,
	[reg_id] [varchar](50) NULL,
	[reg_dt] [datetime] NOT NULL,
	[chg_id] [varchar](50) NULL,
	[chg_dt] [datetime] NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_product_material] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_info_tims]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_info_tims](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[at_no] [varchar](50) NULL,
	[product] [nvarchar](50) NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[ext_no] [nvarchar](50) NULL,
	[staff_id] [nvarchar](250) NULL,
	[staff_id_oqc] [nvarchar](100) NULL,
	[material_code] [varchar](255) NULL,
	[recevice_dt] [nvarchar](50) NULL,
	[buyer_qr] [varchar](255) NULL,
	[material_type] [varchar](255) NULL,
	[gr_qty] [int] NULL,
	[output_dt] [nvarchar](50) NULL,
	[real_qty] [int] NULL,
	[number_divide] [int] NULL,
	[status] [varchar](255) NULL,
	[sts_update] [varchar](50) NULL,
	[mt_no] [varchar](255) NULL,
	[bb_no] [varchar](255) NULL,
	[dl_no] [varchar](100) NULL,
	[sd_no] [varchar](100) NULL,
	[return_date] [nvarchar](50) NULL,
	[input_dt] [varchar](50) NULL,
	[lot_no] [varchar](250) NULL,
	[location_code] [varchar](255) NULL,
	[from_lct_code] [varchar](255) NULL,
	[orgin_mt_cd] [varchar](50) NULL,
	[to_lct_code] [varchar](255) NULL,
	[use_yn] [char](1) NULL,
	[alert_ng] [int] NULL,
	[remark] [nvarchar](250) NULL,
	[receipt_date] [datetime] NULL,
	[end_production_dt] [datetime] NULL,
	[reg_date] [datetime] NULL,
	[reg_id] [varchar](50) NULL,
	[chg_date] [datetime] NULL,
	[chg_id] [varchar](50) NULL,
	[location_number] [varchar](50) NULL,
	[ExportCode] [varchar](100) NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_w_material_info_tims_1] PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product_material_detail]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product_material_detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](255) NOT NULL,
	[process_code] [int] NOT NULL,
	[level] [int] NOT NULL,
	[name] [varchar](10) NOT NULL,
	[MaterialParent] [varchar](300) NULL,
	[MaterialNo] [varchar](300) NOT NULL,
	[CreateId] [varchar](50) NULL,
	[ChangeId] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[ChangeDate] [datetime] NULL,
 CONSTRAINT [PK_product_material_detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_info_mms]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_info_mms](
	[wmtid] [bigint] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NOT NULL,
	[material_code] [varchar](255) NULL,
	[material_type] [varchar](255) NULL,
	[gr_qty] [int] NOT NULL,
	[real_qty] [int] NOT NULL,
	[number_divide] [int] NOT NULL,
	[id_actual_oqc] [int] NULL,
	[status] [varchar](50) NULL,
	[mt_no] [varchar](255) NULL,
	[bb_no] [varchar](255) NULL,
	[sd_no] [varchar](50) NULL,
	[expiry_date] [datetime] NULL,
	[export_date] [datetime] NULL,
	[date_of_receipt] [datetime] NULL,
	[rece_wip_dt] [datetime] NULL,
	[recevice_dt_tims] [datetime] NULL,
	[lot_no] [varchar](250) NULL,
	[location_code] [varchar](255) NULL,
	[from_lct_code] [varchar](255) NULL,
	[to_lct_code] [varchar](255) NULL,
	[receipt_date] [datetime] NULL,
	[reg_date] [datetime] NULL,
	[reg_id] [varchar](50) NULL,
	[chg_date] [datetime] NULL,
	[chg_id] [varchar](50) NULL,
	[orgin_mt_cd] [varchar](100) NULL,
	[ExportCode] [varchar](50) NULL,
	[sts_update] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime] NULL,
	[description] [nvarchar](500) NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_w_material_info_mms_1] PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[inventory_products]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[inventory_products](
	[materialid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[material_code] [varchar](255) NULL,
	[recei_wip_date] [datetime] NULL,
	[picking_date] [datetime] NULL,
	[sd_no] [varchar](255) NULL,
	[ex_no] [varchar](50) NULL,
	[lct_sts_cd] [varchar](50) NULL,
	[mt_no] [varchar](250) NULL,
	[mt_type] [varchar](255) NULL,
	[gr_qty] [float] NOT NULL,
	[real_qty] [float] NOT NULL,
	[bb_no] [varchar](500) NULL,
	[orgin_mt_cd] [varchar](500) NULL,
	[recei_date] [datetime] NULL,
	[expiry_date] [datetime] NULL,
	[export_date] [datetime] NULL,
	[date_of_receipt] [datetime] NULL,
	[lot_no] [varchar](250) NULL,
	[from_lct_cd] [varchar](255) NULL,
	[location_code] [varchar](255) NULL,
	[status] [varchar](100) NULL,
	[create_id] [varchar](250) NULL,
	[create_date] [datetime] NULL,
	[change_id] [varchar](250) NULL,
	[change_date] [datetime] NULL,
	[ExportCode] [varchar](50) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[shipping_wip_dt] [datetime2](7) NULL,
	[ShippingToMachineDatetime] [datetime2](7) NULL,
	[return_date] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_inventory_products] PRIMARY KEY CLUSTERED 
(
	[materialid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[atm_tims]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[atm_tims] AS
SELECT
	ISNULL( hangdoi.wat, 0 ) AS waiting,
	x.md_cd AS model,
	x.unit_cd AS unit_cd,
	x.prj_nm AS prj_nm,
	v.waiting AS waitold,
	v.at_no AS at_no,
	v.product AS product,
	x.product_nm AS product_nm,
	v.reg_dt AS reg_dt,
	w.m_lieu AS m_lieu,
	v.need_m AS need_m,
	v.actual AS actual,
	v.id_actual AS id_actual,
	CONVERT(DECIMAL(10,2),( ( ( v.actual* v.need_m ) / w.m_lieu ) *100 )) AS HS,
	x.mt_no AS mt_no,
	x.mt_nm AS mt_nm,
	CONVERT(DECIMAL(10,2),( ( 1 / v.need_m ) * w.m_lieu )) AS actual_lt,
	u.NG AS NG,
	v.target AS target 
FROM
	(
		(
			(
				(
					(
					SELECT
						ISNULL( SUM(a.gr_qty ), 0 ) AS waiting,
						a.at_no AS at_no,
						a.product AS product,
						a.target AS target,
						CONVERT(DATE,a.reg_dt,121) as reg_dt,
						( SELECT top 1 product_material.need_m FROM product_material WHERE ( ( product_material.style_no = a.product ) AND ( product_material.active =1 ) ) ) AS need_m,
						a.actual AS actual,
						a.id_actual AS id_actual 
					FROM
						(
						SELECT
							n.gr_qty AS gr_qty,
							n.material_code AS mt_cd,
							b.product AS product,
							b.reg_dt AS reg_dt,
							a.at_no AS at_no,
							a.actual AS actual,
							a.id_actual AS id_actual,
							b.target AS target 
						FROM
							(
								( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
								LEFT JOIN w_material_info_tims n ON ( ( ( n.at_no = b.at_no ) AND ( n.location_code LIKE '006%' ) AND ( n.status IN ( '008', '002' ) ) ) ) 
							) 
						WHERE
							( ( a.type ='TIMS' ) AND ( a.IsFinish = '1' )) 
						) a 
					GROUP BY
						a.at_no ,a.product,a.target,a.reg_dt,a.actual,a.id_actual
					) v
					LEFT JOIN (
					
						select ISNULL( SUM( AA.wat ), 0 ) AS wat,
							AA.at_no AS at_no,
							AA.product AS product 
							FROM(
						SELECT
							ISNULL( ( wat.gr_qty ), 0 ) AS wat,
							waa.at_no AS at_no,
							waa.product AS product 
						FROM
							w_material_info_mms wat 
							INNER JOIN w_actual waa on wat.id_actual=waa.id_actual
						WHERE( wat.location_code = '006000000000000000' ) 
								AND ( wat.status IN( '002','008' ) --AND waa.at_no='PO20210627-031'
								AND (wat.id_actual not  in  (
											SELECT
												ac.id_actual 
											FROM
												w_actual ac 
											WHERE
												( ac.product = waa.product ) 
												AND ( ac.at_no = waa.at_no ) 
												AND ( ac.IsFinish = '1' ) 
												and ac.type='TIMS'
								) 
							))
						--GROUP BY waa.product ,waa.at_no 

						union all
						SELECT
							ISNULL( ( wat.gr_qty ), 0 ) AS wat,
							waa.at_no AS at_no,
							waa.product AS product 
						FROM
							w_material_info_tims wat 
							INNER JOIN w_actual waa on wat.id_actual=waa.id_actual
						WHERE( wat.location_code = '006000000000000000' ) 
								AND ( wat.status IN( '002','008' ) --AND waa.at_no='PO20210627-031'
								AND (wat.id_actual not  in  (
											SELECT
												ac.id_actual 
											FROM
												w_actual ac 
											WHERE
												( ac.product = waa.product ) 
												AND ( ac.at_no = waa.at_no ) 
												AND ( ac.IsFinish = '1' ) 
												and ac.type='TIMS'
								) 
							))
						--GROUP BY waa.product ,waa.at_no 
						) AA
						GROUP BY AA.product ,AA.at_no 
					) hangdoi ON ( ( ( hangdoi.at_no = v.at_no ) AND ( hangdoi.product = v.product ) ) ) 
				)
				JOIN (
				SELECT
					u.NG AS NG,
					u.at_no AS at_no 
				FROM
					(
					SELECT
						ISNULL( SUM( a.gr_qty ), 0 ) AS NG,
						a.at_no AS at_no 
					FROM
						(
						SELECT
							a.at_no AS at_no,
							n.gr_qty AS gr_qty,
							n.material_code AS mt_cd 
						FROM
							(
								( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
								LEFT JOIN w_material_info_tims n ON (
									( ( n.at_no = b.at_no ) AND ( n.material_code LIKE CONCAT ( b.at_no, '%' ) ) AND ( n.material_code LIKE '%TIMS%' ) ) 
								) 
							) 
						WHERE
							( ( a.type = 'TIMS' ) AND ( a.IsFinish = '1' ) ) 
						) a 
					GROUP BY
						a.at_no 
					) u 
				) u ON ( ( u.at_no = v.at_no ) ) 
			)
			LEFT JOIN (
--			SELECT b.at_no
--,sum(a.gr_qty) m_lieu
--FROM w_material_info_mms A
--INNER JOIN w_actual B ON A.id_actual=B.id_actual
--inner join w_actual_primary c on b.at_no=c.at_no
--WHERE   a.material_type <> 'CMT' and a.mt_no in (SELECT 
--	product_material.mt_no
--FROM
--	product_material
--WHERE (product_material.style_no =b.product)AND (product_material.use_yn = 'Y')
--		UNION ALL
--SELECT 
--	product_material_detail.MaterialNo
--FROM
--	(product_material_detail
--JOIN product_material ON (((product_material_detail.ProductCode = product_material.style_no)
--	AND (product_material_detail.MaterialParent = product_material.mt_no))))
--WHERE (product_material.style_no = b.product) AND (product_material.use_yn = 'Y')
--)

--GROUP BY b.at_no
	SELECT b.at_no
,sum(a.gr_qty) m_lieu
FROM inventory_products A
INNER JOIN w_actual B ON A.id_actual=B.id_actual
inner join w_actual_primary c on b.at_no=c.at_no
WHERE   a.mt_type <> 'CMT' and a.mt_no in (
											SELECT 
												product_material.mt_no
											FROM
												product_material
											WHERE (product_material.style_no =b.product)AND (product_material.use_yn = 'Y') AND (process_code = c.process_code)
											UNION ALL
											SELECT 
												product_material_detail.MaterialNo
											FROM
												(product_material_detail
											JOIN product_material ON (((product_material_detail.ProductCode = product_material.style_no)
												AND (product_material_detail.MaterialParent = product_material.mt_no))))
											WHERE (product_material.style_no = b.product) AND (product_material.use_yn = 'Y') AND (product_material.process_code = c.process_code) 
											)GROUP BY b.at_no
										) w ON ( ( v.at_no = w.at_no ) ) 
									)
									LEFT JOIN (
									SELECT
										d.unit_cd AS unit_cd,
										d.mt_nm AS mt_nm,
										e.at_no AS at_no,
										d.mt_no AS mt_no,
										ff.model AS model,
										ff.prj_nm AS prj_nm,
										ff.product_nm AS product_nm,
										ff.md_cd AS md_cd 
									FROM
										(
											(
												d_material_info d
												JOIN (
												SELECT
													a.id_actual AS id_actual,
													a.at_no AS at_no,
													b.type AS type,
													b.product AS product,
													bom.mt_no AS mt_no 
												FROM
													(
														( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
														JOIN product_material bom ON ( ( ( bom.style_no = b.product ) AND ( bom.use_yn = 'Y' ) AND (b.process_code = bom.process_code) ) ) 
													) 
												WHERE
													( a.type = 'TIMS' ) AND ( a.IsFinish = '1' ) 
												) e ON ( ( d.mt_no = e.mt_no ) ) 
											)
											JOIN (
											SELECT
												d.md_cd AS model,
												d.prj_nm AS prj_nm,
												d.style_nm AS product_nm,
												e.at_no AS at_no,
												d.md_cd AS md_cd 
											FROM
												(
													d_style_info d
													JOIN (
													SELECT
														a.id_actual AS id_actual,
														a.at_no AS at_no,
														b.type AS type,
														b.product AS product,
														b.process_code AS process_code,
														bom.mt_no AS mt_no 
													FROM
														(
															( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
															JOIN product_material bom ON ( ( ( bom.style_no = b.product ) AND ( bom.use_yn = 'Y' )  AND (b.process_code = bom.process_code)) ) 
														) 
													WHERE
														( a.type = 'TIMS' ) AND ( a.IsFinish = '1' ) 
													) e ON ( ( d.style_no = e.product ) ) 
												) 
											) ff ON ( ( ff.at_no = e.at_no ) ) 
										) 
									) x ON ( ( x.at_no = u.at_no ) ) 
								)

GO
/****** Object:  View [dbo].[atm_timsoqc]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE VIEW [dbo].[atm_timsoqc] AS
SELECT
	ISNULL( hangdoi.wat, 0 ) AS waiting,
	x.md_cd AS model,
	x.unit_cd AS unit_cd,
	x.prj_nm AS prj_nm,
	v.waiting AS waitold,
	v.at_no AS at_no,
	v.product AS product,
	x.product_nm AS product_nm,
	v.reg_dt AS reg_dt,
	w.m_lieu AS m_lieu,
	v.need_m AS need_m,
	v.actual AS actual,
	v.id_actual AS id_actual,
	CONVERT(DECIMAL(10,2),( ( ( v.actual* v.need_m ) / w.m_lieu ) *100 )) AS HS,
	x.mt_no AS mt_no,
	x.mt_nm AS mt_nm,
	CONVERT(DECIMAL(10,2),( ( 1 / v.need_m ) * w.m_lieu )) AS actual_lt,
	u.NG AS NG,
	v.target AS target 
FROM
	(
		(
			(
				(
					(
					SELECT
						ISNULL( SUM(a.gr_qty ), 0 ) AS waiting,
						a.at_no AS at_no,
						a.product AS product,
						a.target AS target,
						CONVERT(DATETIME,a.reg_dt,121) as reg_dt,
						( SELECT top 1 product_material.need_m FROM product_material WHERE ( ( product_material.style_no = a.product ) AND ( product_material.active =1 ) ) ) AS need_m,
						a.actual AS actual,
						a.id_actual AS id_actual 
					FROM
						(
						SELECT
							n.gr_qty AS gr_qty,
							n.material_code AS mt_cd,
							b.product AS product,
							b.reg_dt AS reg_dt,
							a.at_no AS at_no,
							a.actual AS actual,
							a.id_actual AS id_actual,
							b.target AS target 
						FROM
							(
								( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
								LEFT JOIN w_material_info_tims n ON ( ( ( n.at_no = b.at_no ) AND ( n.location_code LIKE '006%' ) AND ( n.status IN ( '008', '002' ) ) ) ) 
							) 
						WHERE
							( ( a.type ='TIMS' ) AND ( a.name = 'OQC'  )) 
						) a 
					GROUP BY
						a.at_no ,a.product,a.target,a.reg_dt,a.actual,a.id_actual
					) v
					LEFT JOIN (
					SELECT
						ISNULL( SUM( wat.gr_qty ), 0 ) AS wat,
						wat.at_no AS at_no,
						wat.product AS product 
					FROM
						w_material_info_tims wat 
					WHERE( wat.location_code = '006000000000000000' ) 
							AND ( wat.status IN( '002','008' ) and wat.at_no is not null
							AND (wat.id_actual not in  (
										SELECT TOP 1
											ac.id_actual 
										FROM
											w_actual ac 
										WHERE
											( ac.product = wat.product ) 
											AND ( ac.at_no = wat.at_no ) 
											AND ( ac.name = 'OQC' ) 
											and ac.type='TIMS'
							) 
						))
					GROUP BY wat.product ,wat.at_no 
					) hangdoi ON ( ( ( hangdoi.at_no = v.at_no ) AND ( hangdoi.product = v.product ) ) ) 
				)
				JOIN (
				SELECT
					u.NG AS NG,
					u.at_no AS at_no 
				FROM
					(
					SELECT
						ISNULL( SUM( a.gr_qty ), 0 ) AS NG,
						a.at_no AS at_no 
					FROM
						(
						SELECT
							a.at_no AS at_no,
							n.gr_qty AS gr_qty,
							n.material_code AS mt_cd 
						FROM
							(
								( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
								LEFT JOIN w_material_info_tims n ON (
									( ( n.at_no = b.at_no ) AND ( n.material_code LIKE CONCAT ( b.at_no, '%' ) ) AND ( n.material_code LIKE '%TIMS%' ) ) 
								) 
							) 
						WHERE
							( ( a.type = 'TIMS' ) AND ( a.name = 'OQC' ) ) 
						) a 
					GROUP BY
						a.at_no 
					) u 
				) u ON ( ( u.at_no = v.at_no ) ) 
			)
			LEFT JOIN (
			SELECT b.at_no,sum(a.gr_qty) m_lieu
FROM w_material_info_mms A
INNER JOIN w_actual B ON A.id_actual=B.id_actual
inner join w_actual_primary c on b.at_no=c.at_no

inner join product_material d on c.product=d.style_no and a.mt_no=d.mt_no
WHERE  d.use_yn='Y' and a.material_type <> 'CMT'

GROUP BY b.at_no
			) w ON ( ( u.at_no = w.at_no ) ) 
		)
		LEFT JOIN (
		SELECT
			d.unit_cd AS unit_cd,
			d.mt_nm AS mt_nm,
			e.at_no AS at_no,
			d.mt_no AS mt_no,
			ff.model AS model,
			ff.prj_nm AS prj_nm,
			ff.product_nm AS product_nm,
			ff.md_cd AS md_cd 
		FROM
			(
				(
					d_material_info d
					JOIN (
					SELECT
						a.id_actual AS id_actual,
						a.at_no AS at_no,
						b.type AS type,
						b.product AS product,
						bom.mt_no AS mt_no 
					FROM
						(
							( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
							JOIN product_material bom ON ( ( ( bom.style_no = b.product ) AND ( bom.use_yn = 'Y' ) ) ) 
						) 
					WHERE
						( a.type = 'TIMS' ) AND ( a.name = 'OQC' ) 
					) e ON ( ( d.mt_no = e.mt_no ) ) 
				)
				JOIN (
				SELECT
					d.md_cd AS model,
					d.prj_nm AS prj_nm,
					d.style_nm AS product_nm,
					e.at_no AS at_no,
					d.md_cd AS md_cd 
				FROM
					(
						d_style_info d
						JOIN (
						SELECT
							a.id_actual AS id_actual,
							a.at_no AS at_no,
							b.type AS type,
							b.product AS product,
							bom.mt_no AS mt_no 
						FROM
							(
								( w_actual a JOIN w_actual_primary b ON ( ( a.at_no = b.at_no ) ) )
								JOIN product_material bom ON ( ( ( bom.style_no = b.product ) AND ( bom.use_yn = 'Y' ) ) ) 
							) 
						WHERE
							( a.type = 'TIMS' ) AND ( a.name = 'OQC' ) 
						) e ON ( ( d.style_no = e.product ) ) 
					) 
				) ff ON ( ( ff.at_no = e.at_no ) ) 
			) 
		) x ON ( ( x.at_no = u.at_no ) ) 
	)
GO
/****** Object:  Table [dbo].[comm_dt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comm_dt](
	[cdid] [int] IDENTITY(1,1) NOT NULL,
	[mt_cd] [varchar](6) NOT NULL,
	[dt_cd] [varchar](200) NOT NULL,
	[dt_nm] [nvarchar](100) NOT NULL,
	[dt_kr] [varchar](50) NULL,
	[dt_vn] [varchar](50) NULL,
	[dt_exp] [nvarchar](100) NULL,
	[up_cd] [varchar](20) NULL,
	[val1] [varchar](20) NULL,
	[val1_nm] [varchar](50) NULL,
	[val2] [varchar](20) NULL,
	[val2_nm] [varchar](50) NULL,
	[val3] [varchar](20) NULL,
	[val3_nm] [varchar](50) NULL,
	[val4] [varchar](20) NULL,
	[val4_nm] [varchar](50) NULL,
	[dt_order] [int] NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[unit] [varchar](10) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_comm_dt] PRIMARY KEY CLUSTERED 
(
	[cdid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_excelwipgeneral_two]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_excelwipgeneral_two] AS
SELECT 
        a.materialid AS wmtid,
        a.material_code AS mt_cd,
        b.mt_nm AS mt_nm,
        CONCAT(ISNULL(a.gr_qty, ''),ISNULL(b.unit_cd, '')) AS lenght,
        CONCAT(ISNULL(b.width, 0),'*',ISNULL(a.gr_qty, 0)) AS size,
        ISNULL(b.spec, 0) AS spec,
        a.mt_no AS mt_no,
        CONCAT((CASE WHEN (b.bundle_unit = 'Roll') THEN ROUND((a.gr_qty / b.spec), 2) ELSE ROUND(a.gr_qty, 2) END),' ', ISNULL(b.bundle_unit, '')) AS qty,
        a.recei_date AS recevice_dt,
        (SELECT dt_nm FROM comm_dt WHERE ((comm_dt.dt_cd = a.status) AND (comm_dt.mt_cd = 'WHS005'))) AS sts_nm
    FROM
        inventory_products As a
        LEFT JOIN d_material_info b ON a.mt_no = b.mt_no
    WHERE a.location_code LIKE '002%'

GO
/****** Object:  View [dbo].[v_excelwipgeneral_one]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[v_excelwipgeneral_one] AS
  SELECT 
        a.materialid AS wmtid,
        a.material_code AS mt_cd,
        b.mt_nm AS mt_nm,
        CONCAT(SUM(a.gr_qty),
                ISNULL(b.unit_cd, '')) AS lenght,
        CONCAT(ISNULL(b.width, 0),
                '*',
                ISNULL(a.gr_qty, 0)) AS size,
        ISNULL(b.spec, 0) AS spec,
        a.mt_no AS mt_no,
        SUM((CASE
            WHEN (b.bundle_unit = 'Roll') THEN ROUND((a.gr_qty / b.spec), 2)
            ELSE ROUND(a.gr_qty, 2)
        END)) AS qty,
        b.bundle_unit AS bundle_unit,
        a.recei_date AS recevice_dt,
       (CASE
                            WHEN
                                (b.bundle_unit = 'Roll')
                            THEN
                                ((SELECT 
                                         SUM(gr_qty)
                                    FROM
                                        inventory_products
                                    WHERE
                                        ((a.status = '001')
                                            AND (a.materialid = materialid))) / b.spec)
                            ELSE (SELECT 
                                    SUM(gr_qty)
                                FROM
                                    inventory_products
                                WHERE
                                    (a.status = '001')
                                        AND (a.materialid = materialid))
                        END) AS qty2,
        (SELECT 
                Top 1 c.product
            FROM
                (w_actual_primary As c
                JOIN w_actual As d ON ((d.at_no = c.at_no)))
            WHERE
                (d.id_actual = a.id_actual)) AS product_cd,
        (SELECT 
                comm_dt.dt_nm
            FROM
                comm_dt
            WHERE
                ((comm_dt.dt_cd = a.status)
                    AND (comm_dt.mt_cd = 'WHS005'))) AS sts_nm
    FROM
        (inventory_products a
        LEFT JOIN d_material_info b ON ((a.mt_no = b.mt_no)))
    WHERE
        (a.location_code LIKE '002%')
   GROUP BY a.mt_no, a.materialid, a.material_code, b.mt_nm, b.unit_cd,b.width, a.gr_qty, b.spec, b.bundle_unit, a.recei_date, a.id_actual, a.status

GO
/****** Object:  Table [dbo].[author_action]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[author_action](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[mn_cd] [varchar](12) NULL,
	[url_link] [varchar](100) NULL,
	[id_button] [varchar](50) NULL,
	[type] [varchar](30) NULL,
	[name_table] [varchar](150) NULL,
	[sts_action] [varchar](3) NULL,
	[re_mark] [nvarchar](50) NULL,
	[active] [bit] NULL,
	[create_id] [varchar](50) NULL,
	[create_date] [datetime] NULL,
	[change_id] [varchar](50) NULL,
	[change_date] [datetime] NULL,
 CONSTRAINT [pk_author_action] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[author_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[author_info](
	[atno] [int] IDENTITY(1,1) NOT NULL,
	[at_cd] [varchar](6) NULL,
	[at_nm] [nvarchar](50) NULL,
	[role] [varchar](5) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[re_mark] [nvarchar](50) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_author_info] PRIMARY KEY CLUSTERED 
(
	[atno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[author_menu_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[author_menu_info](
	[amno] [int] IDENTITY(1,1) NOT NULL,
	[at_cd] [varchar](6) NULL,
	[mn_cd] [varchar](12) NULL,
	[mn_nm] [nvarchar](50) NULL,
	[url_link] [varchar](100) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[st_yn] [varchar](1) NULL,
	[ct_yn] [varchar](1) NULL,
	[mt_yn] [varchar](1) NULL,
	[del_yn] [varchar](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[nameen] [nvarchar](500) NULL,
	[namevi] [nvarchar](500) NULL,
	[namekr] [nvarchar](500) NULL,
	[role] [nvarchar](50) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_author_menu_info] PRIMARY KEY CLUSTERED 
(
	[amno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[buyer_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[buyer_info](
	[byno] [int] IDENTITY(1,1) NOT NULL,
	[buyer_cd] [varchar](20) NULL,
	[buyer_nm] [nvarchar](50) NULL,
	[ceo_nm] [varchar](50) NULL,
	[manager_nm] [varchar](50) NULL,
	[brd_nm] [varchar](50) NULL,
	[logo] [varchar](200) NULL,
	[phone_nb] [varchar](20) NULL,
	[cell_nb] [varchar](20) NULL,
	[fax_nb] [varchar](20) NULL,
	[e_mail] [varchar](50) NULL,
	[address] [nvarchar](200) NULL,
	[web_site] [varchar](50) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[stampid] [int] NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_buyer_info] PRIMARY KEY CLUSTERED 
(
	[byno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[comm_mt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comm_mt](
	[mt_id] [int] IDENTITY(1,1) NOT NULL,
	[div_cd] [varchar](3) NULL,
	[mt_cd] [varchar](6) NULL,
	[mt_nm] [nvarchar](50) NULL,
	[mt_exp] [nvarchar](100) NULL,
	[memo] [varchar](100) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_comm_mt] PRIMARY KEY CLUSTERED 
(
	[mt_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_bobbin_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_bobbin_info](
	[bno] [int] IDENTITY(1,1) NOT NULL,
	[mc_type] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[mt_cd] [varchar](200) NULL,
	[bb_nm] [varchar](50) NULL,
	[purpose] [nvarchar](200) NULL,
	[barcode] [varchar](50) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[count_number] [int] NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_bobbin_info] PRIMARY KEY CLUSTERED 
(
	[bno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_bobbin_lct_hist]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_bobbin_lct_hist](
	[blno] [int] IDENTITY(1,1) NOT NULL,
	[mc_type] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[mt_cd] [varchar](200) NOT NULL,
	[bb_nm] [varchar](50) NULL,
	[start_dt] [datetime2](7) NULL,
	[end_dt] [datetime2](7) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_bobbin_lct_hist] PRIMARY KEY CLUSTERED 
(
	[blno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_machine_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_machine_info](
	[mno] [int] IDENTITY(1,1) NOT NULL,
	[mc_type] [varchar](6) NULL,
	[mc_no] [varchar](20) NULL,
	[mc_nm] [nvarchar](50) NULL,
	[purpose] [varchar](200) NULL,
	[color] [varchar](20) NULL,
	[barcode] [varchar](50) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_machine_info] PRIMARY KEY CLUSTERED 
(
	[mno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_model_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_model_info](
	[mdid] [int] IDENTITY(1,1) NOT NULL,
	[md_cd] [varchar](200) NULL,
	[md_nm] [varchar](200) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_model_info] PRIMARY KEY CLUSTERED 
(
	[mdid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_mold_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_mold_info](
	[mdno] [int] IDENTITY(1,1) NOT NULL,
	[md_no] [varchar](20) NULL,
	[md_nm] [varchar](50) NULL,
	[purpose] [varchar](200) NULL,
	[barcode] [varchar](50) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_d_mold_info] PRIMARY KEY CLUSTERED 
(
	[mdno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_pro_unit_mc]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_pro_unit_mc](
	[pmid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NOT NULL,
	[start_dt] [datetime2](7) NOT NULL,
	[end_dt] [datetime2](7) NOT NULL,
	[remark] [varchar](500) NULL,
	[mc_no] [varchar](20) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_pro_unit_mc] PRIMARY KEY CLUSTERED 
(
	[pmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_pro_unit_mold]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_pro_unit_mold](
	[mdid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NOT NULL,
	[start_dt] [datetime2](7) NOT NULL,
	[end_dt] [datetime2](7) NOT NULL,
	[remark] [varchar](500) NULL,
	[md_no] [varchar](20) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_d_pro_unit_mold] PRIMARY KEY CLUSTERED 
(
	[mdid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_pro_unit_staff]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_pro_unit_staff](
	[psid] [int] IDENTITY(1,1) NOT NULL,
	[staff_id] [varchar](50) NULL,
	[actual] [float] NOT NULL,
	[defect] [float] NOT NULL,
	[id_actual] [int] NOT NULL,
	[staff_tp] [varchar](6) NULL,
	[start_dt] [datetime2](7) NOT NULL,
	[end_dt] [datetime2](7) NOT NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_pro_unit_staff] PRIMARY KEY CLUSTERED 
(
	[psid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_rounting_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_rounting_info](
	[idr] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](100) NULL,
	[process_code] [int] NULL,
	[name] [varchar](10) NULL,
	[level] [int] NULL,
	[don_vi_pr] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[description] [nvarchar](100) NULL,
	[IsFinish] [char](1) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_d_rounting_info] PRIMARY KEY CLUSTERED 
(
	[idr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[department_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[department_info](
	[dpno] [int] IDENTITY(1,1) NOT NULL,
	[depart_cd] [varchar](9) NULL,
	[depart_nm] [nvarchar](50) NULL,
	[up_depart_cd] [varchar](18) NULL,
	[level_cd] [varchar](3) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[order_no] [int] NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[mn_full] [varchar](100) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_department_info] PRIMARY KEY CLUSTERED 
(
	[dpno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExportToMachine]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExportToMachine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExportCode] [varchar](50) NOT NULL,
	[ProductCode] [nvarchar](50) NULL,
	[ProductName] [nvarchar](50) NULL,
	[MachineCode] [nvarchar](50) NULL,
	[IsFinish] [char](50) NULL,
	[Description] [nvarchar](500) NULL,
	[CreateId] [varchar](50) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[ChangeId] [varchar](50) NULL,
	[ChangeDate] [datetime2](7) NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_ExportToMachine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[generalfg]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[generalfg](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[buyer_qr] [varchar](max) NOT NULL,
	[product_code] [varchar](100) NOT NULL,
	[at_no] [varchar](100) NULL,
	[type] [varchar](10) NOT NULL,
	[md_cd] [varchar](max) NULL,
	[dl_no] [varchar](100) NULL,
	[qty] [int] NOT NULL,
	[lot_no] [varchar](100) NULL,
	[status] [varchar](5) NULL,
	[use_yn] [char](2) NULL,
	[reg_id] [varchar](50) NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](50) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_generalfg] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[language]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[language](
	[router] [varchar](50) NOT NULL,
	[keyname] [nvarchar](100) NOT NULL,
	[en] [nvarchar](max) NULL,
	[vi] [nvarchar](max) NULL,
	[kr] [nvarchar](max) NULL,
 CONSTRAINT [PK__language__8C23A7AE2AA7D74F] PRIMARY KEY CLUSTERED 
(
	[keyname] ASC,
	[router] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lct_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lct_info](
	[lctno] [int] IDENTITY(1,1) NOT NULL,
	[lct_cd] [varchar](18) NULL,
	[lct_nm] [nvarchar](50) NULL,
	[up_lct_cd] [varchar](18) NULL,
	[level_cd] [varchar](3) NULL,
	[index_cd] [varchar](3) NULL,
	[shelf_cd] [varchar](20) NULL,
	[order_no] [int] NULL,
	[real_use_yn] [char](1) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[lct_rfid] [varchar](50) NULL,
	[lct_bar_cd] [varchar](50) NULL,
	[sf_yn] [char](1) NULL,
	[is_yn] [char](1) NULL,
	[mt_yn] [char](1) NULL,
	[mv_yn] [char](1) NULL,
	[ti_yn] [char](1) NULL,
	[fg_yn] [char](1) NULL,
	[rt_yn] [char](1) NULL,
	[ft_yn] [char](1) NULL,
	[wp_yn] [char](1) NULL,
	[nt_yn] [char](1) NULL,
	[pk_yn] [char](1) NULL,
	[manager_id] [varchar](20) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[mn_full] [nvarchar](500) NULL,
	[sap_lct_cd] [varchar](20) NULL,
	[userid] [varchar](50) NULL,
	[selected] [varchar](50) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_lct_info] PRIMARY KEY CLUSTERED 
(
	[lctno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[m_board]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[m_board](
	[mno] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](2000) NOT NULL,
	[content] [varchar](max) NULL,
	[viewcnt] [int] NOT NULL,
	[replycnt] [int] NOT NULL,
	[div_cd] [char](1) NULL,
	[start_dt] [datetime2](7) NULL,
	[end_dt] [datetime2](7) NULL,
	[widthsize] [int] NOT NULL,
	[heightsize] [int] NOT NULL,
	[back_color] [varchar](20) NULL,
	[order_no] [int] NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_m_board] PRIMARY KEY CLUSTERED 
(
	[mno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[m_facline_qc]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[m_facline_qc](
	[fqno] [int] IDENTITY(1,1) NOT NULL,
	[fq_no] [varchar](11) NULL,
	[ml_no] [varchar](200) NULL,
	[ml_tims] [varchar](200) NULL,
	[product_cd] [varchar](500) NULL,
	[shift] [varchar](2) NULL,
	[at_no] [varchar](20) NULL,
	[work_dt] [varchar](30) NULL,
	[item_vcd] [varchar](200) NULL,
	[item_nm] [nvarchar](200) NULL,
	[item_exp] [nvarchar](500) NULL,
	[check_qty] [int] NOT NULL,
	[ok_qty] [int] NOT NULL,
	[ng_qty] [int] NOT NULL,
	[remain_qty] [int] NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime] NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime] NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_m_facline_qc] PRIMARY KEY CLUSTERED 
(
	[fqno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[m_facline_qc_value]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[m_facline_qc_value](
	[fqhno] [int] IDENTITY(1,1) NOT NULL,
	[fq_no] [varchar](11) NULL,
	[product] [varchar](100) NULL,
	[at_no] [varchar](100) NULL,
	[shift] [varchar](100) NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_cd] [varchar](20) NULL,
	[check_value] [nvarchar](500) NULL,
	[check_qty] [int] NOT NULL,
	[date_ymd] [varchar](10) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_m_facline_qc_value] PRIMARY KEY CLUSTERED 
(
	[fqhno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[manufac_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[manufac_info](
	[mfno] [int] IDENTITY(1,1) NOT NULL,
	[mf_cd] [varchar](50) NULL,
	[mf_nm] [nvarchar](50) NULL,
	[brd_nm] [nvarchar](50) NULL,
	[logo] [varchar](200) NULL,
	[phone_nb] [varchar](14) NULL,
	[web_site] [varchar](50) NULL,
	[address] [nvarchar](200) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_manufac_info] PRIMARY KEY CLUSTERED 
(
	[mfno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[materialbom]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[materialbom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](100) NOT NULL,
	[MaterialPrarent] [varchar](200) NOT NULL,
	[MaterialNo] [varchar](200) NOT NULL,
	[CreateId] [varchar](50) NOT NULL,
	[CreateDate] [datetime2](7)  NULL,
	[ChangeId] [varchar](50) NULL,
	[ChangeDate] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_materialbom] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mb_author_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mb_author_info](
	[mano] [int] IDENTITY(1,1) NOT NULL,
	[userid] [varchar](50) NOT NULL,
	[at_cd] [varchar](6) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_mb_author_info] PRIMARY KEY CLUSTERED 
(
	[mano] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mb_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mb_info](
	[userid] [nvarchar](50) NOT NULL,
	[uname] [nvarchar](200) NULL,
	[nick_name] [nvarchar](50) NULL,
	[upw] [varchar](50) NULL,
	[grade] [varchar](50) NULL,
	[depart_cd] [varchar](9) NULL,
	[gender] [char](1) NULL,
	[position_cd] [varchar](6) NULL,
	[tel_nb] [varchar](20) NULL,
	[cel_nb] [varchar](20) NULL,
	[e_mail] [varchar](100) NULL,
	[sms_yn] [char](1) NULL,
	[join_dt] [varchar](100) NULL,
	[birth_dt] [varchar](100) NULL,
	[scr_yn] [char](1) NULL,
	[mail_yn] [char](1) NULL,
	[join_ip] [varchar](50) NULL,
	[join_domain] [varchar](100) NULL,
	[ltacc_dt] [datetime2](7) NULL,
	[ltacc_domain] [varchar](100) NULL,
	[mbout_dt] [datetime2](7) NULL,
	[mbout_yn] [char](1) NULL,
	[accblock_yn] [char](1) NULL,
	[session_key] [varchar](50) NULL,
	[session_limit] [datetime2](7) NULL,
	[memo] [varchar](500) NULL,
	[del_yn] [char](1) NULL,
	[check_yn] [char](1) NULL,
	[rem_me] [char](1) NULL,
	[barcode] [varchar](100) NULL,
	[mbjoin_dt] [datetime2](7) NULL,
	[log_ip] [varchar](50) NULL,
	[lct_cd] [varchar](18) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[re_mark] [varchar](50) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_mb_info] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_mb_info] UNIQUE NONCLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[menu_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[menu_info](
	[mnno] [int] IDENTITY(1,1) NOT NULL,
	[mn_cd] [varchar](12) NULL,
	[mn_nm] [nvarchar](50) NULL,
	[up_mn_cd] [nvarchar](50) NULL,
	[level_cd] [varchar](3) NULL,
	[url_link] [varchar](100) NULL,
	[re_mark] [nvarchar](500) NULL,
	[col_css] [varchar](20) NULL,
	[sub_yn] [char](1) NULL,
	[order_no] [int] NOT NULL,
	[use_yn] [char](1) NULL,
	[mn_full] [nvarchar](100) NULL,
	[mn_cd_full] [nvarchar](100) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[selected] [varchar](50) NULL,
	[nameen] [nvarchar](500) NULL,
	[namevi] [nvarchar](500) NULL,
	[namekr] [nvarchar](500) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_menu_info] PRIMARY KEY CLUSTERED 
(
	[mnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[notice_board]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notice_board](
	[bno] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](2000) NULL,
	[content] [nvarchar](200) NULL,
	[mn_cd] [varchar](20) NULL,
	[viewcnt] [int] NULL,
	[replycnt] [int] NULL,
	[div_cd] [char](2) NULL,
	[lng_cd] [varchar](3) NULL,
	[start_dt] [datetime2](7) NULL,
	[end_dt] [datetime2](7) NULL,
	[widthsize] [int] NULL,
	[heightsize] [int] NULL,
	[back_color] [varchar](20) NULL,
	[order_no] [int] NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_noitice_board] PRIMARY KEY CLUSTERED 
(
	[bno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product_routing]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product_routing](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](50) NULL,
	[process_code] [int] NULL,
	[IsApply] [char](1) NULL,
	[process_name] [nvarchar](50) NULL,
	[description] [nvarchar](100) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_product_routing] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[qc_item_mt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[qc_item_mt](
	[ino] [int] IDENTITY(1,1) NOT NULL,
	[item_type] [varchar](6) NULL,
	[item_vcd] [varchar](20) NULL,
	[item_cd] [varchar](20) NULL,
	[ver] [varchar](3) NULL,
	[item_nm] [nvarchar](200) NULL,
	[item_exp] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [pk_qc_item_mt] PRIMARY KEY CLUSTERED 
(
	[ino] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[qc_itemcheck_dt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[qc_itemcheck_dt](
	[icdno] [int] IDENTITY(1,1) NOT NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_cd] [varchar](20) NULL,
	[defect_yn] [char](1) NULL,
	[check_name] [nvarchar](200) NULL,
	[order_no] [int] NOT NULL,
	[re_mark] [varchar](100) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_qc_itemcheck_dt] PRIMARY KEY CLUSTERED 
(
	[icdno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[qc_itemcheck_mt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[qc_itemcheck_mt](
	[icno] [int] IDENTITY(1,1) NOT NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_type] [varchar](6) NULL,
	[check_subject] [nvarchar](500) NULL,
	[min_value] [decimal](18, 7) NULL,
	[max_value] [decimal](18, 7) NULL,
	[range_type] [varchar](6) NULL,
	[order_no] [int] NOT NULL,
	[re_mark] [varchar](100) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_qc_itemcheck_mt] PRIMARY KEY CLUSTERED 
(
	[icno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shippingfgsorting]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shippingfgsorting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ShippingCode] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [nvarchar](50) NULL,
	[IsFinish] [char](50) NULL,
	[Description] [varchar](1000) NULL,
	[CreateId] [varchar](20) NULL,
	[CreateDate] [datetime2](0) NOT NULL,
	[ChangeId] [varchar](20) NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__shipping__3213E83F37081E8C] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [ShippingCode] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shippingfgsortingdetail]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shippingfgsortingdetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ShippingCode] [varchar](50) NULL,
	[buyer_qr] [varchar](50) NULL,
	[productCode] [varchar](50) NULL,
	[lot_no] [varchar](50) NULL,
	[Model] [varchar](50) NULL,
	[location] [varchar](50) NULL,
	[Quantity] [int] NULL,
	[CreateId] [varchar](20) NULL,
	[CreateDate] [datetime2](0) NOT NULL,
	[ChangeId] [varchar](20) NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [ShippingCodee] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC,
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shippingsdmaterial]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shippingsdmaterial](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sd_no] [varchar](100) NULL,
	[mt_no] [varchar](300) NULL,
	[quantity] [float] NOT NULL,
	[meter] [float] NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](50) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_shippingsdmaterial] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shippingtimssorting]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shippingtimssorting](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ShippingCode] [varchar](50) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [nvarchar](50) NULL,
	[IsFinish] [char](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[CreateId] [varchar](20) NULL,
	[CreateDate] [datetime2](0) NOT NULL,
	[ChangeId] [varchar](20) NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__shipping__3213E83FB0D1D541] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [ShippingCodesss] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shippingtimssortingdetail]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shippingtimssortingdetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ShippingCode] [varchar](50) NULL,
	[buyer_qr] [varchar](50) NULL,
	[productCode] [varchar](50) NULL,
	[lot_no] [varchar](50) NULL,
	[Model] [varchar](50) NULL,
	[location] [varchar](50) NULL,
	[Quantity] [int] NULL,
	[CreateId] [varchar](20) NULL,
	[CreateDate] [datetime2](0) NOT NULL,
	[ChangeId] [varchar](20) NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [ShippingCodesdds] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC,
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[stamp_detail]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stamp_detail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[buyer_qr] [varchar](50) NOT NULL,
	[stamp_code] [varchar](50) NOT NULL,
	[product_code] [varchar](50) NOT NULL,
	[ssver] [varchar](50) NULL,
	[vendor_code] [varchar](100) NULL,
	[vendor_line] [varchar](3) NULL,
	[label_printer] [varchar](10) NULL,
	[is_sample] [varchar](10) NULL,
	[pcn] [varchar](10) NULL,
	[lot_date] [varchar](23) NULL,
	[serial_number] [varchar](11) NULL,
	[machine_line] [varchar](3) NULL,
	[shift] [varchar](1) NULL,
	[standard_qty] [int] NOT NULL,
	[is_sent] [varchar](1) NULL,
	[box_code] [varchar](50) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_stamp_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[stamp_master]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stamp_master](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[stamp_code] [varchar](50) NOT NULL,
	[stamp_name] [varchar](500) NULL,
 CONSTRAINT [pk_stamp_master] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[supplier_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[supplier_info](
	[spno] [int] IDENTITY(1,1) NOT NULL,
	[sp_cd] [varchar](50) NULL,
	[sp_nm] [varchar](500) NULL,
	[bsn_tp] [varchar](10) NULL,
	[phone_nb] [varchar](50) NULL,
	[cell_nb] [varchar](50) NULL,
	[fax_nb] [varchar](500) NULL,
	[e_mail] [varchar](500) NULL,
	[web_site] [varchar](500) NULL,
	[address] [varchar](500) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_supplier_info] PRIMARY KEY CLUSTERED 
(
	[spno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempleTable]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempleTable](
	[id] [int] NOT NULL,
	[rece_wip_dt] [datetime] NULL,
	[mt_cd] [varchar](255) NOT NULL,
	[mt_no] [varchar](250) NULL,
	[lot_no] [varchar](250) NULL,
	[gr_qty] [float] NOT NULL,
	[location_code] [varchar](255) NULL,
	[expiry_dt] [datetime] NULL,
	[dt_of_receipt] [datetime] NULL,
	[expore_dt] [datetime] NULL,
	[sd_no] [varchar](255) NULL,
	[status] [varchar](100) NULL,
	[ExportCode] [varchar](50) NULL,
	[sts_nm] [nvarchar](100) NULL,
	[lct_nm] [nvarchar](50) NULL,
	[po] [varchar](18) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmp_w_material_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmp_w_material_info](
	[tmpid] [int] IDENTITY(1,1) NOT NULL,
	[mt_no] [varchar](255) NULL,
	[gr_qty] [int] NOT NULL,
	[real_qty] [int] NOT NULL,
	[mt_type] [varchar](255) NULL,
	[expiry_date] [datetime] NULL,
	[date_of_receipt] [datetime] NULL,
	[export_date] [datetime] NULL,
	[lot_no] [varchar](255) NULL,
	[status] [varchar](255) NULL,
	[reg_id] [varchar](255) NOT NULL,
	[reg_date] [datetime] NOT NULL,
	[chg_id] [varchar](255) NULL,
	[chg_date] [datetime] NULL,
	[active] [bit] NULL,
	[product_code] [varchar](50) NULL,
	[month] [int] NULL,
	[lengh] [int] NULL,
	[number_qr] [int] NULL,
	[number_qr_mapped] [varchar](250) NULL,
 CONSTRAINT [pk_tmp_w_material_info] PRIMARY KEY CLUSTERED 
(
	[tmpid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_author]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_author](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [varchar](50) NULL,
	[at_nm] [varchar](50) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_user_author] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[version_app]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[version_app](
	[id_app] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) NULL,
	[name_file] [varchar](200) NULL,
	[version] [int] NOT NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_version_app] PRIMARY KEY CLUSTERED 
(
	[id_app] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_box_mapping]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_box_mapping](
	[bmno] [int] IDENTITY(1,1) NOT NULL,
	[bx_no] [varchar](50) NULL,
	[buyer_cd] [varchar](50) NULL,
	[mt_cd] [varchar](200) NULL,
	[gr_qty] [int] NOT NULL,
	[product] [varchar](50) NULL,
	[type] [varchar](3) NULL,
	[mapping_dt] [varchar](14) NULL,
	[status] [varchar](3) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_box_mapping] PRIMARY KEY CLUSTERED 
(
	[bmno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_dl_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_dl_info](
	[dlid] [int] IDENTITY(1,1) NOT NULL,
	[dl_no] [varchar](20) NULL,
	[dl_nm] [varchar](50) NULL,
	[status] [varchar](6) NULL,
	[work_dt] [varchar](10) NULL,
	[lct_cd] [varchar](18) NULL,
	[remark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_dl_info] PRIMARY KEY CLUSTERED 
(
	[dlid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_ex_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_ex_info](
	[exid] [int] IDENTITY(1,1) NOT NULL,
	[ex_no] [varchar](20) NULL,
	[ex_nm] [nvarchar](50) NULL,
	[status] [varchar](6) NULL,
	[work_dt] [varchar](10) NULL,
	[lct_cd] [varchar](18) NULL,
	[alert] [int] NOT NULL,
	[remark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_ex_info] PRIMARY KEY CLUSTERED 
(
	[exid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_ext_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_ext_info](
	[extid] [int] IDENTITY(1,1) NOT NULL,
	[ext_no] [varchar](20) NULL,
	[ext_nm] [varchar](50) NULL,
	[status] [varchar](6) NULL,
	[work_dt] [varchar](8) NULL,
	[lct_cd] [varchar](18) NULL,
	[alert] [int] NOT NULL,
	[remark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_ext_info] PRIMARY KEY CLUSTERED 
(
	[extid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_down]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_down](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[mt_cd] [varchar](500) NULL,
	[gr_qty] [float] NOT NULL,
	[gr_down] [float] NOT NULL,
	[reason] [nvarchar](200) NULL,
	[status_now] [varchar](3) NULL,
	[bb_no] [varchar](35) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7)  NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7)  NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_material_down] PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_info_history]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_info_history](
	[wmtid_his] [int] IDENTITY(1,1) NOT NULL,
	[status] [varchar](100) NULL,
	[wmtid] [int] NOT NULL,
	[id_actual] [int] NOT NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[id_actual_oqc] [int] NOT NULL,
	[staff_id] [varchar](100) NULL,
	[staff_id_oqc] [varchar](100) NULL,
	[machine_id] [varchar](200) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NOT NULL,
	[real_qty] [float] NOT NULL,
	[staff_qty] [int] NOT NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [datetime2](7) NOT NULL,
	[return_date] [datetime2](7) NOT NULL,
	[alert_ng] [int] NOT NULL,
	[expiry_dt] [datetime2](7) NOT NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[end_production_dt] [datetime2](7) NOT NULL,
	[lot_no] [varchar](200) NULL,
	[mt_barcode] [varchar](200) NULL,
	[mt_qrcode] [varchar](200) NULL,
	[mt_sts_cd] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[bbmp_sts_cd] [varchar](6) NULL,
	[lct_cd] [varchar](50) NULL,
	[lct_sts_cd] [varchar](6) NULL,
	[from_lct_cd] [varchar](18) NULL,
	[to_lct_cd] [varchar](18) NULL,
	[output_dt] [varchar](20) NULL,
	[input_dt] [varchar](14) NULL,
	[buyer_qr] [varchar](200) NULL,
	[orgin_mt_cd] [varchar](500) NULL,
	[remark] [varchar](200) NULL,
	[sts_update] [varchar](100) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[date_insert] [varchar](25) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_material_info_history] PRIMARY KEY CLUSTERED 
(
	[wmtid_his] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_info_memo]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_info_memo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[md_cd] [varchar](200) NULL,
	[style_no] [varchar](100) NULL,
	[style_nm] [varchar](200) NULL,
	[mt_cd] [varchar](500) NULL,
	[width] [decimal](10, 0) NULL,
	[width_unit] [varchar](3) NULL,
	[spec] [decimal](10, 0) NULL,
	[spec_unit] [varchar](6) NULL,
	[sd_no] [varchar](500) NULL,
	[lot_no] [varchar](500) NULL,
	[status] [varchar](50) NULL,
	[memo] [varchar](2000) NULL,
	[month_excel] [varchar](10) NULL,
	[receiving_dt] [varchar](25) NULL,
	[tx] [int] NULL,
	[total_m] [decimal](10, 0) NULL,
	[total_m2] [decimal](10, 0) NULL,
	[total_ea] [decimal](10, 0) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [pk_w_material_info_memo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_info_tam]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_info_tam](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[staff_id] [varchar](100) NULL,
	[staff_id_oqc] [varchar](100) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[picking_dt] [varchar](25) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [datetime2](7) NULL,
	[return_date] [varchar](14) NULL,
	[alert_ng] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](14) NULL,
	[lot_no] [varchar](200) NULL,
	[mt_barcode] [varchar](200) NULL,
	[mt_qrcode] [varchar](200) NULL,
	[status] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[bbmp_sts_cd] [varchar](6) NULL,
	[lct_cd] [varchar](50) NULL,
	[lct_sts_cd] [varchar](6) NULL,
	[from_lct_cd] [varchar](18) NULL,
	[to_lct_cd] [varchar](18) NULL,
	[output_dt] [varchar](14) NULL,
	[input_dt] [varchar](14) NULL,
	[buyer_qr] [varchar](200) NULL,
	[orgin_mt_cd] [varchar](500) NULL,
	[remark] [varchar](200) NULL,
	[sts_update] [varchar](100) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_material_info_tam] PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_mapping_mms]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_mapping_mms](
	[wmmId] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](100) NOT NULL,
	[mt_cd] [varchar](100) NOT NULL,
	[mt_no] [varchar](100) NOT NULL,
	[mapping_dt] [datetime2](7) NULL,
	[use_yn] [varchar](100) NULL,
	[del_yn] [varchar](100) NULL,
	[reg_id] [varchar](100) NULL,
	[reg_date] [datetime2](7) NULL,
	[chg_id] [varchar](100) NULL,
	[chg_date] [datetime2](7) NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_w_material_mapping_mms] PRIMARY KEY CLUSTERED 
(
	[wmmId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_material_mapping_tims]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_material_mapping_tims](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](200) NULL,
	[mapping_dt] [nvarchar](50) NULL,
	[bb_no] [varchar](100) NULL,
	[ext_no] [nvarchar](50) NULL,
	[expiry_date] [nvarchar](50) NULL,
	[export_date] [nvarchar](50) NULL,
	[date_of_receipt] [datetime] NULL,
	[lot_no] [varchar](250) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](7) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[status] [varchar](50) NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_w_material_mapping_tims] PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_policy_mt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_policy_mt](
	[wid] [int] IDENTITY(1,1) NOT NULL,
	[policy_code] [varchar](6) NULL,
	[policy_name] [nvarchar](200) NULL,
	[policy_start_dt] [datetime2](7) NULL,
	[policy_end_dt] [datetime2](7) NULL,
	[work_starttime] [nvarchar](50) NULL,
	[work_endtime] [varchar](5) NULL,
	[lunch_start_time] [varchar](5) NULL,
	[lunch_end_time] [varchar](5) NULL,
	[dinner_start_time] [varchar](5) NULL,
	[dinner_end_time] [varchar](5) NULL,
	[work_hour] [decimal](4, 2) NULL,
	[use_yn] [char](1) NULL,
	[last_yn] [char](1) NULL,
	[re_mark] [nvarchar](500) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_policy_mt] PRIMARY KEY CLUSTERED 
(
	[wid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_product_qc]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_product_qc](
	[pqno] [int] IDENTITY(1,1) NOT NULL,
	[pq_no] [varchar](11) NULL,
	[ml_no] [varchar](200) NULL,
	[work_dt] [varchar](20) NULL,
	[item_vcd] [varchar](20) NULL,
	[check_qty] [int] NOT NULL,
	[ok_qty] [int] NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_product_qc] PRIMARY KEY CLUSTERED 
(
	[pqno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_product_qc_value]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_product_qc_value](
	[pqhno] [int] IDENTITY(1,1) NOT NULL,
	[pq_no] [varchar](11) NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_cd] [varchar](20) NULL,
	[check_value] [varchar](500) NULL,
	[check_qty] [int] NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_product_qc_value] PRIMARY KEY CLUSTERED 
(
	[pqhno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_rd_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_rd_info](
	[rid] [int] IDENTITY(1,1) NOT NULL,
	[rd_no] [varchar](20) NULL,
	[rd_nm] [varchar](50) NULL,
	[status] [varchar](6) NULL,
	[lct_cd] [varchar](18) NULL,
	[receiving_dt] [varchar](8) NULL,
	[remark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_rd_info] PRIMARY KEY CLUSTERED 
(
	[rid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_sd_info]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_sd_info](
	[sid] [int] IDENTITY(1,1) NOT NULL,
	[sd_no] [varchar](20) NULL,
	[sd_nm] [nvarchar](100) NULL,
	[status] [varchar](6) NULL,
	[product_cd] [varchar](50) NULL,
	[lct_cd] [varchar](18) NULL,
	[alert] [int] NOT NULL,
	[remark] [varchar](1000) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_sd_info] PRIMARY KEY CLUSTERED 
(
	[sid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_vt_dt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_vt_dt](
	[vdno] [int] IDENTITY(1,1) NOT NULL,
	[vn_cd] [varchar](20) NULL,
	[mt_cd] [varchar](200) NULL,
	[wmtid] [int] NOT NULL,
	[id_actual] [int] NOT NULL,
	[id_actual_oqc] [int] NOT NULL,
	[staff_id] [varchar](100) NULL,
	[staff_id_oqc] [varchar](100) NULL,
	[machine_id] [varchar](200) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [int] NOT NULL,
	[real_qty] [int] NOT NULL,
	[staff_qty] [int] NOT NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [datetime2](7) NOT NULL,
	[return_date] [datetime2](7) NOT NULL,
	[alert_ng] [int] NOT NULL,
	[expiry_dt] [datetime2](7) NOT NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [datetime2](7) NOT NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](7) NOT NULL,
	[lot_no] [varchar](200) NULL,
	[mt_barcode] [varchar](200) NULL,
	[mt_qrcode] [varchar](200) NULL,
	[status] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[bbmp_sts_cd] [varchar](6) NULL,
	[lct_cd] [varchar](50) NULL,
	[lct_sts_cd] [varchar](6) NULL,
	[from_lct_cd] [varchar](18) NULL,
	[to_lct_cd] [varchar](18) NULL,
	[output_dt] [varchar](14) NULL,
	[input_dt] [varchar](14) NULL,
	[buyer_qr] [varchar](200) NULL,
	[orgin_mt_cd] [varchar](500) NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_vt_dt] PRIMARY KEY CLUSTERED 
(
	[vdno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[w_vt_mt]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[w_vt_mt](
	[vno] [int] IDENTITY(1,1) NOT NULL,
	[vn_cd] [varchar](11) NULL,
	[vn_nm] [varchar](50) NULL,
	[start_dt] [datetime2](7) NOT NULL,
	[end_dt] [datetime2](7) NOT NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NOT NULL,
	[reg_dt] [datetime2](7) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](7) NULL,
	[active] [bit] NULL,
 CONSTRAINT [pk_w_vt_mt] PRIMARY KEY CLUSTERED 
(
	[vno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-150802]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-150802] ON [dbo].[comm_dt]
(
	[mt_cd] ASC,
	[dt_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220222-135853]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220222-135853] ON [dbo].[d_bobbin_info]
(
	[bb_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-145313]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-145313] ON [dbo].[d_material_info]
(
	[mt_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-152105]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-152105] ON [dbo].[generalfg]
(
	[status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220301-151105]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220301-151105] ON [dbo].[inventory_products]
(
	[material_code] ASC,
	[recei_wip_date] ASC,
	[mt_no] ASC,
	[mt_type] ASC,
	[status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220222-140038]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220222-140038] ON [dbo].[m_facline_qc]
(
	[ml_no] ASC,
	[ml_tims] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220301-151746]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220301-151746] ON [dbo].[product_material]
(
	[style_no] ASC,
	[process_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220301-151634]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220301-151634] ON [dbo].[product_material_detail]
(
	[ProductCode] ASC,
	[MaterialParent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-152810]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-152810] ON [dbo].[stamp_detail]
(
	[buyer_qr] ASC,
	[product_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220301-150448]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220301-150448] ON [dbo].[w_actual]
(
	[at_no] ASC,
	[product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220301-151909]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220301-151909] ON [dbo].[w_actual_primary]
(
	[at_no] ASC,
	[process_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-153158]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-153158] ON [dbo].[w_box_mapping]
(
	[bx_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-154730]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-154730] ON [dbo].[w_material_info_memo]
(
	[style_no] ASC,
	[mt_cd] ASC,
	[receiving_dt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220222-104503]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220222-104503] ON [dbo].[w_material_info_mms]
(
	[id_actual] ASC,
	[material_code] ASC,
	[location_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220221-162944]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220221-162944] ON [dbo].[w_material_info_tam]
(
	[mt_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220301-145820]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220301-145820] ON [dbo].[w_material_info_tims]
(
	[at_no] ASC,
	[id_actual] ASC,
	[material_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220222-135606]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220222-135606] ON [dbo].[w_material_mapping_tims]
(
	[mt_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20220217-145043]    Script Date: 3/10/2022 10:16:07 AM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20220217-145043] ON [dbo].[w_sd_info]
(
	[sd_no] ASC,
	[product_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[author_action] ADD  CONSTRAINT [DF__author_ac__isavt__25869641]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[author_action] ADD  CONSTRAINT [DF_author_action_create_date]  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[author_action] ADD  CONSTRAINT [DF_author_action_change_date]  DEFAULT (getdate()) FOR [change_date]
GO
ALTER TABLE [dbo].[author_info] ADD  CONSTRAINT [DF__author_in__use_y__267ABA7A]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[author_info] ADD  CONSTRAINT [DF__author_in__reg_d__276EDEB3]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[author_info] ADD  CONSTRAINT [DF__author_in__chg_d__286302EC]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[author_info] ADD  CONSTRAINT [DF__author_in__isact__29572725]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__use_y__2A4B4B5E]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__st_yn__2C3393D0]  DEFAULT ('Y') FOR [st_yn]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__ct_yn__2E1BDC42]  DEFAULT ('N') FOR [ct_yn]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__mt_yn__30F848ED]  DEFAULT ('N') FOR [mt_yn]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__del_y__32E0915F]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__reg_d__34C8D9D1]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__chg_d__35BCFE0A]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[author_menu_info] ADD  CONSTRAINT [DF__author_me__isact__36B12243]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__use_y__2B3F6F97]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__del_y__2D27B809]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__stamp__2F10007B]  DEFAULT ((0)) FOR [stampid]
GO
ALTER TABLE [dbo].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__reg_d__300424B4]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__chg_d__31EC6D26]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__isact__33D4B598]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF_comm_dt_mt_cd]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF_comm_dt_dt_cd]  DEFAULT (NULL) FOR [dt_cd]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF_comm_dt_dt_nm]  DEFAULT (NULL) FOR [dt_nm]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__dt_orde__3E52440B]  DEFAULT ((0)) FOR [dt_order]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__use_yn__403A8C7D]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__del_yn__4222D4EF]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF_comm_dt_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF_comm_dt_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[comm_dt] ADD  CONSTRAINT [DF_comm_dt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__use_yn__3A81B327]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[comm_mt] ADD  CONSTRAINT [DF_comm_mt_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[comm_mt] ADD  CONSTRAINT [DF_comm_mt_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[comm_mt] ADD  CONSTRAINT [DF_comm_mt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___use_y__3D5E1FD2]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___count__3F466844]  DEFAULT ((0)) FOR [count_number]
GO
ALTER TABLE [dbo].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___del_y__412EB0B6]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___reg_d__4316F928]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___chg_d__44FF419A]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___isavt__46E78A0C]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF_d_bobbin_lct_hist_mt_cd]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___start__48CFD27E]  DEFAULT ('2019-10-07 01:01:59') FOR [start_dt]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___end_d__49C3F6B7]  DEFAULT ('9999-12-31 23:59:59') FOR [end_dt]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___use_y__4CA06362]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___del_y__4D94879B]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___reg_d__4E88ABD4]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___chg_d__5070F446]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___isact__52593CB8]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF_d_bom_info_need_time]  DEFAULT ((0)) FOR [need_time]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF__d_bom_info__cav__4F7CD00D]  DEFAULT ((1)) FOR [cav]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF_d_bom_info_need_m]  DEFAULT ((0)) FOR [need_m]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF_d_bom_info_buocdap]  DEFAULT ((0)) FOR [buocdap]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF__d_bom_inf__del_y__5165187F]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF__d_bom_inf__isapp__534D60F1]  DEFAULT ('N') FOR [isapply]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF_d_bom_info_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF_d_bom_info_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[d_bom_info] ADD  CONSTRAINT [DF__d_bom_inf__isact__5441852A]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__use_y__5535A963]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__del_y__571DF1D5]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__reg_d__59063A47]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__chg_d__59FA5E80]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__isact__5AEE82B9]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF_d_material_info_mt_no]  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__gr_qt__5CD6CB2B]  DEFAULT ((0)) FOR [gr_qty]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__unit___60A75C0F]  DEFAULT ('EA') FOR [unit_cd]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__bundl__619B8048]  DEFAULT ((0)) FOR [bundle_qty]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__bundl__6383C8BA]  DEFAULT ('Roll') FOR [bundle_unit]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__consu__6477ECF3]  DEFAULT ('N') FOR [consum_yn]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__use_y__656C112C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__del_y__6E01572D]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF_d_material_info_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__reg_d__6EF57B66]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__chg_d__6FE99F9F]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_material_info] ADD  CONSTRAINT [DF__d_materia__isact__70DDC3D8]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__use_y__5EBF139D]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__del_y__5FB337D6]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_model_info] ADD  CONSTRAINT [DF_d_model_info_reg_id]  DEFAULT ('') FOR [reg_id]
GO
ALTER TABLE [dbo].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__reg_d__6754599E]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__chg_d__693CA210]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__isact__6B24EA82]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_mold_info] ADD  CONSTRAINT [DF_d_mold_info_use_yn]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_mold_info] ADD  CONSTRAINT [DF_d_mold_info_del_yn]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_mold_info] ADD  CONSTRAINT [DF_d_mold_info_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[d_mold_info] ADD  CONSTRAINT [DF_d_mold_info_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_mold_info] ADD  CONSTRAINT [DF_d_mold_info_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_mold_info] ADD  CONSTRAINT [DF_d_mold_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF_d_pro_unit_mc_id_actual]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__start__72C60C4A]  DEFAULT ('2019-01-01 00:00:01') FOR [start_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__end_d__73BA3083]  DEFAULT ('9999-12-31 23:59:59') FOR [end_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__use_y__787EE5A0]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__del_y__7A672E12]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__reg_d__7C4F7684]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__chg_d__7E37BEF6]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__isact__7F2BE32F]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_id_actual]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_start_dt]  DEFAULT ('2019-01-01 00:00:01') FOR [start_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_end_dt]  DEFAULT ('9999-12-31 23:59:59') FOR [end_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_use_yn]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_del_yn]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_mold] ADD  CONSTRAINT [DF_d_pro_unit_mold_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF_d_pro_unit_staff_actual]  DEFAULT ((0)) FOR [actual]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF_d_pro_unit_staff_defect]  DEFAULT ((0)) FOR [defect]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF_d_pro_unit_staff_id_actual]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF__d_pro_uni__start__76969D2E]  DEFAULT ('2019-01-01 00:00:01') FOR [start_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF__d_pro_uni__end_d__778AC167]  DEFAULT ('9999-12-31 23:59:59') FOR [end_dt]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF__d_pro_uni__use_y__01142BA1]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF__d_pro_uni__del_y__02084FDA]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF_d_pro_unit_staff_reg_id]  DEFAULT ('root') FOR [reg_id]
GO
ALTER TABLE [dbo].[d_pro_unit_staff] ADD  CONSTRAINT [DF__d_pro_uni__isact__04E4BC85]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__reg_d__797309D9]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_rounting_info] ADD  CONSTRAINT [DF_d_rounting_info_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__chg_d__7B5B524B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__isact__7D439ABD]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF_d_style_info_pack_amt]  DEFAULT ((0)) FOR [pack_amt]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__use_y__05D8E0BE]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__del_y__07C12930]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF_d_style_info_reg_id]  DEFAULT ('root') FOR [reg_id]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__reg_d__08B54D69]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__chg_d__09A971A2]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__isact__0A9D95DB]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__produ__19AACF41]  DEFAULT ('0') FOR [productType]
GO
ALTER TABLE [dbo].[department_info] ADD  CONSTRAINT [DF__departmen__use_y__0B91BA14]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[department_info] ADD  CONSTRAINT [DF__departmen__del_y__0C85DE4D]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[department_info] ADD  CONSTRAINT [DF__departmen__reg_d__0D7A0286]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[department_info] ADD  CONSTRAINT [DF__departmen__chg_d__0E6E26BF]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[department_info] ADD  CONSTRAINT [DF__departmen__isact__0F624AF8]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[ExportToMachine] ADD  CONSTRAINT [DF_ExportToMachine_ExportCode]  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [dbo].[ExportToMachine] ADD  CONSTRAINT [DF_ExportToMachine_CreateId]  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [dbo].[ExportToMachine] ADD  CONSTRAINT [DF_ExportToMachine_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ExportToMachine] ADD  CONSTRAINT [DF_ExportToMachine_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF_generalfg_buyer_qr]  DEFAULT ('') FOR [buyer_qr]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF_generalfg_product_code]  DEFAULT ('') FOR [product_code]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF_generalfg_type]  DEFAULT ('') FOR [type]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF__generalfg__qty__114A936A]  DEFAULT ((0)) FOR [qty]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF_generalfg_status]  DEFAULT ('') FOR [status]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF__generalfg__use_y__123EB7A3]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF__generalfg__reg_d__1332DBDC]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF__generalfg__chg_d__14270015]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[generalfg] ADD  CONSTRAINT [DF__generalfg__isact__151B244E]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_material_code]  DEFAULT (NULL) FOR [material_code]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_recei_wip_date]  DEFAULT (getdate()) FOR [recei_wip_date]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_ex_no]  DEFAULT ('') FOR [ex_no]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_gr_qty]  DEFAULT ((0)) FOR [gr_qty]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_real_qty]  DEFAULT ((0)) FOR [real_qty]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_active]  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_create_id]  DEFAULT ('') FOR [create_id]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_change_id]  DEFAULT ('') FOR [change_id]
GO
ALTER TABLE [dbo].[inventory_products] ADD  CONSTRAINT [DF_inventory_products_active_1]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__order___17F790F9]  DEFAULT ((1)) FOR [order_no]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__real_u__18EBB532]  DEFAULT ('N') FOR [real_use_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__use_yn__19DFD96B]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__sf_yn__1AD3FDA4]  DEFAULT ('N') FOR [sf_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__is_yn__1DB06A4F]  DEFAULT ('N') FOR [is_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__mt_yn__1EA48E88]  DEFAULT ('N') FOR [mt_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__mv_yn__1F98B2C1]  DEFAULT ('N') FOR [mv_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__ti_yn__208CD6FA]  DEFAULT ('N') FOR [ti_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__fg_yn__2180FB33]  DEFAULT ('N') FOR [fg_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__rt_yn__2A164134]  DEFAULT ('N') FOR [rt_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__ft_yn__2B0A656D]  DEFAULT ('N') FOR [ft_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__wp_yn__2BFE89A6]  DEFAULT ('N') FOR [wp_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__nt_yn__2CF2ADDF]  DEFAULT ('N') FOR [nt_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__pk_yn__2EDAF651]  DEFAULT ('N') FOR [pk_yn]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__reg_dt__2FCF1A8A]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__chg_dt__30C33EC3]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[lct_info] ADD  CONSTRAINT [DF__lct_info__isaciv__31B762FC]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__viewcnt__1CBC4616]  DEFAULT ((0)) FOR [viewcnt]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__replycn__22751F6C]  DEFAULT ((0)) FOR [replycnt]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__div_cd__236943A5]  DEFAULT ('A') FOR [div_cd]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__back_co__245D67DE]  DEFAULT ('#FFFFFF') FOR [back_color]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__order_n__25518C17]  DEFAULT ((0)) FOR [order_no]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__del_yn__2645B050]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__reg_dt__2DE6D218]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__chg_dt__3587F3E0]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[m_board] ADD  CONSTRAINT [DF__m_board__isactiv__367C1819]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__check__282DF8C2]  DEFAULT ((1)) FOR [check_qty]
GO
ALTER TABLE [dbo].[m_facline_qc] ADD  DEFAULT ((0)) FOR [ok_qty]
GO
ALTER TABLE [dbo].[m_facline_qc] ADD  DEFAULT ((0)) FOR [ng_qty]
GO
ALTER TABLE [dbo].[m_facline_qc] ADD  DEFAULT ((0)) FOR [remain_qty]
GO
ALTER TABLE [dbo].[m_facline_qc] ADD  CONSTRAINT [DF_m_facline_qc_reg_id]  DEFAULT ('root') FOR [reg_id]
GO
ALTER TABLE [dbo].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__isact__3493CFA7]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__check__3864608B]  DEFAULT ((1)) FOR [check_qty]
GO
ALTER TABLE [dbo].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__reg_d__395884C4]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__chg_d__3A4CA8FD]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[m_facline_qc_value] ADD  CONSTRAINT [DF_m_facline_qc_value_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__use_y__3D2915A8]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__del_y__3F115E1A]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__reg_d__40F9A68C]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__chg_d__42E1EEFE]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[manufac_info] ADD  CONSTRAINT [DF_manufac_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[materialbom] ADD  CONSTRAINT [DF__materialb__produ__3E1D39E1]  DEFAULT ('0') FOR [ProductCode]
GO
ALTER TABLE [dbo].[materialbom] ADD  CONSTRAINT [DF__materialb__mater__40058253]  DEFAULT ('0') FOR [MaterialPrarent]
GO
ALTER TABLE [dbo].[materialbom] ADD  CONSTRAINT [DF__materialb__chang__41EDCAC5]  DEFAULT ('0') FOR [CreateDate]
GO
ALTER TABLE [dbo].[materialbom] ADD  CONSTRAINT [DF__materialb__creat__43D61337]  DEFAULT ('0001-01-01') FOR [ChangeId]
GO
ALTER TABLE [dbo].[materialbom] ADD  CONSTRAINT [DF__materialb__chang__44CA3770]  DEFAULT ('0001-01-01') FOR [ChangeDate]
GO
ALTER TABLE [dbo].[materialbom] ADD  CONSTRAINT [DF_materialbom_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__use_y__46B27FE2]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__reg_d__47A6A41B]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__chg_d__489AC854]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[mb_author_info] ADD  CONSTRAINT [DF_mb_author_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF_mb_info_depart_cd]  DEFAULT (NULL) FOR [depart_cd]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__gender__4A8310C6]  DEFAULT ('M') FOR [gender]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__sms_yn__4B7734FF]  DEFAULT ('N') FOR [sms_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__scr_yn__4C6B5938]  DEFAULT ('N') FOR [scr_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__mail_yn__4D5F7D71]  DEFAULT ('N') FOR [mail_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__ltacc_d__4E53A1AA]  DEFAULT (getdate()) FOR [ltacc_dt]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__mbout_d__4F47C5E3]  DEFAULT (getdate()) FOR [mbout_dt]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__mbout_y__503BEA1C]  DEFAULT ('N') FOR [mbout_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__accbloc__51300E55]  DEFAULT ('N') FOR [accblock_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__session__5224328E]  DEFAULT ('none') FOR [session_key]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__session__531856C7]  DEFAULT (getdate()) FOR [session_limit]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__del_yn__540C7B00]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__check_y__55009F39]  DEFAULT ('N') FOR [check_yn]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__mbjoin___55F4C372]  DEFAULT (getdate()) FOR [mbjoin_dt]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__reg_dt__56E8E7AB]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF__mb_info__chg_dt__57DD0BE4]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[mb_info] ADD  CONSTRAINT [DF_mb_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF__menu_info__col_c__607251E5]  DEFAULT ('fa-th') FOR [col_css]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF__menu_info__sub_y__6166761E]  DEFAULT ('N') FOR [sub_yn]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF__menu_info__order__625A9A57]  DEFAULT ((0)) FOR [order_no]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF__menu_info__use_y__634EBE90]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF__menu_info__reg_d__6442E2C9]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF__menu_info__chg_d__681373AD]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[menu_info] ADD  CONSTRAINT [DF_menu_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[notice_board] ADD  CONSTRAINT [DF_notice_board_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[product_material] ADD  CONSTRAINT [DF_product_material_cav]  DEFAULT ((1)) FOR [cav]
GO
ALTER TABLE [dbo].[product_material] ADD  CONSTRAINT [DF_product_material_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[product_material] ADD  CONSTRAINT [DF_product_material_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[product_material] ADD  CONSTRAINT [DF_product_material_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[product_material_detail] ADD  CONSTRAINT [DF_product_material_detail_level]  DEFAULT ((0)) FOR [level]
GO
ALTER TABLE [dbo].[product_material_detail] ADD  CONSTRAINT [DF_product_material_detail_MaterialParent]  DEFAULT ((0)) FOR [MaterialParent]
GO
ALTER TABLE [dbo].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__use_y__72910220]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__del_y__73852659]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__reg_d__74794A92]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__chg_d__756D6ECB]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[qc_item_mt] ADD  CONSTRAINT [DF_qc_item_mt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__defec__7755B73D]  DEFAULT ('Y') FOR [defect_yn]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__order__7849DB76]  DEFAULT ((1)) FOR [order_no]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__use_y__793DFFAF]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__del_y__7A3223E8]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__reg_d__7B264821]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__chg_d__7C1A6C5A]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[qc_itemcheck_dt] ADD  CONSTRAINT [DF_qc_itemcheck_dt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__min_v__7E02B4CC]  DEFAULT ((0.0000000)) FOR [min_value]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__max_v__7EF6D905]  DEFAULT ((0.0000000)) FOR [max_value]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__order__7FEAFD3E]  DEFAULT ((1)) FOR [order_no]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__use_y__00DF2177]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__del_y__02C769E9]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__reg_d__03BB8E22]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__chg_d__0A688BB1]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[qc_itemcheck_mt] ADD  CONSTRAINT [DF_qc_itemcheck_mt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Shipp__3E3D3572]  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Produ__3F3159AB]  DEFAULT (NULL) FOR [ProductCode]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Produ__40257DE4]  DEFAULT (NULL) FOR [ProductName]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__IsFin__4119A21D]  DEFAULT (NULL) FOR [IsFinish]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Descr__420DC656]  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Creat__4301EA8F]  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Creat__43F60EC8]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Chang__44EA3301]  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [dbo].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Chang__45DE573A]  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [productCode]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [Model]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [location]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [Quantity]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [dbo].[shippingfgsortingdetail] ADD  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [dbo].[shippingsdmaterial] ADD  CONSTRAINT [DF_shippingsdmaterial_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[shippingsdmaterial] ADD  CONSTRAINT [DF_shippingsdmaterial_meter]  DEFAULT ((0)) FOR [meter]
GO
ALTER TABLE [dbo].[shippingsdmaterial] ADD  CONSTRAINT [DF_shippingsdmaterial_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[shippingsdmaterial] ADD  CONSTRAINT [DF__shippings__reg_d__078C1F06]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[shippingsdmaterial] ADD  CONSTRAINT [DF_shippingsdmaterial_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[shippingsdmaterial] ADD  CONSTRAINT [DF_shippingsdmaterial_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Shipp__5708E33C]  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Produ__57FD0775]  DEFAULT (NULL) FOR [ProductCode]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Produ__58F12BAE]  DEFAULT (NULL) FOR [ProductName]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__IsFin__59E54FE7]  DEFAULT (NULL) FOR [IsFinish]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Descr__5AD97420]  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Creat__5BCD9859]  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Creat__5CC1BC92]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Chang__5DB5E0CB]  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [dbo].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Chang__5EAA0504]  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [productCode]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [Model]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [location]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [Quantity]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [dbo].[shippingtimssortingdetail] ADD  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__vendo__05A3D694]  DEFAULT ('DZIH') FOR [vendor_code]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__vendo__0697FACD]  DEFAULT ('A') FOR [vendor_line]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__label__0880433F]  DEFAULT ('1') FOR [label_printer]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__is_sa__09746778]  DEFAULT ('N') FOR [is_sample]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_detai__pcn__0B5CAFEA]  DEFAULT ('0') FOR [pcn]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__seria__0C50D423]  DEFAULT ('001') FOR [serial_number]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__machi__0D44F85C]  DEFAULT ('01') FOR [machine_line]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__shift__0F2D40CE]  DEFAULT ('0') FOR [shift]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__stand__10216507]  DEFAULT ((0)) FOR [standard_qty]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__is_se__11158940]  DEFAULT ('N') FOR [is_sent]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF_stamp_detail_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__reg_d__1209AD79]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF__stamp_det__chg_d__13F1F5EB]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[stamp_detail] ADD  CONSTRAINT [DF_stamp_detail_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[stamp_master] ADD  CONSTRAINT [DF_stamp_master_stamp_code]  DEFAULT (NULL) FOR [stamp_code]
GO
ALTER TABLE [dbo].[supplier_info] ADD  CONSTRAINT [DF__supplier___use_y__14E61A24]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[supplier_info] ADD  CONSTRAINT [DF__supplier___del_y__16CE6296]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[supplier_info] ADD  CONSTRAINT [DF_supplier_info_reg_id]  DEFAULT ('root') FOR [reg_id]
GO
ALTER TABLE [dbo].[supplier_info] ADD  CONSTRAINT [DF__supplier___reg_d__17C286CF]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[supplier_info] ADD  CONSTRAINT [DF__supplier___chg_d__18B6AB08]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[supplier_info] ADD  CONSTRAINT [DF_supplier_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_gr_qty]  DEFAULT ((0)) FOR [gr_qty]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_real_qty]  DEFAULT ((0)) FOR [real_qty]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_expiry_date]  DEFAULT (getdate()) FOR [expiry_date]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_date_of_receipt]  DEFAULT (getdate()) FOR [date_of_receipt]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_export_date]  DEFAULT (getdate()) FOR [export_date]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_reg_date]  DEFAULT (getdate()) FOR [reg_date]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_chg_date]  DEFAULT (getdate()) FOR [chg_date]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_month]  DEFAULT ((0)) FOR [month]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_lengh]  DEFAULT ((0)) FOR [lengh]
GO
ALTER TABLE [dbo].[tmp_w_material_info] ADD  CONSTRAINT [DF_tmp_w_material_info_number_qr]  DEFAULT ((0)) FOR [number_qr]
GO
ALTER TABLE [dbo].[user_author] ADD  CONSTRAINT [DF__user_auth__chg_d__1C873BEC]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[user_author] ADD  CONSTRAINT [DF_user_author_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[version_app] ADD  CONSTRAINT [DF__version_a__versi__1F63A897]  DEFAULT ((0)) FOR [version]
GO
ALTER TABLE [dbo].[version_app] ADD  CONSTRAINT [DF__version_a__chg_d__2057CCD0]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[version_app] ADD  CONSTRAINT [DF_version_app_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[version_app] ADD  CONSTRAINT [DF_version_app_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_actual] ADD  CONSTRAINT [DF_w_actual_actual]  DEFAULT ((0)) FOR [actual]
GO
ALTER TABLE [dbo].[w_actual] ADD  CONSTRAINT [DF_w_actual_defect]  DEFAULT ((0)) FOR [defect]
GO
ALTER TABLE [dbo].[w_actual] ADD  CONSTRAINT [DF_w_actual_IsFinish]  DEFAULT ((0)) FOR [IsFinish]
GO
ALTER TABLE [dbo].[w_actual] ADD  CONSTRAINT [DF__w_actual__reg_dt__214BF109]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_actual] ADD  CONSTRAINT [DF__w_actual__chg_dt__2334397B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_actual] ADD  CONSTRAINT [DF_w_actual_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF_w_actual_primary_target]  DEFAULT ((0)) FOR [target]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___finis__24285DB4]  DEFAULT ('N') FOR [finish_yn]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___isapp__251C81ED]  DEFAULT ('N') FOR [isapply]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF_w_actual_primary_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF_w_actual_primary_reg_dt]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF_w_actual_primary_chg_dt]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_actual_primary] ADD  CONSTRAINT [DF_w_actual_primary_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__gr_qt__29E1370A]  DEFAULT ((0)) FOR [gr_qty]
GO
ALTER TABLE [dbo].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__use_y__2BC97F7C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__del_y__2DB1C7EE]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__reg_d__2F9A1060]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__chg_d__308E3499]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_box_mapping] ADD  CONSTRAINT [DF_w_box_mapping_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__use_y__2AD55B43]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__reg_d__2CBDA3B5]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__chg_d__2EA5EC27]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_dl_info] ADD  CONSTRAINT [DF_w_dl_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__alert__345EC57D]  DEFAULT ((0)) FOR [alert]
GO
ALTER TABLE [dbo].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__use_y__36470DEF]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__reg_d__382F5661]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__chg_d__3A179ED3]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_ex_info] ADD  CONSTRAINT [DF_w_ex_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__alert__3552E9B6]  DEFAULT ((0)) FOR [alert]
GO
ALTER TABLE [dbo].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__use_y__373B3228]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__reg_d__3CF40B7E]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__chg_d__3DE82FB7]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_ext_info] ADD  CONSTRAINT [DF_w_ext_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_down] ADD  CONSTRAINT [DF__w_materia__use_y__39237A9A]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_material_down] ADD  CONSTRAINT [DF__w_materia__reg_d__3B0BC30C]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_material_down] ADD  CONSTRAINT [DF__w_materia__chg_d__3BFFE745]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_material_down] ADD  CONSTRAINT [DF_w_material_down_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__id_ac__4D2A7347]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__id_ac__4E1E9780]  DEFAULT ((0)) FOR [id_actual_oqc]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__alert__4F12BBB9]  DEFAULT ((0)) FOR [alert_ng]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_st__50FB042B]  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__bbmp___52E34C9D]  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__use_y__53D770D6]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__reg_d__55BFB948]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__chg_d__589C25F3]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_material_info_history] ADD  CONSTRAINT [DF_w_material_info_history_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_info_memo] ADD  CONSTRAINT [DF_w_material_info_memo_tx]  DEFAULT ((0)) FOR [tx]
GO
ALTER TABLE [dbo].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__use_y__4A4E069C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_material_info_memo] ADD  CONSTRAINT [DF_w_material_info_memo_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__reg_d__4B422AD5]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__chg_d__4C364F0E]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_material_info_memo] ADD  CONSTRAINT [DF_w_material_info_memo_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_id_actual]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_gr_qty]  DEFAULT ((0)) FOR [gr_qty]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_real_qty]  DEFAULT ((0)) FOR [real_qty]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_number_divide]  DEFAULT ((0)) FOR [number_divide]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_id_actual_oqc]  DEFAULT ((0)) FOR [id_actual_oqc]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_reg_date]  DEFAULT (getdate()) FOR [reg_date]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_chg_date]  DEFAULT (getdate()) FOR [chg_date]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_ShippingToMachineDatetime]  DEFAULT (getdate()) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [dbo].[w_material_info_mms] ADD  CONSTRAINT [DF_w_material_info_mms_Active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__id_ac__5D60DB10]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__id_ac__5E54FF49]  DEFAULT ((0)) FOR [id_actual_oqc]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF_w_material_info_tam_date]  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__alert__6501FCD8]  DEFAULT ((0)) FOR [alert_ng]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_st__66EA454A]  DEFAULT ('000') FOR [status]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__bbmp___67DE6983]  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__use_y__68D28DBC]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__reg_d__69C6B1F5]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__chg_d__6BAEFA67]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_material_info_tam] ADD  CONSTRAINT [DF_w_material_info_tam_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_gr_qty]  DEFAULT ((0)) FOR [gr_qty]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_real_qty]  DEFAULT ((0)) FOR [real_qty]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_alert_ng]  DEFAULT ((0)) FOR [alert_ng]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_receipt_date]  DEFAULT (getdate()) FOR [receipt_date]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_create_date]  DEFAULT (getdate()) FOR [reg_date]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_change_date]  DEFAULT (getdate()) FOR [chg_date]
GO
ALTER TABLE [dbo].[w_material_info_tims] ADD  CONSTRAINT [DF_w_material_info_tims_Active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_mt_lot]  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_mt_cd]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_mt_no]  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_reg_date]  DEFAULT (getdate()) FOR [reg_date]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_chg_id]  DEFAULT (getdate()) FOR [chg_id]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_chg_date]  DEFAULT (getdate()) FOR [chg_date]
GO
ALTER TABLE [dbo].[w_material_mapping_mms] ADD  CONSTRAINT [DF_w_material_mapping_mms_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_material_mapping_tims] ADD  CONSTRAINT [DF__w_materia__use_y__54CB950F]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_material_mapping_tims] ADD  CONSTRAINT [DF__w_materia__del_y__57A801BA]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[w_material_mapping_tims] ADD  CONSTRAINT [DF__w_materia__chg_d__5A846E65]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_material_mapping_tims] ADD  CONSTRAINT [DF_w_material_mapping_tims_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___work___5F492382]  DEFAULT ((0.00)) FOR [work_hour]
GO
ALTER TABLE [dbo].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___use_y__61316BF4]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___last___6319B466]  DEFAULT ('Y') FOR [last_yn]
GO
ALTER TABLE [dbo].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___reg_d__6CA31EA0]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___chg_d__6D9742D9]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_policy_mt] ADD  CONSTRAINT [DF_w_policy_mt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_product_qc] ADD  CONSTRAINT [DF__w_product__check__603D47BB]  DEFAULT ((0)) FOR [check_qty]
GO
ALTER TABLE [dbo].[w_product_qc] ADD  CONSTRAINT [DF__w_product__ok_qt__6225902D]  DEFAULT ((0)) FOR [ok_qty]
GO
ALTER TABLE [dbo].[w_product_qc] ADD  CONSTRAINT [DF__w_product__reg_d__640DD89F]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_product_qc] ADD  CONSTRAINT [DF__w_product__chg_d__65F62111]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_product_qc] ADD  CONSTRAINT [DF_w_product_qc_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_product_qc_value] ADD  CONSTRAINT [DF__w_product__check__6E8B6712]  DEFAULT ((0)) FOR [check_qty]
GO
ALTER TABLE [dbo].[w_product_qc_value] ADD  CONSTRAINT [DF__w_product__reg_d__6F7F8B4B]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_product_qc_value] ADD  CONSTRAINT [DF__w_product__chg_d__7073AF84]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_product_qc_value] ADD  CONSTRAINT [DF_w_product_qc_value_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__use_y__725BF7F6]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__reg_d__753864A1]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__chg_d__762C88DA]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_rd_info] ADD  CONSTRAINT [DF_w_rd_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__alert__7720AD13]  DEFAULT ((0)) FOR [alert]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__use_y__7814D14C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__del_y__79FD19BE]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF_w_sd_info_reg_id]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__reg_d__02925FBF]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__chg_d__047AA831]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_sd_info] ADD  CONSTRAINT [DF_w_sd_info_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__id_actu__7908F585]  DEFAULT ((0)) FOR [id_actual]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__id_actu__7AF13DF7]  DEFAULT ((0)) FOR [id_actual_oqc]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__alert_n__7BE56230]  DEFAULT ((0)) FOR [alert_ng]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__mt_sts___7DCDAAA2]  DEFAULT ('000') FOR [status]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__bbmp_st__7EC1CEDB]  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__reg_dt__00AA174D]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF__w_vt_dt__chg_dt__038683F8]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_vt_dt] ADD  CONSTRAINT [DF_w_vt_dt_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__use_yn__7FB5F314]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [dbo].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__del_yn__019E3B86]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [dbo].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__reg_dt__056ECC6A]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [dbo].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__chg_dt__0662F0A3]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [dbo].[w_vt_mt] ADD  CONSTRAINT [DF_w_vt_mt_active]  DEFAULT ((1)) FOR [active]
GO
/****** Object:  StoredProcedure [dbo].[CreateBuyerQR]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateBuyerQR]
	-- Add the parameters for the stored procedure here
	@ListBuyerQR varchar(max)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

	SELECT *
	INTO #listBuyerQR
	FROM OPENJSON(@ListBuyerQR)
	WITH 
	(
		Buyer_Qr varchar(100) '$.buyer_qr',
		Stamp_Code varchar(5) '$.stamp_code',
		Product_Code varchar(100) '$.product_code',
		SSVER varchar(5) '$.ssver',
		Vendor_Code varchar(50) '$.vendor_code',
		Vendor_Line varchar(50) '$.vendor_line',
		Label_Printer varchar(50) '$.label_printer',
		Is_Sample varchar(100) '$.is_sample',
		PCN varchar(50) '$.pcn',
		Lot_Date varchar(50) '$.lot_date',
		Serial_Number varchar(50) '$.serial_number',
		Machine_Line varchar(50) '$.machine_line',
		Shift varchar(50) '$.shift',
		Standard_Qty varchar(5) '$.standard_qty',
		Is_Sent varchar(2) '$.is_sent',
		Box_Code varchar(300) '$.box_code',
		Reg_Id varchar(300) '$.reg_id',
		Reg_Dt varchar(300) '$.reg_dt',
		Chg_Id varchar(300) '$.chg_id',
		Chg_Dt varchar(300) '$.chg_dt'
	)	
   

   INSERT INTO stamp_detail (Buyer_Qr, Stamp_Code, Product_Code,SSVER, Vendor_Code, Vendor_Line, Label_Printer,Is_Sample, PCN,
    Lot_Date, Serial_Number, Machine_Line, Shift, Standard_Qty, Is_Sent, Box_Code, Reg_Id, Reg_Dt, Chg_Id, Chg_Dt)

	select Buyer_Qr, Stamp_Code, Product_Code,SSVER, Vendor_Code, Vendor_Line, Label_Printer,Is_Sample, PCN,
    Lot_Date, Serial_Number, Machine_Line, Shift, Standard_Qty, Is_Sent, Box_Code, Reg_Id, Reg_Dt, Chg_Id, Chg_Dt 
	From #listBuyerQR



	Select s.id,s.buyer_qr,s.stamp_code,d.style_nm product_name,Lot_Date lotNo,s.product_code,d.md_cd model,st.stamp_name , Standard_Qty quantity
	From stamp_detail s 
	join d_style_info d on d.style_no = s.product_code
    join stamp_master st on st.stamp_code = s.stamp_code
    where  s.buyer_qr  in (Select Buyer_Qr From #listBuyerQR )
    order by s.buyer_qr asc
END


GO
/****** Object:  StoredProcedure [dbo].[FILTER_BuyerMes]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[FILTER_BuyerMes]
	-- Add the parameters for the stored procedure here
	  (@tblBuyerQR [dbo].[UT_StringList] READONLY, 
		@model varchar(100),
		@productname varchar(100)
	  
	  )
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH FILTER_w_material_info_tims
                                AS
                                 ( 	SELECT  m.wmtid AS wmtid,
                                 m.product ProductNo,
                                 m.buyer_qr BuyerCode,
                                 m.gr_qty Quantity,
                                 'MES'  TypeSystem , 
                                 m.bb_no,  
                                @model Model, 
                                @productname ProductName 
                                 FROM	w_material_info_tims m 
                                 WHERE m.buyer_qr IN (select StringValue from @tblBuyerQR)),
                                 FILTER_stam_detail AS 
                                ( SELECT s.id AS wmtid,       
                                  s.product_code ProductNo, 
                                 s.buyer_qr BuyerCode,      
                                 s.standard_qty Quantity,   
                                 'SAP' TypeSystem ,          
                                 '' bb_no ,          
                                @model Model, 
                                 @productname ProductName 
                                FROM stamp_detail s
                                WHERE s.buyer_qr IN   (select StringValue from @tblBuyerQR) ),
                                 FILTER_RESULT_EXIST AS  (
                                    SELECT *
                                    FROM    FILTER_w_material_info_tims UNION 
                                 SELECT *  FROM    FILTER_stam_detail )
                                SELECT min(f.wmtid)wmtid, min(f.ProductNo)ProductNo, min(f.BuyerCode) BuyerCode,min(f.Quantity)Quantity,min(f.TypeSystem)TypeSystem, max(f.bb_no)bb_no,min(f.Model)Model,min(f.ProductName)ProductName
                                FROM    FILTER_RESULT_EXIST f 
                                 GROUP BY f.BuyerCode  
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllFinishProducts]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllFinishProducts] 
@product nvarchar(100),
@productname nvarchar(150),
@model nvarchar(150),
@AtNo nvarchar(150),
@RegStart nvarchar(150),
@RegEnd nvarchar(150),
@type nvarchar(100)
AS
BEGIN

select * from w_material_info_mms
	--if @type!= 'MMS' then
 -- SELECT max(a.id_actualpr) AS id_actualpr
	--	, max(a.at_no) AS at_no
	--	, max(a.type) AS type
	--	, SUM(a.target) AS totalTarget

	--	, max(a.target) AS target
	--	, max(a.product) AS product
	--	, max(a.md_cd) AS md_cd
	--	, max(a.remark) AS remark

	--	, max(a.style_nm) AS style_nm
	--	, max(a.process_count) AS process_count
	--	, SUM(a.actual) AS actual
	--	, any_value(a.count_pr_w) AS count_pr_w
	--FROM viewactual_primary a
	--WHERE (@AtNo='' OR a.at_no LIKE CONCAT('%',@AtNo,'%'))
	--AND (_productname='' OR a.style_nm LIKE CONCAT('%',_productname,'%'))
	--AND (_model='' OR a.md_cd LIKE CONCAT('%',_model,'%'))
	--AND (_product='' OR  a.product LIKE CONCAT('%',_product,'%'))
	--AND ((_RegStart='' AND _RegEnd='' ) OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( _RegStart, '%Y/%m/%d' ) AND DATE_FORMAT( _RegEnd, '%Y/%m/%d' )))
	--AND a.`finish_yn` IN ('YT')
	--GROUP BY a.at_no;
	--ELSE 
	
	--  SELECT max(a.id_actualpr) AS id_actualpr
	--	, max(a.at_no) AS at_no
	--	, max(a.`type`) AS `type`
	--	, SUM(a.target) AS totalTarget
	--	, max(a.target) AS target
	--	, max(a.product) AS product
	--	, max(a.md_cd) AS md_cd
	--	, max(a.remark) AS remark
	--	, max(a.style_nm) AS style_nm
	--	, max(a.process_count) AS process_count
	--	, SUM(a.actual) AS actual
	--	, any_value(a.count_pr_w) AS count_pr_w
	--FROM viewactual_primary a
	--WHERE (_AtNo='' OR a.at_no LIKE CONCAT('%',_AtNo,'%'))
	--AND (_productname='' OR a.style_nm LIKE CONCAT('%',_productname,'%'))
	--AND (_model='' OR a.md_cd LIKE CONCAT('%',_model,'%'))
	--AND (_product='' OR  a.product LIKE CONCAT('%',_product,'%'))
	--AND ((_RegStart='' AND _RegEnd='' ) OR (DATE_FORMAT( a.reg_dt, '%Y/%m/%d' ) BETWEEN DATE_FORMAT( _RegStart, '%Y/%m/%d' ) AND DATE_FORMAT( _RegEnd, '%Y/%m/%d' )))
	--/*AND a.`finish_yn` IN ('Y')*/
	--/*nga thêm YT*/
	--AND a.`finish_yn` IN ('Y','YT')
	--GROUP BY a.at_no;
	
	--END if;
END
GO
/****** Object:  StoredProcedure [dbo].[GetListBoxMapping]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetListBoxMapping]
	@BoxCode nvarchar(100) = '',
	@ProductCode nvarchar(100) = '',
	@BuyerCode nvarchar(100) = '',
	@Date nvarchar(100) = '',
	@intpage int = 0,
	@introw int = 0

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

   SELECT MAX(a.bmno)As bmno, a.bx_no, MAX(a.product) As ProductNo, SUM(a.gr_qty) AS totalQty, MAX(a.status) As status
	--(select Top 1 b.dt_nm from comm_dt b where b.mt_cd = 'WHS013' and b.dt_cd = MAX(a.status)) as statusName
	INTO ##Mapping_Box_Temporary
    FROM w_box_mapping a
	WHERE (@BoxCode = '' OR @BoxCode IS NULL OR a.bx_no LIKE '%' + @BoxCode +'%')
	    AND (@ProductCode = '' OR  @ProductCode IS NULL OR a.product LIKE '%'+ @ProductCode +'%')
		AND (@BuyerCode = 'undefined' OR @BuyerCode IS NULl OR a.buyer_cd LIKE '%'+ @BuyerCode + '%')
		AND (@Date  = '' OR @Date IS NULL OR a.reg_dt LIKE '%'+ @Date + '%')
	    GROUP BY a.bx_no
	-- Order By MAX(a.bmno) Desc 
    ORDER BY MAX(a.bmno) Desc OFFSET @intpage ROWS FETCH NEXT @introw ROWS ONLY


	SELECT  a.*, (select Top 1 b.dt_nm from comm_dt b where b.mt_cd = 'WHS013' and b.dt_cd = a.status) as statusName
	FROM  ##Mapping_Box_Temporary As a 
	 ORDER BY a.bmno Desc 
	
	

	Drop Table ##Mapping_Box_Temporary
END
GO
/****** Object:  StoredProcedure [dbo].[GetListMaterialAndLot]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetListMaterialAndLot]
@materialcode nvarchar(100),
@sts nvarchar(50),
@buyercode nvarchar(100)
AS
DECLARE @mtcdd nvarchar(50);
Declare @atno  nvarchar(50);

if(Exists(select material_code from w_material_info_tims where material_code=@materialcode))
begin
	set @mtcdd = (select CONCAT( material_code,' (',gr_qty,')')from w_material_info_tims where material_code=@materialcode);
	set @atno = (select at_no from w_material_info_tims where material_code=@materialcode);
end
else
begin
set @mtcdd = (select CONCAT( material_code,' (',gr_qty,')')from w_material_info_mms where material_code=@materialcode);
set @atno = (select at_no from w_actual where id_actual in(select id_actual from w_material_info_mms where material_code=@materialcode));
end

declare  @tmpb table(mapping_dt datetime,
				reg_dt datetime,
				id int,
				cha nvarchar(100),
				mt_cd nvarchar(100),
				mt_nm nvarchar(max),
				cccc nvarchar(100),
				mt_lot nvarchar(100),
				buyer_qr nvarchar(100),
				type nvarchar(100),
				bb_no nvarchar(100),
				process nvarchar(100),
				process_cd nvarchar(100),
				congnhan_time nvarchar(max),
				machine nvarchar(max),
				expiry_dt varchar(10),
				dt_of_receipt varchar(10),
				expore_dt varchar(10),
				mt_type nvarchar(100),
				SLLD int,
				mt_no nvarchar(100),
				date varchar(10),
				lot_no nvarchar(100),
				--mt_tpye nvarchar(50),
				size nvarchar(100));
declare  @tmpaa table(mapping_dt datetime,
				reg_dt datetime,
				id int,
				cha nvarchar(100),
				mt_cd nvarchar(100),
				mt_nm nvarchar(max),
				cccc nvarchar(100),
				mt_lot nvarchar(100),
				buyer_qr nvarchar(100),
				type nvarchar(100),
				bb_no nvarchar(100),
				process nvarchar(100),
				process_cd nvarchar(100),
				congnhan_time nvarchar(max),
				machine nvarchar(max),
				expiry_dt varchar(10),
				dt_of_receipt varchar(10),
				expore_dt varchar(10),
				mt_type nvarchar(100),
				SLLD int,
				mt_no nvarchar(100),
				date varchar(10),
				lot_no nvarchar(100),
				--mt_tpye nvarchar(50),
				size nvarchar(100));

select *
into #tmpactual
from w_actual where at_no=@atno and active=1--and type='SX'
select *
into #tmpmmsinfo
from w_material_info_mms where id_actual in (select id_actual from #tmpactual where type='SX')

select * 
into #timptimsinfo
from w_material_info_tims where id_actual in (select id_actual from #tmpactual where type='TIMS')


	insert @tmpb(mapping_dt,reg_dt,id,mt_cd,cccc,mt_lot,buyer_qr,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type,size,mt_nm)
	select b.* ,(CONCAT( q.width, 'MM x ', q.spec, 'M' )) size,q.mt_nm
	from (SELECT  mpptims.mapping_dt,mpptims.reg_dt,mpptims.wmmid id,
	mpptims.mt_cd,mpptims.mt_cd cccc,mpptims.mt_lot,infotims.buyer_qr,wactual.type,
	(CASE WHEN mtinfotims.bb_no IS NULL THEN mtinfomms.bb_no
	ELSE mtinfotims.bb_no END) bb_no,
	--(CASE
	--					WHEN (infotims.end_production_dt IS NOT  NULL)  THEN 	'OQC'	
	--					ELSE (SELECT dt_nm FROM comm_dt WHERE mt_cd ='COM007' and dt_cd=wactual.name)
	--				END
	--				)process
					(SELECT dt_nm FROM comm_dt WHERE mt_cd ='COM007' and dt_cd=wactual.name) process,wactual.NAME process_cd,
					(STUFF((SELECT TOP 1 ' '+CONCAT(n.userid,' - ',n.uname,' Start: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.start_dt,120),120),' End: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.end_dt,120),120))
					FROM
						d_pro_unit_staff AS staff
						JOIN mb_info AS n ON n.userid = staff.staff_id 
					WHERE
						staff.id_actual = wactual.id_actual and staff.staff_id=infotims.staff_id
						AND ((CONVERT(datetime, mpptims.mapping_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
						AND CONVERT(datetime, staff.end_dt, 121 )  )
						or ( CONVERT(datetime, mtinfotims.end_production_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
						AND CONVERT(datetime, staff.end_dt, 121 ) )
						)
						FOR XML PATH('')
					),1,1,'' )) congnhan_time,'' machine,
					(
					CASE
						
							WHEN ( wactual.type = 'TIMS' AND mtinfotims.real_qty IS NULL) THEN
							ISNULL(( SELECT TOP 1 check_qty FROM m_facline_qc WHERE ml_tims = mpptims.mt_lot AND ml_no = mpptims.mt_cd ), mtinfomms.real_qty ) 
							WHEN ( wactual.type = 'TIMS' AND mtinfotims.real_qty IS NOT NULL) THEN
							ISNULL(( SELECT TOP 1 check_qty FROM m_facline_qc WHERE ml_tims = mpptims.mt_lot AND ml_no = mpptims.mt_cd ), mtinfotims.real_qty ) 
							WHEN ( mtinfomms.material_type IS NULL) THEN
							(CASE 
							WHEN  mtinfomms.material_type!= 'CMT' THEN mtinfomms.gr_qty 
							WHEN ( mtinfomms.id_actual_oqc IS NULL OR mtinfomms.id_actual_oqc = '' OR mtinfomms.id_actual_oqc = 0 ) 
							THEN 
							ISNULL((
								SELECT TOP 1
									check_qty 
								FROM
									m_facline_qc 
								WHERE
									ml_tims = mpptims.mt_lot 
									AND ml_no = mpptims.mt_cd),
								ISNULL((
										mtinfomms.real_qty -(
										SELECT TOP 1
											( check_qty - ok_qty ) 
										FROM
											m_facline_qc 
										WHERE
											ml_no = mpptims.mt_cd 
										ORDER BY
											reg_dt ASC 
										)),
									mtinfomms.real_qty 
								))
						
							END)

							ELSE mtinfomms.real_qty 
						END 
						) SLLD,
					(CASE
					WHEN mtinfotims.mt_no IS NULL THEN mtinfomms.mt_no
					ELSE mtinfotims.mt_no END ) mt_no,
					(CASE WHEN mtinfotims.reg_date IS NULL THEN mtinfomms.reg_date
					ELSE mtinfotims.reg_date END ) AS  date,
					--(CASE WHEN mtinfotims.expiry_dt IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME,mtinfomms.expiry_date,120),120) ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME,mtinfotims.expiry_dt,120),120) END ) expiry_dt,
				--(CASE WHEN mtinfotims.date_of_receipt IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME,mtinfomms.date_of_receipt,120),120) ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME,mtinfotims.date_of_receipt,120),120) END ) dt_of_receipt,
			--(CASE WHEN mtinfotims.expore_dt IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.export_date,120),120) ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfotims.expore_dt,120),120) END ) expore_dt,
			'' expiry_dt,'' dt_of_receipt,'' expore_dt,
					(CASE
					WHEN mtinfotims.lot_no IS NULL THEN mtinfomms.lot_no
					ELSE mtinfotims.lot_no END ) lot_no,
					(CASE
					WHEN mtinfotims.material_type IS NULL THEN mtinfomms.material_type
					ELSE mtinfotims.material_type END ) mt_type
			
	FROM w_material_mapping_tims mpptims
LEFT JOIN #timptimsinfo mtinfotims ON mtinfotims.material_code = mpptims.mt_cd
	LEFT JOIN #tmpmmsinfo mtinfomms ON mtinfomms.material_code = mpptims.mt_cd
	INNER JOIN #timptimsinfo infotims ON infotims.material_code = mpptims.mt_lot
	INNER JOIN #tmpactual wactual  ON wactual.id_actual=infotims.id_actual
	)b
	LEFT JOIN d_material_info  AS q ON q.mt_no =b.mt_no;

	--select *
	--From @tmpb ;

	
	insert @tmpaa(mapping_dt,reg_dt,id,mt_cd,cccc,mt_lot,buyer_qr,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type,size,mt_nm)
	select b.*  ,(CONCAT( q.width, 'MM x ', q.spec, 'M' )) size,q.mt_nm
	from (SELECT  mppmms.mapping_dt,mppmms.reg_date reg_dt ,mppmms.wmmId id,
	mppmms.mt_cd mt_cd,infomms.material_code cccc,mppmms.mt_lot,null buyer_qr,wactual.type,
	(CASE 
	WHEN mtinfomms.bb_no IS NULL THEN invt.bb_no
	ELSE mtinfomms.bb_no
	END ) bb_no,
	(SELECT dt_nm FROM comm_dt WHERE mt_cd = 'COM007' AND dt_cd = wactual.NAME ) process,
	wactual.NAME process_cd,
	(STUFF((SELECT TOP 1 ' '+ CONCAT(n.userid,' - ',n.uname,' Start: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.start_dt,120),120) ,' End: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.end_dt,120),120) )
			FROM
		d_pro_unit_staff AS staff
		JOIN mb_info AS n ON n.userid = staff.staff_id 
	WHERE
		staff.id_actual = wactual.id_actual 
		AND CONVERT(datetime, mppmms.mapping_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
		AND CONVERT(datetime, staff.end_dt, 121 )  
		FOR XML PATH('')
	),1,1,'' )) congnhan_time,
	(STUFF((SELECT TOP 1 ' '+ CONCAT(machine.mc_no,' Start: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, machine.start_dt,120),120) ,' End: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, machine.end_dt,120),120)  )
		FROM
			d_pro_unit_mc AS machine
		WHERE
			machine.id_actual = wactual.id_actual 
			AND CONVERT(datetime, mppmms.mapping_dt,121 ) BETWEEN CONVERT(datetime, machine.start_dt, 121 ) 
			AND CONVERT(datetime, machine.end_dt, 121 )  
			FOR XML PATH('')
		),1,1,'' )) machine,
		--isnull((CASE 
		--WHEN mtinfomms.material_type IS NULL THEN invt.gr_qty
		--WHEN mtinfomms.material_type !='CMT'  THEN mtinfomms.gr_qty
		--ELSE (mtinfomms.gr_qty -(SELECT TOP 1 ( check_qty - ok_qty ) 
		--					FROM
		--						m_facline_qc 
		--					WHERE
		--						ml_no = mtinfomms.material_code 
		--					ORDER BY
		--						reg_dt ASC ))
		--END),0) SLLD,
		isnull((CASE 
		WHEN mtinfomms.material_type IS NULL THEN invt.real_qty
		WHEN mtinfomms.material_type IS not NULL and mtinfomms.material_type !='CMT' THEN mtinfomms.gr_qty
		ELSE (mtinfomms.real_qty -(SELECT TOP 1 ( check_qty - ok_qty ) 
							FROM
								m_facline_qc 
							WHERE
								ml_no = mtinfomms.material_code 
							ORDER BY
								reg_dt ASC ))
		END),mtinfomms.real_qty) SLLD,
		(CASE
		WHEN mtinfomms.mt_no IS NULL THEN invt.mt_no
		ELSE mtinfomms.mt_no END ) mt_no,
		(CASE
		WHEN mtinfomms.reg_date IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.create_date,120),120)
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.reg_date,120),120) END ) AS  date,
		(CASE
		WHEN mtinfomms.expiry_date IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.expiry_date,120),120) 
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.expiry_date,120),120) END ) expiry_dt,
		(CASE
		WHEN mtinfomms.date_of_receipt IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.date_of_receipt,120),120)
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.date_of_receipt,120),120) END ) dt_of_receipt,
		(CASE
		WHEN mtinfomms.export_date IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.export_date,120),120)
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.export_date,120),120) END ) expore_dt,
		(CASE
		WHEN mtinfomms.lot_no IS NULL THEN invt.lot_no
		ELSE mtinfomms.lot_no END ) lot_no,
		(CASE
		WHEN mtinfomms.material_type IS NULL THEN invt.mt_type
		ELSE mtinfomms.material_type END ) mt_type
	FROM w_material_mapping_mms mppmms
	INNER JOIN #tmpmmsinfo infomms ON mppmms.mt_lot = infomms.material_code
	LEFT JOIN #tmpmmsinfo mtinfomms ON mppmms.mt_cd = mtinfomms.material_code
	LEFT JOIN inventory_products invt ON invt.material_code= mppmms.mt_cd
	INNER JOIN #tmpactual  wactual ON wactual.id_actual =infomms.id_actual
	) b
	LEFT JOIN d_material_info  AS q ON q.mt_no =b.mt_no;
	--select *
	--From @tmpaa ;
--	Drop table #tmpactual;
--Drop table #tmpmmsinfo;
--Drop table #timptimsinfo;
	if(@sts='buyer')
		begin
			with tree as(
			--select 0 AS order_lv,  *
			--	From @tmpb where buyer_qr=@buyercode  and mt_lot=@materialcode
			--	UNION ALL
				select 1 AS order_lv,  *
				From @tmpb where mt_lot=@materialcode
				UNION ALL
				select t.order_lv +1 As order_lv, b.*
				From @tmpb b
				inner join tree t on  t.mt_cd=b.mt_lot--(t.mt_cd like concat(b.mt_lot,'-DV%') or t.mt_cd=b.mt_lot)--t.mt_cd=b.mt_lot
				UNION ALL
				select t.order_lv +1 As order_lv, c.*
				From @tmpaa c
				inner join tree t on t.mt_cd=c.mt_lot-- (t.mt_cd like concat(c.mt_lot,'-DV%') or t.mt_cd=c.mt_lot)-- t.mt_cd=c.mt_lot
			)
	--with tree as(
	--select *
	--From @tmpb where mt_lot=@materialcode
	--UNION ALL
	--select b.*
	--From @tmpb b
	--inner join tree t on t.mt_cd=b.mt_lot
	--)
	--select * from tree
	

			SELECT 0 order_lv,null mapping_dt, b.reg_date reg_dt,1 id, @buyercode AS buyer_qr, @mtcdd AS  cha,b.material_code mt_lot,'1' cccc,b.material_code mt_cd,'TIMS' type,
			b.bb_no,'OQC' process, '1' process_cd,concat(n.userid,' - ',n.uname ,' Start: ', CONVERT(VARCHAR(23),CONVERT(DATETIME, k.start_dt,120),120),
								 ' End: ',CONVERT(VARCHAR(23),CONVERT(DATETIME, k.end_dt,120),120) 
								 ) congnhan_time,'' machine, b.gr_qty AS  SLLD, b.mt_no mt_no,'1' size,'1' mt_nm,
			'' date,'' expiry_dt,'' dt_of_receipt,'' expore_dt,'' lot_no,'CMT' mt_type 
						FROM d_pro_unit_staff AS k
						JOIN mb_info AS n ON n.userid=k.staff_id
						JOIN w_material_info_tims b on  k.id_actual=b.id_actual_oqc
						WHERE k.staff_id=b.staff_id_oqc 
						 AND CONVERT(datetime,b.end_production_dt,121)
						  BETWEEN CONVERT(datetime,k.start_dt,121)  AND CONVERT(datetime,k.end_dt,121) 
						AND  b.buyer_qr = @buyercode
			union  all
			SELECT  order_lv,mapping_dt,reg_dt,id,@buyercode AS buyer_qr,@mtcdd AS  cha,mt_lot,cccc,mt_cd,type,
			bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,
			date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type
				FROM tree;

	end
else 
	begin
				with tree as(
			--select 0 AS order_lv,  *
			--	From @tmpb where buyer_qr=@buyercode  and mt_lot=@materialcode
			--	UNION ALL
				select 1 AS order_lv,  *
				From @tmpb where mt_lot=@materialcode
				UNION ALL
				select t.order_lv +1 As order_lv, b.*
				From @tmpb b
				inner join tree t on  t.mt_cd=b.mt_lot--(t.mt_cd like concat(b.mt_lot,'-DV%') or t.mt_cd=b.mt_lot)--t.mt_cd=b.mt_lot
				UNION ALL
				select t.order_lv +1 As order_lv, c.*
				From @tmpaa c
				inner join tree t on t.mt_cd=c.mt_lot-- (t.mt_cd like concat(c.mt_lot,'-DV%') or t.mt_cd=c.mt_lot)-- t.mt_cd=c.mt_lot
			)
		SELECT  order_lv,mapping_dt,reg_dt,id,@buyercode AS buyer_qr,@mtcdd AS  cha,mt_lot,cccc,mt_cd,type,
		bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,
		date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type
					FROM tree;
	end
GO
/****** Object:  StoredProcedure [dbo].[GetListMaterialAndLotTest]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetListMaterialAndLotTest]
@materialcode nvarchar(100),
@sts nvarchar(50),
@buyercode nvarchar(100)
AS
DECLARE @mtcdd nvarchar(50);
Declare @atno  nvarchar(50);

if(Exists(select material_code from w_material_info_tims where material_code=@materialcode))
begin
	set @mtcdd = (select CONCAT( material_code,' (',gr_qty,')')from w_material_info_tims where material_code=@materialcode);
	set @atno = (select at_no from w_material_info_tims where material_code=@materialcode);
end
else
begin
set @mtcdd = (select CONCAT( material_code,' (',gr_qty,')')from w_material_info_mms where material_code=@materialcode);
set @atno = (select at_no from w_actual where id_actual in(select id_actual from w_material_info_mms where material_code=@materialcode));
end

declare  @tmpb table(mapping_dt datetime,
				reg_dt datetime,
				id int,
				cha nvarchar(100),
				mt_cd nvarchar(100),
				mt_nm nvarchar(max),
				cccc nvarchar(100),
				mt_lot nvarchar(100),
				buyer_qr nvarchar(100),
				type nvarchar(100),
				bb_no nvarchar(100),
				process nvarchar(100),
				process_cd nvarchar(100),
				congnhan_time nvarchar(max),
				machine nvarchar(max),
				expiry_dt varchar(10),
				dt_of_receipt varchar(10),
				expore_dt varchar(10),
				mt_type nvarchar(100),
				SLLD int,
				mt_no nvarchar(100),
				date varchar(10),
				lot_no nvarchar(100),
				--mt_tpye nvarchar(50),
				size nvarchar(100));
declare  @tmpaa table(mapping_dt datetime,
				reg_dt datetime,
				id int,
				cha nvarchar(100),
				mt_cd nvarchar(100),
				mt_nm nvarchar(max),
				cccc nvarchar(100),
				mt_lot nvarchar(100),
				buyer_qr nvarchar(100),
				type nvarchar(100),
				bb_no nvarchar(100),
				process nvarchar(100),
				process_cd nvarchar(100),
				congnhan_time nvarchar(max),
				machine nvarchar(max),
				expiry_dt varchar(10),
				dt_of_receipt varchar(10),
				expore_dt varchar(10),
				mt_type nvarchar(100),
				SLLD int,
				mt_no nvarchar(100),
				date varchar(10),
				lot_no nvarchar(100),
				--mt_tpye nvarchar(50),
				size nvarchar(100));

select *
into #tmpactual
from w_actual where at_no=@atno and active=1--and type='SX'
select *
into #tmpmmsinfo
from w_material_info_mms where id_actual in (select id_actual from #tmpactual where type='SX')

select * 
into #timptimsinfo
from w_material_info_tims where id_actual in (select id_actual from #tmpactual where type='TIMS')


	insert @tmpb(mapping_dt,reg_dt,id,mt_cd,cccc,mt_lot,buyer_qr,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type,size,mt_nm)
	select b.* ,(CONCAT( q.width, 'MM x ', q.spec, 'M' )) size,q.mt_nm
	from (SELECT  mpptims.mapping_dt,mpptims.reg_dt,mpptims.wmmid id,
	mpptims.mt_cd,mpptims.mt_cd cccc,mpptims.mt_lot,infotims.buyer_qr,wactual.type,
	(CASE WHEN mtinfotims.bb_no IS NULL THEN mtinfomms.bb_no
	ELSE mtinfotims.bb_no END) bb_no,
					/*(SELECT dt_nm FROM comm_dt WHERE mt_cd ='COM007' and dt_cd=wactual.name) process,*/--Fix lai
					'' process,
					wactual.NAME process_cd,
					/*(STUFF((SELECT TOP 1 ' '+CONCAT(n.userid,' - ',n.uname,' Start: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.start_dt,120),120),' End: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.end_dt,120),120))
					FROM
						d_pro_unit_staff AS staff
						JOIN mb_info AS n ON n.userid = staff.staff_id 
					WHERE
						staff.id_actual = wactual.id_actual and staff.staff_id=infotims.staff_id
						AND ((CONVERT(datetime, mpptims.mapping_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
						AND CONVERT(datetime, staff.end_dt, 121 )  )
						or ( CONVERT(datetime, mtinfotims.end_production_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
						AND CONVERT(datetime, staff.end_dt, 121 ) )
						)
						FOR XML PATH('')
					),1,1,'' )) congnhan_time*/--Fix lai
					'' congnhan_time,
					'' machine,
					(
					CASE
						
							WHEN ( wactual.type = 'TIMS' AND mtinfotims.real_qty IS NULL) THEN
							ISNULL(( SELECT TOP 1 check_qty FROM m_facline_qc WHERE ml_tims = mpptims.mt_lot AND ml_no = mpptims.mt_cd ), mtinfomms.real_qty ) 
							WHEN ( wactual.type = 'TIMS' AND mtinfotims.real_qty IS NOT NULL) THEN
							ISNULL(( SELECT TOP 1 check_qty FROM m_facline_qc WHERE ml_tims = mpptims.mt_lot AND ml_no = mpptims.mt_cd ), mtinfotims.real_qty ) 
							WHEN ( mtinfomms.material_type IS NULL) THEN
							(CASE 
							WHEN  mtinfomms.material_type!= 'CMT' THEN mtinfomms.gr_qty 
							WHEN ( mtinfomms.id_actual_oqc IS NULL OR mtinfomms.id_actual_oqc = '' OR mtinfomms.id_actual_oqc = 0 ) 
							THEN 
							ISNULL((
								SELECT TOP 1
									check_qty 
								FROM
									m_facline_qc 
								WHERE
									ml_tims = mpptims.mt_lot 
									AND ml_no = mpptims.mt_cd),
								ISNULL((
										mtinfomms.real_qty -(
										SELECT TOP 1
											( check_qty - ok_qty ) 
										FROM
											m_facline_qc 
										WHERE
											ml_no = mpptims.mt_cd 
										ORDER BY
											reg_dt ASC 
										)),
									mtinfomms.real_qty 
								))
						
							END)

							ELSE mtinfomms.real_qty 
						END 
						) SLLD,
					(CASE
					WHEN mtinfotims.mt_no IS NULL THEN mtinfomms.mt_no
					ELSE mtinfotims.mt_no END ) mt_no,
					(CASE WHEN mtinfotims.reg_date IS NULL THEN mtinfomms.reg_date
					ELSE mtinfotims.reg_date END ) AS  date,
				'' expiry_dt,'' dt_of_receipt,'' expore_dt,
					(CASE
					WHEN mtinfotims.lot_no IS NULL THEN mtinfomms.lot_no
					ELSE mtinfotims.lot_no END ) lot_no,
					(CASE
					WHEN mtinfotims.material_type IS NULL THEN mtinfomms.material_type
					ELSE mtinfotims.material_type END ) mt_type
			
	FROM w_material_mapping_tims mpptims
LEFT JOIN #timptimsinfo mtinfotims ON mtinfotims.material_code = mpptims.mt_cd
	LEFT JOIN #tmpmmsinfo mtinfomms ON mtinfomms.material_code = mpptims.mt_cd
	INNER JOIN #timptimsinfo infotims ON infotims.material_code = mpptims.mt_lot
	INNER JOIN #tmpactual wactual  ON wactual.id_actual=infotims.id_actual
	)b
	LEFT JOIN d_material_info  AS q ON q.mt_no =b.mt_no;

	--select *
	--From @tmpb ;

	
	insert @tmpaa(mapping_dt,reg_dt,id,mt_cd,cccc,mt_lot,buyer_qr,type,bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type,size,mt_nm)
	select b.*  ,(CONCAT( q.width, 'MM x ', q.spec, 'M' )) size,q.mt_nm
	from (SELECT  mppmms.mapping_dt,mppmms.reg_date reg_dt ,mppmms.wmmId id,
	mppmms.mt_cd mt_cd,infomms.material_code cccc,mppmms.mt_lot,null buyer_qr,wactual.type,
	(CASE 
	WHEN mtinfomms.bb_no IS NULL THEN invt.bb_no
	ELSE mtinfomms.bb_no
	END ) bb_no,
	/*(SELECT dt_nm FROM comm_dt WHERE mt_cd = 'COM007' AND dt_cd = wactual.NAME ) process,*/--Fix lai
	'' process,
	wactual.NAME process_cd,
	/*(STUFF((SELECT TOP 1 ' '+ CONCAT(n.userid,' - ',n.uname,' Start: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.start_dt,120),120) ,' End: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, staff.end_dt,120),120) )
			FROM
		d_pro_unit_staff AS staff
		JOIN mb_info AS n ON n.userid = staff.staff_id 
	WHERE
		staff.id_actual = wactual.id_actual 
		AND CONVERT(datetime, mppmms.mapping_dt,121 ) BETWEEN CONVERT(datetime, staff.start_dt, 121 ) 
		AND CONVERT(datetime, staff.end_dt, 121 )  
		FOR XML PATH('')
	),1,1,'' )) congnhan_time,*/--Fix lai
	'' congnhan_time,
	/*(STUFF((SELECT TOP 1 ' '+ CONCAT(machine.mc_no,' Start: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, machine.start_dt,120),120) ,' End: ',CONVERT(VARCHAR(19),CONVERT(DATETIME, machine.end_dt,120),120)  )
		FROM
			d_pro_unit_mc AS machine
		WHERE
			machine.id_actual = wactual.id_actual 
			AND CONVERT(datetime, mppmms.mapping_dt,121 ) BETWEEN CONVERT(datetime, machine.start_dt, 121 ) 
			AND CONVERT(datetime, machine.end_dt, 121 )  
			FOR XML PATH('')
		),1,1,'' )) machine,*/--Fix lai
		'' machine,
		--isnull((CASE 
		--WHEN mtinfomms.material_type IS NULL THEN invt.gr_qty
		--WHEN mtinfomms.material_type !='CMT'  THEN mtinfomms.gr_qty
		--ELSE (mtinfomms.gr_qty -(SELECT TOP 1 ( check_qty - ok_qty ) 
		--					FROM
		--						m_facline_qc 
		--					WHERE
		--						ml_no = mtinfomms.material_code 
		--					ORDER BY
		--						reg_dt ASC ))
		--END),0) SLLD,
		isnull((CASE 
		WHEN mtinfomms.material_type IS NULL THEN invt.real_qty
		WHEN mtinfomms.material_type IS not NULL and mtinfomms.material_type !='CMT' THEN mtinfomms.gr_qty
		ELSE (mtinfomms.real_qty -(SELECT TOP 1 ( check_qty - ok_qty ) 
							FROM
								m_facline_qc 
							WHERE
								ml_no = mtinfomms.material_code 
							ORDER BY
								reg_dt ASC ))
		END),mtinfomms.real_qty) SLLD,
		(CASE
		WHEN mtinfomms.mt_no IS NULL THEN invt.mt_no
		ELSE mtinfomms.mt_no END ) mt_no,
		(CASE
		WHEN mtinfomms.reg_date IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.create_date,120),120)
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.reg_date,120),120) END ) AS  date,
		(CASE
		WHEN mtinfomms.expiry_date IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.expiry_date,120),120) 
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.expiry_date,120),120) END ) expiry_dt,
		(CASE
		WHEN mtinfomms.date_of_receipt IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.date_of_receipt,120),120)
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.date_of_receipt,120),120) END ) dt_of_receipt,
		(CASE
		WHEN mtinfomms.export_date IS NULL THEN CONVERT(VARCHAR(10),CONVERT(DATETIME, invt.export_date,120),120)
		ELSE CONVERT(VARCHAR(10),CONVERT(DATETIME, mtinfomms.export_date,120),120) END ) expore_dt,
		(CASE
		WHEN mtinfomms.lot_no IS NULL THEN invt.lot_no
		ELSE mtinfomms.lot_no END ) lot_no,
		(CASE
		WHEN mtinfomms.material_type IS NULL THEN invt.mt_type
		ELSE mtinfomms.material_type END ) mt_type
	FROM w_material_mapping_mms mppmms
	INNER JOIN #tmpmmsinfo infomms ON mppmms.mt_lot = infomms.material_code
	LEFT JOIN #tmpmmsinfo mtinfomms ON mppmms.mt_cd = mtinfomms.material_code
	LEFT JOIN inventory_products invt ON invt.material_code= mppmms.mt_cd
	INNER JOIN #tmpactual  wactual ON wactual.id_actual =infomms.id_actual
	) b
	LEFT JOIN d_material_info  AS q ON q.mt_no =b.mt_no;
	--select *
	--From @tmpaa ;
--	Drop table #tmpactual;
--Drop table #tmpmmsinfo;
--Drop table #timptimsinfo;
	if(@sts='buyer')
		begin
			with tree as(
			--select 0 AS order_lv,  *
			--	From @tmpb where buyer_qr=@buyercode  and mt_lot=@materialcode
			--	UNION ALL
				select 1 AS order_lv,  *
				From @tmpb where mt_lot=@materialcode
				UNION ALL
				select t.order_lv +1 As order_lv, b.*
				From @tmpb b
				inner join tree t on  t.mt_cd=b.mt_lot--(t.mt_cd like concat(b.mt_lot,'-DV%') or t.mt_cd=b.mt_lot)--t.mt_cd=b.mt_lot
				UNION ALL
				select t.order_lv +1 As order_lv, c.*
				From @tmpaa c
				inner join tree t on t.mt_cd=c.mt_lot-- (t.mt_cd like concat(c.mt_lot,'-DV%') or t.mt_cd=c.mt_lot)-- t.mt_cd=c.mt_lot
			)

			SELECT 0 order_lv,null mapping_dt, b.reg_date reg_dt,1 id, @buyercode AS buyer_qr, @mtcdd AS  cha,b.material_code mt_lot,'1' cccc,b.material_code mt_cd,'TIMS' type,
			b.bb_no,'OQC' process, '1' process_cd,concat(n.userid,' - ',n.uname ,' Start: ', CONVERT(VARCHAR(23),CONVERT(DATETIME, k.start_dt,120),120),
								 ' End: ',CONVERT(VARCHAR(23),CONVERT(DATETIME, k.end_dt,120),120) 
								 ) congnhan_time,'' machine, b.gr_qty AS  SLLD, b.mt_no mt_no,'1' size,'1' mt_nm,
			'' date,'' expiry_dt,'' dt_of_receipt,'' expore_dt,'' lot_no,'CMT' mt_type 
						FROM d_pro_unit_staff AS k
						JOIN mb_info AS n ON n.userid=k.staff_id
						JOIN w_material_info_tims b on  k.id_actual=b.id_actual_oqc
						WHERE k.staff_id=b.staff_id_oqc 
						 AND CONVERT(datetime,b.end_production_dt,121)
						  BETWEEN CONVERT(datetime,k.start_dt,121)  AND CONVERT(datetime,k.end_dt,121) 
						AND  b.buyer_qr = @buyercode
			union  all
			SELECT  order_lv,mapping_dt,reg_dt,id,@buyercode AS buyer_qr,@mtcdd AS  cha,mt_lot,cccc,mt_cd,type,
			bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,
			date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type
				FROM tree;

	end
else 
	begin
				with tree as(
			--select 0 AS order_lv,  *
			--	From @tmpb where buyer_qr=@buyercode  and mt_lot=@materialcode
			--	UNION ALL
				select 1 AS order_lv,  *
				From @tmpb where mt_lot=@materialcode
				UNION ALL
				select t.order_lv +1 As order_lv, b.*
				From @tmpb b
				inner join tree t on  t.mt_cd=b.mt_lot--(t.mt_cd like concat(b.mt_lot,'-DV%') or t.mt_cd=b.mt_lot)--t.mt_cd=b.mt_lot
				UNION ALL
				select t.order_lv +1 As order_lv, c.*
				From @tmpaa c
				inner join tree t on t.mt_cd=c.mt_lot-- (t.mt_cd like concat(c.mt_lot,'-DV%') or t.mt_cd=c.mt_lot)-- t.mt_cd=c.mt_lot
			)
		SELECT  order_lv,mapping_dt,reg_dt,id,@buyercode AS buyer_qr,@mtcdd AS  cha,mt_lot,cccc,mt_cd,type,
		bb_no,process,process_cd,congnhan_time,machine,SLLD,mt_no,size,mt_nm,
		date,expiry_dt,dt_of_receipt,expore_dt,lot_no,mt_type
					FROM tree;
	end
GO
/****** Object:  StoredProcedure [dbo].[GetListMaterialInfo]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetListMaterialInfo]
	-- Add the parameters for the stored procedure here
	@sdno varchar(100) = '',
	@mtno varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    SELECT  a.mt_no, Max(a.id) as id, Sum(a.quantity) as SoluongCap, Max(a.sd_no) as sd_no, Sum(a.meter) as meter
	Into ##temporary_shipping_dmaterial
	FROM shippingsdmaterial AS a
	WHERE a.sd_no = @sdno and a.active = 1
	GROUP BY a.mt_no

	select sd_no,mt_no,material_code Into ##temporary_inventory_products from inventory_products  where sd_no=@sdno AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')

	select sd_no,mt_no,material_code Into ##temporary_w_material_info_mms from w_material_info_mms where sd_no=@sdno AND ( orgin_mt_cd IS NULL OR orgin_mt_cd = '')

	select sd_no,mt_no,count(material_code)SoLuongNhanDuoc 
	Into ##temporary_UnionAllJoin
	from(
		select * From ##temporary_inventory_products
		union all
		select * From ##temporary_w_material_info_mms
	) aa
	group by sd_no,mt_no





SELECT  MAX(abc.id) As wmtid, MAX(abc.mt_no) AS mt_no, MAX(abc.SoluongCap) AS SoluongCap, isnull(info.SoLuongNhanDuoc,0) SoLuongNhanDuoc, MAX(abc.meter) AS meter,
	CASE
		WHEN MAX(abc.SoluongCap) > 0  THEN (Max(abc.SoluongCap) -  isnull(info.SoLuongNhanDuoc,0)) 
		ELSE 0
	END AS SoluongConLai
FROM ##temporary_shipping_dmaterial AS abc
	LEFT JOIN ##temporary_UnionAllJoin AS info  ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no 
GROUP BY abc.mt_no, abc.SoluongCap, info.SoLuongNhanDuoc
Order By abc.mt_no ASC


Drop Table ##temporary_shipping_dmaterial
Drop Table ##temporary_inventory_products
Drop Table ##temporary_w_material_info_mms
Drop Table ##temporary_UnionAllJoin

END
GO
/****** Object:  StoredProcedure [dbo].[GetListPickingScan]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetListPickingScan]
	-- Add the parameters for the stored procedure here
	@mtno varchar(100) = '',
	@sdno varchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here

	Select material_code, sd_no, mt_no 
	Into ##temporary_inventory_product 
	From inventory_products 
	 WHERE sd_no = @sdno And mt_no = @mtno


	SELECT  max(a.mt_no) AS mt_no,max(a.id) AS id, sum(a.quantity) AS SoluongCap, max(a.sd_no) AS sd_no
	Into ##termporary_shippingsd_material
    FROM shippingsdmaterial AS a
    WHERE a.sd_no = @sdno
    group by a.mt_no


	
	SELECT max(abc.mt_no) as mt_no,max(abc.SoluongCap) as SoluongCap, COUNT(info.material_code) AS SoLuongNhanDuoc,
                CASE
                    WHEN max(abc.SoluongCap) > 0 THEN (max(abc.SoluongCap) -  COUNT(info.material_code)) 
                    ELSE 0
                END as SoluongConLai

    FROM ##termporary_shippingsd_material AS abc
		left JOIN ##temporary_inventory_product As info ON info.sd_no = abc.sd_no AND abc.mt_no = info.mt_no 
    Where abc.mt_no = @mtno
    GROUP BY abc.mt_no

	Drop Table ##temporary_inventory_product
	Drop Table ##termporary_shippingsd_material
END
GO
/****** Object:  StoredProcedure [dbo].[MaterialMemo]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MaterialMemo]
	@datemonth varchar(20) = '',
	@date varchar(100) = '',
	@product varchar(200) = '',
	@material varchar(200) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

SELECT (CASE
        WHEN ('08:00:00' <= FORMAT( CAST( a.receiving_dt AS datetime ),'%H:%i:%s') AND  FORMAT( CAST( a.receiving_dt AS datetime ),'yyyy/MM/dd')  <  '23:59:00') THEN
        FORMAT( CAST( a.receiving_dt AS DATETIME ),'yyyy/MM/dd')

        when (FORMAT( CAST(a.receiving_dt AS datetime ),'yyyy/MM/dd')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,a.receiving_dt),'yyyy/MM/dd')
            ELSE ''
        END )  as reg_date,
		a.receiving_dt,
		a.mt_cd AS mt_no ,a.style_no,
		a.width AS width ,
		a.spec AS spec ,
		a.TX  ,a.total_m,a.total_m2,a.total_ea, a.lot_no
		Into ##Memo
		FROM w_material_info_memo a
		WHERE   
		left(convert(varchar, a.receiving_dt, 120),7) = @datemonth
		--FORMAT(CAST(a.receiving_dt as datetime), 'yyyy-MM') = @datemonth
		and (@product='' OR  a.style_no like @product )
		and (@material='' OR  a.mt_cd like @material )



	SELECT max(TABLE1.mt_no) mt_no, max(TABLE1.reg_date) reg_date, max(TABLE1.style_no) product,MAX(TABLE1.width) width, MAX(TABLE1.spec) spec, SUM(TABLE1.TX) total_roll, SUM(TABLE1.total_m) total_m, SUM(TABLE1.total_m2) total_m2, SUM(TABLE1.total_ea) total_ea, MAX(TABLE1.lot_no) AS lot_no
	FROM ##Memo AS TABLE1
    WHERE  (@date='' OR left(convert(varchar, TABLE1.reg_date, 120),10) = @date)
	--(@date='' OR   FORMAT(CAST(TABLE1.reg_date as datetime),'yyyy-MM-dd') like @date )
    GROUP BY TABLE1.reg_date, TABLE1.mt_no, TABLE1.style_no
    ORDER BY  TABLE1.reg_date DESC, TABLE1.style_no

	Drop Table ##Memo
END
GO
/****** Object:  StoredProcedure [dbo].[MaterialShipping]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
/*
	select left(convert(varchar, getdate(), 120),10)
*/
-- =============================================
CREATE PROCEDURE [dbo].[MaterialShipping]
	-- Add the parameters for the stored procedure here
	@datemonth varchar(20) = '',
	@date varchar(100) = '',
	@product varchar(200) = '',
	@material varchar(200) = ''
	
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT b.lot_no, b.mt_no, b.gr_qty, b.recei_wip_date, b.real_qty, b.sd_no,
	(CASE 
				WHEN ('08:00:00' <= FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss') 
					  AND  FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss')  <  '23:59:00') 
				THEN
					FORMAT( CAST( b.recei_wip_date AS DATETIME ),'yyyy-MM-dd')

				when (FORMAT( CAST( b.recei_wip_date AS datetime ),'HH:mm:ss')  < '08:00:00') THEN  FORMAT(DATEADD(day,-1,b.recei_wip_date),'yyyy-MM-dd')
					ELSE ''
				END )  as reg_date
	into ##inventory_temporary_table 
	FROM  inventory_products b 
	WHERE left(convert(varchar, b.recei_wip_date, 120),7) = @datemonth  And b.mt_type='PMT' AND b.status!='004'  
		--DATEPART(year, b.recei_wip_date) = left(@datemonth,4) and DATEPART(month, b.recei_wip_date) = right(@datemonth,2) 
		--FORMAT(CAST(b.recei_wip_date as datetime), 'yyyy-MM') = @datemonth and b.mt_type='PMT' AND b.status!='004'            

     SELECT 
            TABLE1.mt_no as MaterialNo,
            TABLE1.unit_cd,
            TABLE1.product_cd as product,
            count(TABLE1.gr_qty) countSocuon,
            SUM(CONVERT(int,TABLE1.gr_qty)) TongSoMet,
            ( TABLE1.reg_date ) recevingDate
     FROM 
			(SELECT b.lot_no, a.sd_no,a.product_cd, b.mt_no, b.gr_qty, c.spec,c.width,c.unit_cd, 
			b.recei_wip_date, b.real_qty,b.reg_date

			FROM w_sd_info AS a
			JOIN ##inventory_temporary_table AS b ON a.sd_no = b.sd_no
			JOIN d_material_info AS c ON b.mt_no = c.mt_no
							
			WHERE(@product='' OR  a.product_cd like @product )
			and (@material='' OR  b.mt_no like @material )
	
			)
			AS TABLE1  
    WHERE  left(convert(varchar, TABLE1.reg_date, 120),10) = @date  
	--(@date='' OR (DATEPART(year,  TABLE1.reg_date) = left(@date,4) And DATEPART(month,  TABLE1.reg_date) = right(@date,2) And DATEPART(day, TABLE1.reg_date) = right(@date,2)))
    GROUP BY TABLE1.reg_date, TABLE1.mt_no, TABLE1.product_cd,TABLE1.unit_cd
    ORDER BY  TABLE1.reg_date DESC, TABLE1.product_cd

	Drop Table ##inventory_temporary_table
END
GO
/****** Object:  StoredProcedure [dbo].[PrintMaterialStamp]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PrintMaterialStamp]
	@ListItem varchar(max)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

   select *
	into #listItem
	from OPENJSON(@ListItem)
	with 
	(
		Mt_No varchar(100) '$.mt_no',
		Gr_Qty float '$.gr_qty',
		Real_Qty float '$.real_qty',
		Mt_Type varchar(5) '$.mt_type',
		Expore_Dt varchar(50) '$.expore_dt',
		Expiry_Dt varchar(50) '$.expiry_dt',
		Dt_Of_Receipt varchar(50) '$.dt_of_receipt',
		Lot_No varchar(100) '$.lot_no',
		Reg_Id varchar(50) '$.reg_id',
		Chg_Id varchar(50) '$.chg_id',
		Reg_Dt varchar(50) '$.reg_dt',
		Chg_Dt varchar(50) '$.chg_dt',
		DateCur varchar(50) '$.date',
		Status varchar(5) '$.status',
		Use_Yn varchar(2) '$.use_yn',
		Mt_Cd varchar(300) '$.mt_cd',
		Mt_Barcode varchar(300) '$.mt_barcode',
		Mt_Qrcode varchar(300) '$.mt_qrcode'
	)	
	-- Insert new list Item Request
	--select 'Insert new list Item Request' -- Check store
	 INSERT INTO w_material_info_tam (mt_type, mt_cd, mt_no, gr_qty, date, expiry_dt, dt_of_receipt, expore_dt, lot_no, mt_barcode, mt_qrcode, status, use_yn, reg_id, reg_dt, chg_id, chg_dt,real_qty)
	 select Mt_Type, Mt_Cd, Mt_No, Gr_Qty, DateCur, Expiry_Dt, Dt_Of_Receipt, Expore_Dt, Lot_No, Mt_Barcode, Mt_Qrcode,       					Status, Use_Yn, Reg_Id, Reg_Dt, Chg_Id, Chg_Dt, Real_Qty
	from #listItem	

	Select Scope_Identity()
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateLengthMaterial]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateLengthMaterial]
	-- Add the parameters for the stored procedure here
	
	--@Gr_Qty float  = 0,
	--@Real_Qty float = 0,
	@Chg_Id varchar(100) = '',
	@Chg_dt  varchar(100) = '',
	@ListData NVARCHAR(MAX) = '[]'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	
	IF OBJECT_ID('tempdb.dbo.#ListLengthMaterial') IS NOT NULL 
	Drop Table  #ListLengthMaterial

	SELECT *
	INTO #ListLengthMaterial
	FROM OPENJSON(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@ListData,'\\t','\t'),'\\r','\r'),'\\n','\n'),'\\f','\f'),'\\b','\b'),'\"','"'),'\\','\'))
	--FROM OPENJSON(@ListDat@ListDataa)
	WITH 
	(
		ID varchar(200) '$.id'
	)	

		--Update w_material_info_tam SET gr_qty= @Gr_Qty , real_qty = @Real_Qty, chg_id = @Chg_Id, chg_dt = @Chg_dt WHERE mt_cd = (Select ID From #ListLengthMaterial) 
		Update w_material_info_tam SET chg_id = @Chg_Id, chg_dt = @Chg_dt WHERE mt_cd IN (Select ID From #ListLengthMaterial)  
		
END
GO
/****** Object:  StoredProcedure [dbo].[usp_BuyerQR_Filter]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_BuyerQR_Filter]
    (@tblBuyerQR [dbo].[UT_StringList] READONLY)
AS
BEGIN
    SET NOCOUNT ON;
     
    SELECT a.StringValue FROM @tblBuyerQR a WHERE NOT EXISTS (SELECT b.buyer_qr FROM dbo.generalfg b WHERE b.buyer_qr = a.StringValue);
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetListMappingStaTims_FindMerge]    Script Date: 3/10/2022 10:16:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetListMappingStaTims_FindMerge] @mt_code VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @loopCode VARCHAR(255);

	DECLARE @loopTbl TABLE (
		code VARCHAR(200) PRIMARY KEY
	)
	INSERT INTO @loopTbl (code)
		SELECT
			value
		FROM STRING_SPLIT(@mt_code, ',');

	--DECLARE @tempTbl TABLE (
	--	wmtid INT PRIMARY KEY
	--   ,material_code VARCHAR(255)
	--   ,id_actual INT
	--   ,real_qty INT
	--   ,gr_qty INT
	--   ,status VARCHAR(255)
	--   ,mt_no VARCHAR(255)
	--   ,bb_no VARCHAR(255)
	--   ,chg_id VARCHAR(50)
	--   ,chg_date DATETIME
	--   ,reg_id VARCHAR(50)
	--   ,reg_date DATETIME
	--)
	--WHILE EXISTS (SELECT
	--		*
	--	FROM @loopTbl)
	--BEGIN
	--SELECT TOP 1
	--	@loopCode = code
	--FROM @loopTbl;
	--INSERT INTO @tempTbl
	--	SELECT
	--		wmit.wmtid
	--	   ,wmit.material_code
	--	   ,wmit.id_actual
	--	   ,wmit.real_qty
	--	   ,wmit.gr_qty
	--	   ,wmit.status
	--	   ,wmit.mt_no
	--	   ,wmit.bb_no
	--	   ,wmit.chg_id
	--	   ,wmit.chg_date
	--	   ,wmit.reg_id
	--	   ,wmit.reg_date
	--	FROM dbo.w_material_info_tims wmit
	--	--WHERE CONTAINS(wmit.material_code, @loopCode);  
	--	WHERE wmit.material_code LIKE CONCAT(@loopCode, '%')
	----WHERE EXISTS (SELECT a.code FROM @tempTbl a WHERE a.code LIKE CONCAT(wmit.material_code, '%'))
	----ORDER BY wmit.reg_date DESC
	--;

	--DELETE @loopTbl
	--WHERE code = @loopCode

	--END

	--SELECT
	--	wmtid
	--   ,material_code
	--   ,id_actual
	--   ,real_qty
	--   ,gr_qty
	--   ,status
	--   ,mt_no
	--   ,bb_no
	--   ,chg_id
	--   ,chg_date
	--   ,reg_id
	--   ,reg_date
	--FROM @tempTbl
	--;

	SELECT
		wmit.wmtid
	   ,wmit.material_code
	   ,wmit.id_actual
	   ,wmit.real_qty
	   ,wmit.gr_qty
	   ,wmit.status
	   ,wmit.mt_no
	   ,wmit.bb_no
	   ,wmit.chg_id
	   ,wmit.chg_date
	   ,wmit.reg_id
	   ,wmit.reg_date
	--FROM @tempTbl
	FROM dbo.w_material_info_tims wmit
	WHERE EXISTS (SELECT a.code FROM @loopTbl a WHERE wmit.material_code LIKE CONCAT(a.code, '%'))

END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_action', @level2type=N'COLUMN',@level2name=N'mn_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_action', @level2type=N'COLUMN',@level2name=N'url_link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_info', @level2type=N'COLUMN',@level2name=N'at_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_info', @level2type=N'COLUMN',@level2name=N'at_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'at_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'mn_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'st_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'ct_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'mt_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'author_menu_info', @level2type=N'COLUMN',@level2name=N'del_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'buyer_info', @level2type=N'COLUMN',@level2name=N'address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'buyer_info', @level2type=N'COLUMN',@level2name=N'web_site'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'buyer_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'buyer_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'buyer_info', @level2type=N'COLUMN',@level2name=N'del_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'd_style_info', @level2type=N'COLUMN',@level2name=N'productType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N'depart_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N'depart_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N'up_depart_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N'level_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'department_info', @level2type=N'COLUMN',@level2name=N'del_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'General FG những mã buyer từ SAP đưa vào.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'generalfg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'recei_wip_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'picking_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'recei_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'expiry_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'export_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'date_of_receipt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'create_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'inventory_products', @level2type=N'COLUMN',@level2name=N'change_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'lct_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'lct_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'up_lct_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'level_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'order_no'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??RFID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'lct_rfid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'lct_bar_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Factory YN' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'lct_info', @level2type=N'COLUMN',@level2name=N'ft_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'제조사코드' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'mf_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'제조사명' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'mf_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'브랜드명' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'brd_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'로고' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'logo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'전화번호' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'phone_nb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'웹사이트' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'web_site'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'주소' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'메모' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'사용유무' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'삭제유무' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'manufac_info', @level2type=N'COLUMN',@level2name=N'del_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_author_info', @level2type=N'COLUMN',@level2name=N'userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_author_info', @level2type=N'COLUMN',@level2name=N'at_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_author_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_author_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'uname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'nick_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'upw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'grade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'tel_nb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'cel_nb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'e_mail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMS??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'sms_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'scr_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'mail_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'ltacc_dt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'mbout_dt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'mbout_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'accblock_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'session_key'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'session_limit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'del_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'check_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'mb_info', @level2type=N'COLUMN',@level2name=N'log_ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'mn_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'mn_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'up_mn_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'level_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'url_link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'col_css'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'sub_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'mn_full'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu_info', @level2type=N'COLUMN',@level2name=N'mn_cd_full'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'?????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'sp_cd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'sp_nm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'phone_nb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'e_mail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'web_site'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'??' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N're_mark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'???' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'use_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'supplier_info', @level2type=N'COLUMN',@level2name=N'del_yn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_actual', @level2type=N'COLUMN',@level2name=N'at_no'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_actual_primary', @level2type=N'COLUMN',@level2name=N'at_no'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_history', @level2type=N'COLUMN',@level2name=N'at_no'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_history', @level2type=N'COLUMN',@level2name=N'staff_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_mms', @level2type=N'COLUMN',@level2name=N'expiry_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_mms', @level2type=N'COLUMN',@level2name=N'export_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_mms', @level2type=N'COLUMN',@level2name=N'date_of_receipt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_mms', @level2type=N'COLUMN',@level2name=N'rece_wip_dt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_info_tam', @level2type=N'COLUMN',@level2name=N'staff_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_mapping_tims', @level2type=N'COLUMN',@level2name=N'expiry_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_mapping_tims', @level2type=N'COLUMN',@level2name=N'export_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'getdate()' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_material_mapping_tims', @level2type=N'COLUMN',@level2name=N'date_of_receipt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'w_vt_dt', @level2type=N'COLUMN',@level2name=N'staff_id'
GO
