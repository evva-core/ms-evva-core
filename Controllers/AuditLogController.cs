using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuditLogController : GenericController<AuditLog>
    {
        private readonly IAuditLogControllerService _auditLogService;

        public AuditLogController(IAuditLogControllerService auditLogService) 
            : base(auditLogService)
        {
            _auditLogService = auditLogService;
        }
    }
}
