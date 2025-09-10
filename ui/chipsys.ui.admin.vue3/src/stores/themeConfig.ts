import { defineStore } from 'pinia'

/**
 * 布局配置
 * 修改配置时：
 * 1、需要每次都清理 `window.localStorage` 浏览器永久缓�?
 * 2、或者点击布局配置最底部 `一键恢复默认` 按钮即可看到效果
 */
export const useThemeConfig = defineStore('themeConfig', {
  state: (): ThemeConfigState => ({
    themeConfig: {
      // 是否开启布局配置抽屉
      isDrawer: false,
      // 是否创建一个h5历史，否则创建一�?hash 历史记录
      isCreateWebHistory: true,

      /**
       * 全局主题
       */
      // 默认 primary 主题颜色 蓝色 #409eff 红色 #F34D37 紫色 #6954f0 绿色 #41b584
      primary: '#6954f0',
      // 是否开启深色模�?
      isDark: true,

      /**
       * 顶栏设置
       */
      // 默认顶栏导航背景颜色 #FFFFFF #323233
      topBar: '#FFFFFF',
      // 默认顶栏导航字体颜色
      topBarColor: '#eaeaea',
      // 是否开启顶栏背景颜色渐�?
      isTopBarColorGradual: false,

      /**
       * 菜单设置
       */
      // 默认菜单导航背景颜色 #FFFFFF #252526
      menuBar: '#FFFFFF',
      // 默认菜单导航字体颜色
      menuBarColor: '#eaeaea',
      // 默认菜单高亮背景�?
      menuBarActiveColor: 'rgba(0, 0, 0, 0.2)',
      // 是否开启菜单背景颜色渐�?
      isMenuBarColorGradual: false,

      /**
       * 分栏设置
       */
      // 默认分栏菜单背景颜色
      columnsMenuBar: '#333333',
      // 默认分栏菜单字体颜色
      columnsMenuBarColor: '#eaeaea',
      // 默认分栏菜单高亮字体颜色
      columnsMenuBarActiveColor: '#ffffff',
      // 是否开启分栏菜单背景颜色渐�?
      isColumnsMenuBarColorGradual: false,
      // 是否开启分栏菜单鼠标悬停预加载(预览菜单)
      isColumnsMenuHoverPreload: false,

      /**
       * 界面设置
       */
      // 是否开启菜单水平折叠效�?
      isCollapse: false,
      // 是否开启菜单手风琴效果
      isUniqueOpened: true,
      // 是否开启固�?Header
      isFixedHeader: true,
      // 初始化变量，用于更新菜单 el-scrollbar 的高度，请勿删除
      isFixedHeaderChange: false,
      // 是否开启经典布局分割菜单（仅经典布局生效�?
      isClassicSplitMenu: true,
      // 是否开启自动锁�?
      isLockScreen: false,
      // 开启自动锁屏倒计�?s/�?
      lockScreenTime: 30,

      /**
       * 界面显示
       */
      // 是否开启侧边栏 Logo
      isShowLogo: true,
      // 初始化变量，用于 el-scrollbar 的高度更新，请勿删除
      isShowLogoChange: false,
      // 是否开�?Breadcrumb，强制经典、横向布局不显�?
      isBreadcrumb: true,
      // 是否开�?Tagsview
      isTagsview: true,
      // 是否开�?Breadcrumb 图标
      isBreadcrumbIcon: false,
      // 是否开�?Tagsview 图标
      isTagsviewIcon: false,
      // 是否开�?TagsView 缓存
      isCacheTagsView: true,
      // 是否开�?TagsView 拖拽
      isSortableTagsView: true,
      // 是否开�?TagsView 共用
      isShareTagsView: false,
      // 是否开�?Footer 底部版权信息
      isFooter: false,
      // 是否开启灰色模�?
      isGrayscale: false,
      // 是否开启色弱模�?
      isInvert: false,
      // 是否开启水�?
      isWatermark: false,
      // 水印文案
      watermarkText: '中台Admin',

      /**
       * 其它设置
       */
      // Tagsview 风格：可选�?<tags-style-one|tags-style-four|tags-style-five>"，默�?tags-style-five
      // 定义的值与 `/src/layout/navBars/tagsView/tagsView.vue` 中的 class 同名
      tagsStyle: 'tags-style-five',
      // 主页面切换动画：可选�?<slide-right|slide-left|opacitys>"，默�?slide-right
      animation: 'opacitys',
      // 分栏高亮风格：可选�?<columns-round|columns-card>"，默�?columns-round
      columnsAsideStyle: 'columns-round',
      // 分栏布局风格：可选�?<columns-horizontal|columns-vertical>"，默�?columns-horizontal
      columnsAsideLayout: 'columns-vertical',

      /**
       * 布局切换
       * 注意：为了演示，切换布局时，颜色会被还原成默认，代码位置�?@/layout/navBars/topBar/setings.vue
       * 中的 `initSetLayoutChange(设置布局切换，重置主题样�?` 方法
       */
      // 布局切换：可选�?<默认 defaults | 经典 classic | 横向 transverse | 分栏 columns>"，默�?defaults
      layout: 'columns',

      /**
       * 后端控制路由
       */
      // 是否开启后端控制路�?
      isRequestRoutes: true,

      /**
       * 全局网站标题 / 副标�?
       */
      // 网站主标题（菜单导航、浏览器当前网页标题�?
      globalTitle: '中台Admin',
      // 网站副标题（登录页顶部文字）
      globalViceTitle: '中台Admin',
      // 网站副标题（登录页顶部文字）
      globalViceTitleMsg: '后台权限管理框架',
      // 默认初始语言，可选�?<zh-cn|en|zh-tw>"，默�?zh-cn
      globalI18n: 'zh-cn',
      // 默认全局组件大小，可选�?<large|'default'|small>"，默�?'large'
      globalComponentSize: 'default',
    },
  }),
  actions: {
    setThemeConfig(data: ThemeConfigState) {
      this.themeConfig = data.themeConfig
    },
  },
})
