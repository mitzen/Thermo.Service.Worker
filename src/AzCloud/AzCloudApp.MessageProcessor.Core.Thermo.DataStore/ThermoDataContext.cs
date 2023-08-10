using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Microsoft.EntityFrameworkCore;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore
{
    public class ThermoDataContext : DbContext
    {
        public virtual DbSet<AttendanceDataStore> AttendanceRecord { get; set; }

        public virtual DbSet<HeartBeatDataStore> HeartBeat { get; set; }

        public virtual DbSet<UsersDataStore> Users { get; set; }

        public virtual DbSet<SMTPSettingsDataStore> SMTPSettings { get; set; }

        public virtual DbSet<CompanyDataStore> Company { get; set; }

        public virtual DbSet<Company_DeviceDataStore> Company_Device { get; set; }

        public virtual DbSet<EmailAlertRecipientDataStore> EmailAlertRecipient { get; set; }

        public ThermoDataContext()
        {
        }

        public ThermoDataContext(DbContextOptions<ThermoDataContext> options) : base(options)
        {

        }
    }
}
