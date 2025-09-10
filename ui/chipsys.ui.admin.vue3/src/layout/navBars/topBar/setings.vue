<template>
  <div class="layout-breadcrumb-seting">
    <el-drawer
      :title="$t('message.layout.configTitle')"
      v-model="getThemeConfig.isDrawer"
      direction="rtl"
      destroy-on-close
      size="285px"
      resizable
      @close="onDrawerClose"
    >
      <el-scrollbar class="layout-breadcrumb-seting-bar">
        <!-- 全局主题 -->
        <el-divider content-position="left">{{ $t('message.layout.oneTitle') }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.primary') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker v-model="getThemeConfig.primary" :predefine="predefinePrimaryColors" @change="onColorPickerChange"> </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsDark') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isDark" @change="onAddDarkChange"></el-switch>
          </div>
        </div>

        <!-- 顶栏设置 -->
        <el-divider content-position="left">{{ $t('message.layout.twoTopTitle') }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoTopBar') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker v-model="getThemeConfig.topBar" :predefine="predefineTopBarBgColors" @change="onBgColorPickerChange('topBar')">
            </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoTopBarColor') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker v-model="getThemeConfig.topBarColor" :predefine="predefineFontColors" @change="onBgColorPickerChange('topBarColor')">
            </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt10">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoIsTopBarColorGradual') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isTopBarColorGradual" @change="onTopBarGradualChange"></el-switch>
          </div>
        </div>

        <!-- 菜单设置 -->
        <el-divider content-position="left">{{ $t('message.layout.twoMenuTitle') }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoMenuBar') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker v-model="getThemeConfig.menuBar" :predefine="predefineMenuBarBgColors" @change="onBgColorPickerChange('menuBar')">
            </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoMenuBarColor') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker v-model="getThemeConfig.menuBarColor" :predefine="predefineFontColors" @change="onBgColorPickerChange('menuBarColor')">
            </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoMenuBarActiveColor') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker
              v-model="getThemeConfig.menuBarActiveColor"
              show-alpha
              :predefine="predefineActiveBgColors"
              @change="onBgColorPickerChange('menuBarActiveColor')"
            />
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt14">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoIsMenuBarColorGradual') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isMenuBarColorGradual" @change="onMenuBarGradualChange"></el-switch>
          </div>
        </div>

        <!-- 分栏设置 -->
        <el-divider content-position="left" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">{{
          $t('message.layout.twoColumnsTitle')
        }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoColumnsMenuBar') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker
              v-model="getThemeConfig.columnsMenuBar"
              :predefine="predefineColumnsMenuBarBgColors"
              @change="onBgColorPickerChange('columnsMenuBar')"
              :disabled="getThemeConfig.layout !== 'columns'"
            >
            </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoColumnsMenuBarColor') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-color-picker
              v-model="getThemeConfig.columnsMenuBarColor"
              :predefine="predefineFontColors"
              @change="onBgColorPickerChange('columnsMenuBarColor')"
              :disabled="getThemeConfig.layout !== 'columns'"
            >
            </el-color-picker>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt14" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoIsColumnsMenuBarColorGradual') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch
              v-model="getThemeConfig.isColumnsMenuBarColorGradual"
              @change="onColumnsMenuBarGradualChange"
              :disabled="getThemeConfig.layout !== 'columns'"
            ></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt14" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.twoIsColumnsMenuHoverPreload') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch
              v-model="getThemeConfig.isColumnsMenuHoverPreload"
              @change="onColumnsMenuHoverPreloadChange"
              :disabled="getThemeConfig.layout !== 'columns'"
            ></el-switch>
          </div>
        </div>

        <!-- 界面设置 -->
        <el-divider content-position="left">{{ $t('message.layout.threeTitle') }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex" :style="{ opacity: getThemeConfig.layout === 'transverse' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.threeIsCollapse') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch
              v-model="getThemeConfig.isCollapse"
              :disabled="getThemeConfig.layout === 'transverse'"
              @change="onThemeConfigChange"
            ></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15" :style="{ opacity: getThemeConfig.layout === 'transverse' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.threeIsUniqueOpened') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch
              v-model="getThemeConfig.isUniqueOpened"
              :disabled="getThemeConfig.layout === 'transverse'"
              @change="setLocalThemeConfig"
            ></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.threeIsFixedHeader') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isFixedHeader" @change="onIsFixedHeaderChange"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15" :style="{ opacity: getThemeConfig.layout !== 'classic' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.threeIsClassicSplitMenu') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isClassicSplitMenu" :disabled="getThemeConfig.layout !== 'classic'" @change="onClassicSplitMenuChange">
            </el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.threeIsLockScreen') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isLockScreen" @change="setLocalThemeConfig"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt11">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.threeLockScreenTime') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-input-number
              v-model="getThemeConfig.lockScreenTime"
              controls-position="right"
              :min="1"
              :max="9999"
              @change="setLocalThemeConfig"
              style="width: 110px"
            >
            </el-input-number>
          </div>
        </div>

        <!-- 界面显示 -->
        <el-divider content-position="left">{{ $t('message.layout.fourTitle') }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsShowLogo') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isShowLogo" @change="onIsShowLogoChange"></el-switch>
          </div>
        </div>
        <div
          class="layout-breadcrumb-seting-bar-flex mt15"
          :style="{ opacity: getThemeConfig.layout === 'classic' || getThemeConfig.layout === 'transverse' ? 0.5 : 1 }"
        >
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsBreadcrumb') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch
              v-model="getThemeConfig.isBreadcrumb"
              :disabled="getThemeConfig.layout === 'classic' || getThemeConfig.layout === 'transverse'"
              @change="onIsBreadcrumbChange"
            ></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsBreadcrumbIcon') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isBreadcrumbIcon" @change="setLocalThemeConfig"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsTagsview') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isTagsview" @change="setLocalThemeConfig"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsTagsviewIcon') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isTagsviewIcon" @change="setLocalThemeConfig"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsCacheTagsView') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isCacheTagsView" @change="setLocalThemeConfig"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15" :style="{ opacity: state.isMobile ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsSortableTagsView') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch
              v-model="getThemeConfig.isSortableTagsView"
              :disabled="state.isMobile ? true : false"
              @change="onSortableTagsViewChange"
            ></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsShareTagsView') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isShareTagsView" @change="onShareTagsViewChange"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsFooter') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isFooter" @change="setLocalThemeConfig"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsGrayscale') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isGrayscale" @change="onAddFilterChange('grayscale')"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsInvert') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isInvert" @change="onAddFilterChange('invert')"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourIsWatermark') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-switch v-model="getThemeConfig.isWatermark" @change="onWatermarkChange"></el-switch>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt14">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fourWatermarkText') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-input v-model="getThemeConfig.watermarkText" style="width: 110px" @input="onWatermarkTextInput"></el-input>
          </div>
        </div>

        <!-- 其它设置 -->
        <el-divider content-position="left">{{ $t('message.layout.fiveTitle') }}</el-divider>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fiveTagsStyle') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-select v-model="getThemeConfig.tagsStyle" placeholder="请选择" style="width: 110px" @change="setLocalThemeConfig">
              <el-option label="卡片" value="tags-style-one"></el-option>
              <el-option label="简�? value="tags-style-four"></el-option>
              <el-option label="圆滑" value="tags-style-five"></el-option>
            </el-select>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fiveAnimation') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-select
              v-model="getThemeConfig.animation"
              placeholder="请选择"
              placement="bottom-end"
              style="width: 110px"
              @change="setLocalThemeConfig"
            >
              <el-option label="右滑�? value="slide-right"></el-option>
              <el-option label="左滑�? value="slide-left"></el-option>
              <el-option label="淡入淡出" value="opacitys"></el-option>
            </el-select>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fiveColumnsAsideStyle') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-select
              v-model="getThemeConfig.columnsAsideStyle"
              placeholder="请选择"
              style="width: 110px"
              :disabled="getThemeConfig.layout !== 'columns' ? true : false"
              @change="setLocalThemeConfig"
            >
              <el-option label="圆角" value="columns-round"></el-option>
              <el-option label="卡片" value="columns-card"></el-option>
            </el-select>
          </div>
        </div>
        <div class="layout-breadcrumb-seting-bar-flex mt15 mb27" :style="{ opacity: getThemeConfig.layout !== 'columns' ? 0.5 : 1 }">
          <div class="layout-breadcrumb-seting-bar-flex-label">{{ $t('message.layout.fiveColumnsAsideLayout') }}</div>
          <div class="layout-breadcrumb-seting-bar-flex-value">
            <el-select
              v-model="getThemeConfig.columnsAsideLayout"
              placeholder="请选择"
              style="width: 110px"
              :disabled="getThemeConfig.layout !== 'columns' ? true : false"
              @change="setLocalThemeConfig"
            >
              <el-option label="水平" value="columns-horizontal"></el-option>
              <el-option label="垂直" value="columns-vertical"></el-option>
            </el-select>
          </div>
        </div>

        <!-- 布局切换 -->
        <el-divider content-position="left">{{ $t('message.layout.sixTitle') }}</el-divider>
        <div class="layout-drawer-content-flex">
          <!-- defaults 布局 -->
          <div class="layout-drawer-content-item" @click="onSetLayout('defaults')">
            <section class="el-container el-circular" :class="{ 'drawer-layout-active': getThemeConfig.layout === 'defaults' }">
              <aside class="el-aside" style="width: 20px"></aside>
              <section class="el-container is-vertical">
                <header class="el-header" style="height: 10px"></header>
                <main class="el-main"></main>
              </section>
            </section>
            <div class="layout-tips-warp" :class="{ 'layout-tips-warp-active': getThemeConfig.layout === 'defaults' }">
              <div class="layout-tips-box">
                <p class="layout-tips-txt">{{ $t('message.layout.sixDefaults') }}</p>
              </div>
            </div>
          </div>
          <!-- classic 布局 -->
          <div class="layout-drawer-content-item" @click="onSetLayout('classic')">
            <section class="el-container is-vertical el-circular" :class="{ 'drawer-layout-active': getThemeConfig.layout === 'classic' }">
              <header class="el-header" style="height: 10px"></header>
              <section class="el-container">
                <aside class="el-aside" style="width: 20px"></aside>
                <section class="el-container is-vertical">
                  <main class="el-main"></main>
                </section>
              </section>
            </section>
            <div class="layout-tips-warp" :class="{ 'layout-tips-warp-active': getThemeConfig.layout === 'classic' }">
              <div class="layout-tips-box">
                <p class="layout-tips-txt">{{ $t('message.layout.sixClassic') }}</p>
              </div>
            </div>
          </div>
          <!-- transverse 布局 -->
          <div class="layout-drawer-content-item" @click="onSetLayout('transverse')">
            <section class="el-container is-vertical el-circular" :class="{ 'drawer-layout-active': getThemeConfig.layout === 'transverse' }">
              <header class="el-header" style="height: 10px"></header>
              <section class="el-container">
                <section class="el-container is-vertical">
                  <main class="el-main"></main>
                </section>
              </section>
            </section>
            <div class="layout-tips-warp" :class="{ 'layout-tips-warp-active': getThemeConfig.layout === 'transverse' }">
              <div class="layout-tips-box">
                <p class="layout-tips-txt">{{ $t('message.layout.sixTransverse') }}</p>
              </div>
            </div>
          </div>
          <!-- columns 布局 -->
          <div class="layout-drawer-content-item" @click="onSetLayout('columns')">
            <section class="el-container el-circular" :class="{ 'drawer-layout-active': getThemeConfig.layout === 'columns' }">
              <aside class="el-aside-dark" style="width: 10px"></aside>
              <aside class="el-aside" style="width: 20px"></aside>
              <section class="el-container is-vertical">
                <header class="el-header" style="height: 10px"></header>
                <main class="el-main"></main>
              </section>
            </section>
            <div class="layout-tips-warp" :class="{ 'layout-tips-warp-active': getThemeConfig.layout === 'columns' }">
              <div class="layout-tips-box">
                <p class="layout-tips-txt">{{ $t('message.layout.sixColumns') }}</p>
              </div>
            </div>
          </div>
        </div>
        <div class="copy-config">
          <el-alert :title="$t('message.layout.tipText')" type="warning" :closable="false"> </el-alert>
          <el-button class="copy-config-btn" type="primary" ref="copyConfigBtnRef" @click="onCopyConfigClick">
            <el-icon class="mr5">
              <ele-CopyDocument />
            </el-icon>
            {{ $t('message.layout.copyText') }}
          </el-button>
          <el-button class="copy-config-btn-reset" type="info" @click="onResetConfigClick">
            <el-icon class="mr5">
              <ele-RefreshRight />
            </el-icon>
            {{ $t('message.layout.resetText') }}
          </el-button>
        </div>
      </el-scrollbar>
    </el-drawer>
  </div>
