<template>
  <div class="layout-pd">
    <el-card shadow="hover" header="表单表格验证">
      <el-form ref="tableRulesRef" :model="state.tableData">
        <el-table ref="tableRef" :data="state.tableData.data" border class="module-table-uncollected" max-height="200">
          <el-table-column
            v-for="(item, index) in state.tableData.header"
            :key="index"
            show-overflow-tooltip
            :prop="item.prop"
            :width="item.width"
            :label="item.label"
          >
            <template v-slot:header>
              <span v-if="item.isRequired" class="color-danger">*</span>
              <span class="pl5">{{ item.label }}</span>
              <el-tooltip v-if="item.isTooltip" effect="dark" content="这是tooltip" placement="top">
                <i class="iconfont icon-quanxian" />
              </el-tooltip>
            </template>
            <template v-slot="scope">
              <el-form-item
                :prop="`data.${scope.$index}.${item.prop}`"
                :rules="[{ required: item.isRequired, message: '不能为空', trigger: `${item.type}` == 'input' ? 'blur' : 'change' }]"
              >
                <el-select v-if="item.type === 'select'" v-model="scope.row[item.prop]" placeholder="请选择">
                  <el-option v-for="sel in state.tableData.option" :key="sel.value" :label="sel.label" :value="sel.value" />
                </el-select>
                <el-date-picker
                  v-else-if="item.type === 'date'"
                  v-model="scope.row[item.prop]"
                  type="date"
                  placeholder="选择日期"
                  style="width: 100%"
                />
                <el-input v-else-if="item.type === 'input'" v-model="scope.row[item.prop]" placeholder="请输入内�? />
                <el-input v-else-if="item.type === 'dialog'" v-model="scope.row[item.prop]" readonly placeholder="请输入内�?>
                  <template v-slot:suffix>
                    <i class="iconfont icon-shouye_dongtaihui" />
                  </template>
                </el-input>
              </el-form-item>
            </template>
          </el-table-column>
        </el-table>
      </el-form>
      <el-row class="flex mt15">
        <div class="flex-margin">
          <el-button type="success" @click="onValidate(tableRulesRef)">表格验证</el-button>
          <el-button type="primary" @click="onAddRow">新增一�?/el-button>
        </div>
      </el-row>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/pagesTableRules">
import { reactive, ref, nextTick } from 'vue'
import { ElMessage } from 'element-plus'
import type { FormInstance } from 'element-plus'

// 定义变量内容
const tableRulesRef = ref<FormInstance>()
const tableRef = ref()
const state = reactive<TableRulesState>({
  tableData: {
    data: [],
    header: [
      { prop: 'a1', width: '', label: '一级分�?, isRequired: true, type: 'select' },
      { prop: 'a2', width: '', label: '二级分类', isRequired: true, type: 'select' },
      { prop: 'a3', width: '', label: '三级分类', isRequired: true, type: 'select' },
      { prop: 'a4', width: '', label: '四级分类', isRequired: true, type: 'date' },
      { prop: 'a5', width: '', label: '五级分类', isRequired: true, type: 'input' },
      { prop: 'a6', width: '', label: '六级分类', isTooltip: true, type: 'dialog' },
      { prop: 'a7', width: '', label: '演示级分�?, type: 'input' },
      { prop: 'a8', width: '', label: '颜色是分�?, type: 'input' },
    ],
    option: [
      { value: '选项1', label: '黄金�? },
      { value: '选项2', label: '双皮�? },
      { value: '选项3', label: '蚵仔�? },
    ],
  },
})

// 表格验证
const onValidate = (formEl: FormInstance | undefined) => {
  if (state.tableData.data.length <= 0) return ElMessage.warning('请先点击增加一�?)
  if (!formEl) return
  formEl.validate((valid) => {
    if (!valid) return ElMessage.warning('表格项必填未�?)
    ElMessage.success('全部验证通过')
  })
}
// 新增一�?
const onAddRow = () => {
  state.tableData.data.push({
    a1: '',
    a2: '',
    a3: '',
    a4: '',
    a5: '',
    a6: '',
    a7: '',
    a8: '',
  })
  tableRef.value.doLayout()
  nextTick(() => {
    tableRef.value.setScrollTop(1000000)
  })
}
</script>
