type EnumType = {
  [key: string]: {
    name: string
    value: number | string
    desc: string
  }
}

// 下拉选项的接�?
interface DropdownOption {
  label: string
  value: string | number
}

/** 根据枚举 name 查找描述 */
export function getDescByName<T extends EnumType, K extends keyof T>(enumObj: T, key: K): string | undefined {
  return enumObj[key]?.desc || ''
}

/** 根据枚举 value 查找描述 */
export function getDescByValue<T extends EnumType>(enumObj: T, value: T[keyof T]['value']): string | undefined {
  for (const [key, item] of Object.entries(enumObj)) {
    if (item.value === value) {
      return item.desc
    }
  }
  return ''
}

/** 枚举转换为下拉选项列表（使用值作为value�?*/
export function toOptionsByValue<T extends EnumType>(enumObj: T, includeUnknown: boolean = false): DropdownOption[] {
  return Object.values(enumObj).reduce((options, item) => {
    if (includeUnknown || item.name !== 'Unknown') {
      options.push({ label: item.desc, value: item.value })
    }
    return options
  }, [] as DropdownOption[])
}

/** 转换为下拉选项列表（使用名称作为value�?*/
export function toOptionsByName<T extends EnumType>(enumObj: T, includeUnknown: boolean = false): DropdownOption[] {
  return Object.values(enumObj).reduce((options, item) => {
    if (includeUnknown || item.name !== 'Unknown') {
      options.push({ label: item.desc, value: item.name })
    }
    return options
  }, [] as DropdownOption[])
}

export default {
  getDescByName,
  getDescByValue,
  toOptionsByName,
  toOptionsByValue,
}
