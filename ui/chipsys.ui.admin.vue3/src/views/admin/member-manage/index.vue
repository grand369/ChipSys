<template>
  <div class="member-manage-container">
    <el-row :gutter="16">
      <!-- 左侧：会员等级列表 -->
      <el-col :xs="24" :sm="8" :md="6" :lg="6" :xl="6">
        <el-card shadow="never" class="h100">
          <template #header>
            <div class="card-header">
              <span>会员等级</span>
              <el-button type="primary" size="small" @click="openAddLevel">新增等级</el-button>
            </div>
          </template>

          <el-menu :default-active="state.activeLevel" @select="handleLevelSelect">
            <el-menu-item index="all">
              全部
              <el-tag class="ml8" size="small" type="info">{{ levelTotal }}</el-tag>
            </el-menu-item>
            <el-menu-item v-for="lvl in state.levels" :key="lvl.level" :index="lvl.level">
              <div class="level-item">
                <div class="level-text">
                  <span>{{ renderLevelText(lvl.level) }}</span>
                  <el-tag class="ml8" size="small">{{ lvl.count }}</el-tag>
                </div>
                <div class="level-actions" @click.stop>
                  <el-button text type="primary" size="small" @click="openEditLevel(lvl)">编辑</el-button>
                  <el-button text type="danger" size="small" @click="deleteLevel(lvl)">删除</el-button>
                </div>
              </div>
            </el-menu-item>
          </el-menu>
        </el-card>
      </el-col>

      <!-- 右侧：会员列表 + 操作按钮 -->
      <el-col :xs="24" :sm="16" :md="18" :lg="18" :xl="18">
        <el-card shadow="never" class="h100">
          <template #header>
            <div class="card-header">
              <div>
                <span>会员管理</span>
              </div>
              <div>
                <el-input v-model="state.keyword" placeholder="请输入关键词" class="w200 mr8" clearable @keyup.enter.native="refresh" />
                <el-button type="primary" @click="openAdd">新增会员</el-button>
              </div>
            </div>
          </template>

          <el-table :data="state.tableData.list" border v-loading="state.loading">
            <el-table-column prop="id" label="ID" width="90" />
            <el-table-column prop="nickName" label="昵称" min-width="140" />
            <el-table-column prop="realName" label="姓名" width="140" />
            <el-table-column prop="mobile" label="手机号" width="150" />
            <el-table-column prop="email" label="邮箱" min-width="180" />
            <el-table-column prop="level" label="等级" width="120" />
            <el-table-column label="状态" width="120">
              <template #default="{ row }">
                <el-tag :type="row.enabled ? 'success' : 'danger'">{{ row.enabled ? '启用' : '禁用' }}</el-tag>
              </template>
            </el-table-column>
            <el-table-column prop="createdTime" label="创建时间" width="180" />
            <el-table-column label="操作" width="280" fixed="right">
              <template #default="{ row }">
                <el-button type="primary" text size="small" @click="openEdit(row)">编辑</el-button>
                <el-button :type="row.enabled ? 'danger' : 'success'" text size="small" @click="toggleEnabled(row)">{{ row.enabled ? '禁用' : '启用' }}</el-button>
                <el-divider direction="vertical" />
                <el-dropdown>
                  <span class="el-dropdown-link">
                    调整会员等级<el-icon class="el-icon--right"><arrow-down /></el-icon>
                  </span>
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item @click="adjustLevel(row, 'Free')">免费</el-dropdown-item>
                      <el-dropdown-item @click="adjustLevel(row, 'Basic')">基础版</el-dropdown-item>
                      <el-dropdown-item @click="adjustLevel(row, 'Standard')">标准版</el-dropdown-item>
                      <el-dropdown-item @click="adjustLevel(row, 'Premium')">高级版</el-dropdown-item>
                      <el-dropdown-item @click="adjustLevel(row, 'Enterprise')">企业版</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </el-dropdown>
              </template>
            </el-table-column>
          </el-table>

          <div class="pagination-container">
            <el-pagination
              v-model:current-page="state.currentPage"
              v-model:page-size="state.pageSize"
              :total="state.tableData.total"
              :page-sizes="[10, 20, 50, 100]"
              layout="total, sizes, prev, pager, next, jumper"
              @size-change="handleSizeChange"
              @current-change="handleCurrentChange"
            />
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 新增/编辑对话框（不含等级） -->
    <el-dialog v-model="state.dialog.visible" :title="state.dialog.type === 'add' ? '新增会员' : '编辑会员'" width="520px" destroy-on-close>
      <el-form :model="state.dialog.form" label-width="90px">
        <el-form-item label="昵称">
          <el-input v-model="state.dialog.form.nickName" placeholder="请输入昵称" />
        </el-form-item>
        <el-form-item label="姓名">
          <el-input v-model="state.dialog.form.realName" placeholder="请输入姓名" />
        </el-form-item>
        <el-form-item label="手机号">
          <el-input v-model="state.dialog.form.mobile" placeholder="请输入手机号" />
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="state.dialog.form.email" placeholder="请输入邮箱" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="state.dialog.visible = false">取消</el-button>
        <el-button type="primary" @click="submitDialog">确定</el-button>
      </template>
    </el-dialog>
    <!-- 新增/编辑会员等级对话框 -->
    <el-dialog v-model="state.levelDialog.visible" :title="state.levelDialog.type === 'add' ? '新增会员等级' : '编辑会员等级'" width="560px" destroy-on-close>
      <el-form :model="state.levelDialog.form" label-width="120px">
        <el-form-item label="等级标识">
          <el-input v-model="state.levelDialog.form.level" placeholder="如：Free/Basic/Standard/Premium/Enterprise" />
        </el-form-item>
        <el-form-item label="分类限制">
          <el-input-number v-model="state.levelDialog.form.categoryLimit" :min="0" />
        </el-form-item>
        <el-form-item label="产品数据量">
          <el-input-number v-model="state.levelDialog.form.productDataLimit" :min="0" />
        </el-form-item>
        <el-form-item label="供应商数据量">
          <el-input-number v-model="state.levelDialog.form.supplierDataLimit" :min="0" />
        </el-form-item>
        <el-form-item label="生效时间">
          <el-date-picker
            v-model="state.levelDialog.form.effectiveTime"
            type="datetime"
            placeholder="请选择生效时间"
            value-format="YYYY-MM-DD HH:mm:ss"
            :default-time="new Date()"
          />
        </el-form-item>
        <el-form-item label="过期时间">
          <el-date-picker
            v-model="state.levelDialog.form.expireTime"
            type="datetime"
            placeholder="请选择过期时间"
            value-format="YYYY-MM-DD HH:mm:ss"
            :default-time="new Date(new Date().setHours(23,59,59,0))"
          />
        </el-form-item>
        <el-form-item label="显示联系人明文">
          <el-switch v-model="state.levelDialog.form.showFullContactInfo" />
        </el-form-item>
        <el-form-item label="启用">
          <el-switch v-model="state.levelDialog.form.enabled" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="state.levelDialog.visible = false">取消</el-button>
        <el-button type="primary" @click="submitLevelDialog">确定</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted, computed } from 'vue'
