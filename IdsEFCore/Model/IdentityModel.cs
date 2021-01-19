using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.Model
{
    public class ProcessConsentResult
    {
        public string RedirectUri { get; set; }
        public string ValidationError { get; set; }
    }
    public class ConsentRequest
    {
        public string Type { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
        public string Description { get; set; }
    }
    public class ConsentContextModel
    {
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string Logo { get; set; }
        public bool AllowRememberConsent { get; set; }
        public IEnumerable<ScopeModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeModel> ApiScopes { get; set; }
    }
    public class ScopeModel
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }
}
