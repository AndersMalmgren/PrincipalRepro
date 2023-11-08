namespace PrincipalRepro;

public sealed class EntityA
{
    public Guid PKKey {get;set;}
    public int FKKeyOne {get;set;}
    public int FKKeyTwo { get; set; }
    public Guid? ParentId { get; set; }

    public ICollection<EntityB> EntityBs { get; set; }

    public EntityA? Parent { get; set; }
}