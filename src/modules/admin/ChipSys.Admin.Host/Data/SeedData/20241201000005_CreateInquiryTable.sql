-- 创建询价表
-- 执行时间：2024-12-01 00:00:05

-- 1. 创建询价表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='client_inquiry' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[client_inquiry](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](100) NOT NULL,
        [ProductName] [nvarchar](100) NOT NULL,
        [ProductModel] [nvarchar](50) NULL,
        [ProductBrand] [nvarchar](50) NULL,
        [ProductManufacturer] [nvarchar](100) NULL,
        [Quantity] [int] NOT NULL,
        [BudgetRange] [nvarchar](100) NULL,
        [ExpectedDeliveryDays] [int] NOT NULL,
        [ValidDays] [int] NOT NULL,
        [TechnicalRequirements] [nvarchar](1000) NULL,
        [QualityRequirements] [nvarchar](500) NULL,
        [OtherRequirements] [nvarchar](500) NULL,
        [ContactInfo] [nvarchar](500) NOT NULL,
        [QuoteCount] [int] NOT NULL DEFAULT(0),
        [Status] [nvarchar](20) NOT NULL DEFAULT('Draft'),
        [MemberId] [bigint] NOT NULL,
        [CreatedUserId] [bigint] NULL,
        [CreatedUserName] [nvarchar](50) NULL,
        [CreatedTime] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [ModifiedUserId] [bigint] NULL,
        [ModifiedUserName] [nvarchar](50) NULL,
        [ModifiedTime] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT(0),
        CONSTRAINT [PK_client_inquiry] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- 2. 创建索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_inquiry_MemberId' AND object_id = OBJECT_ID('client_inquiry'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_inquiry_MemberId] ON [dbo].[client_inquiry]
    (
        [MemberId] ASC
    );
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_inquiry_Status' AND object_id = OBJECT_ID('client_inquiry'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_inquiry_Status] ON [dbo].[client_inquiry]
    (
        [Status] ASC
    );
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_inquiry_CreatedTime' AND object_id = OBJECT_ID('client_inquiry'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_inquiry_CreatedTime] ON [dbo].[client_inquiry]
    (
        [CreatedTime] DESC
    );
END

-- 3. 添加表注释
EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'询价表', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'client_inquiry';

-- 4. 添加字段注释
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'询价标题', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'Title';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'产品名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ProductName';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'产品型号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ProductModel';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'产品品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ProductBrand';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'产品制造商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ProductManufacturer';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'需求数量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'Quantity';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'预算范围', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'BudgetRange';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'期望交期天数', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ExpectedDeliveryDays';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'询价有效期天数', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ValidDays';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'技术参数要求', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'TechnicalRequirements';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'质量要求', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'QualityRequirements';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'其他要求', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'OtherRequirements';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'联系信息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'ContactInfo';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'报价数量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'QuoteCount';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'状态：Draft-草稿，Published-已发布，Closed-已关闭，Completed-已完成，Cancelled-已取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'Status';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'会员ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_inquiry', @level2type = N'COLUMN', @level2name = N'MemberId';
