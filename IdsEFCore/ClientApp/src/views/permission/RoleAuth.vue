<template>
  <page-header-wrapper>
    <div>
      <a-row>
        <a-col :span="4">
          <a-card :bordered="true">
            <a-tree show-line :default-expanded-keys="['0-0']" @select="onSelect">
              <a-icon slot="switcherIcon" type="down" />
              <a-tree-node key="0-0" :title="$t('Role')">
                <a-tree-node v-for="item in roleList" :key="item.Id" :title="item.RoleName" />
              </a-tree-node>
            </a-tree>
          </a-card>
        </a-col>
        <a-col :span="19" style="margin-left: 40px">
          <a-card :bordered="true">
            <div class="table-page-search-wrapper"></div>
            <a-form layout="inline" style="margin-bottom: 15px">
              <a-button type="primary" :disabled="saveDisable" @click="Save()">{{ $t('Save') }}</a-button>
              <a-button type="primary" style="margin-left: 15px" @click="SelectAll()">{{ $t('All_Select') }}</a-button>
            </a-form>
            <a-table
              :data-source="data"
              :row-selection="rowSelection"
              :row-key="(record) => record.id"
              :loading="loading"
              defaultExpandAllRows
            >
              <a-table-column key="name" data-index="name" :title="$t('Menu_Name')"> </a-table-column>
              <a-table-column key="desc" data-index="desc" :title="$t('Desription')">
                <template slot-scope="text, record">
                  <span>
                    <p>{{ $t(record.name) }}</p>
                  </span>
                </template>
              </a-table-column>
              <a-table-column key="path" data-index="path" :title="$t('Menu_Path')" />
              <a-table-column key="component" data-index="component" :title="$t('Menu_Component')" />
              <a-table-column key="icon" data-index="icon" :title="$t('Menu_Icon')" />
              <a-table-column key="actionType" data-index="actionType" :title="$t('Menu_ActionType')">
                <template slot-scope="text, record">
                  <span>
                    <p>{{ record.actionType }}</p>
                  </span>
                </template>
              </a-table-column>
            </a-table>
          </a-card>
        </a-col>
      </a-row>
    </div>
  </page-header-wrapper>
</template>
<script>
import RoleApi from '@/api/role.js'
import MenuApi from '@/api/menu'
export default {
  data () {
    return {
      roleList: [],
      data: [],
      loading: false,
      columns: [
        {
          title: this.$t('region'),
          dataIndex: 'region',
          sorter: true
        },
        {
          title: this.$t('Lan_Code'),
          dataIndex: 'code'
        },
        {
          title: this.$t('Lan_Desc'),
          dataIndex: 'displayName'
        }
      ],
      selectRole: '',
      saveDisable: true,
      selectedRowKeys: [],
      basicData: []
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
    this.getRole()
    this.LoadTable()
  },
  methods: {
    // 全选
    SelectAll () {
      var data = this.basicData.map(x => { return x.id })
      if (this.selectedRowKeys.length === 0) {
        this.selectedRowKeys = data
      } else {
        this.selectedRowKeys = []
      }
    },
    // 多选的方法
    onSelectChange (selectedRowKeys) {
      console.log(selectedRowKeys)
      this.selectedRowKeys = selectedRowKeys
    },
    onSelect (selectedKeys, info) {
      this.selectRole = selectedKeys[0]
      console.log(this.selectRole)
      if (selectedKeys.length <= 0) {
        this.saveDisable = true
      } else {
        this.saveDisable = false
      }
      this.getSelectList()
    },
    // 选中角色获取角色绑定的菜单
    async getSelectList () {
      this.loading = true
      var roleId = this.selectRole
      var res = await RoleApi.GetMenus(roleId)
      this.onSelectChange(res)
      this.loading = false
    },
    async getRole () {
      try {
        var res = await RoleApi.getList({
          pageSize: 999,
          page: 1
        })
        this.roleList = res.Data || []
      } catch (e) {
        this.$notification.error({
          message: 'Error',
          description: this.$t(e)
        })
      }
    },
    // 加载所有菜单
    async LoadTable () {
      this.data = []
      this.loading = true
      var local = await MenuApi.getMenuList()
      this.basicData = local
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
    async Save () {
      var PostData = { RoleId: this.selectRole }
      PostData.MenuIds = this.selectedRowKeys
      await RoleApi.SaveMenus(PostData)
      this.$notification.success({
        message: this.$t('Notiication'),
        description: this.$t('SaveOk')
      })
    }
  }
}
</script>
