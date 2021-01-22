import request from '@/utils/request'
var TokenApi = {
    GetToken () {
        return request({
            url: `api/Account/GetTokenInfo`,
            method: 'get'
        })
    }
}
export default TokenApi
