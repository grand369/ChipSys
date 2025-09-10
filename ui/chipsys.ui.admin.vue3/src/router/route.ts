import { RouteRecordRaw } from 'vue-router'

/**
 * 建议：路�?path 路径与文件夹名称相同，找文件可浏览器地址找，方便定位文件位置
 *
 * 路由meta对象参数说明
 * meta: {
 *      title:          菜单栏及 tagsView 栏、菜单搜索名称（国际化）
 *      isLink�?       是否超链接菜单，开启外链条件，`1、isLink: 链接地址不为�?2、isIframe:false`
 *      isHide�?       是否隐藏此路�?
 *      isKeepAlive�?  是否缓存组件状�?
 *      isAffix�?      是否固定�?tagsView 栏上
 *      isIframe�?     是否内嵌窗口，开启条件，`1、isIframe:true 2、isLink：链接地址不为空`
 *      roles�?        当前路由权限标识，取角色管理。控制路由显示、隐藏。超级管理员：admin 普通角色：common
 *      icon�?         菜单、tagsView 图标，阿里：�?`iconfont xxx`，fontawesome：加 `fa xxx`
 * }
 */

// 扩展 RouteMeta 接口
declare module 'vue-router' {
  interface RouteMeta {
    id?: number
    title?: string
    isLink?: string
    isHide?: boolean
    isKeepAlive?: boolean
    isAffix?: boolean
    isIframe?: boolean
    isPublic?: boolean
    roles?: string[]
    icon?: string
  }
}

/**
 * 定义登录访问界面
 */
export const commonRoutes = [
  {
    path: '/personal',
    name: 'admin/personal',
    component: () => import('/@/views/admin/personal/index.vue'),
    meta: {
      title: '个人中心',
      isLink: '',
      isHide: true,
      isKeepAlive: true,
      isAffix: false,
      isIframe: false,
      roles: ['admin'],
      icon: 'ele-Message',
    },
  },
  {
    path: '/site-msg',
    name: 'admin/site-msg',
    component: () => import('/@/views/admin/site-msg/index.vue'),
    meta: {
      title: '站内�?,
      isLink: '',
      isHide: true,
      isKeepAlive: true,
      isAffix: false,
      isIframe: false,
      roles: ['admin'],
      icon: 'ele-Message',
    },
  },
  {
    path: '/site-msg/detail',
    name: 'admin/site-msg/detail',
    component: () => import('/@/views/admin/site-msg/detail.vue'),
    meta: {
      title: '站内信详�?,
      isLink: '',
      isHide: true,
      isKeepAlive: true,
      isAffix: false,
      isIframe: false,
      roles: ['admin'],
      icon: 'ele-Comment',
    },
  },
]

/**
 * 定义动态路�?
 * 前端添加路由，请在顶级节点的 `children 数组` 里添�?
 * @description 未开�?isRequestRoutes �?true 时使用（前端控制路由），开启时第一个顶�?children 的路由将被替换成接口请求回来的路由数�?
 * @description 各字段请查看 `/@/views/example/system/menu/component/addMenu.vue 下的 ruleForm`
 * @returns 返回路由菜单数据
 */
