using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Reflection;
using System.Runtime.Loader;

namespace PluginApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public object NoRefDll()
        {
            string fileName = Environment.CurrentDirectory + "\\DLL\\MyPlugin.dll";

            AssemblyLoadContext abContext = new AssemblyLoadContext(Guid.NewGuid().ToString(), true);
            
            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var assembly = abContext.LoadFromStream(stream);

            var type = assembly.ExportedTypes.FirstOrDefault();
            var plugin = Activator.CreateInstance(type);

            var res = type.GetMethod("Execute")?.Invoke(plugin, null);
            abContext.Unload();

            return res;
        }

        [HttpPost]
        public object RefDll()
        {
            string fileName = Environment.CurrentDirectory + "\\DLL\\DemoPlugin.dll";

            AssemblyLoadContext abContext = new AssemblyLoadContext(Guid.NewGuid().ToString(), true);

            //不能使用LoadFromAssemblyPath，否则无法卸载成功
            //var assembly = abContext.LoadFromAssemblyPath(fileName);

            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var assembly = abContext.LoadFromStream(stream);

            var type = assembly.ExportedTypes.FirstOrDefault();
            var plugin = (IPlugin?)Activator.CreateInstance(type);

            var res = plugin.Execute();
            abContext.Unload();

            return res;
        }

    }
}
