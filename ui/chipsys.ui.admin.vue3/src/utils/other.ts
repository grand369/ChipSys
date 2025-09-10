import { nextTick, defineAsyncComponent } from 'vue'
import type { App } from 'vue'
import * as svg from '@element-plus/icons-vue'
import router from '/@/router/index'
import pinia from '/@/stores/index'
import { storeToRefs } from 'pinia'
import { useThemeConfig } from '/@/stores/themeConfig'
import { i18n } from '/@/i18n/index'
import { Local } from '/@/utils/storage'
import { verifyUrl } from '/@/utils/toolsValidate'

// 引入组件
const SvgIcon = defineAsyncComponent(() => import('/@/components/svgIcon/index.vue'))
const MyIcon = defineAsyncComponent(() => import('/@/components/my-icon/index.vue'))

/**
 * 导出全局注册 element plus svg 图标
 * @param app vue 实例
 * @description 使用：https://element-plus.gitee.io/zh-CN/component/icon.html
 */
export function elSvg(app: App) {
  const icons = svg as any
  for (const i in icons) {
    app.component(`ele-${icons[i].name}`, icons[i])
  }
  app.component('SvgIcon', SvgIcon)
  app.component('MyIcon', MyIcon)
}

/**
 * 设置浏览器标题国际化
 * @method const title = useTitle(); ==> title()
 */
export function useTitle() {
  const stores = useThemeConfig(pinia)
  const { themeConfig } = storeToRefs(stores)
  nextTick(() => {
    let webTitle = ''
    let globalTitle: string = themeConfig.value.globalTitle
    const { path, meta } = router.currentRoute.value
    if (path === '/login') {
      webTitle = <string>meta.title
    } else {
      webTitle = setTagsViewNameI18n(router.currentRoute.value)
    }

    document.title = webTitle ? `${webTitle} - ${globalTitle}` : globalTitle
  })
}

/**
 * 设置 自定�?tagsView 名称�?自定�?tagsView 名称国际�?
 * @param params 路由 query、params 中的 tagsViewName
 * @returns 返回当前 tagsViewName 名称
 */
export function setTagsViewNameI18n(item: any) {
  let tagsViewName: string = ''
  const { query, params, meta } = item
  // 修复tagsViewName匹配到其他含下列单词的路�?
  const pattern = /^\{("(zh-cn|en|zh-tw)":"[^,]+",?){1,3}}$/
  if (query?.tagsViewName || params?.tagsViewName) {
    if (pattern.test(query?.tagsViewName) || pattern.test(params?.tagsViewName)) {
      // 国际�?
      const urlTagsParams = (query?.tagsViewName && JSON.parse(query?.tagsViewName)) || (params?.tagsViewName && JSON.parse(params?.tagsViewName))
      tagsViewName = urlTagsParams[i18n.global.locale.value]
    } else {
      // 非国际化
      tagsViewName = query?.tagsViewName || params?.tagsViewName
    }
  } else {
    // 非自定义 tagsView 名称
    if (meta.title) tagsViewName = i18n.global.t(meta.title)
  }
  return tagsViewName
}

/**
 * 图片懒加�?
 * @param el dom 目标元素
 * @param arr 列表数据
 * @description data-xxx 属性用于存储页面或应用程序的私有自定义数据
 */
export const lazyImg = (el: string, arr: EmptyArrayType) => {
  const io = new IntersectionObserver((res) => {
    res.forEach((v: any) => {
      if (v.isIntersecting) {
        const { img, key } = v.target.dataset
        v.target.src = img
        v.target.onload = () => {
          io.unobserve(v.target)
          arr[key]['loading'] = false
        }
      }
    })
  })
  nextTick(() => {
    document.querySelectorAll(el).forEach((img) => io.observe(img))
  })
}

/**
 * 全局组件大小
 * @returns 返回 `window.localStorage` 中读取的缓存�?`globalComponentSize`
 */
export const globalComponentSize = (): string => {
  const stores = useThemeConfig(pinia)
  const { themeConfig } = storeToRefs(stores)
  return Local.get('themeConfig')?.globalComponentSize || themeConfig.value?.globalComponentSize
}

/**
 * 对象深克�?
 * @param obj 源对�?
 * @returns 克隆后的对象
 */
export function deepClone(obj: EmptyObjectType) {
  let newObj: EmptyObjectType
  try {
    newObj = obj.push ? [] : {}
  } catch (error) {
    newObj = {}
  }
  for (let attr in obj) {
    if (obj[attr] && typeof obj[attr] === 'object') {
      newObj[attr] = deepClone(obj[attr])
    } else {
      newObj[attr] = obj[attr]
    }
  }
  return newObj
}

/**
 * 判断是否是移动端
 */
export function isMobile() {
  if (
    navigator.userAgent.match(
      /('phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone')/i
    )
  ) {
    return true
  } else {
    return false
  }
}

/**
 * 判断数组对象中所有属性是否为空，为空则删除当前行对象
 * @description @感谢大黄
 * @param list 数组对象
 * @returns 删除空值后的数组对�?
 */
export function handleEmpty(list: EmptyArrayType) {
  const arr = []
  for (const i in list) {
    const d = []
    for (const j in list[i]) {
      d.push(list[i][j])
    }
    const leng = d.filter((item) => item === '').length
    if (leng !== d.length) {
      arr.push(list[i])
    }
  }
  return arr
}

/**
 * 打开外部链接
 * @param val 当前点击项菜�?
 */
export function handleOpenLink(val: RouteItem) {
  const stores = useThemeConfig(pinia)
  const { themeConfig } = storeToRefs(stores)

  const { origin, pathname } = window.location
  router.push(val.path)
  if (verifyUrl(<string>val.meta?.isLink)) window.open(val.meta?.isLink)
  else themeConfig.value.isCreateWebHistory ? window.open(`${origin}${val.meta?.isLink}`) : window.open(`${origin}${pathname}#${val.meta?.isLink}`)
}

/**
 * 统一批量导出
 * @method elSvg 导出全局注册 element plus svg 图标
 * @method useTitle 设置浏览器标题国际化
 * @method setTagsViewNameI18n 设置 自定�?tagsView 名称�?自定�?tagsView 名称国际�?
 * @method lazyImg 图片懒加�?
 * @method globalComponentSize() element plus 全局组件大小
 * @method deepClone 对象深克�?
 * @method isMobile 判断是否是移动端
 * @method handleEmpty 判断数组对象中所有属性是否为空，为空则删除当前行对象
 * @method handleOpenLink 打开外部链接
 */
const other = {
  elSvg: (app: App) => {
    elSvg(app)
  },
  useTitle: () => {
    useTitle()
  },
  setTagsViewNameI18n(route: RouteToFrom) {
    return setTagsViewNameI18n(route)
  },
  lazyImg: (el: string, arr: EmptyArrayType) => {
    lazyImg(el, arr)
  },
  globalComponentSize: () => {
    return globalComponentSize()
  },
  deepClone: (obj: EmptyObjectType) => {
    return deepClone(obj)
  },
  isMobile: () => {
    return isMobile()
  },
  handleEmpty: (list: EmptyArrayType) => {
    return handleEmpty(list)
  },
  handleOpenLink: (val: RouteItem) => {
    handleOpenLink(val)
  },
}

// 统一批量导出
export default other
