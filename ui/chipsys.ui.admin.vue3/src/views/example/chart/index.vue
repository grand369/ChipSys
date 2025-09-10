<template>
  <div class="chart-scrollbar layout-padding">
    <div class="chart-warp layout-padding-auto layout-padding-view">
      <div class="chart-warp-top">
        <ChartHead />
      </div>
      <div class="chart-warp-bottom">
        <!-- 左边 -->
        <div class="big-data-down-left">
          <div class="flex-warp-item">
            <div class="flex-warp-item-box">
              <div class="flex-title">天气预报</div>
              <div class="flex-content">
                <div class="sky">
                  <SvgIcon name="ele-Sunny" class="sky-left" />
                  <div class="sky-center">
                    <div class="mb2">
                      <span>多云转晴</span>
                      <span>东南�?/span>
                      <span class="span ml5">�?/span>
                    </div>
                  </div>
                  <div class="sky-right">
                    <span>25</span>
                    <span>°C</span>
                  </div>
                </div>
                <div class="sky-dd">
                  <div class="sky-dl" v-for="(v, k) in state.skyList" :key="k" :class="{ 'sky-dl-first': k === 1 }">
                    <div>{{ v.v1 }}</div>
                    <div v-if="v.type === 'title'">{{ v.v2 }}</div>
                    <div v-else>
                      <SvgIcon :name="v.v2" />
                    </div>
                    <div>{{ v.v3 }}</div>
                    <div class="tip">{{ v.v5 }}</div>
                    <div>{{ v.v7 }}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="flex-warp-item">
            <div class="flex-warp-item-box">
              <div class="flex-title">当前设备状�?/div>
              <div class="flex-content flex-content-overflow">
                <div class="d-states">
                  <div class="d-states-item">
                    <SvgIcon name="ele-Odometer" class="i-bg1" />
                    <div class="d-states-flex">
                      <div class="d-states-item-label">园区设备�?/div>
                      <div class="d-states-item-value">99</div>
                    </div>
                  </div>
                  <div class="d-states-item">
                    <SvgIcon name="ele-FirstAidKit" class="i-bg2" />
                    <div class="d-states-flex">
                      <div class="d-states-item-label">预警设备�?/div>
                      <div class="d-states-item-value">10</div>
                    </div>
                  </div>
                  <div class="d-states-item">
                    <SvgIcon name="ele-VideoPlay" class="i-bg3" />
                    <div class="d-states-flex">
                      <div class="d-states-item-label">运行设备�?/div>
                      <div class="d-states-item-value">20</div>
                    </div>
                  </div>
                </div>
                <div class="d-btn">
                  <div class="d-btn-item" v-for="(v, k) in state.dBtnList" :key="k">
                    <i class="d-btn-item-left el-icon-money"></i>
                    <div class="d-btn-item-center">
                      <div>{{ v.v2 }}|{{ v.v3 }}</div>
                    </div>
                    <div class="d-btn-item-eight">{{ v.v4 }}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="flex-warp-item">
            <div class="flex-warp-item-box">
              <div class="flex-title">�?0天预警总数</div>
              <div class="flex-content">
                <div style="height: 100%" ref="chartsWarningRef"></div>
              </div>
            </div>
          </div>
        </div>

        <!-- 中间 -->
        <div class="big-data-down-center">
          <div class="big-data-down-center-one">
            <div class="big-data-down-center-one-content">
              <div style="height: 100%" ref="chartsCenterOneRef"></div>
            </div>
          </div>
          <div class="big-data-down-center-two">
            <div class="flex-warp-item-box">
              <div class="flex-title">
                <span>当前设备监测</span>
                <span class="flex-title-small">单位：次</span>
              </div>
              <div class="flex-content">
                <div class="flex-content-left">
                  <div class="monitor-item" v-for="(v, k) in state.chartData4List" :key="k">
                    <div class="monitor-wave">
                      <div class="monitor-z-index">
                        <div class="monitor-item-label">{{ v.label }}</div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="flex-content-right">
                  <div style="height: 100%" ref="chartsMonitorRef"></div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- 右边 -->
        <div class="big-data-down-right">
          <div class="flex-warp-item">
            <div class="flex-warp-item-box">
              <div class="flex-title">
                <span>�?天产品追溯扫码统�?/span>
                <span class="flex-title-small">单位：次</span>
              </div>
              <div class="flex-content">
                <div style="height: 100%" ref="chartsSevenDaysRef"></div>
              </div>
            </div>
          </div>
          <div class="flex-warp-item">
            <div class="flex-warp-item-box">
              <div class="flex-title">当前任务统计</div>
              <div class="flex-content">
                <div class="task">
                  <div class="task-item task-first-item">
                    <div class="task-item-value task-first">25</div>
                    <div class="task-item-label">待办任务</div>
                  </div>
                  <div class="task-item">
                    <div class="task-item-box task1">
                      <div class="task-item-value">12</div>
                      <div class="task-item-label">施肥</div>
                    </div>
                  </div>
                  <div class="task-item">
                    <div class="task-item-box task2">
                      <div class="task-item-value">3</div>
                      <div class="task-item-label">施药</div>
                    </div>
                  </div>
                  <div class="task-item">
                    <div class="task-item-box task3">
                      <div class="task-item-value">5</div>
                      <div class="task-item-label">农事</div>
                    </div>
                  </div>
                </div>
                <div class="progress">
                  <div class="progress-item">
                    <span>施肥�?/span>
                    <div class="progress-box">
                      <el-progress :percentage="70" color="#43bdf0"></el-progress>
                    </div>
                  </div>
                  <div class="progress-item">
                    <span>施药�?/span>
                    <div class="progress-box">
                      <el-progress :percentage="36" color="#43bdf0"></el-progress>
                    </div>
                  </div>
                  <div class="progress-item">
                    <span>农事�?/span>
                    <div class="progress-box">
                      <el-progress :percentage="91" color="#43bdf0"></el-progress>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="flex-warp-item">
            <div class="flex-warp-item-box">
              <div class="flex-title">
                <span>�?天投入品记录</span>
                <span class="flex-title-small">单位：件</span>
              </div>
              <div class="flex-content">
                <div style="height: 100%" ref="chartsInvestmentRef"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts" name="example/chartIndex">
