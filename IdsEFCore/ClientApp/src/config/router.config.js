// eslint-disable-next-line
import { UserLayout, BasicLayout } from '@/layouts'
export const asyncRouterMap = [

  {
    path: '/',
    name: 'index',
    component: BasicLayout,
    meta: { title: 'menu.home' },
    redirect: '/dashboard/workplace',
    children: [
      // dashboardnp
    ]
  },
  {
    path: '*', redirect: '/404', hidden: true
  }
]

/**
 * 基础路由
 * @type { *[] }
 */
export const constantRouterMap = [
  {
    path: '/Account',
    component: UserLayout,
    redirect: '/Account/Login',
    hidden: true,
    children: [
      {
        path: 'Login',
        name: 'Login',
        component: () => import(/* webpackChunkName: "user" */ '@/views/user/Login')
      }
    ]
  },
  {
    path: '/consent',
    component: UserLayout,
    redirect: 'consent',
    hidden: true,
    children: [
      {
        path: '/consent',
        name: 'consent',
        component: () => import(/* webpackChunkName: "user" */ '@/views/user/UserScope')
      }
    ]
  },
  {
    path: '/404',
    component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404')
  }

]
