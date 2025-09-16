<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="favoriteType">
          <el-select v-model="state.filterModel.favoriteType" placeholder="收藏类型" clearable class="w120">
            <el-option label="供应商" value="Supplier" />
            <el-option label="产品" value="Product" />
            <el-option label="联系人" value="Contact" />
          </el-select>
        </el-form-item>
        <el-form-item prop="favoriteName">
          <el-input v-model="state.filterModel.favoriteName" placeholder="收藏对象名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'client.member-favorite.add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.memberFavoriteListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="favoriteType" label="收藏类型" width="100" align="center">
          <template #default="{ row }">
            <el-tag :type="getFavoriteTypeTagType(row.favoriteType)">
              {{ getFavoriteTypeText(row.favoriteType) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="favoriteName" label="收藏对象名称" min-width="200" show-overflow-tooltip />
        <el-table-column prop="remark" label="备注" min-width="200" show-overflow-tooltip />
        <el-table-column prop="createdTime" label="收藏时间" width="180" align="center">
          <template #default="{ row }">
            {{ formatDate(row.createdTime) }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'client.member-favorite.update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'client.member-favorite.delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
      
      <!-- 分页组件 -->
      <el-pagination
        v-model:current-page="state.currentPage"
        v-model:page-size="state.pageSize"
        :page-sizes="[10, 20, 50, 100]"
        :total="state.total"
        layout="total, sizes, prev, pager, next, jumper"
        @size-change="onSizeChange"
        @current-change="onCurrentChange"
        class="my-pagination"
      />
    </el-card>

    <member-favorite-form ref="memberFavoriteFormRef" :title="state.memberFavoriteFormTitle"></member-favorite-form>
  </div>
</template>

<script lang="ts" setup name="client/member-favorite">
import { MemberFavoriteGetPageOutput, MemberFavoriteGetPageInput } from '/@/api/client/data-contracts'
import { MemberFavoriteApi } from '/@/api/client/MemberFavorite'
import eventBus from '/@/utils/mitt'
import { getCurrentInstance, reactive, ref, onMounted, defineAsyncComponent, useTemplateRef } from 'vue'

// 引入组件
const MemberFavoriteForm = defineAsyncComponent(() => import('./components/member-favorite-form.vue'))

const { proxy } = getCurrentInstance() as any

const memberFavoriteFormRef = useTemplateRef('memberFavoriteFormRef')

const state = reactive({
  loading: false,
  memberFavoriteFormTitle: '',
  currentPage: 1,
  pageSize: 20,
  total: 0,
  filterModel: {
    favoriteType: '',
    favoriteName: '',
  } as MemberFavoriteGetPageInput,
  memberFavoriteListData: [] as MemberFavoriteGetPageOutput[],
})

// 获取收藏类型标签类型
const getFavoriteTypeTagType = (type: string) => {
  switch (type) {
    case 'Supplier':
      return 'success'
    case 'Product':
      return 'primary'
    case 'Contact':
      return 'warning'
    default:
      return 'info'
  }
}

// 获取收藏类型文本
const getFavoriteTypeText = (type: string) => {
  switch (type) {
    case 'Supplier':
      return '供应商'
    case 'Product':
      return '产品'
    case 'Contact':
      return '联系人'
    default:
      return type
  }
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return ''
  return new Date(date).toLocaleString('zh-CN')
}

// 查询
const onQuery = async () => {
  state.loading = true
  try {
    const res = await new MemberFavoriteApi().getPage({
      currentPage: state.currentPage,
      pageSize: state.pageSize,
      filter: state.filterModel,
    })
    
    if (res?.success) {
      state.memberFavoriteListData = res.data?.list || []
      state.total = res.data?.total || 0
    }
  } catch (error) {
    console.error('查询会员收藏失败:', error)
  } finally {
    state.loading = false
  }
}

// 新增
const onAdd = () => {
  state.memberFavoriteFormTitle = '新增会员收藏'
  memberFavoriteFormRef.value?.open()
}

// 编辑
const onEdit = (row: MemberFavoriteGetPageOutput) => {
  state.memberFavoriteFormTitle = '编辑会员收藏'
  memberFavoriteFormRef.value?.open(row)
}

// 删除
const onDelete = (row: MemberFavoriteGetPageOutput) => {
  proxy.$modal.confirm('确定要删除这条收藏记录吗？').then(async () => {
    try {
      const res = await new MemberFavoriteApi().delete({ id: row.id })
      if (res?.success) {
        proxy.$modal.msgSuccess('删除成功')
        onQuery()
      }
    } catch (error) {
      console.error('删除会员收藏失败:', error)
    }
  })
}

// 分页大小改变
const onSizeChange = (val: number) => {
  state.pageSize = val
  onQuery()
}

// 当前页改变
const onCurrentChange = (val: number) => {
  state.currentPage = val
  onQuery()
}

// 监听表单提交事件
eventBus.on('refreshMemberFavoriteList', () => {
  onQuery()
})

// 页面加载时查询
onMounted(() => {
  onQuery()
})

// 页面卸载时移除事件监听
onUnmounted(() => {
  eventBus.off('refreshMemberFavoriteList')
})
</script>
