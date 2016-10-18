namespace CaboodleES.Interface
{
    internal interface IComponentCollection
    {
        Component Add(ulong eid);
        Component Get(ulong eid);
        Component Remove(ulong eid);
        bool Has(ulong eid);
        void Clear();
        ulong GetFilter();
        System.Type GetCType();
    }
}
