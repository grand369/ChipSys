let _boundaryCheckingState = true // 是否进行越界检查的全局开�?

/**
 * 把错误的数据转正
 * @private
 * @example strip(0.09999999999999998)=0.1
 */
function strip(num: number | string, precision = 15) {
  return +parseFloat(Number(num).toPrecision(precision))
}

/**
 * Return digits length of a number
 * @private
 * @param {*number} num Input number
 */
function digitLength(num: number | string) {
  // Get digit length of e
  const eSplit = num.toString().split(/[eE]/)
  const len = (eSplit[0].split('.')[1] || '').length - +(eSplit[1] || 0)
  return len > 0 ? len : 0
}

/**
 * 把小数转成整�?如果是小数则放大成整�?
 * @private
 * @param {*number} num 输入�?
 */
function float2Fixed(num: number | string) {
  if (num.toString().indexOf('e') === -1) {
    return Number(num.toString().replace('.', ''))
  }
  const dLen = digitLength(num)
  return dLen > 0 ? strip(Number(num) * Math.pow(10, dLen)) : Number(num)
}

/**
 * 检测数字是否越界，如果越界给出提示
 * @private
 * @param {*number} num 输入�?
 */
function checkBoundary(num: number | string) {
  if (_boundaryCheckingState) {
    if (num > Number.MAX_SAFE_INTEGER || num < Number.MIN_SAFE_INTEGER) {
      window.console.warn(`${num} 超出了精度限制，结果可能不正确`)
    }
  }
}

/**
 * 把递归操作扁平迭代�?
 * @param {number[]} arr 要操作的数字数组
 * @param {function} operation 迭代操作
 * @private
 */
function iteratorOperation(arr: number[], operation: Function) {
  const [num1, num2, ...others] = arr
  let res = operation(num1, num2)

  others.forEach((num) => {
    res = operation(res, num)
  })

  return res
}

/**
 * 高精度乘�?
 * @export
 */
export function times(...nums: any) {
  if (nums.length > 2) {
    return iteratorOperation(nums, times)
  }

  const [num1, num2] = nums
  const num1Changed = float2Fixed(num1)
  const num2Changed = float2Fixed(num2)
  const baseNum = digitLength(num1) + digitLength(num2)
  const leftValue = num1Changed * num2Changed

  checkBoundary(leftValue)

  return leftValue / Math.pow(10, baseNum)
}

/**
 * 高精度加�?
 * @export
 */
export function plus(...nums: any) {
  if (nums.length > 2) {
    return iteratorOperation(nums, plus)
  }

  const [num1, num2] = nums
  // 取最大的小数�?
  const baseNum = Math.pow(10, Math.max(digitLength(num1), digitLength(num2)))
  // 把小数都转为整数然后再计�?
  return (times(num1, baseNum) + times(num2, baseNum)) / baseNum
}

/**
 * 高精度减�?
 * @export
 */
export function minus(...nums: any) {
  if (nums.length > 2) {
    return iteratorOperation(nums, minus)
  }

  const [num1, num2] = nums
  const baseNum = Math.pow(10, Math.max(digitLength(num1), digitLength(num2)))
  return (times(num1, baseNum) - times(num2, baseNum)) / baseNum
}

/**
 * 高精度除�?
 * @export
 */
export function divide(...nums: any) {
  if (nums.length > 2) {
    return iteratorOperation(nums, divide)
  }

  const [num1, num2] = nums
  const num1Changed = float2Fixed(num1)
  const num2Changed = float2Fixed(num2)
  checkBoundary(num1Changed)
  checkBoundary(num2Changed)
  // 重要，这里必须用strip进行修正
  return times(num1Changed / num2Changed, strip(Math.pow(10, digitLength(num2) - digitLength(num1))))
}

/**
 * 四舍五入
 * @export
 */
export function round(num: number, ratio: number) {
  const base = Math.pow(10, ratio)
  let result = divide(Math.round(Math.abs(times(num, base))), base)
  if (num < 0 && result !== 0) {
    result = times(result, -1)
  }
  // 位数不足则补0
  return result
}

/**
 * 是否进行边界检查，默认开�?
 * @param flag 标记开关，true 为开启，false 为关闭，默认�?true
 * @export
 */
export function enableBoundaryChecking(flag = true) {
  _boundaryCheckingState = flag
}

export default {
  times,
  plus,
  minus,
  divide,
  round,
  enableBoundaryChecking,
}
