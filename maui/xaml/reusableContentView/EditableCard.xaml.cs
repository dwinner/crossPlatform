namespace c3_ReusableContentView;

public partial class EditableCard
{
   public static readonly BindableProperty TextProperty = BindableProperty.Create(
      nameof(Text),
      typeof(string),
      typeof(EditableCard)
   );

   private bool _isEditing;

   public EditableCard()
   {
      InitializeComponent();
      BindingContext = this;
   }

   public string Text
   {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
   }

   private void OnEditButtonClicked(object sender, EventArgs e)
   {
      _isEditing = !_isEditing;
      if (_isEditing)
      {
         editor.IsReadOnly = false;
         editor.Focus();
         editor.CursorPosition = string.IsNullOrEmpty(editor.Text)
            ? 0
            : editor.Text.Length;
         editButton.Text = "Save";
      }
      else
      {
         editor.IsReadOnly = true;
         editButton.Focus();
         editButton.Text = "Edit";
      }
   }
}