/**
 * 工具类集合，适用于平时开�?
 * 新增多行注释信息，鼠标放到方法名即可查看
 */

/**
 * 验证百分比（不可以小数）
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyNumberPercentage(val: string): string {
  // 匹配空格
  let v = val.replace(/(^\s*)|(\s*$)/g, '')
  // 只能是数字和小数点，不能是其他输�?
  v = v.replace(/[^\d]/g, '')
  // 不能�?开�?
  v = v.replace(/^0/g, '')
  // 数字超过100，赋值成最大�?00
  v = v.replace(/^[1-9]\d\d{1,3}$/, '100')
  // 返回结果
  return v
}

/**
 * 验证百分比（可以小数�?
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyNumberPercentageFloat(val: string): string {
  let v = verifyNumberIntegerAndFloat(val)
  // 数字超过100，赋值成最大�?00
  v = v.replace(/^[1-9]\d\d{1,3}$/, '100')
  // 超过100之后不给再输入�?
  v = v.replace(/^100\.$/, '100')
  // 返回结果
  return v
}

/**
 * 小数或整�?不可以负�?
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyNumberIntegerAndFloat(val: string) {
  // 匹配空格
  let v = val.replace(/(^\s*)|(\s*$)/g, '')
  // 只能是数字和小数点，不能是其他输�?
  v = v.replace(/[^\d.]/g, '')
  // �?开始只能输入一�?
  v = v.replace(/^0{2}$/g, '0')
  // 保证第一位只能是数字，不能是�?
  v = v.replace(/^\./g, '')
  // 小数只能出现1�?
  v = v.replace('.', '$#$').replace(/\./g, '').replace('$#$', '.')
  // 小数点后面保�?�?
  v = v.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3')
  // 返回结果
  return v
}

/**
 * 正整数验�?
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifiyNumberInteger(val: string) {
  // 匹配空格
  let v = val.replace(/(^\s*)|(\s*$)/g, '')
  // 去掉 '.' , 防止贴贴的时候出现问�?�?0.1.12.12
  v = v.replace(/[\.]*/g, '')
  // 去掉�?0 开始后面的�? 防止贴贴的时候出现问�?�?00121323
  v = v.replace(/(^0[\d]*)$/g, '0')
  // 首位�?,只能出现一�?
  v = v.replace(/^0\d$/g, '0')
  // 只匹配数�?
  v = v.replace(/[^\d]/g, '')
  // 返回结果
  return v
}

/**
 * 去掉中文及空�?
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyCnAndSpace(val: string) {
  // 匹配中文与空�?
  let v = val.replace(/[\u4e00-\u9fa5\s]+/g, '')
  // 匹配空格
  v = v.replace(/(^\s*)|(\s*$)/g, '')
  // 返回结果
  return v
}

/**
 * 去掉英文及空�?
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyEnAndSpace(val: string) {
  // 匹配英文与空�?
  let v = val.replace(/[a-zA-Z]+/g, '')
  // 匹配空格
  v = v.replace(/(^\s*)|(\s*$)/g, '')
  // 返回结果
  return v
}

/**
 * 禁止输入空格
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyAndSpace(val: string) {
  // 匹配空格
  let v = val.replace(/(^\s*)|(\s*$)/g, '')
  // 返回结果
  return v
}

/**
 * 金额�?`,` 区分开
 * @param val 当前值字符串
 * @returns 返回处理后的字符�?
 */
export function verifyNumberComma(val: string) {
  // 调用小数或整�?不可以负�?方法
  let v: any = verifyNumberIntegerAndFloat(val)
  // 字符串转成数�?
  v = v.toString().split('.')
  // \B 匹配非单词边界，两边都是单词字符或者两边都是非单词字符
  v[0] = v[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',')
  // 数组转字符串
  v = v.join('.')
  // 返回结果
  return v
}

