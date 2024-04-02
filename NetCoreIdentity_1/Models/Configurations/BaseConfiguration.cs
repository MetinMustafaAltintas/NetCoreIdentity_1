using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreIdentity_1.Models.Entities;
using NetCoreIdentity_1.Models.Interfaces;

namespace NetCoreIdentity_1.Models.Configurations
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedDate).HasColumnName("YaratilmaTarihi");
            builder.Property(x => x.ModifiedDate).HasColumnName("GuncellenmeTarihi");
            builder.Property(x => x.DeletedDate).HasColumnName("SilinmeTarihi");
            builder.Property(x => x.Status).HasColumnName("VeriDurumu");
        }
    }
}
