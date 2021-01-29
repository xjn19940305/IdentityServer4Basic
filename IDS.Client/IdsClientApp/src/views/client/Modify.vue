<template>
  <div>
    <a-card :bordered="false">
      <a-breadcrumb>
        <a-breadcrumb-item>
          <router-link to="/">
            {{ $t('Index') }}
          </router-link>
        </a-breadcrumb-item>
        <a-breadcrumb-item>
          <router-link to="/client">
            {{ $t('Client') }}
          </router-link>
        </a-breadcrumb-item>
        <a-breadcrumb-item>Client Modify</a-breadcrumb-item>
      </a-breadcrumb>
    </a-card>
    <a-card :bordered="false">
      <div class="page-header-index-wide page-header-wrapper-grid-content-main">
        <a-row :gutter="24">
          <a-col :md="24" :lg="17">
            <a-card
              style="width: 100%"
              :bordered="false"
              :tabList="tabListNoTitle"
              :activeTabKey="noTitleKey"
              @tabChange="(key) => handleTabChange(key, 'noTitleKey')"
            >
              <client-manager v-if="noTitleKey === 'ClientManager'"></client-manager>
              <scope-manager v-else-if="noTitleKey === 'ScopeManager'"></scope-manager>
              <project-page v-else-if="noTitleKey === 'SecretManager'"></project-page>
            </a-card>
          </a-col>
        </a-row>
      </div>
    </a-card>
  </div>
</template>
<script>
import ClientManager from '@/views/client/ClientManager'
import ScopeManager from '@/views/client/ScopeManager'

export default {
  components: {
    ClientManager,
    ScopeManager
  },
  data () {
    return {
      tabListNoTitle: [
        {
          key: 'ClientManager',
          tab: '客户端管理'
        },
        {
          key: 'ScopeManager',
          tab: 'scope管理'
        },
        {
          key: 'SecretManager',
          tab: '秘钥管理'
        }
      ],
      noTitleKey: 'ClientManager'
    }
  },
  computed: {
  },
  mounted () {
  },
  methods: {
    handleTabChange (key, type) {
      this[type] = key
    }
  }
}
</script>

<style lang="less" scoped>
.page-header-wrapper-grid-content-main {
  width: 100%;
  height: 100%;
  min-height: 100%;
  transition: 0.3s;

  .account-center-avatarHolder {
    text-align: center;
    margin-bottom: 24px;

    & > .avatar {
      margin: 0 auto;
      width: 104px;
      height: 104px;
      margin-bottom: 20px;
      border-radius: 50%;
      overflow: hidden;
      img {
        height: 100%;
        width: 100%;
      }
    }

    .username {
      color: rgba(0, 0, 0, 0.85);
      font-size: 20px;
      line-height: 28px;
      font-weight: 500;
      margin-bottom: 4px;
    }
  }

  .account-center-detail {
    p {
      margin-bottom: 8px;
      padding-left: 26px;
      position: relative;
    }

    i {
      position: absolute;
      height: 14px;
      width: 14px;
      left: 0;
      top: 4px;
      background: url(https://gw.alipayobjects.com/zos/rmsportal/pBjWzVAHnOOtAUvZmZfy.svg);
    }

    .title {
      background-position: 0 0;
    }
    .group {
      background-position: 0 -22px;
    }
    .address {
      background-position: 0 -44px;
    }
  }

  .account-center-tags {
    .ant-tag {
      margin-bottom: 8px;
    }
  }

  .account-center-team {
    .members {
      a {
        display: block;
        margin: 12px 0;
        line-height: 24px;
        height: 24px;
        .member {
          font-size: 14px;
          color: rgba(0, 0, 0, 0.65);
          line-height: 24px;
          max-width: 100px;
          vertical-align: top;
          margin-left: 12px;
          transition: all 0.3s;
          display: inline-block;
        }
        &:hover {
          span {
            color: #1890ff;
          }
        }
      }
    }
  }

  .tagsTitle,
  .teamTitle {
    font-weight: 500;
    color: rgba(0, 0, 0, 0.85);
    margin-bottom: 12px;
  }
}
</style>
