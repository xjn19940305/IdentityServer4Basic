<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Trans_Code')">
                <a-input v-model="Code" placeholder @keydown.native.stop="handleKeyDown" />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Trans_Value')">
                <a-input v-model="Value" placeholder @keydown.native.stop="handleKeyDown" />
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
        :row-key="(record) => record.TranslateId"
        :data-source="data"
        :pagination="pagination"
        :loading="loading"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <a-table-column key="TranslateCode" data-index="TranslateCode" :title="$t('Trans_Code')" />
        <a-table-column key="TranslateContent" data-index="TranslateContent" :title="$t('Trans_Value')" />
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a @click="Save(record.TranslateId)">{{ $t('Public_Update') }}</a>
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
        <a-form-item :label="$t('Trans_Code')" :hidden="true">
          <a-input v-decorator="['TranslateId']" />
        </a-form-item>
        <a-form-item :label="$t('Lan_Code')">
          <a-select
            :placeholder="$t('CHOOSEONE')"
            v-decorator="['LanguageTypeId', { rules: [{ required: true, max: 50, message: this.$t('CHOOSEONE') }] }]"
          >
            <a-select-option value>=={{ $t('DEFAULT_SELECT') }}==</a-select-option>
            <a-select-option v-for="item in LangList" :key="item.Id.toString()">{{ item.Description }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item :label="$t('Trans_Code')">
          <a-input
            :readOnly="IsReadOnly"
            :placeholder="$t('Trans_Code')"
            @keydown.native.stop="handleSubmit"
            v-decorator="['TranslateCode']"
          />
        </a-form-item>
        <a-form-item :label="$t('Trans_Value')">
          <a-input
            :placeholder="$t('Trans_Value')"
            @keydown.native.stop="handleSubmit"
            v-decorator="['TranslateContent']"
          />
        </a-form-item>
      </a-form>
    </a-modal>
  </page-header-wrapper>
</template>

<script>
import languageApi from '@/api/language'
export default {
  data () {
    return {
      Code: '',
      Value: '',
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
    this.getLangTypeList()
  },
  methods: {
    async getLangTypeList () {
      var params = {
        Code: '',
        Description: ''
      }
      this.LangList = await languageApi.getLanglist(params) || []
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
      var current = this.pagination.current || this.pagination.defaultCurrent
      this.fetch({
        pageSize: this.pagination.defaultPageSize,
        page: current,
        Code: this.Code || '',
        Value: this.Value || ''
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
        Code: this.Code || '',
        Value: this.Value || '',
        ...filters
      })
    },
    async fetch (params = {}) {
      console.log('params:', params)
      const pagination = { ...this.pagination }
      params.page = params.page || pagination.defaultCurrent
      params.pageSize = params.pageSize || pagination.defaultPageSize
      this.loading = true
      var res = await languageApi.getAllTranslate(params)
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
        var res = await languageApi.getTranslateFromId(id)
        console.log(res)
      } catch (e) {
        console.log(e)
      } finally {
        this.updateLoading = false
      }
      res.LanguageTypeId = res.LanguageTypeId.toString()
      this.setFormValues(res)
      this.show = true
    },
    // 新增或修改语言
    async handleConfirm () {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.op === 'add') {
            await languageApi.addTranslate(values)
          } else {
            await languageApi.updateTranslate(values)
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
      await languageApi.DeleteTranslate(data)
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