import { MemberLevelManageApi } from '/@/api/admin/MemberLevelManage'
import { ElMessage, ElMessageBox } from 'element-plus'
import { ArrowDown } from '@element-plus/icons-vue'
import { memberManageApi } from '/@/api/admin/MemberManage'

const state = reactive({
  loading: false,
  levels: [] as { level: string; count: number }[],
  activeLevel: 'all',
  keyword: '',
  currentPage: 1,
  pageSize: 10,
  tableData: { list: [], total: 0 } as { list: any[]; total: number },
  dialog: {
    visible: false,
    type: 'add' as 'add' | 'edit',
    form: { id: 0, nickName: '', realName: '', mobile: '', email: '' },
  },
})

state.levelDialog = {
  visible: false,
  type: 'add',
  form: { id: 0, level: '', categoryLimit: 0, productDataLimit: 0, supplierDataLimit: 0, showFullContactInfo: false, enabled: true },
} as any

const openAddLevel = () => {
  state.levelDialog.type = 'add'
  state.levelDialog.form = { id: 0, level: '', categoryLimit: 0, productDataLimit: 0, supplierDataLimit: 0, showFullContactInfo: false, enabled: true, effectiveTime: '', expireTime: '' }
  state.levelDialog.visible = true
}

const openEditLevel = (lvl: any) => {
  state.levelDialog.type = 'edit'
  // 先查询该等级的最新一条记录，填充完整信息
  const api = new MemberLevelManageApi()
  api.getPage({ currentPage: 1, pageSize: 1, filter: { level: lvl.level } } as any).then(res => {
    const row = res?.data?.list?.[0]
    if (row) {
      state.levelDialog.form = {
        id: row.id,
        level: row.level,
        categoryLimit: row.categoryLimit ?? 0,
        productDataLimit: row.productDataLimit ?? 0,
        supplierDataLimit: row.supplierDataLimit ?? 0,
        showFullContactInfo: !!row.showFullContactInfo,
        enabled: !!row.enabled,
        effectiveTime: row.effectiveTime || '',
        expireTime: row.expireTime || '',
      }
    } else {
      state.levelDialog.form = { id: 0, level: lvl.level, categoryLimit: 0, productDataLimit: 0, supplierDataLimit: 0, showFullContactInfo: false, enabled: true, effectiveTime: '', expireTime: '' }
    }
    state.levelDialog.visible = true
  })
}

