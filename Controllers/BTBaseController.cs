using Microsoft.AspNetCore.Mvc;
using TheBugTracker.Extensions;

namespace TheBugTracker.Controllers
{
    [Controller]
    public abstract class BTBaseController : Controller
    {
        protected int _companyId => User.Identity!.GetCompanyId();
    }
}
