-- 创建报价表
-- 执行时间：2024-12-01 00:00:06

-- 1. 创建报价表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='client_quote' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[client_quote](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [InquiryId] [bigint] NOT NULL,
        [SupplierId] [bigint] NOT NULL,
        [SupplierName] [nvarchar](100) NOT NULL,
        [QuotePrice] [decimal](18,2) NOT NULL,
        [Currency] [nvarchar](10) NOT NULL DEFAULT('CNY'),
        [DeliveryDays] [int] NOT NULL,
        [MinOrderQuantity] [int] NOT NULL,
        [ValidDays] [int] NOT NULL,
        [ValidUntil] [datetime2](7) NULL,
        [ProductSpecification] [nvarchar](1000) NULL,
        [QualityAssurance] [nvarchar](500) NULL,
        [QuoteRemark] [nvarchar](1000) NULL,
        [ContactName] [nvarchar](50) NOT NULL,
        [ContactPhone] [nvarchar](20) NOT NULL,
        [ContactEmail] [nvarchar](100) NULL,
        [Status] [nvarchar](20) NOT NULL DEFAULT('Pending'),
        [AcceptedTime] [datetime2](7) NULL,
        [RejectedTime] [datetime2](7) NULL,
        [CreatedUserId] [bigint] NULL,
        [CreatedUserName] [nvarchar](50) NULL,
        [CreatedTime] [datetime2](7) NOT NULL DEFAULT(GETDATE()),
        [ModifiedUserId] [bigint] NULL,
        [ModifiedUserName] [nvarchar](50) NULL,
        [ModifiedTime] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT(0),
        CONSTRAINT [PK_client_quote] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- 2. 创建索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_quote_InquiryId' AND object_id = OBJECT_ID('client_quote'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_quote_InquiryId] ON [dbo].[client_quote]
    (
        [InquiryId] ASC
    );
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_quote_SupplierId' AND object_id = OBJECT_ID('client_quote'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_quote_SupplierId] ON [dbo].[client_quote]
    (
        [SupplierId] ASC
    );
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_quote_Status' AND object_id = OBJECT_ID('client_quote'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_quote_Status] ON [dbo].[client_quote]
    (
        [Status] ASC
    );
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_client_quote_CreatedTime' AND object_id = OBJECT_ID('client_quote'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_client_quote_CreatedTime] ON [dbo].[client_quote]
    (
        [CreatedTime] DESC
    );
END

-- 3. 创建外键约束
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_client_quote_client_inquiry')
BEGIN
    ALTER TABLE [dbo].[client_quote] 
    ADD CONSTRAINT [FK_client_quote_client_inquiry] 
    FOREIGN KEY([InquiryId]) REFERENCES [dbo].[client_inquiry] ([Id]);
END

-- 4. 添加表注释
EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'报价表', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'client_quote';

-- 5. 添加字段注释
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'询价ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'InquiryId';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'供应商ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'SupplierId';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'供应商名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'SupplierName';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'报价金额', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'QuotePrice';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'货币类型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'Currency';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'交期天数', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'DeliveryDays';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'最小起订量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'MinOrderQuantity';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'有效期天数', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'ValidDays';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'有效期至', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'ValidUntil';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'产品规格说明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'ProductSpecification';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'质量保证', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'QualityAssurance';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'报价备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'QuoteRemark';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'联系人姓名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'ContactName';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'联系电话', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'ContactPhone';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'联系邮箱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'ContactEmail';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'状态：Pending-待审核，Approved-已通过，Accepted-已接受，Rejected-已拒绝，Cancelled-已取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'Status';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'接受时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'AcceptedTime';
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'拒绝时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client_quote', @level2type = N'COLUMN', @level2name = N'RejectedTime';
