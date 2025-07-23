using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;

namespace c6_OfflineDataSyncServer;

public class Blog : EntityTableData
{
   [Required]
   [MinLength(1)]
   [MaxLength(255)]
   public string Title { get; set; } = string.Empty;
}