<template>
  <page-header-wrapper>
    <a-card :bordered="false">
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
      <a-form layout="inline" style="margin-bottom: 10px">
        <a-button type="primary" @click="Add()">
          {{ $t('Public_Add') }}
        </a-button>
        <a-button
          :disabled="deleteDisable"
          type="danger"
          style="margin-left: 15px"
          @click="Delete()"
          :loading="deleteLoading"
        >
          {{ $t('Public_Delete') }}</a-button
        >
      </a-form>
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
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a-button type="primary" @click="Save(record.Id)" :loading="updateLoading">{{
                $t('Public_Update')
              }}</a-button>
            </span>
          </template>
        </a-table-column>
      </a-table>
    </a-card>
    <a-modal
      :title="dialogTitle"
      v-model="show"
      :centered="true"
      :maskClosable="false"
      :width="800"
      @cancel="handleCancel"
      @ok="handleConfirm"
    >
      <a-form :form="form">
        <a-form-item :hidden="true">
          <a-input v-decorator="['Id']" />
        </a-form-item>
        <a-form-item :label="$t('RoleName')">
          <a-input
            :placeholder="$t('RoleName')"
            @keydown.native.stop="handleKeyDown"
            v-decorator="['RoleName', { rules: [{ required: true, max: 50, message: this.$t('REQUIRE') }] }]"
          />
        </a-form-item>
        <a-form-item :label="$t('Description')">
          <a-input
            :placeholder="$t('Description')"
            @keydown.native.stop="handleKeyDown"
            v-decorator="['Description']"
          />
        </a-form-item>
        <a-form-item :label="$t('Sort')">
          <a-input :placeholder="$t('Sort')" @keydown.native.stop="handleKeyDown" v-decorator="['Sort']" />
        </a-form-item>
      </a-form>
    </a-modal>
  </page-header-wrapper>
</template>

<script>
import RoleApi from '@/api/role.js'
export default {
  data () {
    return {
      RoleName: '',
      roleType: '',
      data: [],
      loading: false,
      show: false,
      form: this.$form.createForm(this),
      updateLoading: false,
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
    roleType (val) {
      console.log(val)
    }
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
    // 新增
    Add () {
      this.dialogTitle = this.$t('Public_Add')
      this.op = 'add'
      this.form.resetFields()
      this.show = true
    },
    async Save (id) {
      this.dialogTitle = this.$t('Public_Update')
      this.op = 'save'
      this.form.resetFields()
      this.updateLoading = true
      var res = {}
      try {
        console.log(id)
        res = await RoleApi.Get(id)
        this.setFormValues(res)
      } catch (e) {
        console.log(e)
      } finally {
        this.updateLoading = false
      }
      // console.log('res', res)
      this.show = true
    },
    // 新增或修改语言
    async handleConfirm () {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.op === 'add') {
            await RoleApi.Add(values)
          } else {
            await RoleApi.Update(values)
          }
          this.$notification.success({
            message: this.$t('Notiication'),
            description: this.$t('SaveOk')
          })
          this.selectedRowKeys = []
          this.QueryTable()
          this.show = false
          this.form.resetFields()
        }
      })
    },
    async Delete () {
      var data = this.selectedRowKeys
      this.deleteLoading = true
      await RoleApi.Delete(data)
      this.$notification.success({
        message: this.$t('Notiication'),
        description: this.$t('DeleteOk')
      })
      this.selectedRowKeys = []
      this.QueryTable()
      this.deleteLoading = false
    },
    // 设置表单数据
    setFormValues (data) {
      console.log('设置数据', data)
      Object.keys(data).forEach(key => {
        this.form.getFieldDecorator(key)
        const obj = {}
        obj[key] = data[key]
        this.form.setFieldsValue(obj)
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
