using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace c6_OfflineDataSyncClient.Model;

public class Blog : OfflineClientEntity
{
   [MinLength(1)]
   [MaxLength(255)]
   public string Title { get; set; } = string.Empty;

   [NotMapped]
   public bool InSync { get; set; } = true;
}