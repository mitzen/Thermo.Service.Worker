using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Microsoft.EntityFrameworkCore;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore
{
    public class ThermoDataContext : DbContext
    {
        public virtual DbSet<AttendanceRecord> AttendanceRecord { get; set; }

        public virtual DbSet<Heartbeat> HeartBeat { get; set; }

        public ThermoDataContext(DbContextOptions<ThermoDataContext> options) : base(options)
        {

        }
    }
}