</template>

<script setup lang="ts" name="layoutBreadcrumbSeting">
import { ref, nextTick, onUnmounted, onMounted, computed, reactive } from 'vue'
import { ElMessage } from 'element-plus'
import { useI18n } from 'vue-i18n'
import { storeToRefs } from 'pinia'
import { useThemeConfig } from '/@/stores/themeConfig'
import { useChangeColor } from '/@/utils/theme'
import { verifyAndSpace } from '/@/utils/toolsValidate'
import { Local } from '/@/utils/storage'
import Watermark from '/@/utils/watermark'
import commonFunction from '/@/utils/commonFunction'
import other from '/@/utils/other'
import mittBus from '/@/utils/mitt'
import { useUserInfo } from '/@/stores/userInfo'

// 定义变量内容
// 预定义主要颜�?
const predefinePrimaryColors = ref([
  '#F34D37', //红色
  '#409eff', //蓝色
  '#6954f0', //紫色
  '#41b584', //绿色
])
// 预定义顶栏背景颜�?
const predefineTopBarBgColors = ref([
  '#ffffff', //白色
  '#323233', //黑色
])
// 预定义菜单背景颜�?
const predefineMenuBarBgColors = ref([
  '#ffffff', //白色
  '#252526', //黑色
])
// 预定义分栏背景颜�?
const predefineColumnsMenuBarBgColors = ref([
  '#ffffff', //白色
  '#333333', //黑色
])

