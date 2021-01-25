import request from '@/utils/request'
var ClientApi = {
    getList (params) {
        var pm = Object.keys(params).map(function (key) {
            return encodeURIComponent(key) + '=' + encodeURIComponent(params[key])
        }).join('&')
        return request({
            url: `api/Client/GetList?${pm}`,
            method: 'get'
        })
    },
    Add (data) {
        return request({
            url: `api/Client/Add`,
            method: 'post',
            data
        })
    },
    Update (data) {
        return request({
            url: `api/Client/Update`,
            method: 'put',
            data
        })
    },
    Delete (data) {
        return request({
            url: `api/Client/Delete`,
            method: 'delete',
            data
        })
    },
    Get (Id) {
        return request({
            url: `api/Client/Get/${Id}`,
            method: 'get'
        })
    }
}
export default ClientApi
