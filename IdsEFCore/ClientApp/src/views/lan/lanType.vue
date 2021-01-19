<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Lan_Code')">
                <a-input v-model="Code" placeholder />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('Lan_Desc')">
                <a-input v-model="Value" placeholder />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-button type="primary" @click="QueryTable">{{ $t('Query') }}</a-button>
              <a-button style="margin-left: 8px">{{ $t('Reset') }}</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <a-form layout="inline">
        <a-button type="primary" style="margin-bottom: 10px" @click="Add()">{{ $t('Public_Add') }}</a-button>
      </a-form>
      <a-table :row-key="(record) => record.Id" :data-source="data" :loading="loading" :row-selection="rowSelection">
        <a-table-column key="Code" data-index="Code" :title="$t('Lan_Code')" />
        <a-table-column key="Description" data-index="Description" :title="$t('Lan_Desc')" />
        <a-table-column key="Sort" data-index="Sort" :title="$t('Lan_Sort')" />
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a-popconfirm :title="$t('public_msg_confirmDelete')" @confirm="() => Delete(record.Id)">
                <a href="javascript:;">{{ $t('Public_Delete') }}</a>
              </a-popconfirm>
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
        <a-form-item :label="$t('Lan_Code')">
          <a-input
            :placeholder="$t('Lan_Code')"
            v-decorator="['Code', { rules: [{ required: true, max: 50, message: this.$t('REQUIRE') }] }]"
          />
        </a-form-item>
        <a-form-item :label="$t('Lan_Desc')">
          <a-input
            :placeholder="$t('Lan_Desc')"
            v-decorator="['Description', { rules: [{ required: true, max: 50, message: this.$t('REQUIRE') }] }]"
          />
        </a-form-item>
        <a-form-item :label="$t('Lan_Sort')">
          <a-input :placeholder="$t('Lan_Sort')" v-decorator="['Sort']" />
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
      loading: false,
      show: false,
      form: this.$form.createForm(this),
      dialogTitle: '',
      CodeReadOnly: true,
      selectedRowKeys: []
    }
  },
  mounted () {
    this.QueryTable()
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
    // 多选的方法
    onSelectChange (selectedRowKeys) {
      console.log(selectedRowKeys)
      this.selectedRowKeys = selectedRowKeys
    },
    QueryTable () {
      this.fetch({
        Code: this.Code || '',
        Description: this.Value || ''
      })
    },
    async fetch (params = {}) {
      console.log('params:', params)
      this.loading = true
      var res = await languageApi.getLanglist(params)
      this.loading = false
      this.data = res
    },
    // 新增
    Add () {
      this.dialogTitle = this.$t('Public_Add')
      this.form.resetFields()
      this.CodeReadOnly = false
      this.show = true
    },
    // 新增或修改语言
    async handleConfirm () {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          await languageApi.addLanType(values)
          this.$notification.success({
            message: this.$t('Notiication'),
            description: this.$t('SaveOk', { msg: '' })
          })
          this.selectedRowKeys = []
          this.fetch()
          this.show = false
          this.form.resetFields()
        }
      })
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
