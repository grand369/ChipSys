import store from 'store'
import httpApi from '../libs/config/http-api.js'
import { showToast } from '../libs/utils/util.js'

let lock = false
let promiseResultList = []

export const api = {
	//登录
	login(params, config = {}) {
		config = uni.$u.deepMerge({
			custom: { 
				auth: false,
				loading: {
					enable: true,
					msg: '登录�?
				},
				error: {
					enable: true
				}
			} 
		}, config)
		
		if(params.isPwd){
			return httpApi.post('/api/admin/auth/login', {
				"userName": params.mobile,
				"password": params.password
			}, config)
		} else {
			return httpApi.post('/api/admin/auth/mobile-login', {
				"mobile": params.mobile,
				"code": params.smsCode
			}, config)
		}
	},
	//获得短信验证�?
	getSmsCode(params, config = {}) {
		config = uni.$u.deepMerge({
			custom: { 
				auth: false,
				loading: {
					enable: true,
					msg: '正在获取验证�?
				},
				ok: {
					enable: true,
					msg: '验证码已发�?
				},
				error: {
					enable: true
				}
			} 
		}, config)
		
		const data = {
			mobile: params.mobile,
			codeId: params.code,
			captchaId: params.code
		}
		return httpApi.post('/api/admin/captcha/send-sms-code', data, config)
	},
	//修改密码-发送短信验证码
	sendSmsCodeForChangePassword(params, config = {}) {
		config = uni.$u.deepMerge({
			custom: {
				loading: {
					enable: true,
					msg: '正在发送验证码'
				},
				ok: {
					enable: true,
					msg: '验证码已发�?
				},
				error: {
					enable: true
				}
			},
		}, config)
		config.params = params
		
		return httpApi.post('/api/admin/captcha/send-sms-code', null, config)
	},
	//修改密码
	changePassword(params, config = {}) {
		config = uni.$u.deepMerge({
			custom: {
				loading: {
					enable: true,
					msg: '保存�?
				},
				error: {
					enable: true
				}
			},
		}, config)
		
		if(params.isPwd){
			return httpApi.put('api/admin/user/change-password', {
				oldPassword: params.currentPassword,
				newPassword: params.newPassword,
				confirmPassword: params.newPassword
			}, config)
		}else{
			return httpApi.put('api/admin/user/change-password', {
				oldPassword: params.currentPassword,
				newPassword: params.newPassword,
				confirmPassword: params.newPassword
			}, config)
		}
	},
	//获得手机�?
	getPhoneNumber(params, config = {}) {
		config = uni.$u.deepMerge({
			custom: {
				auth: false,
				error: {
					enable: false
				}
			},
		}, config)
		
		return httpApi.post('', params, config)
	},
	//刷新token
	refreshToken(params, config = {}) {
		return new Promise((resolve, reject) => {
			promiseResultList.push({
				resolve,
				reject
			})
			if (!lock) {
				lock = true
				
				config = uni.$u.deepMerge({
					custom: {
						auth: false,
						catch: false,
						error: {
							enable: false
						}
					},
				}, config)
				config.params = params
				
				httpApi.get('/api/admin/auth/refresh', config).then(res => {
					if(res?.success){
						store.commit('account/updateAuth', res.result, { root: true })
						while (promiseResultList.length) {
							promiseResultList.shift().resolve(res.data)
						}
					}else{
						while (promiseResultList.length) {
							promiseResultList.shift().reject()
						}
					}
					lock = false
					if(!res?.success){
						const lastPage = uni.$u.page()
						showToast('您还没有登录', function(){
							//判断当前界面是否已是登录界面
							const currentPage = uni.$u.page()
							const loginPage = '/pages/account/login'
							if(lastPage === currentPage && currentPage !== loginPage){
								uni.navigateTo({
									url: loginPage
								})
							}
						},1000)
					}
				}).catch(err => {
					while (promiseResultList.length) {
						promiseResultList.shift().reject(err)
					}
					lock = false
				})
			}
		})
	},
}
