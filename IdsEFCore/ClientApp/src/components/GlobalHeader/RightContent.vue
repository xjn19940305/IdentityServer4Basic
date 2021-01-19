<template>
  <div :class="wrpCls">
    <avatar-dropdown :menu="showMenu" :current-user="currentUser" class="ant-pro-global-header-index-action" />
    <a-dropdown>
      <span class="ant-pro-global-header-index-action">
        <a-icon type="global" :style="{ fontSize: '16px' }" />
      </span>
    </a-dropdown>
  </div>
</template>

<script>
import AvatarDropdown from './AvatarDropdown'
import i18nMixin from '@/store/i18n-mixin'
export default {
  name: 'RightContent',
  components: {
    AvatarDropdown
  },
  props: {
    prefixCls: {
      type: String,
      default: 'ant-pro-global-header-index-action'
    },
    isMobile: {
      type: Boolean,
      default: () => false
    },
    topMenu: {
      type: Boolean,
      required: true
    },
    theme: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      showMenu: true,
      currentUser: {},
      langList: []
    }
  },
  computed: {
    wrpCls () {
      return {
        'ant-pro-global-header-index-right': true,
        [`ant-pro-global-header-index-${(this.isMobile || !this.topMenu) ? 'light' : this.theme}`]: true
      }
    }
  },
  mixins: [i18nMixin],
  mounted () {
     var UserInfo = JSON.parse(localStorage.getItem('UserInfo'))
    this.currentUser = {
      name: UserInfo.Account || ''
    }
  },
  methods: {
    changeLang ({ key }) {
      this.setLang(key)
    }
  }
}
</script>
