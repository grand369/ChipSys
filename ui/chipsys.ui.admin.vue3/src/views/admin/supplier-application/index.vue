<template>
  <div class="supplier-application">
    <div class="page-header">
      <h1>供应商申请审核</h1>
    </div>

    <div class="search-form">
      <el-form :model="searchForm" inline>
        <el-form-item label="申请状态">
          <el-select v-model="searchForm.status" placeholder="请选择状态" clearable>
            <el-option label="待审核" value="pending" />
            <el-option label="已通过" value="approved" />
            <el-option label="已拒绝" value="rejected" />
          </el-select>
        </el-form-item>
        <el-form-item label="公司名称">
          <el-input v-model="searchForm.companyName" placeholder="请输入公司名称" clearable />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleSearch">查询</el-button>
          <el-button @click="handleReset">重置</el-button>
        </el-form-item>
      </el-form>
    </div>

    <div class="table-container">
      <el-table :data="tableData" v-loading="loading">
        <el-table-column prop="id" label="申请ID" width="80" />
        <el-table-column prop="companyName" label="公司名称" min-width="150" />
        <el-table-column prop="contactName" label="联系人" width="100" />
        <el-table-column prop="contactPhone" label="联系电话" width="120" />
        <el-table-column prop="status" label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="getStatusType(row.status)">
              {{ getStatusText(row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdTime" label="申请时间" width="150">
          <template #default="{ row }">
            {{ formatDate(row.createdTime) }}
          </template>
        </el-table-column>
        <el-table-column prop="reviewTime" label="审核时间" width="150">
          <template #default="{ row }">
            {{ row.reviewTime ? formatDate(row.reviewTime) : '-' }}
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button type="primary" size="small" @click="viewDetail(row)">
              查看详情
            </el-button>
            <el-button 
              v-if="row.status === 'pending'"
              type="success" 
              size="small" 
              @click="handleReview(row, 'approved')"
            >
              通过
            </el-button>
            <el-button 
              v-if="row.status === 'pending'"
              type="danger" 
              size="small" 
              @click="handleReview(row, 'rejected')"
            >
              拒绝
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="pagination">
        <el-pagination
          v-model:current-page="pagination.currentPage"
          v-model:page-size="pagination.pageSize"
          :total="pagination.total"
          :page-sizes="[10, 20, 50, 100]"
          layout="total, sizes, prev, pager, next, jumper"
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
        />
      </div>
    </div>

    <!-- 详情对话框 -->
    <el-dialog v-model="detailDialog.visible" title="申请详情" width="800px" destroy-on-close>
      <div v-if="detailDialog.data" class="detail-content">
        <el-descriptions :column="2" border>
          <el-descriptions-item label="申请ID">{{ detailDialog.data.id }}</el-descriptions-item>
          <el-descriptions-item label="申请状态">
            <el-tag :type="getStatusType(detailDialog.data.status)">
              {{ getStatusText(detailDialog.data.status) }}
            </el-tag>
          </el-descriptions-item>
          <el-descriptions-item label="公司名称">{{ detailDialog.data.companyName }}</el-descriptions-item>
          <el-descriptions-item label="公司简称">{{ detailDialog.data.shortName || '-' }}</el-descriptions-item>
          <el-descriptions-item label="统一社会信用代码">{{ detailDialog.data.creditCode || '-' }}</el-descriptions-item>
          <el-descriptions-item label="营业执照号">{{ detailDialog.data.businessLicense || '-' }}</el-descriptions-item>
          <el-descriptions-item label="法定代表人">{{ detailDialog.data.legalPerson || '-' }}</el-descriptions-item>
          <el-descriptions-item label="注册资本">{{ detailDialog.data.registeredCapital || '-' }}</el-descriptions-item>
          <el-descriptions-item label="成立日期">{{ detailDialog.data.establishedDate || '-' }}</el-descriptions-item>
          <el-descriptions-item label="经营范围" :span="2">
            {{ detailDialog.data.businessScope || '-' }}
          </el-descriptions-item>
          <el-descriptions-item label="公司地址" :span="2">
            {{ detailDialog.data.address || '-' }}
          </el-descriptions-item>
          <el-descriptions-item label="联系人姓名">{{ detailDialog.data.contactName }}</el-descriptions-item>
          <el-descriptions-item label="联系人电话">{{ detailDialog.data.contactPhone }}</el-descriptions-item>
          <el-descriptions-item label="联系人邮箱">{{ detailDialog.data.contactEmail || '-' }}</el-descriptions-item>
          <el-descriptions-item label="申请时间">{{ formatDate(detailDialog.data.createdTime) }}</el-descriptions-item>
          <el-descriptions-item label="审核时间">
            {{ detailDialog.data.reviewTime ? formatDate(detailDialog.data.reviewTime) : '-' }}
          </el-descriptions-item>
          <el-descriptions-item v-if="detailDialog.data.reviewComment" label="审核意见" :span="2">
            {{ detailDialog.data.reviewComment }}
          </el-descriptions-item>
        </el-descriptions>

        <div v-if="detailDialog.data.materials && detailDialog.data.materials.length > 0" class="materials">
          <h4>申请材料</h4>
          <div class="material-list">
            <div v-for="(material, index) in detailDialog.data.materials" :key="index" class="material-item">
              <el-link :href="material" target="_blank" type="primary">
                材料 {{ index + 1 }}
              </el-link>
            </div>
          </div>
        </div>
      </div>

      <template #footer>
        <el-button @click="detailDialog.visible = false">关闭</el-button>
        <el-button 
          v-if="detailDialog.data?.status === 'pending'"
          type="success" 
          @click="handleReview(detailDialog.data, 'approved')"
        >
          通过审核
        </el-button>
        <el-button 
          v-if="detailDialog.data?.status === 'pending'"
          type="danger" 
          @click="handleReview(detailDialog.data, 'rejected')"
        >
          拒绝申请
        </el-button>
      </template>
    </el-dialog>

    <!-- 审核对话框 -->
    <el-dialog v-model="reviewDialog.visible" :title="reviewDialog.type === 'approved' ? '通过审核' : '拒绝申请'" width="500px">
      <el-form :model="reviewDialog.form" label-width="100px">
        <el-form-item label="审核意见">
          <el-input
            v-model="reviewDialog.form.reviewComment"
            type="textarea"
            :rows="4"
            :placeholder="reviewDialog.type === 'approved' ? '请输入通过理由（可选）' : '请输入拒绝原因'"
          />
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="reviewDialog.visible = false">取消</el-button>
        <el-button type="primary" @click="submitReview" :loading="reviewDialog.loading">
          确定
        </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { SupplierApplicationManageApi } from '/@/api/admin/SupplierApplicationManage'

const state = reactive({
  tableData: [] as any[],
  loading: false
})

const searchForm = reactive({
  status: '',
  companyName: ''
})

const pagination = reactive({
  currentPage: 1,
  pageSize: 20,
  total: 0
})

const detailDialog = reactive({
  visible: false,
  data: null as any
})

const reviewDialog = reactive({
  visible: false,
  type: 'approved' as 'approved' | 'rejected',
  loading: false,
  form: {
    id: 0,
    reviewComment: ''
  }
})

// 获取状态类型
const getStatusType = (status: string) => {
  const types: Record<string, string> = {
    'pending': 'warning',
    'approved': 'success',
    'rejected': 'danger'
  }
  return types[status] || 'info'
}

// 获取状态文本
const getStatusText = (status: string) => {
  const texts: Record<string, string> = {
    'pending': '待审核',
    'approved': '已通过',
    'rejected': '已拒绝'
  }
  return texts[status] || status
}

// 格式化日期
const formatDate = (date: string) => {
  if (!date) return ''
  return new Date(date).toLocaleString()
}

// 获取列表数据
const fetchData = async () => {
  state.loading = true
  try {
    const api = new SupplierApplicationManageApi()
    const res = await api.getPage({
      currentPage: pagination.currentPage,
      pageSize: pagination.pageSize,
      filter: {
        status: searchForm.status || undefined,
        companyName: searchForm.companyName || undefined
      }
    })

    if (res?.success) {
      state.tableData = res.data?.list || []
      pagination.total = res.data?.total || 0
    }
  } catch (error) {
    ElMessage.error('获取数据失败')
  } finally {
    state.loading = false
  }
}

// 搜索
const handleSearch = () => {
  pagination.currentPage = 1
  fetchData()
}

// 重置
const handleReset = () => {
  searchForm.status = ''
  searchForm.companyName = ''
  pagination.currentPage = 1
  fetchData()
}

// 查看详情
const viewDetail = (row: any) => {
  detailDialog.data = row
  detailDialog.visible = true
}

// 处理审核
const handleReview = (row: any, type: 'approved' | 'rejected') => {
  reviewDialog.type = type
  reviewDialog.form.id = row.id
  reviewDialog.form.reviewComment = ''
  reviewDialog.visible = true
}

// 提交审核
const submitReview = async () => {
  try {
    reviewDialog.loading = true
    
    const api = new SupplierApplicationManageApi()
    const res = await api.review({
      id: reviewDialog.form.id,
      status: reviewDialog.type,
      reviewComment: reviewDialog.form.reviewComment
    })

    if (res?.success) {
      ElMessage.success(reviewDialog.type === 'approved' ? '审核通过' : '审核拒绝')
      reviewDialog.visible = false
      fetchData()
    } else {
      ElMessage.error(res?.message || '操作失败')
    }
  } catch (error) {
    ElMessage.error('操作失败')
  } finally {
    reviewDialog.loading = false
  }
}

// 分页大小改变
const handleSizeChange = (size: number) => {
  pagination.pageSize = size
  pagination.currentPage = 1
  fetchData()
}

// 当前页改变
const handleCurrentChange = (page: number) => {
  pagination.currentPage = page
  fetchData()
}

onMounted(() => {
  fetchData()
})
</script>

<style scoped>
.supplier-application {
  padding: 20px;
}

.page-header {
  margin-bottom: 20px;
}

.page-header h1 {
  font-size: 24px;
  color: #333;
}

.search-form {
  background: #f5f7fa;
  padding: 20px;
  border-radius: 8px;
  margin-bottom: 20px;
}

.table-container {
  background: white;
  border-radius: 8px;
  padding: 20px;
}

.pagination {
  margin-top: 20px;
  text-align: right;
}

.detail-content {
  max-height: 600px;
  overflow-y: auto;
}

.materials {
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid #e4e7ed;
}

.materials h4 {
  margin-bottom: 12px;
  color: #333;
}

.material-list {
  display: flex;
  flex-wrap: wrap;
  gap: 12px;
}

.material-item {
  padding: 8px 12px;
  background: #f5f7fa;
  border-radius: 4px;
  border: 1px solid #e4e7ed;
}
</style>
