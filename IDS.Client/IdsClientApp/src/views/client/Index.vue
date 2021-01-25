<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="10" :sm="24">
              <a-form-item :label="$t('Comment_Article')">
                <a-input v-model="ArticleTitle" placeholder @keydown.native.stop="handleQuery" />
              </a-form-item>
            </a-col>
            <a-col :md="10" :sm="24">
              <a-form-item :label="$t('Comment_Meeting')">
                <a-input v-model="MeetingTitle" placeholder @keydown.native.stop="handleQuery" />
              </a-form-item>
            </a-col>
            <a-col :md="10" :sm="24">
              <a-form-item :label="$t('Comment_Content')">
                <a-input v-model="Content" placeholder @keydown.native.stop="handleQuery" />
              </a-form-item>
            </a-col>

            <a-col :md="8" :sm="24">
              <a-button type="primary" @click="QueryTable">{{ $t('Query') }}</a-button>
              <a-button style="margin-left: 8px" @click="ResetQuery">{{ $t('Reset') }}</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <a-form layout="inline" style="margin-bottom: 10px"> </a-form>
      <a-table
        :row-key="(record) => record.Id"
        :data-source="data"
        :loading="loading"
        :pagination="pagination"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <a-table-column key="ArticleTitle" data-index="ArticleTitle" :title="$t('Comment_Article')" width="16%" />
        <a-table-column key="MeetingTitle" data-index="MeetingTitle" :title="$t('Comment_Meeting')" width="16%" />
        <a-table-column key="Content" data-index="Content" :title="$t('Comment_Content')" width="20%">
          <template slot-scope="text, record">
            <span>
              {{ record.Content }}
            </span>
          </template>
        </a-table-column>
        <a-table-column key="Status" data-index="Status" :title="$t('Comment_Status')" width="10%">
          <template slot-scope="text, record">
            <span>
              <a-tag color="#2db7f5" v-if="record.Status === 10"> {{ $t('Not_reviewed') }} </a-tag>
            </span>
            <span>
              <a-tag color="#108ee9" v-if="record.Status === 20"> {{ $t('Passed') }} </a-tag>
            </span>
            <span>
              <a-tag color="#f50" v-if="record.Status === 30"> {{ $t('Rejected') }} </a-tag>
            </span>
          </template>
        </a-table-column>
        <a-table-column key="CreateDate" data-index="CreateDate" :title="$t('CreateDate')" width="15%">
          <template slot-scope="text, record">
            <span>
              {{ new Date(record.CreateDate).toLocaleString() }}
            </span>
          </template>
        </a-table-column>
        <a-table-column key="action" :title="$t('Action')" width="20%">
          <template slot-scope="text, record">
            <span v-if="record.Status === 10">
              <a-button type="primary" @click="ChangeStatus(record.Id, 20)">{{ $t('Passed_Btn') }}</a-button>
              <a-button type="danger" style="margin-left: 10px" @click="ChangeStatus(record.Id, 30)">{{
                $t('Rejected_Btn')
              }}</a-button>
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
      MeetingTitle: '',
      ArticleTitle: '',
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
    ResetQuery () {
      this.MeetingTitle = ''
      this.ArticleTitle = ''
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
        MeetingTitle: this.MeetingTitle || '',
        ArticleTitle: this.ArticleTitle || '',
        Content: this.Content || '',
        ...filters
      })
    },
    QueryTable () {
      var current = this.pagination.current || this.pagination.defaultCurrent
      this.fetch({
        pageSize: this.pagination.defaultPageSize,
        page: current,
        MeetingTitle: this.MeetingTitle || '',
        ArticleTitle: this.ArticleTitle || '',
        Content: this.Content || ''
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
      var res = await ClientApi.getList(params)
      pagination.total = Number(res.totalElements)
      this.loading = false
      var result = res.Data || []
      this.data = result
      this.pagination = pagination
    }
  }
}
</script>