/**
 * 匹配文字变色（搜索时�?
 * @param val 当前值字符串
 * @param text 要处理的字符串�?
 * @param color 搜索到时字体高亮颜色
 * @returns 返回处理后的字符�?
 */
export function verifyTextColor(val: string, text = '', color = 'red') {
  // 返回内容，添加颜�?
  let v = text.replace(new RegExp(val, 'gi'), `<span style='color: ${color}'>${val}</span>`)
  // 返回结果
  return v
}

/**
 * 数字转中文大�?
 * @param val 当前值字符串
 * @param unit 默认：仟佰拾亿仟佰拾万仟佰拾元角�?
 * @returns 返回处理后的字符�?
 */
export function verifyNumberCnUppercase(val: any, unit = '仟佰拾亿仟佰拾万仟佰拾元角分', v = '') {
  // 当前内容字符串添�?2�?，为什�??
  val += '00'
  // 返回某个指定的字符串值在字符串中首次出现的位置，没有出现，则该方法返�?-1
  let lookup = val.indexOf('.')
  // substring：不包含结束下标内容，substr：包含结束下标内�?
  if (lookup >= 0) val = val.substring(0, lookup) + val.substr(lookup + 1, 2)
  // 根据内容 val 的长度，截取返回对应大写
  unit = unit.substr(unit.length - val.length)
  // 循环截取拼接大写
  for (let i = 0; i < val.length; i++) {
    v += '零壹贰叁肆伍陆柒捌玖'.substr(val.substr(i, 1), 1) + unit.substr(i, 1)
  }
  // 正则处理
  v = v
    .replace(/零角零分$/, '�?)
    .replace(/零[仟佰拾]/g, '�?)
    .replace(/零{2,}/g, '�?)
    .replace(/�?[亿|万])/g, '$1')
    .replace(/�?�?, '�?)
    .replace(/亿零{0,3}�?, '�?)
    .replace(/^�?, '零元')
  // 返回结果
  return v
}

/**
 * 手机号码
 * @param val 当前值字符串
 * @returns 返回 true: 手机号码正确
 */
export function verifyPhone(val: string) {
  // false: 手机号码不正�?
  if (!/^((12[0-9])|(13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0|1,5-9]))\d{8}$/.test(val)) return false
  // true: 手机号码正确
  else return true
}

/**
 * 国内电话号码
 * @param val 当前值字符串
 * @returns 返回 true: 国内电话号码正确
 */
export function verifyTelPhone(val: string) {
  // false: 国内电话号码不正�?
  if (!/\d{3}-\d{8}|\d{4}-\d{7}/.test(val)) return false
  // true: 国内电话号码正确
  else return true
}

/**
 * 登录账号 (字母开头，允许5-16字节，允许字母数字下划线)
 * @param val 当前值字符串
 * @returns 返回 true: 登录账号正确
 */
export function verifyAccount(val: string) {
  // false: 登录账号不正�?
  if (!/^[a-zA-Z][a-zA-Z0-9_]{4,15}$/.test(val)) return false
  // true: 登录账号正确
  else return true
}

/**
 * 密码 (以字母开头，长度�?~16之间，只能包含字母、数字和下划�?
 * @param val 当前值字符串
 * @returns 返回 true: 密码正确
 */
export function verifyPassword(val: string) {
  // false: 密码不正�?
  if (!/^[a-zA-Z]\w{5,15}$/.test(val)) return false
  // true: 密码正确
  else return true
}

/**
 * 混合密码 (字母+数字+可选特殊字符，长度�?-16之间)
 * @param val 当前值字符串
 * @returns 返回 true: 强密码正�?
 */
export function verifyPasswordHybrid(val: string) {
  // false: 混合密码不正�?
  if (!/^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d!@#$%^&.*]{6,16}$/.test(val)) return false
  // true: 混合密码正确
  else return true
}

/**
 * 强密�?(字母+数字+特殊字符，长度在6-16之间)
 * @param val 当前值字符串
 * @returns 返回 true: 强密码正�?
 */
export function verifyPasswordPowerful(val: string) {
  // false: 强密码不正确
  if (!/^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&\.*]+$)(?![a-zA-z\d]+$)(?![a-zA-z!@#$%^&\.*]+$)(?![\d!@#$%^&\.*]+$)[a-zA-Z\d!@#$%^&\.*]{6,16}$/.test(val))
    return false
  // true: 强密码正�?
  else return true
}

/**
 * 密码强度
 * @param val 当前值字符串
 * @description 弱：纯数字，纯字母，纯特殊字�?
 * @description 中：字母+数字，字�?特殊字符，数�?特殊字符
 * @description 强：字母+数字+特殊字符
 * @returns 返回处理后的字符串：弱、中、强
 */
export function verifyPasswordStrength(val: string) {
  let v = ''
  // 弱：纯数字，纯字母，纯特殊字�?
  if (/^(?:\d+|[a-zA-Z]+|[!@#$%^&\.*]+){6,16}$/.test(val)) v = '�?
  // 中：字母+数字，字�?特殊字符，数�?特殊字符
  if (/^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&\.*]+$)[a-zA-Z\d!@#$%^&\.*]{6,16}$/.test(val)) v = '�?
  // 强：字母+数字+特殊字符
  if (/^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&\.*]+$)(?![a-zA-z\d]+$)(?![a-zA-z!@#$%^&\.*]+$)(?![\d!@#$%^&\.*]+$)[a-zA-Z\d!@#$%^&\.*]{6,16}$/.test(val))
    v = '�?
  // 返回结果
  return v
}

/**
 * IP地址
 * @param val 当前值字符串
 * @returns 返回 true: IP地址正确
 */
export function verifyIPAddress(val: string) {
  // false: IP地址不正�?
  if (
    !/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/.test(
      val
    )
  )
    return false
  // true: IP地址正确
  else return true
}

/**
 * 邮箱
 * @param val 当前值字符串
 * @returns 返回 true: 邮箱正确
 */
export function verifyEmail(val: string) {
  // false: 邮箱不正�?
  if (
    !/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(
      val
    )
  )
    return false
  // true: 邮箱正确
  else return true
}

/**
 * 身份�?
 * @param val 当前值字符串
 * @returns 返回 true: 身份证正�?
 */
export function verifyIdCard(val: string) {
  // false: 身份证不正确
  if (!/^[1-9]\d{5}(18|19|20)\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/.test(val)) return false
  // true: 身份证正�?
  else return true
}

/**
 * 姓名
 * @param val 当前值字符串
 * @returns 返回 true: 姓名正确
 */
export function verifyFullName(val: string) {
  // false: 姓名不正�?
  if (!/^[\u4e00-\u9fa5]{1,6}(·[\u4e00-\u9fa5]{1,6}){0,2}$/.test(val)) return false
  // true: 姓名正确
  else return true
}

/**
 * 邮政编码
 * @param val 当前值字符串
 * @returns 返回 true: 邮政编码正确
 */
export function verifyPostalCode(val: string) {
  // false: 邮政编码不正�?
  if (!/^[1-9][0-9]{5}$/.test(val)) return false
  // true: 邮政编码正确
  else return true
}

/**
 * url 处理
 * @param val 当前值字符串
 * @returns 返回 true: url 正确
 */
export function verifyUrl(val: string) {
  // false: url不正�?
  if (
    !/^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/i.test(
      val
    )
  )
    return false
  // true: url正确
  else return true
}

/**
 * 车牌�?
 * @param val 当前值字符串
 * @returns 返回 true：车牌号正确
 */
export function verifyCarNum(val: string) {
  // false: 车牌号不正确
  if (
    !/^(([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z](([0-9]{5}[DF])|([DF]([A-HJ-NP-Z0-9])[0-9]{4})))|([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳使领]))$/.test(
      val
    )
  )
    return false
  // true：车牌号正确
  else return true
}
