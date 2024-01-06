using kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace kurs.Services
{
    public class DatabaseBackupManager
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string _directoryName;

        public DatabaseBackupManager(DatabaseContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _directoryName = Path.Combine(_env.WebRootPath, "uploads", "backups");
            if (!Directory.Exists(_directoryName))
            {
                Directory.CreateDirectory(_directoryName);
            }
        }

        public List<FileInfo> GetBackups()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_directoryName);
            return directoryInfo.EnumerateFiles().ToList();
        }
        public void Backup()
        {
            var databaseName = _context.Database.GetDbConnection().Database;
            var backupPath = Path.Combine(_directoryName, $"{databaseName}_{DateTime.Now:yyyyMMddHHmmss}.bak");
            var backupCommand = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupPath}'";
            _context.Database.ExecuteSqlRaw(backupCommand);
        }
        public void Restore(string filename)
        {
            var backupPath = Path.Combine(_directoryName, filename);
            if (!string.IsNullOrEmpty(filename) && File.Exists(backupPath))
            {
                var databaseName = _context.Database.GetDbConnection().Database;
                var restoreQuery = $"USE MASTER ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE "
                                   + $"RESTORE DATABASE [{databaseName}] FROM DISK = '{backupPath}' WITH REPLACE "
                                   + $"ALTER DATABASE [{databaseName}] SET MULTI_USER";
                _context.Database.ExecuteSqlRaw(restoreQuery);
            }
        }
        public void DeleteBackup(string filename)
        {
            var backupPath = Path.Combine(_directoryName, filename);
            if (!string.IsNullOrEmpty(filename) && File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
        }
    }
}
