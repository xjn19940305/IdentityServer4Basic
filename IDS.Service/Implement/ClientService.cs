using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IDS.Database;
using IDS.Model;
using IDS.Model.Client;
using IDS.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDS.Service.Implement
{
    public class ClientService : IClientService
    {
        private readonly IMapper mapper;
        private readonly IDSContext Context;

        public ClientService(
            IMapper mapper,
            IDSContext Context
            )
        {
            this.mapper = mapper;
            this.Context = Context;
        }
        public async Task Add(ClientDTO dto)
        {
            var entity = mapper.Map<Client>(dto);
            entity.Created = DateTime.UtcNow;
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
        }
        public async Task Update(ClientDTO dto)
        {
            var entity = await Context.Clients.FirstOrDefaultAsync(x => x.Id == dto.Id);
            var data = mapper.Map(dto, entity);
            data.Updated = DateTime.UtcNow;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }
        public async Task Delete(int Id)
        {
            var list = await Context.Clients.Where(x => x.Id == Id).ToListAsync();
            Context.RemoveRange(list);
            await Context.SaveChangesAsync();
        }

        public async Task<PagedViewModel> GetList(ClientRequest request)
        {
            var model = new PagedViewModel();
            var data = Context.Clients
                .Include(x=>x.AllowedGrantTypes)
                .Include(x=>x.AllowedScopes)
                .Include(x=>x.RedirectUris)
                .Include(x=>x.PostLogoutRedirectUris)
                .OrderByDescending(x => x.Created).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.clientId))
                data = data.Where(x => x.ClientId == request.clientId);
            if (!string.IsNullOrWhiteSpace(request.clientName))
                data = data.Where(x => x.ClientName.Contains(request.clientName));
            model.totalElements = data.Count();
            data = data.Skip((request.page - 1) * request.pageSize).Take(request.pageSize);
            model.Data = await data.Select(x => mapper.Map<ClientDTOResponse>(x)).ToListAsync();
            return model;
        }

        public async Task<ClientDTO> Get(int Id)
        {
            var data = await Context.Clients.FirstOrDefaultAsync(x => x.Id == Id);
            return mapper.Map<ClientDTO>(data);
        }
    }
}
