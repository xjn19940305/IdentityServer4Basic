<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="10" :sm="24">
              <a-form-item :label="$t('clientId')">
                <a-input v-model="clientId" placeholder @keydown.native.stop="handleQuery" />
              </a-form-item>
            </a-col>
            <a-col :md="10" :sm="24">
              <a-form-item :label="$t('clientName')">
                <a-input v-model="clientName" placeholder @keydown.native.stop="handleQuery" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-button type="primary" @click="QueryTable">{{ $t('Query') }}</a-button>
              <a-button style="margin-left: 8px" @click="ResetQuery">{{ $t('Reset') }}</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <a-form layout="inline" style="margin-bottom: 10px; margin-top: 10px">
        <a-button type="primary" @click="Add()">
          {{ $t('Public_Add') }}
        </a-button>
      </a-form>
      <a-table
        :row-key="(record) => record.Id"
        :data-source="data"
        :loading="loading"
        :pagination="pagination"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <a-table-column key="clientId" data-index="clientId" :title="$t('clientId')" />
        <a-table-column key="clientName" data-index="clientName" :title="$t('clientName')" />
        <a-table-column
          key="requireClientSecret"
          data-index="requireClientSecret"
          :title="$t('requireClientSecret')"
          width="2%"
        >
          <template slot-scope="text, record">
            <a-tag color="#f50" v-if="record.requireClientSecret"> true </a-tag>
            <a-tag color="#2db7f5" v-else> false </a-tag>
          </template>
        </a-table-column>
        <a-table-column key="requireConsent" data-index="requireConsent" :title="$t('requireConsent')" width="2%">
          <template slot-scope="text, record">
            <a-tag color="#f50" v-if="record.requireConsent"> true </a-tag>
            <a-tag color="#2db7f5" v-else> false </a-tag>
          </template>
        </a-table-column>
        <a-table-column key="accessTokenLifetime" data-index="accessTokenLifetime" :title="$t('accessTokenLifetime')" />
        <a-table-column key="allowedScopes" data-index="allowedScopes" :title="$t('allowedScopes')">
          <template slot-scope="text, record">
            <span v-for="item in record.allowedScopes" :key="item.id">
              {{ item.scope }}
            </span>
          </template>
        </a-table-column>
        <a-table-column key="redirectUris" data-index="redirectUris" :title="$t('redirectUris')">
          <template slot-scope="text, record">
            <span v-for="item in record.redirectUris" :key="item.id">
              {{ item.redirectUri }}
            </span>
          </template>
        </a-table-column>
        <a-table-column
          key="postLogoutRedirectUris"
          data-index="postLogoutRedirectUris"
          :title="$t('postLogoutRedirectUris')"
        >
          <template slot-scope="text, record">
            <span v-for="item in record.postLogoutRedirectUris" :key="item.id">
              {{ item.postLogoutRedirectUri }}
            </span>
          </template>
        </a-table-column>
        <a-table-column key="CreateDate" data-index="CreateDate" :title="$t('CreateDate')" width="15%">
          <template slot-scope="text, record">
            <span>
              {{ new Date(record.created).toLocaleString() }}
            </span>
          </template>
        </a-table-column>
        <a-table-column key="action" :title="$t('Action')" width="20%">
          <template slot-scope="text, record">
            <span>
              <a @click="$router.push({ path: '/client/Modify', query: { Id: record.id } })">{{
                $t('Public_Update')
              }}</a>
              <a-divider type="vertical" />
              <a-popconfirm :title="$t('public_msg_confirmDelete')" @confirm="() => Delete(record.Id)">
                <a href="javascript:;">{{ $t('Public_Delete') }}</a>
              </a-popconfirm>
            </span>
          </template>
        </a-table-column>
      </a-table>
    </a-card>
  </page-header-wrapper>
</template>

<script>
import ClientApi from '@/api/client'
export default {
  data () {
    return {
      clientName: '',
      clientId: '',
      Content: '',
      data: [],
      loading: false,
      show: false,
      form: this.$form.createForm(this),
      deleteLoading: false,
      dialogTitle: '',
      selectedRowKeys: [],
      deleteDisable: true,
      pagination: { defaultCurrent: 1, defaultPageSize: 20, showSizeChanger: true, pageSizeOptions: ['50', '100', '500'] }
    }
  },
  mounted () {
    this.fetch()
  },
  watch: {
  },
  computed: {
    rowSelection: function () {
      return {
        selectedRowKeys: this.selectedRowKeys,
        onChange: this.onSelectChange
      }
    }
  },
  methods: {
    Add () {
      this.$router.push({ path: '/client/Modify' })
    },
    ResetQuery () {
      this.clientName = ''
      this.clientId = ''
      this.Content = ''
    },
    // 回车方法
    handleQuery (e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode
      if (eCode === 13) {
        // 调用对应的方法
        this.QueryTable()
      }
    },
    // 多选的方法
    onSelectChange (selectedRowKeys) {
      selectedRowKeys.length > 0 ? this.deleteDisable = false : this.deleteDisable = true
      this.selectedRowKeys = selectedRowKeys
    },
    handleTableChange (pagination, filters, sorter) {
      console.log(pagination)
      const pager = { ...this.pagination }
      pager.current = pagination.current
      pager.defaultPageSize = pagination.pageSize
      this.pagination = pager
      this.fetch({
        pageSize: pagination.pageSize,
        page: pagination.current,
        clientName: this.clientName || '',
        clientId: this.clientId || '',
        ...filters
      })
    },
    QueryTable () {
      var current = this.pagination.current || this.pagination.defaultCurrent
      this.fetch({
        pageSize: this.pagination.defaultPageSize,
        page: current,
        clientName: this.clientName || '',
        clientId: this.clientId || ''
      })
    },
    async ChangeStatus (Id, status) {
      await ClientApi.changeStatus({ Id, Status: status })
      this.QueryTable()
    },
    async fetch (params = {}) {
      console.log('params:', params)
      const pagination = { ...this.pagination }
      params.page = params.page || pagination.defaultCurrent
      params.pageSize = params.pageSize || pagination.defaultPageSize
      this.loading = true
      try {
        var res = await ClientApi.getList(params)
        pagination.total = Number(res.totalElements)

        var result = res.data || []
        this.data = result
      } catch (e) {
        console.log(e)
      } finally {
        this.loading = false
        this.pagination = pagination
      }
    }
  }
}
</script>