import { defineAsyncComponent, reactive, onMounted, watch, nextTick, onActivated, ref } from 'vue'
import * as echarts from 'echarts'
import 'echarts-wordcloud'
import { storeToRefs } from 'pinia'
import { useTagsViewRoutes } from '/@/stores/tagsViewRoutes'
import { skyList, dBtnList, chartData4List } from './chart'

// 引入组件
const ChartHead = defineAsyncComponent(() => import('./head.vue'))

// 定义变量内容
const chartsCenterOneRef = ref()
const chartsSevenDaysRef = ref()
const chartsWarningRef = ref()
const chartsMonitorRef = ref()
const chartsInvestmentRef = ref()
const storesTagsViewRoutes = useTagsViewRoutes()
const { isTagsViewCurrenFull } = storeToRefs(storesTagsViewRoutes)
const state = reactive({
  skyList,
  dBtnList,
  chartData4List,
  myCharts: [] as EmptyArrayType,
})

// 初始化中间图�?
const initChartsCenterOne = () => {
  const myChart = echarts.init(chartsCenterOneRef.value)
  const option = {
    grid: {
      top: 15,
      right: 15,
      bottom: 20,
      left: 30,
    },
    tooltip: {},
    series: [
      {
        type: 'wordCloud',
        sizeRange: [12, 40],
        rotationRange: [0, 0],
        rotationStep: 45,
        gridSize: Math.random() * 20 + 5,
        shape: 'circle',
        width: '100%',
        height: '100%',
        textStyle: {
          fontFamily: 'sans-serif',
          fontWeight: 'bold',
          color: function () {
            return `rgb(${[Math.round(Math.random() * 160), Math.round(Math.random() * 160), Math.round(Math.random() * 160)].join(',')})`
          },
        },
        data: [
          { name: 'admin.ui.plus', value: 520 },
          { name: 'xiaoxue', value: 520 },
          { name: 'chipsys-admin', value: 500 },
          { name: '更名', value: 420 },
          { name: '智慧农业', value: 520 },
          { name: '男神', value: 2.64 },
          { name: '好身�?, value: 4.03 },
          { name: '校草', value: 24.95 },
          { name: '�?, value: 4.04 },
          { name: '时尚', value: 5.27 },
          { name: '阳光活力', value: 5.8 },
          { name: '初恋', value: 3.09 },
          { name: '英俊潇洒', value: 24.71 },
          { name: '霸气', value: 6.33 },
          { name: '腼腆', value: 2.55 },
          { name: '蠢萌', value: 3.88 },
          { name: '青春', value: 8.04 },
          { name: '网红', value: 5.87 },
          { name: '�?, value: 6.97 },
          { name: '认真', value: 2.53 },
          { name: '古典', value: 2.49 },
          { name: '温柔', value: 3.91 },
          { name: '有个�?, value: 3.25 },
          { name: '可爱', value: 9.93 },
          { name: '幽默诙谐', value: 3.65 },
        ],
      },
    ],
  }
  myChart.setOption(option)
  state.myCharts.push(myChart)
}
// 初始化近7天产品追溯扫码统�?
const initChartsSevenDays = () => {
  const myChart = echarts.init(chartsSevenDaysRef.value)
  const option = {
    grid: {
      top: 15,
      right: 15,
      bottom: 20,
      left: 30,
    },
    tooltip: {
      trigger: 'axis',
    },
    xAxis: {
      type: 'category',
      boundaryGap: false,
      data: ['1�?, '2�?, '3�?, '4�?, '5�?, '6�?, '7�?],
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        name: '邮件营销',
        type: 'line',
        stack: '总量',
        data: [12, 32, 11, 34, 90, 23, 21],
      },
      {
        name: '联盟广告',
        type: 'line',
        stack: '总量',
        data: [22, 82, 91, 24, 90, 30, 30],
      },
      {
        name: '视频广告',
        type: 'line',
        stack: '总量',
        data: [50, 32, 18, 14, 90, 30, 50],
      },
    ],
  }
  myChart.setOption(option)
  state.myCharts.push(myChart)
}
// 初始化近30天预警总数
const initChartsWarning = () => {
  const myChart = echarts.init(chartsWarningRef.value)
  const option = {
    grid: {
      top: 50,
      right: 20,
      bottom: 30,
      left: 30,
    },
    tooltip: {
      trigger: 'item',
    },
    series: [
      {
        name: '面积模式',
        type: 'pie',
        radius: [20, 50],
        center: ['50%', '50%'],
        roseType: 'area',
        itemStyle: {
          borderRadius: 8,
        },
        data: [
          { value: 40, name: '监测设备预警' },
          { value: 38, name: '天气预警' },
          { value: 32, name: '任务预警' },
          { value: 30, name: '病虫害预�? },
        ],
      },
    ],
  }
  myChart.setOption(option)
  state.myCharts.push(myChart)
}
// 初始化当前设备监�?
const initChartsMonitor = () => {
  const myChart = echarts.init(chartsMonitorRef.value)
  const option = {
    grid: {
      top: 15,
      right: 15,
      bottom: 20,
      left: 30,
    },
    tooltip: {
      trigger: 'axis',
    },
    xAxis: {
      type: 'category',
      boundaryGap: false,
      data: ['02:00', '04:00', '06:00', '08:00', '10:00', '12:00', '14:00'],
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        itemStyle: {
          color: '#289df5',
          borderColor: '#289df5',
          areaStyle: {
            type: 'default',
            opacity: 0.1,
          },
        },
        data: [20, 32, 31, 34, 12, 13, 20],
        type: 'line',
        areaStyle: {},
      },
    ],
  }
  myChart.setOption(option)
  state.myCharts.push(myChart)
}
// 初始化近7天投入品记录
const initChartsInvestment = () => {
  const myChart = echarts.init(chartsInvestmentRef.value)
  const option = {
    grid: {
      top: 15,
      right: 15,
      bottom: 20,
      left: 30,
    },
    tooltip: {
      trigger: 'axis',
    },
    xAxis: {
      type: 'category',
      data: ['1�?, '2�?, '3�?, '4�?, '5�?, '6�?, '7�?],
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        data: [10, 20, 15, 80, 70, 11, 30],
        type: 'bar',
      },
    ],
  }
  myChart.setOption(option)
  state.myCharts.push(myChart)
}
// 批量设置 echarts resize
const initEchartsResizeFun = () => {
  nextTick(() => {
    for (let i = 0; i < state.myCharts.length; i++) {
      state.myCharts[i].resize()
    }
  })
}
// 批量设置 echarts resize
const initEchartsResize = () => {
  window.addEventListener('resize', initEchartsResizeFun)
}
// 页面加载�?
onMounted(() => {
  initChartsCenterOne()
  initChartsSevenDays()
  initChartsWarning()
  initChartsMonitor()
  initChartsInvestment()
  initEchartsResize()
})
// 由于页面缓存原因，keep-alive
onActivated(() => {
  initEchartsResizeFun()
})
// 监听 pinia 中的 tagsview 开启全屏变化，重新 resize 图表，防止不出现/大小不变�?
watch(
  () => isTagsViewCurrenFull.value,
  () => {
    initEchartsResizeFun()
  }
)
</script>

<style scoped lang="scss">
@use './chart.scss' as *;
</style>
