import storage from 'store'
import { ACCESS_TOKEN } from '@/store/mutation-types'
import { welcome } from '@/utils/util'
import UserApi from '@/api/user'
const user = {
  state: {
    token: '',
    name: '',
    welcome: '',
    avatar: '',
    roles: [],
    info: {}
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_NAME: (state, { name, welcome }) => {
      state.name = name
      state.welcome = welcome
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_INFO: (state, info) => {
      state.info = info
    }
  },

  actions: {
    // 登录
    Login ({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        UserApi.Login(userInfo).then(x => {
          console.log('Login Success', x)
          resolve()
        }).catch(f => {
          console.log('loginError', f)
          reject(f)
        })
      })
    },

    // 获取用户信息
    GetInfo ({ commit }) {
      return new Promise(async (resolve, reject) => {
        try {
          var token = storage.get(ACCESS_TOKEN)
          var userInfo = await UserApi.GetUserInfo(token)
          localStorage.setItem('UserInfo', JSON.stringify(userInfo))
          commit('SET_NAME', { name: userInfo.Account, welcome: welcome() })
          commit('SET_ROLES', [userInfo])
          resolve()
        } catch (e) {
          reject(e)
        }
      })
    },

    // 登出
    Logout ({ commit, state }) {
      return new Promise((resolve) => {
        try {
          commit('SET_TOKEN', '')
          commit('SET_ROLES', [])
          storage.remove(ACCESS_TOKEN)
          resolve()
        } catch (e) {
        } finally {

        }
      })
    }

  }
}

export default user
