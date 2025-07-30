using News.Views;

namespace News;

public partial class AppShell
{
   public AppShell()
   {
      InitializeComponent();
      Routing.RegisterRoute("articleview", typeof(ArticleView));
   }
}