
namespace Blog.Domain.AggregatesModels.BlogCategories.Commands.CreateCategory
{
    public class CreateCategoryCommand : CategoryCommand
    {
        public CreateCategoryCommand()
        {
        }

        public CreateCategoryCommand(string name, string slug)
        {
            Name = name;
            Slug = slug;
        }
        public override bool IsValid()
        {
            return true;
        }
    }
}
