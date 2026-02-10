using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Mvc;
namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // strongly coupled
        // private readonly IMyLogger _myLogger;
      
        // public DemoController(){
        //     _myLogger = new LogToServerMemory();
          
        // }
        //   [HttpGet]
        // public ActionResult Index(){
        //     _myLogger.Log("Index method started");
        //       return Ok("Hello World");
        // }


        // loosely coupled
        private readonly IMyLogger _myLogger;
        public DemoController(IMyLogger myLogger){
            _myLogger = myLogger;
        }
          [HttpGet]
        public ActionResult Index(){
            _myLogger.Log("Index method started");
              return Ok("Hello World");
        }
    }
}