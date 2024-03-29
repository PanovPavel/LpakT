USE [master]
GO
/****** Object:  Database [LpakTesting]    Script Date: 20.03.2024 7:19:12 ******/
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
/****** Object:  Table [dbo].[Customer]    Script Date: 20.03.2024 7:19:12 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [qk_Customer_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [qk_Customer_TaxNumber] UNIQUE NONCLUSTERED 
(
	[TaxNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FieldOfBusiness]    Script Date: 20.03.2024 7:19:12 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [qk_FieldOfBusiness] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 20.03.2024 7:19:12 ******/
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
/****** Object:  Table [dbo].[StatusOrder]    Script Date: 20.03.2024 7:19:12 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [qk_status_NameStatus] UNIQUE NONCLUSTERED 
(
	[NameStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
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
