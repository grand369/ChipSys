<template>
	<view class="app-page app-pwd">
		<!-- 自定义顶部导航栏 -->
		<u-navbar
			:autoBack="false"
			:placeholder="true"
			@leftClick="onBack"
		>
		</u-navbar>
		
		<view class="app-pwd__title">重新设置密码</view>
		<view class="u-flex app-pwd__desc">为您的账�?{{formatAccount}} 设置一个新密码</view>
		
		<u--form ref="pwdForm" :model="account"  labelWidth="70" errorType="toast" class="app-pwd__form">
			<u-form-item
				v-if="isMobile"
				label="验证�?
				prop="smsCode"
				borderBottom
			>
				<u-<!-- #ifdef APP-NVUE -->-<!-- #endif -->input
					key="smsCode"
					placeholder="请输入验证码"
					border="none"
					clearable
					v-model="account.smsCode"
				>
					<template slot="suffix">
						<u-code
							ref="smsCode"
							seconds="60"
							:keepRunning="true"
							uniqueKey="page-change-password"
							startText="发送验证码"
							@change="codeChange"
						></u-code>
						<u--text type="primary" :text="codeTips"  @click="onGetCode"></u--text>
					</template>
				</u-<!-- #ifdef APP-NVUE -->-<!-- #endif -->input>
			</u-form-item>
			<u-form-item
				v-else
				label="当前密码"
				prop="currentPassword"
				borderBottom
			>
				<u--input
					key="currentPassword"
					type="password"
					placeholder="当前密码"
					border="none"
					clearable
					v-model="account.currentPassword"
				></u--input>
			</u-form-item>
			<u-form-item
				label="新密�?
				prop="newPassword"
				borderBottom
			>
				<u--input
					key="newPassword"
					type="password"
					placeholder="6-30位密�?
					border="none"
					clearable
					v-model="account.newPassword"
				></u--input>
			</u-form-item>
			<view class="app-pwd__form__submit">
				<u-button type="primary" @click="onSave">保存新密�?/u-button>
			</view>
		</u--form>
	</view>
</template>

<script>
	import { mapState, mapActions, mapGetters } from 'vuex'
	import { api } from '@/api/api-account.js'
	import { showToast } from '@/libs/utils/util.js'
	
	export default {
		name: 'page-account-change-password',
		data() {
			return {
				codeTips: '',
				account:{
					smsCode:'',
					newPassword: '',
					currentPassword: ''
				},
				rules: {
					// 短信验证�?
					smsCode: [
						{
							required: true,
							message: '请输入短信验证码',
							//trigger: ['change','blur']
						}
					],
					// 当前密码
					currentPassword: [
						{
							required: true,
							message: '请输入当前密�?,
						}
					],
					// 新密�?
					newPassword: [
						{
							required: true,
							message: '请输入新密码',
						},
						{
							type: 'string',
							min: 6,
							message: '新密码不低于6�?,
						}
					],
				}
			}
		},
		onLoad() {
			
		},
		onReady(){
			this.$refs.pwdForm.setRules(this.rules)
		},
		computed: {
			...mapGetters('account', ['isLogin', 'userName', 'mobile']),
			isMobile(){
				return uni.$u.test.mobile(this.mobile)
			},
			formatAccount(){
				return this.isMobile ? `${this.mobile.substr(0, 3)}****${this.mobile.substr(7)}` : this.userName
			},
		},
		methods: {
			// 返回
			back(){
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
			//保存新密�?
			onSave(){
				const me = this
				this.$refs.pwdForm.validate().then(async valid => {
					const res = await api.changePassword({
						isPwd: !this.isMobile,
						phoneNumber: this.mobile,
						verificationCode: this.account.smsCode,
						currentPassword: this.account.currentPassword,
						newPassword: this.account.newPassword
					})
					if(res?.success){
						// #ifdef APP-NVUE
						const eventChannel = this.$scope.eventChannel
						// #endif
						// #ifndef APP-NVUE
						const eventChannel = this.getOpenerEventChannel()
						// #endif
						eventChannel.emit('save')
						//#ifdef H5
						showToast('修改成功', function(){
							me.back()
						}, 500)
						//#endif
						//#ifndef H5
						me.back()
						showToast('修改成功', null, 500)
						//#endif
					}
				}).catch(errors => {
				})
			},
			//获取短信验证�?
			async onGetCode(){
				//检查手机号
				if(!this.mobile) {
					showToast('手机号不能为空！')
					return
				}
				
				if(!this.$refs.smsCode?.canGetCode) {
					return
				}
				
				const res = await api.sendSmsCodeForChangePassword({
					mobile: this.mobile
				})
				if(res?.result?.success === true || (!res?.result && res?.success)){
					// 通知验证码组件内部开始倒计�?
					this.$refs.smsCode.start()
				}
			},
			//点击返回
			onBack() {
				this.back()
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
	.app-pwd{
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
