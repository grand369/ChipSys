# ChipMgr模块初始化数据说明

## 文件说明

本目录包含ChipMgr模块的初始化数据脚本，用于创建测试数据：

### 1. dt_category_data.sql
- **产品分类数据**：50个常见芯片产品分类
  - 消费电子类：手机、电脑、平板、智能手表、游戏机芯片
  - 通信类：5G、WiFi、物联网、卫星通信芯片
  - 汽车电子类：汽车处理器、新能源汽车、自动驾驶芯片
  - 工业控制类：工控、机器人、传感器芯片
  - 存储类：存储、内存芯片
  - 显示类：显示驱动、图像处理芯片
  - 音频类：音频、语音识别芯片
  - 电源管理类：电源管理、电池管理芯片
  - 安全类：安全、生物识别芯片
  - 医疗电子类：医疗设备、健康监测芯片
  - 航空航天类：航空航天、军用芯片
  - 家电类：家电控制、空调、冰箱芯片
  - 照明类：LED驱动、智能照明芯片
  - 安防类：安防监控、门禁芯片
  - 金融类：金融终端、支付芯片
  - 教育类：教育设备、电子书芯片
  - 娱乐类：音响、投影芯片
  - 运动健身类：运动设备、智能手环芯片
  - 农业类：农业物联网、农机控制芯片
  - 环保类：环境监测、节能控制芯片
  - 其他类：通用处理器、专用芯片

- **供应商数据**：50个供应商信息
  - 国际知名芯片厂商：Intel、AMD、NVIDIA、Qualcomm、Apple
  - 亚洲芯片厂商：Samsung、TSMC、MediaTek、海思半导体
  - 中国芯片厂商：紫光展锐、中芯国际、长江存储、长鑫存储、寒武纪科技
  - 欧洲芯片厂商：ARM、STMicroelectronics、Infineon、NXP、Dialog
  - 存储芯片厂商：Micron、SK Hynix、Kioxia、Western Digital、Seagate
  - 模拟芯片厂商：Texas Instruments、Analog Devices、Maxim、Linear、Microchip
  - 通信芯片厂商：Broadcom、Marvell、Cisco、Xilinx、Altera
  - 传感器芯片厂商：Bosch、STMicroelectronics、InvenSense、Knowles、Murata
  - 电源管理芯片厂商：ON Semiconductor、Vishay、Renesas、ROHM、Toshiba
  - 中国新兴芯片厂商：地平线机器人、比特大陆、燧原科技、天数智芯、摩尔线程

### 2. dt_supplier_data.sql
- **供应商数据**：50个供应商信息
  - 国际知名芯片厂商：Intel、AMD、NVIDIA、Qualcomm、Apple
  - 亚洲芯片厂商：Samsung、TSMC、MediaTek、海思半导体
  - 中国芯片厂商：紫光展锐、中芯国际、长江存储、长鑫存储、寒武纪科技
  - 欧洲芯片厂商：ARM、STMicroelectronics、Infineon、NXP、Dialog
  - 存储芯片厂商：Micron、SK Hynix、Kioxia、Western Digital、Seagate
  - 模拟芯片厂商：Texas Instruments、Analog Devices、Maxim、Linear、Microchip
  - 通信芯片厂商：Broadcom、Marvell、Cisco、Xilinx、Altera
  - 传感器芯片厂商：Bosch、STMicroelectronics、InvenSense、Knowles、Murata
  - 电源管理芯片厂商：ON Semiconductor、Vishay、Renesas、ROHM、Toshiba
  - 中国新兴芯片厂商：地平线机器人、比特大陆、燧原科技、天数智芯、摩尔线程

### 3. dt_contact_data.sql
- **联系人数据**：每个供应商2-3个联系人，共约150个联系人
  - 包含CEO、CTO、CFO、销售总监等不同职位
  - 提供完整的联系信息

### 4. dt_product_data.sql
- **产品数据**：500个产品信息（示例包含前50个）
  - 手机芯片：Apple A17 Pro、Snapdragon 8 Gen 3、Dimensity 9300等
  - 电脑芯片：Intel Core i9-14900K、AMD Ryzen 9 7950X、Apple M3 Max等
  - 平板芯片：Apple A17 Pro iPad、Snapdragon 8cx Gen 3等
  - 智能手表芯片：Apple S9、Snapdragon Wear 4100+等
  - 游戏机芯片：PS5 APU、Xbox Series X APU、Nintendo Switch Tegra等
  - 5G芯片：Snapdragon X75、MediaTek M80等
  - WiFi芯片：FastConnect 7800、MT7927等
  - 物联网芯片：ESP32-S3、nRF52840等

### 5. dt_product_supplier_data.sql
- **产品供应商关联数据**：产品与供应商的关联关系
  - 每个产品对应1-3个不同的供应商
  - 包含供应商产品编码、采购价格、最小订购量、交期等信息

## 执行顺序

请按以下顺序执行SQL脚本：

1. **dt_category_data.sql** - 先执行，创建产品分类数据
2. **dt_supplier_data.sql** - 再执行，创建供应商数据
3. **dt_contact_data.sql** - 然后执行，创建联系人数据
4. **dt_product_data.sql** - 接着执行，创建产品数据
5. **dt_product_supplier_data.sql** - 最后执行，创建产品供应商关联数据

## 数据特点

### 产品分类特点
- 涵盖芯片行业的各个细分领域
- 分类层次清晰，便于管理和查询
- 包含中英文描述，适合国际化应用

### 供应商特点
- 包含全球主要芯片厂商
- 涵盖不同技术领域和产品类型
- 提供完整的联系信息和公司描述

### 产品特点
- 包含最新的芯片产品信息
- 涵盖不同价格段和性能等级
- 提供详细的技术规格和描述

### 关联关系特点
- 真实反映芯片行业的供应链关系
- 包含采购价格、交期等商业信息
- 支持多供应商策略

## 注意事项

1. **表名适配**：所有表名都使用dt_前缀，符合ChipMgr模块的命名规范
2. **字段适配**：所有字段都根据实体对象定义进行了调整，符合数据库字段要求
3. **ID范围**：
   - 产品分类ID：1001-1050
   - 供应商ID：2001-2050
   - 联系人ID：3001-3150
   - 产品ID：3001-3500
   - 产品供应商关联ID：4001-4500
4. **租户ID**：所有数据都关联到租户ID 718483086807109
5. **用户ID**：所有数据都关联到用户ID 161223411986501
6. **实体对象对应**：
   - CategoryEntity → dt_category
   - SupplierEntity → dt_supplier
   - ContactEntity → dt_contact
   - ProductEntity → dt_product
   - ProductSupplierEntity → dt_product_supplier

## 扩展说明

如需添加更多数据：
- 产品分类：继续使用1051-1100的ID范围
- 供应商：继续使用2051-2100的ID范围  
- 产品：继续使用3501-4000的ID范围
- 关联关系：继续使用4501-5000的ID范围

## 测试建议

1. 先在小范围测试数据上验证脚本正确性
2. 确认表结构和字段类型匹配
3. 检查外键约束和数据完整性
4. 验证业务逻辑的正确性
