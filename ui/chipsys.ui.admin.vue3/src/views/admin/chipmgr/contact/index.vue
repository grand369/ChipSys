<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.name" placeholder="联系人姓名" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="phone">
          <el-input v-model="state.filterModel.phone" placeholder="电话" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="email">
          <el-input v-model="state.filterModel.email" placeholder="邮箱" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item prop="supplierId">
          <el-select v-model="state.filterModel.supplierId" placeholder="供应商" clearable class="w200">
            <el-option
              v-for="item in state.supplierOptions"
              :key="item.id"
              :label="item.name"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:contact:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.contactListData"
        row-key="id"
        border
        style="width: 100%"
      >
        <el-table-column prop="name" label="姓名" min-width="100" show-overflow-tooltip />
        <el-table-column prop="phone" label="电话" min-width="120" show-overflow-tooltip />
        <el-table-column prop="email" label="邮箱" min-width="150" show-overflow-tooltip />
        <el-table-column prop="position" label="职位" min-width="100" show-overflow-tooltip />
        <el-table-column prop="supplierName" label="供应商" min-width="150" show-overflow-tooltip />
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:contact:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:contact:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <contact-form ref="contactFormRef" :title="state.contactFormTitle"></contact-form>
  </div>
</template>

<script lang="ts" setup name="admin/chipmgr/contact">
import { ContactGetListOutput, ContactGetListInput, SupplierGetListOutput } from '/@/api/admin/data-contracts'
import { ContactApi } from '/@/api/admin/Contact'
import { SupplierApi } from '/@/api/admin/Supplier'
import eventBus from '/@/utils/mitt'

// 引入组件
const ContactForm = defineAsyncComponent(() => import('./components/contact-form.vue'))

const { proxy } = getCurrentInstance() as any

const contactFormRef = useTemplateRef('contactFormRef')

const state = reactive({
  loading: false,
  contactFormTitle: '',
  filterModel: {
    name: '',
    phone: '',
    email: '',
    supplierId: undefined as number | undefined,
  } as ContactGetListInput,
  contactListData: [] as Array<ContactGetListOutput>,
  supplierOptions: [] as Array<SupplierGetListOutput>,
})

onMounted(() => {
  onQuery()
  loadSupplierOptions()
  eventBus.off('refreshContact')
  eventBus.on('refreshContact', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshContact')
})

const loadSupplierOptions = async () => {
  const res = await new SupplierApi().getList({}).catch(() => {})
  if (res && res.data) {
    state.supplierOptions = res.data
  }
}

const onQuery = async () => {
  state.loading = true
  const res = await new ContactApi().getList(state.filterModel).catch(() => {
    state.loading = false
  })
  if (res && res.data) {
    state.contactListData = res.data
  } else {
    state.contactListData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.contactFormTitle = '新增联系人'
  contactFormRef.value?.open()
}

const onEdit = (row: ContactGetListOutput) => {
  state.contactFormTitle = '编辑联系人'
  contactFormRef.value?.open(row)
}

const onDelete = (row: ContactGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new ContactApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
