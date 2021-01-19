<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <a-form layout="inline" style="margin-bottom: 10px">
        <a-button type="primary" @click="Add()">{{ $t('Public_Add') }}</a-button>
        <a-button type="primary" style="margin-left: 15px" @click="LoadTable()">{{ $t('Refresh') }}</a-button>
      </a-form>
      <a-table
        :data-source="data"
        :row-selection="rowSelection"
        :row-key="(record) => record.id"
        :loading="loading"
        defaultExpandAllRows
      >
        <a-table-column key="name" data-index="name" :title="$t('Menu_Name')" />
        <a-table-column key="desc" data-index="desc" :title="$t('Desription')">
          <template slot-scope="text, record">
            <span>
              <p>{{ $t(record.name) }}</p>
            </span>
          </template>
        </a-table-column>
        <a-table-column key="path" data-index="path" :title="$t('Menu_Path')" />
        <a-table-column key="component" data-index="component" :title="$t('Menu_Component')" />
        <a-table-column key="description" data-index="description" :title="$t('Description')" />
        <a-table-column key="sort" data-index="sort" :title="$t('Sort')" />
        <a-table-column key="icon" data-index="icon" :title="$t('Icon')" />
        <a-table-column key="actionType" data-index="actionType" :title="$t('IsBtn')"> </a-table-column>
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a @click="Save(record.id)">{{ $t('Public_Update') }}</a>
              <a-divider type="vertical" />
              <a-popconfirm :title="$t('public_msg_confirmDelete')" @confirm="() => Delete(record.id)">
                <a href="javascript:;">{{ $t('Public_Delete') }}</a>
              </a-popconfirm>
            </span>
          </template>
        </a-table-column>
      </a-table>
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
          <a-form-item :label="$t('Menu_Name')">
            <a-input
              :placeholder="$t('Menu_Name')"
              v-decorator="['name', { rules: [{ required: true, max: 50, message: this.$t('REQUIRE') }] }]"
            />
          </a-form-item>
          <a-form-item :label="$t('Description')">
            <a-input
              :placeholder="$t('Description')"
              v-decorator="['Description', { rules: [{ required: true, max: 50, message: this.$t('REQUIRE') }] }]"
            />
          </a-form-item>
          <a-form-item :label="$t('Menu_Path')">
            <a-input :placeholder="$t('Menu_Path')" v-decorator="['Path']" />
          </a-form-item>
          <a-form-item :label="$t('Menu_Component')">
            <a-input :placeholder="$t('Menu_Component')" v-decorator="['Component']" />
          </a-form-item>
          <a-form-item :label="$t('Sort')">
            <a-input :placeholder="$t('Sort')" v-decorator="['Sort']" />
          </a-form-item>
          <a-form-item :label="$t('Icon')">
            <a-input @click="ShowIconList()" :placeholder="$t('Icon')" v-decorator="['Icon']" />
          </a-form-item>
          <a-card v-if="IconShow" :body-style="{ padding: '24px 32px' }" :bordered="false">
            <icon-selector @change="handleIconChange" />
          </a-card>
          <a-form-item :label="$t('Redirect')">
            <a-input :placeholder="$t('Redirect')" v-decorator="['redirect']" />
          </a-form-item>
          <a-form-item :label="$t('Parent_Path')">
            <a-tree-select
              :tree-data="treeData"
              :placeholder="$t('CHOOSEONE')"
              tree-default-expand-all
              @select="GetTreeValue"
              v-decorator="['ParentId', { rules: [{ required: true, max: 50, message: this.$t('CHOOSEONE') }] }]"
              :defaultExpandAllRows="true"
            >
              <span slot="title" slot-scope="{ value }" style="color: #08c">Child Node1 {{ value }}</span>
            </a-tree-select>
          </a-form-item>
          <a-form-item :label="$t('Is_Hide')">
            <a-switch v-model="IsHide" v-decorator="['hideChildren']" />
          </a-form-item>
          <a-form-item :label="$t('Is_HideSelf')">
            <a-switch v-model="IsHideSelf" v-decorator="['Show']" />
          </a-form-item>
          <a-form-item :label="$t('IsBtn')">
            <a-switch v-model="IsBtn" v-decorator="['IsBtn']" />
          </a-form-item>
        </a-form>
      </a-modal>
    </a-card>
  </page-header-wrapper>
