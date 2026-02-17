using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Configuration
{
    internal class OTPConfig : IEntityTypeConfiguration<OTP>
    {
        public void Configure(EntityTypeBuilder<OTP> builder)
        {
            builder.ToTable("Otp");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.OtpHash).IsRequired();
            builder.Property(x => x.OtpHash).IsRequired();
            builder.Property(x => x.ExpiryTime).IsRequired();
            builder.Property(x => x.IsUsed).IsRequired();
            builder.Property(x => x.AttemptCount).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();    

            // Foriegn Key relation ship with user
            builder.HasOne(o => o.User)
                .WithMany(u => u.OTPs)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OTP_User");
        }
    }
}
