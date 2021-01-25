import { asyncRouterMap, constantRouterMap } from '@/config/router.config'

const permission = {
  state: {
    routers: constantRouterMap,
    addRouters: [],
    routerState: false
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      state.addRouters = routers
      state.routers = constantRouterMap.concat(routers)
    },
    SET_ROUTERSTATE: (state, value) => {
      state.routerState = value
    }
  },
  actions: {
    GenerateRoutes ({ commit }) {
      return new Promise(resolve => {
        commit('SET_ROUTERS', asyncRouterMap)
        commit('SET_ROUTERSTATE', true)
        resolve()
      })
    }
  }
}

export default permission
