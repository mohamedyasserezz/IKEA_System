using LinkDev.IKEA.DAL.Entities.Department;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistance.Data.Configurations.Departments
{
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).HasColumnType("VarChar(50)").IsRequired();
            builder.Property(D => D.Code).HasColumnType("VarChar(20)").IsRequired();
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GetDate()");
            //builder.Property(D => D.CreatedBy).HasDefaultValueSql("GetDate()");
            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GetDate()");
            //builder.Property(D => D.LastModifiedBy).HasComputedColumnSql("GetDate()");
        }
    }
}
