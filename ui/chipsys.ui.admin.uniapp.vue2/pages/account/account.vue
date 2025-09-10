<template>
	<view class="app-account">
		<view class="app-account__btn-logout" @click="showLogout=true">退出登�?/view>
		<u-modal
			ref="logoutModal" 
			title="确定退出登录吗�? 
			:show="showLogout" 
			:zoom="false" 
			showCancelButton
			closeOnClickOverlay
			confirmText="确定"
			@confirm="onLogout" 
			@cancel="showLogout=false"
		>
		</u-modal>
	</view>
</template>

<script>
	import { mapActions } from 'vuex'
	
	export default {
		name: 'page-account-account',
		data() {
			return {
				showLogout: false
			}
		},
		onLoad(){
		},
		methods: {
			...mapActions('account', {
				logout: 'logout'
			}),
			onLogout(){
				this.showLogout = false
				this.logout()
				uni.reLaunch({
					url: '/pages/tabbar/my/my'
				})
			}
		}
	}
</script>

<!-- 添加界面背景�?-->
<style lang="scss">
	page {
		background-color: $u-bg-color;
	}
</style>
<style lang="scss" scoped>
	.app-account{
		&__btn-logout{
			background: #FFF;
			color: #f56c6c;
			letter-spacing: 1px;
			padding: 10px 15px;
			text-align: center;
			margin-top: 20px;
			font-size: 15px;
		}
	}
</style>