export const dynamicRoutes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: '/',
    component: () => import('/@/layout/index.vue'),
    //redirect: '/platform/workbench',
    meta: {
      isKeepAlive: true,
    },
    children: import.meta.env.PROD
      ? []
      : [
          {
            path: '/example',
            name: 'example',
            component: () => import('/@/layout/routerView/parent.vue'),
            redirect: '',
            meta: {
              title: '参考样�?,
              isLink: '',
              isHide: false,
              isKeepAlive: true,
              isAffix: false,
              isIframe: false,
              roles: ['admin'],
              icon: 'ele-Menu',
              isDir: true,
            },
            children: [
              {
                path: '/example/home',
                name: 'example/home',
                component: () => import('/@/views/example/home/index.vue'),
                meta: {
                  title: 'message.router.home',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-shouye',
                },
              },
              {
                path: '/example/system',
                name: 'example/system',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/system/menu',
                meta: {
                  title: 'message.router.system',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin'],
                  icon: 'iconfont icon-xitongshezhi',
                },
                children: [
                  {
                    path: '/example/system/menu',
                    name: 'example/systemMenu',
                    component: () => import('/@/views/example/system/menu/index.vue'),
                    meta: {
                      title: 'message.router.systemMenu',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-caidan',
                    },
                  },
                  {
                    path: '/example/system/role',
                    name: 'example/systemRole',
                    component: () => import('/@/views/example/system/role/index.vue'),
                    meta: {
                      title: 'message.router.systemRole',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-ColdDrink',
                    },
                  },
                  {
                    path: '/example/system/user',
                    name: 'example/systemUser',
                    component: () => import('/@/views/example/system/user/index.vue'),
                    meta: {
                      title: 'message.router.systemUser',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-icon-',
                    },
                  },
                  {
                    path: '/example/system/dept',
                    name: 'example/systemDept',
                    component: () => import('/@/views/example/system/dept/index.vue'),
                    meta: {
                      title: 'message.router.systemDept',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-OfficeBuilding',
                    },
                  },
                  {
                    path: '/example/system/dic',
                    name: 'example/systemDic',
                    component: () => import('/@/views/example/system/dic/index.vue'),
                    meta: {
                      title: 'message.router.systemDic',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-SetUp',
                    },
                  },
                ],
              },
              {
                path: '/example/limits',
                name: 'example/limits',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/limits/frontEnd',
                meta: {
                  title: 'message.router.limits',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-quanxian',
                },
                children: [
                  {
                    path: '/example/limits/frontEnd',
                    name: 'example/limitsFrontEnd',
                    component: () => import('/@/layout/routerView/parent.vue'),
                    redirect: '/limits/frontEnd/page',
                    meta: {
                      title: 'message.router.limitsFrontEnd',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: '',
                    },
                    children: [
                      {
                        path: '/example/limits/frontEnd/page',
                        name: 'example/limitsFrontEndPage',
                        component: () => import('/@/views/example/limits/frontEnd/page/index.vue'),
                        meta: {
                          title: 'message.router.limitsFrontEndPage',
                          isLink: '',
                          isHide: false,
                          isKeepAlive: true,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: '',
                        },
                      },
                      {
                        path: '/example/limits/frontEnd/btn',
                        name: 'example/limitsFrontEndBtn',
                        component: () => import('/@/views/example/limits/frontEnd/btn/index.vue'),
                        meta: {
                          title: 'message.router.limitsFrontEndBtn',
                          isLink: '',
                          isHide: false,
                          isKeepAlive: true,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: '',
                        },
                      },
                    ],
                  },
                  {
                    path: '/example/limits/backEnd',
                    name: 'example/limitsBackEnd',
                    component: () => import('/@/layout/routerView/parent.vue'),
                    meta: {
                      title: 'message.router.limitsBackEnd',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: '',
                    },
                    children: [
                      {
                        path: '/example/limits/backEnd/page',
                        name: 'example/limitsBackEndEndPage',
                        component: () => import('/@/views/example/limits/backEnd/page/index.vue'),
                        meta: {
                          title: 'message.router.limitsBackEndEndPage',
                          isLink: '',
                          isHide: false,
                          isKeepAlive: true,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: '',
                        },
                      },
                    ],
                  },
                ],
              },
              {
                path: '/example/menu',
                name: 'example/menu',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/example/menu/menu1',
                meta: {
                  title: 'message.router.menu',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-caidan',
                },
                children: [
                  {
                    path: '/example/menu/menu1',
                    name: 'example/menu1',
                    component: () => import('/@/layout/routerView/parent.vue'),
                    redirect: '/example/menu/menu1/menu11',
                    meta: {
                      title: 'message.router.menu1',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-caidan',
                    },
                    children: [
                      {
                        path: '/example/menu/menu1/menu11',
                        name: 'example/menu11',
                        component: () => import('/@/views/example/menu/menu1/menu11/index.vue'),
                        meta: {
                          title: 'message.router.menu11',
                          isLink: '',
                          isHide: false,
                          isKeepAlive: true,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: 'iconfont icon-caidan',
                        },
                      },
                      {
                        path: '/example/menu/menu1/menu12',
                        name: 'example/menu12',
                        component: () => import('/@/layout/routerView/parent.vue'),
                        redirect: '/example/menu/menu1/menu12/menu121',
                        meta: {
                          title: 'message.router.menu12',
                          isLink: '',
                          isHide: false,
                          isKeepAlive: true,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: 'iconfont icon-caidan',
                        },
                        children: [
                          {
                            path: '/example/menu/menu1/menu12/menu121',
                            name: 'example/menu121',
                            component: () => import('/@/views/example/menu/menu1/menu12/menu121/index.vue'),
                            meta: {
                              title: 'message.router.menu121',
                              isLink: '',
                              isHide: false,
                              isKeepAlive: true,
                              isAffix: false,
                              isIframe: false,
                              roles: ['admin', 'common'],
                              icon: 'iconfont icon-caidan',
                            },
                          },
                          {
                            path: '/example/menu/menu1/menu12/menu122',
                            name: 'example/menu122',
                            component: () => import('/@/views/example/menu/menu1/menu12/menu122/index.vue'),
                            meta: {
                              title: 'message.router.menu122',
                              isLink: '',
                              isHide: false,
                              isKeepAlive: true,
                              isAffix: false,
                              isIframe: false,
                              roles: ['admin', 'common'],
                              icon: 'iconfont icon-caidan',
                            },
                          },
                        ],
                      },
                      {
                        path: '/example/menu/menu1/menu13',
                        name: 'example/menu13',
                        component: () => import('/@/views/example/menu/menu1/menu13/index.vue'),
                        meta: {
                          title: 'message.router.menu13',
                          isLink: '',
                          isHide: false,
                          isKeepAlive: true,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: 'iconfont icon-caidan',
                        },
                      },
                    ],
                  },
                  {
                    path: '/example/menu/menu2',
                    name: 'example/menu2',
                    component: () => import('/@/views/example/menu/menu2/index.vue'),
                    meta: {
                      title: 'message.router.menu2',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-caidan',
                    },
                  },
                ],
              },
              {
                path: '/example/fun',
                name: 'example/funIndex',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/fun/tagsView',
                meta: {
                  title: 'message.router.funIndex',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-crew_feature',
                },
                children: [
                  {
                    path: '/example/fun/tagsView',
                    name: 'example/funTagsView',
                    component: () => import('/@/views/example/fun/tagsView/index.vue'),
                    meta: {
                      title: 'message.router.funTagsView',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Pointer',
                    },
                  },
                  {
                    path: '/example/fun/countup',
                    name: 'example/funCountup',
                    component: () => import('/@/views/example/fun/countup/index.vue'),
                    meta: {
                      title: 'message.router.funCountup',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Odometer',
                    },
                  },
                  {
                    path: '/example/fun/wangEditor',
                    name: 'example/funWangEditor',
                    component: () => import('/@/views/example/fun/wangEditor/index.vue'),
                    meta: {
                      title: 'message.router.funWangEditor',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-fuwenbenkuang',
                    },
                  },
                  {
                    path: '/example/fun/cropper',
                    name: 'example/funCropper',
                    component: () => import('/@/views/example/fun/cropper/index.vue'),
                    meta: {
                      title: 'message.router.funCropper',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-caijian',
                    },
                  },
                  {
                    path: '/example/fun/qrcode',
                    name: 'example/funQrcode',
                    component: () => import('/@/views/example/fun/qrcode/index.vue'),
                    meta: {
                      title: 'message.router.funQrcode',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-ico',
                    },
                  },
                  // {
                  //   path: '/example/fun/echartsMap',
                  //   name: 'example/funEchartsMap',
                  //   component: () => import('/@/views/example/fun/echartsMap/index.vue'),
                  //   meta: {
                  //     title: 'message.router.funEchartsMap',
                  //     isLink: '',
                  //     isHide: false,
                  //     isKeepAlive: true,
                  //     isAffix: false,
                  //     isIframe: false,
                  //     roles: ['admin', 'common'],
                  //     icon: 'iconfont icon-ditu',
                  //   },
                  // },
                  {
                    path: '/example/fun/printJs',
                    name: 'example/funPrintJs',
                    component: () => import('/@/views/example/fun/printJs/index.vue'),
                    meta: {
                      title: 'message.router.funPrintJs',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Printer',
                    },
                  },
                  {
                    path: '/example/fun/clipboard',
                    name: 'example/funClipboard',
                    component: () => import('/@/views/example/fun/clipboard/index.vue'),
                    meta: {
                      title: 'message.router.funClipboard',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-DocumentCopy',
                    },
                  },
                  {
                    path: '/example/fun/gridLayout',
                    name: 'example/funGridLayout',
                    component: () => import('/@/views/example/fun/gridLayout/index.vue'),
                    meta: {
                      title: 'message.router.funGridLayout',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-tuodong',
                    },
                  },
                  {
                    path: '/example/fun/splitpanes',
                    name: 'example/funSplitpanes',
                    component: () => import('/@/views/example/fun/splitpanes/index.vue'),
                    meta: {
                      title: 'message.router.funSplitpanes',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon--chaifenlie',
                    },
                  },
                ],
              },
              {
                path: '/example/pages',
                name: 'example/pagesIndex',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/pages/filtering',
                meta: {
                  title: 'message.router.pagesIndex',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-fuzhiyemian',
                },
                children: [
                  {
                    path: '/example/pages/filtering',
                    name: 'example/pagesFiltering',
                    component: () => import('/@/views/example/pages/filtering/index.vue'),
                    meta: {
                      title: 'message.router.pagesFiltering',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Sell',
                    },
                    /**
                     * 注意此处详情写法�?
                     * 1、嵌套进父级里时，面包屑显示为：首页/页面/过滤筛选组�?过滤筛选组件详�?
                     * 2、不嵌套进父级时，面包屑显示为：首页/页面/过滤筛选组�?过滤筛选组件详�?
                     * 3、想要父级不高亮，面包屑显示为：首页/页面/过滤筛选组件详情，设置路径为：/pages/filteringDetails
                     */
                    children: [
                      {
                        path: '/example/pages/filtering/details',
                        name: 'example/pagesFilteringDetails',
                        component: () => import('/@/views/example/pages/filtering/details.vue'),
                        meta: {
                          title: 'message.router.pagesFilteringDetails',
                          isLink: '',
                          isHide: true,
                          isKeepAlive: false,
                          isAffix: false,
                          isIframe: false,
                          roles: ['admin', 'common'],
                          icon: 'ele-Sunny',
                        },
                      },
                    ],
                  },
                  {
                    path: '/example/pages/filtering/details1',
                    name: 'example/pagesFilteringDetails1',
                    component: () => import('/@/views/example/pages/filtering/details1.vue'),
                    meta: {
                      title: 'message.router.pagesFilteringDetails1',
                      isLink: '',
                      isHide: true,
                      isKeepAlive: false,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Sunny',
                    },
                  },
                  {
                    path: '/example/pages/iocnfont',
                    name: 'example/pagesIocnfont',
                    component: () => import('/@/views/example/pages/iocnfont/index.vue'),
                    meta: {
                      title: 'message.router.pagesIocnfont',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Present',
                    },
                  },
                  {
                    path: '/example/pages/element',
                    name: 'example/pagesElement',
                    component: () => import('/@/views/example/pages/element/index.vue'),
                    meta: {
                      title: 'message.router.pagesElement',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Eleme',
                    },
                  },
                  {
                    path: '/example/pages/awesome',
                    name: 'example/pagesAwesome',
                    component: () => import('/@/views/example/pages/awesome/index.vue'),
                    meta: {
                      title: 'message.router.pagesAwesome',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-SetUp',
                    },
                  },
                  {
                    path: '/example/pages/formAdapt',
                    name: 'example/pagesFormAdapt',
                    component: () => import('/@/views/example/pages/formAdapt/index.vue'),
                    meta: {
                      title: 'message.router.pagesFormAdapt',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-biaodan',
                    },
                  },
                  {
                    path: '/example/pages/tableRules',
                    name: 'example/pagesTableRules',
                    component: () => import('/@/views/example/pages/tableRules/index.vue'),
                    meta: {
                      title: 'message.router.pagesTableRules',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-jiliandongxuanzeqi',
                    },
                  },
                  {
                    path: '/example/pages/formI18n',
                    name: 'example/pagesFormI18n',
                    component: () => import('/@/views/example/pages/formI18n/index.vue'),
                    meta: {
                      title: 'message.router.pagesFormI18n',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-diqiu',
                    },
                  },
                  {
                    path: '/example/pages/formRules',
                    name: 'example/pagesFormRules',
                    component: () => import('/@/views/example/pages/formRules/index.vue'),
                    meta: {
                      title: 'message.router.pagesFormRules',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-shuxing',
                    },
                  },
                  {
                    path: '/example/pages/listAdapt',
                    name: 'example/pagesListAdapt',
                    component: () => import('/@/views/example/pages/listAdapt/index.vue'),
                    meta: {
                      title: 'message.router.pagesListAdapt',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-chazhaobiaodanliebiao',
                    },
                  },
                  {
                    path: '/example/pages/waterfall',
                    name: 'example/pagesWaterfall',
                    component: () => import('/@/views/example/pages/waterfall/index.vue'),
                    meta: {
                      title: 'message.router.pagesWaterfall',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-zidingyibuju',
                    },
                  },
                  {
                    path: '/example/pages/steps',
                    name: 'example/pagesSteps',
                    component: () => import('/@/views/example/pages/steps/index.vue'),
                    meta: {
                      title: 'message.router.pagesSteps',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-step',
                    },
                  },
                  {
                    path: '/example/pages/preview',
                    name: 'example/pagesPreview',
                    component: () => import('/@/views/example/pages/preview/index.vue'),
                    meta: {
                      title: 'message.router.pagesPreview',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-15tupianyulan',
                    },
                  },
                  {
                    path: '/example/pages/waves',
                    name: 'example/pagesWaves',
                    component: () => import('/@/views/example/pages/waves/index.vue'),
                    meta: {
                      title: 'message.router.pagesWaves',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-bolangneng',
                    },
                  },
                  {
                    path: '/example/pages/tree',
                    name: 'example/pagesTree',
                    component: () => import('/@/views/example/pages/tree/index.vue'),
                    meta: {
                      title: 'message.router.pagesTree',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-shuxingtu',
                    },
                  },
                  {
                    path: '/example/pages/drag',
                    name: 'example/pagesDrag',
                    component: () => import('/@/views/example/pages/drag/index.vue'),
                    meta: {
                      title: 'message.router.pagesDrag',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Pointer',
                    },
                  },
                  {
                    path: '/example/pages/lazyImg',
                    name: 'example/pagesLazyImg',
                    component: () => import('/@/views/example/pages/lazyImg/index.vue'),
                    meta: {
                      title: 'message.router.pagesLazyImg',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-PictureFilled',
                    },
                  },
                  {
                    path: '/example/pages/dynamicForm',
                    name: 'example/pagesDynamicForm',
                    component: () => import('/@/views/example/pages/dynamicForm/index.vue'),
                    meta: {
                      title: 'message.router.pagesDynamicForm',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-wenducanshu-05',
                    },
                  },
                  {
                    path: '/example/pages/workflow',
                    name: 'example/pagesWorkflow',
                    component: () => import('/@/views/example/pages/workflow/index.vue'),
                    meta: {
                      title: 'message.router.pagesWorkflow',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-Connection',
                    },
                  },
                ],
              },
              {
                path: '/example/make',
                name: 'example/makeIndex',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/make/selector',
                meta: {
                  title: 'message.router.makeIndex',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin'],
                  icon: 'iconfont icon-siweidaotu',
                },
                children: [
                  {
                    path: '/example/make/selector',
                    name: 'example/makeSelector',
                    component: () => import('/@/views/example/make/selector/index.vue'),
                    meta: {
                      title: 'message.router.makeSelector',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-xuanzeqi',
                    },
                  },
                  {
                    path: '/example/make/noticeBar',
                    name: 'example/makeNoticeBar',
                    component: () => import('/@/views/example/make/noticeBar/index.vue'),
                    meta: {
                      title: 'message.router.makeNoticeBar',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'ele-Bell',
                    },
                  },
                  {
                    path: '/example/make/svgDemo',
                    name: 'example/makeSvgDemo',
                    component: () => import('/@/views/example/make/svgDemo/index.vue'),
                    meta: {
                      title: 'message.router.makeSvgDemo',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'fa fa-thumbs-o-up',
                    },
                  },
                  {
                    path: '/example/make/tableDemo',
                    name: 'example/makeTableDemo',
                    component: () => import('/@/views/example/make/tableDemo/index.vue'),
                    meta: {
                      title: 'message.router.makeTableDemo',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin', 'common'],
                      icon: 'iconfont icon-shuju',
                    },
                  },
                ],
              },
              {
                path: '/example/params',
                name: 'example/paramsIndex',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/params/common',
                meta: {
                  title: 'message.router.paramsIndex',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin'],
                  icon: 'iconfont icon-zhongduancanshu',
                },
                children: [
                  {
                    path: '/example/params/common',
                    name: 'example/paramsCommon',
                    component: () => import('/@/views/example/params/common/index.vue'),
                    meta: {
                      title: 'message.router.paramsCommon',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-putong',
                    },
                  },
                  {
                    path: '/example/params/common/details',
                    name: 'example/paramsCommonDetails',
                    component: () => import('/@/views/example/params/common/details.vue'),
                    meta: {
                      title: 'message.router.paramsCommonDetails',
                      isLink: '',
                      isHide: true,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-Comment',
                    },
                  },
                  {
                    path: '/example/params/dynamic',
                    name: 'example/paramsDynamic',
                    component: () => import('/@/views/example/params/dynamic/index.vue'),
                    meta: {
                      title: 'message.router.paramsDynamic',
                      isLink: '',
                      isHide: false,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-dongtai',
                    },
                  },
                  /**
                   * tagsViewName 为要设置不同�?"tagsView 名称" 字段
                   * 如若需设置不同 "tagsView 名称"，tagsViewName 字段必须要有
                   */
                  {
                    path: '/example/params/dynamic/details/:t/:id/:tagsViewName',
                    name: 'example/paramsDynamicDetails',
                    component: () => import('/@/views/example/params/dynamic/details.vue'),
                    meta: {
                      title: 'message.router.paramsDynamicDetails',
                      isLink: '',
                      isHide: true,
                      isKeepAlive: true,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'ele-Lightning',
                    },
                  },
                ],
              },
              {
                path: '/example/visualizing',
                name: 'example/visualizingIndex',
                component: () => import('/@/layout/routerView/parent.vue'),
                redirect: '/visualizing/visualizingLinkDemo1',
                meta: {
                  title: 'message.router.visualizingIndex',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin'],
                  icon: 'ele-ChatLineRound',
                },
                /**
                 * 打开内置全屏
                 * component 都为 `() => import('/@/layout/routerView/link.vue')`
                 * isLink 链接为内置的路由地址，地址�?staticRoutes 中定�?
                 */
                children: [
                  {
                    path: '/example/visualizing/visualizingLinkDemo1',
                    name: 'example/visualizingLinkDemo1',
                    component: () => import('/@/layout/routerView/link.vue'),
                    meta: {
                      title: 'message.router.visualizingLinkDemo1',
                      isLink: '/example/visualizingDemo1',
                      isHide: false,
                      isKeepAlive: false,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-caozuo-wailian',
                    },
                  },
                  {
                    path: '/example/visualizing/visualizingLinkDemo2',
                    name: 'example/visualizingLinkDemo2',
                    component: () => import('/@/layout/routerView/link.vue'),
                    meta: {
                      title: 'message.router.visualizingLinkDemo2',
                      isLink: '/example/visualizingDemo2',
                      isHide: false,
                      isKeepAlive: false,
                      isAffix: false,
                      isIframe: false,
                      roles: ['admin'],
                      icon: 'iconfont icon-caozuo-wailian',
                    },
                  },
                ],
              },
              {
                path: '/example/chart',
                name: 'example/chartIndex',
                component: () => import('/@/views/example/chart/index.vue'),
                meta: {
                  title: 'message.router.chartIndex',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-ico_shuju',
                },
              },
              {
                path: '/example/personal',
                name: 'example/personal',
                component: () => import('/@/views/example/personal/index.vue'),
                meta: {
                  title: 'message.router.personal',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-gerenzhongxin',
                },
              },
              {
                path: '/example/tools',
                name: 'example/tools',
                component: () => import('/@/views/example/tools/index.vue'),
                meta: {
                  title: 'message.router.tools',
                  isLink: '',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin', 'common'],
                  icon: 'iconfont icon-gongju',
                },
              },
              {
                path: '/example/link',
                name: 'example/layoutLinkView',
                component: () => import('/@/layout/routerView/link.vue'),
                meta: {
                  title: 'message.router.layoutLinkView',
                  isLink: 'https://element-plus.org/zh-CN/guide/design.html',
                  isHide: false,
                  isKeepAlive: false,
                  isAffix: false,
                  isIframe: false,
                  roles: ['admin'],
                  icon: 'iconfont icon-caozuo-wailian',
                },
              },
              {
                path: '/example/iframesOne',
                name: 'example/layoutIframeViewOne',
                component: () => import('/@/layout/routerView/iframes.vue'),
                meta: {
                  title: 'message.router.layoutIframeViewOne',
                  isLink: 'https://nodejs.org/zh-cn/',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: true,
                  roles: ['admin'],
                  icon: 'iconfont icon-neiqianshujuchucun',
                },
              },
              {
                path: '/example/iframesTwo',
                name: 'example/layoutIframeViewTwo',
                component: () => import('/@/layout/routerView/iframes.vue'),
                meta: {
                  title: 'message.router.layoutIframeViewTwo',
                  isLink: 'https://undraw.co/illustrations',
                  isHide: false,
                  isKeepAlive: true,
                  isAffix: false,
                  isIframe: true,
                  roles: ['admin'],
                  icon: 'iconfont icon-neiqianshujuchucun',
                },
              },
            ],
          },
        ],
  },
]

