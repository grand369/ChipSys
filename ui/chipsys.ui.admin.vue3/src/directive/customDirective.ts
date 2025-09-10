import type { App } from 'vue'

/**
 * 按钮波浪指令
 * @directive 默认方式：v-waves，如 `<div v-waves></div>`
 * @directive 参数方式：v-waves=" |light|red|orange|purple|green|teal"，如 `<div v-waves="'light'"></div>`
 */
export function wavesDirective(app: App) {
  app.directive('waves', {
    mounted(el, binding) {
      el.classList.add('waves-effect')
      binding.value && el.classList.add(`waves-${binding.value}`)
      function setConvertStyle(obj: { [key: string]: unknown }) {
        let style: string = ''
        for (let i in obj) {
          if (obj.hasOwnProperty(i)) style += `${i}:${obj[i]};`
        }
        return style
      }
      function onCurrentClick(e: { [key: string]: unknown }) {
        let elDiv = document.createElement('div')
        elDiv.classList.add('waves-ripple')
        el.appendChild(elDiv)
        let styles = {
          left: `${e.layerX}px`,
          top: `${e.layerY}px`,
          opacity: 1,
          transform: `scale(${(el.clientWidth / 100) * 10})`,
          'transition-duration': `750ms`,
          'transition-timing-function': `cubic-bezier(0.250, 0.460, 0.450, 0.940)`,
        }
        elDiv.setAttribute('style', setConvertStyle(styles))
        setTimeout(() => {
          elDiv.setAttribute(
            'style',
            setConvertStyle({
              opacity: 0,
              transform: styles.transform,
              left: styles.left,
              top: styles.top,
            })
          )
          setTimeout(() => {
            elDiv && el.removeChild(elDiv)
          }, 750)
        }, 450)
      }
      el.addEventListener('mousedown', onCurrentClick, false)
    },
    unmounted(el) {
      el.addEventListener('mousedown', () => {})
    },
  })
}

/**
 * 自定义拖动指�?
 * @description  使用方式：v-drag="[dragDom,dragHeader]"，如 `<div v-drag="['.drag-container .el-dialog', '.drag-container .el-dialog__header']"></div>`
 * @description dragDom 要拖动的元素，dragHeader 要拖动的 Header 位置
 * @link 注意：https://github.com/element-plus/element-plus/issues/522
 * @lick 参考：https://blog.csdn.net/weixin_46391323/article/details/105228020?utm_medium=distribute.pc_relevant.none-task-blog-baidujs_title-10&spm=1001.2101.3001.4242
 */
export function dragDirective(app: App) {
  app.directive('drag', {
    mounted(el, binding) {
      if (!binding.value) return false

      const dragDom = document.querySelector(binding.value[0]) as HTMLElement
      const dragHeader = document.querySelector(binding.value[1]) as HTMLElement

      dragHeader.onmouseover = () => (dragHeader.style.cursor = `move`)

      function down(e: any, type: string) {
        // 鼠标按下，计算当前元素距离可视区的距�?
        const disX = type === 'pc' ? e.clientX - dragHeader.offsetLeft : e.touches[0].clientX - dragHeader.offsetLeft
        const disY = type === 'pc' ? e.clientY - dragHeader.offsetTop : e.touches[0].clientY - dragHeader.offsetTop

        // body当前宽度
        const screenWidth = document.body.clientWidth
        // 可见区域高度(应为body高度，可某些环境下无法获�?
        const screenHeight = document.documentElement.clientHeight

        // 对话框宽�?
        const dragDomWidth = dragDom.offsetWidth
        // 对话框高�?
        const dragDomheight = dragDom.offsetHeight

        const minDragDomLeft = dragDom.offsetLeft
        const maxDragDomLeft = screenWidth - dragDom.offsetLeft - dragDomWidth

        const minDragDomTop = dragDom.offsetTop
        const maxDragDomTop = screenHeight - dragDom.offsetTop - dragDomheight

        // 获取到的值带px 正则匹配替换
        let styL: any = getComputedStyle(dragDom).left
        let styT: any = getComputedStyle(dragDom).top

        // 注意在ie�?第一次获取到的值为组件自带50% 移动之后赋值为px
        if (styL.includes('%')) {
          styL = +document.body.clientWidth * (+styL.replace(/\%/g, '') / 100)
          styT = +document.body.clientHeight * (+styT.replace(/\%/g, '') / 100)
        } else {
          styL = +styL.replace(/\px/g, '')
          styT = +styT.replace(/\px/g, '')
        }

        return {
          disX,
          disY,
          minDragDomLeft,
          maxDragDomLeft,
          minDragDomTop,
          maxDragDomTop,
          styL,
          styT,
        }
      }

      function move(e: any, type: string, obj: any) {
        let { disX, disY, minDragDomLeft, maxDragDomLeft, minDragDomTop, maxDragDomTop, styL, styT } = obj

        // 通过事件委托，计算移动的距离
        let left = type === 'pc' ? e.clientX - disX : e.touches[0].clientX - disX
        let top = type === 'pc' ? e.clientY - disY : e.touches[0].clientY - disY

        // 边界处理
        if (-left > minDragDomLeft) {
          left = -minDragDomLeft
        } else if (left > maxDragDomLeft) {
          left = maxDragDomLeft
        }

        if (-top > minDragDomTop) {
          top = -minDragDomTop
        } else if (top > maxDragDomTop) {
          top = maxDragDomTop
        }

        // 移动当前元素
        dragDom.style.cssText += `;left:${left + styL}px;top:${top + styT}px;`
      }

      /**
       * pc�?
       * onmousedown 鼠标按下触发事件
       * onmousemove 鼠标按下时持续触发事�?
       * onmouseup 鼠标抬起触发事件
       */
      dragHeader.onmousedown = (e) => {
        const obj = down(e, 'pc')
        document.onmousemove = (e) => {
          move(e, 'pc', obj)
        }
        document.onmouseup = () => {
          document.onmousemove = null
          document.onmouseup = null
        }
      }

      /**
       * 移动�?
       * ontouchstart 当按下手指时，触发ontouchstart
       * ontouchmove 当移动手指时，触发ontouchmove
       * ontouchend 当移走手指时，触发ontouchend
       */
      dragHeader.ontouchstart = (e) => {
        const obj = down(e, 'app')
        document.ontouchmove = (e) => {
          move(e, 'app', obj)
        }
        document.ontouchend = () => {
          document.ontouchmove = null
          document.ontouchend = null
        }
      }
    },
  })
}