</template>
<script>
import MenuApi from '@/api/menu'
import IconSelector from '@/components/IconSelector'
export default {
  data () {
    return {
      IsHide: false,
      IsHideSelf: false,
      IsBtn: false,
      data: [],
      selectedRowKeys: [],
      form: this.$form.createForm(this),
      dialogTitle: '',
      op: '',
      show: false,
      treeData: [
        {
          title: 'root',
          key: 'root',
          value: '0',
          parentId: '0',
          children: []
        }
      ],
      IconShow: false,
      MenuId: ''
    }
  },
  components: {
    IconSelector
  },
  computed: {
    rowSelection: function () {
      return {
        selectedRowKeys: this.selectedRowKeys,
        onChange: this.onSelectChange
      }
    }
  },
  created () {
    // this.getApplication()
    this.LoadTable()
    this.LoadSelectTree()
  },
  methods: {
    // 多选的方法
    onSelectChange (selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    async LoadTable () {
      this.data = []
      this.loading = true
      var local = await MenuApi.getMenuList()
      console.log(local)
      local.forEach(f => {
        if (f.IsBtn === false) {
          f.actionType = this.$t('Menu')
        } else {
          f.actionType = this.$t('Button')
        }
      })
      this.ToTreeList(local, this.data, '0')
      this.loading = false
    },
    ToTreeList (list, tree, parentId) {
      list.forEach(item => {
        // 判断是否为父级菜单
        if (item.parentId === parentId) {
          item.meta = item.meta || {}
          const child = {
            ...item,
            key: item.key || item.name,
            icon: item.meta.icon || '',
            children: []
          }
          // 迭代 list， 找到当前菜单相符合的所有子菜单
          this.ToTreeList(list, child.children, item.id)
          // 删掉不存在 children 值的属性
          if (child.children.length <= 0) {
            delete child.children
          }
          // 加入到树中
          tree.push(child)
        }
      })
    },
    Add () {
      this.dialogTitle = this.$t('Public_Add')
      this.op = 'add'
      this.clear()
      this.form.resetFields()
      this.setFormValues({
        Component: 'RouteView'
      })
      this.show = true
    },
    async Delete (id) {
      await MenuApi.Delete(id)
      this.$notification.success({
        message: 'Success',
        description: 'Delete Success'
      })
      this.LoadTable()
      this.LoadSelectTree()
    },
    // 取消
    handleCancel () {
      this.show = false
      this.form.resetFields()
    },
    clear () {
      this.IsHideSelf = this.IsHide = this.IsBtn = false
      this.MenuId = ''
    },
    // 新增/修改菜单
    async handleConfirm () {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          console.log('handleConfirm', values)
          if (this.op === 'add') {
            await MenuApi.Add(values)
          } else {
            values.MenuId = this.MenuId
            await MenuApi.Update(values)
          }
          this.$notification.success({
            message: this.$t('Notiication'),
            description: this.$t('SaveOk')
          })
          this.selectedRowKeys = []
          this.LoadTable()
          await this.LoadSelectTree()
          this.show = false
          this.form.resetFields()
        }
      })
    },
    // 修改菜单
    async Save (id) {
      this.dialogTitle = this.$t('Public_Update')
      this.op = 'save'
      this.clear()
      this.form.resetFields()
      this.updateLoading = true
      try {
        var t = await MenuApi.Get(id)
        this.MenuId = t.MenuId
        this.IsHide = t.hideChildren
        this.IsHideSelf = t.Show
        this.IsBtn = t.IsBtn
      } catch (e) {
        console.log('error', e)
        this.updateLoading = false
      }
      this.setFormValues(t)
      this.show = true
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
    async selectEvent (e) {
      await this.LoadSelectTree(e)
    },
    async LoadSelectTree () {
      this.treeData[0].children = []
      this.setFormValues({
        parentId: '0'
      })
      var res = await MenuApi.getMenuList()
      this.getTreeSelectList(res, this.treeData[0].children, '0')
    },
    getTreeSelectList (list, tree, parentId) {
      list.forEach(item => {
        // 判断是否为父级菜单
        if (item.parentId === parentId) {
          const child = {
            title: item.name,
            key: item.id + '',
            value: item.id + '',
            parentId: item.parentId,
            children: []
          }
          // 迭代 list， 找到当前菜单相符合的所有子菜单
          this.getTreeSelectList(list, child.children, item.id)
          // 删掉不存在 children 值的属性
          if (child.children.length <= 0) {
            delete child.children
          }
          // 加入到树中
          tree.push(child)
        }
      })
    },
    GetTreeValue (value, node, text) {
      console.log('val', value)
    },
    handleIconChange (icon) {
      this.setFormValues({
        Icon: icon
      })
      this.IconShow = false
    },
    ShowIconList () {
      this.IconShow = true
    }
  }
}
</script>
