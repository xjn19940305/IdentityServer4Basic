<template>
  <global-footer class="footer custom-render">
    <template v-slot:links>
      <a href="#" target="_blank">备案号:11111</a>
      <!-- <a href="https://www.github.com/vueComponent/ant-design-vue-pro" target="_blank">Github</a>
      <a href="https://www.github.com/sendya/" target="_blank">@Sendya</a> -->
    </template>
    <template v-slot:copyright>
      <a href="https://github.com/vueComponent" target="_blank">版权信息</a>
    </template>
  </global-footer>
</template>

<script>
import { GlobalFooter } from '@ant-design-vue/pro-layout'

export default {
  name: 'ProGlobalFooter',
  components: {
    GlobalFooter
  }
}
</script>
