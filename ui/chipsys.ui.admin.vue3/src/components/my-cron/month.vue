<template>
  <el-form>
    <el-form-item>
      <el-radio v-model="radioValue" :label="1"> 月，允许的通配符[, - * /] </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="2">
        周期�?
        <el-input-number v-model="cycle01" :min="1" :max="11" /> - <el-input-number v-model="cycle02" :min="cycle01 + 1" :max="12" /> �?
      </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="3">
        �?
        <el-input-number v-model="average01" :min="1" :max="11" /> 月开始，�?
        <el-input-number v-model="average02" :min="1" :max="12 - average01" /> 月月执行一�?
      </el-radio>
    </el-form-item>

    <el-form-item>
      <el-radio v-model="radioValue" :label="4"> 指定 </el-radio>
      <el-select clearable v-model="checkboxList" placeholder="可多�? multiple :multiple-limit="8">
        <el-option v-for="item in monthList" :key="item.key" :label="item.value" :value="item.key" />
      </el-select>
    </el-form-item>
  </el-form>
</template>

<script lang="ts" setup>
import { ref, computed, watch, nextTick } from 'vue'
const emit = defineEmits(['update'])
const props = defineProps({
  cron: {
    type: Object,
    default: () => {
      return {
        second: '*',
        min: '*',
        hour: '*',
        day: '*',
        month: '*',
        week: '?',
        year: '',
      }
    },
  },
  check: {
    type: Function,
    default: () => {},
  },
})
const radioValue = ref(1)
const cycle01 = ref(1)
const cycle02 = ref(2)
const average01 = ref(1)
const average02 = ref(1)
const checkboxList = ref([])
const checkCopy = ref([1])
const monthList = ref([
  { key: 1, value: '一�? },
  { key: 2, value: '二月' },
  { key: 3, value: '三月' },
  { key: 4, value: '四月' },
  { key: 5, value: '五月' },
  { key: 6, value: '六月' },
  { key: 7, value: '七月' },
  { key: 8, value: '八月' },
  { key: 9, value: '九月' },
  { key: 10, value: '十月' },
  { key: 11, value: '十一�? },
  { key: 12, value: '十二�? },
])
const cycleTotal = computed(() => {
  return cycle01.value + '-' + cycle02.value
})
const averageTotal = computed(() => {
  return average01.value + '/' + average02.value
})
const checkboxString = computed(() => {
  return checkboxList.value.join(',')
})
watch([cycle01, cycle02], () => {
  cycle01.value = props.check(cycle01.value, 1, 11)
  cycle02.value = props.check(cycle02.value, cycle01.value + 1, 12)
})
watch([average01, average02], () => {
  average01.value = props.check(average01.value, 1, 11)
  average02.value = props.check(average02.value, 1, 12 - average01.value)
})
watch(
  () => props.cron.month,
  (value) => {
    nextTick(() => {
      changeRadioValue(value)
    })
  },
  {
    deep: true,
    immediate: true,
  }
)
watch([radioValue, cycleTotal, averageTotal, checkboxString], () => onRadioChange())
const changeRadioValue = (value: any) => {
  if (value === '*') {
    radioValue.value = 1
  } else if (value.indexOf('-') > -1) {
    const indexArr = value.split('-')
    cycle01.value = Number(indexArr[0])
    cycle02.value = Number(indexArr[1])
    radioValue.value = 2
  } else if (value.indexOf('/') > -1) {
    const indexArr = value.split('/')
    average01.value = Number(indexArr[0])
    average02.value = Number(indexArr[1])
    radioValue.value = 3
  } else {
    checkboxList.value = [...new Set(value.split(',').map((item: any) => Number(item)))] as never[]
    radioValue.value = 4
  }
}
const onRadioChange = () => {
  switch (radioValue.value) {
    case 1:
      emit('update', 'month', '*', 'month')
      break
    case 2:
      emit('update', 'month', cycleTotal.value, 'month')
      break
    case 3:
      emit('update', 'month', averageTotal.value, 'month')
      break
    case 4:
      if (checkboxList.value.length === 0) {
        checkboxList.value.push(checkCopy.value[0] as never)
      } else {
        checkCopy.value = checkboxList.value
      }
      emit('update', 'month', checkboxString.value, 'month')
      break
  }
}
</script>

<style lang="scss" scoped>
.el-input-number--small,
.el-select,
.el-select--small {
  margin: 0 0.2rem;
}
.el-select,
.el-select--small {
  width: 18.8rem;
}
.el-radio {
  margin-right: 5px;
}
</style>
