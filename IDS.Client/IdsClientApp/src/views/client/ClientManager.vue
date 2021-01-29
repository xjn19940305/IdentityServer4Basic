<template>
  <a-form-model ref="ruleForm" :model="form" :rules="rules" :label-col="labelCol" :wrapper-col="wrapperCol">
    <a-form-model-item ref="ClientId" :label="$t('ClientId')" prop="ClientId">
      <a-input v-model="form.ClientId" />
    </a-form-model-item>
    <a-form-model-item ref="ClientName" :label="$t('ClientName')" prop="ClientName">
      <a-input v-model="form.ClientName" />
    </a-form-model-item>
    <a-form-model-item
      :label="$t('AllowedGrantTypes')"
      prop="AllowedGrantTypes"
      :label-col="{ span: 5 }"
      :wrapper-col="{ span: 14 }"
    >
      <a-select v-model="form.AllowedGrantTypes" placeholder="please select your AllowedGrantTypes">
        <a-select-option value="0">Implicit</a-select-option>
        <a-select-option value="1">ImplicitAndClientCredentials</a-select-option>
        <a-select-option value="2">Code</a-select-option>
        <a-select-option value="3">CodeAndClientCredentials</a-select-option>
        <a-select-option value="4">Hybrid</a-select-option>
        <a-select-option value="5">HybridAndClientCredentials</a-select-option>
        <a-select-option value="6">Implicit</a-select-option>
        <a-select-option value="7">ClientCredentials</a-select-option>
        <a-select-option value="8">ResourceOwnerPassword</a-select-option>
        <a-select-option value="9">ResourceOwnerPasswordAndClientCredentials</a-select-option>
        <a-select-option value="10">DeviceFlow</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item :label="$t('RequirePkce')" prop="RequirePkce">
      <a-switch v-model="form.RequirePkce" />
    </a-form-model-item>
    <a-form-model-item :label="$t('AllowOfflineAccess')" prop="AllowOfflineAccess">
      <a-switch v-model="form.AllowOfflineAccess" />
    </a-form-model-item>
    <a-form-model-item :label="$t('RequireConsent')" prop="RequireConsent">
      <a-switch v-model="form.RequireConsent" />
    </a-form-model-item>
    <a-form-model-item :wrapper-col="{ span: 14, offset: 4 }">
      <a-button type="primary" @click="onSubmit"> Create </a-button>
      <a-button style="margin-left: 10px" @click="resetForm"> Reset </a-button>
    </a-form-model-item>
  </a-form-model>
</template>
<script>
export default {
  data () {
    return {
      Id: 0,
      labelCol: { span: 4 },
      wrapperCol: { span: 14 },
      other: '',
      form: {
        name: '',
        AllowedGrantTypes: undefined,
        RequirePkce: false,
        AllowOfflineAccess: true,
        RequireConsent: false
      },
      rules: {
        ClientId: [
          { required: true, message: this.$t('Require') }
        ],
        ClientName: [
          { required: true, message: this.$t('Require') }
        ],
        AllowedGrantTypes: [{ required: true, message: 'Please select Activity zone' }]
      }
    }
  },
  mounted () {
    this.Init()
  },
  methods: {
    Init () {
      this.Id = this.$route.query.Id
    },
    onSubmit () {
      this.$refs.ruleForm.validate(valid => {
        if (valid) {
          console.log(this.form)
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    resetForm () {
      this.$refs.ruleForm.resetFields()
    }
  }
}
</script>
