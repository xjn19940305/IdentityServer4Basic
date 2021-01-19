<template>
  <div>
    <a-modal
      :title="dialogTitle"
      v-model="show"
      :centered="true"
      :maskClosable="false"
      :width="1000"
      @cancel="handleCancel"
      @ok="handleConfirm"
    >
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('RoleName')">
                <a-input v-model="RoleName" placeholder @keydown.native.stop="handleQuery" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-button type="primary" @click="QueryTable">{{ $t('Query') }}</a-button>
              <a-button style="margin-left: 8px">{{ $t('Reset') }}</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <a-table
        :row-key="(record) => record.Id"
        :data-source="data"
        :loading="loading"
        :pagination="pagination"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <a-table-column key="RoleName" data-index="RoleName" :title="$t('RoleName')" />
        <a-table-column key="Description" data-index="Description" :title="$t('Description')" />
        <a-table-column key="Sort" data-index="Sort" :title="$t('Sort')" />
      </a-table>
    </a-modal>
  </div>
</template>
<script>
import RoleApi from '@/api/role'
import UserApi from '@/api/user'
export default {
  data () {
    return {
      RoleName: '',
      data: [],
      loading: false,
      show: false,
      form: this.$form.createForm(this),
      updateLoading: false,
      deleteLoading: false,
      dialogTitle: '',
      selectedRowKeys: [],
      deleteDisable: true,
      UserId: 0,
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
    async ShowC (Id) {
      this.UserId = Id
      var temp = await UserApi.GetRoleIds(this.UserId)
      this.selectedRowKeys = temp
      this.show = true
    },
    // 回车方法
    handleQuery (e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode
      if (eCode === 13) {
        // 调用对应的方法
        this.QueryTable()
      }
    },
    // 回车方法
    handleKeyDown (e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm()
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
        RoleName: this.RoleName || '',
        ...filters
      })
    },
    QueryTable () {
      this.fetch({
        pageSize: this.pagination.defaultPageSize,
        page: this.pagination.defaultCurrent,
        RoleName: this.RoleName || ''
      })
    },
    async fetch (params = {}) {
      console.log('params:', params)
      const pagination = { ...this.pagination }
      params.page = params.page || pagination.defaultCurrent
      params.pageSize = params.pageSize || pagination.defaultPageSize
      this.loading = true
      var res = await RoleApi.getList(params)
      pagination.total = Number(res.totalElements)
      this.loading = false
      var result = res.Data || []
      this.data = result
      this.pagination = pagination
    },
    // 新增或修改语言
    async handleConfirm () {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          console.log(this.selectedRowKeys)
          var data = { UserId: this.UserId, RoleIds: this.selectedRowKeys }
          await UserApi.SaveUserRole(data)
          this.$notification.success({
            message: this.$t('Notiication'),
            description: this.$t('SaveOk')
          })
          this.show = false
        }
      })
    },
    // 取消
    handleCancel () {
      this.show = false
      this.form.resetFields()
    }
  }
}
</script>
