//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Dependency
//{
//    public static class Extension
//    {

//        public static IServiceCollection AddExtendService(this IServiceCollection services, IConfiguration cofiguration)
//        {
//            services.TryAddScoped<ILanguageService, LanguageService>();
//            services.TryAddScoped<IMenuService, MenuService>();
//            services.TryAddScoped<IRoleService, RoleService>();
//            services.TryAddScoped<IUserService, UserService>();
//            services.TryAddScoped<RedisCache>();
//            var ue = cofiguration.GetSection("UEditor").Value;
//            services.AddUEditorService(ue);
//            return services;
//        }
//    }
//}
