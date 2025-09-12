-- ChipMgr模块供应商初始化数据脚本
-- 表名: dt_supplier

-- 供应商数据 (50个供应商信息)
INSERT INTO dt_supplier (Id, Name, Code, ContactPerson, Phone, Email, Address, Rating, Website, Status, Description, IsDeleted, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, TenantId) VALUES
-- 国际知名芯片厂商
(2001, 'Intel Corporation', 'INTEL', 'John Smith', '+1-408-765-8080', 'contact@intel.com', '2200 Mission College Blvd, Santa Clara, CA 95054, USA', 5, 'https://www.intel.com', 1, '全球领先的半导体公司，主要生产CPU、GPU等处理器', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2002, 'Advanced Micro Devices (AMD)', 'AMD', 'Lisa Su', '+1-408-749-4000', 'info@amd.com', '2485 Augustine Dr, Santa Clara, CA 95054, USA', 5, 'https://www.amd.com', 1, '高性能计算、图形和可视化技术公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2003, 'NVIDIA Corporation', 'NVIDIA', 'Jensen Huang', '+1-408-486-2000', 'info@nvidia.com', '2788 San Tomas Expressway, Santa Clara, CA 95051, USA', 5, 'https://www.nvidia.com', 1, 'GPU和AI计算平台领导者', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2004, 'Qualcomm Technologies', 'QUALCOMM', 'Cristiano Amon', '+1-858-587-1121', 'info@qualcomm.com', '5775 Morehouse Dr, San Diego, CA 92121, USA', 5, 'https://www.qualcomm.com', 1, '移动通信和无线技术公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2005, 'Apple Inc.', 'APPLE', 'Tim Cook', '+1-408-996-1010', 'info@apple.com', '1 Apple Park Way, Cupertino, CA 95014, USA', 5, 'https://www.apple.com', 1, '消费电子产品和技术服务公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 亚洲芯片厂商
(2006, 'Samsung Electronics', 'SAMSUNG', 'Kim Ki-nam', '+82-2-2255-0114', 'info@samsung.com', '129 Samsung-ro, Yeongtong-gu, Suwon-si, Gyeonggi-do, South Korea', 5, 'https://www.samsung.com', 1, '全球领先的半导体和电子设备制造商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2007, 'TSMC (Taiwan Semiconductor)', 'TSMC', 'C.C. Wei', '+886-3-563-6688', 'info@tsmc.com', 'No. 8, Li-Hsin Rd. 6, Hsinchu Science Park, Hsinchu 300-096, Taiwan', 5, 'https://www.tsmc.com', 1, '全球最大的专业集成电路制造服务公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2008, 'MediaTek Inc.', 'MEDIATEK', 'Rick Tsai', '+886-3-567-0766', 'info@mediatek.com', 'No. 1, Dusing 1st Rd., Hsinchu Science Park, Hsinchu 300-091, Taiwan', 5, 'https://www.mediatek.com', 1, '全球领先的半导体公司，专注于移动通信和数字多媒体', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2009, '联发科技股份有限公司', 'MTK', '蔡力行', '+886-3-567-0766', 'info@mediatek.com', '新竹科学园区力行一路1号', 5, 'https://www.mediatek.com', 1, '全球领先的半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2010, '海思半导体', 'HISILICON', '何庭波', '+86-755-2878-0808', 'info@hisilicon.com', '深圳市南山区科技园南区深南大道9988号', 5, 'https://www.hisilicon.com', 1, '华为旗下半导体公司，专注于芯片设计', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 中国芯片厂商
(2011, '紫光展锐', 'UNISOC', '楚庆', '+86-21-6100-2000', 'info@unisoc.com', '上海市浦东新区张江高科技园区', 4, 'https://www.unisoc.com', 1, '中国领先的芯片设计公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2012, '中芯国际', 'SMIC', '赵海军', '+86-21-3861-0000', 'info@smic.com', '上海市浦东新区张江高科技园区', 4, 'https://www.smic.com', 1, '中国领先的集成电路制造企业', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2013, '长江存储', 'YMTC', '杨士宁', '+86-27-8758-8888', 'info@ymtc.com', '武汉市东湖新技术开发区', 4, 'https://www.ymtc.com', 1, '专注于3D NAND闪存设计制造', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2014, '长鑫存储', 'CXMT', '朱一明', '+86-551-6533-8888', 'info@cxmt.com', '合肥市经济技术开发区', 4, 'https://www.cxmt.com', 1, '专注于DRAM内存芯片制造', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2015, '寒武纪科技', 'CAMBRIAN', '陈天石', '+86-10-8289-8888', 'info@cambricon.com', '北京市海淀区中关村软件园', 4, 'https://www.cambricon.com', 1, 'AI芯片设计公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 欧洲芯片厂商
(2016, 'ARM Limited', 'ARM', 'Simon Segars', '+44-1223-400-400', 'info@arm.com', '110 Fulbourn Rd, Cambridge CB1 9NJ, UK', 5, 'https://www.arm.com', 1, '全球领先的半导体知识产权提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2017, 'STMicroelectronics', 'STM', 'Jean-Marc Chery', '+41-22-929-29-29', 'info@st.com', '39 Chemin du Champ des Filles, 1228 Plan-les-Ouates, Switzerland', 5, 'https://www.st.com', 1, '全球领先的半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2018, 'Infineon Technologies', 'INFINEON', 'Reinhard Ploss', '+49-89-234-0', 'info@infineon.com', 'Am Campeon 1-12, 85579 Neubiberg, Germany', 5, 'https://www.infineon.com', 1, '全球领先的半导体解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2019, 'NXP Semiconductors', 'NXP', 'Kurt Sievers', '+31-40-272-9999', 'info@nxp.com', 'High Tech Campus 60, 5656 AG Eindhoven, Netherlands', 5, 'https://www.nxp.com', 1, '全球领先的半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2020, 'Dialog Semiconductor', 'DIALOG', 'Jalal Bagherli', '+49-89-456-0', 'info@dialog.com', 'Tower Bridge House, St Katharine''s Way, London E1W 1AA, UK', 4, 'https://www.dialog.com', 1, '混合信号集成电路设计公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 存储芯片厂商
(2021, 'Micron Technology', 'MICRON', 'Sanjay Mehrotra', '+1-208-368-4000', 'info@micron.com', '8000 S Federal Way, Boise, ID 83716, USA', 5, 'https://www.micron.com', 1, '全球领先的存储解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2022, 'SK Hynix', 'SKHYNIX', 'Lee Seok-hee', '+82-31-209-3114', 'info@skhynix.com', '2091, Gyeongchung-daero, Bubal-eup, Icheon-si, Gyeonggi-do, South Korea', 5, 'https://www.skhynix.com', 1, '全球领先的存储半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2023, 'Kioxia Corporation', 'KIOXIA', 'Nobuo Hayasaka', '+81-3-3457-3405', 'info@kioxia.com', '1-1, Kamikodanaka 4-chome, Nakahara-ku, Kawasaki, Kanagawa, Japan', 5, 'https://www.kioxia.com', 1, '全球领先的存储解决方案公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2024, 'Western Digital', 'WDC', 'David Goeckeler', '+1-408-717-6000', 'info@wdc.com', '5601 Great Oaks Pkwy, San Jose, CA 95119, USA', 5, 'https://www.wdc.com', 1, '全球领先的数据存储解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2025, 'Seagate Technology', 'SEAGATE', 'Dave Mosley', '+1-408-658-1000', 'info@seagate.com', '10200 S De Anza Blvd, Cupertino, CA 95014, USA', 5, 'https://www.seagate.com', 1, '全球领先的数据存储解决方案公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 模拟芯片厂商
(2026, 'Texas Instruments', 'TI', 'Rich Templeton', '+1-972-995-2011', 'info@ti.com', '12500 TI Blvd, Dallas, TX 75243, USA', 5, 'https://www.ti.com', 1, '全球领先的模拟和嵌入式处理半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2027, 'Analog Devices', 'ADI', 'Vincent Roche', '+1-781-329-4700', 'info@analog.com', '1 Analog Way, Wilmington, MA 01887, USA', 5, 'https://www.analog.com', 1, '全球领先的高性能模拟技术公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2028, 'Maxim Integrated', 'MAXIM', 'Tunç Doluca', '+1-408-737-7600', 'info@maxim.com', '160 Rio Robles, San Jose, CA 95134, USA', 5, 'https://www.maxim.com', 1, '全球领先的模拟和混合信号集成电路公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2029, 'Linear Technology', 'LINEAR', 'Lothar Maier', '+1-408-432-1900', 'info@linear.com', '1630 McCarthy Blvd, Milpitas, CA 95035, USA', 5, 'https://www.linear.com', 1, '高性能模拟集成电路设计公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2030, 'Microchip Technology', 'MICROCHIP', 'Steve Sanghi', '+1-480-792-7200', 'info@microchip.com', '2355 W Chandler Blvd, Chandler, AZ 85224, USA', 5, 'https://www.microchip.com', 1, '全球领先的微控制器和模拟半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 通信芯片厂商
(2031, 'Broadcom Inc.', 'BROADCOM', 'Hock Tan', '+1-408-433-8000', 'info@broadcom.com', '1320 Ridder Park Dr, San Jose, CA 95131, USA', 5, 'https://www.broadcom.com', 1, '全球领先的半导体和基础设施软件公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2032, 'Marvell Technology', 'MARVELL', 'Matt Murphy', '+1-408-222-2500', 'info@marvell.com', '5488 Marvell Ln, Santa Clara, CA 95054, USA', 5, 'https://www.marvell.com', 1, '全球领先的数据基础设施半导体解决方案公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2033, 'Cisco Systems', 'CISCO', 'Chuck Robbins', '+1-408-526-4000', 'info@cisco.com', '170 W Tasman Dr, San Jose, CA 95134, USA', 5, 'https://www.cisco.com', 1, '全球领先的网络设备和技术公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2034, 'Xilinx Inc.', 'XILINX', 'Victor Peng', '+1-408-559-7778', 'info@xilinx.com', '2100 Logic Dr, San Jose, CA 95124, USA', 5, 'https://www.xilinx.com', 1, '全球领先的可编程逻辑器件公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2035, 'Altera Corporation', 'ALTERA', 'John Daane', '+1-408-544-7000', 'info@altera.com', '101 Innovation Dr, San Jose, CA 95134, USA', 5, 'https://www.altera.com', 1, '可编程逻辑器件和嵌入式处理器公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 传感器芯片厂商
(2036, 'Bosch Sensortec', 'BOSCH_SENSOR', 'Stefan Finkbeiner', '+49-89-552-0', 'info@bosch-sensortec.com', 'Robert-Bosch-Str. 1, 71272 Renningen, Germany', 5, 'https://www.bosch-sensortec.com', 1, '全球领先的MEMS传感器解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2037, 'STMicroelectronics Sensors', 'STM_SENSOR', 'Jean-Marc Chery', '+41-22-929-29-29', 'info@st.com', '39 Chemin du Champ des Filles, 1228 Plan-les-Ouates, Switzerland', 5, 'https://www.st.com', 1, '全球领先的传感器解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2038, 'InvenSense', 'INVENSENSE', 'Behrooz Abdi', '+1-408-988-7339', 'info@invensense.com', '1745 Technology Dr, San Jose, CA 95110, USA', 4, 'https://www.invensense.com', 1, 'MEMS传感器和音频解决方案公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2039, 'Knowles Corporation', 'KNOWLES', 'Jeffrey Niew', '+1-630-250-5100', 'info@knowles.com', '1151 Maplewood Dr, Itasca, IL 60143, USA', 4, 'https://www.knowles.com', 1, '全球领先的音频和MEMS解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2040, 'Murata Manufacturing', 'MURATA', 'Tsuneo Murata', '+81-77-568-0111', 'info@murata.com', '10-1, Higashikotari 1-chome, Nagaokakyo-shi, Kyoto, Japan', 5, 'https://www.murata.com', 1, '全球领先的电子元器件制造商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 电源管理芯片厂商
(2041, 'ON Semiconductor', 'ON_SEMI', 'Hassane El-Khoury', '+1-602-244-6600', 'info@onsemi.com', '5005 E McDowell Rd, Phoenix, AZ 85008, USA', 5, 'https://www.onsemi.com', 1, '全球领先的半导体解决方案提供商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2042, 'Vishay Intertechnology', 'VISHAY', 'Gerald Paul', '+1-610-644-1300', 'info@vishay.com', '63 Lincoln Hwy, Malvern, PA 19355, USA', 5, 'https://www.vishay.com', 1, '全球领先的电子元器件制造商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2043, 'Renesas Electronics', 'RENESAS', 'Hidetoshi Shibata', '+81-3-6756-5555', 'info@renesas.com', '3-2-24 Toyosu, Koto-ku, Tokyo 135-0061, Japan', 5, 'https://www.renesas.com', 1, '全球领先的微控制器和模拟半导体公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2044, 'ROHM Semiconductor', 'ROHM', 'Isao Matsumoto', '+81-75-311-2121', 'info@rohm.com', '21 Saiin Mizosaki-cho, Ukyo-ku, Kyoto 615-8585, Japan', 5, 'https://www.rohm.com', 1, '全球领先的半导体和电子元器件制造商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2045, 'Toshiba Electronic Devices', 'TOSHIBA', 'Hiroyuki Sato', '+81-3-3457-2101', 'info@toshiba.com', '1-1-1 Shibaura, Minato-ku, Tokyo 105-8001, Japan', 5, 'https://www.toshiba.com', 1, '全球领先的半导体和电子设备制造商', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),

-- 中国新兴芯片厂商
(2046, '地平线机器人', 'HORIZON_ROBOTICS', '余凯', '+86-10-8289-8888', 'info@horizon.ai', '北京市海淀区中关村软件园', 4, 'https://www.horizon.ai', 1, 'AI芯片和解决方案公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2047, '比特大陆', 'BITMAIN', '吴忌寒', '+86-10-8289-8888', 'info@bitmain.com', '北京市海淀区中关村软件园', 4, 'https://www.bitmain.com', 1, '区块链和AI芯片公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2048, '燧原科技', 'ENFLAME', '赵立东', '+86-21-6100-2000', 'info@enflame-tech.com', '上海市浦东新区张江高科技园区', 4, 'https://www.enflame-tech.com', 1, 'AI训练芯片公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2049, '天数智芯', 'ILUVATAR', '李云鹏', '+86-21-6100-2000', 'info@iluvatar.com', '上海市浦东新区张江高科技园区', 4, 'https://www.iluvatar.com', 1, 'AI推理芯片公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109),
(2050, '摩尔线程', 'MOORE_THREADS', '张建中', '+86-10-8289-8888', 'info@moorethreads.com', '北京市海淀区中关村软件园', 4, 'https://www.moorethreads.com', 1, 'GPU芯片设计公司', 0, 161223411986501, 'admin', '管理员', '2025-01-10 10:00:00', 718483086807109);
