using System.Web;

namespace News.Views;

[QueryProperty(nameof(Url), "url")]
public partial class ArticleView
{
   public ArticleView()
   {
      InitializeComponent();
   }

   public string Url
   {
      set =>
         BindingContext = new UrlWebViewSource
         {
            Url = HttpUtility.UrlDecode(value)
         };
   }
}