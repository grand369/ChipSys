-- 创建触发器：自动更新询价的报价数量
-- 执行时间：2024-12-01 00:00:07

-- 1. 创建报价插入触发器
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_client_quote_Insert')
    DROP TRIGGER [dbo].[TR_client_quote_Insert];
GO

CREATE TRIGGER [dbo].[TR_client_quote_Insert]
ON [dbo].[client_quote]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- 更新询价的报价数量
    UPDATE i
    SET QuoteCount = (
        SELECT COUNT(*)
        FROM client_quote q
        WHERE q.InquiryId = i.Id
          AND q.IsDeleted = 0
    )
    FROM client_inquiry i
    INNER JOIN inserted ins ON i.Id = ins.InquiryId
    WHERE i.IsDeleted = 0;
END;
GO

-- 2. 创建报价删除触发器
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_client_quote_Delete')
    DROP TRIGGER [dbo].[TR_client_quote_Delete];
GO

CREATE TRIGGER [dbo].[TR_client_quote_Delete]
ON [dbo].[client_quote]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- 只处理IsDeleted字段的更新
    IF UPDATE(IsDeleted)
    BEGIN
        -- 更新询价的报价数量
        UPDATE i
        SET QuoteCount = (
            SELECT COUNT(*)
            FROM client_quote q
            WHERE q.InquiryId = i.Id
              AND q.IsDeleted = 0
        )
        FROM client_inquiry i
        INNER JOIN inserted ins ON i.Id = ins.InquiryId
        WHERE i.IsDeleted = 0;
    END;
END;
GO

-- 3. 创建询价删除触发器
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_client_inquiry_Delete')
    DROP TRIGGER [dbo].[TR_client_inquiry_Delete];
GO

CREATE TRIGGER [dbo].[TR_client_inquiry_Delete]
ON [dbo].[client_inquiry]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- 只处理IsDeleted字段的更新
    IF UPDATE(IsDeleted)
    BEGIN
        -- 软删除相关的报价记录
        UPDATE q
        SET IsDeleted = 1,
            ModifiedTime = GETDATE()
        FROM client_quote q
        INNER JOIN inserted ins ON q.InquiryId = ins.Id
        WHERE ins.IsDeleted = 1
          AND q.IsDeleted = 0;
    END;
END;
GO
