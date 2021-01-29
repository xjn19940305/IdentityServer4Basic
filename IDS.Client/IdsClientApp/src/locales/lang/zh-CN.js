import antd from 'ant-design-vue/es/locale-provider/zh_CN'
import momentCN from 'moment/locale/zh-cn'

const components = {
  antLocale: antd,
  momentName: 'zh-cn',
  momentLocale: momentCN
}

const locale = {
  'message': '-',
  'menu.home': '主页',
  'menu.dashboard': '仪表盘',
  'menu.dashboard.analysis': '分析页',
  'menu.dashboard.monitor': '监控页',
  'menu.dashboard.workplace': '工作台',
  'client_Manager': '客户端管理',
  'clientId': '客户端ID',
  'clientName': '客户端名称',
  'requireClientSecret': '是否需要客户端秘钥',
  'requireConsent': '是否需要手动授权',
  'accessTokenLifetime': 'token有效时间',
  'allowedScopes': '允许Scopes',
  'redirectUris': '登陆成功返回的地址',
  'postLogoutRedirectUris': '注销登录返回的地址',
  'CreateDate': '创建日期',
  'Action': '操作',
  'Query': '查询',
  'Reset': '重置',
  'Public_Add': '新增',
  'Public_Update': '更新',
  'Public_Delete': '删除'
}

export default {
  ...components,
  ...locale
}
