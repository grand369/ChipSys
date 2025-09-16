import { AuthApi } from '/@/api/admin/Auth'
import { memberAuthApi } from '/@/api/client'
import { merge } from 'lodash-es'
import { Local, Session } from '/@/utils/storage'
import { useThemeConfig } from '/@/stores/themeConfig'
import Watermark from '/@/utils/watermark'
import { TokenInfo } from '/@/api/admin/data-contracts'

export const adminTokenKey = 'admin-token'
export const adminTokenInfoKey = 'admin-token-info'

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
  state: (): UserInfosState => ({
    userInfos: {
      token: Local.get(adminTokenKey) || '',
      userName: '',
      photo: '',
      time: 0,
      roles: [],
      authBtnList: [],
    },
  }),
  actions: {
    async setUserInfos() {
      const userInfos = <UserInfos>await this.getUserInfo().catch(() => {})
      if (userInfos && Object.keys(userInfos).length > 0) {
        merge(this.userInfos, userInfos)
      }
    },
    setUserName(userName: string) {
      this.userInfos.userName = userName
    },
    setPhoto(photo: string) {
      this.userInfos.photo = photo
    },
    setToken(token: string) {
      this.userInfos.token = token
      Local.set(adminTokenKey, token)
      // 同时设置到adminTokenInfoKey，确保getToken能正确读取
      const tokenInfo = { accessToken: token } as TokenInfo
      Local.set(adminTokenInfoKey, tokenInfo)
    },
    setTokenInfo(tokenInfo: TokenInfo | undefined) {
      this.userInfos.token = tokenInfo?.accessToken as string
      Local.set(adminTokenInfoKey, tokenInfo)
    },
    getToken() {
      const tokenInfo = this.getTokenInfo()
      this.userInfos.token = tokenInfo?.accessToken as string
      return tokenInfo?.accessToken
    },
    getTokenInfo() {
      const tokenInfo = Local.get(adminTokenInfoKey) as TokenInfo
      return tokenInfo
    },
    removeTokenInfo() {
      this.userInfos.token = ''
      Local.remove(adminTokenInfoKey)
      Local.remove(adminTokenKey)
    },
    clear() {
      this.removeTokenInfo()
      window.requests = []
      window.location.reload()
    },
    //查询用户信息
    async getUserInfo() {
      try {
        // 检查是否为会员用户
        const memberInfo = Session.get('member_info')
        if (memberInfo) {
          // 会员用户信息获取
          const userInfos = {} as any
          userInfos.userName = memberInfo.nickName || memberInfo.realName || '会员用户'
          userInfos.photo = memberInfo.wechatInfo?.avatar || ''
          userInfos.time = new Date().getTime()
          userInfos.roles = ['member'] // 会员角色
          userInfos.authBtnList = [] // 会员暂时不需要权限按钮
          
          // 水印文案
          const storesThemeConfig = useThemeConfig()
          if (storesThemeConfig.themeConfig.isWatermark) {
            storesThemeConfig.themeConfig.watermarkText = '会员用户'
            Watermark.set(storesThemeConfig.themeConfig.watermarkText)
            Local.remove('themeConfig')
            Local.set('themeConfig', storesThemeConfig.themeConfig)
          } else {
            Watermark.del()
          }
          
          return userInfos
        } else {
          // 管理员用户信息获取
          const profile = await new AuthApi().getUserProfile().catch(() => {})
          const permissions = await new AuthApi().getUserPermissions().catch(() => {})

          const userInfos = {} as any
          const user = profile?.data
          if (profile?.success) {
            userInfos.userName = user?.nickName || user?.name
            userInfos.photo = user?.avatar ? user?.avatar : ''
            userInfos.time = new Date().getTime()
            userInfos.roles = []
          }

          if (permissions?.success) {
            userInfos.authBtnList = permissions.data?.permissions
          }

          // 水印文案
          const storesThemeConfig = useThemeConfig()
          if (storesThemeConfig.themeConfig.isWatermark) {
            storesThemeConfig.themeConfig.watermarkText = user?.watermarkText || '中台Admin'
            Watermark.set(storesThemeConfig.themeConfig.watermarkText)
            Local.remove('themeConfig')
            Local.set('themeConfig', storesThemeConfig.themeConfig)
          } else {
            Watermark.del()
          }

          return userInfos
        }
      } catch (err) {
        console.error('获取用户信息失败:', err)
        throw err
      }
    },
  },
})
