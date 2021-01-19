<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Account')">
                <a-input v-model="Account" placeholder @keydown.native.stop="handleKeyDown" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Email')">
                <a-input v-model="Email" placeholder @keydown.native.stop="handleKeyDown" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('NickName')">
                <a-input v-model="NickName" placeholder @keydown.native.stop="handleKeyDown" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Phone')">
                <a-input v-model="Phone" placeholder @keydown.native.stop="handleKeyDown" />
              </a-form-item>
            </a-col>
            <a-col :md="4" :sm="24">
              <a-button type="primary" @click="QueryTable">{{ $t('Query') }}</a-button>
              <a-button style="margin-left: 8px">{{ $t('Reset') }}</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <a-form layout="inline">
        <a-button type="primary" style="margin-bottom: 10px" @click="Add()">{{ $t('Public_Add') }}</a-button>
        <a-button :disabled="deleteDisable" type="danger" style="margin-left: 15px" @click="Delete()">{{
          $t('Public_Delete')
        }}</a-button>
      </a-form>
      <a-table
        :row-key="(record) => record.Id"
        :data-source="data"
        :pagination="pagination"
        :loading="loading"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <a-table-column key="Account" data-index="Account" :title="$t('Account')" />
        <a-table-column key="NickName" data-index="NickName" :title="$t('NickName')" />
        <a-table-column key="Phone" data-index="Phone" :title="$t('Phone')" />
        <a-table-column key="Email" data-index="Email" :title="$t('Email')" />
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a @click="Save(record.Id)">{{ $t('Public_Update') }}</a>
              <a-divider type="vertical" />
              <a-popconfirm :title="$t('public_msg_confirmChangePwd')" @confirm="() => ChangePwd(record.Id)">
                <a href="javascript:;">{{ $t('Public_Reset_PWD') }}</a>
              </a-popconfirm>
              <a-divider type="vertical" />
              <a @click="$refs.roleSelectModule.ShowC(record.Id)">{{ $t('Role_Bind') }}</a>
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
        <a-form-item :label="$t('Account')">
          <a-input
            :readOnly="IsReadOnly"
            :placeholder="$t('Account')"
            @keydown.native.stop="handleSubmit"
            v-decorator="['Account']"
          />
        </a-form-item>
        <a-form-item :label="$t('NickName')">
          <a-input :placeholder="$t('NickName')" @keydown.native.stop="handleSubmit" v-decorator="['NickName']" />
        </a-form-item>
        <a-form-item :label="$t('Phone')">
          <a-input :placeholder="$t('Phone')" @keydown.native.stop="handleSubmit" v-decorator="['Phone']" />
        </a-form-item>
        <a-form-item :label="$t('Email')">
          <a-input :placeholder="$t('Email')" @keydown.native.stop="handleSubmit" v-decorator="['Email']" />
        </a-form-item>
      </a-form>
    </a-modal>
    <role-select-module ref="roleSelectModule" @callBack="FillData($event)"></role-select-module>
  </page-header-wrapper>
</template>

<script>
import UserApi from '@/api/user'
import roleSelectModule from '@/views/permission/modules/roleSelectModule'
export default {
  components: {
    roleSelectModule
  },
  data () {
    return {
      Account: '',
      Email: '',
      NickName: '',
      Phone: '',
      data: [],
      pagination: { defaultCurrent: 1, defaultPageSize: 20, showSizeChanger: true, pageSizeOptions: ['50', '100', '500'] },
      loading: false,
      show: false,
      form: this.$form.createForm(this),
      updateLoading: false,
      dialogTitle: '',
      appList: [],
      LangList: [],
      IsReadOnly: false,
      selectedRowKeys: [],
      deleteDisable: true
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
  mounted () {
    this.QueryTable()
  },
  methods: {
    FillData (data) {
      var controlVal = this.form.getFieldsValue()
      controlVal.sceneCode = data
      //   this.form.setFieldsValue(controlVal)
      console.log(data)
    },
    // 回车方法
    handleKeyDown (e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode
      if (eCode === 13) {
        // 调用对应的方法
        this.QueryTable()
      }
    },
    handleSubmit (e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm()
      }
    },
    // 多选的方法
    onSelectChange (selectedRowKeys) {
      console.log(selectedRowKeys)
      if (selectedRowKeys.length > 0) {
        this.deleteDisable = false
      } else {
        this.deleteDisable = true
      }
      this.selectedRowKeys = selectedRowKeys
    },
    QueryTable () {
      this.fetch({
        pageSize: this.pagination.defaultPageSize,
        page: this.pagination.defaultCurrent,
        Account: this.Account || '',
        Email: this.Email || '',
        Phone: this.Phone || '',
        NickName: this.NickName || ''
      })
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
        Account: this.Account || '',
        Email: this.Email || '',
        Phone: this.Phone || '',
        NickName: this.NickName || '',
        ...filters
      })
    },
    async fetch (params = {}) {
      console.log('params:', params)
      const pagination = { ...this.pagination }
      params.page = params.page || pagination.defaultCurrent
      params.pageSize = params.pageSize || pagination.defaultPageSize
      this.loading = true
      var res = await UserApi.getList(params)
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
      this.IsReadOnly = false
      this.form.resetFields()
      this.show = true
    },
    async Save (id) {
      this.dialogTitle = this.$t('Public_Update')
      this.op = 'save'
      this.IsReadOnly = true
      this.form.resetFields()
      this.updateLoading = true
      try {
        var res = await UserApi.Get(id)
        console.log(res)
      } catch (e) {
        console.log(e)
      } finally {
        this.updateLoading = false
      }
      this.setFormValues(res)
      this.show = true
    },
    async ChangePwd (id) {
      await UserApi.ResetPwd(id)
      this.$notification.success({
        message: this.$t('Notiication'),
        description: this.$t('SaveOk')
      })
    },
    // 新增或修改语言
    async handleConfirm () {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.op === 'add') {
            await UserApi.Add(values)
          } else {
            await UserApi.Update(values)
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
    async Delete (ids) {
      var data = this.selectedRowKeys
      await UserApi.Delete(data)
      this.$notification.success({
        message: this.$t('Notiication'),
        description: this.$t('DeleteOk')
      })
      this.selectedRowKeys = []
      this.QueryTable()
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