/**
 * 定义404�?01界面
 * @link 参考：https://next.router.vuejs.org/zh/guide/essentials/history-mode.html#netlify
 */
export const notFoundAndNoPower = [
  {
    path: '/:path(.*)*',
    name: 'notFound',
    component: () => import('/@/views/error/404.vue'),
    meta: {
      title: 'message.staticRoutes.notFound',
      isHide: true,
    },
  },
  {
    path: '/401',
    name: 'noPower',
    component: () => import('/@/views/error/401.vue'),
    meta: {
      title: 'message.staticRoutes.noPower',
      isHide: true,
    },
  },
]

/**
 * 定义静态路由（默认路由�?
 * 此路由不要动，前端添加路由的话，请在 `dynamicRoutes 数组` 中添�?
 * @description 前端控制直接�?dynamicRoutes 中的路由，后端控制不需要修改，请求接口路由数据时，会覆�?dynamicRoutes 第一个顶�?children 的内容（全屏，不包含 layout 中的路由出口�?
 * @returns 返回路由菜单数据
 */
export const staticRoutes: Array<RouteRecordRaw> = [
  {
    path: '/login',
    name: 'login',
    component: () => import('/@/views/admin/login/index.vue'),
    meta: {
      title: '登录',
      isPublic: true,
    },
  },
  /**
   * 提示：写在这里的为全屏界面，不建议写在这�?
   * 请写�?`dynamicRoutes` 路由数组�?
   */
  {
    path: '/example/visualizingDemo1',
    name: 'example/visualizingDemo1',
    component: () => import('/@/views/example/visualizing/demo1.vue'),
    meta: {
      title: 'message.router.visualizingLinkDemo1',
    },
  },
  {
    path: '/example/visualizingDemo2',
    name: 'example/visualizingDemo2',
    component: () => import('/@/views/example/visualizing/demo2.vue'),
    meta: {
      title: 'message.router.visualizingLinkDemo2',
    },
  },
]
