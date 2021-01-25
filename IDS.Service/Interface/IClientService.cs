using IdentityServer4.EntityFramework.Entities;
using IDS.Model;
using IDS.Model.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Service.Interface
{
    public interface IClientService
    {
        /// <summary>
        /// 新增客户端
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Add(ClientDTO dto);
        /// <summary>
        /// 修改客户端
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Update(ClientDTO dto);
        /// <summary>
        /// 删除客户端
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task Delete(int Id);
        /// <summary>
        /// 根据ID获取客户端信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ClientDTO> Get(int Id);
        /// <summary>
        /// 分页显示客户端列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> GetList(ClientRequest request);
    }
}
