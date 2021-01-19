import request from '@/utils/request'
var MenuApi = {
    getMenuList () {
        return request({
            url: `api/Menu/GetMenuList`,
            method: 'get'
        })
    },
    getAuthMenuList (data) {
        return request({
            url: `api/Menu/GetAuthMenuList`,
            method: 'post',
            data
        })
    },
    Add (data) {
        return request({
            url: `api/Menu/AddMenu`,
            method: 'post',
            data
        })
    },
    Update (data) {
        return request({
            url: `api/Menu/Update`,
            method: 'put',
            data
        })
    },
    Delete (MenuId) {
        return request({
            url: `api/Menu/Delete?Id=${MenuId}`,
            method: 'delete'
        })
    },
    Get (MenuId) {
        return request({
            url: `api/Menu/Get?MenuId=${MenuId}`,
            method: 'get'
        })
    }
}
export default MenuApi
