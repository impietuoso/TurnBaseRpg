namespace TricksAndTreatsOrThreats
{
    public static class DataViewExtensions
    {
        public static TV Clone<TV, TD>(this TV view, TD data) where TV : DataView<TD>
        {
            var clone = UnityEngine.Object.Instantiate(view, view.transform.parent);
            clone.SetData(data);
            return clone;
        }
    }
}