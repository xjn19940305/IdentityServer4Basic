<template>
  <div class="main"></div>
</template>

<script>
// import TokenApi from '@/api/token'
// import storage from 'store'
// import { ACCESS_TOKEN } from '@/store/mutation-types'
export default {
  data () {
    return {}
  },
  created () {
    this.Init()
  },
  methods: {
    async Init () {
      // var url = this.$route.query.ReturnUrl
      // console.log('url', url)
      // var result = await TokenApi.GetToken()
      // console.log('result', result)
      // storage.set(ACCESS_TOKEN, result.accessToken, 2 * 60 * 1000)
      // this.$router.push({ path: url })
    }
  }
}
</script>
