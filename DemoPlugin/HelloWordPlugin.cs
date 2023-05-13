using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoPlugin
{
    public class HelloWordPlugin : IPlugin
    {
        public object Execute()
        {
            return "hello world 3";
        }
    }
}
