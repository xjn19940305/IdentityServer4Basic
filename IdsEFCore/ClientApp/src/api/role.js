import request from '@/utils/request'
var RoleApi = {
    getList (params) {
        params = params || {}
        var RoleName = params.RoleName || ''
        return request({
            url: `api/Role/GetList?RoleName=${RoleName}&pageSize=${params.pageSize}&page=${params.page}`,
            method: 'get'
        })
    },
    Add (data) {
        return request({
            url: `api/Role/Add`,
            method: 'post',
            data
        })
    },
    Update (data) {
        return request({
            url: `api/Role/Update`,
            method: 'put',
            data
        })
    },
    Delete (ids) {
        return request({
            url: `api/Role/Delete`,
            method: 'delete',
            data: ids
        })
    },
    Get (MenuId) {
        return request({
            url: `api/Role/Get/${MenuId}`,
            method: 'get'
        })
    },
    GetMenus (RoleId) {
        return request({
            url: `api/Role/GetMenusIds/${RoleId}`,
            method: 'get'
        })
    },
    SaveMenus (data) {
        return request({
            url: `api/Role/Save`,
            method: 'put',
            data
        })
    }
}
export default RoleApi
