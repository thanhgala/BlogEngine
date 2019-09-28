using System;
using FrameworkCore.Utils.ObjUtils;

namespace FrameworkCore.Utils.ActionUtils.Models
{
    public class ActionModel<T> : DisposableModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        
        public Action<T> Action { get; set; }

        public ActionModel(Action<T> action)
        {
            Action = action;
        }
    }
}