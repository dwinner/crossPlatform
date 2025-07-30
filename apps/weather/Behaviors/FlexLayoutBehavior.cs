namespace Weather.Behaviors;

public partial class FlexLayoutBehavior : Behavior<FlexLayout>
{
   private FlexLayout _view;

   private static Page ActivePage
   {
      get
      {
         var mainPage = Application.Current?.Windows[0].Page;
         return mainPage
                ?? throw new InvalidOperationException("No current active page");
      }
   }

   private static void SetState(VisualElement view, string state)
   {
      VisualStateManager.GoToState(view, state);
      if (view is not Layout layout)
      {
         return;
      }

      foreach (var child in layout.Children.OfType<VisualElement>())
      {
         SetState(child, state);
      }
   }

   private void UpdateState() =>
      MainThread.BeginInvokeOnMainThread(() =>
      {
         var page = ActivePage;
         var visualState = page.Width > page.Height
            ? "Landscape"
            : "Portrait";
         SetState(_view, visualState);
      });

   protected override void OnAttachedTo(FlexLayout view)
   {
      _view = view;
      base.OnAttachedTo(view);
      UpdateState();
      ActivePage.SizeChanged += MainPage_SizeChanged;
   }

   private void MainPage_SizeChanged(object sender, EventArgs e)
   {
      UpdateState();
   }

   protected override void OnDetachingFrom(FlexLayout view)
   {
      base.OnDetachingFrom(view);
      ActivePage.SizeChanged -= MainPage_SizeChanged;
      _view = null;
   }
}