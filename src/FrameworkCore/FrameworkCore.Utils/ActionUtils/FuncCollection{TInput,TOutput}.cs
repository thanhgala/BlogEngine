using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkCore.Utils.ActionUtils.Models;
using FrameworkCore.Utils.LinqUtils;
using FrameworkCore.Utils.ObjUtils;

namespace FrameworkCore.Utils.ActionUtils
{
    public class FuncCollection<TInput, TOutput> : DisposableModel
    {
        protected List<FuncModel<TInput, TOutput>> Funcs { get; set; } = new List<FuncModel<TInput, TOutput>>();

        public virtual List<FuncModel<TInput, TOutput>> Get()
        {
            return Funcs;
        }

        public virtual string Add(Func<TInput, TOutput> func)
        {
            var funcModel = new FuncModel<TInput, TOutput>(func);

            Funcs.Add(funcModel);

            return funcModel.Id;
        }

        public virtual void Remove(string funcId)
        {
            Funcs = Funcs.RemoveWhere(x => x.Id == funcId).ToList();
        }

        public virtual void Empty()
        {
            if (Funcs?.Any() != true)
            {
                return;
            }

            Funcs = Funcs.RemoveWhere(x => x.Func != null).ToList();
        }
        
        protected override void DisposeUnmanagedResources()
        {
            if (Funcs?.Any() != true)
            {
                return;
            }
            
            foreach (var funcModel in Funcs)
            {
                funcModel.Dispose();
            }
        }
    }
}