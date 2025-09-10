<template>
  <el-date-picker
    v-model="state.dateRange"
    :value-format="timeFormat"
    format="YYYY-MM-DD"
    type="daterange"
    start-placeholder="开始时�?
    end-placeholder="结束时间"
    :shortcuts="state.shortcuts"
    @change="change"
  ></el-date-picker>
</template>

<script lang="ts" setup>
import dayjs from 'dayjs'
import { reactive, ref } from 'vue'

const emit = defineEmits(['update:startDate', 'update:endDate'])

const startDate = defineModel<string | null | undefined>('startDate', { default: '' })
const endDate = defineModel<string | null | undefined>('endDate', { default: '' })

const timeFormat = ref('YYYY-MM-DD').value

const state = reactive({
  dateRange: [startDate, endDate],
  shortcuts: [
    {
      text: '最近一�?,
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(1, 'years').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近半�?,
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(6, 'months').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近三�?,
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(3, 'months').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近一�?,
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(1, 'months').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近七�?,
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(7, 'days').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '最近三�?,
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().subtract(3, 'days').startOf('day').format(timeFormat)
        return [start, end]
      },
    },
    {
      text: '今天',
      value: () => {
        const end = dayjs().endOf('day').format(timeFormat)
        const start = dayjs().startOf('day').format(timeFormat)
        return [start, end]
      },
    },
  ],
})

const change = (value: any) => {
  if (value && value.length === 2) {
    startDate.value = value[0]
    endDate.value = value[1]
  } else {
    startDate.value = ''
    endDate.value = ''
  }
}
</script>
