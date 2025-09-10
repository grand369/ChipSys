<script>
	export default {
		onLaunch: function() {
			//console.log('App Launch')
		},
		onShow: function() {
			//检查更�?
			// #ifdef MP
			const updateManager = uni.getUpdateManager()
			updateManager.onCheckForUpdate(function (checkRes) {
				// 请求完新版本信息的回�?
				if(checkRes.hasUpdate){
					updateManager.onUpdateReady(function (res) {
						uni.showModal({
							title: '更新提示',
							content: '新版本已经准备好，是否重启应用？',
							success(confirmRes) {
							  if (confirmRes.confirm) {
								// 新的版本已经下载好，调用 applyUpdate 应用新版本并重启
								updateManager.applyUpdate()
							  }
							}
						})
					})
				}
			})
			// #endif
			//console.log('App Show')
		},
		onHide: function() {
			//console.log('App Hide')
		}
	}
</script>
<style lang="scss">
	@import "./static/iconfont.css";
	@import "uview-ui/index.scss";
	@import "uview-ui/libs/css/flex.scss";
	/*每个页面公共css */
	@import "./libs/styles/index.scss";
</style>
