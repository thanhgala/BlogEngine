using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkCore.Utils.ActionUtils.Models;
using FrameworkCore.Utils.LinqUtils;
using FrameworkCore.Utils.ObjUtils;

namespace FrameworkCore.Utils.ActionUtils
{
    public class ActionCollection : DisposableModel
    {
        protected List<ActionModel> Actions { get; set; } = new List<ActionModel>();

        public virtual List<ActionModel> Get()
        {
            return Actions;
        }

        public virtual string Add(Action action)
        {
            var actionModel = new ActionModel(action);

            Actions.Add(actionModel);

            return actionModel.Id;
        }

        public virtual void Remove(string actionId)
        {
            Actions = Actions.RemoveWhere(x => x.Id == actionId).ToList();
        }

        public virtual void Empty()
        {
            if (Actions?.Any() != true)
            {
                return;
            }

            Actions = Actions.RemoveWhere(x => x.Action != null).ToList();
        }

        protected override void DisposeUnmanagedResources()
        {
            if (Actions?.Any() != true)
            {
                return;
            }
            
            foreach (var actionModel in Actions)
            {
                actionModel.Dispose();
            }
        }
    }
}