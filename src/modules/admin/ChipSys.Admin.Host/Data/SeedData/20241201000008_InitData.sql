-- 初始化询价和报价数据
-- 执行时间：2024-12-01 00:00:08

-- 1. 插入示例询价数据
INSERT INTO [dbo].[client_inquiry] (
    [Title], [ProductName], [ProductModel], [ProductBrand], [ProductManufacturer],
    [Quantity], [BudgetRange], [ExpectedDeliveryDays], [ValidDays],
    [TechnicalRequirements], [QualityRequirements], [OtherRequirements],
    [ContactInfo], [Status], [MemberId],
    [CreatedUserId], [CreatedUserName], [CreatedTime], [IsDeleted]
) VALUES 
(
    '采购高性能CPU芯片',
    'Intel Core i7处理器',
    'i7-12700K',
    'Intel',
    'Intel Corporation',
    1000,
    '2000-2500元/片',
    30,
    60,
    '支持DDR4-3200，TDP 125W，基础频率3.6GHz，最大睿频5.0GHz',
    '通过Intel质量认证，提供3年质保',
    '需要提供原厂包装和质保卡',
    '联系人：张经理，电话：13800138000，邮箱：zhang@company.com',
    'Published',
    1,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
),
(
    '采购存储芯片',
    'NAND Flash存储芯片',
    'K9F1G08U0D',
    'Samsung',
    'Samsung Electronics',
    5000,
    '50-80元/片',
    45,
    90,
    '容量1GB，接口类型：NAND Flash，工作电压：3.3V',
    '符合JEDEC标准，提供1年质保',
    '需要提供测试报告和认证证书',
    '联系人：李工程师，电话：13900139000，邮箱：li@company.com',
    'Published',
    1,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
),
(
    '采购电源管理芯片',
    'DC-DC转换器',
    'LM2596',
    'TI',
    'Texas Instruments',
    2000,
    '10-15元/片',
    20,
    45,
    '输入电压范围：4.5V-40V，输出电压：1.2V-37V可调，最大输出电流：3A',
    '符合RoHS标准，工作温度范围：-40°C到+125°C',
    '需要提供完整的应用电路图和设计指南',
    '联系人：王主管，电话：13700137000，邮箱：wang@company.com',
    'Draft',
    1,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
);

-- 2. 插入示例报价数据
INSERT INTO [dbo].[client_quote] (
    [InquiryId], [SupplierId], [SupplierName], [QuotePrice], [Currency],
    [DeliveryDays], [MinOrderQuantity], [ValidDays], [ValidUntil],
    [ProductSpecification], [QualityAssurance], [QuoteRemark],
    [ContactName], [ContactPhone], [ContactEmail], [Status],
    [CreatedUserId], [CreatedUserName], [CreatedTime], [IsDeleted]
) VALUES 
(
    1, -- 对应第一个询价
    1001,
    '北京芯片科技有限公司',
    2200.00,
    'CNY',
    25,
    500,
    30,
    DATEADD(DAY, 30, GETDATE()),
    '全新原装Intel Core i7-12700K处理器，支持DDR4-3200，TDP 125W',
    '提供Intel官方质保，3年免费保修服务',
    '我们公司是Intel官方授权经销商，可以提供完整的质保服务和技术支持',
    '陈经理',
    '13800138001',
    'chen@chip-tech.com',
    'Pending',
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
),
(
    1, -- 对应第一个询价
    1002,
    '上海电子元器件有限公司',
    2300.00,
    'CNY',
    35,
    1000,
    45,
    DATEADD(DAY, 45, GETDATE()),
    'Intel Core i7-12700K处理器，原装正品，支持DDR4-3200',
    '提供2年质保，免费技术支持',
    '价格包含运费和保险，可提供批量采购优惠',
    '刘工程师',
    '13900139001',
    'liu@electronics.com',
    'Pending',
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
),
(
    2, -- 对应第二个询价
    1003,
    '深圳存储技术有限公司',
    65.00,
    'CNY',
    40,
    1000,
    60,
    DATEADD(DAY, 60, GETDATE()),
    'Samsung K9F1G08U0D NAND Flash存储芯片，1GB容量',
    '符合JEDEC标准，提供完整的测试报告',
    '可提供样品测试，支持定制化需求',
    '张主管',
    '13700137001',
    'zhang@storage-tech.com',
    'Pending',
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
);

-- 3. 更新询价的报价数量（触发器会自动处理，这里手动更新确保数据正确）
UPDATE i
SET QuoteCount = (
    SELECT COUNT(*)
    FROM client_quote q
    WHERE q.InquiryId = i.Id
      AND q.IsDeleted = 0
)
FROM client_inquiry i
WHERE i.IsDeleted = 0;
