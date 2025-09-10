<template>
  <el-tooltip effect="dark" content="列设�? placement="top">
    <el-button ref="colSetRef" icon="ele-Setting" circle v-bind="$attrs" />
  </el-tooltip>
  <el-popover
    ref="colSetPopoverRef"
    placement="bottom-end"
    transition="el-zoom-in-top"
    :virtual-ref="colSetRef"
    trigger="click"
    virtual-triggering
    :width="300"
    popper-class="my-col-set-popper"
    @show="onColSet"
  >
    <div class="my-col-set-header-box">
      <div class="my-flex my-flex-between">
        <div class="my-flex my-flex-items-center">
          <SvgIcon name="ele-Rank" title="点击并拖动进行排�? />
          <el-checkbox v-model="checkAll" :indeterminate="checkIndeterminate" class="ml12" label="全部" @change="onCheckAllChange" />
        </div>
        <el-button type="primary" link @click="onResetDefault">恢复默认</el-button>
      </div>
    </div>
    <el-scrollbar>
      <div ref="sortableRef" class="my-col-set-body-box">
        <div v-for="(item, index) in colsModel" :key="item.attrs.prop" :data-id="item.attrs.prop" class="my-flex my-flex-between my-col-set-item-box">
          <div class="my-flex my-flex-fill">
            <el-button link class="handle">
              <template #icon>
                <SvgIcon name="ele-Rank" />
              </template>
            </el-button>
            <el-checkbox v-model="item.isShow" class="ml8 mr8" :label="item.attrs.label" />
          </div>
          <div class="my-flex">
            <el-button link title="置顶" @click="onMoveToTop(item)">
              <template #icon>
                <el-icon size="18px">
                  <my-icon name="toTop" color="var(--color)"></my-icon>
                </el-icon>
              </template>
            </el-button>
            <el-button
              link
              :title="isFixedLeft(item) ? '取消固定在左�? : '固定在左�?"
              :class="isFixedLeft(item) ? 'selected' : ''"
              @click="onFixedLeft(item)"
            >
              <template #icon>
                <el-icon size="18px">
                  <my-icon :name="isFixedLeft(item) ? 'fixedLeftFill' : 'fixedLeft'" color="var(--color)"></my-icon>
                </el-icon>
              </template>
            </el-button>
            <el-button
              link
              :title="isFixedRight(item) ? '取消固定在右�? : '固定在右�?"
              :class="isFixedRight(item) ? 'selected' : ''"
              @click="onFixedRight(item)"
            >
              <template #icon>
                <el-icon size="18px">
                  <my-icon :name="isFixedRight(item) ? 'fixedRightFill' : 'fixedRight'" color="var(--color)"></my-icon>
                </el-icon>
              </template>
            </el-button>
          </div>
        </div>
      </div>
    </el-scrollbar>
  </el-popover>
</template>

<script lang="ts" setup>
import { useTemplateRef, computed, nextTick } from 'vue'
import Sortable from 'sortablejs'
import { cloneDeep } from 'lodash-es'

// 定义父组件传过来的列数组模型
const colsModel = defineModel({
  type: Array<{
    attrs: Record<string, any>
    slot?: string
    isShow?: boolean
  }>,
  required: true,
})

const colsModelOrg = cloneDeep(colsModel.value)

const colSetRef = useTemplateRef('colSetRef')
useTemplateRef('colSetPopoverRef')
const sortableRef = useTemplateRef('sortableRef')

// 全�?
const checkAll = computed(() => {
  const headers = colsModel.value.filter((v) => v.isShow).length
  return headers === colsModel.value.length
})

// 半�?
const checkIndeterminate = computed(() => {
  const headers = colsModel.value.filter((v) => v.isShow).length
  return headers > 0 && headers < colsModel.value.length
})

// 全选变�?
const onCheckAllChange = <T,>(val: T) => {
  if (val) colsModel.value.forEach((v) => (v.isShow = true))
  else colsModel.value.forEach((v) => (v.isShow = false))
}

// 列设�?
const onColSet = () => {
  nextTick(() => {
    const sortable = Sortable.create(sortableRef.value, {
      handle: '.handle',
      dataIdAttr: 'data-id',
      animation: 150,
      onEnd: () => {
        const headerList: Array<{ attrs: Record<string, any>; slot?: string; isShow?: boolean }> = []
        sortable.toArray().forEach((val: string) => {
          colsModel.value.forEach((v) => {
            if (v.attrs.prop === val) headerList.push({ ...v })
          })
        })
        colsModel.value = headerList
      },
    })
  })
}

// 恢复默认
const onResetDefault = () => {
  colsModel.value = cloneDeep(colsModelOrg)
}

// 置顶
const onMoveToTop = (item: any) => {
  const index = colsModel.value.findIndex((v) => v.attrs.prop === item.attrs.prop)
  if (index > 0) {
    const [removed] = colsModel.value.splice(index, 1)
    colsModel.value.unshift(removed)
  }
}

// 判断是否固定在左�?
const isFixedLeft = (item: any): boolean => {
  return item.attrs.fixed === true || item.attrs.fixed === 'left'
}

// 判断是否固定在右�?
const isFixedRight = (item: any): boolean => {
  return item.attrs.fixed === 'right'
}

// 固定在左�?
const onFixedLeft = (item: any) => {
  if (isFixedLeft(item)) item.attrs.fixed = false
  else item.attrs.fixed = 'left'
}

// 固定在右�?
const onFixedRight = (item: any) => {
  if (isFixedRight(item)) item.attrs.fixed = false
  else item.attrs.fixed = 'right'
}
</script>

<style lang="scss">
.my-col-set-popper {
  padding: 0px !important;
}
</style>
<style scoped lang="scss">
.my-col-set-header-box {
  border-bottom: 1px solid var(--el-border-color-lighter);
  padding: 4px 12px;
}
.my-col-set-body-box {
  margin: 0px;
  padding: 0px;
  max-height: 303px;
  .handle {
    cursor: grab;
  }

  .my-col-set-item-box {
    padding: 0px 12px 0px 10px;
    :deep() {
      .el-button + .el-button {
        margin-left: 6px;
      }

      .el-button.is-link.selected {
        color: var(--el-color-primary-light-3);
      }
    }
  }
}
</style>
