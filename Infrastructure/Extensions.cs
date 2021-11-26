namespace ProjectBank.Infrastructure;

public static class Extensions
{
    public static IEnumerable<int> ToListOfIDs(this IEnumerable<Category> categories)
    {
        foreach (var Category in categories)
        {
            yield return Category.Id;
        }
    }
}