const submitLevelDialog = async () => {
  const api = new MemberLevelManageApi()
  if (state.levelDialog.type === 'add') {
    const res = await api.add({
      level: state.levelDialog.form.level,
      categoryLimit: state.levelDialog.form.categoryLimit,
      productDataLimit: state.levelDialog.form.productDataLimit,
      supplierDataLimit: state.levelDialog.form.supplierDataLimit,
      showFullContactInfo: state.levelDialog.form.showFullContactInfo,
      enabled: state.levelDialog.form.enabled,
      effectiveTime: state.levelDialog.form.effectiveTime,
      expireTime: state.levelDialog.form.expireTime,
      // 不传递MemberId，让后端设置为0（等级模板）
    })
    if (res?.success) {
      ElMessage.success('新增等级成功')
      state.levelDialog.visible = false
      fetchLevels()
    }
  } else {
    const res = await api.update({
      id: state.levelDialog.form.id,
      level: state.levelDialog.form.level,
      categoryLimit: state.levelDialog.form.categoryLimit,
      productDataLimit: state.levelDialog.form.productDataLimit,
      supplierDataLimit: state.levelDialog.form.supplierDataLimit,
      showFullContactInfo: state.levelDialog.form.showFullContactInfo,
      enabled: state.levelDialog.form.enabled,
      effectiveTime: state.levelDialog.form.effectiveTime,
      expireTime: state.levelDialog.form.expireTime,
      // 不传递MemberId，保持原有值
    })
    if (res?.success) {
      ElMessage.success('保存成功')
      state.levelDialog.visible = false
      fetchLevels()
    }
  }
}

const deleteLevel = async (lvl: any) => {
  await ElMessageBox.confirm(`确认删除会员等级【${renderLevelText(lvl.level)}】吗？`, '提示', { type: 'warning' })
  const api = new MemberLevelManageApi()
  // 先查出该等级的一条记录，拿到id再删除
  const pageRes = await api.getPage({ currentPage: 1, pageSize: 1, filter: { level: lvl.level } } as any)
  const id = pageRes?.data?.list?.[0]?.id
  if (!id) {
    ElMessage.error('未找到可删除的等级记录')
    return
  }
  const res = await api.delete(id)
  if (res?.success) {
    ElMessage.success('删除成功')
    fetchLevels()
  }
}

