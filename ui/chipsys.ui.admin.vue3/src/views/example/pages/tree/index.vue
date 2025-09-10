<template>
  <div class="tree-container layout-pd">
    <el-card shadow="hover" header="element plus Tree 树形控件改成表格">
      <div v-loading="state.treeLoading">
        <div class="tree-head">
          <div class="tree-head-check"><el-checkbox v-model="state.treeCheckAll" @change="onCheckAllChange"></el-checkbox></div>
          <div class="tree-head-one">商品 ID</div>
          <div style="flex: 1; display: flex">
            <div class="tree-head-two">商品名称</div>
            <div class="tree-head-three">描述</div>
          </div>
        </div>
        <el-tree :data="state.treeTableData" show-checkbox node-key="id" ref="treeTableRef" :props="state.treeDefaultProps" @check="onCheckTree">
          <template #default="{ node, data }">
            <span class="tree-custom-node">
              <span style="flex: 1">{{ node.label }}</span>
              <span v-if="data.isShow" style="flex: 1; display: flex">
                <span style="flex: 1">{{ data.label1 }}</span>
                <span style="flex: 1">{{ data.label2 }}</span>
              </span>
            </span>
          </template>
        </el-tree>
      </div>
      <el-button @click="onSelect" class="mt15" type="primary">
        <SvgIcon name="iconfont icon-shuxingtu" />
        选择元素
      </el-button>
    </el-card>
  </div>
</template>

<script setup lang="ts" name="example/pagesTree">
import { reactive, onBeforeMount, ref } from 'vue'
import { ElMessage } from 'element-plus'

// 定义变量内容
const treeTableRef = ref()
const state = reactive({
  treeCheckAll: false,
  treeLoading: false,
  treeTableData: [] as RowTreeType[],
  treeDefaultProps: {
    children: 'children',
    label: 'label',
  },
  treeSelArr: [] as RowTreeType[],
  treeLength: 0,
})

// 初始化树的长�?
const initTreeLengh = (arr: RowTreeType[]) => {
  let count = 0
  arr.map((item) => {
    if (item.children) {
      count += item.children.length
    }
  })
  state.treeLength = count + arr.length
}
// 全选改变时
const onCheckAllChange = () => {
  if (state.treeCheckAll) {
    treeTableRef.value.setCheckedNodes(state.treeTableData)
  } else {
    treeTableRef.value.setCheckedKeys([])
  }
}
// 节点选中状态发生变化时的回�?
const onCheckTree = () => {
  state.treeSelArr = []
  state.treeSelArr = treeTableRef.value.getCheckedNodes()
  state.treeSelArr.length == state.treeLength ? (state.treeCheckAll = true) : (state.treeCheckAll = false)
}
// 选择元素按钮
const onSelect = () => {
  let treeArr = treeTableRef.value.getCheckedNodes()
  if (treeArr.length <= 0) {
    ElMessage.warning('请选择元素')
    return
  } else {
    // console.log(treeTableRef.value.getCheckedNodes());
  }
}
// 初始化树模拟数据
const getTreeData = () => {
  state.treeTableData = [
    {
      id: 1,
      label: '12987121',
      label1: '好滋好味鸡蛋�?,
      label2: '荷兰优质淡奶，奶香浓而不�?,
      isShow: true,
      children: [
        {
          id: 11,
          label: '一�?1-1',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
        {
          id: 12,
          label: '一�?1-2',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
      ],
    },
    {
      id: 2,
      label: '12987122',
      label1: '好滋好味鸡蛋�?,
      label2: '荷兰优质淡奶，奶香浓而不�?,
      isShow: true,
      children: [
        {
          id: 21,
          label: '二级 2-1',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
        {
          id: 22,
          label: '二级 2-2',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
      ],
    },
    {
      id: 3,
      label: '12987123',
      label1: '好滋好味鸡蛋�?,
      label2: '荷兰优质淡奶，奶香浓而不�?,
      isShow: true,
      children: [
        {
          id: 31,
          label: '二级 3-1',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
        {
          id: 32,
          label: '二级 3-2',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
        {
          id: 33,
          label: '二级 3-3',
          label1: '好滋好味鸡蛋�?,
          label2: '荷兰优质淡奶，奶香浓而不�?,
          isShow: false,
        },
      ],
    },
  ]
  initTreeLengh(state.treeTableData)
}
// 页面加载�?
onBeforeMount(() => {
  getTreeData()
})
</script>

<style scoped lang="scss">
.tree-container {
  .tree-head {
    height: 48px;
    line-height: 48px;
    border: 1px solid var(--next-border-color-light);
    border-bottom: none;
    display: flex;
    padding-right: 8px;
    font-weight: bold;
    color: #909399;
    .tree-head-check {
      width: 38px;
      text-align: right;
    }
    .tree-head-one,
    .tree-head-two,
    .tree-head-three {
      flex: 1;
    }
    .tree-head-one {
      padding-left: 8px;
    }
  }
  .el-tree {
    overflow: hidden;
    border-bottom: 1px solid var(--next-border-color-light);
    .tree-custom-node {
      flex: 1;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding-right: 8px;
      width: 100%;
    }
    &:deep(.el-tree-node) {
      border: 1px solid var(--next-border-color-light);
      border-bottom: none;
      color: #606266;
      .el-tree-node__content {
        line-height: 57px !important;
        height: 57px !important;
      }
      .el-tree-node__children {
        .el-tree-node {
          border: none;
        }
        .el-tree-node__content {
          border-top: 1px solid var(--next-border-color-light);
        }
      }
    }
  }
}
</style>
