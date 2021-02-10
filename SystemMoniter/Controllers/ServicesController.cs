using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using SystemMoniter.Helper;

namespace SystemMoniter.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private ServiceEngine eng = new ServiceEngine();

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public string ServiceStatus()
        {
            return eng.ServiceStatus();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public string ContinueService()
        {
            return eng.ContinueService();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public string StartService()
        {
            return eng.StartService();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public string PauseService()
        {
            return eng.PauseService();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public string StopService()
        {
            return eng.StopService();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public string RestartService()
        {
            return eng.RestartService();
        }
    }
}

