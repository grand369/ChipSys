<template>
	<view class="app-page app-login">
		<!-- 自定义顶部导航栏 -->
		<u-navbar
			:autoBack="false"
			:placeholder="true"
			leftIcon="close"
			@leftClick="onBack"
		>
			<!-- <view slot="right">注册</view> -->
		</u-navbar>
		<view class="app-login__title">{{isPwd ? '账号密码登录' : '欢迎登录中台Admin'}}</view>
		<view class="app-login__desc">{{isPwd ? '使用已注册手机号登录' : '手机号码未注册将自动创建新账�?}}</view>
		
		<u--form ref="loginForm" :model="account"  labelWidth="0" errorType="toast" class="app-login__form">
			<u-form-item
				v-if="isPwd"
				prop="mobile"
				borderBottom
			>
				<u--input
					v-model="account.mobile"
					key="pwd-mobile"
					placeholder="请输入手机号"
					border="none"
					clearable
				></u--input>
			</u-form-item>
			<u-form-item
				v-else
				prop="mobile"
				borderBottom
			>
				<u--input
					v-model="account.mobile"
					key="mobile"
					type="number"
					placeholder="请输入手机号"
					border="none"
					maxlength="11"
					clearable
					@confirm="onShowImgCode"
				></u--input>
			</u-form-item>
			<u-form-item
				v-if="isPwd"
				prop="password"
				borderBottom
			>
				<u--input
					key="password"
					type="password"
					placeholder="请输入密�?
					border="none"
					clearable
					v-model="account.password"
					@confirm="onLogin"
				></u--input>
			</u-form-item>
			<u-form-item
				v-if="!isPwd"
				prop="smsCode"
				borderBottom
			>
				<u-<!-- #ifdef APP-NVUE -->-<!-- #endif -->input
					key="smsCode"
					placeholder="请输入短信验证码"
					border="none"
					clearable
					v-model="account.smsCode"
					@confirm="onLogin"
				>
					<template slot="suffix">
						<u-code
							ref="smsCode"
							seconds="60"
							:keepRunning="true"
							uniqueKey="page-login"
							@change="codeChange"
						></u-code>
						<u--text type="primary" :text="codeTips"  @click="onShowImgCode"></u--text>
					</template>
				</u-<!-- #ifdef APP-NVUE -->-<!-- #endif -->input>
			</u-form-item>
			<view class="app-login__form__submit">
				<u-button type="primary" @click="onLogin">立即登录</u-button>
			</view>
			<view class="u-flex u-flex-between app-login__form__switch"> 
				<text @click="onSwitchLogin" style="font-size: 14px;">{{isPwd ? '验证码登�? : '密码登录'}}</text>
				<!-- #ifdef MP-WEIXIN -->
				<!-- <view>
					<button 
						type="default"
						open-type="getPhoneNumber"
						plain
						style="border:unset;padding:0px;line-height: unset;font-size: 14px;"
						@getphonenumber="onGetPhoneNumber"
					>一键登�?/button>
				</view> -->
				<!-- #endif -->
			</view>
			<!-- <u-form-item
				prop="isAgree"
				class="app-login__form__agree"
			>
				<view class="u-flex">
					<u-checkbox-group v-model="account.isAgree">
						<u-checkbox name="agree" label="我已阅读并同�? shape="circle" size="12" labelSize="10" labelColor="#909193"></u-checkbox>
					</u-checkbox-group>
					<navigator url="/pages/account/agreement" class="app-login__form__agree__agreement">
						<view>《用户协议�?/view>
					</navigator>
				</view>
			</u-form-item> -->
		</u--form>
		
		<u-modal 
			ref="codeModal" 
			title="请输入图片验证码" 
			:show="showImgCode" 
			:zoom="false" 
			showCancelButton 
			@confirm="onGetCode" 
			@cancel="showImgCode=false"
		>
			<view>
				<u-<!-- #ifdef APP-NVUE -->-<!-- #endif -->input
					placeholder="看不清？点击图片换一�?
					clearable
					maxlength="4"
					v-model="account.imgCode"
					@confirm="onGetCode"
				>
					<template slot="suffix">
						<u--image :src="imgCodeSrc" width="72" height="38" :showLoading="true" @click="onChangeImgCode" style="height:100%;">
							<template v-slot:loading>
								<u-loading-icon></u-loading-icon>
							</template>
						</u--image>
					</template>
				</u-<!-- #ifdef APP-NVUE -->-<!-- #endif -->input>
			</view>
		</u-modal>
	</view>
</template>

<script>
	import { mapActions, mapGetters } from 'vuex'
	import { ssoUrl } from '@/libs/config/config.js'
	import { api as apiAccount } from '@/api/api-account.js'
	import { showToast } from '@/libs/utils/util.js'
	
	export default {
		name: 'page-account-login',
		data() {
			return {
				routeData: null,
				isPwd: true,
				showImgCode: false,
				imgCodeSrc:'',
				codeTips: '',
				hasImgCode: false, //有图形验证码
				account:{
					mobile: 'admin',
					password: '123asd',
					smsCode:'',
					imgCode:'',
					// isAgree: [],
					code: ''
				},
				rules: {
					// 手机�?
					mobile: [
						{
							required: true,
							message: '请输入手机号',
							trigger: ['change']
							//trigger: ['change','blur']
						},
						{
							validator: (rule, value, callback) => {
								return this.isPwd || uni.$u.test.mobile(value);
							},
							message: '手机号码不正�?,
							trigger: ['change']
							//trigger: ['change','blur']
						}
					],
					// 密码
					password: [
						{
							required: true,
							message: '请输入密�?,
							//trigger: ['change','blur']
						}
					],
					// 短信验证�?
					smsCode: [
						{
							required: true,
							message: '请输入短信验证码',
							//trigger: ['change','blur']
						}
					],
					//是否同意
					// isAgree: [
					// 	{
					// 		required: true,
					// 		type: 'array',
					// 		message: '请阅读并勾选下方协�?,
					// 		//trigger: ['change']
					// 	}
					// ]
				}
			}
		},
		onLoad() {
			// #ifdef APP-NVUE
			const eventChannel = this.$scope.eventChannel
			// #endif
			
			// #ifndef APP-NVUE
			const eventChannel = this.getOpenerEventChannel()
			// #endif
			
			eventChannel.on('login', (data) => {
				if(data?.route){
					this.routeData = data.route
				}
			})
		},
		onReady(){
			this.$refs.loginForm.setRules(this.rules)
		},
		computed: {
			...mapGetters('account', ['isLogin'])
		},
		methods: {
			...mapActions('account', {
				login: 'login'
			}),
			// 返回
			back(){
				//已登录且配置路由
				if(this.isLogin && this.routeData){
					uni.$u.route(this.routeData)
					return
				}
				
				const pages = uni.$u.pages()
				if(pages.length > 1 && (!pages[0].$vm?._data?._auth || (pages[0].$vm?._data?._auth && this.isLogin))){
					uni.$u.route({
						type: 'back',
					})
				} else {
					uni.$u.route({
						type: 'tab',
						url: '/pages/tabbar/my/my'
					})
				}
			},
			codeChange(text) {
				this.codeTips = text
			},
			//立即登录
			onLogin(){
				const me = this
				this.$refs.loginForm.validate().then(async valid => {
					//#ifndef H5
					uni.login({
						success: async function(uniRes) {
							me.account.code = uniRes.code
					//#endif
							let data = uni.$u.deepClone(me.account)
							data.isPwd = me.isPwd
							const res = await me.login(data)
							if(res?.success){
								//#ifdef H5
								showToast('登录成功', function(){
									me.back()
								}, 500)
								//#endif
								//#ifndef H5
								me.back()
								showToast('登录成功', null, 500)
								//#endif
							}
					//#ifndef H5
						}
					})
					//#endif
				}).catch(errors => {
					
				})
			},
			//获得用户协议
			onGetAgreement(){
				
			},
			//显示图形验证�?
			onShowImgCode(){
				const me = this
				this.$refs.loginForm.validateField(['mobile'], function(errors){
					if(errors.length){
						uni.$u.toast(errors[0].message)
						return
					}
					
					if(me.$refs.smsCode?.canGetCode) {
						if(me.hasImgCode){
							me.onChangeImgCode()
							me.showImgCode = true
						}else{
							me.onGetCode()
						}
					}
				})
			},
			//更改图形验证�?
			onChangeImgCode(){
				if(this.hasImgCode){
					this.account.imgCode = ''
					this.imgCodeSrc =  ssoUrl + '/account/verifyimage?mobile=' + this.account.mobile + '&time=' + new Date().getTime()
				}
			},
			//获取短信验证�?
			async onGetCode(){
				if(!this.$refs.smsCode?.canGetCode) {
					return
				}
				
				if(this.hasImgCode && !this.account.imgCode){
					uni.$u.toast('请输入图片验证码');
					return
				}

				const me = this
				//#ifndef H5
				uni.login({
					provider: 'weixin',
					success: async function(loginRes) {
						me.account.code = loginRes.code
				//#endif
						const res = await apiAccount.getSmsCode(me.account)
						if(res?.result?.success === true || (!res?.result && res?.success)){
							me.showImgCode = false
							// 通知验证码组件内部开始倒计�?
							me.$refs.smsCode.start()
						} else {
							me.onChangeImgCode()
						}
				//#ifndef H5
					}
				})
				//#endif
			},
			//切换登录
			onSwitchLogin(){
				this.isPwd = !this.isPwd;
			},
			//点击返回
			onBack() {
				this.back()
			},
			//获取手机�?
			onGetPhoneNumber({detail}){
				if(detail?.errMsg === 'getPhoneNumber:ok'){
					uni.login({
						provider: 'weixin',
						success: async function(loginRes) {
							apiAccount.getPhoneNumber({
								code: loginRes.code,
								encryptedData: detail.encryptedData,
								iv:  detail.iv
							}).then(r => {
								
							}).catch(error => {
								
							})
						}
					})
				}
			}
		}
	}
</script>

<!-- 添加界面背景�?-->
<!-- <style lang="scss">
	page {
		background-color: $u-bg-color;
	}
</style> -->
<style lang="scss" scoped>
	.app-page{
		padding:30px;
	}
	.app-login{
		&__title{
			color: $u-main-color;
			font-size: 22px;
			font-weight: 600;
		}
		&__desc{
			color:$u-tips-color;
			font-size: 14px;
			margin-top: 5px;
			margin-bottom: 50px;
		}
		&__form{
			&__submit{
				margin-bottom: 10px;
				margin-top:20px;
			}
			&__switch{
				margin-top: 20px;
				margin-bottom: 20px;
			}
			&__agree{
				&__agreement{
					color:rgb(41, 121, 255);
					font-size:10px;
					margin-left:5px;
				}
			}
		}
	}
</style>
