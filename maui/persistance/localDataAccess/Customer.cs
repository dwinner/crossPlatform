using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace LocalDataAccess;

[Table("Customers")]
public class Customer : ObservableObject
{
   private string _companyName;
   private string _country;
   private int _id;
   private string _physicalAddress;

   [PrimaryKey]
   [AutoIncrement]
   public int Id
   {
      get => _id;
      set => SetProperty(ref _id, value);
   }

   [NotNull]
   public string CompanyName
   {
      get => _companyName;
      set => SetProperty(ref _companyName, value);
   }

   [MaxLength(50)]
   public string PhysicalAddress
   {
      get => _physicalAddress;
      set => SetProperty(ref _physicalAddress, value);
   }

   public string Country
   {
      get => _country;
      set => SetProperty(ref _country, value);
   }
}