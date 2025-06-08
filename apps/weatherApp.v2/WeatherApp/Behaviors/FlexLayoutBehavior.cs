namespace WeatherApp.Behaviors;

public class FlexLayoutBehavior : Behavior<FlexLayout>
{
   private FlexLayout? _view;

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
         var page = Application.Current.MainPage;
         SetState(_view, page.Width > page.Height
            ? "Landscape"
            : "Portrait"
         );
      });
   }

   protected override void OnAttachedTo(FlexLayout view)
   {
      _view = view;
      base.OnAttachedTo(view);
      UpdateState();
      Application.Current.MainPage.SizeChanged += MainPage_SizeChanged;
   }

   void MainPage_SizeChanged(object sender, EventArgs e)
   {
      UpdateState();
   }

   protected override void OnDetachingFrom(FlexLayout view)
   {
      base.OnDetachingFrom(view);
      Application.Current.MainPage.SizeChanged -= MainPage_SizeChanged;
      _view = null;
   }
}