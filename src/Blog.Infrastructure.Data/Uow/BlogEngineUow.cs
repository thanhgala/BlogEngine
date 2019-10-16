using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Core.Uow;
using Blog.Infrastructure.Data.Context;
using FrameworkCore.Infrastructure.DAL;

namespace Blog.Infrastructure.Data.Uow
{
    public class BlogEngineUow : UnitOfWork<BlogDbContext>, IBlogEngineUow
    {
        public BlogEngineUow(BlogDbContext context) : base(context)
        {
        }
    }
}
