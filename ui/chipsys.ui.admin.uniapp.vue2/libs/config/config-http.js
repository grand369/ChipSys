import store from 'store'
import { showToast } from '../../libs/utils/util.js'
import { api as apiAccount } from '@/api/api-account.js'

/* config 为默认全局配置*/
export const setConfig = (config) => {
	config.custom = {
		auth: true,
		catch: true,
		// 加载中配�?
		loading: {
			enable: false,
			msg: '加载�?
		},
		// 成功配置
		ok: {
			enable: false,
			msg: '',
			complete: null
		},
		// 错误配置
		error: {
			enable: false,
			msg: '',
			complete: null
		},
	}
	// 全局自定义验证器
	config.validateStatus = (statusCode) => {
		switch(statusCode){
			case 400:
				showToast('请求参数不正�?)
			break;
			case 401:
				/*
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
				*/
			break;
			case 404:
				showToast('请求地址不存�?)
			break;
			case 408:
				showToast('请求超时')
			break;
			case 500:
				showToast('请求异常')
			break;
		}
		return statusCode //statusCode != 400 || statusCode != 500
    }
	
	return config
}

// 请求拦截
export const requestConfig = async(config) => { // 可使用async await 做异步操�?
	config.data = config.data || {}
	if(config.custom?.auth) {
		let account = store.state.account
		if (!account?.token) { // 如果token不存在，return Promise.reject(config) 会取消本次请�?
			return Promise.reject(config)
		}
		
		// if (account.expireTime < Date.now()) {
		// 	await apiAccount.refreshToken({ token: account?.token })
		// }
		
		config.header.Authorization = 'Bearer ' + account.token
	}
	
	if (config.custom?.loading?.enable) {
		uni.showLoading({
			mask: true,
			title: config?.custom?.loading?.msg
		});
	}
	
	return config 
}

export const checkStatusCode = (statusCode) => {
	return [400, 401, 404, 408, 500].findIndex(d => d == statusCode) === -1
}

// 响应成功拦截
export const responseOk = async(response) => { // 可使用async await 做异步操�?
	// 自定义参�?
	const custom = response.config?.custom
	
	if (custom?.loading?.enable) {
		uni.hideLoading()
	}

	//const data = response.data && uni.$u.test.jsonString(response.data) ? JSON.parse(response.data) : response.data
	const data = response.data
	const error = custom?.error
	if (data && !data.success) { 
		// 失败提示
		if (error?.enable && checkStatusCode(response.statusCode)) {
			let msg = error?.msg || data?.msg
			showToast(msg, error?.complete)
		}

		// 如果需要catch返回，则进行reject
		if (custom?.catch) {
			return Promise.reject(data)
		} else {
			// 否则返回一个pending中的promise，请求不会进入catch�?
			// return new Promise(() => { })
			return data
		}
	}else{
		// 成功提示
		const ok = custom?.ok
		if(ok?.enable){
			showToast(ok?.msg, ok?.complete)
		}
	}
	
	return data === undefined ? {} : data
}
// 响应失败拦截
export const responseError = (response) => {
	// 自定义参�?
	const custom = response.config?.custom
	
	if (custom?.loading?.enable) {
		uni.hideLoading()
	}
	
	// 提示错误消息
	const error = custom?.error
	
	if(error?.enable  && checkStatusCode(response.statusCode)){
		const resError = response.data
		let msg = error?.msg || resError?.msg || response.errMsg
		showToast(msg, error?.complete)
	}
	
	// 对响应错误做点什�?
	return Promise.reject(response)
}
