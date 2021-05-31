namespace DominandoEFCore.Multitenant.Domain.Abstract
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        //Estrategia 1
        // public string TenantId { get; set; }
    }
}