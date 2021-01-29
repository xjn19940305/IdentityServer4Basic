<template>
  <div>this is scope page ID:{{ Id }}</div>
</template>
<script>
export default {
  data () {
    return {
      Id: 0
    }
  },
  mounted () {
    this.Init()
  },
  methods: {
    Init () {
      this.Id = this.$route.query.Id
    }
  }
}
</script>
