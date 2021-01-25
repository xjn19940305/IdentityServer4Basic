using IDS.Model.Client;
using IDS.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS.Client.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int Id)
        {
            var data = await clientService.Get(Id);
            return Ok(data);
        }
        /// <summary>
        /// 获取分页客户端信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ClientRequest request)
        {
            var data = await clientService.GetList(request);
            return Ok(data);
        }

        /// <summary>
        /// 新增客户端
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientDTO dto)
        {
            await clientService.Add(dto);
            return Ok();
        }
        /// <summary>
        /// 更新客户端
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientDTO dto)
        {
            await clientService.Update(dto);
            return Ok();
        }
        /// <summary>
        /// 删除客户端
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            await clientService.Delete(id);
            return Ok();
        }
    }
}
