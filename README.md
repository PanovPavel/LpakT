


<details>
  <summary>скрипт создания бд</summary>


```sql
USE [master]
GO
/****** Object:  Database [LpakTesting]    Script Date: 20.03.2024 6:41:52 ******/
CREATE DATABASE [LpakTesting]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LpakTesting', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LpakTesting.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LpakTesting_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LpakTesting_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LpakTesting] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LpakTesting].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LpakTesting] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LpakTesting] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LpakTesting] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LpakTesting] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LpakTesting] SET ARITHABORT OFF 
GO
ALTER DATABASE [LpakTesting] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LpakTesting] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LpakTesting] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LpakTesting] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LpakTesting] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LpakTesting] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LpakTesting] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LpakTesting] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LpakTesting] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LpakTesting] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LpakTesting] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LpakTesting] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LpakTesting] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LpakTesting] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LpakTesting] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LpakTesting] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LpakTesting] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LpakTesting] SET RECOVERY FULL 
GO
ALTER DATABASE [LpakTesting] SET  MULTI_USER 
GO
ALTER DATABASE [LpakTesting] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LpakTesting] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LpakTesting] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LpakTesting] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LpakTesting] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LpakTesting] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LpakTesting', N'ON'
GO
ALTER DATABASE [LpakTesting] SET QUERY_STORE = ON
GO
ALTER DATABASE [LpakTesting] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LpakTesting]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 20.03.2024 6:41:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[TaxNumber] [char](15) NOT NULL,
	[Comment] [text] NULL,
	[FieldOfBusinessId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FieldOfBusiness]    Script Date: 20.03.2024 6:41:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldOfBusiness](
	[FieldOfBusinessId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FieldOfBusiness] PRIMARY KEY CLUSTERED 
(
	[FieldOfBusinessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 20.03.2024 6:41:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [uniqueidentifier] NOT NULL,
	[StatusId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[DataTime] [datetime] NOT NULL,
	[NameWork] [varchar](255) NOT NULL,
	[DescriptionWork] [varchar](1000) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusOrder]    Script Date: 20.03.2024 6:41:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusOrder](
	[StatusId] [uniqueidentifier] NOT NULL,
	[NameStatus] [varchar](50) NOT NULL,
 CONSTRAINT [StatusOrder_pk] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'f33e14cb-1adc-4715-88fa-058ab2f7f19f', N'ТехноЛидер', N'567738710225   ', N'', N'6a5bac6f-c266-4d5d-8a3b-f27022bb01f6')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'ef6ab083-1bf4-4406-a45c-13a91e74e450', N'ИнтерКомп', N'038663367688   ', N'', N'8f880ea2-e834-4a06-9ad6-12814cdf0933')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'848d4aba-0713-4709-8781-15d7d9152a24', N'ГлобалТрейд', N'2245881945     ', N'', N'16db98d2-54f8-4ed7-b25b-e18e755b2d31')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'431893a4-bda1-42f6-aacb-1aee3110a2e3', N'Авангард Индустриес', N'929933670984   ', N'Sit amet justo donec enim diam vulputate ut pharetra sit. Non arcu risus quis varius quam quisque id diam. Massa tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin. Commodo sed egestas egestas fringilla phasellus faucibus scelerisque eleifend donec. Tellus rutrum tellus pellentesque eu tincidunt.', N'85cbd726-bcfa-4885-bc20-f99939441179')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'932a6cfc-d3b6-46b3-8b3b-1d93a96db008', N'Глобал Лидерс', N'294868989407   ', N'', N'6a5bac6f-c266-4d5d-8a3b-f27022bb01f6')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'01da07e6-4006-41ac-9f12-265fbe0aa95d', N'ПрофиТех', N'102688362500   ', N'', N'42eea1e8-a712-4ac2-913c-b7700aa2bdf5')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'8661ec59-0a67-4313-b116-294e5dab7c12', N'Техно Инновации Груп', N'609153377455   ', N'', N'6a5bac6f-c266-4d5d-8a3b-f27022bb01f6')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'c7bd46fc-bcd1-407a-aa6f-37054fe34355', N'БизнесМастер', N'3820673898     ', N'Nen ielq', N'6a5bac6f-c266-4d5d-8a3b-f27022bb01f6')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'd0c0d7f5-8d9b-48b8-9189-51ce7a0d290f', N'Привет', N'347714716251   ', N'', N'6a5bac6f-c266-4d5d-8a3b-f27022bb01f6')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'97fa5375-b188-4d84-b3e0-8394c5eeff87', N'НовыйПрогресс', N'271333234790   ', N'', N'78dbc74e-ec72-4aed-966e-9a49dd3f2b04')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'52f16c0c-d839-4627-8800-8e64fffd42b5', N'АльфаСтройИнвест', N'6487712943     ', N'', N'9e4321ca-1604-43c3-9779-df74624063ef')
INSERT [dbo].[Customer] ([CustomerId], [Name], [TaxNumber], [Comment], [FieldOfBusinessId]) VALUES (N'638f8805-76e1-4fc8-a43b-e5554096889c', N'Иван', N'189299605828   ', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Laoreet suspendisse interdum consectetur libero id faucibus nisl tincidunt. Quis vel eros donec ac odio tempor. Elementum tempus egestas sed sed risus pretium. Sollicitudin tempor id eu nisl nunc mi. Integer malesuada nunc vel risus commodo viverra maecenas accumsan lacus.', N'd8033c35-8bc0-4aa7-89c8-f08fe680ff20')
GO
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'4592188b-222e-480e-85c3-d70637d9c8ba', N'T231e2st')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'b71dfb34-e7e2-4e6a-8804-cc5e001934d6', N'Test')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'85cbd726-bcfa-4885-bc20-f99939441179', N'Бумага')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'6a5bac6f-c266-4d5d-8a3b-f27022bb01f6', N'Грузоперевозки')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'9e4321ca-1604-43c3-9779-df74624063ef', N'Здравохранение')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'd8033c35-8bc0-4aa7-89c8-f08fe680ff20', N'Искусство')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'16db98d2-54f8-4ed7-b25b-e18e755b2d31', N'Производство')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'8f880ea2-e834-4a06-9ad6-12814cdf0933', N'Строительство дорог')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'42eea1e8-a712-4ac2-913c-b7700aa2bdf5', N'Упаковка')
INSERT [dbo].[FieldOfBusiness] ([FieldOfBusinessId], [Name]) VALUES (N'78dbc74e-ec72-4aed-966e-9a49dd3f2b04', N'Финансовое дело')
GO
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'2e6328bb-87fe-4ef0-bba4-073407e4fec4', N'8ab339e8-add2-40a8-89e0-a50c3ebc78da', N'01da07e6-4006-41ac-9f12-265fbe0aa95d', CAST(N'2024-03-19T00:00:00.000' AS DateTime), N'Проект доставки', N'')
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'9dae1d14-6702-4b54-8924-09cd7d2f447b', N'8ab339e8-add2-40a8-89e0-a50c3ebc78da', N'f33e14cb-1adc-4715-88fa-058ab2f7f19f', CAST(N'2024-03-19T00:00:00.000' AS DateTime), N'Маркетинговая реклама', N'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500')
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'017a5763-7c8e-4ab7-8699-0be3d84de8f8', N'7036185d-ef38-41e0-bbcb-3b53c6d0401c', N'f33e14cb-1adc-4715-88fa-058ab2f7f19f', CAST(N'2024-03-19T00:00:00.000' AS DateTime), N'Анализ бюджета', N'Проанализировать перераспределение бюджета на производственной линии')
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'4f0bd1ac-a645-41f9-89f4-54794b6f497e', N'7036185d-ef38-41e0-bbcb-3b53c6d0401c', N'932a6cfc-d3b6-46b3-8b3b-1d93a96db008', CAST(N'2023-03-12T20:41:40.000' AS DateTime), N'Обслуживание', N'Обслужить оборудование производственного цеха')
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'6ef1ee4a-3600-4e28-b706-6cc8e8cc62ff', N'c41dec67-1008-4e8f-b6e1-8d6dd7dce263', N'431893a4-bda1-42f6-aacb-1aee3110a2e3', CAST(N'2024-03-13T00:00:00.000' AS DateTime), N'Разработка по', N'Разработка программного обеспечения (англ. software development) — деятельность по созданию нового программного обеспечения[1]. ')
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'0c0c39ca-7d6e-49d1-ad48-717b66fe303b', N'c41dec67-1008-4e8f-b6e1-8d6dd7dce263', N'f33e14cb-1adc-4715-88fa-058ab2f7f19f', CAST(N'2024-03-19T00:00:00.000' AS DateTime), N'd', N'd')
INSERT [dbo].[Orders] ([OrderId], [StatusId], [CustomerId], [DataTime], [NameWork], [DescriptionWork]) VALUES (N'28fa7f85-9247-408a-b2c5-cccae7543033', N'7036185d-ef38-41e0-bbcb-3b53c6d0401c', N'52f16c0c-d839-4627-8800-8e64fffd42b5', CAST(N'2024-03-20T00:00:00.000' AS DateTime), N'Финансовый анализ', N' Porttitor lacus luctus accumsan tortor posuere. Phasellus egestas tellus rutrum tellus pellentesque eu tincidunt tortor aliquam. Consequat ac felis donec et odio pellentesque diam volutpat. Elit duis tristique sollicitudin nibh sit.')
GO
INSERT [dbo].[StatusOrder] ([StatusId], [NameStatus]) VALUES (N'7881fc4b-4445-4617-9c84-8b4ffa02892b', N'TestStatusWork')
INSERT [dbo].[StatusOrder] ([StatusId], [NameStatus]) VALUES (N'c41dec67-1008-4e8f-b6e1-8d6dd7dce263', N'В работе')
INSERT [dbo].[StatusOrder] ([StatusId], [NameStatus]) VALUES (N'8ab339e8-add2-40a8-89e0-a50c3ebc78da', N'Завершено')
INSERT [dbo].[StatusOrder] ([StatusId], [NameStatus]) VALUES (N'7036185d-ef38-41e0-bbcb-3b53c6d0401c', N'Создан')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [qk_Customer_Name]    Script Date: 20.03.2024 6:41:52 ******/
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [qk_Customer_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [qk_Customer_TaxNumber]    Script Date: 20.03.2024 6:41:52 ******/
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [qk_Customer_TaxNumber] UNIQUE NONCLUSTERED 
(
	[TaxNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [qk_FieldOfBusiness]    Script Date: 20.03.2024 6:41:52 ******/
ALTER TABLE [dbo].[FieldOfBusiness] ADD  CONSTRAINT [qk_FieldOfBusiness] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [qk_status_NameStatus]    Script Date: 20.03.2024 6:41:52 ******/
ALTER TABLE [dbo].[StatusOrder] ADD  CONSTRAINT [qk_status_NameStatus] UNIQUE NONCLUSTERED 
(
	[NameStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_FieldsOfBusiness_FieldOfBusinessId] FOREIGN KEY([FieldOfBusinessId])
REFERENCES [dbo].[FieldOfBusiness] ([FieldOfBusinessId])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_FieldsOfBusiness_FieldOfBusinessId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_Order_Status_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[StatusOrder] ([StatusId])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_Order_Status_StatusId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_Orders_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_Orders_Customer_CustomerId]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StatusId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'StatusId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DataTimeStart' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Orders', @level2type=N'COLUMN',@level2name=N'DataTime'
GO
USE [master]
GO
ALTER DATABASE [LpakTesting] SET  READ_WRITE 
GO
``` 

</details>

<details>
  <summary>Скриншоты</summary>
	
![image](https://github.com/PanovPavel/LpakT/assets/49455695/ab43ceb9-45f4-49c1-80a5-96cb317c53a1)
![image](https://github.com/PanovPavel/LpakT/assets/49455695/1b9d0313-7bc1-4856-b991-5276d3bdd05c)

</details>