const grayWhiteColor = '#eaeaea' //灰白�?
const grayBlackColor = '#606266' //灰黑�?
// 预定义字体颜�?
const predefineFontColors = ref([
  grayWhiteColor, //灰白�?
  grayBlackColor, //灰黑�?
])

const { locale } = useI18n()
const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)
const { copyText } = commonFunction()
const { getLightColor, getDarkColor } = useChangeColor()
const state = reactive({
  isMobile: false,
})

const GrayWhiteBgColor = 'rgba(0, 0, 0, 0.2)' //浅灰黑色
// 预定义高亮背景颜�?
const predefineActiveBgColors = ref([
  GrayWhiteBgColor, //浅灰黑色
  getLightColor(predefinePrimaryColors.value[0], 9 / 10), //浅红�?
  getLightColor(predefinePrimaryColors.value[1], 9 / 10), //浅蓝�?
  getLightColor(predefinePrimaryColors.value[2], 9 / 10), //浅紫�?
  getLightColor(predefinePrimaryColors.value[3], 9 / 10), //浅绿�?
])

// 获取布局配置信息
const getThemeConfig = computed(() => {
  return themeConfig.value
})
// 1、全局主题
const onColorPickerChange = () => {
  if (!getThemeConfig.value.primary) return ElMessage.warning('全局主题 primary 颜色值不能为�?)
  document.documentElement.style.setProperty('--el-color-primary', getThemeConfig.value.primary)
  if (getThemeConfig.value.isDark) {
    // 颜色加深
    for (let i = 1; i <= 9; i++) {
      document.documentElement.style.setProperty(`--el-color-primary-light-${i}`, `${getDarkColor(getThemeConfig.value.primary, i / 10)}`)
    }
  } else {
    // 颜色变浅
    for (let i = 1; i <= 9; i++) {
      document.documentElement.style.setProperty(`--el-color-primary-light-${i}`, `${getLightColor(getThemeConfig.value.primary, i / 10)}`)
    }
  }
  // 颜色加深
  document.documentElement.style.setProperty('--el-color-primary-dark-2', `${getDarkColor(getThemeConfig.value.primary, 0.1)}`)
  setDispatchThemeConfig()

  onBgColorPickerChange('menuBar')
}
// 2、菜�?/ 顶栏
const onBgColorPickerChange = (bg: string) => {
  const bgColor = themeConfig.value[bg]
  document.documentElement.style.setProperty(`--next-bg-${bg}`, bgColor)
  if (bg === 'menuBar') {
    document.documentElement.style.setProperty(`--next-bg-menuBar-light-1`, getLightColor(getThemeConfig.value.menuBar, 0.05))
  }
  onTopBarGradualChange()
  onMenuBarGradualChange()
  onColumnsMenuBarGradualChange()
  setDispatchThemeConfig()

  if (bg === 'topBar' || bg === 'menuBar' || bg === 'columnsMenuBar') {
    const whiteTheme = ['#FFFFFF', '#FFF', '#fff', '#ffffff']
    const colorName = bg + 'Color'
    if (whiteTheme.includes(bgColor)) {
      if (bg === 'menuBar') {
        const activeColorName = bg + 'ActiveColor'
        getThemeConfig.value[activeColorName] = getLightColor(getThemeConfig.value.primary, 9 / 10)
        onBgColorPickerChange(activeColorName)
      }
      getThemeConfig.value[colorName] = grayBlackColor
    } else {
      if (bg === 'menuBar') {
        const activeColorName = bg + 'ActiveColor'
        getThemeConfig.value[activeColorName] = GrayWhiteBgColor
        onBgColorPickerChange(activeColorName)
      }
      getThemeConfig.value[colorName] = grayWhiteColor
    }
    onBgColorPickerChange(colorName)
  }
}
// 设置激活颜�?
const onActiveColorPickerChange = (name: string) => {
  document.documentElement.style.setProperty(`--next-color-${name}`, themeConfig.value[name])
}
// 2、菜�?/ 顶栏 --> 顶栏背景渐变
const onTopBarGradualChange = () => {
  setGraduaFun('.layout-navbars-breadcrumb-index', getThemeConfig.value.isTopBarColorGradual, getThemeConfig.value.topBar)
}
// 2、菜�?/ 顶栏 --> 菜单背景渐变
const onMenuBarGradualChange = () => {
  setGraduaFun('.layout-container .el-aside', getThemeConfig.value.isMenuBarColorGradual, getThemeConfig.value.menuBar)
}
// 2、菜�?/ 顶栏 --> 分栏菜单背景渐变
const onColumnsMenuBarGradualChange = () => {
  setGraduaFun('.layout-container .layout-columns-aside', getThemeConfig.value.isColumnsMenuBarColorGradual, getThemeConfig.value.columnsMenuBar)
}
// 2、菜�?/ 顶栏 --> 背景渐变函数
const setGraduaFun = (el: string, bool: boolean, color: string) => {
  nextTick(() => {
    setTimeout(() => {
      let els = document.querySelector(el)
      if (!els) return false
      document.documentElement.style.setProperty('--el-menu-bg-color', document.documentElement.style.getPropertyValue('--next-bg-menuBar'))
      if (bool) els.setAttribute('style', `background:linear-gradient(to bottom , ${color}, ${getLightColor(color, 0.5)})`)
      else els.setAttribute('style', ``)
      setLocalThemeConfig()
    }, 300)
  })
}
// 2、分栏设�?->
const onColumnsMenuHoverPreloadChange = () => {
  setLocalThemeConfig()
}
// 3、界面设�?--> 菜单水平折叠
const onThemeConfigChange = () => {
  setDispatchThemeConfig()
}
// 3、界面设�?--> 固定 Header
const onIsFixedHeaderChange = () => {
  getThemeConfig.value.isFixedHeaderChange = getThemeConfig.value.isFixedHeader ? false : true
  setLocalThemeConfig()
}
// 3、界面设�?--> 经典布局分割菜单
const onClassicSplitMenuChange = () => {
  // getThemeConfig.value.isBreadcrumb = false
  setLocalThemeConfig()
  mittBus.emit('getBreadcrumbIndexSetFilterRoutes')
}
// 4、界面显�?--> 侧边�?Logo
const onIsShowLogoChange = () => {
  getThemeConfig.value.isShowLogoChange = getThemeConfig.value.isShowLogo ? false : true
  setLocalThemeConfig()
}
// 4、界面显�?--> 面包�?Breadcrumb
const onIsBreadcrumbChange = () => {
  if (getThemeConfig.value.layout === 'classic') {
    getThemeConfig.value.isClassicSplitMenu = false
  }
  setLocalThemeConfig()
}
// 4、界面显�?--> 开�?TagsView 拖拽
const onSortableTagsViewChange = () => {
  mittBus.emit('openOrCloseSortable')
  setLocalThemeConfig()
}
// 4、界面显�?--> 开�?TagsView 共用
const onShareTagsViewChange = () => {
  mittBus.emit('openShareTagsView')
  setLocalThemeConfig()
}
// 4、界面显�?--> 灰色模式/色弱模式
const onAddFilterChange = (attr: string) => {
  if (attr === 'grayscale') {
    if (getThemeConfig.value.isGrayscale) getThemeConfig.value.isInvert = false
  } else {
    if (getThemeConfig.value.isInvert) getThemeConfig.value.isGrayscale = false
  }
  const cssAttr =
    attr === 'grayscale' ? `grayscale(${getThemeConfig.value.isGrayscale ? 1 : 0})` : `invert(${getThemeConfig.value.isInvert ? '80%' : '0%'})`
  const appEle = document.body
  appEle.setAttribute('style', `filter: ${cssAttr}`)
  setLocalThemeConfig()
}
// 4、界面显�?--> 深色模式
const onAddDarkChange = () => {
  const html = document.documentElement as HTMLElement
  if (getThemeConfig.value.isDark) {
    html.setAttribute('class', 'dark')
    html.setAttribute('data-theme', 'dark')
  } else {
    html.setAttribute('class', '')
    html.setAttribute('data-theme', '')
  }
  onColorPickerChange()
}
// 4、界面显�?--> 开启水�?
const onWatermarkChange = () => {
  getThemeConfig.value.isWatermark ? Watermark.set(getThemeConfig.value.watermarkText) : Watermark.del()
  setLocalThemeConfig()
}
// 4、界面显�?--> 水印文案
const onWatermarkTextInput = (val: string) => {
  getThemeConfig.value.watermarkText = verifyAndSpace(val)
  if (getThemeConfig.value.watermarkText === '') return false
  if (getThemeConfig.value.isWatermark) Watermark.set(getThemeConfig.value.watermarkText)
  setLocalThemeConfig()
}
// 5、布局切换
const onSetLayout = (layout: string) => {
  Local.set('oldLayout', layout)
  if (getThemeConfig.value.layout === layout) return false
  if (layout === 'transverse') getThemeConfig.value.isCollapse = false
  getThemeConfig.value.layout = layout
  getThemeConfig.value.isDrawer = false
  initLayoutChangeFun()
}
// 设置布局切换函数
const initLayoutChangeFun = () => {
  onBgColorPickerChange('menuBar')
  onBgColorPickerChange('menuBarColor')
  onBgColorPickerChange('menuBarActiveColor')
  onBgColorPickerChange('topBar')
  onBgColorPickerChange('topBarColor')
  onBgColorPickerChange('columnsMenuBar')
  onBgColorPickerChange('columnsMenuBarColor')
  onActiveColorPickerChange('columnsMenuBarActiveColor')
}
// 关闭弹窗时，初始化变量。变量用于处�?layoutScrollbarRef.value.update() 更新滚动条高�?
const onDrawerClose = () => {
  getThemeConfig.value.isFixedHeaderChange = false
  getThemeConfig.value.isShowLogoChange = false
  getThemeConfig.value.isDrawer = false
  setLocalThemeConfig()
}
// 布局配置弹窗打开
const openDrawer = () => {
  getThemeConfig.value.isDrawer = true
}
// 触发 store 布局配置更新
const setDispatchThemeConfig = () => {
  setLocalThemeConfig()
  setLocalThemeConfigStyle()
}
// 存储布局配置
const setLocalThemeConfig = () => {
  Local.remove('themeConfig')
  Local.set('themeConfig', getThemeConfig.value)
}
// 存储布局配置全局主题样式（html根标签）
const setLocalThemeConfigStyle = () => {
  Local.set('themeConfigStyle', document.documentElement.style.cssText)
}
// 一键复制配�?
const onCopyConfigClick = () => {
  let copyThemeConfig = Local.get('themeConfig')
  copyThemeConfig.isDrawer = false
  copyText(JSON.stringify(copyThemeConfig)).then(() => {
    getThemeConfig.value.isDrawer = false
  })
}
// 一键恢复默�?
const onResetConfigClick = () => {
  const storesUseUserInfo = useUserInfo()
  const tokenInfo = storesUseUserInfo.getTokenInfo()
  Local.clear()
  storesUseUserInfo.setTokenInfo(tokenInfo)
  window.location.reload()
  // @ts-ignore
  Local.set('version', __NEXT_VERSION__)
}
// 初始化菜单样式等
const initSetStyle = () => {
  // 2、菜�?/ 顶栏 --> 顶栏背景渐变
  onTopBarGradualChange()
  // 2、菜�?/ 顶栏 --> 菜单背景渐变
  onMenuBarGradualChange()
  // 2、菜�?/ 顶栏 --> 分栏菜单背景渐变
  onColumnsMenuBarGradualChange()
}
onMounted(() => {
  nextTick(() => {
    // 判断当前布局是否不相同，不相同则初始化当前布局的样式，防止监听窗口大小改变时，布局配置logo、菜单背景等部分布局失效问题
    if (!Local.get('frequency')) initLayoutChangeFun()
    Local.set('frequency', 1)
    // 监听窗口大小改变，非默认布局，设置成默认布局（适配移动端）
    mittBus.on('layoutMobileResize', (res: LayoutMobileResize) => {
      getThemeConfig.value.layout = res.layout
      getThemeConfig.value.isDrawer = false
      initLayoutChangeFun()
      state.isMobile = other.isMobile()
    })
    setTimeout(() => {
      // 默认样式
      onColorPickerChange()
      // 灰色模式
      if (getThemeConfig.value.isGrayscale) onAddFilterChange('grayscale')
      // 色弱模式
      if (getThemeConfig.value.isInvert) onAddFilterChange('invert')
      // 深色模式
      if (getThemeConfig.value.isDark) onAddDarkChange()
      // 开启水�?
      onWatermarkChange()
      // 语言国际�?
      if (Local.get('themeConfig')) locale.value = Local.get('themeConfig').globalI18n
      // 初始化菜单样式等
      initSetStyle()
    }, 100)
  })
})
onUnmounted(() => {
  mittBus.off('layoutMobileResize', () => {})
})

// 暴露变量
defineExpose({
  openDrawer,
})
</script>

<style scoped lang="scss">
.layout-breadcrumb-seting-bar {
  height: calc(100vh - 50px);
  padding: 0 15px;
  :deep(.el-scrollbar__view) {
    overflow-x: hidden !important;
  }
  .layout-breadcrumb-seting-bar-flex {
    display: flex;
    align-items: center;
    margin-bottom: 5px;
    &-label {
      flex: 1;
      color: var(--el-text-color-primary);
    }
  }
  .layout-drawer-content-flex {
    overflow: hidden;
    display: flex;
    flex-wrap: wrap;
    align-content: flex-start;
    margin: 0 -5px;
    .layout-drawer-content-item {
      width: 50%;
      height: 70px;
      cursor: pointer;
      border: 1px solid transparent;
      position: relative;
      padding: 5px;
      .el-container {
        height: 100%;
        .el-aside-dark {
          background-color: var(--next-color-seting-header);
        }
        .el-aside {
          background-color: var(--next-color-seting-aside);
        }
        .el-header {
          background-color: var(--next-color-seting-header);
        }
        .el-main {
          background-color: var(--next-color-seting-main);
        }
      }
      .el-circular {
        border-radius: 2px;
        overflow: hidden;
        border: 1px solid transparent;
        transition: all 0.3s ease-in-out;
      }
      .drawer-layout-active {
        border: 1px solid;
        border-color: var(--el-color-primary);
      }
      .layout-tips-warp,
      .layout-tips-warp-active {
        transition: all 0.3s ease-in-out;
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        border: 1px solid;
        border-color: var(--el-color-primary-light-5);
        border-radius: 100%;
        padding: 4px;
        .layout-tips-box {
          transition: inherit;
          width: 30px;
          height: 30px;
          z-index: 9;
          border: 1px solid;
          border-color: var(--el-color-primary-light-5);
          border-radius: 100%;
          .layout-tips-txt {
            transition: inherit;
            position: relative;
            top: 5px;
            font-size: 12px;
            line-height: 1;
            letter-spacing: 2px;
            white-space: nowrap;
            color: var(--el-color-primary-light-5);
            text-align: center;
            transform: rotate(30deg);
            left: -1px;
            background-color: var(--next-color-seting-main);
            width: 32px;
            height: 17px;
            line-height: 17px;
          }
        }
      }
      .layout-tips-warp-active {
        border: 1px solid;
        border-color: var(--el-color-primary);
        .layout-tips-box {
          border: 1px solid;
          border-color: var(--el-color-primary);
          .layout-tips-txt {
            color: var(--el-color-primary) !important;
            background-color: var(--next-color-seting-main) !important;
          }
        }
      }
      &:hover {
        .el-circular {
          transition: all 0.3s ease-in-out;
          border: 1px solid;
          border-color: var(--el-color-primary);
        }
        .layout-tips-warp {
          transition: all 0.3s ease-in-out;
          border-color: var(--el-color-primary);
          .layout-tips-box {
            transition: inherit;
            border-color: var(--el-color-primary);
            .layout-tips-txt {
              transition: inherit;
              color: var(--el-color-primary) !important;
              background-color: var(--next-color-seting-main) !important;
            }
          }
        }
      }
    }
  }
  .copy-config {
    margin: 10px 0;
    .copy-config-btn {
      width: 100%;
      margin-top: 15px;
    }
    .copy-config-btn-reset {
      width: 100%;
      margin: 10px 0 0;
    }
  }
}
</style>
