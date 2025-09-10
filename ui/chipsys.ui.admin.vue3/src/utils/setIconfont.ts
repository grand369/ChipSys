// 字体图标 url
const cssCdnUrlList: Array<string> = [
  //兼容国内项目
  '//at.alicdn.com/t/c/font_2298093_rnp72ifj3ba.css',
  //'//cdn.bootcdn.net/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css',
  //兼容国内外项�?
  '//cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css',
]
// 第三�?js url
const jsCdnUrlList: Array<string> = []

// 动态批量设置字体图�?
export function setCssCdn() {
  if (cssCdnUrlList.length <= 0) return false
  cssCdnUrlList.map((v) => {
    let link = document.createElement('link')
    link.rel = 'stylesheet'
    link.href = v
    link.crossOrigin = 'anonymous'
    document.getElementsByTagName('head')[0].appendChild(link)
  })
}

// 动态批量设置第三方js
export function setJsCdn() {
  if (jsCdnUrlList.length <= 0) return false
  jsCdnUrlList.map((v) => {
    let link = document.createElement('script')
    link.src = v
    document.body.appendChild(link)
  })
}

/**
 * 批量设置字体图标、动态js
 * @method cssCdn 动态批量设置字体图�?
 * @method jsCdn 动态批量设置第三方js
 */
const setIntroduction = {
  // 设置css
  cssCdn: () => {
    setCssCdn()
  },
  // 设置js
  jsCdn: () => {
    setJsCdn()
  },
}

// 导出函数方法
export default setIntroduction
