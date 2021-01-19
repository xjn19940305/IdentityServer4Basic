<template>
  <div class="main">
    <a-form id="formLogin" class="user-layout-login" ref="formLogin" :form="form" @submit="handleSubmit">
      <a-form-item>
        <a-input
          size="large"
          type="text"
          :placeholder="$t('Account')"
          v-decorator="[
            'Account',
            {
              rules: [{ required: true, message: $t('Type_Account') }],
            },
          ]"
        >
          <a-icon slot="prefix" type="user" :style="{ color: 'rgba(0,0,0,.25)' }" />
        </a-input>
      </a-form-item>

      <a-form-item>
        <a-input-password
          size="large"
          :placeholder="$t('Password')"
          v-decorator="[
            'Password',
            { rules: [{ required: true, message: $t('Type_Passwordd') }], validateTrigger: 'blur' },
          ]"
        >
          <a-icon slot="prefix" type="lock" :style="{ color: 'rgba(0,0,0,.25)' }" />
        </a-input-password>
      </a-form-item>
      <a-form-item style="margin-top: 24px">
        <a-button
          size="large"
          type="primary"
          htmlType="submit"
          class="login-button"
          :loading="state.loginBtn"
          :disabled="state.loginBtn"
        >
          {{ $t('Login') }}</a-button
        >
      </a-form-item>
    </a-form>
  </div>
</template>

<script>
import { mapActions } from 'vuex'
// import { timeFix } from '@/utils/util'

export default {
  components: {
  },
  data () {
    return {
      loginBtn: false,
      // login type: 0 email, 1 username, 2 telephone
      loginType: 0,
      stepCaptchaVisible: false,
      form: this.$form.createForm(this),
      state: {
        time: 60,
        loginBtn: false,
        // login type: 0 email, 1 username, 2 telephone
        loginType: 0,
        smsSendBtn: false
      }
    }
  },
  created () {
    this.Init()
  },
  methods: {
    Init () {
      var dt = new Date()
      fetch('/static/config.json?t=' + dt.getTime())
        .then(response => response.json())
        .then(ok => {
          var obj = {
            BASEURL: ok.baseUrl,
            UEDITORURL: ok.UeditorUrl
          }
          console.log('obj', obj)
          localStorage.setItem('GLOBAL', JSON.stringify(obj))
        })
    },
    ...mapActions(['Login', 'Logout']),
    handleSubmit (e) {
      e.preventDefault()
      const {
        state,
        Login
      } = this
      this.form.validateFields((err, values) => {
        if (!err) {
          console.log('form', values)
          state.loginBtn = true
          Login(values)
            .then((res) => this.loginSuccess())
            .catch()
            .finally(() => {
              state.loginBtn = false
            })
        }
      })
    },
    stepCaptchaSuccess () {
      this.loginSuccess()
    },
    stepCaptchaCancel () {
      this.Logout().then(() => {
        this.loginBtn = false
        this.stepCaptchaVisible = false
      })
    },
    loginSuccess () {
      console.log('login ok')
      this.$router.push({ path: '/Account/UserScope?ReturnUrl=' + encodeURIComponent(this.$route.query.ReturnUrl) })
    }
  }
}
</script>

<style lang="less" scoped>
.user-layout-login {
  label {
    font-size: 14px;
  }

  .getCaptcha {
    display: block;
    width: 100%;
    height: 40px;
  }

  .forge-password {
    font-size: 14px;
  }

  button.login-button {
    padding: 0 15px;
    font-size: 16px;
    height: 40px;
    width: 100%;
  }

  .user-login-other {
    text-align: left;
    margin-top: 24px;
    line-height: 22px;

    .item-icon {
      font-size: 24px;
      color: rgba(0, 0, 0, 0.2);
      margin-left: 16px;
      vertical-align: middle;
      cursor: pointer;
      transition: color 0.3s;

      &:hover {
        color: #1890ff;
      }
    }

    .register {
      float: right;
    }
  }
}
</style>
