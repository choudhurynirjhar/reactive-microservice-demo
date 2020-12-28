USE [ecom]
GO

/****** Object:  Table [dbo].[Warehouse]    Script Date: 12/28/2020 10:45:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Warehouse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[InventoryId] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [ecom]
GO

/****** Object:  Table [dbo].[User]    Script Date: 12/28/2020 10:45:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Address] [varchar](100) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [ecom]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 12/28/2020 10:45:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[Type] [varchar](50) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [ecom]
GO

/****** Object:  Table [dbo].[OrderDetail]    Script Date: 12/28/2020 10:45:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[OrderDetail](
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductName] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [ecom]
GO

/****** Object:  Table [dbo].[Order]    Script Date: 12/28/2020 10:45:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UpdatedTime] [datetime] NOT NULL,
	[UserName] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [ecom]
GO

/****** Object:  Table [dbo].[Inventory]    Script Date: 12/28/2020 10:45:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Inventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductId] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



USE [ecom]
GO

/****** Object:  StoredProcedure [dbo].[CREATE_ORDER]    Script Date: 12/28/2020 10:48:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CREATE_ORDER]
	-- Add the parameters for the stored procedure here
	@userId int, 
	@userName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Order] VALUES (@userId, GETDATE(), @userName)

	SELECT @@IDENTITY
END

GO


USE [ecom]
GO

/****** Object:  StoredProcedure [dbo].[CREATE_ORDER_DETAILS]    Script Date: 12/28/2020 10:48:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CREATE_ORDER_DETAILS]
	-- Add the parameters for the stored procedure here
	@orderId int, 
	@productId int,
	@quantity int,
	@productName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[OrderDetail]
	([OrderId],[ProductId],[Quantity],[ProductName]) VALUES (@orderId, @productId, @quantity, @productName)

END

GO


USE [ecom]
GO

/****** Object:  StoredProcedure [dbo].[UPDATE_INVENTORY]    Script Date: 12/28/2020 10:48:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_INVENTORY]
	-- Add the parameters for the stored procedure here
	@productId int, 
	@quantity int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Inventory] SET [Quantity] = @quantity WHERE [ProductId] = @productId

END

GO


