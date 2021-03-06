﻿using ReactiveHelpers.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ReactiveAddins
{
    [ContractClass(typeof(IModuleManagerContract))]
    public interface IModuleManager
    {

        void LoadFromPath(string path);

        void AddModule(IModuleInfo module);

        IEnumerable<IModuleInfo> All(Func<IModuleInfo, bool> predicate=null);

        void Not(Func<IModuleInfo, bool> predicate, Action<IModuleInfo> action);

        IModuleInfo GetByName(string name);
        
    }

    #region Contract
    [ContractClassFor(typeof(IModuleManager))]
    internal sealed class IModuleManagerContract : IModuleManager
    {
        public void AddModule(IModuleInfo module)
        {
            Contract.Requires<ArgumentNullException>(module != null);
        }

        public IEnumerable<IModuleInfo> All(Func<IModuleInfo, bool> predicate=null)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IModuleInfo>>() != null);
            return default(IEnumerable<IModuleInfo>);
        }

        public IModuleInfo GetByName(string name)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Ensures(Contract.Result<IModuleInfo>() != null);
            return default(IModuleInfo);
        }



        public void LoadFromPath(string path)
        {
            Contract.Requires<ArgumentNullException>(path != null);
        }

        public void Not(Func<IModuleInfo, bool> predicate, Action<IModuleInfo> action)
        {
            
        }
    }
    #endregion
}
