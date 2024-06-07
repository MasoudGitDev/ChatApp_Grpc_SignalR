namespace Shared.Server.Extensions;
public static class QueryExtensions {
    public static async Task<LinkedList<TModel>> ToLinkedListAsync<TModel>(this IQueryable<TModel> queryableSource) {
        LinkedList<TModel> models = new();
        foreach(var model in queryableSource) { 
            models.AddLast(model);
        }
        return await Task.FromResult(models);
    }
}
