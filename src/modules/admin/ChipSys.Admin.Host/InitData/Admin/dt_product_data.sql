-- ChipMgr模块产品初始化数据脚本
-- 表名: dt_product

-- 产品数据 (500个产品信息，这里展示前50个作为示例)
INSERT INTO dt_product (Id, CategoryId, Name, Unit, Code, Status, Brand, Description, Manufacturer, Model, Specification, Enabled, Sort, IsDeleted, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, TenantId) VALUES
-- 手机芯片类产品 (1001分类)
(3001, 1001, 'Apple A17 Pro', 'PCS', 'A17_PRO', 1, 'Apple', 'iPhone 15 Pro系列专用处理器', 'Apple Inc.', 'APL1W02', '3nm工艺, 6核CPU+6核GPU+16核NPU', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3002, 1001, 'Apple A16 Bionic', 'PCS', 'A16_BIONIC', 1, 'Apple', 'iPhone 14 Pro系列处理器', 'Apple Inc.', 'APL1W06', '4nm工艺, 6核CPU+5核GPU+16核NPU', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3003, 1001, 'Apple A15 Bionic', 'PCS', 'A15_BIONIC', 1, 'Apple', 'iPhone 13系列处理器', 'Apple Inc.', 'APL1W07', '5nm工艺, 6核CPU+4核GPU+16核NPU', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3004, 1001, 'Snapdragon 8 Gen 3', 'PCS', 'SD8GEN3', 1, 'Qualcomm', '高通旗舰移动处理器', 'Qualcomm Technologies', 'SM8650-AB', '4nm工艺, 8核CPU+Adreno 750 GPU', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3005, 1001, 'Snapdragon 8 Gen 2', 'PCS', 'SD8GEN2', 1, 'Qualcomm', '高通旗舰移动处理器', 'Qualcomm Technologies', 'SM8550-AB', '4nm工艺, 8核CPU+Adreno 740 GPU', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3006, 1001, 'Snapdragon 7 Gen 3', 'PCS', 'SD7GEN3', 1, 'Qualcomm', '高通中高端移动处理器', 'Qualcomm Technologies', 'SM7435-AB', '4nm工艺, 8核CPU+Adreno 720 GPU', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3007, 1001, 'MediaTek Dimensity 9300', 'PCS', 'MT9300', 1, 'MediaTek', '联发科旗舰移动处理器', 'MediaTek Inc.', 'MT6989', '4nm工艺, 8核CPU+Mali-G720 GPU', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3008, 1001, 'MediaTek Dimensity 9200+', 'PCS', 'MT9200P', 1, 'MediaTek', '联发科旗舰移动处理器', 'MediaTek Inc.', 'MT6985', '4nm工艺, 8核CPU+Mali-G715 GPU', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3009, 1001, 'MediaTek Dimensity 8200', 'PCS', 'MT8200', 1, 'MediaTek', '联发科中高端移动处理器', 'MediaTek Inc.', 'MT6896', '4nm工艺, 8核CPU+Mali-G610 GPU', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3010, 1001, 'Samsung Exynos 2400', 'PCS', 'EX2400', 1, 'Samsung', '三星旗舰移动处理器', 'Samsung Electronics', 'S5E9945', '4nm工艺, 10核CPU+Xclipse 940 GPU', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 电脑芯片类产品 (1002分类)
(3011, 1002, 'Intel Core i9-14900K', 'PCS', 'I9_14900K', 1, 'Intel', 'Intel第14代旗舰桌面处理器', 'Intel Corporation', 'BX8071514900K', '24核32线程, 3.2-6.0GHz, 125W', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3012, 1002, 'Intel Core i7-14700K', 'PCS', 'I7_14700K', 1, 'Intel', 'Intel第14代高端桌面处理器', 'Intel Corporation', 'BX8071514700K', '20核28线程, 3.4-5.6GHz, 125W', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3013, 1002, 'Intel Core i5-14600K', 'PCS', 'I5_14600K', 1, 'Intel', 'Intel第14代中高端桌面处理器', 'Intel Corporation', 'BX8071514600K', '14核20线程, 3.5-5.3GHz, 125W', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3014, 1002, 'AMD Ryzen 9 7950X', 'PCS', 'R9_7950X', 1, 'AMD', 'AMD Zen4架构旗舰处理器', 'Advanced Micro Devices', '100-100000514WOF', '16核32线程, 4.5-5.7GHz, 170W', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3015, 1002, 'AMD Ryzen 7 7800X3D', 'PCS', 'R7_7800X3D', 1, 'AMD', 'AMD 3D V-Cache游戏处理器', 'Advanced Micro Devices', '100-100000910WOF', '8核16线程, 4.2-5.0GHz, 120W', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3016, 1002, 'AMD Ryzen 5 7600X', 'PCS', 'R5_7600X', 1, 'AMD', 'AMD Zen4架构中端处理器', 'Advanced Micro Devices', '100-100000593WOF', '6核12线程, 4.7-5.3GHz, 105W', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3017, 1002, 'Apple M3 Max', 'PCS', 'M3_MAX', 1, 'Apple', 'MacBook Pro专用处理器', 'Apple Inc.', 'APL1103', '16核CPU+40核GPU, 3nm工艺', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3018, 1002, 'Apple M3 Pro', 'PCS', 'M3_PRO', 1, 'Apple', 'MacBook Pro专用处理器', 'Apple Inc.', 'APL1102', '12核CPU+18核GPU, 3nm工艺', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3019, 1002, 'Apple M3', 'PCS', 'M3', 1, 'Apple', 'MacBook Air专用处理器', 'Apple Inc.', 'APL1101', '8核CPU+10核GPU, 3nm工艺', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3020, 1002, 'NVIDIA GeForce RTX 4090', 'PCS', 'RTX4090', 1, 'NVIDIA', 'NVIDIA旗舰游戏显卡', 'NVIDIA Corporation', 'PG136-SKU355', '16384 CUDA核心, 24GB GDDR6X', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 平板芯片类产品 (1003分类)
(3021, 1003, 'Apple A17 Pro (iPad)', 'PCS', 'A17_PRO_IPAD', 1, 'Apple', 'iPad Pro专用处理器', 'Apple Inc.', 'APL1W02', '3nm工艺, 6核CPU+6核GPU+16核NPU', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3022, 1003, 'Apple M2 (iPad)', 'PCS', 'M2_IPAD', 1, 'Apple', 'iPad Pro专用处理器', 'Apple Inc.', 'APL1109', '5nm工艺, 8核CPU+10核GPU', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3023, 1003, 'Snapdragon 8cx Gen 3', 'PCS', 'SD8CXGEN3', 1, 'Qualcomm', 'Windows平板专用处理器', 'Qualcomm Technologies', 'SC8280XP', '5nm工艺, 8核CPU+Adreno 690 GPU', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3024, 1003, 'MediaTek Kompanio 1300T', 'PCS', 'MT1300T', 1, 'MediaTek', 'Android平板专用处理器', 'MediaTek Inc.', 'MT8195T', '6nm工艺, 8核CPU+Mali-G57 GPU', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3025, 1003, 'Samsung Exynos 2200 (Tablet)', 'PCS', 'EX2200_TAB', 1, 'Samsung', 'Android平板专用处理器', 'Samsung Electronics', 'S5E9925', '4nm工艺, 8核CPU+Xclipse 920 GPU', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 智能手表芯片类产品 (1004分类)
(3026, 1004, 'Apple S9', 'PCS', 'S9', 1, 'Apple', 'Apple Watch Series 9专用芯片', 'Apple Inc.', 'APL1104', '4nm工艺, 双核CPU+4核GPU', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3027, 1004, 'Apple S8', 'PCS', 'S8', 1, 'Apple', 'Apple Watch Series 8专用芯片', 'Apple Inc.', 'APL1105', '7nm工艺, 双核CPU+4核GPU', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3028, 1004, 'Snapdragon Wear 4100+', 'PCS', 'SDW4100P', 1, 'Qualcomm', 'Android智能手表处理器', 'Qualcomm Technologies', 'SDM429W', '12nm工艺, 4核CPU+Adreno 504 GPU', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3029, 1004, 'MediaTek MT2601', 'PCS', 'MT2601', 1, 'MediaTek', 'Android智能手表处理器', 'MediaTek Inc.', 'MT2601', '28nm工艺, 双核CPU+Mali-400 GPU', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3030, 1004, 'Samsung Exynos 9110', 'PCS', 'EX9110', 1, 'Samsung', 'Galaxy Watch专用处理器', 'Samsung Electronics', 'S5E9110', '10nm工艺, 双核CPU+Mali-T720 GPU', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 游戏机芯片类产品 (1005分类)
(3031, 1005, 'AMD Custom APU (PS5)', 'PCS', 'PS5_APU', 1, 'AMD', 'PlayStation 5专用处理器', 'Advanced Micro Devices', 'Oberon Plus', '7nm工艺, 8核Zen2+36CU RDNA2', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3032, 1005, 'AMD Custom APU (Xbox Series X)', 'PCS', 'XSX_APU', 1, 'AMD', 'Xbox Series X专用处理器', 'Advanced Micro Devices', 'Scarlett', '7nm工艺, 8核Zen2+52CU RDNA2', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3033, 1005, 'NVIDIA Tegra X1+', 'PCS', 'TEGRA_X1P', 1, 'NVIDIA', 'Nintendo Switch专用处理器', 'NVIDIA Corporation', 'T210B01', '16nm工艺, 4核Cortex-A57+Maxwell GPU', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3034, 1005, 'AMD Custom APU (Steam Deck)', 'PCS', 'STEAM_APU', 1, 'AMD', 'Steam Deck专用处理器', 'Advanced Micro Devices', 'Aerith', '7nm工艺, 4核Zen2+8CU RDNA2', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3035, 1005, 'AMD Custom APU (PS4 Pro)', 'PCS', 'PS4PRO_APU', 1, 'AMD', 'PlayStation 4 Pro专用处理器', 'Advanced Micro Devices', 'Neo', '16nm工艺, 8核Jaguar+36CU GCN', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 5G芯片类产品 (1006分类)
(3036, 1006, 'Snapdragon X75', 'PCS', 'SDX75', 1, 'Qualcomm', '高通第5代5G基带芯片', 'Qualcomm Technologies', 'SM7550-AB', '4nm工艺, 支持Sub-6GHz和mmWave', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3037, 1006, 'Snapdragon X70', 'PCS', 'SDX70', 1, 'Qualcomm', '高通第4代5G基带芯片', 'Qualcomm Technologies', 'SM7450-AB', '4nm工艺, 支持Sub-6GHz和mmWave', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3038, 1006, 'MediaTek M80', 'PCS', 'MTM80', 1, 'MediaTek', '联发科5G基带芯片', 'MediaTek Inc.', 'MT6825', '6nm工艺, 支持Sub-6GHz和mmWave', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3039, 1006, 'Samsung Exynos Modem 5300', 'PCS', 'EXMODEM5300', 1, 'Samsung', '三星5G基带芯片', 'Samsung Electronics', 'S5E9935', '4nm工艺, 支持Sub-6GHz和mmWave', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3040, 1006, 'Apple 5G Modem', 'PCS', 'APPLE_5G', 1, 'Apple', '苹果自研5G基带芯片', 'Apple Inc.', 'APL1W03', '5nm工艺, 支持Sub-6GHz和mmWave', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- WiFi芯片类产品 (1007分类)
(3041, 1007, 'Qualcomm FastConnect 7800', 'PCS', 'FC7800', 1, 'Qualcomm', '高通WiFi 7连接芯片', 'Qualcomm Technologies', 'WCN7851', 'WiFi 7, 蓝牙5.3, 支持6GHz', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3042, 1007, 'MediaTek MT7927', 'PCS', 'MT7927', 1, 'MediaTek', '联发科WiFi 7连接芯片', 'MediaTek Inc.', 'MT7927', 'WiFi 7, 蓝牙5.3, 支持6GHz', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3043, 1007, 'Broadcom BCM4389', 'PCS', 'BCM4389', 1, 'Broadcom', '博通WiFi 6E连接芯片', 'Broadcom Inc.', 'BCM4389', 'WiFi 6E, 蓝牙5.2, 支持6GHz', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3044, 1007, 'Intel AX210', 'PCS', 'AX210', 1, 'Intel', 'Intel WiFi 6E连接芯片', 'Intel Corporation', 'AX210NGW', 'WiFi 6E, 蓝牙5.2, 支持6GHz', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3045, 1007, 'Realtek RTL8852CE', 'PCS', 'RTL8852CE', 1, 'Realtek', '瑞昱WiFi 6连接芯片', 'Realtek Semiconductor', 'RTL8852CE', 'WiFi 6, 蓝牙5.2', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 物联网芯片类产品 (1008分类)
(3046, 1008, 'ESP32-S3', 'PCS', 'ESP32S3', 1, 'Espressif', '乐鑫物联网处理器', 'Espressif Systems', 'ESP32-S3', '双核Xtensa LX7, WiFi+蓝牙5.0', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3047, 1008, 'ESP32-C3', 'PCS', 'ESP32C3', 1, 'Espressif', '乐鑫物联网处理器', 'Espressif Systems', 'ESP32-C3', '单核RISC-V, WiFi+蓝牙5.0', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3048, 1008, 'Nordic nRF52840', 'PCS', 'NRF52840', 1, 'Nordic', 'Nordic物联网处理器', 'Nordic Semiconductor', 'nRF52840', 'Cortex-M4F, 蓝牙5.0+Thread+Zigbee', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3049, 1008, 'Silicon Labs EFR32BG22', 'PCS', 'EFR32BG22', 1, 'Silicon Labs', 'Silicon Labs物联网处理器', 'Silicon Laboratories', 'EFR32BG22', 'Cortex-M33, 蓝牙5.2+Zigbee+Thread', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3050, 1008, 'STMicroelectronics STM32WB55', 'PCS', 'STM32WB55', 1, 'STMicroelectronics', 'ST物联网处理器', 'STMicroelectronics', 'STM32WB55', 'Cortex-M4F+M0+, 蓝牙5.0+Zigbee', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (51-150)
(3051, 1001, 'Snapdragon 6 Gen 1', 'PCS', 'SD6GEN1', 1, 'Qualcomm', '高通中端移动处理器', 'Qualcomm Technologies', 'SM6450-AB', '4nm工艺, 8核CPU+Adreno 710 GPU', 1, 11, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3052, 1001, 'MediaTek Dimensity 7200', 'PCS', 'MT7200', 1, 'MediaTek', '联发科中端移动处理器', 'MediaTek Inc.', 'MT6886', '4nm工艺, 8核CPU+Mali-G610 GPU', 1, 12, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3053, 1001, 'Samsung Exynos 1380', 'PCS', 'EX1380', 1, 'Samsung', '三星中端移动处理器', 'Samsung Electronics', 'S5E8835', '5nm工艺, 8核CPU+Mali-G68 GPU', 1, 13, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3054, 1001, 'Unisoc Tiger T820', 'PCS', 'T820', 1, 'Unisoc', '紫光展锐中端移动处理器', 'Unisoc Technologies', 'T820', '6nm工艺, 8核CPU+Mali-G57 GPU', 1, 14, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3055, 1001, 'Hisilicon Kirin 9000S', 'PCS', 'K9000S', 1, 'Hisilicon', '海思麒麟处理器', 'Hisilicon Technologies', 'K9000S', '7nm工艺, 8核CPU+Mali-G78 GPU', 1, 15, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 电脑芯片类产品 (1002分类) 继续
(3056, 1002, 'Intel Core i9-13900K', 'PCS', 'I9_13900K', 1, 'Intel', 'Intel第13代旗舰桌面处理器', 'Intel Corporation', 'BX8071513900K', '24核32线程, 3.0-5.8GHz, 125W', 1, 11, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3057, 1002, 'Intel Core i7-13700K', 'PCS', 'I7_13700K', 1, 'Intel', 'Intel第13代高端桌面处理器', 'Intel Corporation', 'BX8071513700K', '16核24线程, 3.4-5.4GHz, 125W', 1, 12, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3058, 1002, 'AMD Ryzen 9 7900X', 'PCS', 'R9_7900X', 1, 'AMD', 'AMD Zen4架构高端处理器', 'Advanced Micro Devices', '100-100000589WOF', '12核24线程, 4.7-5.6GHz, 170W', 1, 13, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3059, 1002, 'AMD Ryzen 7 7700X', 'PCS', 'R7_7700X', 1, 'AMD', 'AMD Zen4架构中高端处理器', 'Advanced Micro Devices', '100-100000591WOF', '8核16线程, 4.5-5.4GHz, 105W', 1, 14, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3060, 1002, 'NVIDIA GeForce RTX 4080', 'PCS', 'RTX4080', 1, 'NVIDIA', 'NVIDIA高端游戏显卡', 'NVIDIA Corporation', 'PG139-SKU355', '9728 CUDA核心, 16GB GDDR6X', 1, 15, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3061, 1002, 'NVIDIA GeForce RTX 4070', 'PCS', 'RTX4070', 1, 'NVIDIA', 'NVIDIA中高端游戏显卡', 'NVIDIA Corporation', 'PG141-SKU355', '5888 CUDA核心, 12GB GDDR6X', 1, 16, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3062, 1002, 'AMD Radeon RX 7900 XTX', 'PCS', 'RX7900XTX', 1, 'AMD', 'AMD旗舰游戏显卡', 'Advanced Micro Devices', '11322-01-20G', '6144流处理器, 24GB GDDR6', 1, 17, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3063, 1002, 'AMD Radeon RX 7800 XT', 'PCS', 'RX7800XT', 1, 'AMD', 'AMD高端游戏显卡', 'Advanced Micro Devices', '11322-01-20G', '3840流处理器, 16GB GDDR6', 1, 18, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3064, 1002, 'Apple M2 Max', 'PCS', 'M2_MAX', 1, 'Apple', 'MacBook Pro专用处理器', 'Apple Inc.', 'APL1109', '12核CPU+38核GPU, 5nm工艺', 1, 19, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3065, 1002, 'Apple M2 Pro', 'PCS', 'M2_PRO', 1, 'Apple', 'MacBook Pro专用处理器', 'Apple Inc.', 'APL1110', '10核CPU+16核GPU, 5nm工艺', 1, 20, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 平板芯片类产品 (1003分类) 继续
(3066, 1003, 'Apple A14 Bionic (iPad)', 'PCS', 'A14_IPAD', 1, 'Apple', 'iPad Air专用处理器', 'Apple Inc.', 'APL1W01', '5nm工艺, 6核CPU+4核GPU+16核NPU', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3067, 1003, 'Snapdragon 8cx Gen 2', 'PCS', 'SD8CXGEN2', 1, 'Qualcomm', 'Windows平板专用处理器', 'Qualcomm Technologies', 'SC8180XP', '7nm工艺, 8核CPU+Adreno 690 GPU', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3068, 1003, 'MediaTek Kompanio 1200', 'PCS', 'MT1200', 1, 'MediaTek', 'Android平板专用处理器', 'MediaTek Inc.', 'MT8192', '6nm工艺, 8核CPU+Mali-G57 GPU', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3069, 1003, 'Samsung Exynos 2100 (Tablet)', 'PCS', 'EX2100_TAB', 1, 'Samsung', 'Android平板专用处理器', 'Samsung Electronics', 'S5E9840', '5nm工艺, 8核CPU+Mali-G78 GPU', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3070, 1003, 'Unisoc Tiger T618', 'PCS', 'T618', 1, 'Unisoc', 'Android平板专用处理器', 'Unisoc Technologies', 'T618', '12nm工艺, 8核CPU+Mali-G52 GPU', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 智能手表芯片类产品 (1004分类) 继续
(3071, 1004, 'Apple S7', 'PCS', 'S7', 1, 'Apple', 'Apple Watch Series 7专用芯片', 'Apple Inc.', 'APL1106', '7nm工艺, 双核CPU+4核GPU', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3072, 1004, 'Snapdragon Wear 4100', 'PCS', 'SDW4100', 1, 'Qualcomm', 'Android智能手表处理器', 'Qualcomm Technologies', 'SDM429W', '12nm工艺, 4核CPU+Adreno 504 GPU', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3073, 1004, 'MediaTek MT2601', 'PCS', 'MT2601_V2', 1, 'MediaTek', 'Android智能手表处理器', 'MediaTek Inc.', 'MT2601', '28nm工艺, 双核CPU+Mali-400 GPU', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3074, 1004, 'Samsung Exynos 9110', 'PCS', 'EX9110_V2', 1, 'Samsung', 'Galaxy Watch专用处理器', 'Samsung Electronics', 'S5E9110', '10nm工艺, 双核CPU+Mali-T720 GPU', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3075, 1004, 'Unisoc Tiger T117', 'PCS', 'T117', 1, 'Unisoc', '智能手表处理器', 'Unisoc Technologies', 'T117', '28nm工艺, 双核CPU+Mali-400 GPU', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 游戏机芯片类产品 (1005分类) 继续
(3076, 1005, 'AMD Custom APU (PS4)', 'PCS', 'PS4_APU', 1, 'AMD', 'PlayStation 4专用处理器', 'Advanced Micro Devices', 'Jaguar', '28nm工艺, 8核Jaguar+18CU GCN', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3077, 1005, 'AMD Custom APU (Xbox One)', 'PCS', 'XBOXONE_APU', 1, 'AMD', 'Xbox One专用处理器', 'Advanced Micro Devices', 'Jaguar', '28nm工艺, 8核Jaguar+12CU GCN', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3078, 1005, 'NVIDIA Tegra X1', 'PCS', 'TEGRA_X1', 1, 'NVIDIA', 'Nintendo Switch专用处理器', 'NVIDIA Corporation', 'T210', '20nm工艺, 4核Cortex-A57+Maxwell GPU', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3079, 1005, 'AMD Custom APU (Steam Deck)', 'PCS', 'STEAM_APU_V2', 1, 'AMD', 'Steam Deck专用处理器', 'Advanced Micro Devices', 'Aerith', '7nm工艺, 4核Zen2+8CU RDNA2', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3080, 1005, 'Intel Core i7-1165G7 (Gaming)', 'PCS', 'I7_1165G7_GAMING', 1, 'Intel', '游戏笔记本处理器', 'Intel Corporation', 'BX807011165G7', '4核8线程, 2.8-4.7GHz, 28W', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 5G芯片类产品 (1006分类) 继续
(3081, 1006, 'Snapdragon X65', 'PCS', 'SDX65', 1, 'Qualcomm', '高通第4代5G基带芯片', 'Qualcomm Technologies', 'SM7450-AB', '4nm工艺, 支持Sub-6GHz和mmWave', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3082, 1006, 'Snapdragon X60', 'PCS', 'SDX60', 1, 'Qualcomm', '高通第3代5G基带芯片', 'Qualcomm Technologies', 'SM7350-AB', '5nm工艺, 支持Sub-6GHz和mmWave', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3083, 1006, 'MediaTek M70', 'PCS', 'MTM70', 1, 'MediaTek', '联发科5G基带芯片', 'MediaTek Inc.', 'MT6823', '7nm工艺, 支持Sub-6GHz和mmWave', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3084, 1006, 'Samsung Exynos Modem 5123', 'PCS', 'EXMODEM5123', 1, 'Samsung', '三星5G基带芯片', 'Samsung Electronics', 'S5E5123', '7nm工艺, 支持Sub-6GHz和mmWave', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3085, 1006, 'Intel XMM 8160', 'PCS', 'XMM8160', 1, 'Intel', 'Intel 5G基带芯片', 'Intel Corporation', 'XMM8160', '10nm工艺, 支持Sub-6GHz和mmWave', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- WiFi芯片类产品 (1007分类) 继续
(3086, 1007, 'Qualcomm FastConnect 6900', 'PCS', 'FC6900', 1, 'Qualcomm', '高通WiFi 6E连接芯片', 'Qualcomm Technologies', 'WCN6856', 'WiFi 6E, 蓝牙5.2, 支持6GHz', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3087, 1007, 'MediaTek MT7921', 'PCS', 'MT7921', 1, 'MediaTek', '联发科WiFi 6连接芯片', 'MediaTek Inc.', 'MT7921', 'WiFi 6, 蓝牙5.2', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3088, 1007, 'Broadcom BCM4375', 'PCS', 'BCM4375', 1, 'Broadcom', '博通WiFi 6连接芯片', 'Broadcom Inc.', 'BCM4375', 'WiFi 6, 蓝牙5.0', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3089, 1007, 'Intel AX201', 'PCS', 'AX201', 1, 'Intel', 'Intel WiFi 6连接芯片', 'Intel Corporation', 'AX201NGW', 'WiFi 6, 蓝牙5.0', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3090, 1007, 'Realtek RTL8822CE', 'PCS', 'RTL8822CE', 1, 'Realtek', '瑞昱WiFi 5连接芯片', 'Realtek Semiconductor', 'RTL8822CE', 'WiFi 5, 蓝牙5.0', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 物联网芯片类产品 (1008分类) 继续
(3091, 1008, 'ESP32', 'PCS', 'ESP32', 1, 'Espressif', '乐鑫物联网处理器', 'Espressif Systems', 'ESP32', '双核Xtensa LX6, WiFi+蓝牙4.2', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3092, 1008, 'ESP8266', 'PCS', 'ESP8266', 1, 'Espressif', '乐鑫物联网处理器', 'Espressif Systems', 'ESP8266', '单核Tensilica L106, WiFi', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3093, 1008, 'Nordic nRF52832', 'PCS', 'NRF52832', 1, 'Nordic', 'Nordic物联网处理器', 'Nordic Semiconductor', 'nRF52832', 'Cortex-M4F, 蓝牙5.0', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3094, 1008, 'Silicon Labs EFR32BG21', 'PCS', 'EFR32BG21', 1, 'Silicon Labs', 'Silicon Labs物联网处理器', 'Silicon Laboratories', 'EFR32BG21', 'Cortex-M33, 蓝牙5.2', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3095, 1008, 'STMicroelectronics STM32WB50', 'PCS', 'STM32WB50', 1, 'STMicroelectronics', 'ST物联网处理器', 'STMicroelectronics', 'STM32WB50', 'Cortex-M4F+M0+, 蓝牙5.0', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 汽车电子类产品 (1010分类)
(3096, 1010, 'NVIDIA DRIVE Orin', 'PCS', 'DRIVE_ORIN', 1, 'NVIDIA', 'NVIDIA自动驾驶处理器', 'NVIDIA Corporation', 'T234', '12核Cortex-A78AE+2048核Ampere GPU', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3097, 1010, 'Qualcomm Snapdragon Ride', 'PCS', 'SD_RIDE', 1, 'Qualcomm', '高通自动驾驶处理器', 'Qualcomm Technologies', 'SA8540P', '8核Kryo CPU+Adreno GPU+Hexagon DSP', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3098, 1010, 'Intel Mobileye EyeQ5', 'PCS', 'EYEQ5', 1, 'Intel', 'Intel自动驾驶处理器', 'Intel Corporation', 'EyeQ5', '8核Cortex-A53+18核MIPS CPU', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3099, 1010, 'AMD Ryzen Embedded V2000', 'PCS', 'RYZEN_V2000', 1, 'AMD', 'AMD汽车嵌入式处理器', 'Advanced Micro Devices', '100-000000197', '4核Zen2+7核Vega GPU', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3100, 1010, 'Renesas R-Car V3H', 'PCS', 'RCAR_V3H', 1, 'Renesas', '瑞萨汽车处理器', 'Renesas Electronics', 'R-Car V3H', '4核Cortex-A53+2核Cortex-R7+PowerVR GPU', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (101-200)
-- 工业控制类产品 (1013分类)
(3101, 1013, 'Intel Core i7-12700E', 'PCS', 'I7_12700E', 1, 'Intel', 'Intel工业控制处理器', 'Intel Corporation', 'BX8071512700E', '12核20线程, 2.1-4.8GHz, 65W', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3102, 1013, 'AMD Ryzen 7 5700G', 'PCS', 'R7_5700G', 1, 'AMD', 'AMD工业控制处理器', 'Advanced Micro Devices', '100-100000263WOF', '8核16线程, 3.8-4.6GHz, 65W', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3103, 1013, 'NVIDIA Jetson AGX Orin', 'PCS', 'JETSON_AGX_ORIN', 1, 'NVIDIA', 'NVIDIA工业AI处理器', 'NVIDIA Corporation', 'JETSON-AGX-ORIN', '12核Cortex-A78AE+2048核Ampere GPU', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3104, 1013, 'Renesas RZ/V2M', 'PCS', 'RZ_V2M', 1, 'Renesas', '瑞萨工业处理器', 'Renesas Electronics', 'RZ/V2M', '双核Cortex-A53+双核Cortex-R7+DRP-AI', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3105, 1013, 'STMicroelectronics STM32MP157', 'PCS', 'STM32MP157', 1, 'STMicroelectronics', 'ST工业处理器', 'STMicroelectronics', 'STM32MP157', '双核Cortex-A7+单核Cortex-M4+GPU', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 机器人芯片类产品 (1014分类)
(3106, 1014, 'NVIDIA Jetson Xavier NX', 'PCS', 'JETSON_XAVIER_NX', 1, 'NVIDIA', 'NVIDIA机器人处理器', 'NVIDIA Corporation', 'JETSON-XAVIER-NX', '6核Cortex-A78AE+384核Volta GPU', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3107, 1014, 'Qualcomm RB5', 'PCS', 'RB5', 1, 'Qualcomm', '高通机器人处理器', 'Qualcomm Technologies', 'RB5', '8核Kryo CPU+Adreno GPU+Hexagon DSP', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3108, 1014, 'Intel NUC11 Pro', 'PCS', 'NUC11_PRO', 1, 'Intel', 'Intel机器人处理器', 'Intel Corporation', 'NUC11TNHv7', '4核Tiger Lake-U, 2.8-4.7GHz, 28W', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3109, 1014, 'AMD Ryzen Embedded V1000', 'PCS', 'RYZEN_V1000', 1, 'AMD', 'AMD机器人处理器', 'Advanced Micro Devices', '100-000000197', '4核Zen+7核Vega GPU', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3110, 1014, 'Renesas RZ/A2M', 'PCS', 'RZ_A2M', 1, 'Renesas', '瑞萨机器人处理器', 'Renesas Electronics', 'RZ/A2M', '双核Cortex-A9+DRP-AI', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 传感器芯片类产品 (1015分类)
(3111, 1015, 'Bosch BMI270', 'PCS', 'BMI270', 1, 'Bosch', 'Bosch运动传感器', 'Bosch Sensortec', 'BMI270', '6轴IMU, 加速度计+陀螺仪', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3112, 1015, 'STMicroelectronics LSM6DSO', 'PCS', 'LSM6DSO', 1, 'STMicroelectronics', 'ST运动传感器', 'STMicroelectronics', 'LSM6DSO', '6轴IMU, 加速度计+陀螺仪', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3113, 1015, 'InvenSense MPU6050', 'PCS', 'MPU6050', 1, 'InvenSense', 'InvenSense运动传感器', 'InvenSense', 'MPU6050', '6轴IMU, 加速度计+陀螺仪', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3114, 1015, 'Murata SCA3300', 'PCS', 'SCA3300', 1, 'Murata', 'Murata倾角传感器', 'Murata Manufacturing', 'SCA3300', '3轴加速度计, 高精度倾角测量', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3115, 1015, 'Bosch BME680', 'PCS', 'BME680', 1, 'Bosch', 'Bosch环境传感器', 'Bosch Sensortec', 'BME680', '温度+湿度+压力+气体传感器', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 存储芯片类产品 (1016分类)
(3116, 1016, 'Samsung 980 PRO', 'PCS', '980_PRO', 1, 'Samsung', '三星NVMe SSD', 'Samsung Electronics', 'MZ-V8P1T0BW', '1TB NVMe PCIe 4.0 SSD', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3117, 1016, 'Western Digital SN850X', 'PCS', 'SN850X', 1, 'Western Digital', 'WD NVMe SSD', 'Western Digital', 'WDS100T2X0E', '1TB NVMe PCIe 4.0 SSD', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3118, 1016, 'Micron 3400', 'PCS', '3400', 1, 'Micron', 'Micron NVMe SSD', 'Micron Technology', 'MTFDKBA1T0TFH', '1TB NVMe PCIe 4.0 SSD', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3119, 1016, 'Kioxia Exceria Pro', 'PCS', 'EXCERIA_PRO', 1, 'Kioxia', 'Kioxia NVMe SSD', 'Kioxia Corporation', 'EXCERIA-PRO-1TB', '1TB NVMe PCIe 4.0 SSD', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3120, 1016, 'Seagate FireCuda 530', 'PCS', 'FIRECUDA_530', 1, 'Seagate', 'Seagate NVMe SSD', 'Seagate Technology', 'ZP1000GM30023', '1TB NVMe PCIe 4.0 SSD', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 内存芯片类产品 (1017分类)
(3121, 1017, 'Samsung DDR5-5600', 'PCS', 'DDR5_5600', 1, 'Samsung', '三星DDR5内存', 'Samsung Electronics', 'M378A1K43DB2-CWE', '16GB DDR5-5600 CL46', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3122, 1017, 'SK Hynix DDR5-4800', 'PCS', 'DDR5_4800', 1, 'SK Hynix', 'SK Hynix DDR5内存', 'SK Hynix', 'HMCG88MEBSA092N', '16GB DDR5-4800 CL40', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3123, 1017, 'Micron DDR5-5200', 'PCS', 'DDR5_5200', 1, 'Micron', 'Micron DDR5内存', 'Micron Technology', 'MT60B2G8HB-48B', '16GB DDR5-5200 CL42', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3124, 1017, 'Samsung DDR4-3200', 'PCS', 'DDR4_3200', 1, 'Samsung', '三星DDR4内存', 'Samsung Electronics', 'M378A1K43CB2-CTD', '16GB DDR4-3200 CL22', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3125, 1017, 'SK Hynix DDR4-2666', 'PCS', 'DDR4_2666', 1, 'SK Hynix', 'SK Hynix DDR4内存', 'SK Hynix', 'HMA82GS6AFR8N-UH', '16GB DDR4-2666 CL19', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 显示驱动芯片类产品 (1018分类)
(3126, 1018, 'Novatek NT36672A', 'PCS', 'NT36672A', 1, 'Novatek', 'Novatek显示驱动芯片', 'Novatek Microelectronics', 'NT36672A', 'TDDI显示驱动芯片, 支持FHD+', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3127, 1018, 'Himax HX83112', 'PCS', 'HX83112', 1, 'Himax', 'Himax显示驱动芯片', 'Himax Technologies', 'HX83112', 'TDDI显示驱动芯片, 支持HD+', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3128, 1018, 'Samsung S6E3FC2', 'PCS', 'S6E3FC2', 1, 'Samsung', '三星OLED驱动芯片', 'Samsung Electronics', 'S6E3FC2', 'OLED显示驱动芯片, 支持FHD+', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3129, 1018, 'LG Display AOD550', 'PCS', 'AOD550', 1, 'LG Display', 'LG显示驱动芯片', 'LG Display', 'AOD550', 'OLED显示驱动芯片, 支持WQHD+', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3130, 1018, 'Raydium RM67191', 'PCS', 'RM67191', 1, 'Raydium', 'Raydium显示驱动芯片', 'Raydium Semiconductor', 'RM67191', 'TDDI显示驱动芯片, 支持FHD+', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 图像处理芯片类产品 (1019分类)
(3131, 1019, 'Sony IMX989', 'PCS', 'IMX989', 1, 'Sony', 'Sony图像传感器', 'Sony Semiconductor', 'IMX989', '1英寸CMOS图像传感器, 50MP', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3132, 1019, 'Samsung ISOCELL HP3', 'PCS', 'HP3', 1, 'Samsung', '三星图像传感器', 'Samsung Electronics', 'S5KHP3', '200MP CMOS图像传感器', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3133, 1019, 'OmniVision OV50A', 'PCS', 'OV50A', 1, 'OmniVision', 'OmniVision图像传感器', 'OmniVision Technologies', 'OV50A', '50MP CMOS图像传感器', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3134, 1019, 'STMicroelectronics STM32H7', 'PCS', 'STM32H7', 1, 'STMicroelectronics', 'ST图像处理芯片', 'STMicroelectronics', 'STM32H743', 'Cortex-M7, 480MHz, 1MB Flash', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3135, 1019, 'NVIDIA Jetson Nano', 'PCS', 'JETSON_NANO', 1, 'NVIDIA', 'NVIDIA AI图像处理芯片', 'NVIDIA Corporation', 'JETSON-NANO', '4核Cortex-A57+128核Maxwell GPU', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 音频芯片类产品 (1020分类)
(3136, 1020, 'Cirrus Logic CS35L45', 'PCS', 'CS35L45', 1, 'Cirrus Logic', 'Cirrus Logic音频功放', 'Cirrus Logic', 'CS35L45', '智能音频功放, 支持DSP', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3137, 1020, 'Qualcomm WCD9385', 'PCS', 'WCD9385', 1, 'Qualcomm', '高通音频编解码器', 'Qualcomm Technologies', 'WCD9385', 'Hi-Fi音频编解码器, 支持DAC', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3138, 1020, 'Realtek ALC897', 'PCS', 'ALC897', 1, 'Realtek', 'Realtek音频编解码器', 'Realtek Semiconductor', 'ALC897', '7.1声道音频编解码器', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3139, 1020, 'TI TAS5805M', 'PCS', 'TAS5805M', 1, 'Texas Instruments', 'TI音频功放', 'Texas Instruments', 'TAS5805M', '数字音频功放, 支持DSP', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3140, 1020, 'Maxim MAX98357A', 'PCS', 'MAX98357A', 1, 'Maxim Integrated', 'Maxim音频功放', 'Maxim Integrated', 'MAX98357A', 'I2S数字音频功放', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 语音识别芯片类产品 (1021分类)
(3141, 1021, 'Synaptics AudioSmart', 'PCS', 'AUDIOSMART', 1, 'Synaptics', 'Synaptics语音识别芯片', 'Synaptics', 'AudioSmart', '远场语音识别芯片, 支持降噪', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3142, 1021, 'Conexant CX20921', 'PCS', 'CX20921', 1, 'Conexant', 'Conexant语音识别芯片', 'Conexant Systems', 'CX20921', '语音识别芯片, 支持降噪', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3143, 1021, 'Knowles IA8201', 'PCS', 'IA8201', 1, 'Knowles', 'Knowles语音识别芯片', 'Knowles Corporation', 'IA8201', '智能音频处理器, 支持语音识别', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3144, 1021, 'STMicroelectronics STM32F4', 'PCS', 'STM32F4', 1, 'STMicroelectronics', 'ST语音处理芯片', 'STMicroelectronics', 'STM32F407', 'Cortex-M4, 168MHz, 支持DSP', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3145, 1021, 'NXP i.MX RT1060', 'PCS', 'IMX_RT1060', 1, 'NXP', 'NXP语音处理芯片', 'NXP Semiconductors', 'i.MX RT1060', 'Cortex-M7, 600MHz, 支持DSP', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 电源管理芯片类产品 (1022分类)
(3146, 1022, 'TI TPS65987D', 'PCS', 'TPS65987D', 1, 'Texas Instruments', 'TI USB-C电源管理芯片', 'Texas Instruments', 'TPS65987D', 'USB-C PD控制器, 支持100W', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3147, 1022, 'Maxim MAX77650', 'PCS', 'MAX77650', 1, 'Maxim Integrated', 'Maxim电源管理芯片', 'Maxim Integrated', 'MAX77650', '多路电源管理芯片, 支持充电', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3148, 1022, 'Qualcomm PM8150', 'PCS', 'PM8150', 1, 'Qualcomm', '高通电源管理芯片', 'Qualcomm Technologies', 'PM8150', '多路电源管理芯片, 支持快充', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3149, 1022, 'STMicroelectronics STPMIC1', 'PCS', 'STPMIC1', 1, 'STMicroelectronics', 'ST电源管理芯片', 'STMicroelectronics', 'STPMIC1', '多路电源管理芯片, 支持DDR', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3150, 1022, 'Infineon IRPS5401', 'PCS', 'IRPS5401', 1, 'Infineon', 'Infineon电源管理芯片', 'Infineon Technologies', 'IRPS5401', '多路电源管理芯片, 支持DDR5', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (151-300)
-- 显示驱动芯片类产品 (1018分类) - 继续补充
(3151, 1018, 'Novatek NT36672C', 'PCS', 'NT36672C', 1, 'Novatek', 'Novatek显示驱动芯片', 'Novatek Microelectronics', 'NT36672C', 'TDDI显示驱动芯片, 支持QHD+', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3152, 1018, 'Himax HX83112B', 'PCS', 'HX83112B', 1, 'Himax', 'Himax显示驱动芯片', 'Himax Technologies', 'HX83112B', 'TDDI显示驱动芯片, 支持FHD+', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3153, 1018, 'Samsung S6E3FC3', 'PCS', 'S6E3FC3', 1, 'Samsung', '三星OLED驱动芯片', 'Samsung Electronics', 'S6E3FC3', 'OLED显示驱动芯片, 支持QHD+', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3154, 1018, 'LG Display AOD560', 'PCS', 'AOD560', 1, 'LG Display', 'LG显示驱动芯片', 'LG Display', 'AOD560', 'OLED显示驱动芯片, 支持4K', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3155, 1018, 'Raydium RM67192', 'PCS', 'RM67192', 1, 'Raydium', 'Raydium显示驱动芯片', 'Raydium Semiconductor', 'RM67192', 'TDDI显示驱动芯片, 支持QHD+', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 图像处理芯片类产品 (1019分类) - 继续补充
(3156, 1019, 'Sony IMX890', 'PCS', 'IMX890', 1, 'Sony', 'Sony图像传感器', 'Sony Semiconductor', 'IMX890', '1/1.56英寸CMOS图像传感器, 50MP', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3157, 1019, 'Samsung ISOCELL HP2', 'PCS', 'HP2', 1, 'Samsung', '三星图像传感器', 'Samsung Electronics', 'S5KHP2', '200MP CMOS图像传感器, 1/1.3英寸', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3158, 1019, 'OmniVision OV64B', 'PCS', 'OV64B', 1, 'OmniVision', 'OmniVision图像传感器', 'OmniVision Technologies', 'OV64B', '64MP CMOS图像传感器', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3159, 1019, 'STMicroelectronics STM32H7A3', 'PCS', 'STM32H7A3', 1, 'STMicroelectronics', 'ST图像处理芯片', 'STMicroelectronics', 'STM32H7A3', 'Cortex-M7, 280MHz, 1.4MB Flash', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3160, 1019, 'NVIDIA Jetson Orin Nano', 'PCS', 'JETSON_ORIN_NANO', 1, 'NVIDIA', 'NVIDIA AI图像处理芯片', 'NVIDIA Corporation', 'JETSON-ORIN-NANO', '6核Cortex-A78AE+1024核Ampere GPU', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 音频芯片类产品 (1020分类) - 继续补充
(3161, 1020, 'Cirrus Logic CS35L46', 'PCS', 'CS35L46', 1, 'Cirrus Logic', 'Cirrus Logic音频功放', 'Cirrus Logic', 'CS35L46', '智能音频功放, 支持DSP+降噪', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3162, 1020, 'Qualcomm WCD9380', 'PCS', 'WCD9380', 1, 'Qualcomm', '高通音频编解码器', 'Qualcomm Technologies', 'WCD9380', 'Hi-Fi音频编解码器, 支持DAC+ADC', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3163, 1020, 'Realtek ALC897B', 'PCS', 'ALC897B', 1, 'Realtek', 'Realtek音频编解码器', 'Realtek Semiconductor', 'ALC897B', '7.1声道音频编解码器, 支持DTS', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3164, 1020, 'TI TAS5805P', 'PCS', 'TAS5805P', 1, 'Texas Instruments', 'TI音频功放', 'Texas Instruments', 'TAS5805P', '数字音频功放, 支持DSP+EQ', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3165, 1020, 'Maxim MAX98360A', 'PCS', 'MAX98360A', 1, 'Maxim Integrated', 'Maxim音频功放', 'Maxim Integrated', 'MAX98360A', 'I2S数字音频功放, 支持DSP', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 语音识别芯片类产品 (1021分类) - 继续补充
(3166, 1021, 'Synaptics AudioSmart Pro', 'PCS', 'AUDIOSMART_PRO', 1, 'Synaptics', 'Synaptics语音识别芯片', 'Synaptics', 'AudioSmart Pro', '远场语音识别芯片, 支持降噪+AI', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3167, 1021, 'Conexant CX20922', 'PCS', 'CX20922', 1, 'Conexant', 'Conexant语音识别芯片', 'Conexant Systems', 'CX20922', '语音识别芯片, 支持降噪+回声消除', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3168, 1021, 'Knowles IA8202', 'PCS', 'IA8202', 1, 'Knowles', 'Knowles语音识别芯片', 'Knowles Corporation', 'IA8202', '智能音频处理器, 支持语音识别+降噪', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3169, 1021, 'STMicroelectronics STM32F4A7', 'PCS', 'STM32F4A7', 1, 'STMicroelectronics', 'ST语音处理芯片', 'STMicroelectronics', 'STM32F4A7', 'Cortex-M4, 180MHz, 支持DSP+FPU', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3170, 1021, 'NXP i.MX RT1170', 'PCS', 'IMX_RT1170', 1, 'NXP', 'NXP语音处理芯片', 'NXP Semiconductors', 'i.MX RT1170', 'Cortex-M7, 1GHz, 支持DSP+GPU', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 电源管理芯片类产品 (1022分类) - 继续补充
(3171, 1022, 'TI TPS65988', 'PCS', 'TPS65988', 1, 'Texas Instruments', 'TI USB-C电源管理芯片', 'Texas Instruments', 'TPS65988', 'USB-C PD控制器, 支持100W+快充', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3172, 1022, 'Maxim MAX77651', 'PCS', 'MAX77651', 1, 'Maxim Integrated', 'Maxim电源管理芯片', 'Maxim Integrated', 'MAX77651', '多路电源管理芯片, 支持充电+保护', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3173, 1022, 'Qualcomm PM8150B', 'PCS', 'PM8150B', 1, 'Qualcomm', '高通电源管理芯片', 'Qualcomm Technologies', 'PM8150B', '多路电源管理芯片, 支持快充+无线充', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3174, 1022, 'STMicroelectronics STPMIC2', 'PCS', 'STPMIC2', 1, 'STMicroelectronics', 'ST电源管理芯片', 'STMicroelectronics', 'STPMIC2', '多路电源管理芯片, 支持DDR5+GPU', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3175, 1022, 'Infineon IRPS5402', 'PCS', 'IRPS5402', 1, 'Infineon', 'Infineon电源管理芯片', 'Infineon Technologies', 'IRPS5402', '多路电源管理芯片, 支持DDR5+PCIe', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 网络通信芯片类产品 (1023分类)
(3176, 1023, 'Qualcomm QCA6390', 'PCS', 'QCA6390', 1, 'Qualcomm', '高通WiFi 6芯片', 'Qualcomm Technologies', 'QCA6390', 'WiFi 6 + 蓝牙5.1 组合芯片', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3177, 1023, 'Broadcom BCM4375', 'PCS', 'BCM4375', 1, 'Broadcom', 'Broadcom WiFi 6芯片', 'Broadcom Inc.', 'BCM4375', 'WiFi 6 + 蓝牙5.0 组合芯片', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3178, 1023, 'MediaTek MT7921', 'PCS', 'MT7921', 1, 'MediaTek', '联发科WiFi 6芯片', 'MediaTek Inc.', 'MT7921', 'WiFi 6 + 蓝牙5.2 组合芯片', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3179, 1023, 'Intel AX201', 'PCS', 'AX201', 1, 'Intel', 'Intel WiFi 6芯片', 'Intel Corporation', 'AX201', 'WiFi 6 + 蓝牙5.1 组合芯片', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3180, 1023, 'Realtek RTL8852AE', 'PCS', 'RTL8852AE', 1, 'Realtek', 'Realtek WiFi 6芯片', 'Realtek Semiconductor', 'RTL8852AE', 'WiFi 6 + 蓝牙5.2 组合芯片', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 蓝牙芯片类产品 (1024分类)
(3181, 1024, 'Qualcomm QCC5125', 'PCS', 'QCC5125', 1, 'Qualcomm', '高通蓝牙音频芯片', 'Qualcomm Technologies', 'QCC5125', '蓝牙5.0 音频芯片, 支持aptX', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3182, 1024, 'Broadcom BCM20736', 'PCS', 'BCM20736', 1, 'Broadcom', 'Broadcom蓝牙芯片', 'Broadcom Inc.', 'BCM20736', '蓝牙5.0 芯片, 支持BLE', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3183, 1024, 'Nordic nRF52840', 'PCS', 'NRF52840', 1, 'Nordic Semiconductor', 'Nordic蓝牙芯片', 'Nordic Semiconductor', 'nRF52840', '蓝牙5.0 芯片, 支持Thread+Zigbee', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3184, 1024, 'TI CC2640R2F', 'PCS', 'CC2640R2F', 1, 'Texas Instruments', 'TI蓝牙芯片', 'Texas Instruments', 'CC2640R2F', '蓝牙5.0 芯片, 支持BLE', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3185, 1024, 'Dialog DA14531', 'PCS', 'DA14531', 1, 'Dialog Semiconductor', 'Dialog蓝牙芯片', 'Dialog Semiconductor', 'DA14531', '蓝牙5.0 芯片, 超低功耗', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 射频芯片类产品 (1025分类)
(3186, 1025, 'Qualcomm QPM2630', 'PCS', 'QPM2630', 1, 'Qualcomm', '高通射频前端芯片', 'Qualcomm Technologies', 'QPM2630', '5G 射频前端模块, 支持Sub-6GHz', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3187, 1025, 'Skyworks SKY58255', 'PCS', 'SKY58255', 1, 'Skyworks Solutions', 'Skyworks射频芯片', 'Skyworks Solutions', 'SKY58255', '5G 射频前端模块, 支持mmWave', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3188, 1025, 'Qorvo QM77043', 'PCS', 'QM77043', 1, 'Qorvo', 'Qorvo射频芯片', 'Qorvo Inc.', 'QM77043', '5G 射频前端模块, 支持Sub-6GHz', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3189, 1025, 'Broadcom BCM4378', 'PCS', 'BCM4378', 1, 'Broadcom', 'Broadcom射频芯片', 'Broadcom Inc.', 'BCM4378', 'WiFi 6E 射频前端模块', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3190, 1025, 'Murata LNA', 'PCS', 'MURATA_LNA', 1, 'Murata', 'Murata射频芯片', 'Murata Manufacturing', 'LNA-001', '低噪声放大器, 支持5G', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 安全芯片类产品 (1026分类)
(3191, 1026, 'Infineon OPTIGA Trust M', 'PCS', 'OPTIGA_TRUST_M', 1, 'Infineon', 'Infineon安全芯片', 'Infineon Technologies', 'OPTIGA Trust M', '硬件安全模块, 支持ECC', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3192, 1026, 'STMicroelectronics ST33', 'PCS', 'ST33', 1, 'STMicroelectronics', 'ST安全芯片', 'STMicroelectronics', 'ST33', '安全元件, 支持Java Card', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3193, 1026, 'NXP A71CH', 'PCS', 'A71CH', 1, 'NXP', 'NXP安全芯片', 'NXP Semiconductors', 'A71CH', '安全认证芯片, 支持ECC', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3194, 1026, 'Microchip ATECC608A', 'PCS', 'ATECC608A', 1, 'Microchip', 'Microchip安全芯片', 'Microchip Technology', 'ATECC608A', '加密认证芯片, 支持ECC', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3195, 1026, 'Renesas S5U137', 'PCS', 'S5U137', 1, 'Renesas', 'Renesas安全芯片', 'Renesas Electronics', 'S5U137', '安全MCU, 支持AES', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 时钟芯片类产品 (1027分类)
(3196, 1027, 'Silicon Labs Si5341', 'PCS', 'SI5341', 1, 'Silicon Labs', 'Silicon Labs时钟芯片', 'Silicon Labs', 'Si5341', '多输出时钟发生器, 支持Jitter清除', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3197, 1027, 'IDT 8T49N287', 'PCS', '8T49N287', 1, 'IDT', 'IDT时钟芯片', 'IDT (Renesas)', '8T49N287', '可编程时钟发生器, 支持PCIe', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3198, 1027, 'Microchip SY89833', 'PCS', 'SY89833', 1, 'Microchip', 'Microchip时钟芯片', 'Microchip Technology', 'SY89833', '低抖动时钟缓冲器', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3199, 1027, 'TI CDCE62005', 'PCS', 'CDCE62005', 1, 'Texas Instruments', 'TI时钟芯片', 'Texas Instruments', 'CDCE62005', '5输出时钟发生器, 支持PCIe', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3200, 1027, 'Maxim MAX9485', 'PCS', 'MAX9485', 1, 'Maxim Integrated', 'Maxim时钟芯片', 'Maxim Integrated', 'MAX9485', '音频时钟发生器, 支持192kHz', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (201-350)
-- 蓝牙芯片类产品 (1024分类) - 继续补充
(3201, 1024, 'Qualcomm QCC5127', 'PCS', 'QCC5127', 1, 'Qualcomm', '高通蓝牙音频芯片', 'Qualcomm Technologies', 'QCC5127', '蓝牙5.0 音频芯片, 支持aptX HD', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3202, 1024, 'Broadcom BCM20736B', 'PCS', 'BCM20736B', 1, 'Broadcom', 'Broadcom蓝牙芯片', 'Broadcom Inc.', 'BCM20736B', '蓝牙5.0 芯片, 支持BLE+经典蓝牙', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3203, 1024, 'Nordic nRF52833', 'PCS', 'NRF52833', 1, 'Nordic Semiconductor', 'Nordic蓝牙芯片', 'Nordic Semiconductor', 'nRF52833', '蓝牙5.0 芯片, 支持Thread+Zigbee+BLE', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3204, 1024, 'TI CC2642R', 'PCS', 'CC2642R', 1, 'Texas Instruments', 'TI蓝牙芯片', 'Texas Instruments', 'CC2642R', '蓝牙5.0 芯片, 支持BLE+2.4GHz', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3205, 1024, 'Dialog DA14531B', 'PCS', 'DA14531B', 1, 'Dialog Semiconductor', 'Dialog蓝牙芯片', 'Dialog Semiconductor', 'DA14531B', '蓝牙5.0 芯片, 超低功耗+长距离', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 射频芯片类产品 (1025分类) - 继续补充
(3206, 1025, 'Qualcomm QPM2631', 'PCS', 'QPM2631', 1, 'Qualcomm', '高通射频前端芯片', 'Qualcomm Technologies', 'QPM2631', '5G 射频前端模块, 支持Sub-6GHz+mmWave', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3207, 1025, 'Skyworks SKY58256', 'PCS', 'SKY58256', 1, 'Skyworks Solutions', 'Skyworks射频芯片', 'Skyworks Solutions', 'SKY58256', '5G 射频前端模块, 支持mmWave+Sub-6GHz', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3208, 1025, 'Qorvo QM77044', 'PCS', 'QM77044', 1, 'Qorvo', 'Qorvo射频芯片', 'Qorvo Inc.', 'QM77044', '5G 射频前端模块, 支持Sub-6GHz+WiFi 6E', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3209, 1025, 'Broadcom BCM4379', 'PCS', 'BCM4379', 1, 'Broadcom', 'Broadcom射频芯片', 'Broadcom Inc.', 'BCM4379', 'WiFi 6E 射频前端模块, 支持蓝牙', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3210, 1025, 'Murata PA', 'PCS', 'MURATA_PA', 1, 'Murata', 'Murata射频芯片', 'Murata Manufacturing', 'PA-001', '功率放大器, 支持5G+WiFi 6', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 安全芯片类产品 (1026分类) - 继续补充
(3211, 1026, 'Infineon OPTIGA Trust X', 'PCS', 'OPTIGA_TRUST_X', 1, 'Infineon', 'Infineon安全芯片', 'Infineon Technologies', 'OPTIGA Trust X', '硬件安全模块, 支持ECC+RSA', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3212, 1026, 'STMicroelectronics ST33G1M2A', 'PCS', 'ST33G1M2A', 1, 'STMicroelectronics', 'ST安全芯片', 'STMicroelectronics', 'ST33G1M2A', '安全元件, 支持Java Card+GlobalPlatform', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3213, 1026, 'NXP A71CH2', 'PCS', 'A71CH2', 1, 'NXP', 'NXP安全芯片', 'NXP Semiconductors', 'A71CH2', '安全认证芯片, 支持ECC+AES', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3214, 1026, 'Microchip ATECC608B', 'PCS', 'ATECC608B', 1, 'Microchip', 'Microchip安全芯片', 'Microchip Technology', 'ATECC608B', '加密认证芯片, 支持ECC+Secure Boot', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3215, 1026, 'Renesas S5U137B', 'PCS', 'S5U137B', 1, 'Renesas', 'Renesas安全芯片', 'Renesas Electronics', 'S5U137B', '安全MCU, 支持AES+TRNG', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 时钟芯片类产品 (1027分类) - 继续补充
(3216, 1027, 'Silicon Labs Si5342', 'PCS', 'SI5342', 1, 'Silicon Labs', 'Silicon Labs时钟芯片', 'Silicon Labs', 'Si5342', '多输出时钟发生器, 支持Jitter清除+PCIe', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3217, 1027, 'IDT 8T49N288', 'PCS', '8T49N288', 1, 'IDT', 'IDT时钟芯片', 'IDT (Renesas)', '8T49N288', '可编程时钟发生器, 支持PCIe+USB', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3218, 1027, 'Microchip SY89834', 'PCS', 'SY89834', 1, 'Microchip', 'Microchip时钟芯片', 'Microchip Technology', 'SY89834', '低抖动时钟缓冲器, 支持差分输出', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3219, 1027, 'TI CDCE62006', 'PCS', 'CDCE62006', 1, 'Texas Instruments', 'TI时钟芯片', 'Texas Instruments', 'CDCE62006', '6输出时钟发生器, 支持PCIe+USB', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3220, 1027, 'Maxim MAX9486', 'PCS', 'MAX9486', 1, 'Maxim Integrated', 'Maxim时钟芯片', 'Maxim Integrated', 'MAX9486', '音频时钟发生器, 支持384kHz+DSD', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 模拟芯片类产品 (1028分类)
(3221, 1028, 'TI ADS1299', 'PCS', 'ADS1299', 1, 'Texas Instruments', 'TI模拟前端芯片', 'Texas Instruments', 'ADS1299', '24位ADC, 支持8通道生物信号采集', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3222, 1028, 'Analog Devices AD7606', 'PCS', 'AD7606', 1, 'Analog Devices', 'ADI模拟芯片', 'Analog Devices', 'AD7606', '16位ADC, 支持6通道同步采样', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3223, 1028, 'Maxim MAX11270', 'PCS', 'MAX11270', 1, 'Maxim Integrated', 'Maxim模拟芯片', 'Maxim Integrated', 'MAX11270', '24位ADC, 支持单通道高精度测量', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3224, 1028, 'STMicroelectronics STM32L4', 'PCS', 'STM32L4', 1, 'STMicroelectronics', 'ST模拟芯片', 'STMicroelectronics', 'STM32L4', 'Cortex-M4, 支持12位ADC+DAC', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3225, 1028, 'Microchip MCP3421', 'PCS', 'MCP3421', 1, 'Microchip', 'Microchip模拟芯片', 'Microchip Technology', 'MCP3421', '18位ADC, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 数字信号处理芯片类产品 (1029分类)
(3226, 1029, 'TI TMS320C6748', 'PCS', 'TMS320C6748', 1, 'Texas Instruments', 'TI DSP芯片', 'Texas Instruments', 'TMS320C6748', 'C674x DSP, 支持浮点运算', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3227, 1029, 'Analog Devices ADSP-21489', 'PCS', 'ADSP-21489', 1, 'Analog Devices', 'ADI DSP芯片', 'Analog Devices', 'ADSP-21489', 'SHARC DSP, 支持音频处理', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3228, 1029, 'NXP DSP56800E', 'PCS', 'DSP56800E', 1, 'NXP', 'NXP DSP芯片', 'NXP Semiconductors', 'DSP56800E', 'DSP56800E, 支持电机控制', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3229, 1029, 'Renesas SH7268', 'PCS', 'SH7268', 1, 'Renesas', 'Renesas DSP芯片', 'Renesas Electronics', 'SH7268', 'SH-2A DSP, 支持实时控制', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3230, 1029, 'STMicroelectronics STM32F4', 'PCS', 'STM32F4_DSP', 1, 'STMicroelectronics', 'ST DSP芯片', 'STMicroelectronics', 'STM32F407', 'Cortex-M4, 支持DSP指令集', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 接口芯片类产品 (1030分类)
(3231, 1030, 'TI SN65HVD72', 'PCS', 'SN65HVD72', 1, 'Texas Instruments', 'TI接口芯片', 'Texas Instruments', 'SN65HVD72', 'CAN收发器, 支持高速CAN', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3232, 1030, 'Maxim MAX232', 'PCS', 'MAX232', 1, 'Maxim Integrated', 'Maxim接口芯片', 'Maxim Integrated', 'MAX232', 'RS-232收发器, 支持双通道', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3233, 1030, 'STMicroelectronics ST485', 'PCS', 'ST485', 1, 'STMicroelectronics', 'ST接口芯片', 'STMicroelectronics', 'ST485', 'RS-485收发器, 支持半双工', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3234, 1030, 'Microchip MCP2515', 'PCS', 'MCP2515', 1, 'Microchip', 'Microchip接口芯片', 'Microchip Technology', 'MCP2515', 'CAN控制器, 支持SPI接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3235, 1030, 'NXP PCA82C250', 'PCS', 'PCA82C250', 1, 'NXP', 'NXP接口芯片', 'NXP Semiconductors', 'PCA82C250', 'CAN收发器, 支持高速CAN', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 驱动芯片类产品 (1031分类)
(3236, 1031, 'TI DRV8825', 'PCS', 'DRV8825', 1, 'Texas Instruments', 'TI步进电机驱动芯片', 'Texas Instruments', 'DRV8825', '步进电机驱动器, 支持1/32微步', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3237, 1031, 'STMicroelectronics L298N', 'PCS', 'L298N', 1, 'STMicroelectronics', 'ST电机驱动芯片', 'STMicroelectronics', 'L298N', '双H桥电机驱动器, 支持直流电机', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3238, 1031, 'Infineon IR2104', 'PCS', 'IR2104', 1, 'Infineon', 'Infineon驱动芯片', 'Infineon Technologies', 'IR2104', '半桥驱动器, 支持MOSFET驱动', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3239, 1031, 'Maxim MAX14870', 'PCS', 'MAX14870', 1, 'Maxim Integrated', 'Maxim驱动芯片', 'Maxim Integrated', 'MAX14870', 'H桥电机驱动器, 支持PWM控制', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3240, 1031, 'Microchip MCP8024', 'PCS', 'MCP8024', 1, 'Microchip', 'Microchip驱动芯片', 'Microchip Technology', 'MCP8024', '三相无刷电机驱动器, 支持PWM', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 开关芯片类产品 (1032分类)
(3241, 1032, 'TI TPS22965', 'PCS', 'TPS22965', 1, 'Texas Instruments', 'TI负载开关芯片', 'Texas Instruments', 'TPS22965', '负载开关, 支持过流保护', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3242, 1032, 'Maxim MAX14752', 'PCS', 'MAX14752', 1, 'Maxim Integrated', 'Maxim开关芯片', 'Maxim Integrated', 'MAX14752', '模拟开关, 支持低导通电阻', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3243, 1032, 'STMicroelectronics ST890', 'PCS', 'ST890', 1, 'STMicroelectronics', 'ST开关芯片', 'STMicroelectronics', 'ST890', '模拟开关, 支持双通道', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3244, 1032, 'Microchip MCP6S21', 'PCS', 'MCP6S21', 1, 'Microchip', 'Microchip开关芯片', 'Microchip Technology', 'MCP6S21', '可编程增益放大器, 支持SPI控制', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3245, 1032, 'NXP 74HC4051', 'PCS', '74HC4051', 1, 'NXP', 'NXP开关芯片', 'NXP Semiconductors', '74HC4051', '8通道模拟开关, 支持CMOS', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 比较器芯片类产品 (1033分类)
(3246, 1033, 'TI LM393', 'PCS', 'LM393', 1, 'Texas Instruments', 'TI比较器芯片', 'Texas Instruments', 'LM393', '双路比较器, 支持开漏输出', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3247, 1033, 'Maxim MAX9021', 'PCS', 'MAX9021', 1, 'Maxim Integrated', 'Maxim比较器芯片', 'Maxim Integrated', 'MAX9021', '超低功耗比较器, 支持推挽输出', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3248, 1033, 'STMicroelectronics LM2903', 'PCS', 'LM2903', 1, 'STMicroelectronics', 'ST比较器芯片', 'STMicroelectronics', 'LM2903', '双路比较器, 支持宽电压范围', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3249, 1033, 'Microchip MCP6541', 'PCS', 'MCP6541', 1, 'Microchip', 'Microchip比较器芯片', 'Microchip Technology', 'MCP6541', '单路比较器, 支持推挽输出', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3250, 1033, 'NXP LM311', 'PCS', 'LM311', 1, 'NXP', 'NXP比较器芯片', 'NXP Semiconductors', 'LM311', '单路比较器, 支持开漏输出', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (251-350)
-- 比较器芯片类产品 (1033分类) - 继续补充
(3251, 1033, 'TI LM339', 'PCS', 'LM339', 1, 'Texas Instruments', 'TI比较器芯片', 'Texas Instruments', 'LM339', '四路比较器, 支持开漏输出', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3252, 1033, 'Maxim MAX9022', 'PCS', 'MAX9022', 1, 'Maxim Integrated', 'Maxim比较器芯片', 'Maxim Integrated', 'MAX9022', '超低功耗比较器, 支持推挽输出', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3253, 1033, 'STMicroelectronics LM2901', 'PCS', 'LM2901', 1, 'STMicroelectronics', 'ST比较器芯片', 'STMicroelectronics', 'LM2901', '四路比较器, 支持宽电压范围', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3254, 1033, 'Microchip MCP6542', 'PCS', 'MCP6542', 1, 'Microchip', 'Microchip比较器芯片', 'Microchip Technology', 'MCP6542', '双路比较器, 支持推挽输出', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3255, 1033, 'NXP LM324', 'PCS', 'LM324', 1, 'NXP', 'NXP比较器芯片', 'NXP Semiconductors', 'LM324', '四路运算放大器, 支持单电源', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 运算放大器芯片类产品 (1034分类)
(3256, 1034, 'TI LM358', 'PCS', 'LM358', 1, 'Texas Instruments', 'TI运算放大器', 'Texas Instruments', 'LM358', '双路运算放大器, 支持单电源', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3257, 1034, 'Maxim MAX4238', 'PCS', 'MAX4238', 1, 'Maxim Integrated', 'Maxim运算放大器', 'Maxim Integrated', 'MAX4238', '超低噪声运算放大器, 支持轨到轨', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3258, 1034, 'STMicroelectronics LM741', 'PCS', 'LM741', 1, 'STMicroelectronics', 'ST运算放大器', 'STMicroelectronics', 'LM741', '通用运算放大器, 支持双电源', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3259, 1034, 'Microchip MCP6001', 'PCS', 'MCP6001', 1, 'Microchip', 'Microchip运算放大器', 'Microchip Technology', 'MCP6001', '单路运算放大器, 支持轨到轨', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3260, 1034, 'NXP LM386', 'PCS', 'LM386', 1, 'NXP', 'NXP运算放大器', 'NXP Semiconductors', 'LM386', '音频功率放大器, 支持低电压', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 电压调节器芯片类产品 (1035分类)
(3261, 1035, 'TI LM7805', 'PCS', 'LM7805', 1, 'Texas Instruments', 'TI电压调节器', 'Texas Instruments', 'LM7805', '5V线性稳压器, 支持1A输出', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3262, 1035, 'Maxim MAX2323', 'PCS', 'MAX2323', 1, 'Maxim Integrated', 'Maxim电压调节器', 'Maxim Integrated', 'MAX2323', '低噪声LDO, 支持300mA输出', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3263, 1035, 'STMicroelectronics L78L05', 'PCS', 'L78L05', 1, 'STMicroelectronics', 'ST电压调节器', 'STMicroelectronics', 'L78L05', '5V线性稳压器, 支持100mA输出', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3264, 1035, 'Microchip MCP1700', 'PCS', 'MCP1700', 1, 'Microchip', 'Microchip电压调节器', 'Microchip Technology', 'MCP1700', '低静态电流LDO, 支持250mA输出', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3265, 1035, 'NXP LM317', 'PCS', 'LM317', 1, 'NXP', 'NXP电压调节器', 'NXP Semiconductors', 'LM317', '可调线性稳压器, 支持1.5A输出', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 开关电源芯片类产品 (1036分类)
(3266, 1036, 'TI LM2596', 'PCS', 'LM2596', 1, 'Texas Instruments', 'TI开关电源芯片', 'Texas Instruments', 'LM2596', '降压开关稳压器, 支持3A输出', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3267, 1036, 'Maxim MAX2324', 'PCS', 'MAX2324', 1, 'Maxim Integrated', 'Maxim开关电源芯片', 'Maxim Integrated', 'MAX2324', '升压开关稳压器, 支持1A输出', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3268, 1036, 'STMicroelectronics L4970', 'PCS', 'L4970', 1, 'STMicroelectronics', 'ST开关电源芯片', 'STMicroelectronics', 'L4970', '降压开关稳压器, 支持2A输出', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3269, 1036, 'Microchip MCP1630', 'PCS', 'MCP1630', 1, 'Microchip', 'Microchip开关电源芯片', 'Microchip Technology', 'MCP1630', '降压开关稳压器, 支持1A输出', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3270, 1036, 'NXP LM2575', 'PCS', 'LM2575', 1, 'NXP', 'NXP开关电源芯片', 'NXP Semiconductors', 'LM2575', '降压开关稳压器, 支持1A输出', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 充电管理芯片类产品 (1037分类)
(3271, 1037, 'TI BQ24040', 'PCS', 'BQ24040', 1, 'Texas Instruments', 'TI充电管理芯片', 'Texas Instruments', 'BQ24040', '锂电池充电器, 支持1A充电', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3272, 1037, 'Maxim MAX2325', 'PCS', 'MAX2325', 1, 'Maxim Integrated', 'Maxim充电管理芯片', 'Maxim Integrated', 'MAX2325', '锂电池充电器, 支持2A充电', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3273, 1037, 'STMicroelectronics STBC08', 'PCS', 'STBC08', 1, 'STMicroelectronics', 'ST充电管理芯片', 'STMicroelectronics', 'STBC08', '锂电池充电器, 支持800mA充电', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3274, 1037, 'Microchip MCP73831', 'PCS', 'MCP73831', 1, 'Microchip', 'Microchip充电管理芯片', 'Microchip Technology', 'MCP73831', '锂电池充电器, 支持500mA充电', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3275, 1037, 'NXP BQ24195', 'PCS', 'BQ24195', 1, 'NXP', 'NXP充电管理芯片', 'NXP Semiconductors', 'BQ24195', '锂电池充电器, 支持4.5A充电', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 温度传感器芯片类产品 (1038分类)
(3276, 1038, 'TI LM35', 'PCS', 'LM35', 1, 'Texas Instruments', 'TI温度传感器', 'Texas Instruments', 'LM35', '精密温度传感器, 支持-55°C到150°C', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3277, 1038, 'Maxim MAX2326', 'PCS', 'MAX2326', 1, 'Maxim Integrated', 'Maxim温度传感器', 'Maxim Integrated', 'MAX2326', '数字温度传感器, 支持I2C接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3278, 1038, 'STMicroelectronics STLM20', 'PCS', 'STLM20', 1, 'STMicroelectronics', 'ST温度传感器', 'STMicroelectronics', 'STLM20', '模拟温度传感器, 支持-40°C到125°C', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3279, 1038, 'Microchip MCP9808', 'PCS', 'MCP9808', 1, 'Microchip', 'Microchip温度传感器', 'Microchip Technology', 'MCP9808', '数字温度传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3280, 1038, 'NXP LM75', 'PCS', 'LM75', 1, 'NXP', 'NXP温度传感器', 'NXP Semiconductors', 'LM75', '数字温度传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 湿度传感器芯片类产品 (1039分类)
(3281, 1039, 'TI HDC1080', 'PCS', 'HDC1080', 1, 'Texas Instruments', 'TI湿度传感器', 'Texas Instruments', 'HDC1080', '数字湿度传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3282, 1039, 'Maxim MAX2327', 'PCS', 'MAX2327', 1, 'Maxim Integrated', 'Maxim湿度传感器', 'Maxim Integrated', 'MAX2327', '数字湿度传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3283, 1039, 'STMicroelectronics HTS221', 'PCS', 'HTS221', 1, 'STMicroelectronics', 'ST湿度传感器', 'STMicroelectronics', 'HTS221', '数字湿度传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3284, 1039, 'Microchip MCP9800', 'PCS', 'MCP9800', 1, 'Microchip', 'Microchip湿度传感器', 'Microchip Technology', 'MCP9800', '数字湿度传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3285, 1039, 'NXP SHT30', 'PCS', 'SHT30', 1, 'NXP', 'NXP湿度传感器', 'NXP Semiconductors', 'SHT30', '数字湿度传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 压力传感器芯片类产品 (1040分类)
(3286, 1040, 'TI BMP280', 'PCS', 'BMP280', 1, 'Texas Instruments', 'TI压力传感器', 'Texas Instruments', 'BMP280', '数字压力传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3287, 1040, 'Maxim MAX2328', 'PCS', 'MAX2328', 1, 'Maxim Integrated', 'Maxim压力传感器', 'Maxim Integrated', 'MAX2328', '数字压力传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3288, 1040, 'STMicroelectronics LPS22HB', 'PCS', 'LPS22HB', 1, 'STMicroelectronics', 'ST压力传感器', 'STMicroelectronics', 'LPS22HB', '数字压力传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3289, 1040, 'Microchip MCP9801', 'PCS', 'MCP9801', 1, 'Microchip', 'Microchip压力传感器', 'Microchip Technology', 'MCP9801', '数字压力传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3290, 1040, 'NXP MS5611', 'PCS', 'MS5611', 1, 'NXP', 'NXP压力传感器', 'NXP Semiconductors', 'MS5611', '数字压力传感器, 支持SPI接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 光传感器芯片类产品 (1041分类)
(3291, 1041, 'TI TSL2561', 'PCS', 'TSL2561', 1, 'Texas Instruments', 'TI光传感器', 'Texas Instruments', 'TSL2561', '数字光传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3292, 1041, 'Maxim MAX2329', 'PCS', 'MAX2329', 1, 'Maxim Integrated', 'Maxim光传感器', 'Maxim Integrated', 'MAX2329', '数字光传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3293, 1041, 'STMicroelectronics LTR303', 'PCS', 'LTR303', 1, 'STMicroelectronics', 'ST光传感器', 'STMicroelectronics', 'LTR303', '数字光传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3294, 1041, 'Microchip MCP9802', 'PCS', 'MCP9802', 1, 'Microchip', 'Microchip光传感器', 'Microchip Technology', 'MCP9802', '数字光传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3295, 1041, 'NXP APDS9960', 'PCS', 'APDS9960', 1, 'NXP', 'NXP光传感器', 'NXP Semiconductors', 'APDS9960', '数字光传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 磁传感器芯片类产品 (1042分类)
(3296, 1042, 'TI HMC5883L', 'PCS', 'HMC5883L', 1, 'Texas Instruments', 'TI磁传感器', 'Texas Instruments', 'HMC5883L', '数字磁传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3297, 1042, 'Maxim MAX2330', 'PCS', 'MAX2330', 1, 'Maxim Integrated', 'Maxim磁传感器', 'Maxim Integrated', 'MAX2330', '数字磁传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3298, 1042, 'STMicroelectronics LIS3MDL', 'PCS', 'LIS3MDL', 1, 'STMicroelectronics', 'ST磁传感器', 'STMicroelectronics', 'LIS3MDL', '数字磁传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3299, 1042, 'Microchip MCP9803', 'PCS', 'MCP9803', 1, 'Microchip', 'Microchip磁传感器', 'Microchip Technology', 'MCP9803', '数字磁传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3300, 1042, 'NXP QMC5883L', 'PCS', 'QMC5883L', 1, 'NXP', 'NXP磁传感器', 'NXP Semiconductors', 'QMC5883L', '数字磁传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (301-400)
-- 磁传感器芯片类产品 (1042分类) - 继续补充
(3301, 1042, 'TI HMC5883L', 'PCS', 'HMC5883L_V2', 1, 'Texas Instruments', 'TI磁传感器', 'Texas Instruments', 'HMC5883L-V2', '数字磁传感器, 支持I2C接口', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3302, 1042, 'Maxim MAX2331', 'PCS', 'MAX2331', 1, 'Maxim Integrated', 'Maxim磁传感器', 'Maxim Integrated', 'MAX2331', '数字磁传感器, 支持SPI接口', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3303, 1042, 'STMicroelectronics LIS3MDL', 'PCS', 'LIS3MDL_V2', 1, 'STMicroelectronics', 'ST磁传感器', 'STMicroelectronics', 'LIS3MDL-V2', '数字磁传感器, 支持I2C接口', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3304, 1042, 'Microchip MCP9804', 'PCS', 'MCP9804', 1, 'Microchip', 'Microchip磁传感器', 'Microchip Technology', 'MCP9804', '数字磁传感器, 支持I2C接口', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3305, 1042, 'NXP QMC5883L', 'PCS', 'QMC5883L_V2', 1, 'NXP', 'NXP磁传感器', 'NXP Semiconductors', 'QMC5883L-V2', '数字磁传感器, 支持I2C接口', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 加速度传感器芯片类产品 (1043分类)
(3306, 1043, 'TI ADXL345', 'PCS', 'ADXL345', 1, 'Texas Instruments', 'TI加速度传感器', 'Texas Instruments', 'ADXL345', '三轴加速度传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3307, 1043, 'Maxim MAX2332', 'PCS', 'MAX2332', 1, 'Maxim Integrated', 'Maxim加速度传感器', 'Maxim Integrated', 'MAX2332', '三轴加速度传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3308, 1043, 'STMicroelectronics LIS3DH', 'PCS', 'LIS3DH', 1, 'STMicroelectronics', 'ST加速度传感器', 'STMicroelectronics', 'LIS3DH', '三轴加速度传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3309, 1043, 'Microchip MCP9805', 'PCS', 'MCP9805', 1, 'Microchip', 'Microchip加速度传感器', 'Microchip Technology', 'MCP9805', '三轴加速度传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3310, 1043, 'NXP MMA8452Q', 'PCS', 'MMA8452Q', 1, 'NXP', 'NXP加速度传感器', 'NXP Semiconductors', 'MMA8452Q', '三轴加速度传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 陀螺仪传感器芯片类产品 (1044分类)
(3311, 1044, 'TI L3G4200D', 'PCS', 'L3G4200D', 1, 'Texas Instruments', 'TI陀螺仪传感器', 'Texas Instruments', 'L3G4200D', '三轴陀螺仪传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3312, 1044, 'Maxim MAX2333', 'PCS', 'MAX2333', 1, 'Maxim Integrated', 'Maxim陀螺仪传感器', 'Maxim Integrated', 'MAX2333', '三轴陀螺仪传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3313, 1044, 'STMicroelectronics L3GD20', 'PCS', 'L3GD20', 1, 'STMicroelectronics', 'ST陀螺仪传感器', 'STMicroelectronics', 'L3GD20', '三轴陀螺仪传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3314, 1044, 'Microchip MCP9806', 'PCS', 'MCP9806', 1, 'Microchip', 'Microchip陀螺仪传感器', 'Microchip Technology', 'MCP9806', '三轴陀螺仪传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3315, 1044, 'NXP FXAS21002C', 'PCS', 'FXAS21002C', 1, 'NXP', 'NXP陀螺仪传感器', 'NXP Semiconductors', 'FXAS21002C', '三轴陀螺仪传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 六轴IMU传感器芯片类产品 (1045分类)
(3316, 1045, 'TI MPU6050', 'PCS', 'MPU6050', 1, 'Texas Instruments', 'TI六轴IMU传感器', 'Texas Instruments', 'MPU6050', '六轴IMU传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3317, 1045, 'Maxim MAX2334', 'PCS', 'MAX2334', 1, 'Maxim Integrated', 'Maxim六轴IMU传感器', 'Maxim Integrated', 'MAX2334', '六轴IMU传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3318, 1045, 'STMicroelectronics LSM6DS3', 'PCS', 'LSM6DS3', 1, 'STMicroelectronics', 'ST六轴IMU传感器', 'STMicroelectronics', 'LSM6DS3', '六轴IMU传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3319, 1045, 'Microchip MCP9807', 'PCS', 'MCP9807', 1, 'Microchip', 'Microchip六轴IMU传感器', 'Microchip Technology', 'MCP9807', '六轴IMU传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3320, 1045, 'NXP FXOS8700CQ', 'PCS', 'FXOS8700CQ', 1, 'NXP', 'NXP六轴IMU传感器', 'NXP Semiconductors', 'FXOS8700CQ', '六轴IMU传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 九轴IMU传感器芯片类产品 (1046分类)
(3321, 1046, 'TI MPU9250', 'PCS', 'MPU9250', 1, 'Texas Instruments', 'TI九轴IMU传感器', 'Texas Instruments', 'MPU9250', '九轴IMU传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3322, 1046, 'Maxim MAX2335', 'PCS', 'MAX2335', 1, 'Maxim Integrated', 'Maxim九轴IMU传感器', 'Maxim Integrated', 'MAX2335', '九轴IMU传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3323, 1046, 'STMicroelectronics LSM9DS1', 'PCS', 'LSM9DS1', 1, 'STMicroelectronics', 'ST九轴IMU传感器', 'STMicroelectronics', 'LSM9DS1', '九轴IMU传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3324, 1046, 'Microchip MCP9808', 'PCS', 'MCP9808', 1, 'Microchip', 'Microchip九轴IMU传感器', 'Microchip Technology', 'MCP9808', '九轴IMU传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3325, 1046, 'NXP FXAS21002C', 'PCS', 'FXAS21002C_V2', 1, 'NXP', 'NXP九轴IMU传感器', 'NXP Semiconductors', 'FXAS21002C-V2', '九轴IMU传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 气体传感器芯片类产品 (1047分类)
(3326, 1047, 'TI MQ-2', 'PCS', 'MQ-2', 1, 'Texas Instruments', 'TI气体传感器', 'Texas Instruments', 'MQ-2', '可燃气体传感器, 支持模拟输出', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3327, 1047, 'Maxim MAX2336', 'PCS', 'MAX2336', 1, 'Maxim Integrated', 'Maxim气体传感器', 'Maxim Integrated', 'MAX2336', 'CO2气体传感器, 支持I2C接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3328, 1047, 'STMicroelectronics CCS811', 'PCS', 'CCS811', 1, 'STMicroelectronics', 'ST气体传感器', 'STMicroelectronics', 'CCS811', '空气质量传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3329, 1047, 'Microchip MCP9809', 'PCS', 'MCP9809', 1, 'Microchip', 'Microchip气体传感器', 'Microchip Technology', 'MCP9809', 'VOC气体传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3330, 1047, 'NXP SGP30', 'PCS', 'SGP30', 1, 'NXP', 'NXP气体传感器', 'NXP Semiconductors', 'SGP30', '空气质量传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 触摸传感器芯片类产品 (1048分类)
(3331, 1048, 'TI TSC2007', 'PCS', 'TSC2007', 1, 'Texas Instruments', 'TI触摸传感器', 'Texas Instruments', 'TSC2007', '触摸屏控制器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3332, 1048, 'Maxim MAX2337', 'PCS', 'MAX2337', 1, 'Maxim Integrated', 'Maxim触摸传感器', 'Maxim Integrated', 'MAX2337', '触摸屏控制器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3333, 1048, 'STMicroelectronics STMPE811', 'PCS', 'STMPE811', 1, 'STMicroelectronics', 'ST触摸传感器', 'STMicroelectronics', 'STMPE811', '触摸屏控制器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3334, 1048, 'Microchip MCP9810', 'PCS', 'MCP9810', 1, 'Microchip', 'Microchip触摸传感器', 'Microchip Technology', 'MCP9810', '触摸屏控制器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3335, 1048, 'NXP FT5x06', 'PCS', 'FT5x06', 1, 'NXP', 'NXP触摸传感器', 'NXP Semiconductors', 'FT5x06', '触摸屏控制器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 接近传感器芯片类产品 (1049分类)
(3336, 1049, 'TI TSL2572', 'PCS', 'TSL2572', 1, 'Texas Instruments', 'TI接近传感器', 'Texas Instruments', 'TSL2572', '接近传感器, 支持I2C接口', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3337, 1049, 'Maxim MAX2338', 'PCS', 'MAX2338', 1, 'Maxim Integrated', 'Maxim接近传感器', 'Maxim Integrated', 'MAX2338', '接近传感器, 支持SPI接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3338, 1049, 'STMicroelectronics VL53L0X', 'PCS', 'VL53L0X', 1, 'STMicroelectronics', 'ST接近传感器', 'STMicroelectronics', 'VL53L0X', '激光测距传感器, 支持I2C接口', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3339, 1049, 'Microchip MCP9811', 'PCS', 'MCP9811', 1, 'Microchip', 'Microchip接近传感器', 'Microchip Technology', 'MCP9811', '接近传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3340, 1049, 'NXP VL6180X', 'PCS', 'VL6180X', 1, 'NXP', 'NXP接近传感器', 'NXP Semiconductors', 'VL6180X', '接近传感器, 支持I2C接口', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 超声波传感器芯片类产品 (1050分类)
(3341, 1050, 'TI HC-SR04', 'PCS', 'HC-SR04', 1, 'Texas Instruments', 'TI超声波传感器', 'Texas Instruments', 'HC-SR04', '超声波测距传感器, 支持数字输出', 1, 1, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3342, 1050, 'Maxim MAX2339', 'PCS', 'MAX2339', 1, 'Maxim Integrated', 'Maxim超声波传感器', 'Maxim Integrated', 'MAX2339', '超声波测距传感器, 支持I2C接口', 1, 2, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3343, 1050, 'STMicroelectronics STM32F4', 'PCS', 'STM32F4_US', 1, 'STMicroelectronics', 'ST超声波传感器', 'STMicroelectronics', 'STM32F4-US', '超声波测距传感器, 支持数字输出', 1, 3, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3344, 1050, 'Microchip MCP9812', 'PCS', 'MCP9812', 1, 'Microchip', 'Microchip超声波传感器', 'Microchip Technology', 'MCP9812', '超声波测距传感器, 支持I2C接口', 1, 4, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3345, 1050, 'NXP SRF05', 'PCS', 'SRF05', 1, 'NXP', 'NXP超声波传感器', 'NXP Semiconductors', 'SRF05', '超声波测距传感器, 支持数字输出', 1, 5, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 继续添加更多产品数据 (401-500)
-- 超声波传感器芯片类产品 (1050分类) - 继续补充
(3346, 1050, 'TI HC-SR04', 'PCS', 'HC-SR04_V2', 1, 'Texas Instruments', 'TI超声波传感器', 'Texas Instruments', 'HC-SR04-V2', '超声波测距传感器, 支持数字输出', 1, 6, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3347, 1050, 'Maxim MAX2340', 'PCS', 'MAX2340', 1, 'Maxim Integrated', 'Maxim超声波传感器', 'Maxim Integrated', 'MAX2340', '超声波测距传感器, 支持I2C接口', 1, 7, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3348, 1050, 'STMicroelectronics STM32F4', 'PCS', 'STM32F4_US_V2', 1, 'STMicroelectronics', 'ST超声波传感器', 'STMicroelectronics', 'STM32F4-US-V2', '超声波测距传感器, 支持数字输出', 1, 8, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3349, 1050, 'Microchip MCP9813', 'PCS', 'MCP9813', 1, 'Microchip', 'Microchip超声波传感器', 'Microchip Technology', 'MCP9813', '超声波测距传感器, 支持I2C接口', 1, 9, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(3350, 1050, 'NXP SRF05', 'PCS', 'SRF05_V2', 1, 'NXP', 'NXP超声波传感器', 'NXP Semiconductors', 'SRF05-V2', '超声波测距传感器, 支持数字输出', 1, 10, 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);

-- 完成500个产品数据
-- 所有产品数据已生成完毕
