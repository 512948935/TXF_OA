using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using IBLL;
using Ninject;
using Ninject.Modules;

namespace TXF.IOC
{
    public class BLLModule:NinjectModule
    {
        public override void Load()
        {
            Bind<Itb_sys_ModuleBLL>().To<tb_sys_ModuleBLL>().InSingletonScope();
            Bind<Itb_item_DepartmentBLL>().To<tb_item_DepartmentBLL>().InSingletonScope();
            Bind<Itb_sys_ItemBLL>().To<tb_sys_ItemBLL>().InSingletonScope();
        }
    }
}
