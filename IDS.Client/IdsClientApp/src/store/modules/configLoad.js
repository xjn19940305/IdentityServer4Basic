const configLoad = {
    state: {
        isLoad: false
    },
    mutations: {
        SET_ISLOAD: (state, value) => {
            state.isLoad = value
        }
    },
    actions: {
        InitData ({ commit }) {
            return new Promise(resolve => {
                var dt = new Date()
                fetch('/static/config.json?t=' + dt.getTime())
                    .then(response => response.json())
                    .then(ok => {
                        var obj = {
                            BASEURL: ok.baseUrl
                        }
                        console.log('加载基础数据', obj)
                        localStorage.setItem('GLOBAL', JSON.stringify(obj))
                        commit('SET_ISLOAD', true)
                    })
                resolve()
            })
        }
    }
}

export default configLoad
