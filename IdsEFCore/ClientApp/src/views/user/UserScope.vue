<template>
  <div class="main">
    <a-card class="card" title="用户授权" :bordered="false">
      <a-form-model ref="ruleForm" :model="form">
        <a-form-model-item label="授权信息" prop="Scope">
          <a-checkbox-group v-model="form.Scope" :options="optionsList"> </a-checkbox-group>
        </a-form-model-item>
        <a-form-model-item :wrapper-col="{ span: 24 }">
          <a-button type="primary" @click="onSubmit"> 确认授权 </a-button>
          <a-button type="danger" style="margin-left: 10px" @click="resetForm"> 取消授权 </a-button>
        </a-form-model-item>
      </a-form-model>
    </a-card>
  </div>
</template>

<script>
import UserApi from '@/api/user'
export default {
  components: {
  },
  data () {
    return {
      optionsList: [],
      form: {
        Scope: []
      }
    }
  },
  created () {
    this.Init()
  },
  methods: {
    async Init () {
      var returnUrl = decodeURIComponent(this.$route.query.ReturnUrl)
      var data = await UserApi.GetConsentContext(encodeURIComponent(returnUrl))
      this.form.Scope = data.identityScopes
        .filter(x => x.checked)
        .map(x => {
          return x.value
        })
      data.identityScopes.map(x => {
        var model = {
          label: x.displayName,
          value: x.value,
          indeterminate: false
        }
        if (x.required) {
          model.disabled = x.required
        }
        this.optionsList.push(model)
      })
      data.apiScopes.map(x => {
        var model = {
          label: x.displayName,
          value: x.value,
          indeterminate: false
        }
        if (x.required) {
          model.disabled = x.required
        }
        this.optionsList.push(model)
      })
      console.log(data)
    },
    async onSubmit () {
      console.log('val', this.form.Scope)
      var data = {
        Type: 'yes',
        ReturnUrl: decodeURIComponent(this.$route.query.ReturnUrl),
        RememberConsent: true,
        ScopesConsented: this.form.Scope
      }
      var res = await UserApi.ProcessConsent(data)
      console.log(res)
      if (res.validationError === null) {
        window.location.href = res.redirectUri
      }
    },
    async resetForm () {
      // this.$refs.ruleForm.resetFields()
      var data = {
        Type: 'no',
        ReturnUrl: decodeURIComponent(this.$route.query.ReturnUrl),
        RememberConsent: true,
        ScopesConsented: this.form.Scope
      }
      var res = await UserApi.ProcessConsent(data)
      console.log(res)
    }
  }
}
</script>

<style lang="less" scoped>
.ant-checkbox-wrapper .ant-checkbox-group-item {
  display: block !important;
}
.ant-checkbox-group-item {
  display: block !important;
}
</style>
