/**
 * 时间日期转换
 * @param date 当前时间，new Date() 格式
 * @param format 需要转换的时间格式字符�?
 * @description format 字符串随意，�?`YYYY-mm、YYYY-mm-dd`
 * @description format 季度�?YYYY-mm-dd HH:MM:SS QQQQ"
 * @description format 星期�?YYYY-mm-dd HH:MM:SS WWW"
 * @description format 几周�?YYYY-mm-dd HH:MM:SS ZZZ"
 * @description format 季度 + 星期 + 几周�?YYYY-mm-dd HH:MM:SS WWW QQQQ ZZZ"
 * @returns 返回拼接后的时间字符�?
 */
export function formatDate(date: Date, format: string): string {
  let we = date.getDay() // 星期
  let z = getWeek(date) // �?
  let qut = Math.floor((date.getMonth() + 3) / 3).toString() // 季度
  const opt: { [key: string]: string } = {
    'Y+': date.getFullYear().toString(), // �?
    'm+': (date.getMonth() + 1).toString(), // �?月份�?开始，�?1)
    'd+': date.getDate().toString(), // �?
    'H+': date.getHours().toString(), // �?
    'M+': date.getMinutes().toString(), // �?
    'S+': date.getSeconds().toString(), // �?
    'q+': qut, // 季度
  }
  // 中文数字 (星期)
  const week: { [key: string]: string } = {
    '0': '�?,
    '1': '一',
    '2': '�?,
    '3': '�?,
    '4': '�?,
    '5': '�?,
    '6': '�?,
  }
  // 中文数字（季度）
  const quarter: { [key: string]: string } = {
    '1': '一',
    '2': '�?,
    '3': '�?,
    '4': '�?,
  }
  if (/(W+)/.test(format))
    format = format.replace(RegExp.$1, RegExp.$1.length > 1 ? (RegExp.$1.length > 2 ? '星期' + week[we] : '�? + week[we]) : week[we])
  if (/(Q+)/.test(format)) format = format.replace(RegExp.$1, RegExp.$1.length == 4 ? '�? + quarter[qut] + '季度' : quarter[qut])
  if (/(Z+)/.test(format)) format = format.replace(RegExp.$1, RegExp.$1.length == 3 ? '�? + z + '�? : z + '')
  for (let k in opt) {
    let r = new RegExp('(' + k + ')').exec(format)
    // 若输入的长度不为1，则前面补零
    if (r) format = format.replace(r[1], RegExp.$1.length == 1 ? opt[k] : opt[k].padStart(RegExp.$1.length, '0'))
  }
  return format
}

/**
 * 获取当前日期是第几周
 * @param dateTime 当前传入的日期�?
 * @returns 返回第几周数字�?
 */
export function getWeek(dateTime: Date): number {
  let temptTime = new Date(dateTime.getTime())
  // 周几
  let weekday = temptTime.getDay() || 7
  // �?+5�?周六
  temptTime.setDate(temptTime.getDate() - weekday + 1 + 5)
  let firstDay = new Date(temptTime.getFullYear(), 0, 1)
  let dayOfWeek = firstDay.getDay()
  let spendDay = 1
  if (dayOfWeek != 0) spendDay = 7 - dayOfWeek + 1
  firstDay = new Date(temptTime.getFullYear(), 0, 1 + spendDay)
  let d = Math.ceil((temptTime.valueOf() - firstDay.valueOf()) / 86400000)
  let result = Math.ceil(d / 7)
  return result
}

/**
 * 将时间转换为 `几秒前`、`几分钟前`、`几小时前`、`几天前`
 * @param param 当前时间，new Date() 格式或者字符串时间格式
 * @param format 需要转换的时间格式字符�?
 * @description param 10秒：  10 * 1000
 * @description param 1分：   60 * 1000
 * @description param 1小时�?60 * 60 * 1000
 * @description param 24小时�?0 * 60 * 24 * 1000
 * @description param 3天：   60 * 60* 24 * 1000 * 3
 * @returns 返回拼接后的时间字符�?
 */
export function formatPast(param: string | Date, format: string = 'YYYY-mm-dd'): string {
  // 传入格式处理、存储转换�?
  let t: any, s: number
  // 获取js 时间�?
  let time: number = new Date().getTime()
  // 是否是对�?
  typeof param === 'string' || 'object' ? (t = new Date(param).getTime()) : (t = param)
  // 当前时间�?- 传入时间�?
  time = Number.parseInt(`${time - t}`)
  if (time < 10000) {
    // 10秒内
    return '刚刚'
  } else if (time < 60000 && time >= 10000) {
    // 超过10秒少�?分钟�?
    s = Math.floor(time / 1000)
    return `${s}秒前`
  } else if (time < 3600000 && time >= 60000) {
    // 超过1分钟少于1小时
    s = Math.floor(time / 60000)
    return `${s}分钟前`
  } else if (time < 86400000 && time >= 3600000) {
    // 超过1小时少于24小时
    s = Math.floor(time / 3600000)
    return `${s}小时前`
  } else if (time < 259200000 && time >= 86400000) {
    // 超过1天少�?天内
    s = Math.floor(time / 86400000)
    return `${s}天前`
  } else {
    // 超过3�?
    let date = typeof param === 'string' || 'object' ? new Date(param) : param
    return formatDate(date, format)
  }
}

/**
 * 时间问候语
 * @param param 当前时间，new Date() 格式
 * @description param 调用 `formatAxis(new Date())` 输出 `上午好`
 * @returns 返回拼接后的时间字符�?
 */
export function formatAxis(param: Date): string {
  let hour: number = new Date(param).getHours()
  if (hour < 6) return '凌晨�?
  else if (hour < 9) return '早上�?
  else if (hour < 12) return '上午�?
  else if (hour < 14) return '中午�?
  else if (hour < 17) return '下午�?
  else if (hour < 19) return '傍晚�?
  else if (hour < 22) return '晚上�?
  else return '夜里�?
}
