namespace Orders.Query.Abstractions;

public interface IMaterializer<out TQueryModel, in TSource> //where TQueryModel : IQueryModel
{
    TQueryModel Materialize(TSource source);
}

public interface IMaterializer<out TQueryModel, in TSource1, in TSource2> //where TQueryModel : IQueryModel
{
    TQueryModel Materialize(
        TSource1 source1,
        TSource2 source2
    );
}