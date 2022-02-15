using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleTrader.Domain.Models;

namespace SimpleTrader.EF.Configurations
{
    public class AssetTransactionConfiguration : IEntityTypeConfiguration<AssetTransaction>
    {
        public void Configure(EntityTypeBuilder<AssetTransaction> builder)
        {
            builder.OwnsOne(at => at.Asset);
        }
    }
}
