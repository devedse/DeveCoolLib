namespace DeveCoolLib.Collections
{
    public class IEnumerableCompareItemResult<T>
    {
        public T Old { get; }
        public T New { get; }

        public IEnumerableCompareItemResult(T oldItem, T newItem)
        {
            Old = oldItem;
            New = newItem;
        }
    }
}
