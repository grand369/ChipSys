<template>
  <div class="popup-result">
    <p class="title">最�?次运行时�?/p>
    <ul class="popup-result-scroll">
      <template v-if="isShow">
        <li v-for="item in resultList" :key="item">{{ item }}</li>
      </template>
      <li v-else>计算结果�?..</li>
    </ul>
  </div>
</template>

<script lang="ts" setup>
import { ref, watch, onMounted } from 'vue'
const props = defineProps({
  ex: {
    type: String,
    default: '',
  },
})
const dayRule = ref('')
const dayRuleSup = ref('') as any
const dateArr = ref([]) as any
const resultList = ref([]) as any
const isShow = ref(false)
watch(
  () => props.ex,
  () => expressionChange()
)
// 表达式值变化时，开始去计算结果
const expressionChange = () => {
  // 计算开�?隐藏结果
  isShow.value = false
  // 获取规则数组[0秒�?分�?时�?日�?月�?星期�?年]
  let ruleArr = props.ex.split(' ')
  // 用于记录进入循环的次�?
  let nums = 0
  // 用于暂时存符号时间规则结果的数组
  let resultArr = [] as any
  // 获取当前时间精确至[年、月、日、时、分、秒]
  let nTime = new Date()
  let nYear = nTime.getFullYear()
  let nMonth = nTime.getMonth() + 1
  let nDay = nTime.getDate()
  let nHour = nTime.getHours()
  let nMin = nTime.getMinutes()
  let nSecond = nTime.getSeconds()
  // 根据规则获取到近100年可能年数组、月数组等等
  getSecondArr(ruleArr[0])
  getMinArr(ruleArr[1])
  getHourArr(ruleArr[2])
  getDayArr(ruleArr[3])
  getMonthArr(ruleArr[4])
  getWeekArr(ruleArr[5])
  getYearArr(ruleArr[6], nYear)
  // 将获取到的数组赋�?方便使用
  let sDate = dateArr.value[0] as any
  let mDate = dateArr.value[1] as any
  let hDate = dateArr.value[2] as any
  let DDate = dateArr.value[3] as any
  let MDate = dateArr.value[4] as any
  let YDate = dateArr.value[5] as any
  // 获取当前时间在数组中的索�?
  let sIdx = getIndex(sDate, nSecond) as any
  let mIdx = getIndex(mDate, nMin) as any
  let hIdx = getIndex(hDate, nHour) as any
  let DIdx = getIndex(DDate, nDay) as any
  let MIdx = getIndex(MDate, nMonth) as any
  let YIdx = getIndex(YDate, nYear) as any
  // 重置月日时分秒的函数(后面用的比较�?
  const resetSecond = () => {
    sIdx = 0
    nSecond = sDate[sIdx]
  }
  const resetMin = () => {
    mIdx = 0
    nMin = mDate[mIdx]
    resetSecond()
  }
  const resetHour = () => {
    hIdx = 0
    nHour = hDate[hIdx]
    resetMin()
  }
  const resetDay = () => {
    DIdx = 0
    nDay = DDate[DIdx]
    resetHour()
  }
  const resetMonth = () => {
    MIdx = 0
    nMonth = MDate[MIdx]
    resetDay()
  }
  // 如果当前年份不为数组中当前�?
  if (nYear !== YDate[YIdx]) {
    resetMonth()
  }
  // 如果当前月份不为数组中当前�?
  if (nMonth !== MDate[MIdx]) {
    resetDay()
  }
  // 如果当前“日”不为数组中当前�?
  if (nDay !== DDate[DIdx]) {
    resetHour()
  }
  // 如果当前“时”不为数组中当前�?
  if (nHour !== hDate[hIdx]) {
    resetMin()
  }
  // 如果当前“分”不为数组中当前�?
  if (nMin !== mDate[mIdx]) {
    resetSecond()
  }
  // 循环年份数组
  goYear: for (let Yi = YIdx; Yi < YDate.length; Yi++) {
    let YY = YDate[Yi]
    // 如果到达最大值时
    if (nMonth > MDate[MDate.length - 1]) {
      resetMonth()
      continue
    }
    // 循环月份数组
    goMonth: for (let Mi = MIdx; Mi < MDate.length; Mi++) {
      // 赋值、方便后面运�?
      let MM = MDate[Mi]
      MM = MM < 10 ? '0' + MM : MM
      // 如果到达最大值时
      if (nDay > DDate[DDate.length - 1]) {
        resetDay()
        if (Mi === MDate.length - 1) {
          resetMonth()
          continue goYear
        }
        continue
      }
      // 循环日期数组
      goDay: for (let Di = DIdx; Di < DDate.length; Di++) {
        // 赋值、方便后面运�?
        let DD = DDate[Di]
        let thisDD = DD < 10 ? '0' + DD : DD
        // 如果到达最大值时
        if (nHour > hDate[hDate.length - 1]) {
          resetHour()
          if (Di === DDate.length - 1) {
            resetDay()
            if (Mi === MDate.length - 1) {
              resetMonth()
              continue goYear
            }
            continue goMonth
          }
          continue
        }
        // 判断日期的合法性，不合法的话也是跳出当前循�?
        if (
          checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true &&
          dayRule.value !== 'workDay' &&
          dayRule.value !== 'lastWeek' &&
          dayRule.value !== 'lastDay'
        ) {
          resetDay()
          continue goMonth
        }
        // 如果日期规则中有值时
        if (dayRule.value === 'lastDay') {
          // 如果不是合法日期则需要将前将日期调到合法日期即月末最后一�?
          if (checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
            while (DD > 0 && checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
              DD--
              thisDD = DD < 10 ? '0' + DD : DD
            }
          }
        } else if (dayRule.value === 'workDay') {
          // 校验并调整如果是2�?0号这种日期传进来时需调整至正常月�?
          if (checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
            while (DD > 0 && checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
              DD--
              thisDD = DD < 10 ? '0' + DD : DD
            }
          }
          // 获取达到条件的日期是星期X
          let thisWeek = formatDate(new Date(YY + '-' + MM + '-' + thisDD + ' 00:00:00'), 'week')
          // 当星期日�?
          if (thisWeek === 1) {
            // 先找下一个日，并判断是否为月�?
            DD++
            thisDD = DD < 10 ? '0' + DD : DD
            // 判断下一日已经不是合法日�?
            if (checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
              DD -= 3
            }
          } else if (thisWeek === 7) {
            // 当星�?时只需判断不是1号就可进行操�?
            if (dayRuleSup.value !== 1) {
              DD--
            } else {
              DD += 2
            }
          }
        } else if (dayRule.value === 'weekDay') {
          // 如果指定了是星期�?
          // 获取当前日期是属于星期几
          let thisWeek = formatDate(new Date(YY + '-' + MM + '-' + DD + ' 00:00:00'), 'week')
          // 校验当前星期是否在星期池（dayRuleSup）中
          if (dayRuleSup.value.indexOf(thisWeek) < 0) {
            // 如果到达最大值时
            if (Di === DDate.length - 1) {
              resetDay()
              if (Mi === MDate.length - 1) {
                resetMonth()
                continue goYear
              }
              continue goMonth
            }
            continue
          }
        } else if (dayRule.value === 'assWeek') {
          // 如果指定了是第几周的星期�?
          // 获取每月1号是属于星期�?
          let thisWeek = formatDate(new Date(YY + '-' + MM + '-' + DD + ' 00:00:00'), 'week')
          if (dayRuleSup.value[1] >= thisWeek) {
            DD = (dayRuleSup.value[0] - 1) * 7 + dayRuleSup.value[1] - thisWeek + 1
          } else {
            DD = dayRuleSup.value[0] * 7 + dayRuleSup.value[1] - thisWeek + 1
          }
        } else if (dayRule.value === 'lastWeek') {
          // 如果指定了每月最后一个星期几
          // 校验并调整如果是2�?0号这种日期传进来时需调整至正常月�?
          if (checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
            while (DD > 0 && checkDate(YY + '-' + MM + '-' + thisDD + ' 00:00:00') !== true) {
              DD--
              thisDD = DD < 10 ? '0' + DD : DD
            }
          }
          // 获取月末最后一天是星期�?
          let thisWeek = formatDate(new Date(YY + '-' + MM + '-' + thisDD + ' 00:00:00'), 'week')
          // 找到要求中最近的那个星期�?
          if (dayRuleSup.value < thisWeek) {
            DD -= thisWeek - dayRuleSup.value
          } else if (dayRuleSup.value > thisWeek) {
            DD -= 7 - (dayRuleSup.value - thisWeek)
          }
        }
        // 判断时间值是否小�?0置换成�?5”这种格�?
        DD = DD < 10 ? '0' + DD : DD
        // 循环“时”数�?
        goHour: for (let hi = hIdx; hi < hDate.length; hi++) {
          let hh = hDate[hi] < 10 ? '0' + hDate[hi] : hDate[hi]
          // 如果到达最大值时
          if (nMin > mDate[mDate.length - 1]) {
            resetMin()
            if (hi === hDate.length - 1) {
              resetHour()
              if (Di === DDate.length - 1) {
                resetDay()
                if (Mi === MDate.length - 1) {
                  resetMonth()
                  continue goYear
                }
                continue goMonth
              }
              continue goDay
            }
            continue
          }
          // 循环"�?数组
          goMin: for (let mi = mIdx; mi < mDate.length; mi++) {
            let mm = mDate[mi] < 10 ? '0' + mDate[mi] : mDate[mi]
            // 如果到达最大值时
            if (nSecond > sDate[sDate.length - 1]) {
              resetSecond()
              if (mi === mDate.length - 1) {
                resetMin()
                if (hi === hDate.length - 1) {
                  resetHour()
                  if (Di === DDate.length - 1) {
                    resetDay()
                    if (Mi === MDate.length - 1) {
                      resetMonth()
                      continue goYear
                    }
                    continue goMonth
                  }
                  continue goDay
                }
                continue goHour
              }
              continue
            }
            // 循环"�?数组
            for (let si = sIdx; si <= sDate.length - 1; si++) {
              let ss = sDate[si] < 10 ? '0' + sDate[si] : sDate[si]
              // 添加当前时间（时间合法性在日期循环时已经判断）
              if (MM !== '00' && DD !== '00') {
                resultArr.push(YY + '-' + MM + '-' + DD + ' ' + hh + ':' + mm + ':' + ss)
                nums++
              }
              // 如果条数满了就退出循�?
              if (nums === 5) break goYear
              // 如果到达最大值时
              if (si === sDate.length - 1) {
                resetSecond()
                if (mi === mDate.length - 1) {
                  resetMin()
                  if (hi === hDate.length - 1) {
                    resetHour()
                    if (Di === DDate.length - 1) {
                      resetDay()
                      if (Mi === MDate.length - 1) {
                        resetMonth()
                        continue goYear
                      }
                      continue goMonth
                    }
                    continue goDay
                  }
                  continue goHour
                }
                continue goMin
              }
            } //goSecond
          } //goMin
        } //goHour
      } //goDay
    } //goMonth
  }
  // 判断100年内的结果条�?
  if (resultArr.length === 0) {
    resultList.value = ['没有达到条件的结果！']
  } else {
    resultList.value = resultArr
    if (resultArr.length !== 5) {
      resultList.value.push('最�?00年内只有上面' + resultArr.length + '条结果！')
    }
  }
  // 计算完成-显示结果
  isShow.value = true
}
// 用于计算某位数字在数组中的索�?
const getIndex = (arr: any, value: any) => {
  if (value <= arr[0] || value > arr[arr.length - 1]) {
    return 0
  } else {
    for (let i = 0; i < arr.length - 1; i++) {
      if (value > arr[i] && value <= arr[i + 1]) {
        return i + 1
      }
    }
  }
}
// 获取"�?数组
const getYearArr = (rule: any, year: any) => {
  dateArr.value[5] = getOrderArr(year, year + 100)
  if (rule !== undefined) {
    if (rule.indexOf('-') >= 0) {
      dateArr.value[5] = getCycleArr(rule, year + 100, false)
    } else if (rule.indexOf('/') >= 0) {
      dateArr.value[5] = getAverageArr(rule, year + 100)
    } else if (rule !== '*') {
      dateArr.value[5] = getAssignArr(rule)
    }
  }
}
// 获取"�?数组
const getMonthArr = (rule: any) => {
  dateArr.value[4] = getOrderArr(1, 12)
  if (rule.indexOf('-') >= 0) {
    dateArr.value[4] = getCycleArr(rule, 12, false)
  } else if (rule.indexOf('/') >= 0) {
    dateArr.value[4] = getAverageArr(rule, 12)
  } else if (rule !== '*') {
    dateArr.value[4] = getAssignArr(rule)
  }
}
// 获取"�?数组-主要为日期规�?
const getWeekArr = (rule: any) => {
  // 只有当日期规则的两个值均为“”时则表达日期是有选项�?
  if (dayRule.value === '' && dayRuleSup.value === '') {
    if (rule.indexOf('-') >= 0) {
      dayRule.value = 'weekDay'
      dayRuleSup.value = getCycleArr(rule, 7, false)
    } else if (rule.indexOf('#') >= 0) {
      dayRule.value = 'assWeek'
      let matchRule = rule.match(/[0-9]{1}/g)
      dayRuleSup.value = [Number(matchRule[1]), Number(matchRule[0])]
      dateArr.value[3] = [1]
      if (dayRuleSup.value[1] === 7) {
        dayRuleSup.value[1] = 0
      }
    } else if (rule.indexOf('L') >= 0) {
      dayRule.value = 'lastWeek'
      dayRuleSup.value = Number(rule.match(/[0-9]{1,2}/g)[0])
      dateArr.value[3] = [31]
      if (dayRuleSup.value === 7) {
        dayRuleSup.value = 0
      }
    } else if (rule !== '*' && rule !== '?') {
      dayRule.value = 'weekDay'
      dayRuleSup.value = getAssignArr(rule)
    }
  }
}
// 获取"�?数组-少量为日期规�?
const getDayArr = (rule: any) => {
  dateArr.value[3] = getOrderArr(1, 31)
  dayRule.value = ''
  dayRuleSup.value = ''
  if (rule.indexOf('-') >= 0) {
    dateArr.value[3] = getCycleArr(rule, 31, false)
    dayRuleSup.value = 'null'
  } else if (rule.indexOf('/') >= 0) {
    dateArr.value[3] = getAverageArr(rule, 31)
    dayRuleSup.value = 'null'
  } else if (rule.indexOf('W') >= 0) {
    dayRule.value = 'workDay'
    dayRuleSup.value = Number(rule.match(/[0-9]{1,2}/g)[0])
    dateArr.value[3] = [dayRuleSup.value]
  } else if (rule.indexOf('L') >= 0) {
    dayRule.value = 'lastDay'
    dayRuleSup.value = 'null'
    dateArr.value[3] = [31]
  } else if (rule !== '*' && rule !== '?') {
    dateArr.value[3] = getAssignArr(rule)
    dayRuleSup.value = 'null'
  } else if (rule === '*') {
    dayRuleSup.value = 'null'
  }
}
// 获取"�?数组
const getHourArr = (rule: any) => {
  dateArr.value[2] = getOrderArr(0, 23)
  if (rule.indexOf('-') >= 0) {
    dateArr.value[2] = getCycleArr(rule, 24, true)
  } else if (rule.indexOf('/') >= 0) {
    dateArr.value[2] = getAverageArr(rule, 23)
  } else if (rule !== '*') {
    dateArr.value[2] = getAssignArr(rule)
  }
}
// 获取"�?数组
const getMinArr = (rule: any) => {
  dateArr.value[1] = getOrderArr(0, 59)
  if (rule.indexOf('-') >= 0) {
    dateArr.value[1] = getCycleArr(rule, 60, true)
  } else if (rule.indexOf('/') >= 0) {
    dateArr.value[1] = getAverageArr(rule, 59)
  } else if (rule !== '*') {
    dateArr.value[1] = getAssignArr(rule)
  }
}
// 获取"�?数组
const getSecondArr = (rule: any) => {
  dateArr.value[0] = getOrderArr(0, 59)
  if (rule.indexOf('-') >= 0) {
    dateArr.value[0] = getCycleArr(rule, 60, true)
  } else if (rule.indexOf('/') >= 0) {
    dateArr.value[0] = getAverageArr(rule, 59)
  } else if (rule !== '*') {
    dateArr.value[0] = getAssignArr(rule)
  }
}
// 根据传进来的min-max返回一个顺序的数组
const getOrderArr = (min: any, max: any) => {
  let arr = [] as any
  for (let i = min; i <= max; i++) {
    arr.push(i)
  }
  return arr
}
// 根据规则中指定的零散值返回一个数�?
const getAssignArr = (rule: any) => {
  let arr = [] as any
  let assiginArr = rule.split(',')
  for (let i = 0; i < assiginArr.length; i++) {
    arr[i] = Number(assiginArr[i])
  }
  arr.sort(compare)
  return arr
}
// 根据一定算术规则计算返回一个数�?
const getAverageArr = (rule: any, limit: any) => {
  let arr = [] as any
  let agArr = rule.split('/')
  let min = Number(agArr[0])
  let step = Number(agArr[1])
  while (min <= limit) {
    arr.push(min)
    min += step
  }
  return arr
}
// 根据规则返回一个具有周期性的数组
const getCycleArr = (rule: any, limit: any, status: any) => {
  // status--表示是否�?开始（则从1开始）
  let arr = [] as any
  let cycleArr = rule.split('-')
  let min = Number(cycleArr[0])
  let max = Number(cycleArr[1])
  if (min > max) {
    max += limit
  }
  for (let i = min; i <= max; i++) {
    let add = 0
    if (status === false && i % limit === 0) {
      add = limit
    }
    arr.push(Math.round((i % limit) + add))
  }
  arr.sort(compare)
  return arr
}
// 比较数字大小（用于Array.sort�?
const compare = (value1: any, value2: any) => {
  if (value2 - value1 > 0) {
    return -1
  } else {
    return 1
  }
}
// 格式化日期格式如�?017-9-19 18:04:33
const formatDate = (value: any, type: any = undefined) => {
  // 计算日期相关�?
  let time = typeof value == 'number' ? new Date(value) : value
  let Y = time.getFullYear()
  let M = time.getMonth() + 1
  let D = time.getDate()
  let h = time.getHours()
  let m = time.getMinutes()
  let s = time.getSeconds()
  let week = time.getDay()
  // 如果传递了type的话
  if (type === undefined) {
    return (
      Y +
      '-' +
      (M < 10 ? '0' + M : M) +
      '-' +
      (D < 10 ? '0' + D : D) +
      ' ' +
      (h < 10 ? '0' + h : h) +
      ':' +
      (m < 10 ? '0' + m : m) +
      ':' +
      (s < 10 ? '0' + s : s)
    )
  } else if (type === 'week') {
    // 在quartz�?1为星期日
    return week + 1
  }
}
// 检查日期是否存�?
const checkDate = (value: any) => {
  let time = new Date(value)
  let format = formatDate(time)
  return value === format
}
onMounted(() => {
  expressionChange()
})
</script>
