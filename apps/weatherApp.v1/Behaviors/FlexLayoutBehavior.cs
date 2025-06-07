namespace Weather.Behaviors;

public class FlexLayoutBehavior : Behavior<FlexLayout>
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

   private void SetState(VisualElement view, string state)
   {
      VisualStateManager.GoToState(view, state);
      if (view is Layout layout)
      {
         foreach (var child in layout.Children.OfType<VisualElement>())
         {
            SetState(child, state);
         }
      }
   }

   private void UpdateState()
   {
      MainThread.BeginInvokeOnMainThread(() =>
      {
         var page = ActivePage;
         if (page.Width > page.Height)
         {
            SetState(_view, "Landscape");
            return;
         }

         SetState(_view, "Portrait");
      });
   }

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