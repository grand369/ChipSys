<template>
  <div>
    <el-dialog v-model="state.showDialog" :title="title" draggable :close-on-click-modal="false" :close-on-press-escape="false" width="600px">
      <div class="mb15">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-input v-model="state.fileDirectory" placeholder="文件目录" clearable />
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-switch v-model="state.fileReName" active-text="文件自动重命�? />
          </el-col>
        </el-row>
        <div class="mt5">
          <el-alert class="my-el-alert" title="文件目录不填则默认使用本地上传格式：upload/yyyy/MM/dd" type="info" :closable="false" />
        </div>
      </div>
      <div>
        <!-- :before-remove="() => false" -->
        <el-upload
          ref="uploadRef"
          :action="uploadAction"
          :headers="uploadHeaders"
          :data="{ fileDirectory: state.fileDirectory, fileReName: state.fileReName }"
          drag
          multiple
          :auto-upload="false"
          :before-upload="
            () => {
              state.token = storesUserInfo.getToken()
            }
          "
          :on-success="onSuccess"
          :on-error="onError"
        >
          <el-icon class="el-icon--upload"><ele-UploadFilled /></el-icon>
          <div class="el-upload__text">拖拽上传�?em>点击上传</em></div>
          <!-- <template #tip>
            <div class="el-upload__tip"></div>
          </template> -->
        </el-upload>
      </div>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onClear">清空已上�?/el-button>
          <el-button @click="onCancel">�?�?/el-button>
          <el-button type="primary" @click="onSure" :loading="state.sureLoading">�?�?/el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/file/upload">
import { ElMessage } from 'element-plus'
import type { UploadInstance, UploadProps, UploadFile } from 'element-plus'
import eventBus from '/@/utils/mitt'
import { useUserInfo } from '/@/stores/userInfo'

const storesUserInfo = useUserInfo()

const uploadRef = useTemplateRef<UploadInstance>('uploadRef')

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const state = reactive({
  showDialog: false,
  sureLoading: false,
  fileDirectory: '',
  fileReName: true,
  fileList: [] as UploadFile[],
  token: storesUserInfo.getToken(),
})

const uploadAction = computed(() => {
  return window.__ENV_CONFIG__.VITE_API_URL + '/api/admin/file/upload-file'
})

const uploadHeaders = computed(() => {
  return { Authorization: 'Bearer ' + state.token }
})

// 打开对话�?
const open = async () => {
  state.showDialog = true
}

//上传失败
const onError: UploadProps['onError'] = (error) => {
  let message = ''
  if (error.message) {
    try {
      message = JSON.parse(error.message)?.msg
    } catch (err) {
      message = error.message || ''
    }
  }
  if (message)
    ElMessage({
      message: message,
      type: 'error',
    })
}

// 上传成功
const onSuccess: UploadProps['onSuccess'] = (response) => {
  if (response?.success) {
    eventBus.emit('refreshFile')
  }
}

// 清空已上�?
const onClear = async () => {
  uploadRef.value!.clearFiles(['success', 'fail'])
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = async () => {
  uploadRef.value!.submit()
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.my-el-alert {
  border: none;
  padding-left: 5px;
  padding-right: 5px;
}
</style>
