USE [ShinSungTest]
GO
/****** Object:  Table [real-autodb].[author_action]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE SCHEMA [real-autodb]
GO
CREATE TABLE [real-autodb].[author_action](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[mn_cd] [varchar](12) NULL,
	[url_link] [varchar](100) NULL,
	[id_button] [varchar](50) NULL,
	[type] [varchar](30) NULL,
	[name_table] [varchar](150) NULL,
	[sts_action] [varchar](3) NULL,
	[re_mark] [nvarchar](50) NULL,
 CONSTRAINT [PK__author_a__3213E83F1783A048] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[author_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[author_info](
	[atno] [int] IDENTITY(1,1) NOT NULL,
	[at_cd] [varchar](6) NULL,
	[at_nm] [nvarchar](50) NULL,
	[role] [varchar](5) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[re_mark] [nvarchar](200) NULL,
 CONSTRAINT [PK__author_i__5B373795B63F0681] PRIMARY KEY CLUSTERED 
(
	[atno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[author_menu_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[author_menu_info](
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
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[nameen] [nvarchar](max) NULL,
	[namevi] [nvarchar](max) NULL,
	[namekr] [nvarchar](max) NULL,
 CONSTRAINT [PK__author_m__61F0CE3490DDA2A6] PRIMARY KEY CLUSTERED 
(
	[amno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[buyer_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[buyer_info](
	[byno] [int] IDENTITY(1,1) NOT NULL,
	[buyer_cd] [varchar](20) NULL,
	[buyer_nm] [nvarchar](50) NULL,
	[ceo_nm] [nvarchar](50) NULL,
	[manager_nm] [nvarchar](50) NULL,
	[brd_nm] [nvarchar](50) NULL,
	[logo] [varchar](200) NULL,
	[phone_nb] [varchar](20) NULL,
	[cell_nb] [varchar](20) NULL,
	[fax_nb] [varchar](20) NULL,
	[e_mail] [varchar](50) NULL,
	[address] [nvarchar](200) NULL,
	[web_site] [varchar](50) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[stampId] [int] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__buyer_in__55B92786520EDE09] PRIMARY KEY CLUSTERED 
(
	[byno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IDX_BUYERINFO_01] UNIQUE NONCLUSTERED 
(
	[buyer_cd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[comm_dt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[comm_dt](
	[cdid] [int] IDENTITY(1,1) NOT NULL,
	[mt_cd] [varchar](6) NOT NULL,
	[dt_cd] [varchar](200) NOT NULL,
	[dt_nm] [nvarchar](500) NOT NULL,
	[dt_kr] [varchar](50) NULL,
	[dt_vn] [varchar](50) NULL,
	[dt_exp] [nvarchar](100) NULL,
	[up_cd] [varchar](20) NULL,
	[val1] [varchar](20) NULL,
	[val1_nm] [nvarchar](50) NULL,
	[val2] [varchar](20) NULL,
	[val2_nm] [nvarchar](50) NULL,
	[val3] [varchar](20) NULL,
	[val3_nm] [nvarchar](50) NULL,
	[val4] [varchar](20) NULL,
	[val4_nm] [nvarchar](50) NULL,
	[dt_order] [int] NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[unit] [varchar](10) NULL,
 CONSTRAINT [PK__comm_dt__289F51AC81D1918D] PRIMARY KEY CLUSTERED 
(
	[cdid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[comm_mt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[comm_mt](
	[mt_id] [int] IDENTITY(1,1) NOT NULL,
	[div_cd] [varchar](3) NULL,
	[mt_cd] [varchar](6) NULL,
	[mt_nm] [nvarchar](50) NULL,
	[mt_exp] [nvarchar](100) NULL,
	[memo] [nvarchar](100) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__comm_mt__9106EB28E0234AF8] PRIMARY KEY CLUSTERED 
(
	[mt_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_bobbin_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_bobbin_info](
	[bno] [int] IDENTITY(1,1) NOT NULL,
	[mc_type] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[mt_cd] [varchar](200) NULL,
	[bb_nm] [nvarchar](50) NULL,
	[purpose] [nvarchar](200) NULL,
	[barcode] [varchar](50) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[count_number] [int] NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__d_bobbin__DE97B98870E33DB5] PRIMARY KEY CLUSTERED 
(
	[bno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [bb_no] UNIQUE NONCLUSTERED 
(
	[bb_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_bobbin_lct_hist]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_bobbin_lct_hist](
	[blno] [int] IDENTITY(1,1) NOT NULL,
	[mc_type] [varchar](6) NULL,
	[bb_no] [varchar](35) NULL,
	[mt_cd] [varchar](200) NOT NULL,
	[bb_nm] [nvarchar](50) NULL,
	[start_dt] [varchar](14) NULL,
	[end_dt] [varchar](14) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__d_bobbin__5077FE0C20A2A1ED] PRIMARY KEY CLUSTERED 
(
	[blno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_bom_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_bom_info](
	[bid] [int] IDENTITY(1,1) NOT NULL,
	[bom_no] [varchar](100) NULL,
	[style_no] [varchar](200) NULL,
	[mt_no] [varchar](200) NULL,
	[need_time] [float] NULL,
	[cav] [int] NULL,
	[need_m] [float] NULL,
	[buocdap] [float] NULL,
	[del_yn] [char](1) NOT NULL,
	[IsApply] [char](1) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[bid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_machine_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_machine_info](
	[mno] [int] IDENTITY(1,1) NOT NULL,
	[mc_type] [varchar](6) NULL,
	[mc_no] [varchar](20) NULL,
	[mc_nm] [nvarchar](50) NULL,
	[purpose] [nvarchar](200) NULL,
	[color] [varchar](20) NULL,
	[barcode] [varchar](50) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__d_machin__DF50C617E83FB79F] PRIMARY KEY CLUSTERED 
(
	[mno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_material_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_material_info](
	[mtid] [int] IDENTITY(1,1) NOT NULL,
	[mt_type] [varchar](20) NULL,
	[mt_no] [varchar](300) NOT NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no_origin] [varchar](500) NULL,
	[mt_nm] [nvarchar](300) NULL,
	[mf_cd] [varchar](20) NULL,
	[gr_qty] [varchar](20) NULL,
	[unit_cd] [varchar](6) NULL,
	[bundle_qty] [varchar](6) NULL,
	[bundle_unit] [varchar](6) NULL,
	[sp_cd] [varchar](20) NULL,
	[s_lot_no] [varchar](100) NULL,
	[item_vcd] [varchar](20) NULL,
	[qc_range_cd] [varchar](100) NULL,
	[width] [varchar](20) NULL,
	[width_unit] [varchar](3) NULL,
	[spec] [varchar](20) NULL,
	[spec_unit] [varchar](6) NULL,
	[area] [varchar](20) NULL,
	[area_unit] [nvarchar](50) NULL,
	[thick] [varchar](20) NULL,
	[thick_unit] [varchar](3) NULL,
	[stick] [varchar](20) NULL,
	[stick_unit] [varchar](6) NULL,
	[consum_yn] [char](1) NULL,
	[price] [varchar](20) NULL,
	[tot_price] [varchar](20) NULL,
	[price_unit] [varchar](10) NULL,
	[price_least_unit] [nvarchar](50) NULL,
	[photo_file] [varchar](100) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[barcode] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__d_materi__79F14663CF7D6A5E] PRIMARY KEY CLUSTERED 
(
	[mtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_model_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_model_info](
	[mdid] [int] IDENTITY(1,1) NOT NULL,
	[md_cd] [varchar](200) NULL,
	[md_nm] [nvarchar](200) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__d_model___7C73BD7A6E3AD2A7] PRIMARY KEY CLUSTERED 
(
	[mdid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_mold_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_mold_info](
	[mdno] [int] IDENTITY(1,1) NOT NULL,
	[md_no] [varchar](20) NULL,
	[md_nm] [nvarchar](50) NULL,
	[purpose] [nvarchar](200) NULL,
	[barcode] [varchar](50) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__d_mold_i__7C7342E673425985] PRIMARY KEY CLUSTERED 
(
	[mdno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_pro_unit_mc]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_pro_unit_mc](
	[pmid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[start_dt] [varchar](14) NULL,
	[end_dt] [varchar](14) NULL,
	[remark] [nvarchar](500) NULL,
	[mc_no] [varchar](20) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__d_pro_un__412600BA435E08E5] PRIMARY KEY CLUSTERED 
(
	[pmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_pro_unit_staff]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_pro_unit_staff](
	[psid] [int] IDENTITY(1,1) NOT NULL,
	[staff_id] [varchar](20) NULL,
	[actual] [float] NULL,
	[defect] [float] NULL,
	[id_actual] [int] NULL,
	[staff_tp] [varchar](6) NULL,
	[start_dt] [varchar](14) NULL,
	[end_dt] [varchar](14) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[psid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_rounting_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_rounting_info](
	[idr] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](100) NULL,
	[process_code] [int] NULL,
	[name] [nvarchar](50) NULL,
	[level] [int] NULL,
	[don_vi_pr] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[description] [nvarchar](500) NULL,
	[isFinish] [char](1) NOT NULL,
	[reg_dt] [datetime2](0) NULL,
	[reg_id] [varchar](20) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__d_rounti__DC501A7EE85E9D18] PRIMARY KEY CLUSTERED 
(
	[idr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[d_style_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[d_style_info](
	[sid] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](100) NULL,
	[style_nm] [nvarchar](200) NULL,
	[md_cd] [varchar](50) NULL,
	[prj_nm] [nvarchar](100) NULL,
	[ssver] [varchar](50) NULL,
	[part_nm] [nvarchar](200) NULL,
	[standard] [varchar](200) NULL,
	[cust_rev] [varchar](50) NULL,
	[order_num] [varchar](50) NULL,
	[pack_amt] [int] NULL,
	[cav] [varchar](50) NULL,
	[bom_type] [varchar](50) NULL,
	[tds_no] [varchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[qc_range_cd] [varchar](6) NULL,
	[stamp_code] [varchar](6) NULL,
	[expiry_month] [varchar](6) NULL,
	[expiry] [nvarchar](50) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[drawingname] [nvarchar](50) NULL,
	[loss] [varchar](50) NULL,
	[productType] [char](1) NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK__d_style___DDDFDD361732887A] PRIMARY KEY CLUSTERED 
(
	[sid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[department_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[department_info](
	[dpno] [int] IDENTITY(1,1) NOT NULL,
	[depart_cd] [varchar](9) NULL,
	[depart_nm] [nvarchar](50) NULL,
	[up_depart_cd] [varchar](18) NULL,
	[level_cd] [varchar](3) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[order_no] [int] NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[mn_full] [varchar](100) NULL,
 CONSTRAINT [PK__departme__2DDF83691B2139EC] PRIMARY KEY CLUSTERED 
(
	[dpno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[exporttomachine]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[exporttomachine](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ExportCode] [varchar](20) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [nvarchar](200) NULL,
	[MachineCode] [varchar](50) NULL,
	[IsFinish] [char](50) NULL,
	[Description] [nvarchar](1000) NULL,
	[CreateId] [varchar](20) NULL,
	[CreateDate] [datetime2](0) NOT NULL,
	[ChangeId] [varchar](20) NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__exportto__3213E83FB7C23129] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[generalfg]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[generalfg](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[buyer_qr] [varchar](50) NULL,
	[product_code] [varchar](50) NULL,
	[at_no] [varchar](50) NULL,
	[type] [varchar](10) NULL,
	[md_cd] [varchar](200) NULL,
	[dl_no] [varchar](200) NULL,
	[qty] [int] NULL,
	[lot_no] [varchar](200) NULL,
	[sts_cd] [varchar](10) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [buyer_qr] UNIQUE NONCLUSTERED 
(
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[language]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[language](
	[router] [varchar](50) NOT NULL,
	[keyname] [nvarchar](100) NOT NULL,
	[en] [nvarchar](max) NULL,
	[vi] [nvarchar](max) NULL,
	[kr] [nvarchar](max) NULL,
 CONSTRAINT [PK__language__8C23A7AE2AA7D74F] PRIMARY KEY CLUSTERED 
(
	[keyname] ASC,
	[router] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[lct_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[lct_info](
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
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
	[mn_full] [varchar](500) NULL,
	[sap_lct_cd] [varchar](20) NULL,
	[userid] [varchar](50) NULL,
	[selected] [varchar](50) NULL,
 CONSTRAINT [PK__lct_info__ED27DAC146A8CCC0] PRIMARY KEY CLUSTERED 
(
	[lctno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[m_board]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[m_board](
	[mno] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](2000) NOT NULL,
	[content] [nvarchar](max) NULL,
	[viewcnt] [int] NULL,
	[replycnt] [int] NULL,
	[div_cd] [char](1) NULL,
	[start_dt] [varchar](14) NULL,
	[end_dt] [varchar](14) NULL,
	[widthsize] [int] NULL,
	[heightsize] [int] NULL,
	[back_color] [varchar](20) NULL,
	[order_no] [int] NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__m_board__DF50C61712144784] PRIMARY KEY CLUSTERED 
(
	[mno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[m_facline_qc]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[m_facline_qc](
	[fqno] [int] IDENTITY(1,1) NOT NULL,
	[fq_no] [varchar](11) NULL,
	[ml_no] [varchar](200) NULL,
	[ml_tims] [varchar](200) NULL,
	[product_cd] [varchar](500) NULL,
	[shift] [nvarchar](50) NULL,
	[at_no] [varchar](20) NULL,
	[work_dt] [varchar](20) NULL,
	[item_vcd] [varchar](20) NULL,
	[item_nm] [nvarchar](200) NULL,
	[item_exp] [nvarchar](500) NULL,
	[check_qty] [int] NULL,
	[ok_qty] [int] NULL,
	[ng_qty] [int] NULL,
	[remain_qty] [int] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__m_faclin__337107C65DCBEE0E] PRIMARY KEY CLUSTERED 
(
	[fqno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[m_facline_qc_value]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[m_facline_qc_value](
	[fqhno] [int] IDENTITY(1,1) NOT NULL,
	[fq_no] [varchar](11) NULL,
	[product] [varchar](50) NULL,
	[at_no] [varchar](50) NULL,
	[shift] [nvarchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_cd] [varchar](20) NULL,
	[check_value] [nvarchar](500) NULL,
	[check_qty] [int] NULL,
	[date_ymd] [nvarchar](50) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__m_faclin__3ACA3D8BCF73BD69] PRIMARY KEY CLUSTERED 
(
	[fqhno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[manufac_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[manufac_info](
	[mfno] [int] IDENTITY(1,1) NOT NULL,
	[mf_cd] [varchar](50) NULL,
	[mf_nm] [nvarchar](50) NULL,
	[brd_nm] [nvarchar](50) NULL,
	[logo] [nvarchar](200) NULL,
	[phone_nb] [varchar](14) NULL,
	[web_site] [nvarchar](50) NULL,
	[address] [nvarchar](200) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__manufac___7DCF56547E3C15EC] PRIMARY KEY CLUSTERED 
(
	[mfno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[materialbom]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[materialbom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](50) NOT NULL,
	[MaterialPrarent] [varchar](300) NOT NULL,
	[MaterialNo] [varchar](300) NOT NULL,
	[CreateId] [varchar](50)  NULL,
	[ChangeId] [varchar](50)  NULL,
	[CreateDate] [datetime2](0)  NULL,
	[ChangeDate] [datetime2](0)  NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ProductCode] UNIQUE NONCLUSTERED 
(
	[ProductCode] ASC,
	[MaterialPrarent] ASC,
	[MaterialNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[mb_author_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[mb_author_info](
	[mano] [int] IDENTITY(1,1) NOT NULL,
	[userid] [varchar](20) NULL,
	[at_cd] [varchar](6) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__mb_autho__7A21B366B6FA08BF] PRIMARY KEY CLUSTERED 
(
	[mano] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[mb_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[mb_info](
	[userid] [varchar](50) NOT NULL,
	[uname] [nvarchar](200) NULL,
	[nick_name] [nvarchar](50) NULL,
	[upw] [varchar](50) NULL,
	[grade] [nvarchar](50) NULL,
	[depart_cd] [varchar](9) NULL,
	[gender] [char](1) NULL,
	[position_cd] [varchar](6) NULL,
	[tel_nb] [varchar](20) NULL,
	[cel_nb] [varchar](20) NULL,
	[e_mail] [varchar](100) NULL,
	[sms_yn] [char](1) NULL,
	[join_dt] [varchar](8) NULL,
	[birth_dt] [varchar](8) NULL,
	[scr_yn] [char](1) NULL,
	[mail_yn] [char](1) NULL,
	[join_ip] [varchar](50) NULL,
	[join_domain] [varchar](100) NULL,
	[ltacc_dt] [datetime2](0) NULL,
	[ltacc_domain] [varchar](100) NULL,
	[mbout_dt] [datetime2](0) NULL,
	[mbout_yn] [char](1) NULL,
	[accblock_yn] [char](1) NULL,
	[session_key] [varchar](50) NULL,
	[session_limit] [datetime2](0) NULL,
	[memo] [varchar](500) NULL,
	[del_yn] [char](1) NULL,
	[check_yn] [char](1) NULL,
	[rem_me] [char](1) NULL,
	[barcode] [nvarchar](100) NULL,
	[mbjoin_dt] [datetime2](0) NULL,
	[log_ip] [varchar](50) NULL,
	[lct_cd] [varchar](18) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
	[re_mark] [varchar](50) NULL,
 CONSTRAINT [PK__mb_info__CBA1B2576956C6D9] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[mb_lct_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[mb_lct_info](
	[dlno] [int] IDENTITY(1,1) NOT NULL,
	[userid] [varchar](20) NULL,
	[lct_cd] [varchar](18) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__mb_lct_i__DA9AACB53D37F9FC] PRIMARY KEY CLUSTERED 
(
	[dlno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[mb_message]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[mb_message](
	[tid] [int] IDENTITY(1,1) NOT NULL,
	[message] [nvarchar](300) NULL,
	[del_yn] [char](1) NULL,
	[reg_dt] [datetime2](0) NULL,
	[reg_id] [varchar](20) NULL,
 CONSTRAINT [PK__mb_messa__DC105B0F05BE718C] PRIMARY KEY CLUSTERED 
(
	[tid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[menu_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[menu_info](
	[mnno] [int] IDENTITY(1,1) NOT NULL,
	[mn_cd] [varchar](12) NULL,
	[mn_nm] [nvarchar](50) NULL,
	[up_mn_cd] [nvarchar](50) NULL,
	[level_cd] [varchar](3) NULL,
	[url_link] [nvarchar](100) NULL,
	[re_mark] [nvarchar](500) NULL,
	[col_css] [varchar](20) NULL,
	[sub_yn] [char](1) NULL,
	[order_no] [int] NULL,
	[use_yn] [char](1) NULL,
	[mn_full] [nvarchar](100) NULL,
	[mn_cd_full] [nvarchar](100) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[selected] [varchar](50) NULL,
	[nameen] [nvarchar](max) NULL,
	[namevi] [nvarchar](max) NULL,
	[namekr] [nvarchar](max) NULL,
 CONSTRAINT [PK__menu_inf__774D6AC2C30C6586] PRIMARY KEY CLUSTERED 
(
	[mnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[notice_board]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[notice_board](
	[bno] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](2000) NULL,
	[content] [nvarchar](max) NULL,
	[mn_cd] [nvarchar](50) NULL,
	[viewcnt] [int] NULL,
	[replycnt] [int] NULL,
	[div_cd] [char](1) NULL,
	[lng_cd] [varchar](3) NULL,
	[start_dt] [varchar](14) NULL,
	[end_dt] [varchar](14) NULL,
	[widthsize] [int] NULL,
	[heightsize] [int] NULL,
	[back_color] [varchar](20) NULL,
	[order_no] [int] NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__notice_b__DE97B988CC8517BA] PRIMARY KEY CLUSTERED 
(
	[bno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[product_material]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[product_material](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [nvarchar](100) NULL,
	[process_code] [int] NULL,
	[level] [int] NULL,
	[name] [nvarchar](50) NULL,
	[mt_no] [varchar](200) NULL,
	[need_time] [float] NULL,
	[cav] [int] NULL,
	[need_m] [float] NULL,
	[buocdap] [float] NULL,
	[isActive] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__product___3213E83FE7800E68] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[product_material_detail]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[product_material_detail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](100) NOT NULL,
	[process_code] [int] NULL,
	[level] [int] NOT NULL,
	[name] [nvarchar](50) NULL,
	[MaterialPrarent] [varchar](300) NOT NULL,
	[MaterialNo] [varchar](300) NOT NULL,
	[CreateId] [varchar](50) NOT NULL,
	[ChangeId] [varchar](50) NOT NULL,
	[CreateDate] [datetime2](0) NOT NULL,
	[ChangeDate] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__product___3213E83FEC31C8A6] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [unique(Product,Materialparent,materialNo)] UNIQUE NONCLUSTERED 
(
	[ProductCode] ASC,
	[level] ASC,
	[MaterialPrarent] ASC,
	[MaterialNo] ASC,
	[name] ASC,
	[process_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[product_routing]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[product_routing](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[style_no] [varchar](50) NULL,
	[process_code] [varchar](50) NULL,
	[IsApply] [char](1) NOT NULL,
	[process_name] [nvarchar](50) NULL,
	[description] [nvarchar](500) NULL,
	[reg_dt] [datetime2](0) NULL,
	[reg_id] [varchar](20) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__product___3213E83F96C4FE20] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [style_no] UNIQUE NONCLUSTERED 
(
	[style_no] ASC,
	[process_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[qc_item_mt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[qc_item_mt](
	[ino] [int] IDENTITY(1,1) NOT NULL,
	[item_type] [varchar](6) NULL,
	[item_vcd] [varchar](20) NULL,
	[item_cd] [varchar](20) NULL,
	[ver] [varchar](3) NULL,
	[item_nm] [nvarchar](200) NULL,
	[item_exp] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__qc_item___DC50F6DD6F037EA9] PRIMARY KEY CLUSTERED 
(
	[ino] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[qc_itemcheck_dt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[qc_itemcheck_dt](
	[icdno] [int] IDENTITY(1,1) NOT NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_cd] [varchar](20) NULL,
	[defect_yn] [char](1) NULL,
	[check_name] [nvarchar](200) NULL,
	[order_no] [int] NULL,
	[re_mark] [nvarchar](100) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__qc_itemc__2E400DACA1E1167F] PRIMARY KEY CLUSTERED 
(
	[icdno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[qc_itemcheck_mt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[qc_itemcheck_mt](
	[icno] [int] IDENTITY(1,1) NOT NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_type] [varchar](6) NULL,
	[check_subject] [nvarchar](500) NULL,
	[min_value] [decimal](18, 7) NULL,
	[max_value] [decimal](18, 7) NULL,
	[range_type] [varchar](6) NULL,
	[order_no] [int] NULL,
	[re_mark] [nvarchar](100) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__qc_itemc__9DF973249C473B51] PRIMARY KEY CLUSTERED 
(
	[icno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[shippingfgsorting]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[shippingfgsorting](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ShippingCode] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[shippingfgsortingdetail]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[shippingfgsortingdetail](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ShippingCodee] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC,
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[shippingsdmaterial]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[shippingsdmaterial](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sd_no] [varchar](100) NULL,
	[mt_no] [varchar](300) NULL,
	[quantity] [float] NULL,
	[meter] [float] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[shippingtimssorting]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[shippingtimssorting](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ShippingCodesss] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[shippingtimssortingdetail]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[shippingtimssortingdetail](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ShippingCodesdds] UNIQUE NONCLUSTERED 
(
	[ShippingCode] ASC,
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[stamp_detail]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[stamp_detail](
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
	[lot_date] [varchar](30) NULL,
	[serial_number] [varchar](11) NULL,
	[machine_line] [varchar](3) NULL,
	[shift] [varchar](1) NULL,
	[standard_qty] [int] NOT NULL,
	[is_sent] [char](1) NULL,
	[box_code] [varchar](50) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [buyer_qrsdfdf] UNIQUE NONCLUSTERED 
(
	[buyer_qr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[stamp_master]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[stamp_master](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[stamp_code] [varchar](50) NOT NULL,
	[stamp_name] [nvarchar](500) NULL,
 CONSTRAINT [PK__stamp_ma__3213E83F158D2865] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [stamp_code] UNIQUE NONCLUSTERED 
(
	[stamp_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[supplier_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[supplier_info](
	[spno] [int] IDENTITY(1,1) NOT NULL,
	[sp_cd] [varchar](50) NULL,
	[sp_nm] [nvarchar](500) NULL,
	[bsn_tp] [nvarchar](50) NULL,
	[changer_id] [varchar](20) NULL,
	[phone_nb] [varchar](50) NULL,
	[cell_nb] [varchar](50) NULL,
	[fax_nb] [varchar](500) NULL,
	[e_mail] [nvarchar](500) NULL,
	[web_site] [nvarchar](500) NULL,
	[address] [nvarchar](500) NULL,
	[re_mark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__supplier__2DD5023730F2ACDF] PRIMARY KEY CLUSTERED 
(
	[spno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[user_author]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[user_author](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [varchar](50) NULL,
	[at_nm] [nvarchar](50) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__user_aut__3213E83F27E780AA] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[version_app]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[version_app](
	[id_app] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) NULL,
	[name_file] [nvarchar](200) NULL,
	[version] [int] NOT NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__version___6BE8D887293E745A] PRIMARY KEY CLUSTERED 
(
	[id_app] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_actual]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_actual](
	[id_actual] [int] IDENTITY(1,1) NOT NULL,
	[at_no] [varchar](18) NULL,
	[type] [varchar](50) NULL,
	[product] [varchar](50) NULL,
	[actual] [float] NULL,
	[defect] [float] NULL,
	[name] [nvarchar](50) NULL,
	[level] [int] NULL,
	[date] [varchar](10) NULL,
	[don_vi_pr] [varchar](50) NULL,
	[item_vcd] [varchar](20) NULL,
	[description] [nvarchar](500) NULL,
	[IsFinished] [bit] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__w_actual__76B65472F5399687] PRIMARY KEY CLUSTERED 
(
	[id_actual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_actual_primary]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_actual_primary](
	[id_actualpr] [int] IDENTITY(1,1) NOT NULL,
	[at_no] [varchar](18) NULL,
	[type] [varchar](50) NULL,
	[target] [int] NULL,
	[product] [varchar](50) NULL,
	[process_code] [varchar](50) NULL,
	[remark] [nvarchar](200) NULL,
	[finish_yn] [char](4) NULL,
	[IsApply] [char](1) NULL,
	[IsMove] [bit] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
 CONSTRAINT [PK__w_actual__080B87A72E01C40C] PRIMARY KEY CLUSTERED 
(
	[id_actualpr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_box_mapping]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_box_mapping](
	[bmno] [int] IDENTITY(1,1) NOT NULL,
	[dl_no] [varchar](50) NULL,
	[bx_no] [varchar](50) NULL,
	[buyer_cd] [varchar](50) NULL,
	[mt_cd] [varchar](200) NULL,
	[gr_qty] [int] NOT NULL,
	[product] [varchar](50) NULL,
	[type] [varchar](3) NULL,
	[mapping_dt] [varchar](14) NULL,
	[sts] [nvarchar](50) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_box_ma__16B0C34321B537ED] PRIMARY KEY CLUSTERED 
(
	[bmno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_dl_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_dl_info](
	[dlid] [int] IDENTITY(1,1) NOT NULL,
	[dl_no] [varchar](20) NULL,
	[dl_nm] [nvarchar](50) NULL,
	[dl_sts_cd] [varchar](6) NULL,
	[work_dt] [varchar](10) NULL,
	[lct_cd] [varchar](18) NULL,
	[remark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_dl_inf__DA9A945EAB941C67] PRIMARY KEY CLUSTERED 
(
	[dlid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_ex_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_ex_info](
	[exid] [int] IDENTITY(1,1) NOT NULL,
	[ex_no] [varchar](20) NULL,
	[ex_nm] [nvarchar](50) NULL,
	[ex_sts_cd] [varchar](6) NULL,
	[work_dt] [varchar](10) NULL,
	[lct_cd] [varchar](18) NULL,
	[alert] [int] NULL,
	[remark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_ex_inf__38F37A70FCA0FD82] PRIMARY KEY CLUSTERED 
(
	[exid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_ext_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_ext_info](
	[extid] [int] IDENTITY(1,1) NOT NULL,
	[ext_no] [varchar](20) NULL,
	[ext_nm] [nvarchar](50) NULL,
	[ext_sts_cd] [varchar](6) NULL,
	[work_dt] [varchar](8) NULL,
	[lct_cd] [varchar](18) NULL,
	[alert] [int] NULL,
	[remark] [nvarchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_ext_in__A4CD40E0309317C0] PRIMARY KEY CLUSTERED 
(
	[extid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_down]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_down](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[mt_cd] [varchar](500) NULL,
	[gr_qty] [float] NULL,
	[gr_down] [float] NULL,
	[reason] [nvarchar](200) NULL,
	[sts_now] [nvarchar](50) NULL,
	[bb_no] [varchar](35) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_materi__14D76DC64091DA80] PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_info](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [nvarchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_info_history]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_info_history](
	[wmtid_his] [int] IDENTITY(1,1) NOT NULL,
	[status] [nvarchar](100) NULL,
	[wmtid] [int] NULL,
	[id_actual] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[id_actual_oqc] [int] NULL,
	[staff_id] [varchar](100) NULL,
	[staff_id_oqc] [varchar](100) NULL,
	[machine_id] [varchar](200) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[remark] [nvarchar](200) NULL,
	[sts_update] [varchar](100) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
	[date_insert] [varchar](25) NULL,
 CONSTRAINT [PK__w_materi__BECFC8F84688C0F6] PRIMARY KEY CLUSTERED 
(
	[wmtid_his] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_info_memo]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_info_memo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[md_cd] [varchar](200) NULL,
	[style_no] [varchar](100) NULL,
	[style_nm] [nvarchar](200) NULL,
	[mt_cd] [varchar](500) NULL,
	[width] [decimal](10, 0) NULL,
	[width_unit] [varchar](3) NULL,
	[spec] [decimal](10, 0) NULL,
	[spec_unit] [varchar](6) NULL,
	[sd_no] [varchar](500) NULL,
	[lot_no] [varchar](500) NULL,
	[sts_cd] [varchar](50) NULL,
	[memo] [nvarchar](2000) NULL,
	[month_excel] [varchar](10) NULL,
	[receiving_dt] [varchar](25) NULL,
	[TX] [int] NULL,
	[total_m] [decimal](10, 0) NULL,
	[total_m2] [decimal](10, 0) NULL,
	[total_ea] [decimal](10, 0) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_materi__3213E83FD12005CC] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_info_tam]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_info_tam](
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
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](14) NULL,
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
	[output_dt] [varchar](14) NULL,
	[input_dt] [varchar](14) NULL,
	[buyer_qr] [varchar](200) NULL,
	[orgin_mt_cd] [varchar](500) NULL,
	[remark] [nvarchar](200) NULL,
	[sts_update] [nvarchar](100) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_materi__14D76DC680CB20CD] PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_info01]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_info01](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mapping]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mapping](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mapping01]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mapping01](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_policy_mt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_policy_mt](
	[wid] [int] IDENTITY(1,1) NOT NULL,
	[policy_code] [varchar](6) NULL,
	[policy_name] [nvarchar](200) NULL,
	[policy_start_dt] [varchar](14) NOT NULL,
	[policy_end_dt] [varchar](14) NOT NULL,
	[work_starttime] [varchar](4) NULL,
	[work_endtime] [varchar](4) NULL,
	[lunch_start_time] [varchar](4) NULL,
	[lunch_end_time] [varchar](4) NULL,
	[dinner_start_time] [varchar](4) NULL,
	[dinner_end_time] [varchar](4) NULL,
	[work_hour] [decimal](4, 2) NULL,
	[use_yn] [char](1) NULL,
	[last_yn] [char](1) NULL,
	[re_mark] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_policy__30F153BB7C0C9443] PRIMARY KEY CLUSTERED 
(
	[wid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_product_qc]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_product_qc](
	[pqno] [int] IDENTITY(1,1) NOT NULL,
	[pq_no] [varchar](11) NULL,
	[ml_no] [varchar](200) NULL,
	[work_dt] [varchar](20) NULL,
	[item_vcd] [varchar](20) NULL,
	[check_qty] [int] NULL,
	[ok_qty] [int] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[pqno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_product_qc_value]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_product_qc_value](
	[pqhno] [int] IDENTITY(1,1) NOT NULL,
	[pq_no] [varchar](11) NULL,
	[item_vcd] [varchar](20) NULL,
	[check_id] [varchar](20) NULL,
	[check_cd] [varchar](20) NULL,
	[check_value] [varchar](500) NULL,
	[check_qty] [int] NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[pqhno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_rd_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_rd_info](
	[rid] [int] IDENTITY(1,1) NOT NULL,
	[rd_no] [varchar](20) NULL,
	[rd_nm] [nvarchar](50) NULL,
	[rd_sts_cd] [varchar](6) NULL,
	[lct_cd] [varchar](18) NULL,
	[receiving_dt] [varchar](8) NULL,
	[remark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_rd_inf__C2B7EDE84BE21E40] PRIMARY KEY CLUSTERED 
(
	[rid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_sd_info]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_sd_info](
	[sid] [int] IDENTITY(1,1) NOT NULL,
	[sd_no] [varchar](20) NULL,
	[sd_nm] [nvarchar](100) NULL,
	[sd_sts_cd] [varchar](6) NULL,
	[product_cd] [varchar](50) NULL,
	[lct_cd] [varchar](18) NULL,
	[alert] [int] NULL,
	[remark] [nvarchar](1000) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_sd_inf__DDDFDD36B6B23806] PRIMARY KEY CLUSTERED 
(
	[sid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_vt_dt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_vt_dt](
	[vdno] [int] IDENTITY(1,1) NOT NULL,
	[vn_cd] [varchar](20) NULL,
	[mt_cd] [varchar](200) NULL,
	[wmtid] [int] NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[staff_id] [varchar](100) NULL,
	[staff_id_oqc] [varchar](100) NULL,
	[machine_id] [varchar](200) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [int] NULL,
	[real_qty] [int] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[output_dt] [varchar](14) NULL,
	[input_dt] [varchar](14) NULL,
	[buyer_qr] [varchar](200) NULL,
	[orgin_mt_cd] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[vdno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_vt_mt]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_vt_mt](
	[vno] [int] IDENTITY(1,1) NOT NULL,
	[vn_cd] [varchar](11) NULL,
	[vn_nm] [nvarchar](50) NULL,
	[start_dt] [varchar](14) NULL,
	[end_dt] [varchar](14) NULL,
	[re_mark] [varchar](500) NULL,
	[use_yn] [char](1) NULL,
	[del_yn] [char](1) NOT NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
 CONSTRAINT [PK__w_vt_mt__DDB7F49D08ABD10B] PRIMARY KEY CLUSTERED 
(
	[vno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo04]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo04](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
/****** Object:  Table [real-autodb].[w_material_infopo05]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo05](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo06]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo06](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
/****** Object:  Table [real-autodb].[w_material_infopo07]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo07](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo08]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo08](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo09]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo09](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo10]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo10](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo11]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo11](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_infopo12]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_infopo12](
	[wmtid] [int] IDENTITY(1,1) NOT NULL,
	[id_actual] [int] NULL,
	[id_actual_oqc] [int] NULL,
	[at_no] [varchar](18) NULL,
	[product] [varchar](100) NULL,
	[staff_id] [varchar](max) NULL,
	[staff_id_oqc] [varchar](max) NULL,
	[machine_id] [varchar](max) NULL,
	[mt_type] [varchar](20) NULL,
	[mt_cd] [varchar](500) NULL,
	[mt_no] [varchar](250) NULL,
	[gr_qty] [float] NULL,
	[real_qty] [float] NULL,
	[staff_qty] [int] NULL,
	[sp_cd] [varchar](500) NULL,
	[rd_no] [varchar](50) NULL,
	[sd_no] [varchar](50) NULL,
	[ext_no] [varchar](50) NULL,
	[ex_no] [varchar](50) NULL,
	[dl_no] [varchar](50) NULL,
	[recevice_dt] [varchar](14) NULL,
	[date] [varchar](14) NULL,
	[return_date] [varchar](14) NULL,
	[alert_NG] [int] NULL,
	[expiry_dt] [varchar](14) NULL,
	[dt_of_receipt] [varchar](14) NULL,
	[expore_dt] [varchar](14) NULL,
	[recevice_dt_tims] [varchar](25) NULL,
	[rece_wip_dt] [varchar](25) NULL,
	[picking_dt] [varchar](25) NULL,
	[shipping_wip_dt] [varchar](25) NULL,
	[end_production_dt] [datetime2](0) NULL,
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
	[ExportCode] [varchar](200) NULL,
	[LoctionMachine] [varchar](50) NULL,
	[ShippingToMachineDatetime] [datetime2](0) NULL,
	[Description] [varchar](500) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NOT NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[wmtid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo04]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo04](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo05]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo05](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo06]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo06](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo07]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo07](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo08]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo08](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo09]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo09](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo10]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo10](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo11]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo11](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [real-autodb].[w_material_mappingpo12]    Script Date: 10/21/2021 9:51:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [real-autodb].[w_material_mappingpo12](
	[wmmid] [int] IDENTITY(1,1) NOT NULL,
	[mt_lot] [varchar](300) NULL,
	[mt_cd] [varchar](300) NULL,
	[mt_no] [varchar](300) NULL,
	[mapping_dt] [varchar](14) NULL,
	[bb_no] [varchar](35) NULL,
	[remark] [varchar](200) NULL,
	[sts_share] [char](1) NULL,
	[use_yn] [char](1) NOT NULL,
	[del_yn] [char](1) NULL,
	[reg_id] [varchar](20) NULL,
	[reg_dt] [datetime2](0) NULL,
	[chg_id] [varchar](20) NULL,
	[chg_dt] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[wmmid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_ac__mn_cd__47A6A41B]  DEFAULT (NULL) FOR [mn_cd]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_ac__url_l__489AC854]  DEFAULT (NULL) FOR [url_link]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_ac__id_bu__498EEC8D]  DEFAULT (NULL) FOR [id_button]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_act__type__4A8310C6]  DEFAULT (NULL) FOR [type]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_ac__name___4B7734FF]  DEFAULT (NULL) FOR [name_table]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_ac__sts_a__4C6B5938]  DEFAULT (NULL) FOR [sts_action]
GO
ALTER TABLE [real-autodb].[author_action] ADD  CONSTRAINT [DF__author_ac__re_ma__4D5F7D71]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__at_cd__503BEA1C]  DEFAULT (NULL) FOR [at_cd]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__at_nm__51300E55]  DEFAULT (NULL) FOR [at_nm]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_inf__role__5224328E]  DEFAULT (NULL) FOR [role]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__use_y__531856C7]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__reg_i__540C7B00]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__reg_d__55009F39]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__chg_i__55F4C372]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__chg_d__56E8E7AB]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[author_info] ADD  CONSTRAINT [DF__author_in__re_ma__57DD0BE4]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__at_cd__5AB9788F]  DEFAULT (NULL) FOR [at_cd]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__mn_cd__5BAD9CC8]  DEFAULT (NULL) FOR [mn_cd]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__mn_nm__5CA1C101]  DEFAULT (NULL) FOR [mn_nm]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__url_l__5D95E53A]  DEFAULT (NULL) FOR [url_link]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__re_ma__5E8A0973]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__use_y__5F7E2DAC]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__st_yn__607251E5]  DEFAULT ('Y') FOR [st_yn]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__ct_yn__6166761E]  DEFAULT ('N') FOR [ct_yn]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__mt_yn__625A9A57]  DEFAULT ('N') FOR [mt_yn]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__del_y__634EBE90]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__reg_i__6442E2C9]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__reg_d__65370702]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__chg_i__662B2B3B]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[author_menu_info] ADD  CONSTRAINT [DF__author_me__chg_d__671F4F74]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__buyer__6AEFE058]  DEFAULT (NULL) FOR [buyer_cd]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__buyer__6BE40491]  DEFAULT (NULL) FOR [buyer_nm]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__ceo_n__6CD828CA]  DEFAULT (NULL) FOR [ceo_nm]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__manag__6DCC4D03]  DEFAULT (NULL) FOR [manager_nm]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__brd_n__6EC0713C]  DEFAULT (NULL) FOR [brd_nm]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_info__logo__6FB49575]  DEFAULT (NULL) FOR [logo]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__phone__70A8B9AE]  DEFAULT (NULL) FOR [phone_nb]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__cell___719CDDE7]  DEFAULT (NULL) FOR [cell_nb]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__fax_n__72910220]  DEFAULT (NULL) FOR [fax_nb]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__e_mai__73852659]  DEFAULT (NULL) FOR [e_mail]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__addre__74794A92]  DEFAULT (NULL) FOR [address]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__web_s__756D6ECB]  DEFAULT (NULL) FOR [web_site]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__re_ma__76619304]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__use_y__7755B73D]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__del_y__7849DB76]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__stamp__793DFFAF]  DEFAULT ('0') FOR [stampId]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__reg_i__7A3223E8]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__reg_d__7B264821]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__chg_i__7C1A6C5A]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[buyer_info] ADD  CONSTRAINT [DF__buyer_inf__chg_d__7D0E9093]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__dt_kr__14E61A24]  DEFAULT (NULL) FOR [dt_kr]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__dt_vn__15DA3E5D]  DEFAULT (NULL) FOR [dt_vn]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__dt_exp__16CE6296]  DEFAULT (NULL) FOR [dt_exp]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__up_cd__17C286CF]  DEFAULT (NULL) FOR [up_cd]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val1__18B6AB08]  DEFAULT (NULL) FOR [val1]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val1_nm__19AACF41]  DEFAULT (NULL) FOR [val1_nm]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val2__1A9EF37A]  DEFAULT (NULL) FOR [val2]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val2_nm__1B9317B3]  DEFAULT (NULL) FOR [val2_nm]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val3__1C873BEC]  DEFAULT (NULL) FOR [val3]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val3_nm__1D7B6025]  DEFAULT (NULL) FOR [val3_nm]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val4__1E6F845E]  DEFAULT (NULL) FOR [val4]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__val4_nm__1F63A897]  DEFAULT (NULL) FOR [val4_nm]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__dt_orde__2057CCD0]  DEFAULT ('1') FOR [dt_order]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__use_yn__214BF109]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__del_yn__22401542]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__reg_id__2334397B]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__reg_dt__24285DB4]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__chg_id__251C81ED]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__chg_dt__2610A626]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[comm_dt] ADD  CONSTRAINT [DF__comm_dt__unit__2704CA5F]  DEFAULT (NULL) FOR [unit]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__div_cd__29E1370A]  DEFAULT (NULL) FOR [div_cd]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__mt_cd__2AD55B43]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__mt_nm__2BC97F7C]  DEFAULT (NULL) FOR [mt_nm]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__mt_exp__2CBDA3B5]  DEFAULT (NULL) FOR [mt_exp]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__memo__2DB1C7EE]  DEFAULT (NULL) FOR [memo]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__use_yn__2EA5EC27]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__reg_id__2F9A1060]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__reg_dt__308E3499]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__chg_id__318258D2]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[comm_mt] ADD  CONSTRAINT [DF__comm_mt__chg_dt__32767D0B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___mc_ty__36470DEF]  DEFAULT (NULL) FOR [mc_type]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___bb_no__373B3228]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___mt_cd__382F5661]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___bb_nm__39237A9A]  DEFAULT (NULL) FOR [bb_nm]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___purpo__3A179ED3]  DEFAULT (NULL) FOR [purpose]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___barco__3B0BC30C]  DEFAULT (NULL) FOR [barcode]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___re_ma__3BFFE745]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___use_y__3CF40B7E]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___count__3DE82FB7]  DEFAULT ('0') FOR [count_number]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___del_y__3EDC53F0]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___reg_i__3FD07829]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___reg_d__40C49C62]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___chg_i__41B8C09B]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_bobbin_info] ADD  CONSTRAINT [DF__d_bobbin___chg_d__42ACE4D4]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___mc_ty__4589517F]  DEFAULT (NULL) FOR [mc_type]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___bb_no__467D75B8]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___bb_nm__477199F1]  DEFAULT (NULL) FOR [bb_nm]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___start__4865BE2A]  DEFAULT ('20191007010159') FOR [start_dt]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___end_d__4959E263]  DEFAULT ('99991231235959') FOR [end_dt]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___use_y__4A4E069C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___del_y__4B422AD5]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___reg_i__4C364F0E]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___reg_d__4D2A7347]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___chg_i__4E1E9780]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_bobbin_lct_hist] ADD  CONSTRAINT [DF__d_bobbin___chg_d__4F12BBB9]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [bom_no]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [style_no]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [need_time]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT ('1') FOR [cav]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [need_m]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [buocdap]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT ('N') FOR [IsApply]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT ('0') FOR [IsActive]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_bom_info] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__mc_ty__61316BF4]  DEFAULT (NULL) FOR [mc_type]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__mc_no__6225902D]  DEFAULT (NULL) FOR [mc_no]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__mc_nm__6319B466]  DEFAULT (NULL) FOR [mc_nm]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__purpo__640DD89F]  DEFAULT (NULL) FOR [purpose]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__color__6501FCD8]  DEFAULT (NULL) FOR [color]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__barco__65F62111]  DEFAULT (NULL) FOR [barcode]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__re_ma__66EA454A]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__use_y__67DE6983]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__del_y__68D28DBC]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__reg_i__69C6B1F5]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__reg_d__6ABAD62E]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__chg_i__6BAEFA67]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_machine_info] ADD  CONSTRAINT [DF__d_machine__chg_d__6CA31EA0]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__mt_ty__6F7F8B4B]  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__mt_cd__7073AF84]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__mt_no__7167D3BD]  DEFAULT (NULL) FOR [mt_no_origin]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__mt_nm__725BF7F6]  DEFAULT (NULL) FOR [mt_nm]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__mf_cd__73501C2F]  DEFAULT (NULL) FOR [mf_cd]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__gr_qt__74444068]  DEFAULT ('0') FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__unit___753864A1]  DEFAULT ('EA') FOR [unit_cd]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__bundl__762C88DA]  DEFAULT ('1') FOR [bundle_qty]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__bundl__7720AD13]  DEFAULT ('Roll') FOR [bundle_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__sp_cd__7814D14C]  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__s_lot__7908F585]  DEFAULT (NULL) FOR [s_lot_no]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__item___79FD19BE]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__qc_ra__7AF13DF7]  DEFAULT (NULL) FOR [qc_range_cd]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__width__7BE56230]  DEFAULT (NULL) FOR [width]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__width__7CD98669]  DEFAULT (NULL) FOR [width_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_material__spec__7DCDAAA2]  DEFAULT (NULL) FOR [spec]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__spec___7EC1CEDB]  DEFAULT (NULL) FOR [spec_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_material__area__7FB5F314]  DEFAULT (NULL) FOR [area]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__area___00AA174D]  DEFAULT (NULL) FOR [area_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__thick__019E3B86]  DEFAULT (NULL) FOR [thick]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__thick__02925FBF]  DEFAULT (NULL) FOR [thick_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__stick__038683F8]  DEFAULT (NULL) FOR [stick]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__stick__047AA831]  DEFAULT (NULL) FOR [stick_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__consu__056ECC6A]  DEFAULT ('N') FOR [consum_yn]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__price__0662F0A3]  DEFAULT (NULL) FOR [price]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__tot_p__075714DC]  DEFAULT (NULL) FOR [tot_price]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__price__084B3915]  DEFAULT (NULL) FOR [price_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__price__093F5D4E]  DEFAULT (NULL) FOR [price_least_unit]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__photo__0A338187]  DEFAULT (NULL) FOR [photo_file]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__re_ma__0B27A5C0]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__use_y__0C1BC9F9]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__del_y__0D0FEE32]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__barco__0E04126B]  DEFAULT (NULL) FOR [barcode]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__reg_i__0EF836A4]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__reg_d__0FEC5ADD]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__chg_i__10E07F16]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_material_info] ADD  CONSTRAINT [DF__d_materia__chg_d__11D4A34F]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__md_cd__53D770D6]  DEFAULT (NULL) FOR [md_cd]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__md_nm__54CB950F]  DEFAULT (NULL) FOR [md_nm]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__use_y__55BFB948]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__del_y__56B3DD81]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__reg_i__57A801BA]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__reg_d__589C25F3]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__chg_i__59904A2C]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_model_info] ADD  CONSTRAINT [DF__d_model_i__chg_d__5A846E65]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__md_no__5B78929E]  DEFAULT (NULL) FOR [md_no]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__md_nm__5C6CB6D7]  DEFAULT (NULL) FOR [md_nm]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__purpo__5D60DB10]  DEFAULT (NULL) FOR [purpose]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__barco__5E54FF49]  DEFAULT (NULL) FOR [barcode]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__re_ma__5F492382]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__use_y__603D47BB]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__del_y__61316BF4]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__reg_i__6225902D]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__reg_d__6319B466]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__chg_i__640DD89F]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_mold_info] ADD  CONSTRAINT [DF__d_mold_in__chg_d__6501FCD8]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__id_ac__65F62111]  DEFAULT (NULL) FOR [id_actual]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__start__66EA454A]  DEFAULT ('20190101000001') FOR [start_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__end_d__67DE6983]  DEFAULT ('99991231235959') FOR [end_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__remar__68D28DBC]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__mc_no__69C6B1F5]  DEFAULT (NULL) FOR [mc_no]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__use_y__6ABAD62E]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__del_y__6BAEFA67]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__reg_i__6CA31EA0]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__reg_d__6D9742D9]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__chg_i__6E8B6712]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_pro_unit_mc] ADD  CONSTRAINT [DF__d_pro_uni__chg_d__6F7F8B4B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [staff_id]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [actual]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [defect]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [id_actual]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [staff_tp]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT ('20190101000001') FOR [start_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT ('99991231235959') FOR [end_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_pro_unit_staff] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__style__45544755]  DEFAULT (NULL) FOR [style_no]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__proce__46486B8E]  DEFAULT (NULL) FOR [process_code]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rounting__name__473C8FC7]  DEFAULT (NULL) FOR [name]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__level__4830B400]  DEFAULT (NULL) FOR [level]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__don_v__4924D839]  DEFAULT (NULL) FOR [don_vi_pr]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rounting__type__4A18FC72]  DEFAULT (NULL) FOR [type]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__item___4B0D20AB]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__descr__4C0144E4]  DEFAULT (NULL) FOR [description]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__isFin__4CF5691D]  DEFAULT ('N') FOR [isFinish]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__reg_d__4DE98D56]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__reg_i__4EDDB18F]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__chg_i__4FD1D5C8]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_rounting_info] ADD  CONSTRAINT [DF__d_rountin__chg_d__50C5FA01]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__style__703EA55A]  DEFAULT (NULL) FOR [style_no]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__style__7132C993]  DEFAULT (NULL) FOR [style_nm]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__md_cd__7226EDCC]  DEFAULT (NULL) FOR [md_cd]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__prj_n__731B1205]  DEFAULT (NULL) FOR [prj_nm]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__ssver__740F363E]  DEFAULT (NULL) FOR [ssver]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__part___75035A77]  DEFAULT (NULL) FOR [part_nm]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__stand__75F77EB0]  DEFAULT (NULL) FOR [standard]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__cust___76EBA2E9]  DEFAULT (NULL) FOR [cust_rev]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__order__77DFC722]  DEFAULT (NULL) FOR [order_num]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__pack___78D3EB5B]  DEFAULT (NULL) FOR [pack_amt]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_inf__cav__79C80F94]  DEFAULT (NULL) FOR [cav]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__bom_t__7ABC33CD]  DEFAULT (NULL) FOR [bom_type]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__tds_n__7BB05806]  DEFAULT (NULL) FOR [tds_no]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__item___7CA47C3F]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__qc_ra__7D98A078]  DEFAULT (NULL) FOR [qc_range_cd]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__stamp__7E8CC4B1]  DEFAULT (NULL) FOR [stamp_code]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__expir__7F80E8EA]  DEFAULT (NULL) FOR [expiry_month]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__expir__00750D23]  DEFAULT (NULL) FOR [expiry]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__use_y__0169315C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__del_y__025D5595]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__reg_i__035179CE]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__reg_d__04459E07]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__chg_i__0539C240]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__chg_d__062DE679]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__drawi__07220AB2]  DEFAULT ('') FOR [drawingname]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_in__loss__08162EEB]  DEFAULT ('') FOR [loss]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__produ__090A5324]  DEFAULT ('0') FOR [productType]
GO
ALTER TABLE [real-autodb].[d_style_info] ADD  CONSTRAINT [DF__d_style_i__Descr__09FE775D]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__depar__0CDAE408]  DEFAULT (NULL) FOR [depart_cd]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__depar__0DCF0841]  DEFAULT (NULL) FOR [depart_nm]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__up_de__0EC32C7A]  DEFAULT (NULL) FOR [up_depart_cd]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__level__0FB750B3]  DEFAULT (NULL) FOR [level_cd]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__re_ma__10AB74EC]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__use_y__119F9925]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__order__1293BD5E]  DEFAULT (NULL) FOR [order_no]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__del_y__1387E197]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__reg_i__147C05D0]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__reg_d__15702A09]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__chg_i__16644E42]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__chg_d__1758727B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[department_info] ADD  CONSTRAINT [DF__departmen__mn_fu__184C96B4]  DEFAULT (NULL) FOR [mn_full]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Expor__1B29035F]  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Produ__1C1D2798]  DEFAULT (NULL) FOR [ProductCode]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Produ__1D114BD1]  DEFAULT (NULL) FOR [ProductName]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Machi__1E05700A]  DEFAULT (NULL) FOR [MachineCode]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__IsFin__1EF99443]  DEFAULT (NULL) FOR [IsFinish]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Descr__1FEDB87C]  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Creat__20E1DCB5]  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Creat__21D600EE]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Chang__22CA2527]  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[exporttomachine] ADD  CONSTRAINT [DF__exporttom__Chang__23BE4960]  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [product_code]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT ('SAP') FOR [type]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [md_cd]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT ('0') FOR [qty]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [sts_cd]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[generalfg] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__lct_cd__473C8FC7]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__lct_nm__4830B400]  DEFAULT (NULL) FOR [lct_nm]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__up_lct__4924D839]  DEFAULT (NULL) FOR [up_lct_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__level___4A18FC72]  DEFAULT (NULL) FOR [level_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__index___4B0D20AB]  DEFAULT (NULL) FOR [index_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__shelf___4C0144E4]  DEFAULT (NULL) FOR [shelf_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__order___4CF5691D]  DEFAULT ('1') FOR [order_no]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__real_u__4DE98D56]  DEFAULT ('N') FOR [real_use_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__re_mar__4EDDB18F]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__use_yn__4FD1D5C8]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__lct_rf__50C5FA01]  DEFAULT (NULL) FOR [lct_rfid]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__lct_ba__51BA1E3A]  DEFAULT (NULL) FOR [lct_bar_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__sf_yn__52AE4273]  DEFAULT ('N') FOR [sf_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__is_yn__53A266AC]  DEFAULT ('N') FOR [is_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__mt_yn__54968AE5]  DEFAULT ('N') FOR [mt_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__mv_yn__558AAF1E]  DEFAULT ('N') FOR [mv_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__ti_yn__567ED357]  DEFAULT ('N') FOR [ti_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__fg_yn__5772F790]  DEFAULT ('N') FOR [fg_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__rt_yn__58671BC9]  DEFAULT ('N') FOR [rt_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__ft_yn__595B4002]  DEFAULT ('N') FOR [ft_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__wp_yn__5A4F643B]  DEFAULT ('N') FOR [wp_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__nt_yn__5B438874]  DEFAULT ('N') FOR [nt_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__pk_yn__5C37ACAD]  DEFAULT ('N') FOR [pk_yn]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__manage__5D2BD0E6]  DEFAULT (NULL) FOR [manager_id]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__reg_id__5E1FF51F]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__reg_dt__5F141958]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__chg_id__60083D91]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__chg_dt__60FC61CA]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__mn_ful__61F08603]  DEFAULT (NULL) FOR [mn_full]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__sap_lc__62E4AA3C]  DEFAULT (NULL) FOR [sap_lct_cd]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__userid__63D8CE75]  DEFAULT (NULL) FOR [userid]
GO
ALTER TABLE [real-autodb].[lct_info] ADD  CONSTRAINT [DF__lct_info__select__64CCF2AE]  DEFAULT (NULL) FOR [selected]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__viewcnt__65C116E7]  DEFAULT ('0') FOR [viewcnt]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__replycn__66B53B20]  DEFAULT ('0') FOR [replycnt]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__div_cd__67A95F59]  DEFAULT ('A') FOR [div_cd]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__start_d__689D8392]  DEFAULT (NULL) FOR [start_dt]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__end_dt__6991A7CB]  DEFAULT (NULL) FOR [end_dt]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__widthsi__6A85CC04]  DEFAULT (NULL) FOR [widthsize]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__heights__6B79F03D]  DEFAULT (NULL) FOR [heightsize]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__back_co__6C6E1476]  DEFAULT ('#FFFFFF') FOR [back_color]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__order_n__6D6238AF]  DEFAULT ('0') FOR [order_no]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__del_yn__6E565CE8]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__reg_id__6F4A8121]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__reg_dt__703EA55A]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__chg_id__7132C993]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[m_board] ADD  CONSTRAINT [DF__m_board__chg_dt__7226EDCC]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__fq_no__68687968]  DEFAULT (NULL) FOR [fq_no]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__ml_no__695C9DA1]  DEFAULT (NULL) FOR [ml_no]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__ml_ti__6A50C1DA]  DEFAULT (NULL) FOR [ml_tims]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__produ__6B44E613]  DEFAULT (NULL) FOR [product_cd]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__shift__6C390A4C]  DEFAULT (NULL) FOR [shift]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__at_no__6D2D2E85]  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__work___6E2152BE]  DEFAULT (NULL) FOR [work_dt]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__item___6F1576F7]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__item___70099B30]  DEFAULT (NULL) FOR [item_nm]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__item___70FDBF69]  DEFAULT (NULL) FOR [item_exp]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__check__71F1E3A2]  DEFAULT ('1') FOR [check_qty]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__ok_qt__72E607DB]  DEFAULT ('0') FOR [ok_qty]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__reg_i__73DA2C14]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__reg_d__74CE504D]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__chg_i__75C27486]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[m_facline_qc] ADD  CONSTRAINT [DF__m_facline__chg_d__76B698BF]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__fq_no__7993056A]  DEFAULT (NULL) FOR [fq_no]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__produ__7A8729A3]  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__at_no__7B7B4DDC]  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__shift__7C6F7215]  DEFAULT (NULL) FOR [shift]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__item___7D63964E]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__check__7E57BA87]  DEFAULT (NULL) FOR [check_id]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__check__7F4BDEC0]  DEFAULT (NULL) FOR [check_cd]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__check__004002F9]  DEFAULT (NULL) FOR [check_value]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__check__01342732]  DEFAULT ('1') FOR [check_qty]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__date___02284B6B]  DEFAULT (NULL) FOR [date_ymd]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__reg_i__031C6FA4]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__reg_d__041093DD]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__chg_i__0504B816]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[m_facline_qc_value] ADD  CONSTRAINT [DF__m_facline__chg_d__05F8DC4F]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__mf_cd__0FB750B3]  DEFAULT (NULL) FOR [mf_cd]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__mf_nm__10AB74EC]  DEFAULT (NULL) FOR [mf_nm]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__brd_n__119F9925]  DEFAULT (NULL) FOR [brd_nm]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_in__logo__1293BD5E]  DEFAULT (NULL) FOR [logo]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__phone__1387E197]  DEFAULT (NULL) FOR [phone_nb]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__web_s__147C05D0]  DEFAULT (NULL) FOR [web_site]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__addre__15702A09]  DEFAULT (NULL) FOR [address]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__re_ma__16644E42]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__use_y__1758727B]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__del_y__184C96B4]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__reg_i__1940BAED]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__reg_d__1A34DF26]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__chg_i__1B29035F]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[manufac_info] ADD  CONSTRAINT [DF__manufac_i__chg_d__1C1D2798]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[materialbom] ADD  DEFAULT ('0') FOR [ProductCode]
GO
ALTER TABLE [real-autodb].[materialbom] ADD  DEFAULT ('0') FOR [MaterialPrarent]
GO
ALTER TABLE [real-autodb].[materialbom] ADD  DEFAULT ('') FOR [CreateId]
GO
ALTER TABLE [real-autodb].[materialbom] ADD  DEFAULT ('b''0''') FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[materialbom] ADD  DEFAULT ('0000-00-00 00:00:00') FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[materialbom] ADD  DEFAULT ('0000-00-00 00:00:00') FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__useri__22CA2527]  DEFAULT (NULL) FOR [userid]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__at_cd__23BE4960]  DEFAULT (NULL) FOR [at_cd]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__re_ma__24B26D99]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__use_y__25A691D2]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__reg_i__269AB60B]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__reg_d__278EDA44]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__chg_i__2882FE7D]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[mb_author_info] ADD  CONSTRAINT [DF__mb_author__chg_d__297722B6]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__uname__2A363CC5]  DEFAULT (NULL) FOR [uname]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__nick_na__2B2A60FE]  DEFAULT (NULL) FOR [nick_name]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__upw__2C1E8537]  DEFAULT (NULL) FOR [upw]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__grade__2D12A970]  DEFAULT (NULL) FOR [grade]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__depart___2E06CDA9]  DEFAULT (NULL) FOR [depart_cd]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__gender__2EFAF1E2]  DEFAULT ('M') FOR [gender]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__positio__2FEF161B]  DEFAULT (NULL) FOR [position_cd]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__tel_nb__30E33A54]  DEFAULT (NULL) FOR [tel_nb]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__cel_nb__31D75E8D]  DEFAULT (NULL) FOR [cel_nb]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__e_mail__32CB82C6]  DEFAULT (NULL) FOR [e_mail]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__sms_yn__33BFA6FF]  DEFAULT ('N') FOR [sms_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__join_dt__34B3CB38]  DEFAULT (NULL) FOR [join_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__birth_d__35A7EF71]  DEFAULT (NULL) FOR [birth_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__scr_yn__369C13AA]  DEFAULT ('N') FOR [scr_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__mail_yn__379037E3]  DEFAULT ('N') FOR [mail_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__join_ip__38845C1C]  DEFAULT (NULL) FOR [join_ip]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__join_do__39788055]  DEFAULT (NULL) FOR [join_domain]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__ltacc_d__3A6CA48E]  DEFAULT (getdate()) FOR [ltacc_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__ltacc_d__3B60C8C7]  DEFAULT (NULL) FOR [ltacc_domain]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__mbout_d__3C54ED00]  DEFAULT (getdate()) FOR [mbout_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__mbout_y__3D491139]  DEFAULT ('N') FOR [mbout_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__accbloc__3E3D3572]  DEFAULT ('N') FOR [accblock_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__session__3F3159AB]  DEFAULT ('none') FOR [session_key]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__session__40257DE4]  DEFAULT (getdate()) FOR [session_limit]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__memo__4119A21D]  DEFAULT (NULL) FOR [memo]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__del_yn__420DC656]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__check_y__4301EA8F]  DEFAULT ('N') FOR [check_yn]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__rem_me__43F60EC8]  DEFAULT (NULL) FOR [rem_me]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__barcode__44EA3301]  DEFAULT (NULL) FOR [barcode]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__mbjoin___45DE573A]  DEFAULT (getdate()) FOR [mbjoin_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__log_ip__46D27B73]  DEFAULT (NULL) FOR [log_ip]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__lct_cd__47C69FAC]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__reg_id__48BAC3E5]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__reg_dt__49AEE81E]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__chg_id__4AA30C57]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__chg_dt__4B973090]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[mb_info] ADD  CONSTRAINT [DF__mb_info__re_mark__4C8B54C9]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__useri__4DB4832C]  DEFAULT (NULL) FOR [userid]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__lct_c__4EA8A765]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__re_ma__4F9CCB9E]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__use_y__5090EFD7]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__reg_i__51851410]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__reg_d__52793849]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__chg_i__536D5C82]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[mb_lct_info] ADD  CONSTRAINT [DF__mb_lct_in__chg_d__546180BB]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[mb_message] ADD  CONSTRAINT [DF__mb_messag__messa__5555A4F4]  DEFAULT (NULL) FOR [message]
GO
ALTER TABLE [real-autodb].[mb_message] ADD  CONSTRAINT [DF__mb_messag__del_y__5649C92D]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[mb_message] ADD  CONSTRAINT [DF__mb_messag__reg_d__573DED66]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[mb_message] ADD  CONSTRAINT [DF__mb_messag__reg_i__5832119F]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__mn_cd__5EAA0504]  DEFAULT (NULL) FOR [mn_cd]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__mn_nm__5F9E293D]  DEFAULT (NULL) FOR [mn_nm]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__up_mn__60924D76]  DEFAULT (NULL) FOR [up_mn_cd]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__level__618671AF]  DEFAULT (NULL) FOR [level_cd]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__url_l__627A95E8]  DEFAULT (NULL) FOR [url_link]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__re_ma__636EBA21]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__col_c__6462DE5A]  DEFAULT ('fa-th') FOR [col_css]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__sub_y__65570293]  DEFAULT ('N') FOR [sub_yn]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__order__664B26CC]  DEFAULT ('0') FOR [order_no]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__use_y__673F4B05]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__mn_fu__68336F3E]  DEFAULT (NULL) FOR [mn_full]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__mn_cd__69279377]  DEFAULT (NULL) FOR [mn_cd_full]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__reg_i__6A1BB7B0]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__reg_d__6B0FDBE9]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__chg_i__6C040022]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__chg_d__6CF8245B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[menu_info] ADD  CONSTRAINT [DF__menu_info__selec__6DEC4894]  DEFAULT (NULL) FOR [selected]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__title__70C8B53F]  DEFAULT (NULL) FOR [title]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__mn_cd__71BCD978]  DEFAULT (NULL) FOR [mn_cd]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__viewc__72B0FDB1]  DEFAULT ('0') FOR [viewcnt]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__reply__73A521EA]  DEFAULT ('0') FOR [replycnt]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__div_c__74994623]  DEFAULT ('A') FOR [div_cd]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__lng_c__758D6A5C]  DEFAULT ('EN') FOR [lng_cd]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__start__76818E95]  DEFAULT (NULL) FOR [start_dt]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__end_d__7775B2CE]  DEFAULT (NULL) FOR [end_dt]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__width__7869D707]  DEFAULT (NULL) FOR [widthsize]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__heigh__795DFB40]  DEFAULT (NULL) FOR [heightsize]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__back___7A521F79]  DEFAULT ('#FFFFFF') FOR [back_color]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__order__7B4643B2]  DEFAULT ('0') FOR [order_no]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__del_y__7C3A67EB]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__reg_i__7D2E8C24]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__reg_d__7E22B05D]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__chg_i__7F16D496]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[notice_board] ADD  CONSTRAINT [DF__notice_bo__chg_d__000AF8CF]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__style__7993056A]  DEFAULT (NULL) FOR [style_no]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__proce__7A8729A3]  DEFAULT ('1') FOR [process_code]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__level__7B7B4DDC]  DEFAULT (NULL) FOR [level]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_ma__name__7C6F7215]  DEFAULT (NULL) FOR [name]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__mt_no__7D63964E]  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__need___7E57BA87]  DEFAULT (NULL) FOR [need_time]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_mat__cav__7F4BDEC0]  DEFAULT ('1') FOR [cav]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__need___004002F9]  DEFAULT (NULL) FOR [need_m]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__buocd__01342732]  DEFAULT (NULL) FOR [buocdap]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__isAct__02284B6B]  DEFAULT ('N') FOR [isActive]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__reg_i__031C6FA4]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__reg_d__041093DD]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__chg_i__0504B816]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[product_material] ADD  CONSTRAINT [DF__product_m__chg_d__05F8DC4F]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__Produ__06ED0088]  DEFAULT ('0') FOR [ProductCode]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__proce__07E124C1]  DEFAULT ('1') FOR [process_code]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__level__08D548FA]  DEFAULT ('0') FOR [level]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_ma__name__09C96D33]  DEFAULT ('0') FOR [name]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__Mater__0ABD916C]  DEFAULT ('0') FOR [MaterialPrarent]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__Creat__0BB1B5A5]  DEFAULT ('') FOR [CreateId]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__Chang__0CA5D9DE]  DEFAULT ('b''0''') FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__Creat__0D99FE17]  DEFAULT ('0000-00-00 00:00:00') FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[product_material_detail] ADD  CONSTRAINT [DF__product_m__Chang__0E8E2250]  DEFAULT ('0000-00-00 00:00:00') FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__style__0F824689]  DEFAULT (NULL) FOR [style_no]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__proce__10766AC2]  DEFAULT (NULL) FOR [process_code]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__IsApp__116A8EFB]  DEFAULT ('N') FOR [IsApply]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__proce__125EB334]  DEFAULT (NULL) FOR [process_name]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__descr__1352D76D]  DEFAULT (NULL) FOR [description]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__reg_d__1446FBA6]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__reg_i__153B1FDF]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__chg_i__162F4418]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[product_routing] ADD  CONSTRAINT [DF__product_r__chg_d__17236851]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__item___2A01329B]  DEFAULT (NULL) FOR [item_type]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__item___2AF556D4]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__item___2BE97B0D]  DEFAULT (NULL) FOR [item_cd]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_mt__ver__2CDD9F46]  DEFAULT (NULL) FOR [ver]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__item___2DD1C37F]  DEFAULT (NULL) FOR [item_nm]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__item___2EC5E7B8]  DEFAULT (NULL) FOR [item_exp]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__use_y__2FBA0BF1]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__del_y__30AE302A]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__reg_i__31A25463]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__reg_d__3296789C]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__chg_i__338A9CD5]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[qc_item_mt] ADD  CONSTRAINT [DF__qc_item_m__chg_d__347EC10E]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__item___375B2DB9]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__check__384F51F2]  DEFAULT (NULL) FOR [check_id]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__check__3943762B]  DEFAULT (NULL) FOR [check_cd]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__defec__3A379A64]  DEFAULT ('Y') FOR [defect_yn]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__check__3B2BBE9D]  DEFAULT (NULL) FOR [check_name]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__order__3C1FE2D6]  DEFAULT ('1') FOR [order_no]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__re_ma__3D14070F]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__use_y__3E082B48]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__del_y__3EFC4F81]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__reg_i__3FF073BA]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__reg_d__40E497F3]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__chg_i__41D8BC2C]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_dt] ADD  CONSTRAINT [DF__qc_itemch__chg_d__42CCE065]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__item___45A94D10]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__check__469D7149]  DEFAULT (NULL) FOR [check_id]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__check__47919582]  DEFAULT (NULL) FOR [check_type]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__check__4885B9BB]  DEFAULT (NULL) FOR [check_subject]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__min_v__4979DDF4]  DEFAULT ('0.0000000') FOR [min_value]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__max_v__4A6E022D]  DEFAULT ('0.0000000') FOR [max_value]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__range__4B622666]  DEFAULT (NULL) FOR [range_type]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__order__4C564A9F]  DEFAULT ('1') FOR [order_no]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__re_ma__4D4A6ED8]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__use_y__4E3E9311]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__del_y__4F32B74A]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__reg_i__5026DB83]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__reg_d__511AFFBC]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__chg_i__520F23F5]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[qc_itemcheck_mt] ADD  CONSTRAINT [DF__qc_itemch__chg_d__5303482E]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Shipp__3E3D3572]  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Produ__3F3159AB]  DEFAULT (NULL) FOR [ProductCode]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Produ__40257DE4]  DEFAULT (NULL) FOR [ProductName]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__IsFin__4119A21D]  DEFAULT (NULL) FOR [IsFinish]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Descr__420DC656]  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Creat__4301EA8F]  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Creat__43F60EC8]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Chang__44EA3301]  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[shippingfgsorting] ADD  CONSTRAINT [DF__shippingf__Chang__45DE573A]  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [productCode]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [Model]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [location]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [Quantity]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[shippingfgsortingdetail] ADD  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[shippingsdmaterial] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[shippingsdmaterial] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[shippingsdmaterial] ADD  DEFAULT (NULL) FOR [quantity]
GO
ALTER TABLE [real-autodb].[shippingsdmaterial] ADD  DEFAULT (NULL) FOR [meter]
GO
ALTER TABLE [real-autodb].[shippingsdmaterial] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[shippingsdmaterial] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Shipp__5708E33C]  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Produ__57FD0775]  DEFAULT (NULL) FOR [ProductCode]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Produ__58F12BAE]  DEFAULT (NULL) FOR [ProductName]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__IsFin__59E54FE7]  DEFAULT (NULL) FOR [IsFinish]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Descr__5AD97420]  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Creat__5BCD9859]  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Creat__5CC1BC92]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Chang__5DB5E0CB]  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[shippingtimssorting] ADD  CONSTRAINT [DF__shippingt__Chang__5EAA0504]  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [ShippingCode]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [productCode]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [Model]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [location]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [Quantity]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [CreateId]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (NULL) FOR [ChangeId]
GO
ALTER TABLE [real-autodb].[shippingtimssortingdetail] ADD  DEFAULT (getdate()) FOR [ChangeDate]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT (NULL) FOR [ssver]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('DZIH') FOR [vendor_code]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('A') FOR [vendor_line]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('1') FOR [label_printer]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('N') FOR [is_sample]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('0') FOR [pcn]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('') FOR [lot_date]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('001') FOR [serial_number]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('01') FOR [machine_line]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('0') FOR [shift]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('0') FOR [standard_qty]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT ('N') FOR [is_sent]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT (NULL) FOR [box_code]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[stamp_detail] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[stamp_master] ADD  CONSTRAINT [DF__stamp_mas__stamp__7A521F79]  DEFAULT ('') FOR [stamp_name]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___sp_cd__7B4643B2]  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___sp_nm__7C3A67EB]  DEFAULT (NULL) FOR [sp_nm]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___bsn_t__7D2E8C24]  DEFAULT (NULL) FOR [bsn_tp]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___chang__7E22B05D]  DEFAULT (NULL) FOR [changer_id]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___phone__7F16D496]  DEFAULT (NULL) FOR [phone_nb]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___cell___000AF8CF]  DEFAULT (NULL) FOR [cell_nb]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___fax_n__00FF1D08]  DEFAULT (NULL) FOR [fax_nb]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___e_mai__01F34141]  DEFAULT (NULL) FOR [e_mail]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___web_s__02E7657A]  DEFAULT (NULL) FOR [web_site]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___addre__03DB89B3]  DEFAULT (NULL) FOR [address]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___re_ma__04CFADEC]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___use_y__05C3D225]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___del_y__06B7F65E]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___reg_i__07AC1A97]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___reg_d__08A03ED0]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___chg_i__09946309]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[supplier_info] ADD  CONSTRAINT [DF__supplier___chg_d__0A888742]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[user_author] ADD  CONSTRAINT [DF__user_auth__useri__0B7CAB7B]  DEFAULT (NULL) FOR [userid]
GO
ALTER TABLE [real-autodb].[user_author] ADD  CONSTRAINT [DF__user_auth__at_nm__0C70CFB4]  DEFAULT (NULL) FOR [at_nm]
GO
ALTER TABLE [real-autodb].[user_author] ADD  CONSTRAINT [DF__user_auth__reg_d__0D64F3ED]  DEFAULT (NULL) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[user_author] ADD  CONSTRAINT [DF__user_auth__chg_d__0E591826]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[version_app] ADD  CONSTRAINT [DF__version_ap__type__0F4D3C5F]  DEFAULT (NULL) FOR [type]
GO
ALTER TABLE [real-autodb].[version_app] ADD  CONSTRAINT [DF__version_a__name___10416098]  DEFAULT (NULL) FOR [name_file]
GO
ALTER TABLE [real-autodb].[version_app] ADD  CONSTRAINT [DF__version_a__versi__113584D1]  DEFAULT ('0') FOR [version]
GO
ALTER TABLE [real-autodb].[version_app] ADD  CONSTRAINT [DF__version_a__chg_d__1229A90A]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__at_no__131DCD43]  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__type__1411F17C]  DEFAULT (NULL) FOR [type]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__produc__150615B5]  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__actual__15FA39EE]  DEFAULT (NULL) FOR [actual]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__defect__16EE5E27]  DEFAULT (NULL) FOR [defect]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__name__17E28260]  DEFAULT (NULL) FOR [name]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__level__18D6A699]  DEFAULT (NULL) FOR [level]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__date__19CACAD2]  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__don_vi__1ABEEF0B]  DEFAULT (NULL) FOR [don_vi_pr]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__item_v__1BB31344]  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__descri__1CA7377D]  DEFAULT (NULL) FOR [description]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__IsFini__1D9B5BB6]  DEFAULT ('0') FOR [IsFinished]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__reg_id__1E8F7FEF]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__reg_dt__1F83A428]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__chg_id__2077C861]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_actual] ADD  CONSTRAINT [DF__w_actual__chg_dt__216BEC9A]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___at_no__0A1E72EE]  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual_p__type__0B129727]  DEFAULT (NULL) FOR [type]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___targe__0C06BB60]  DEFAULT (NULL) FOR [target]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___produ__0CFADF99]  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___proce__0DEF03D2]  DEFAULT (NULL) FOR [process_code]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___remar__0EE3280B]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___finis__0FD74C44]  DEFAULT ('N') FOR [finish_yn]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___IsApp__10CB707D]  DEFAULT ('N') FOR [IsApply]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___IsMov__11BF94B6]  DEFAULT ('0') FOR [IsMove]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___reg_i__12B3B8EF]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___reg_d__13A7DD28]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___chg_i__149C0161]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_actual_primary] ADD  CONSTRAINT [DF__w_actual___chg_d__1590259A]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__bx_no__2EC5E7B8]  DEFAULT (NULL) FOR [bx_no]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__buyer__2FBA0BF1]  DEFAULT (NULL) FOR [buyer_cd]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__mt_cd__30AE302A]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__gr_qt__31A25463]  DEFAULT ('0') FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__produ__3296789C]  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_mapp__type__338A9CD5]  DEFAULT (NULL) FOR [type]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__mappi__347EC10E]  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_mappi__sts__3572E547]  DEFAULT (NULL) FOR [sts]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__use_y__36670980]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__del_y__375B2DB9]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__reg_i__384F51F2]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__reg_d__3943762B]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__chg_i__3A379A64]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_box_mapping] ADD  CONSTRAINT [DF__w_box_map__chg_d__3B2BBE9D]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__dl_no__27AED5D5]  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__dl_nm__28A2FA0E]  DEFAULT (NULL) FOR [dl_nm]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__dl_st__29971E47]  DEFAULT (NULL) FOR [dl_sts_cd]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__work___2A8B4280]  DEFAULT (NULL) FOR [work_dt]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__lct_c__2B7F66B9]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__remar__2C738AF2]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__use_y__2D67AF2B]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__reg_i__2E5BD364]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__reg_d__2F4FF79D]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__chg_i__30441BD6]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_dl_info] ADD  CONSTRAINT [DF__w_dl_info__chg_d__3138400F]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__ex_no__3414ACBA]  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__ex_nm__3508D0F3]  DEFAULT (NULL) FOR [ex_nm]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__ex_st__35FCF52C]  DEFAULT (NULL) FOR [ex_sts_cd]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__work___36F11965]  DEFAULT (NULL) FOR [work_dt]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__lct_c__37E53D9E]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__alert__38D961D7]  DEFAULT ('0') FOR [alert]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__remar__39CD8610]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__use_y__3AC1AA49]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__reg_i__3BB5CE82]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__reg_d__3CA9F2BB]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__chg_i__3D9E16F4]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_ex_info] ADD  CONSTRAINT [DF__w_ex_info__chg_d__3E923B2D]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__ext_n__520F23F5]  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__ext_n__5303482E]  DEFAULT (NULL) FOR [ext_nm]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__ext_s__53F76C67]  DEFAULT (NULL) FOR [ext_sts_cd]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__work___54EB90A0]  DEFAULT (NULL) FOR [work_dt]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__lct_c__55DFB4D9]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__alert__56D3D912]  DEFAULT ('0') FOR [alert]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__remar__57C7FD4B]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__use_y__58BC2184]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__reg_i__59B045BD]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__reg_d__5AA469F6]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__chg_i__5B988E2F]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_ext_info] ADD  CONSTRAINT [DF__w_ext_inf__chg_d__5C8CB268]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__mt_cd__4EC8A2F6]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__gr_qt__4FBCC72F]  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__gr_do__50B0EB68]  DEFAULT (NULL) FOR [gr_down]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__reaso__51A50FA1]  DEFAULT (NULL) FOR [reason]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__sts_n__529933DA]  DEFAULT (NULL) FOR [sts_now]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__bb_no__538D5813]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__use_y__54817C4C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__reg_i__5575A085]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__reg_d__5669C4BE]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__chg_i__575DE8F7]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_down] ADD  CONSTRAINT [DF__w_materia__chg_d__58520D30]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [LoctionMachine]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_info] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__statu__1A89E4E1]  DEFAULT (NULL) FOR [status]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__wmtid__1B7E091A]  DEFAULT (NULL) FOR [wmtid]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__id_ac__1C722D53]  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__at_no__1D66518C]  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__produ__1E5A75C5]  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__id_ac__1F4E99FE]  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__staff__2042BE37]  DEFAULT (NULL) FOR [staff_id]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__staff__2136E270]  DEFAULT (NULL) FOR [staff_id_oqc]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__machi__222B06A9]  DEFAULT (NULL) FOR [machine_id]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_ty__231F2AE2]  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_cd__24134F1B]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_no__25077354]  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__gr_qt__25FB978D]  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__real___26EFBBC6]  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__staff__27E3DFFF]  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__sp_cd__28D80438]  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__rd_no__29CC2871]  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__sd_no__2AC04CAA]  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__ext_n__2BB470E3]  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__ex_no__2CA8951C]  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__dl_no__2D9CB955]  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__recev__2E90DD8E]  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_material__date__2F8501C7]  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__retur__30792600]  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__alert__316D4A39]  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__expir__32616E72]  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__dt_of__335592AB]  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__expor__3449B6E4]  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__recev__353DDB1D]  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__end_p__3631FF56]  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__lot_n__3726238F]  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_ba__381A47C8]  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_qr__390E6C01]  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__mt_st__3A02903A]  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__bb_no__3AF6B473]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__bbmp___3BEAD8AC]  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__lct_c__3CDEFCE5]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__lct_s__3DD3211E]  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__from___3EC74557]  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__to_lc__3FBB6990]  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__outpu__40AF8DC9]  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__input__41A3B202]  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__buyer__4297D63B]  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__orgin__438BFA74]  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__remar__44801EAD]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__sts_u__457442E6]  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__use_y__4668671F]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__reg_i__475C8B58]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__reg_d__4850AF91]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__chg_i__4944D3CA]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__chg_d__4A38F803]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_history] ADD  CONSTRAINT [DF__w_materia__date___4B2D1C3C]  DEFAULT (NULL) FOR [date_insert]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__md_cd__4C214075]  DEFAULT (NULL) FOR [md_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__style__4D1564AE]  DEFAULT (NULL) FOR [style_no]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__style__4E0988E7]  DEFAULT (NULL) FOR [style_nm]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__mt_cd__4EFDAD20]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__width__4FF1D159]  DEFAULT (NULL) FOR [width]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__width__50E5F592]  DEFAULT (NULL) FOR [width_unit]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_material__spec__51DA19CB]  DEFAULT (NULL) FOR [spec]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__spec___52CE3E04]  DEFAULT (NULL) FOR [spec_unit]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__sd_no__53C2623D]  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__lot_n__54B68676]  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__sts_c__55AAAAAF]  DEFAULT (NULL) FOR [sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_material__memo__569ECEE8]  DEFAULT (NULL) FOR [memo]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__month__5792F321]  DEFAULT (NULL) FOR [month_excel]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__recei__5887175A]  DEFAULT (NULL) FOR [receiving_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_material_i__TX__597B3B93]  DEFAULT (NULL) FOR [TX]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__total__5A6F5FCC]  DEFAULT (NULL) FOR [total_m]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__total__5B638405]  DEFAULT (NULL) FOR [total_m2]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__total__5C57A83E]  DEFAULT (NULL) FOR [total_ea]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__use_y__5D4BCC77]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__reg_i__5E3FF0B0]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__reg_d__5F3414E9]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__chg_i__60283922]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_info_memo] ADD  CONSTRAINT [DF__w_materia__chg_d__611C5D5B]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__id_ac__62108194]  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__id_ac__6304A5CD]  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__staff__63F8CA06]  DEFAULT (NULL) FOR [staff_id]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__staff__64ECEE3F]  DEFAULT (NULL) FOR [staff_id_oqc]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_ty__65E11278]  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_cd__66D536B1]  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_no__67C95AEA]  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__gr_qt__68BD7F23]  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__real___69B1A35C]  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__sp_cd__6AA5C795]  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__rd_no__6B99EBCE]  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__sd_no__6C8E1007]  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__ext_n__6D823440]  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__ex_no__6E765879]  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__dl_no__6F6A7CB2]  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__picki__705EA0EB]  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__recev__7152C524]  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_material__date__7246E95D]  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__retur__733B0D96]  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__alert__742F31CF]  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__expir__75235608]  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__dt_of__76177A41]  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__expor__770B9E7A]  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__recev__77FFC2B3]  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__lot_n__78F3E6EC]  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_ba__79E80B25]  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_qr__7ADC2F5E]  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__mt_st__7BD05397]  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__bb_no__7CC477D0]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__bbmp___7DB89C09]  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__lct_c__7EACC042]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__lct_s__7FA0E47B]  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__from___009508B4]  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__to_lc__01892CED]  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__outpu__027D5126]  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__input__0371755F]  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__buyer__04659998]  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__orgin__0559BDD1]  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__remar__064DE20A]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__sts_u__07420643]  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__use_y__08362A7C]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__reg_i__092A4EB5]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__reg_d__0A1E72EE]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__chg_i__0B129727]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_info_tam] ADD  CONSTRAINT [DF__w_materia__chg_d__0C06BB60]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [LoctionMachine]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_info01] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  CONSTRAINT [DF__w_materia__bb_no__538M5813]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [sts_share]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_mapping] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  CONSTRAINT [DF__w_materia__bb_no__538M0113]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [sts_share]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_mapping01] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___polic__2374309D]  DEFAULT (NULL) FOR [policy_code]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___polic__246854D6]  DEFAULT (NULL) FOR [policy_name]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___work___255C790F]  DEFAULT (NULL) FOR [work_starttime]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___work___26509D48]  DEFAULT (NULL) FOR [work_endtime]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___lunch__2744C181]  DEFAULT (NULL) FOR [lunch_start_time]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___lunch__2838E5BA]  DEFAULT (NULL) FOR [lunch_end_time]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___dinne__292D09F3]  DEFAULT (NULL) FOR [dinner_start_time]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___dinne__2A212E2C]  DEFAULT (NULL) FOR [dinner_end_time]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___work___2B155265]  DEFAULT ('0.00') FOR [work_hour]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___use_y__2C09769E]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___last___2CFD9AD7]  DEFAULT ('Y') FOR [last_yn]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___re_ma__2DF1BF10]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___reg_i__2EE5E349]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___reg_d__2FDA0782]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___chg_i__30CE2BBB]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_policy_mt] ADD  CONSTRAINT [DF__w_policy___chg_d__31C24FF4]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (NULL) FOR [pq_no]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (NULL) FOR [ml_no]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (NULL) FOR [work_dt]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT ('1') FOR [check_qty]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT ('0') FOR [ok_qty]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_product_qc] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [pq_no]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [item_vcd]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [check_id]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [check_cd]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [check_value]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT ('1') FOR [check_qty]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_product_qc_value] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__rd_no__79B300FB]  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__rd_nm__7AA72534]  DEFAULT (NULL) FOR [rd_nm]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__rd_st__7B9B496D]  DEFAULT (NULL) FOR [rd_sts_cd]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__lct_c__7C8F6DA6]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__recei__7D8391DF]  DEFAULT (NULL) FOR [receiving_dt]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__remar__7E77B618]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__use_y__7F6BDA51]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__reg_i__005FFE8A]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__reg_d__015422C3]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__chg_i__024846FC]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_rd_info] ADD  CONSTRAINT [DF__w_rd_info__chg_d__033C6B35]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__sd_no__57E7F8DC]  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__sd_nm__58DC1D15]  DEFAULT (NULL) FOR [sd_nm]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__sd_st__59D0414E]  DEFAULT (NULL) FOR [sd_sts_cd]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__produ__5AC46587]  DEFAULT (NULL) FOR [product_cd]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__lct_c__5BB889C0]  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__alert__5CACADF9]  DEFAULT ('0') FOR [alert]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__remar__5DA0D232]  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__use_y__5E94F66B]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__del_y__5F891AA4]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__reg_i__607D3EDD]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__reg_d__61716316]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__chg_i__6265874F]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_sd_info] ADD  CONSTRAINT [DF__w_sd_info__chg_d__6359AB88]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [vn_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [wmtid]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [staff_id]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [staff_id_oqc]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [machine_id]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_vt_dt] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__vn_cd__3F51553C]  DEFAULT (NULL) FOR [vn_cd]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__vn_nm__40457975]  DEFAULT (NULL) FOR [vn_nm]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__start_d__41399DAE]  DEFAULT (NULL) FOR [start_dt]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__end_dt__422DC1E7]  DEFAULT (NULL) FOR [end_dt]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__re_mark__4321E620]  DEFAULT (NULL) FOR [re_mark]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__use_yn__44160A59]  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__del_yn__450A2E92]  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__reg_id__45FE52CB]  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__reg_dt__46F27704]  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__chg_id__47E69B3D]  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_vt_mt] ADD  CONSTRAINT [DF__w_vt_mt__chg_dt__48DABF76]  DEFAULT (getdate()) FOR [chg_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [LoctionMachine]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo04] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
---
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [LoctionMachine]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo05] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
--
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [LoctionMachine]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo06] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
--
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT ('0') FOR [id_actual]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT ('0') FOR [id_actual_oqc]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [at_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [product]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [mt_type]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [gr_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [real_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [staff_qty]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [sp_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [rd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [sd_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [ext_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [ex_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [dl_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [recevice_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [date]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [return_date]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT ('0') FOR [alert_NG]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [expiry_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [dt_of_receipt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [expore_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [recevice_dt_tims]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [rece_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [picking_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [shipping_wip_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [end_production_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [lot_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [mt_barcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [mt_qrcode]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT ('000') FOR [mt_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT ('000') FOR [bbmp_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [lct_sts_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [from_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [to_lct_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [output_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [input_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [buyer_qr]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [orgin_mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [sts_update]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [ExportCode]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [LoctionMachine]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [ShippingToMachineDatetime]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [Description]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_infopo07] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
----
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  CONSTRAINT [DF__w_materia__bb_no__538M0413]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [sts_share]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo04] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
----
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  CONSTRAINT [DF__w_materia__bb_no__538M0513]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [sts_share]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo05] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
----
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  CONSTRAINT [DF__w_materia__bb_no__538M0613]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [sts_share]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo06] ADD  DEFAULT (getdate()) FOR [chg_dt]
GO
----
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [mt_lot]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [mt_cd]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [mt_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [mapping_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  CONSTRAINT [DF__w_materia__bb_no__538M0713]  DEFAULT (NULL) FOR [bb_no]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [remark]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [sts_share]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT ('Y') FOR [use_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT ('N') FOR [del_yn]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [reg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (getdate()) FOR [reg_dt]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (NULL) FOR [chg_id]
GO
ALTER TABLE [real-autodb].[w_material_mappingpo07] ADD  DEFAULT (getdate()) FOR [chg_dt]

go


