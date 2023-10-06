using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class RoleService : IRoleService
{
    readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> CreateRole(string name)
    {
        IdentityResult result = await _roleManager.CreateAsync(
            new() { Id = Guid.NewGuid().ToString(), Name = name }
        );

        return result.Succeeded;
    }

    public async Task<bool> DeleteRole(string id)
    {
        AppRole appRole = await _roleManager.FindByIdAsync(id);
        IdentityResult result = await _roleManager.DeleteAsync(appRole);
        return result.Succeeded;
    }

    public async Task<(object, int)> GetAllRoles(int page, int size)
    {
        var query = _roleManager.Roles;

        IQueryable<AppRole> rolesQuery = null;

        if (page != -1 && size != -1) //iş kuralı. endpointe tıklandığında bütün rolleri almak istiyoruz. clinettan -1,-1 gönderilirse tüm rolleri yolluyoruz
            rolesQuery = query.Skip(page * size).Take(size);
        else
            rolesQuery = query;

        var datas = await rolesQuery.Select(r => new { r.Id, r.Name }).ToListAsync();
        return (datas, query.Count());
    }

    public async Task<(string id, string name)> GetRoleById(string id)
    {
        string role = await _roleManager.GetRoleIdAsync(new() { Id = id });
        return (id, role);
    }

    public async Task<bool> UpdateRole(string id, string name)
    {
        AppRole role = await _roleManager.FindByIdAsync(id);
        role.Name = name;
        IdentityResult result = await _roleManager.UpdateAsync(role);
        return result.Succeeded;
    }
}
