import { useUserInfo } from '/@/stores/userInfo'
import { judementSameArr } from '/@/utils/arrayOperation'

/**
 * 单个权限验证
 * @param value 权限�?
 * @returns 有权限，返回 `true`，反之则�?
 */
export function auth(value: string): boolean {
  const stores = useUserInfo()
  return stores.userInfos.authBtnList.some((v: string) => v === value)
}

/**
 * 多个权限验证，满足一个则�?true
 * @param value 权限�?
 * @returns 有权限，返回 `true`，反之则�?
 */
export function auths(value: Array<string>): boolean {
  let flag = false
  const stores = useUserInfo()
  stores.userInfos.authBtnList.map((val: string) => {
    value.map((v: string) => {
      if (val === v) flag = true
    })
  })
  return flag
}

/**
 * 多个权限验证，全部满足则�?true
 * @param value 权限�?
 * @returns 有权限，返回 `true`，反之则�?
 */
export function authAll(value: Array<string>): boolean {
  const stores = useUserInfo()
  return judementSameArr(value, stores.userInfos.authBtnList)
}