const levelTotal = computed(() => state.levels.reduce((s, x) => s + x.count, 0))

const renderLevelText = (level: string) => {
  const map: any = { Free: '免费', Basic: '基础版', Standard: '标准版', Premium: '高级版', Enterprise: '企业版' }
  return map[level] || level
}

const fetchLevels = async () => {
  const res = await memberManageApi.getLevels()
  if (res?.success) state.levels = res.data || []
}

const fetchMembers = async () => {
  state.loading = true
  try {
    const res = await memberManageApi.getMembers({
      currentPage: state.currentPage,
      pageSize: state.pageSize,
      keyword: state.keyword || undefined,
      level: state.activeLevel === 'all' ? undefined : state.activeLevel,
    })
    if (res?.success) state.tableData = res.data
  } finally {
    state.loading = false
  }
}

const handleLevelSelect = (level: string) => {
  state.activeLevel = level
  state.currentPage = 1
  fetchMembers()
}

const handleSizeChange = (val: number) => {
  state.pageSize = val
  fetchMembers()
}

const handleCurrentChange = (val: number) => {
  state.currentPage = val
  fetchMembers()
}

const refresh = () => {
  state.currentPage = 1
  fetchMembers()
}

const openAdd = () => {
  state.dialog.type = 'add'
  state.dialog.form = { id: 0, nickName: '', realName: '', mobile: '', email: '' }
  state.dialog.visible = true
}

const openEdit = (row: any) => {
  state.dialog.type = 'edit'
  state.dialog.form = { id: row.id, nickName: row.nickName, realName: row.realName, mobile: row.mobile, email: row.email }
  state.dialog.visible = true
}

const submitDialog = async () => {
  if (state.dialog.type === 'add') {
    const res = await memberManageApi.add({
      nickName: state.dialog.form.nickName,
      realName: state.dialog.form.realName,
      mobile: state.dialog.form.mobile,
      email: state.dialog.form.email,
    })
    if (res?.success) {
      ElMessage.success('新增成功')
      state.dialog.visible = false
      refresh()
    }
  } else {
    const res = await memberManageApi.update({
      id: state.dialog.form.id,
      nickName: state.dialog.form.nickName,
      realName: state.dialog.form.realName,
      mobile: state.dialog.form.mobile,
      email: state.dialog.form.email,
    })
    if (res?.success) {
      ElMessage.success('保存成功')
      state.dialog.visible = false
      fetchMembers()
    }
  }
}

const toggleEnabled = async (row: any) => {
  await ElMessageBox.confirm(`确认要${row.enabled ? '禁用' : '启用'}该会员吗？`, '提示', { type: 'warning' })
  const res = await memberManageApi.toggleEnabled({ id: row.id, enabled: !row.enabled })
  if (res?.success) {
    ElMessage.success('操作成功')
    fetchMembers()
  }
}

const adjustLevel = async (row: any, level: string) => {
  const res = await memberManageApi.adjustLevel({ memberId: row.id, level })
  if (res?.success) {
    ElMessage.success('等级已调整')
    fetchMembers()
    fetchLevels()
  }
}

onMounted(async () => {
  await fetchLevels()
  await fetchMembers()
})
</script>

<style scoped>
.member-manage-container { height: 100%; }
.h100 { height: 100%; }
.card-header { display: flex; justify-content: space-between; align-items: center; }
.w200 { width: 200px; }
.ml8 { margin-left: 8px; }
.mr8 { margin-right: 8px; }
.pagination-container { margin-top: 12px; display: flex; justify-content: flex-end; }
.level-item { display: flex; align-items: center; justify-content: space-between; width: 100%; }
.level-text { display: flex; align-items: center; }
.level-actions :deep(.el-button) { padding: 0 6px; }
</style>



